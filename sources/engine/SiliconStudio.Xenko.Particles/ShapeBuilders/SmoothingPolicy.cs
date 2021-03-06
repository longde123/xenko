// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.

using SiliconStudio.Core;

namespace SiliconStudio.Xenko.Particles.ShapeBuilders
{
    /// <summary>
    /// Specifies if the ribbon should be additionally smoothed or rendered as is.
    /// </summary>
    [DataContract("SmoothingPolicy")]
    [Display("Smoothing")]
    public enum SmoothingPolicy
    {
        /// <summary>
        /// Ribbons only use control points and edges are hard. Good for straight lines
        /// </summary>
        None,

        /// <summary>
        /// Smoothing using Catmull-Rom interpolation. Generally looks good
        /// </summary>
        Fast,

        /// <summary>
        /// Smoothing based on circumcircles generated around every three adjacent points. Best suited for rapid, circular motions
        /// </summary>
        Best, 
    }
}
