// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System;
using SiliconStudio.Core;
using SiliconStudio.Core.IO;
using SiliconStudio.Core.Reflection;
using SiliconStudio.Core.Serialization;
using SiliconStudio.Core.Serialization.Contents;
using SiliconStudio.Core.Yaml;
using SiliconStudio.Core.Yaml.Events;
using SiliconStudio.Core.Yaml.Serialization;

namespace SiliconStudio.Assets.Serializers
{
    [YamlSerializerFactory(YamlAssetProfile.Name)]
    public class ContentReferenceSerializer : AssetScalarSerializerBase
    {
        public static readonly ContentReferenceSerializer Default = new ContentReferenceSerializer();

        public override bool CanVisit(Type type)
        {
            return AssetRegistry.IsContentType(type);
        }

        public override object ConvertFrom(ref ObjectContext context, Scalar fromScalar)
        {
            AssetId guid;
            UFile location;
            Guid referenceId;
            if (!AssetReference.TryParse(fromScalar.Value, out guid, out location))
            {
                throw new YamlException(fromScalar.Start, fromScalar.End, "Unable to decode asset reference [{0}]. Expecting format GUID:LOCATION".ToFormat(fromScalar.Value));
            }

            var instance = AttachedReferenceManager.CreateProxyObject(context.Descriptor.Type, guid, location);
            return instance;
        }

        public override string ConvertTo(ref ObjectContext objectContext)
        {
            var attachedReference = AttachedReferenceManager.GetAttachedReference(objectContext.Instance);
            if (attachedReference == null)
                throw new YamlException($"Unable to extract asset reference from object [{objectContext.Instance}]");

            return $"{attachedReference.Id}:{attachedReference.Url}";
        }
    }
}
