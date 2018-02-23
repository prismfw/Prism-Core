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
using System.Linq;
using System.Runtime.CompilerServices;
using Prism.Native;
using Prism.UI;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.Data
{
    /// <summary>
    /// Provides methods for setting, clearing, and retrieving data bindings.
    /// </summary>
    public static class BindingOperations
    {
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static ConditionalWeakTable<object, List<KeyValuePair<PropertyPath, BindingBase>>> activeBindings = new ConditionalWeakTable<object, List<KeyValuePair<PropertyPath, BindingBase>>>();

        /// <summary>
        /// Deactivates and removes all bindings where the specified object is the target.
        /// </summary>
        /// <param name="target">The object from which to remove all bindings.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <c>null</c>.</exception>
        public static void ClearAllBindings(object target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var agnostic = ObjectRetriever.GetAgnosticObject(target);

            List<KeyValuePair<PropertyPath, BindingBase>> bindings;
            if (activeBindings.TryGetValue(agnostic, out bindings))
            {
                foreach (var binding in bindings)
                {
                    binding.Value.Deactivate();
                }

                activeBindings.Remove(agnostic);
            }
        }

        /// <summary>
        /// Deactivates and removes the binding where the specified object is the target and the specified <see cref="PropertyPath"/> is the target path.
        /// </summary>
        /// <param name="target">The object from which to remove the binding.</param>
        /// <param name="targetPath">The target <see cref="PropertyPath"/> of the binding to be removed.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <c>null</c> -or- when <paramref name="targetPath"/> is <c>null</c>.</exception>
        public static void ClearBinding(object target, PropertyPath targetPath)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (targetPath == null)
            {
                throw new ArgumentNullException(nameof(targetPath));
            }

            var agnostic = ObjectRetriever.GetAgnosticObject(target);

            List<KeyValuePair<PropertyPath, BindingBase>> bindings;
            if (activeBindings.TryGetValue(agnostic, out bindings))
            {
                var binding = bindings.FirstOrDefault(b => b.Key == targetPath);
                if (binding.Value != null)
                {
                    binding.Value.Deactivate();
                    bindings.Remove(binding);
                }

                if (bindings.Count == 0)
                {
                    activeBindings.Remove(agnostic);
                }
            }
        }

        /// <summary>
        /// Deactivates and removes the binding where the specified object is the target and the specified <see cref="PropertyDescriptor"/> describes the target property.
        /// </summary>
        /// <param name="target">The object from which to remove the binding.</param>
        /// <param name="targetProperty">The <see cref="PropertyDescriptor"/> describing the target property of the binding to be removed.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <c>null</c> -or- when <paramref name="targetProperty"/> is <c>null</c>.</exception>
        public static void ClearBinding(object target, PropertyDescriptor targetProperty)
        {
            if (targetProperty == null)
            {
                throw new ArgumentNullException(nameof(targetProperty));
            }

            ClearBinding(target, new PropertyPath(targetProperty.Name));
        }

        /// <summary>
        /// Gets an enumerable of every binding where the specified object is the target.
        /// </summary>
        /// <param name="target">The object that is the target of the bindings to be retrieved.</param>
        /// <returns>An <see cref="IEnumerable&lt;BindingBase&gt;"/> for enumerating through the binding objects, or <c>null</c> if no bindings were found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <c>null</c>.</exception>
        public static IEnumerable<BindingBase> GetAllBindings(object target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var agnostic = ObjectRetriever.GetAgnosticObject(target);

            List<KeyValuePair<PropertyPath, BindingBase>> bindings;
            if (activeBindings.TryGetValue(agnostic, out bindings))
            {
                return bindings.Select(kvp => kvp.Value);
            }

            return null;
        }

        /// <summary>
        /// Gets the binding where the specified object is the target and the specified <see cref="PropertyPath"/> describes the target property.
        /// </summary>
        /// <param name="target">The object that is the target of the binding to be retrieved.</param>
        /// <param name="targetPath">The <see cref="PropertyPath"/> describing the target property of the binding to retrieve.</param>
        /// <returns>The <see cref="BindingBase"/> instance, or <c>null</c> if no instance was found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <c>null</c> -or- when <paramref name="targetPath"/> is <c>null</c>.</exception>
        public static BindingBase GetBinding(object target, PropertyPath targetPath)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (targetPath == null)
            {
                throw new ArgumentNullException(nameof(targetPath));
            }

            var agnostic = ObjectRetriever.GetAgnosticObject(target);

            List<KeyValuePair<PropertyPath, BindingBase>> bindings;
            if (activeBindings.TryGetValue(agnostic, out bindings))
            {
                return bindings.FirstOrDefault(b => b.Key == targetPath).Value;
            }

            return null;
        }

        /// <summary>
        /// Gets the binding where the specified object is the target and the specified <see cref="PropertyDescriptor"/> describes the target property.
        /// </summary>
        /// <param name="target">The object that is the target of the binding to be retrieved.</param>
        /// <param name="targetProperty">The <see cref="PropertyDescriptor"/> describing the target property of the binding to retrieve.</param>
        /// <returns>The <see cref="BindingBase"/> instance, or <c>null</c> if no instance was found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <c>null</c> -or- when <paramref name="targetProperty"/> is <c>null</c>.</exception>
        public static BindingBase GetBinding(object target, PropertyDescriptor targetProperty)
        {
            if (targetProperty == null)
            {
                throw new ArgumentNullException(nameof(targetProperty));
            }

            return GetBinding(target, new PropertyPath(targetProperty.Name));
        }

        /// <summary>
        /// Sets the specified <see cref="BindingBase"/> instance to the specified object.
        /// </summary>
        /// <param name="target">The object to act as the target of the binding.</param>
        /// <param name="targetProperty">The <see cref="PropertyDescriptor"/> describing the target property of the binding.</param>
        /// <param name="binding">The binding to set on the target object.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <c>null</c> -or- when <paramref name="targetProperty"/> is <c>null</c> -or- when <paramref name="binding"/> is <c>null</c>.</exception>
        public static void SetBinding(object target, PropertyDescriptor targetProperty, BindingBase binding)
        {
            if (targetProperty == null)
            {
                throw new ArgumentNullException(nameof(targetProperty));
            }

            SetBinding(target, new PropertyPath(targetProperty.Name), binding);
        }

        /// <summary>
        /// Sets the specified <see cref="BindingBase"/> instance to the specified object.
        /// </summary>
        /// <param name="target">The object to act as the target of the binding.</param>
        /// <param name="targetProperty">The <see cref="PropertyPath"/> describing the target property of the binding.</param>
        /// <param name="binding">The binding to set on the target object.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <c>null</c> -or- when <paramref name="targetProperty"/> is <c>null</c> -or- when <paramref name="binding"/> is <c>null</c>.</exception>
        public static void SetBinding(object target, PropertyPath targetProperty, BindingBase binding)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (targetProperty == null)
            {
                throw new ArgumentNullException(nameof(targetProperty));
            }

            if (binding == null)
            {
                throw new ArgumentNullException(nameof(binding));
            }

            binding.Deactivate();

            var agnostic = ObjectRetriever.GetAgnosticObject(target);
            var bindings = activeBindings.GetOrCreateValue(agnostic);

            int index = bindings.FindIndex(b => b.Key == targetProperty);
            if (index >= 0)
            {
                bindings[index].Value.Deactivate();
                bindings[index] = new KeyValuePair<PropertyPath, BindingBase>(targetProperty, binding);
            }
            else
            {
                bindings.Add(new KeyValuePair<PropertyPath, BindingBase>(targetProperty, binding));
            }

            var visual = agnostic as Visual;
            if (visual == null || visual.IsLoaded)
            {
                binding.Activate(agnostic, targetProperty);
            }
        }

        internal static void ActivateBindings(object target)
        {
            var agnostic = ObjectRetriever.GetAgnosticObject(target);

            List<KeyValuePair<PropertyPath, BindingBase>> bindings;
            if (activeBindings.TryGetValue(agnostic, out bindings))
            {
                foreach (var binding in bindings)
                {
                    binding.Value.Activate(target, binding.Key);
                }
            }
        }
    }
}
