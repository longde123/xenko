// <auto-generated>
// Do not edit this file yourself!
//
// This code was generated by Paradox Shader Mixin Code Generator.
// To generate it yourself, please install SiliconStudio.Paradox.VisualStudio.Package .vsix
// and re-save the associated .pdxfx.
// </auto-generated>

using SiliconStudio.Core;
using SiliconStudio.Paradox.Effects;
using SiliconStudio.Paradox.Shaders;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Paradox.Graphics;


#line 3 "C:\Projects\Paradox\sources\shaders\DefaultDeferredEffect.pdxfx"
using SiliconStudio.Paradox.Effects.Data;

#line 4
using SiliconStudio.Paradox.Engine;

#line 5
using SiliconStudio.Paradox.DataModel;

#line 7
namespace DefaultEffects
{
    [DataContract]
#line 10
    public partial class LightingParameters : ShaderMixinParameters
    {

        #line 12
        public static readonly ParameterKey<int> PerPixelDirectionalLightCount = ParameterKeys.New<int>();

        #line 13
        public static readonly ParameterKey<int> PerPixelDiffuseDirectionalLightCount = ParameterKeys.New<int>();

        #line 14
        public static readonly ParameterKey<int> PerVertexDirectionalLightCount = ParameterKeys.New<int>();

        #line 15
        public static readonly ParameterKey<int> PerVertexDiffusePixelSpecularDirectionalLightCount = ParameterKeys.New<int>();
    };

    #line 19
    public partial class ParadoxGBufferShaderPass  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 24
            context.CloneProperties();

            #line 24
            mixin.Mixin.CloneFrom(mixin.Parent.Mixin);

            #line 25
            context.Mixin(mixin, "GBuffer");

            #line 26
            context.Mixin(mixin, "NormalVSStream");

            #line 28
            if (context.GetParam(MaterialParameters.SpecularPowerMap) != null)
            {

                #line 30
                context.Mixin(mixin, "SpecularPower");

                {

                    #line 31
                    var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                    #line 31
                    context.Mixin(__subMixin, context.GetParam(MaterialParameters.SpecularPowerMap));
                    mixin.Mixin.AddComposition("SpecularPowerMap", __subMixin.Mixin);
                }
            }

            #line 34
            if (context.GetParam(MaterialParameters.SpecularIntensityMap) != null)
            {

                {

                    #line 36
                    var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                    #line 36
                    context.Mixin(__subMixin, context.GetParam(MaterialParameters.SpecularIntensityMap));
                    mixin.Mixin.AddComposition("SpecularIntensityMap", __subMixin.Mixin);
                }
            }
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxGBufferShaderPass", new ParadoxGBufferShaderPass());
        }
    }

    #line 41
    public partial class ParadoxGBufferPlugin  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            {

                #line 43
                var __subMixin = new ShaderMixinSourceTree() { Name = "ParadoxGBufferShaderPass", Parent = mixin };
                mixin.Children.Add(__subMixin);

                #line 43
                context.BeginChild(__subMixin);

                #line 43
                context.Mixin(__subMixin, "ParadoxGBufferShaderPass");

                #line 43
                context.EndChild();
            }

            #line 47
            context.RemoveMixin(mixin, "NormalVSStream");

            #line 48
            context.RemoveMixin(mixin, "SpecularPowerMap");

            #line 49
            context.RemoveMixin(mixin, "SpecularPowerPerMesh");

            #line 52
            context.Mixin(mixin, "NormalVSGBuffer");

            #line 54
            context.Mixin(mixin, "SpecularPowerGBuffer");
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxGBufferPlugin", new ParadoxGBufferPlugin());
        }
    }

    #line 57
    public partial class ParadoxDeferredLightingPointGroup  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 61
            mixin.Mixin.AddMacro("DEFERRED_MAX_POINT_LIGHT_COUNT", context.GetParam(LightingKeys.MaxDeferredLights));

            #line 62
            context.Mixin(mixin, "DeferredPointLighting");
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxDeferredLightingPointGroup", new ParadoxDeferredLightingPointGroup());
        }
    }

    #line 65
    public partial class DeferredLightingDirectShadowGroup  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 70
            context.Mixin(mixin, "DeferredDirectionalShadowLighting");

            #line 72
            context.Mixin(mixin, "ShadowMapCascadeBase");

            #line 74
            mixin.Mixin.AddMacro("SHADOWMAP_COUNT", 1);

            #line 75
            mixin.Mixin.AddMacro("SHADOWMAP_CASCADE_COUNT", context.GetParam(ShadowMapParameters.ShadowMapCascadeCount));

            #line 76
            mixin.Mixin.AddMacro("SHADOWMAP_TOTAL_COUNT", context.GetParam(ShadowMapParameters.ShadowMapCascadeCount));

            #line 77
            mixin.Mixin.AddMacro("HAS_DYNAMIC_SHADOWMAP_COUNT", 0);
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("DeferredLightingDirectShadowGroup", new DeferredLightingDirectShadowGroup());
        }
    }

    #line 80
    public partial class DeferredLightingSpotShadowGroup  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 85
            context.Mixin(mixin, "DeferredSpotShadowLighting");

            #line 87
            context.Mixin(mixin, "ShadowMapCascadeBase");

            #line 89
            mixin.Mixin.AddMacro("SHADOWMAP_COUNT", 1);

            #line 90
            mixin.Mixin.AddMacro("SHADOWMAP_CASCADE_COUNT", context.GetParam(ShadowMapParameters.ShadowMapCascadeCount));

            #line 91
            mixin.Mixin.AddMacro("SHADOWMAP_TOTAL_COUNT", context.GetParam(ShadowMapParameters.ShadowMapCascadeCount));

            #line 92
            mixin.Mixin.AddMacro("HAS_DYNAMIC_SHADOWMAP_COUNT", 0);
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("DeferredLightingSpotShadowGroup", new DeferredLightingSpotShadowGroup());
        }
    }

    #line 95
    public partial class DeferredLightTypeGroup  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 99
            if (context.GetParam(ShadowMapParameters.LightType) == LightType.Directional)

                #line 100
                context.Mixin(mixin, "DeferredLightingDirectShadowGroup");

            #line 101
            else 
#line 101
            if (context.GetParam(ShadowMapParameters.LightType) == LightType.Spot)

                #line 102
                context.Mixin(mixin, "DeferredLightingSpotShadowGroup");

            #line 104
            else

                #line 104
                context.Mixin(mixin, "DeferredLightingDirectShadowGroup");
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("DeferredLightTypeGroup", new DeferredLightTypeGroup());
        }
    }

    #line 107
    public partial class NearestFilterGroup  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 109
            context.Mixin(mixin, "DeferredLightTypeGroup");

            #line 110
            context.Mixin(mixin, "ShadowMapFilterDefault");
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("NearestFilterGroup", new NearestFilterGroup());
        }
    }

    #line 113
    public partial class PcfGroup  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 115
            context.Mixin(mixin, "DeferredLightTypeGroup");

            #line 116
            context.Mixin(mixin, "ShadowMapFilterPcf");
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("PcfGroup", new PcfGroup());
        }
    }

    #line 119
    public partial class VsmGroup  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 121
            context.Mixin(mixin, "DeferredLightTypeGroup");

            #line 122
            context.Mixin(mixin, "ShadowMapFilterVsm");
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("VsmGroup", new VsmGroup());
        }
    }

    #line 125
    public partial class ParadoxShadowPrepassLighting  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 129
            context.CloneProperties();

            #line 129
            mixin.Mixin.CloneFrom(mixin.Parent.Mixin);

            #line 130
            context.Mixin(mixin, "DeferredShadowLightingShader");

            #line 132
            if (context.GetParam(ShadowMapParameters.FilterType) == ShadowMapFilterType.Nearest)

                {

                    #line 133
                    var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                    #line 133
                    context.Mixin(__subMixin, "NearestFilterGroup");
                    mixin.Mixin.AddCompositionToArray("shadows", __subMixin.Mixin);
                }

            #line 134
            else 
#line 134
            if (context.GetParam(ShadowMapParameters.FilterType) == ShadowMapFilterType.PercentageCloserFiltering)

                {

                    #line 135
                    var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                    #line 135
                    context.Mixin(__subMixin, "PcfGroup");
                    mixin.Mixin.AddCompositionToArray("shadows", __subMixin.Mixin);
                }

            #line 136
            else 
#line 136
            if (context.GetParam(ShadowMapParameters.FilterType) == ShadowMapFilterType.Variance)

                {

                    #line 137
                    var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                    #line 137
                    context.Mixin(__subMixin, "VsmGroup");
                    mixin.Mixin.AddCompositionToArray("shadows", __subMixin.Mixin);
                }
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxShadowPrepassLighting", new ParadoxShadowPrepassLighting());
        }
    }

    #line 140
    public partial class ParadoxDeferredLightingDirectGroup  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 144
            context.Mixin(mixin, "DeferredDirectionalLighting");
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxDeferredLightingDirectGroup", new ParadoxDeferredLightingDirectGroup());
        }
    }

    #line 147
    public partial class ParadoxDeferredLightingSpotGroup  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 151
            context.Mixin(mixin, "DeferredSpotLighting");
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxDeferredLightingSpotGroup", new ParadoxDeferredLightingSpotGroup());
        }
    }

    #line 154
    public partial class ParadoxDirectPrepassLighting  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 156
            context.CloneProperties();

            #line 156
            mixin.Mixin.CloneFrom(mixin.Parent.Mixin);

            {

                #line 157
                var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                #line 157
                context.Mixin(__subMixin, "ParadoxDeferredLightingDirectGroup");
                mixin.Mixin.AddCompositionToArray("lightingGroups", __subMixin.Mixin);
            }
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxDirectPrepassLighting", new ParadoxDirectPrepassLighting());
        }
    }

    #line 160
    public partial class ParadoxPointPrepassLighting  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 164
            context.CloneProperties();

            #line 164
            mixin.Mixin.CloneFrom(mixin.Parent.Mixin);

            #line 165
            if (context.GetParam(LightingKeys.MaxDeferredLights) > 0)

                {

                    #line 166
                    var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                    #line 166
                    context.Mixin(__subMixin, "ParadoxDeferredLightingPointGroup");
                    mixin.Mixin.AddCompositionToArray("lightingGroups", __subMixin.Mixin);
                }
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxPointPrepassLighting", new ParadoxPointPrepassLighting());
        }
    }

    #line 169
    public partial class ParadoxSpotPrepassLighting  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 173
            context.CloneProperties();

            #line 173
            mixin.Mixin.CloneFrom(mixin.Parent.Mixin);

            {

                #line 174
                var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                #line 174
                context.Mixin(__subMixin, "ParadoxDeferredLightingSpotGroup");
                mixin.Mixin.AddCompositionToArray("lightingGroups", __subMixin.Mixin);
            }
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxSpotPrepassLighting", new ParadoxSpotPrepassLighting());
        }
    }

    #line 177
    public partial class ParadoxDeferredSpecular  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 179
            context.Mixin(mixin, "ComputeBRDFColorSpecularBlinnPhong");

            #line 180
            context.Mixin(mixin, "SpecularPowerGBuffer");

            {

                #line 181
                var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                #line 181
                context.Mixin(__subMixin, "ComputeColorOne");
                mixin.Mixin.AddComposition("SpecularIntensityMap", __subMixin.Mixin);
            }
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxDeferredSpecular", new ParadoxDeferredSpecular());
        }
    }

    #line 184
    public partial class ParadoxDefaultLightPrepassEffect  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 188
            context.Mixin(mixin, "PositionVSGBuffer");

            #line 189
            context.Mixin(mixin, "NormalVSGBuffer");

            #line 190
            context.Mixin(mixin, "BRDFDiffuseBase");

            #line 191
            context.Mixin(mixin, "BRDFSpecularBase");

            {

                #line 192
                var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                #line 192
                context.Mixin(__subMixin, "ComputeBRDFColorFresnel");
                mixin.Mixin.AddComposition("DiffuseColor", __subMixin.Mixin);
            }

            {

                #line 193
                var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                #line 193
                context.Mixin(__subMixin, "ComputeBRDFDiffuseLambert");
                mixin.Mixin.AddComposition("DiffuseLighting", __subMixin.Mixin);
            }

            {

                #line 194
                var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                #line 194
                context.Mixin(__subMixin, "ComputeBRDFColor");
                mixin.Mixin.AddComposition("SpecularColor", __subMixin.Mixin);
            }

            {

                #line 195
                var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                #line 195
                context.Mixin(__subMixin, "ParadoxDeferredSpecular");
                mixin.Mixin.AddComposition("SpecularLighting", __subMixin.Mixin);
            }

            {

                #line 197
                var __subMixin = new ShaderMixinSourceTree() { Name = "ParadoxShadowPrepassLighting", Parent = mixin };
                mixin.Children.Add(__subMixin);

                #line 197
                context.BeginChild(__subMixin);

                #line 197
                context.Mixin(__subMixin, "ParadoxShadowPrepassLighting");

                #line 197
                context.EndChild();
            }

            #line 199
            context.Mixin(mixin, "DeferredLightingShader");

            {

                #line 201
                var __subMixin = new ShaderMixinSourceTree() { Name = "ParadoxDirectPrepassLighting", Parent = mixin };
                mixin.Children.Add(__subMixin);

                #line 201
                context.BeginChild(__subMixin);

                #line 201
                context.Mixin(__subMixin, "ParadoxDirectPrepassLighting");

                #line 201
                context.EndChild();
            }

            {

                #line 203
                var __subMixin = new ShaderMixinSourceTree() { Name = "ParadoxSpotPrepassLighting", Parent = mixin };
                mixin.Children.Add(__subMixin);

                #line 203
                context.BeginChild(__subMixin);

                #line 203
                context.Mixin(__subMixin, "ParadoxSpotPrepassLighting");

                #line 203
                context.EndChild();
            }

            {

                #line 205
                var __subMixin = new ShaderMixinSourceTree() { Name = "ParadoxPointPrepassLighting", Parent = mixin };
                mixin.Children.Add(__subMixin);

                #line 205
                context.BeginChild(__subMixin);

                #line 205
                context.Mixin(__subMixin, "ParadoxPointPrepassLighting");

                #line 205
                context.EndChild();
            }
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxDefaultLightPrepassEffect", new ParadoxDefaultLightPrepassEffect());
        }
    }

    #line 208
    public partial class DirectionalLightsShader  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 213
            mixin.Mixin.AddMacro("LIGHTING_MAX_LIGHT_COUNT", context.GetParam(LightingKeys.MaxDirectionalLights));

            #line 214
            if (context.GetParam(LightingKeys.UnrollDirectionalLightLoop))

                #line 215
                mixin.Mixin.AddMacro("LIGHTING_UNROLL_LOOP", true);

            #line 217
            if (context.GetParam(MaterialParameters.LightingType) == MaterialLightingType.DiffusePixel)
            {

                #line 219
                context.Mixin(mixin, "ShadingDiffusePerPixel");
            }

            #line 221
            else 
#line 221
            if (context.GetParam(MaterialParameters.LightingType) == MaterialLightingType.DiffuseVertex)
            {

                #line 223
                context.Mixin(mixin, "ShadingDiffusePerVertex");
            }

            #line 225
            else 
#line 225
            if (context.GetParam(MaterialParameters.LightingType) == MaterialLightingType.DiffuseSpecularPixel)
            {

                #line 227
                context.Mixin(mixin, "ShadingDiffuseSpecularPerPixel");
            }

            #line 229
            else 
#line 229
            if (context.GetParam(MaterialParameters.LightingType) == MaterialLightingType.DiffuseVertexSpecularPixel)
            {

                #line 231
                context.Mixin(mixin, "ShadingDiffusePerVertexSpecularPerPixel");
            }

            #line 233
            context.Mixin(mixin, "DirectionalShading");

            #line 234
            context.Mixin(mixin, "ShadingEyeNormalVS");
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("DirectionalLightsShader", new DirectionalLightsShader());
        }
    }

    #line 237
    public partial class ParadoxDiffuseDeferred  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 241
            if (context.GetParam(MaterialParameters.AlbedoDiffuse) != null)
            {

                {

                    #line 243
                    var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                    #line 243
                    context.Mixin(__subMixin, context.GetParam(MaterialParameters.AlbedoDiffuse));
                    mixin.Mixin.AddComposition("albedoDiffuse", __subMixin.Mixin);
                }
            }
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxDiffuseDeferred", new ParadoxDiffuseDeferred());
        }
    }

    #line 247
    public partial class ParadoxSpecularDeferred  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 251
            if (context.GetParam(MaterialParameters.AlbedoSpecular) != null)
            {

                {

                    #line 253
                    var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                    #line 253
                    context.Mixin(__subMixin, context.GetParam(MaterialParameters.AlbedoSpecular));
                    mixin.Mixin.AddComposition("albedoSpecular", __subMixin.Mixin);
                }
            }
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxSpecularDeferred", new ParadoxSpecularDeferred());
        }
    }

    #line 257
    public partial class ParadoxDefaultDeferredShader  : IShaderMixinBuilder
    {
        public void Generate(ShaderMixinSourceTree mixin, ShaderMixinContext context)
        {

            #line 262
            context.Mixin(mixin, "ParadoxBaseShader");

            #line 264
            context.Mixin(mixin, "ParadoxSkinning");

            #line 266
            context.Mixin(mixin, "ParadoxShadowCast");

            #line 270
            if (context.GetParam(RenderingParameters.UseDeferred) && !context.GetParam(MaterialParameters.UseTransparent))
            {

                #line 272
                context.Mixin(mixin, "ParadoxGBufferPlugin");

                #line 273
                context.Mixin(mixin, "LightDeferredShading");

                #line 274
                context.Mixin(mixin, "ParadoxDiffuseDeferred");

                #line 275
                context.Mixin(mixin, "ParadoxSpecularDeferred");

                #line 277
                if (context.GetParam(MaterialParameters.AmbientMap) != null)
                {

                    #line 279
                    context.Mixin(mixin, "AmbientMapShading");

                    {

                        #line 280
                        var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                        #line 280
                        context.Mixin(__subMixin, context.GetParam(MaterialParameters.AmbientMap));
                        mixin.Mixin.AddComposition("AmbientMap", __subMixin.Mixin);
                    }
                }
            }

            #line 284
            else
            {

                #line 285
                context.Mixin(mixin, "ParadoxDiffuseForward");

                #line 286
                context.Mixin(mixin, "ParadoxSpecularForward");

                #line 288
                if (context.GetParam(MaterialParameters.AmbientMap) != null)
                {

                    #line 290
                    context.Mixin(mixin, "AmbientMapShading");

                    {

                        #line 291
                        var __subMixin = new ShaderMixinSourceTree() { Parent = mixin };

                        #line 291
                        context.Mixin(__subMixin, context.GetParam(MaterialParameters.AmbientMap));
                        mixin.Mixin.AddComposition("AmbientMap", __subMixin.Mixin);
                    }
                }

                #line 294
                if (context.GetParam(MaterialParameters.UseTransparent))
                {

                    #line 296
                    context.Mixin(mixin, "TransparentShading");

                    #line 297
                    context.Mixin(mixin, "DiscardTransparent");
                }
            }
        }

        [ModuleInitializer]
        internal static void __Initialize__()

        {
            ShaderMixinManager.Register("ParadoxDefaultDeferredShader", new ParadoxDefaultDeferredShader());
        }
    }
}
