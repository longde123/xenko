// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.

using System.ComponentModel;
using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;

namespace SiliconStudio.Xenko.Assets.Textures
{
    [DataContract("NormalMapTextureType")]
    [Display("Normal Map")]
    public class NormapMapTextureType : ITextureType
    {
        public bool IsSRgb(ColorSpace colorSpaceReference) => false;

        /// <summary>
        /// Indicating whether the Y-component of normals should be inverted, to compensate for a flipped tangent-space.
        /// </summary>
        /// <userdoc>
        /// Indicates that a positive Y-component (green) faces up in tangent space. This options depends on your normal maps generation tools.
        /// </userdoc>
        [DataMember(10)]
        [DefaultValue(true)]
        public bool InvertY { get; set; } = true;

        bool ITextureType.ColorKeyEnabled => false;

        Color ITextureType.ColorKeyColor => new Color();

        AlphaFormat ITextureType.Alpha => AlphaFormat.None;

        bool ITextureType.PremultiplyAlpha => false;

        TextureHint ITextureType.Hint => TextureHint.NormalMap;
    }
}
