/*
Copyright (C) 2017  Prism Framework Team

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


using System.Diagnostics.CodeAnalysis;

#if !DEBUG
using System.Diagnostics;
#endif

namespace System.Reflection
{
    /// <summary>
    /// Discovers the attributes of a field and provides access to field metadata.
    /// </summary>
    public sealed class FieldInfoN : MemberInfoN
    {
        /// <summary>
        /// Gets the attributes associated with this field.
        /// </summary>
        public FieldAttributes Attributes
        {
            get { return fieldInfo.Attributes; }
        }

        /// <summary>
        /// Gets the type of this field object.
        /// </summary>
        public Type FieldType
        {
            get { return fieldInfo.FieldType; }
        }

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this field is described by <see cref="FieldAttributes.Assembly"/>;
        /// that is, the field is visible at most to other types in the same assembly, and is not visible to derived types
        /// outside the assembly.
        /// </summary>
        public bool IsAssembly
        {
            get { return fieldInfo.IsAssembly; }
        }

        /// <summary>
        /// Gets a value indicating whether the visibility of this field is described by <see cref="FieldAttributes.Family"/>;
        /// that is, the field is visible only within its class and derived classes.
        /// </summary>
        public bool IsFamily
        {
            get { return fieldInfo.IsFamily; }
        }

        /// <summary>
        /// Gets a value indicating whether the visibility of this field is described by <see cref="FieldAttributes.FamANDAssem"/>;
        /// that is, the field can be accessed from derived classes, but only if they are in the same assembly.
        /// </summary>
        public bool IsFamilyAndAssembly
        {
            get { return fieldInfo.IsFamilyAndAssembly; }
        }

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this field is described by <see cref="FieldAttributes.FamORAssem"/>;
        /// that is, the field can be accessed by derived classes wherever they are, and by classes in the same assembly.
        /// </summary>
        public bool IsFamilyOrAssembly
        {
            get { return fieldInfo.IsFamilyOrAssembly; }
        }

        /// <summary>
        /// Gets a value indicating whether the field can only be set in the body of the constructor.
        /// </summary>
        public bool IsInitOnly
        {
            get { return fieldInfo.IsInitOnly; }
        }

        /// <summary>
        /// Gets a value indicating whether the value is written at compile time and cannot be changed.
        /// </summary>
        public bool IsLiteral
        {
            get { return fieldInfo.IsLiteral; }
        }

        /// <summary>
        /// Gets a value indicating whether the field is private.
        /// </summary>
        public bool IsPrivate
        {
            get { return fieldInfo.IsPrivate; }
        }

        /// <summary>
        /// Gets a value indicating whether the field is public.
        /// </summary>
        public bool IsPublic
        {
            get { return fieldInfo.IsPublic; }
        }

        /// <summary>
        /// Gets a value indicating whether the corresponding SpecialName attribute is set in the <see cref="FieldAttributes"/> enumerator.
        /// </summary>
        public bool IsSpecialName
        {
            get { return fieldInfo.IsSpecialName; }
        }

        /// <summary>
        /// Gets a value indicating whether the field is static.
        /// </summary>
        public bool IsStatic
        {
            get { return fieldInfo.IsStatic; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly FieldInfo fieldInfo;

        internal FieldInfoN(FieldInfo fieldInfo)
            : base(fieldInfo)
        {
            this.fieldInfo = fieldInfo;
        }

        /// <summary>
        /// Returns the value of a field supported by a given object.
        /// </summary>
        /// <param name="obj">The object whose field value will be returned.</param>
        /// <returns>An object containing the value of the field reflected by this instance.</returns>
        /// <exception cref="ArgumentException">The method is neither declared nor inherited by the class of <paramref name="obj"/>.</exception>
        /// <exception cref="Exception">The field is non-static and <paramref name="obj"/> is <c>null</c>.</exception>
        /// <exception cref="MemberAccessException">The caller does not have permission to access this field.</exception>
        /// <exception cref="NotSupportedException">A field is marked literal, but the field does not have one of the accepted literal types.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj", Justification = "Matching parameter name with .NET method.")]
        public object GetValue(object obj)
        {
            return fieldInfo.GetValue(GetObject(obj));
        }

        /// <summary>
        /// Sets the value of the field supported by the given object.
        /// </summary>
        /// <param name="obj">The object whose field value will be set.</param>
        /// <param name="value">The value to assign to the field.</param>
        /// <exception cref="ArgumentException">The field does not exist on the object -or- the <paramref name="value"/> parameter cannot be converted and stored in the field.</exception>
        /// <exception cref="Exception">The <paramref name="obj"/> parameter is <c>null</c> and the field is an instance field.</exception>
        /// <exception cref="MemberAccessException">The caller does not have permission to access this field.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj", Justification = "Matching parameter name with .NET method.")]
        public void SetValue(object obj, object value)
        {
            fieldInfo.SetValue(GetObject(obj), value);
        }
    }
}
