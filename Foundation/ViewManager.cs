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
using System.Linq;
using System.Reflection;
using Prism.Systems;

namespace Prism
{
    internal sealed class ViewManager : TypeManager
    {
        internal static new ViewManager Default { get; } = new ViewManager();

        private ViewManager()
        {
        }

        internal void Register(TypeInfo info, Type type)
        {
            var viewAtts = info.GetCustomAttributes<ViewAttribute>(false);
            foreach (var att in viewAtts)
            {
                if (att.ModelType == null)
                {
                    var inter = info.ImplementedInterfaces.FirstOrDefault(i =>
                        i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IView<>)
                    );
                    att.ModelType = inter == null ? typeof(object) : inter.GenericTypeArguments.FirstOrDefault() ?? typeof(object);
                }

                if (att.IsSingleton)
                {
                    RegisterSingleton(new ViewRegistrationKey(att.ModelType, att.Perspective, att.FormFactor), type);
                }
                else
                {
                    Register(new ViewRegistrationKey(att.ModelType, att.Perspective, att.FormFactor),  type);
                }
            }
        }

        protected override TypeRegistrationData GetDataForResolution(Type registerType, string name, bool allowFuzzyNames)
        {
            var enumerator = GetEnumerator();
            var potentials = enumerator.OfType<ViewRegistrationKey>().Where(key => key.RegisteredName == name &&
                        (key.RegisteredType == registerType || key.RegisteredType.GetTypeInfo().IsAssignableFrom(registerType.GetTypeInfo())));
            
            var regkey = GetKey(potentials.Where(key => key.RegisteredType == registerType));
            if (regkey == null)
            {
                var modelType = registerType;
                while (modelType != null)
                {
                    regkey = GetKey(potentials.Where(key => key.RegisteredType == registerType));
                    if (regkey != null)
                    {
                        break;
                    }

                    modelType = modelType.GetTypeInfo().BaseType;
                }
            }
            
            if (regkey == null)
            {
                var interfaces = registerType.GetTypeInfo().ImplementedInterfaces;
                if (interfaces.Any())
                {
                    regkey = GetKey(potentials.Where(key => interfaces.Contains(key.RegisteredType)));
                }
            }

            TypeRegistrationData retval;
            TryGetData(regkey, out retval);
            return retval;
        }

        private static ViewRegistrationKey GetKey(IEnumerable<ViewRegistrationKey> matches)
        {
            return matches.FirstOrDefault(key => key.FormFactor.HasFlag(Device.Current.FormFactor)) ??
                matches.FirstOrDefault(key => key.FormFactor == FormFactor.Unknown) ?? matches.FirstOrDefault();
        }

        private class ViewRegistrationKey : ITypeRegistrationKey
        {
            public string RegisteredName { get; }

            public Type RegisteredType { get; }

            public FormFactor FormFactor { get; }

            private readonly int hash;
            
            public ViewRegistrationKey(Type type, string name, FormFactor formFactor)
            {
                RegisteredType = type;
                RegisteredName = name;
                FormFactor = formFactor;

                hash = RegisteredType == null ? -1 : RegisteredType.GetHashCode() ^ (RegisteredName == null ? 0 : RegisteredName.GetHashCode()) ^ FormFactor.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var viewKey = obj as ViewRegistrationKey;
                if (viewKey == null)
                {
                    return false;
                }

                return viewKey.RegisteredType == RegisteredType && viewKey.RegisteredName == RegisteredName && viewKey.FormFactor == FormFactor;
            }

            public override int GetHashCode()
            {
                return hash;
            }
        }
    }
}
