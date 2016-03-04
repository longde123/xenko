﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

#if SILICONSTUDIO_XENKO_GRAPHICS_API_DIRECT3D11
using System;

using SharpDX.DXGI;

using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Shaders;
using SharpDX.Mathematics.Interop;
using SiliconStudio.Core.Diagnostics;

namespace SiliconStudio.Xenko.Graphics
{
    public partial class GraphicsDevice
    {
        private const GraphicsPlatform GraphicPlatform = GraphicsPlatform.Direct3D11;

        private bool simulateReset = false;

        private SharpDX.Direct3D11.Device nativeDevice;
        private SharpDX.Direct3D11.DeviceContext nativeDeviceContext;

        private SharpDX.Direct3D11.DeviceCreationFlags creationFlags;

        // Used by Texture.SetData

        /// <summary>
        ///     Gets the status of this device.
        /// </summary>
        /// <value>The graphics device status.</value>
        public GraphicsDeviceStatus GraphicsDeviceStatus
        {
            get
            {
                if (simulateReset)
                {
                    simulateReset = false;
                    return GraphicsDeviceStatus.Reset;
                }

                var result = NativeDevice.DeviceRemovedReason;
                if (result == SharpDX.DXGI.ResultCode.DeviceRemoved)
                {
                    return GraphicsDeviceStatus.Removed;
                }

                if (result == SharpDX.DXGI.ResultCode.DeviceReset)
                {
                    return GraphicsDeviceStatus.Reset;
                }

                if (result == SharpDX.DXGI.ResultCode.DeviceHung)
                {
                    return GraphicsDeviceStatus.Hung;
                }

                if (result == SharpDX.DXGI.ResultCode.DriverInternalError)
                {
                    return GraphicsDeviceStatus.InternalError;
                }

                if (result == SharpDX.DXGI.ResultCode.InvalidCall)
                {
                    return GraphicsDeviceStatus.InvalidCall;
                }

                if (result.Code < 0)
                {
                    return GraphicsDeviceStatus.Reset;
                }

                return GraphicsDeviceStatus.Normal;
            }
        }

        /// <summary>
        ///     Gets the native device.
        /// </summary>
        /// <value>The native device.</value>
        internal SharpDX.Direct3D11.Device NativeDevice
        {
            get
            {
                return nativeDevice;
            }
        }

        /// <summary>
        /// Gets the native device context.
        /// </summary>
        /// <value>The native device context.</value>
        internal SharpDX.Direct3D11.DeviceContext NativeDeviceContext
        {
            get
            {
                return nativeDeviceContext;
            }
        }

        /// <summary>
        ///     Marks context as active on the current thread.
        /// </summary>
        public void Begin()
        {
            FrameTriangleCount = 0;
            FrameDrawCalls = 0;
        }

        /// <summary>
        /// Enables profiling.
        /// </summary>
        /// <param name="enabledFlag">if set to <c>true</c> [enabled flag].</param>
        public void EnableProfile(bool enabledFlag)
        {
        }

        /// <summary>
        ///     Unmarks context as active on the current thread.
        /// </summary>
        public void End()
        {
        }

        /// <summary>
        /// Executes a deferred command list.
        /// </summary>
        /// <param name="commandList">The deferred command list.</param>
        public void ExecuteCommandList(CommandList commandList)
        {
            //if (commandList == null) throw new ArgumentNullException("commandList");
            //
            //NativeDeviceContext.ExecuteCommandList(((CommandList)commandList).NativeCommandList, false);
            //commandList.Dispose();
        }

        public void SimulateReset()
        {
            simulateReset = true;
        }

        private void InitializePostFeatures()
        {
            
        }

        /// <summary>
        ///     Initializes the specified device.
        /// </summary>
        /// <param name="graphicsProfiles">The graphics profiles.</param>
        /// <param name="deviceCreationFlags">The device creation flags.</param>
        /// <param name="windowHandle">The window handle.</param>
        private void InitializePlatformDevice(GraphicsProfile[] graphicsProfiles, DeviceCreationFlags deviceCreationFlags, object windowHandle)
        {
            if (nativeDevice != null)
            {
                // Destroy previous device
                ReleaseDevice();
            }

            // Profiling is supported through pix markers
            IsProfilingSupported = true;

            // Map GraphicsProfile to D3D11 FeatureLevel
            SharpDX.Direct3D.FeatureLevel[] levels = graphicsProfiles.ToFeatureLevel();
            creationFlags = (SharpDX.Direct3D11.DeviceCreationFlags)deviceCreationFlags;

            // Create Device D3D11 with feature Level based on profile
            nativeDevice = new SharpDX.Direct3D11.Device(Adapter.NativeAdapter, creationFlags, levels);
            nativeDeviceContext = nativeDevice.ImmediateContext;
            if (IsDebugMode)
            {
                GraphicsResourceBase.SetDebugName(this, nativeDeviceContext, "ImmediateContext");
            }
        }

        private void AdjustDefaultPipelineStateDescription(ref PipelineStateDescription pipelineStateDescription)
        {
            // On D3D, default state is Less instead of our LessEqual
            // Let's update default pipeline state so that it correspond to D3D state after a "ClearState()"
            pipelineStateDescription.DepthStencilState.DepthBufferFunction = CompareFunction.Less;
        }

        protected void DestroyPlatformDevice()
        {
            ReleaseDevice();
        }

        private void ReleaseDevice()
        {
            // Display D3D11 ref counting info
            //ClearState();
            NativeDevice.ImmediateContext.Flush();
            NativeDevice.ImmediateContext.Dispose();

            if (IsDebugMode)
            {
                var deviceDebug = new SharpDX.Direct3D11.DeviceDebug(NativeDevice);
                deviceDebug.ReportLiveDeviceObjects(SharpDX.Direct3D11.ReportingLevel.Detail);
            }

            nativeDevice.Dispose();
        }

        internal void OnDestroyed()
        {
        }
    }
}
#endif
