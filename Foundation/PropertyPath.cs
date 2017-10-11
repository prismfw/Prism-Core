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


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism
{
    /// <summary>
    /// Represents a tokenized path that describes a property below another property or below an owning type.
    /// </summary>
    public sealed class PropertyPath
    {
        /// <summary>
        /// Gets a tokenized string that represents the path to the property.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets a collection of objects to be used when the path refers to indexed parameters.
        /// </summary>
        public ReadOnlyCollection<object> PathParameters { get; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly object[][] pathIndices;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly string[] pathTokens;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyPath"/> class.
        /// </summary>
        /// <param name="path">A tokenized string that specifies the path to the property.</param>
        /// <param name="pathParameters">An optional collection of objects to use when the path refers to indexed parameters.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is <c>null</c>, an empty string, or a whitespace-only string.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="path"/> contains an argument that exceeds the upper or lower bounds of the path parameters.</exception>
        /// <exception cref="FormatException">Thrown when <paramref name="path"/> contains a substring with an invalid format.</exception>
        public PropertyPath(string path, params object[] pathParameters)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(Resources.Strings.ValueCannotBeNullEmptyOrWhitespace, nameof(path));
            }

            Path = path;
            if (pathParameters != null)
            {
                var paramArray = new object[pathParameters.Length];
                pathParameters.CopyTo(paramArray, 0);
                PathParameters = new ReadOnlyCollection<object>(paramArray);
            }

            var tokenIndices = new List<int>() { 0 };
            for (int i = 0; i < Path.Length; i++)
            {
                char c = Path[i];
                if (c == '.' || c == '[')
                {
                    tokenIndices.Add(i);
                }
            }

            pathTokens = new string[tokenIndices.Count];
            for (int i = 0; i < tokenIndices.Count; i++)
            {
                int startIndex = tokenIndices[i];
                if (Path[startIndex] == '.')
                {
                    startIndex++;
                }

                int endIndex = tokenIndices.Count - 1 > i ? tokenIndices[i + 1] : Path.Length;
                pathTokens[i] = Path.Substring(startIndex, endIndex - startIndex);
            }

            pathIndices = new object[pathTokens.Length][];
            for (int i = 0; i < pathTokens.Length; i++)
            {
                string token = pathTokens[i];
                if (token[0] == '[')
                {
                    string[] values = token.Substring(1, token.Length - 2).Split(',');
                    object[] indices = new object[values.Length];

                    for (int j = 0; j < values.Length; j++)
                    {
                        string name = values[j].Trim();
                        if (name.StartsWith("{", StringComparison.Ordinal))
                        {
                            if (name.EndsWith("}", StringComparison.Ordinal))
                            {
                                string value = name.Substring(1, name.Length - 2);
                                int index;
                                if (int.TryParse(value, out index))
                                {
                                    if (index < 0 || PathParameters == null || index >= PathParameters.Count)
                                    {
                                        throw new ArgumentOutOfRangeException(nameof(path), Resources.Strings.PropertyPathArgumentExceedsBounds);
                                    }

                                    indices[j] = PathParameters[index];
                                    continue;
                                }
                            }
                            else
                            {
                                throw new FormatException(Resources.Strings.PropertyPathContainsUnmatchedOpeningBracket);
                            }
                        }
                        else if (name.EndsWith("}", StringComparison.Ordinal))
                        {
                            throw new FormatException(Resources.Strings.PropertyPathContainsUnmatchedClosingBracket);
                        }

                        if (name.StartsWith("'", StringComparison.Ordinal) && name.EndsWith("'", StringComparison.Ordinal))
                        {
                            name = name.Substring(1, name.Length - 2);
                        }

                        int integer;
                        if (int.TryParse(name, out integer))
                        {
                            indices[j] = integer;
                        }
                        else
                        {
                            indices[j] = name;
                        }
                    }

                    pathIndices[i] = indices;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="PropertyPath"/> is equal to the current <see cref="PropertyPath"/>.
        /// </summary>
        /// <param name="other">The <see cref="PropertyPath"/> to compare with the current <see cref="PropertyPath"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="PropertyPath"/> is equal to the current <see cref="PropertyPath"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(PropertyPath other)
        {
            if (other == null || Path != other.Path)
            {
                return false;
            }

            if (PathParameters == null || PathParameters.Count == 0)
            {
                return other.PathParameters == null || other.PathParameters.Count == 0;
            }
            
            if (other.PathParameters == null || PathParameters.Count != other.PathParameters.Count)
            {
                return false;
            }

            for (int i = 0; i < PathParameters.Count; i++)
            {
                if (PathParameters[i] != other.PathParameters[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="PropertyPath"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="PropertyPath"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current <see cref="PropertyPath"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var pp = obj as PropertyPath;
            return pp == null ? false : Equals(pp);
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="PropertyPath"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            int hash = Path.GetHashCode();
            if (PathParameters != null)
            {
                foreach (var parameter in PathParameters)
                {
                    hash ^= parameter.GetHashCode();
                }
            }

            return hash;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="PropertyPath"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="PropertyPath"/>.</returns>
        public override string ToString()
        {
            return Path;
        }

        /// <summary>
        /// Combines the specified <see cref="PropertyPath"/> objects into a single <see cref="PropertyPath"/> instance.
        /// </summary>
        /// <param name="propertyPaths">The <see cref="PropertyPath"/> objects to combine into a single instance.</param>
        /// <returns>
        /// A <see cref="PropertyPath"/> instance with its <see cref="P:Path"/> and <see cref="P:PathParameters"/>
        /// set to a combination of the provided property path objects.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyPaths"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="propertyPaths"/> is an empty collection.</exception>
        public static PropertyPath Combine(params PropertyPath[] propertyPaths)
        {
            if (propertyPaths == null)
            {
                throw new ArgumentNullException(nameof(propertyPaths));
            }

            if (propertyPaths.Length == 0)
            {
                throw new ArgumentException(Resources.Strings.CannotCombineZeroPaths);
            }

            var property = propertyPaths[0];
            string path = property.Path;
            List<object> parameters = property.PathParameters == null ? new List<object>() : new List<object>(property.PathParameters);

            for (int i = 1; i < propertyPaths.Length; i++)
            {
                property = propertyPaths[i];

                string subPath = property.Path;
                for (int j = subPath.Length - 1; j > 0; j--)
                {
                    if (subPath[j] == '}')
                    {
                        int opener = subPath.LastIndexOf('{', j);
                        if (opener > 0 && (j == subPath.Length - 1 || subPath[j + 1] != '\'' || subPath[opener - 1] != '\''))
                        {
                            int index;
                            if (int.TryParse(subPath.Substring(++opener, j - opener), out index))
                            {
                                index += parameters.Count;
                                subPath = subPath.Remove(opener, j - opener).Insert(opener, index.ToString(CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }

                path += "." + subPath;
                if (property.PathParameters != null)
                {
                    parameters.AddRange(property.PathParameters);
                }
            }

            return new PropertyPath(path, parameters.ToArray());
        }

        /// <summary>
        /// Determines whether two <see cref="PropertyPath"/> objects are considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal; otherwise <c>false</c>.</returns>
        public static bool operator ==(PropertyPath value1, PropertyPath value2)
        {
            return ReferenceEquals(value1, null) ? ReferenceEquals(value2, null) : value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether two <see cref="PropertyPath"/> objects are not considered equal.
        /// </summary>
        /// <param name="value1">The first object to compare.</param>
        /// <param name="value2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are not considered equal; otherwise <c>false</c>.</returns>
        public static bool operator !=(PropertyPath value1, PropertyPath value2)
        {
            return ReferenceEquals(value1, null) ? !ReferenceEquals(value2, null) : !value1.Equals(value2);
        }

        internal object[] GetIndexValues(int pathTokenIndex)
        {
            return pathTokenIndex >= 0 && pathTokenIndex < pathIndices.Length ? pathIndices[pathTokenIndex] : null;
        }
        
        internal void ResolvePath(object sourceObj, out WeakReference[] objects, out PropertyDescriptor[] descriptors, bool cacheGetSetMethods)
        {
            objects = new WeakReference[pathTokens.Length];
            descriptors = new PropertyDescriptor[pathTokens.Length];

            for (int i = 0; i < pathTokens.Length; i++)
            {
                var name = pathTokens[i];
                var type = sourceObj.GetType();
                if (type.IsArray && name[0] == '[')
                {
                    // Array indexers need to be handled specially since they cannot be accessed through property info.
                    objects[i] = new WeakReference(sourceObj);
                    descriptors[i] = new PropertyDescriptor(type, type.GetElementType());

                    sourceObj = ((Array)sourceObj).GetValue(pathIndices[i].Cast<int>().ToArray());
                    continue;
                }

                var info = GetProperty(type, name, pathIndices[i]);
                if (info == null)
                {
                    sourceObj = (sourceObj is FrameworkObject ? ObjectRetriever.GetNativeObject(sourceObj) :
                        ObjectRetriever.GetAgnosticObject(sourceObj));

                    var sourceType = sourceObj.GetType();
                    if (sourceType != type)
                    {
                        type = sourceType;
                        info = GetProperty(type, name, pathIndices[i]);
                    }

                    if (info == null)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.CannotResolvePropertyPathForObject, Path), nameof(sourceObj));
                    }
                }

                objects[i] = new WeakReference(sourceObj);

                try
                {
                    sourceObj = info.GetValue(sourceObj, pathIndices[i]);
                }
                catch (Exception e)
                {
                    if (pathIndices != null)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.CannotResolveIndexerForPathToken, pathTokens[i]), e);
                    }
                    else
                    {
                        throw;
                    }
                }

               descriptors[i] = info.DeclaringType.GetRuntimeProperties()
                    .Where(p => p.GetMethod != null && p.GetMethod.IsStatic && p.PropertyType == typeof(PropertyDescriptor))
                    .Select(p => p.GetValue(null) as PropertyDescriptor)
                    .FirstOrDefault(pd => pd.OwnerType == info.DeclaringType && pd.PropertyType == info.PropertyType && pd.Name == info.Name)
                    ?? new PropertyDescriptor(info, cacheGetSetMethods);
            }
        }

        private static PropertyInfo GetProperty(Type type, string name, object[] indices)
        {
            PropertyInfo retval = null;
            foreach (var prop in type.GetRuntimeProperties())
            {
                if (name == prop.Name || (name[0] == '[' && prop.Name == "Item" && IsMatch(prop.GetIndexParameters(), indices)))
                {
                    if (prop.DeclaringType == type)
                    {
                        return prop;
                    }

                    if (retval == null || prop.DeclaringType.GetTypeInfo().IsSubclassOf(retval.DeclaringType))
                    {
                        retval = prop;
                    }
                }
            }

            return retval;
        }

        private static bool IsMatch(ParameterInfo[] indexParams, object[] indices)
        {
            if (indexParams.Length != indices?.Length)
            {
                return false;
            }

            for (int i = 0; i < indexParams.Length; i++)
            {
                if (!indexParams[i].ParameterType.GetTypeInfo().IsAssignableFrom(indices[i].GetType().GetTypeInfo()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
