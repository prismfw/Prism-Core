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


using System.Collections.Generic;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace System.Reflection
{
	/// <summary>
    /// Obtains information about the attributes of a member and provides access to member metadata.
    /// </summary>
    public abstract class MemberInfoN
    {
        /// <summary>
        /// Gets a collection that contains this member's custom attributes.
        /// </summary>
        public IEnumerable<CustomAttributeData> CustomAttributes
        {
            get { return memberInfo.CustomAttributes; }
        }

        /// <summary>
        /// Gets the class that declares this member.
        /// </summary>
        public Type DeclaringType
        {
            get { return memberInfo.DeclaringType; }
        }

        /// <summary>
        /// Gets the module in which the type that declares the member represented by the current <see cref="MemberInfoN"/> is defined.
        /// </summary>
        public Module Module
        {
            get { return memberInfo.Module; }
        }

        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        public string Name
        {
            get { return memberInfo.Name; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly MemberInfo memberInfo;

        internal MemberInfoN(MemberInfo memberInfo)
        {
            this.memberInfo = memberInfo;
        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance, or <c>null</c>.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> equals the type and value of this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return memberInfo.Equals(obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return memberInfo.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return memberInfo.ToString();
        }

        internal object GetObject(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            obj = ObjectRetriever.GetAgnosticObject(obj);
            if (DeclaringType.GetTypeInfo().IsAssignableFrom(obj.GetType().GetTypeInfo()))
            {
                return obj;
            }
            else
            {
                return ObjectRetriever.GetNativeObject(obj);
            }
        }
    }
}
