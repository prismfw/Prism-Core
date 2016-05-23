/*
Copyright (C) 2016  Prism Framework Team

This file is part of the Prism Framework.

The Prism Framework is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

The Prism Framework is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism
{
    /// <summary>
    /// Represents a description of a property member.
    /// </summary>
    public sealed class PropertyDescriptor
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public string Name
        {
            get { return propertyInfo.Name; }
        }

        /// <summary>
        /// Gets a value indicating whether the property is read-only.
        /// </summary>
        public bool IsReadOnly { get; }

        /// <summary>
        /// Gets the type of which the property is a member.
        /// </summary>
        public Type OwnerType
        {
            get { return propertyInfo.DeclaringType; }
        }

        /// <summary>
        /// Gets the type of value that the property holds.
        /// </summary>
        public Type PropertyType
        {
            get { return propertyInfo.PropertyType; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static Dictionary<Type, List<PropertyDescriptor>> currentDescriptors { get; } = new Dictionary<Type, List<PropertyDescriptor>>();

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly PropertyInfo propertyInfo;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly Dictionary<Type, PropertyMetadata> typeMetadatas = new Dictionary<Type, PropertyMetadata>();

        internal PropertyDescriptor(PropertyInfo info)
        {
            propertyInfo = info;
            typeMetadatas[info.DeclaringType] = new PropertyMetadata(PropertyMetadataOptions.None);

            IsReadOnly = info.SetMethod == null || (!info.SetMethod.IsPublic && info.Module == GetType().GetTypeInfo().Module);
        }

        private PropertyDescriptor(string name, Type propertyType, Type ownerType, bool isReadOnly, PropertyMetadata metadata)
        {
            propertyInfo = ownerType.GetRuntimeProperties().FirstOrDefault(p => p.Name == name && p.PropertyType == propertyType);
            if (propertyInfo == null)
            {
                throw new MissingMemberException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.CannotLocatePropertyWithNameAndTypeForType, name, propertyType.Name, ownerType.FullName));
            }

            IsReadOnly = isReadOnly;

            if (metadata == null)
            {
                metadata = new PropertyMetadata(PropertyMetadataOptions.None);
            }
            typeMetadatas[ownerType] = metadata;
            metadata.OnApply(this, null);
            metadata.IsSealed = true;
        }

        /// <summary>
        /// Creates a <see cref="PropertyDescriptor"/> for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="propertyType">The type of value that the property holds.</param>
        /// <param name="ownerType">The type of which the property is a member.</param>
        /// <returns>The newly created <see cref="PropertyDescriptor"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c>, an empty string, or a whitespace-only string.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyType"/> is <c>null</c> -or- when <paramref name="ownerType"/> is <c>null</c>.</exception>
        /// <exception cref="MissingMemberException">Thrown when a property with the specified name and type cannot be located for the <paramref name="ownerType"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a descriptor for the specified property has already been created.</exception>
        public static PropertyDescriptor Create(string name, Type propertyType, Type ownerType)
        {
            return Create(name, propertyType, ownerType, false, null);
        }

        /// <summary>
        /// Creates a <see cref="PropertyDescriptor"/> for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="propertyType">The type of value that the property holds.</param>
        /// <param name="ownerType">The type of which the property is a member.</param>
        /// <param name="isReadOnly">Whether the property is considered read-only.</param>
        /// <returns>The newly created <see cref="PropertyDescriptor"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c>, an empty string, or a whitespace-only string.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyType"/> is <c>null</c> -or- when <paramref name="ownerType"/> is <c>null</c>.</exception>
        /// <exception cref="MissingMemberException">Thrown when a property with the specified name and type cannot be located for the <paramref name="ownerType"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a descriptor for the specified property has already been created.</exception>
        public static PropertyDescriptor Create(string name, Type propertyType, Type ownerType, bool isReadOnly)
        {
            return Create(name, propertyType, ownerType, isReadOnly, null);
        }

        /// <summary>
        /// Creates a <see cref="PropertyDescriptor"/> for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="propertyType">The type of value that the property holds.</param>
        /// <param name="ownerType">The type of which the property is a member.</param>
        /// <param name="metadata">The metadata to apply to the property, if any.</param>
        /// <returns>The newly created <see cref="PropertyDescriptor"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c>, an empty string, or a whitespace-only string.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyType"/> is <c>null</c> -or- when <paramref name="ownerType"/> is <c>null</c>.</exception>
        /// <exception cref="MissingMemberException">Thrown when a property with the specified name and type cannot be located for the <paramref name="ownerType"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a descriptor for the specified property has already been created.</exception>
        public static PropertyDescriptor Create(string name, Type propertyType, Type ownerType, PropertyMetadata metadata)
        {
            return Create(name, propertyType, ownerType, false, metadata);
        }

        /// <summary>
        /// Creates a <see cref="PropertyDescriptor"/> for the specified property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="propertyType">The type of value that the property holds.</param>
        /// <param name="ownerType">The type of which the property is a member.</param>
        /// <param name="isReadOnly">Whether the property is considered read-only.</param>
        /// <param name="metadata">The metadata to apply to the property, if any.</param>
        /// <returns>The newly created <see cref="PropertyDescriptor"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c>, an empty string, or a whitespace-only string.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyType"/> is <c>null</c> -or- when <paramref name="ownerType"/> is <c>null</c>.</exception>
        /// <exception cref="MissingMemberException">Thrown when a property with the specified name and type cannot be located for the <paramref name="ownerType"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a descriptor for the specified property has already been created.</exception>
        public static PropertyDescriptor Create(string name, Type propertyType, Type ownerType, bool isReadOnly, PropertyMetadata metadata)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNullEmptyOrWhitespace, nameof(name));
            }

            if (propertyType == null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            if (ownerType == null)
            {
                throw new ArgumentNullException(nameof(ownerType));
            }

            List<PropertyDescriptor> descriptors;
            if (!currentDescriptors.TryGetValue(ownerType, out descriptors))
            {
                descriptors = new List<PropertyDescriptor>();
                currentDescriptors[ownerType] = descriptors;
            }

            if (descriptors.Any(d => d.Name == name && d.PropertyType == propertyType))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.PropertyDescriptorAlreadyCreated, name, ownerType.FullName));
            }

            var descriptor = new PropertyDescriptor(name, propertyType, ownerType, isReadOnly, metadata);
            descriptors.Add(descriptor);
            return descriptor;
        }

        /// <summary>
        /// Gets the property metadata that is associated with the specified type.
        /// </summary>
        /// <param name="forType">The type for which to return the property metadata.</param>
        /// <returns>The property metadata that is associated with the specified type as a <see cref="PropertyMetadata"/> instance.</returns>
        public PropertyMetadata GetMetadata(Type forType)
        {
            while (forType != null)
            {
                PropertyMetadata metadata;
                if (typeMetadatas.TryGetValue(forType, out metadata))
                {
                    return metadata;
                };

                forType = forType.GetTypeInfo().BaseType;
            }

            return typeMetadatas[OwnerType];
        }

        /// <summary>
        /// Specifies alternate metadata for the property when it is present on instances of the specified type.
        /// </summary>
        /// <param name="forType">The type for which the metadata will be applied.</param>
        /// <param name="metadata">The metadata to apply to the property.</param>
        /// <exception cref="InvalidOperationException">Thrown when the property is read-only.</exception>
        /// <exception cref="ArgumentException">Thrown when alternate metadata has already been specified for the provided type.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="metadata"/> type is less derived than the base metadata type.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="forType"/> is <c>null</c> -or- when <paramref name="metadata"/> is <c>null</c>.</exception>
        public void OverrideMetadata(Type forType, PropertyMetadata metadata)
        {
            if (forType == null)
            {
                throw new ArgumentNullException(nameof(forType));
            }

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (IsReadOnly)
            {
                throw new InvalidOperationException(Resources.Strings.CannotOverrideMetadataOnReadOnlyProperty);
            }

            if (typeMetadatas.ContainsKey(forType))
            {
                throw new ArgumentException(Resources.Strings.TypeHasMetadata);
            }

            var baseType = forType.GetTypeInfo().BaseType;
            while (baseType != null)
            {
                PropertyMetadata baseMetadata;
                if (typeMetadatas.TryGetValue(baseType, out baseMetadata))
                {
                    if (metadata.GetType() != baseMetadata.GetType() && !metadata.GetType().GetTypeInfo().IsSubclassOf(baseMetadata.GetType()))
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.MetadataMustBeSameAsOrDeriveFromBase, baseMetadata.GetType().FullName), nameof(metadata));
                    }

                    metadata.Merge(baseMetadata, this);
                    typeMetadatas[forType] = metadata;
                    metadata.OnApply(this, forType);
                    metadata.IsSealed = true;
                    return;
                }

                baseType = baseType.GetTypeInfo().BaseType;
            }
        }

        internal object GetValue(object obj)
        {
            return propertyInfo.GetValue(obj);
        }

        internal void SetValue(object obj, object value)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException(Resources.Strings.AttemptedToSetReadOnlyProperty);
            }

            propertyInfo.SetValue(obj, value);
        }
    }
}