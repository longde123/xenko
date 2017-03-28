// Copyright (c) 2016-2017 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System;
using SiliconStudio.Core;
using SiliconStudio.Core.Collections;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Rendering.Lights;

namespace SiliconStudio.Xenko.Rendering.Shadows
{
    /// <summary>
    /// Base class for shadow map renderers
    /// </summary>
    [DataContract(Inherited = true, DefaultMemberMode = DataMemberMode.Never)]
    public abstract class LightShadowMapRendererBase : ILightShadowMapRenderer
    {
        protected PoolListStruct<ShadowMapRenderView> ShadowRenderViews;
        protected PoolListStruct<LightShadowMapTexture> ShadowMaps;

        protected LightShadowMapRendererBase()
        {
            ShadowRenderViews = new PoolListStruct<ShadowMapRenderView>(16, () => new ShadowMapRenderView());
            ShadowMaps = new PoolListStruct<LightShadowMapTexture>(16, () => new LightShadowMapTexture());
        }

        /// <summary>
        /// The shadow map render stage this light shadow map renderer uses
        /// </summary>
        [DataMember]
        public RenderStage ShadowCasterRenderStage { get; set; }

        public virtual void Reset(RenderContext context)
        {
            foreach (var view in ShadowRenderViews)
                context.RenderSystem.Views.Remove(view);

            ShadowRenderViews.Clear();
            ShadowMaps.Clear();
        }

        public virtual LightShadowType GetShadowType(LightShadowMap shadowMap)
        {
            var shadowType = (LightShadowType)0;
            switch (shadowMap.GetCascadeCount())
            {
                case 1:
                    shadowType |= LightShadowType.Cascade1;
                    break;
                case 2:
                    shadowType |= LightShadowType.Cascade2;
                    break;
                case 4:
                    shadowType |= LightShadowType.Cascade4;
                    break;
            }

            var pcfFilter = shadowMap.Filter as LightShadowMapFilterTypePcf;
            if (pcfFilter != null)
            {
                switch (pcfFilter.FilterSize)
                {
                    case LightShadowMapFilterTypePcfSize.Filter3x3:
                        shadowType |= LightShadowType.PCF3x3;
                        break;
                    case LightShadowMapFilterTypePcfSize.Filter5x5:
                        shadowType |= LightShadowType.PCF5x5;
                        break;
                    case LightShadowMapFilterTypePcfSize.Filter7x7:
                        shadowType |= LightShadowType.PCF7x7;
                        break;
                }
            }

            if (shadowMap.Debug)
            {
                shadowType |= LightShadowType.Debug;
            }
            return shadowType;
        }

        public abstract ILightShadowMapShaderGroupData CreateShaderGroupData(LightShadowType shadowType);

        public abstract bool CanRenderLight(IDirectLight light);

        public abstract void Collect(RenderContext context, RenderView sourceView, LightShadowMapTexture lightShadowMap);

        public virtual void ApplyViewParameters(RenderDrawContext context, ParameterCollection parameters, LightShadowMapTexture shadowMapTexture)
        {
        }

        public virtual LightShadowMapTexture CreateShadowMapTexture(RenderView renderView, LightComponent lightComponent, IDirectLight light, int shadowMapSize)
        {
            var shadowMap = ShadowMaps.Add();
            shadowMap.Initialize(renderView, lightComponent, light, light.Shadow, shadowMapSize, this);
            return shadowMap;
        }

        /// <summary>
        /// Creates a default view with the shadow caster stage added to it
        /// </summary>
        public virtual ShadowMapRenderView CreateRenderView()
        {
            var shadowRenderView = ShadowRenderViews.Add();
            shadowRenderView.RenderStages.Add(ShadowCasterRenderStage);
            return shadowRenderView;
        }
    }
}