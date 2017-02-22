﻿using System;
using SiliconStudio.Core.Mathematics;

namespace SiliconStudio.Xenko.VirtualReality
{
    internal class OculusTouchController : TouchController
    {
        private readonly TouchControllerHand hand;
        private Vector3 currentPos, currentLinearVelocity, currentAngularVelocity;
        private Quaternion currentRot;
        private DeviceState currentState;
        private float currentTrigger, currentGrip, previousTrigger, previousGrip;
        private uint currentTouchesState, previousTouchesState;
        private Vector2 currentThumbstick;
        private const float triggerAndGripDeadzone = 0.00001f;

        public override Vector3 Position => currentPos;

        public override Quaternion Rotation => currentRot;

        public override Vector3 LinearVelocity => currentLinearVelocity;

        public override Vector3 AngularVelocity => currentAngularVelocity;

        public override DeviceState State => currentState;

        public override float Trigger => currentTrigger;

        public override float Grip => currentGrip;

        public override bool IndexPointing
        {
            get
            {
                if (hand == TouchControllerHand.Left)
                {
                    //ovrTouch_LIndexPointing
                    if ((currentTouchesState & 0x00002000) == 0x00002000)
                    {
                        return true;
                    }
                }
                else if (hand == TouchControllerHand.Right)
                {
                    //ovrTouch_RIndexPointing
                    if ((currentTouchesState & 0x00000020) == 0x00000020)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public override bool IndexResting
        {
            get
            {
                if (hand == TouchControllerHand.Left)
                {
                    //ovrTouch_LIndexTrigger
                    if ((currentTouchesState & 0x00001000) == 0x00001000)
                    {
                        return true;
                    }
                }
                else if (hand == TouchControllerHand.Right)
                {
                    //ovrTouch_RIndexTrigger
                    if ((currentTouchesState & 0x00000010) == 0x00000010)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public override bool ThumbUp
        {
            get
            {
                if (hand == TouchControllerHand.Left)
                {
                    //ovrTouch_LThumbUp
                    if ((currentTouchesState & 0x00004000) == 0x00004000)
                    {
                        return true;
                    }
                }
                else if (hand == TouchControllerHand.Right)
                {
                    //ovrTouch_RThumbUp
                    if ((currentTouchesState & 0x00000040) == 0x00000040)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public override bool ThumbResting
        {
            get
            {
                if (hand == TouchControllerHand.Left)
                {
                    //ovrTouch_LThumbRest
                    if ((currentTouchesState & 0x00000800) == 0x00000800)
                    {
                        return true;
                    }
                }
                else if (hand == TouchControllerHand.Right)
                {
                    //ovrTouch_RThumbRest
                    if ((currentTouchesState & 0x00000008) == 0x00000008)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public OculusTouchController(TouchControllerHand hand)
        {
            this.hand = hand;
            currentState = DeviceState.Invalid;
        }

        internal void UpdateInputs(ref OculusOvr.InputProperties properties)
        {
            previousTouchesState = currentTouchesState;
            previousTrigger = currentTrigger;
            previousGrip = currentGrip;

            if (!properties.Valid)
            {
                currentTrigger = 0.0f;
                currentGrip = 0.0f;
                currentTouchesState = 0;
                currentThumbstick = Vector2.Zero;
                currentTrigger = 0.0f;
                currentGrip = 0.0f;
                return;
            }

            currentTrigger = hand == TouchControllerHand.Left ? properties.IndexTriggerLeft : properties.IndexTriggerRight;
            currentGrip = hand == TouchControllerHand.Left ? properties.HandTriggerLeft : properties.HandTriggerRight;
            currentTouchesState = properties.Touches;
            currentThumbstick = hand == TouchControllerHand.Left ? properties.ThumbstickLeft : properties.ThumbstickRight;
        }

        internal void UpdatePoses(ref OculusOvr.PosesProperties properties)
        {
            if (hand == TouchControllerHand.Left)
            {
                currentPos = properties.PosLeftHand;
                currentRot = properties.RotLeftHand;
                currentLinearVelocity = properties.LinearVelocityLeftHand;
                currentAngularVelocity = new Vector3(MathUtil.DegreesToRadians(properties.AngularVelocityLeftHand.X), MathUtil.DegreesToRadians(properties.AngularVelocityLeftHand.Y), MathUtil.DegreesToRadians(properties.AngularVelocityLeftHand.Z));
                if ((properties.StateLeftHand & 0x0001) == 0x0001)
                {
                    currentState = DeviceState.OutOfRange;

                    if ((properties.StateLeftHand & 0x0002) == 0x0002)
                    {
                        currentState = DeviceState.Valid;
                    }
                }
                else
                {
                    currentState = DeviceState.Invalid;
                }
            }
            else
            {
                currentPos = properties.PosRightHand;
                currentRot = properties.RotRightHand;
                currentLinearVelocity = properties.LinearVelocityRightHand;
                currentAngularVelocity = new Vector3(MathUtil.DegreesToRadians(properties.AngularVelocityRightHand.X), MathUtil.DegreesToRadians(properties.AngularVelocityRightHand.Y), MathUtil.DegreesToRadians(properties.AngularVelocityRightHand.Z));
                if ((properties.StateRightHand & 0x0001) == 0x0001)
                {
                    currentState = DeviceState.OutOfRange;

                    if ((properties.StateRightHand & 0x0002) == 0x0002)
                    {
                        currentState = DeviceState.Valid;
                    }
                }
                else
                {
                    currentState = DeviceState.Invalid;
                }
            }
        }

        public override bool IsPressedDown(TouchControllerButton button)
        {
            switch (button)
            {
                case TouchControllerButton.Thumbstick:
                    var thumbFlag = hand == TouchControllerHand.Left ? 0x00000400 : 0x00000004;
                    //ovrButton_RThumb
                    return (previousTouchesState & thumbFlag) != thumbFlag && (currentTouchesState & thumbFlag) == thumbFlag;
                case TouchControllerButton.A:
                    //ovrButton_A
                    return (previousTouchesState & 0x00000001) != 0x00000001 && (currentTouchesState & 0x00000001) == 0x00000001;
                case TouchControllerButton.B:
                    //ovrButton_B
                    return (previousTouchesState & 0x00000002) != 0x00000002 && (currentTouchesState & 0x00000002) == 0x00000002;
                case TouchControllerButton.X:
                    //ovrButton_X
                    return (previousTouchesState & 0x00000100) != 0x00000100 && (currentTouchesState & 0x00000100) == 0x00000100;
                case TouchControllerButton.Y:
                    //ovrButton_Y
                    return (previousTouchesState & 0x00000200) != 0x00000200 && (currentTouchesState & 0x00000200) == 0x00000200;
                case TouchControllerButton.Trigger:
                    return previousTrigger <= triggerAndGripDeadzone && currentTrigger > triggerAndGripDeadzone;
                case TouchControllerButton.Grip:
                    return previousGrip <= triggerAndGripDeadzone && currentGrip > triggerAndGripDeadzone;
                default:
                    return false;
            }
        }

        public override bool IsPressed(TouchControllerButton button)
        {
            switch (button)
            {
                case TouchControllerButton.Thumbstick:
                    var thumbFlag = hand == TouchControllerHand.Left ? 0x00000400 : 0x00000004;
                    //ovrButton_RThumb
                    return (currentTouchesState & thumbFlag) == thumbFlag;
                case TouchControllerButton.A:
                    //ovrButton_A
                    return (currentTouchesState & 0x00000001) == 0x00000001;
                case TouchControllerButton.B:
                    //ovrButton_B
                    return (currentTouchesState & 0x00000002) == 0x00000002;
                case TouchControllerButton.X:
                    //ovrButton_X
                    return (currentTouchesState & 0x00000100) == 0x00000100;
                case TouchControllerButton.Y:
                    //ovrButton_Y
                    return (currentTouchesState & 0x00000200) == 0x00000200;
                case TouchControllerButton.Trigger:
                    return currentTrigger > triggerAndGripDeadzone;
                case TouchControllerButton.Grip:
                    return currentGrip > triggerAndGripDeadzone;
                default:
                    return false;
            }
        }

        public override bool IsPressReleased(TouchControllerButton button)
        {
            switch (button)
            {
                case TouchControllerButton.Thumbstick:
                    var thumbFlag = hand == TouchControllerHand.Left ? 0x00000400 : 0x00000004;
                    //ovrButton_RThumb
                    return (previousTouchesState & thumbFlag) == thumbFlag && (currentTouchesState & thumbFlag) != thumbFlag;
                case TouchControllerButton.A:
                    //ovrButton_A
                    return (previousTouchesState & 0x00000001) == 0x00000001 && (currentTouchesState & 0x00000001) != 0x00000001;
                case TouchControllerButton.B:
                    //ovrButton_B
                    return (previousTouchesState & 0x00000002) == 0x00000002 && (currentTouchesState & 0x00000002) != 0x00000002;
                case TouchControllerButton.X:
                    //ovrButton_X
                    return (previousTouchesState & 0x00000100) == 0x00000100 && (currentTouchesState & 0x00000100) != 0x00000100;
                case TouchControllerButton.Y:
                    //ovrButton_Y
                    return (previousTouchesState & 0x00000200) == 0x00000200 && (currentTouchesState & 0x00000200) != 0x00000200;
                case TouchControllerButton.Trigger:
                    return previousTrigger > triggerAndGripDeadzone && currentTrigger <= triggerAndGripDeadzone;
                case TouchControllerButton.Grip:
                    return previousGrip > triggerAndGripDeadzone && currentGrip <= triggerAndGripDeadzone;
                default:
                    return false;
            }
        }
    }
}