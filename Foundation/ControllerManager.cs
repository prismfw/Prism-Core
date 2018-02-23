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
            var navAtts = info.GetCustomAttributes<NavigationControllerAttribute>(false);
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
                regkey = null;

                bool exactMatchFound = false;
                var literalMatches = new bool[uriParts.Length];
                foreach (var key in enumerator)
                {
                    // The URI is considered a match to the pattern if they have equal parts and non-parameter parts are equal.
                    // For example, "MyController/Parameter1" would match "MyController/{MyParameter}".
                    string[] patternParts = key.RegisteredName?.Split('/');
                    if (patternParts?.Length != uriParts.Length)
                    {
                        continue;
                    }

                    bool hasCloserLiteral = false;
                    for (int i = 0; i < uriParts.Length; i++)
                    {
                        string uriPart = uriParts[i];
                        string patternPart = patternParts[i];

                        // Literal matches have priority over parameter matches.
                        if (uriPart == patternPart)
                        {
                            hasCloserLiteral = hasCloserLiteral || !literalMatches[i];
                        }
                        else if ((!hasCloserLiteral && literalMatches[i]) || !IsParameter(patternPart.Trim()))
                        {
                            break;
                        }

                        if (i == uriParts.Length - 1)
                        {
                            regkey = key;

                            exactMatchFound = true;
                            for (int j = 0; j < uriParts.Length; j++)
                            {
                                bool isMatch = uriParts[j] == patternParts[j];
                                literalMatches[j] = isMatch;
                                exactMatchFound = exactMatchFound && isMatch;
                            }
                        }
                    }

                    // If each segment is a literal match, then we can guarantee that this
                    // key is the closest match and there's no need to continue searching.
                    if (exactMatchFound)
                    {
                        break;
                    }
                }
            }

            if (regkey == null)
            {
                uriPattern = null;
                return resolveType == null ? null : Activator.CreateInstance(resolveType) as IController;
            }

            uriPattern = regkey.RegisteredName;
            return Resolve(regkey.RegisteredType, uriPattern) as IController;
        }

        private static bool IsParameter(string segment)
        {
            return segment.Length > 1 && segment[0] == '{' && segment[segment.Length - 1] == '}';
        }
    }
}
