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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Prism.Resources;

namespace Prism.Utilities
{
    /// <summary>
    /// Represents a utility for converting an object from one type to another.
    /// </summary>
    public static class TypeConverter
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Type[] convertibleTypes = new[]
        {
            typeof(bool),
            typeof(char),
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(DateTime),
            typeof(string),
        };

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Dictionary<ConversionKey, IMethodInvoker> cachedMethods = new Dictionary<ConversionKey, IMethodInvoker>();

        /// <summary>
        /// Converts the specified object to the desired type.
        /// </summary>
        /// <param name="value">The object to be converted.</param>
        /// <param name="conversionType">The type that the object is to be converted to.</param>
        /// <returns>The converted object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="conversionType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the object cannot be converted to the specified type.</exception>
        public static object Convert(object value, Type conversionType)
        {
            if (conversionType == null)
            {
                throw new ArgumentNullException(nameof(conversionType));
            }

            var convertInfo = conversionType.GetTypeInfo();
            if (value == null)
            {
                return convertInfo.IsValueType ? Activator.CreateInstance(conversionType) : null;
            }

            var valueType = value.GetType();
            var valueInfo = valueType.GetTypeInfo();
            if (convertInfo.IsAssignableFrom(valueInfo))
            {
                return value;
            }

            IMethodInvoker invoker;
            if (cachedMethods.TryGetValue(new ConversionKey(conversionType, valueType), out invoker))
            {
                return invoker.Invoke(value);
            }

            if (convertibleTypes.Contains(conversionType))
            {
                return System.Convert.ChangeType(value, conversionType, CultureInfo.CurrentCulture);
            }

            if (convertInfo.IsEnum)
            {
                int index = Array.IndexOf(convertibleTypes, valueType);
                if (index >= 2 && index <= 9)
                {
                    return Enum.ToObject(conversionType, value);
                }
            }

            var method = GetTypeOperator(convertInfo, conversionType, valueType) ?? GetTypeOperator(valueInfo, conversionType, valueType);
            if (method != null)
            {
                invoker = (IMethodInvoker)Activator.CreateInstance(typeof(MethodInvoker<,>).MakeGenericType(valueType, conversionType), method);
            }
            else
            {
                Array valueArray = new[] { value };
                var ctor = convertInfo.DeclaredConstructors.FirstOrDefault(c => CheckConstructorParams(c, valueArray));
                if (ctor != null)
                {
                    invoker = new MethodInvoker<object, object>((o) => { return ctor.Invoke(new[] { o }); });
                }
                else
                {
                    valueArray = value as Array;
                    if (valueArray != null && valueArray.Rank == 1 && (ctor = convertInfo.DeclaredConstructors.FirstOrDefault(c => CheckConstructorParams(c, valueArray))) != null)
                    {
                        invoker = new MethodInvoker<Array, object>((o) =>
                        {
                            var args = new object[o.Length];
                            o.CopyTo(args, 0);
                            return ctor.Invoke(args);
                        });
                    }
                }
            }

            if (invoker != null)
            {
                cachedMethods[new ConversionKey(conversionType, valueType)] = invoker;
                return invoker.Invoke(value);
            }

            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.CannotConvertObjectToType, valueType.FullName, conversionType.FullName));
        }

        /// <summary>
        /// Sets the conversion method that is to be used when converting from one specific type to another.
        /// </summary>
        /// <param name="valueType">The type of the value that is to be converted.</param>
        /// <param name="conversionType">The type that the value is to be converted to.</param>
        /// <param name="method">The method that performs the conversion.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="valueType"/> is <c>null</c> -or- when <paramref name="conversionType"/> is <c>null</c> -or- when <paramref name="method"/> is <c>null</c>.</exception>
        public static void SetConversionMethod(Type valueType, Type conversionType, Func<object, object> method)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException(nameof(valueType));
            }

            if (conversionType == null)
            {
                throw new ArgumentNullException(nameof(conversionType));
            }

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            cachedMethods[new ConversionKey(conversionType, valueType)] = new MethodInvoker<object, object>(method);
        }

        /// <summary>
        /// Sets the conversion method that is to be used when converting from one specific type to another.
        /// </summary>
        /// <typeparam name="TValueType">The type of the value that is to be converted.</typeparam>
        /// <typeparam name="TConversionType">The type that the value is to be converted to.</typeparam>
        /// <param name="method">The method that performs the conversion.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="method"/> is <c>null</c>.</exception>
        public static void SetConversionMethod<TValueType, TConversionType>(Func<TValueType, TConversionType> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            cachedMethods[new ConversionKey(typeof(TConversionType), typeof(TValueType))] = new MethodInvoker<TValueType, TConversionType>(method);
        }

        private static bool CheckConstructorParams(ConstructorInfo ctor, Array array)
        {
            var parameters = ctor.GetParameters();
            if (parameters.Length != array.Length)
            {
                return false;
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                if (!parameters[0].ParameterType.GetTypeInfo().IsAssignableFrom(array.GetValue(i)?.GetType().GetTypeInfo()))
                {
                    return false;
                }
            }

            return true;
        }

        private static MethodInfo GetTypeOperator(TypeInfo typeInfo, Type returnType, Type paramType)
        {
            var implOp = typeInfo.GetDeclaredMethods("op_Implicit").FirstOrDefault(m =>
            {
                if (m.ReturnType != returnType)
                {
                    return false;
                }

                var pars = m.GetParameters();
                return pars.Length == 1 && pars[0].ParameterType == paramType;
            });

            return implOp ?? typeInfo.GetDeclaredMethods("op_Explicit").FirstOrDefault(m =>
            {
                if (m.ReturnType != returnType)
                {
                    return false;
                }

                var parameters = m.GetParameters();
                return parameters.Length == 1 && parameters[0].ParameterType == paramType;
            });
        }

        private struct ConversionKey
        {
            public readonly Type TargetType;
            public readonly Type ValueType;

            public ConversionKey(Type targetType, Type valueType)
            {
                TargetType = targetType;
                ValueType = valueType;
            }

            public override bool Equals(object obj)
            {
                var key = (ConversionKey)obj;
                return key.TargetType == TargetType && key.ValueType == ValueType;
            }

            public override int GetHashCode()
            {
                return TargetType.GetHashCode() ^ ValueType.GetHashCode();
            }
        }

        private interface IMethodInvoker
        {
            object Invoke(object value);
        }

        private class MethodInvoker<S, T> : IMethodInvoker
        {
            private readonly Func<S, T> function;

            public MethodInvoker(Func<S, T> func)
            {
                function = func;
            }

            public MethodInvoker(MethodInfo method)
            {
                function = (Func<S, T>)method.CreateDelegate(typeof(Func<S, T>));
            }

            public object Invoke(object value)
            {
                return function((S)value);
            }
        }
    }
}
