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
using System.Linq;
using System.Reflection;

namespace Prism
{
    internal sealed class ControllerManager : TypeManager
    {
        internal static new ControllerManager Default { get; } = new ControllerManager();

        private ControllerManager()
        {
        }

        internal void Register(TypeInfo info, Type type)
        {
            var navAtts = info.GetCustomAttributes<NavigationAttribute>(false);
            foreach (var att in navAtts)
            {
                if (att.IsSingleton)
                {
                    RegisterSingleton(type, att.UriPattern, type);
                }
                else
                {
                    Register(type, att.UriPattern, type);
                }
            }
        }

        internal IController Resolve(Type resolveType, string[] uriParts, out string uriPattern)
        {
            ITypeRegistrationKey regkey;

            var enumerator = GetEnumerator();
            if (uriParts == null)
            {
                regkey = enumerator.FirstOrDefault(key => key.RegisteredType == resolveType && key.RegisteredName == null);
            }
            else
            {
                regkey = enumerator.FirstOrDefault(key => key.RegisteredName != null && IsMatch(uriParts, key.RegisteredName.Split('/')));
            }

            if (regkey == null)
            {
                uriPattern = null;
                return resolveType == null ? null : Activator.CreateInstance(resolveType) as IController;
            }

            uriPattern = regkey.RegisteredName;
            return Resolve(regkey.RegisteredType, uriPattern) as IController;
        }

        private static bool IsMatch(string[] uriParts, string[] patternParts)
        {
            // the URI is considered a match to the pattern if they have equal parts and non-parameter parts are equal.
            // for example, "MyController/Parameter1" would match "MyController/{MyParameter}".
            if (patternParts.Length != uriParts.Length)
            {
                return false;
            }

            for (int i = 0; i < uriParts.Length; i++)
            {
                string segment = patternParts[i].Trim();
                if (!(segment.Length > 1 && segment[0] == '{' && segment[segment.Length - 1] == '}') && uriParts[i] != segment)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
