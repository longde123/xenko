using System;

namespace SiliconStudio.Xenko.Assets.Scripts
{
    /// <summary>
    /// <see cref="Block.Slots"/> need to be recomputed if a member with this attribute is changed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class RegenerateSlotsAttribute : Attribute
    {
        
    }
}