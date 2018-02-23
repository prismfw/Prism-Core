/*
Copyright (C) 2018  Prism Framework Team

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


using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Reflection
{
	/// <summary>
    /// Discovers the attributes of a property and provides access to property metadata.
    /// </summary>
    public sealed class PropertyInfoN : MemberInfoN
    {
        /// <summary>
        /// Gets the attributes for this property.
        /// </summary>
        public PropertyAttributes Attributes
        {
            get { return propertyInfo.Attributes; }
        }

        /// <summary>
        /// Gets a value indicating whether the property can be read.
        /// </summary>
        public bool CanRead
        {
            get { return propertyInfo.CanRead; }
        }

        /// <summary>
        /// Gets a value indicating whether the property can be written to.
        /// </summary>
        public bool CanWrite
        {
            get { return propertyInfo.CanWrite; }
        }

        /// <summary>
        /// Gets the get accessor for this property.
        /// </summary>
        public MethodInfoN GetMethod
        {
            get
            {
                if (getMethod == null && propertyInfo.GetMethod != null)
                {
                    getMethod = new MethodInfoN(propertyInfo.GetMethod);
                }

                return getMethod;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MethodInfoN getMethod;

        /// <summary>
        /// Gets a value indicating whether the property is the special name.
        /// </summary>
        public bool IsSpecialName
        {
            get { return propertyInfo.IsSpecialName; }
        }

        /// <summary>
        /// Gets the type of this property.
        /// </summary>
        public Type PropertyType
        {
            get { return propertyInfo.PropertyType; }
        }

        /// <summary>
        /// Gets the set accessor for this property.
        /// </summary>
        public MethodInfoN SetMethod
        {
            get
            {
                if (setMethod == null && propertyInfo.SetMethod != null)
                {
                    setMethod = new MethodInfoN(propertyInfo.SetMethod);
                }

                return setMethod;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private MethodInfoN setMethod;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly PropertyInfo propertyInfo;

        internal PropertyInfoN(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        /// <summary>
        /// Returns a literal value associated with the property by a compiler.
        /// </summary>
        /// <returns>
        /// An <see cref="object"/> that contains the literal value associated with the property.
        /// If the literal value is a class type with an element value of zero, the return value is null.
        /// </returns>
        /// <exception cref="FormatException">
        /// The type of the value is not one of the types permitted by the Common Language
        /// Specification (CLS). See the ECMA Partition II specification, Metadata.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The Constant table in unmanaged metadata does not contain a constant value for the current property.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Matching API with .NET class.")]
        public object GetConstantValue()
        {
            return propertyInfo.GetConstantValue();
        }

        /// <summary>
        /// Returns an array of all the index parameters for the property.
        /// </summary>
        /// <returns>
        /// An array of type <see cref="ParameterInfo"/> containing the parameters for the indexes.
        /// If the property is not indexed, the array has 0 (zero) elements.
        /// </returns>
        public ParameterInfo[] GetIndexParameters()
        {
            return propertyInfo.GetIndexParameters();
        }

        /// <summary>
        /// Returns the property value of a specified object.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <returns>The property value of the specified object.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj", Justification = "Matching parameter name with .NET method.")]
        public object GetValue(object obj)
        {
            return propertyInfo.GetValue(GetObject(obj));
        }

        /// <summary>
        /// Returns the property value of a specified object with optional index values for indexed properties.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <returns>The property value of the specified object.</returns>
        /// <exception cref="ArgumentException">
        /// The <paramref name="index"/> array does not contain the type of arguments needed -or- the property's get accessor is not found.
        /// </exception>
        /// <exception cref="Exception">
        /// The object does not match the target type, or a property is an instance property but <paramref name="obj"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="MemberAccessException">
        /// There was an illegal attempt to access a private or protected method inside a class.
        /// </exception>
        /// <exception cref="TargetInvocationException">
        /// An error occurred while retrieving the property value. For example, an index value specified for an indexed property is out of range.
        /// The <see cref="Exception.InnerException"/> property indicates the reason for the error.
        /// </exception>
        /// <exception cref="TargetParameterCountException">
        /// The number of parameters in <paramref name="index"/> does not match the number of parameters the indexed property takes.
        /// </exception>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj", Justification = "Matching parameter name with .NET method.")]
        public object GetValue(object obj, object[] index)
        {
            return propertyInfo.GetValue(GetObject(obj), index);
        }

        /// <summary>
        /// Sets the property value of a specified object.
        /// </summary>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="value">The new property value.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj", Justification = "Matching parameter name with .NET method.")]
        public void SetValue(object obj, object value)
        {
            propertyInfo.SetValue(GetObject(obj), value);
        }

        /// <summary>
        /// Sets the property value of a specified object with optional index values for indexed properties.
        /// </summary>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="value">The new property value.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be <c>null</c> for non-indexed properties.</param>
        /// <exception cref="ArgumentException">
        /// The <paramref name="index"/> array does not contain the type of arguments needed -or- the property's set accessor is not found.
        /// </exception>
        /// <exception cref="Exception">
        /// The object does not match the target type, or a property is an instance property but <paramref name="obj"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="MemberAccessException">
        /// There was an illegal attempt to access a private or protected method inside a class.
        /// </exception>
        /// <exception cref="TargetInvocationException">
        /// An error occurred while setting the property value. For example, an index value specified for an indexed property is out of range.
        /// The <see cref="Exception.InnerException"/> property indicates the reason for the error.
        /// </exception>
        /// <exception cref="TargetParameterCountException">
        /// The number of parameters in <paramref name="index"/> does not match the number of parameters the indexed property takes.
        /// </exception>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj", Justification = "Matching parameter name with .NET method.")]
        public void SetValue(object obj, object value, object[] index)
        {
            propertyInfo.SetValue(GetObject(obj), value, index);
        }
    }
}
