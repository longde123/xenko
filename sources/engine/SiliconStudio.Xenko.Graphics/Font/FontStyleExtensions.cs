// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
namespace SiliconStudio.Xenko.Graphics.Font
{
    /// <summary>
    /// Extension methods for type <see cref="FontStyle"/>.
    /// </summary>
    public static class FontStyleExtensions
    {
        /// <summary>
        /// Indicate if the style is bold (partially bold).
        /// </summary>
        /// <param name="style">the style</param>
        /// <returns><value>true</value> if bold, false otherwise</returns>
        public static bool IsBold(this FontStyle style)
        {
            return (style & FontStyle.Bold) != 0;
        }

        /// <summary>
        /// Indicate if the style is italic (partially italic).
        /// </summary>
        /// <param name="style">the style</param>
        /// <returns><value>true</value> if italic, false otherwise</returns>
        public static bool IsItalic(this FontStyle style)
        {
            return (style & FontStyle.Italic) != 0;
        }
    }
}
