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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Prism.Native;
using Prism.UI;
using Prism.Utilities;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism
{
    /// <summary>
    /// Represents the base class for platform-agnostic framework objects.  Objects that derive from this class can optionally be paired with a
    /// native object to enable information sharing between platform-agnostic code and the native operating system.  This class is abstract.
    /// </summary>
    public abstract class FrameworkObject
    {
        internal event PropertyChangedEventHandler PropertyChanged;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static readonly ConditionalWeakTable<FrameworkObject, Dictionary<PropertyDescriptor, PropertyValueChangedCallback>> changeCallbacks =
            new ConditionalWeakTable<FrameworkObject, Dictionary<PropertyDescriptor, PropertyValueChangedCallback>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkObject"/> class without any native implementation details.
        /// </summary>
        protected FrameworkObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkObject"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when a <see cref="ResolveAttribute"/> is located in the inheritance chain and <paramref name="nativeObject"/> doesn't match the type specified by the attribute.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "'Object' is not meant to signify a type.")]
        protected FrameworkObject(object nativeObject)
        {
            if (nativeObject == null)
            {
                throw new ArgumentNullException(nameof(nativeObject));
            }

            var attr = GetType().GetTypeInfo().GetCustomAttribute<ResolveAttribute>(true);
            if (attr != null)
            {
                var entry = TypeManager.Default.GetDataForResolution(attr.ResolveType, attr.Name, false);
                if (entry != null && entry.ImplementationType != nativeObject.GetType())
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.ObjectTypeDoesNotMatchRequiredType,
                        nativeObject.GetType().FullName, entry.ImplementationType.FullName));
                }
            }

            ObjectRetriever.SetPair(this, nativeObject);

            var notify = nativeObject as INativeObject;
            if (notify != null)
            {
                notify.PropertyChanged += (o, e) => OnPropertyChanged(e.Property);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkObject"/> class and pairs it with a native object that is resolved from the IoC container.
        /// At least one class in the inheritance chain must be decorated with a <see cref="ResolveAttribute"/> or an exception will be thrown.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object could not be resolved from the IoC container.</exception>
        protected FrameworkObject(ResolveParameter[] resolveParameters)
        {
            object[] resolveParams = null;
            if (resolveParameters != null && resolveParameters != ResolveParameter.EmptyParameters)
            {
                resolveParams = new object[resolveParameters.Length];
                for (int i = 0; i < resolveParameters.Length; i++)
                {
                    var param = resolveParameters[i];
                    if (param.ParameterValue == null && !param.AllowNull)
                    {
                        throw new ArgumentNullException(param.ParameterName);
                    }

                    resolveParams[i] = param.ParameterValue;
                }
            }

            var attr = GetType().GetTypeInfo().GetCustomAttribute<ResolveAttribute>(true);
            if (attr == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.CannotLocateResolveAttribute, GetType().FullName));
            }

            var nativeObject = TypeManager.Default.Resolve(attr.ResolveType, attr.Name, resolveParams, TypeResolutionOptions.UseFuzzyParameterResolution);
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TypeCouldNotBeResolved, attr.ResolveType.FullName));
            }

            ObjectRetriever.SetPair(this, nativeObject);

            var notify = nativeObject as INativeObject;
            if (notify != null)
            {
                notify.PropertyChanged += (o, e) => OnPropertyChanged(e.Property);
            }
        }

        /// <summary>
        /// Adds a handler to the event described by the specified <see cref="EventDescriptor"/>.
        /// </summary>
        /// <param name="event">The <see cref="EventDescriptor"/> describing the event to which to add the handler.</param>
        /// <param name="handler">The event handler to add to the event.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="event"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="event"/> does not describe a valid event on this instance.</exception>
        public void AddHandler(EventDescriptor @event, Delegate handler)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (!@event.OwnerType.GetTypeInfo().IsAssignableFrom(GetType().GetTypeInfo()))
            {
                throw new ArgumentException(Resources.Strings.OwnerTypeDoesNotMatchCurrentType, nameof(@event));
            }

            @event.AddHandler(this, handler);
        }

        /// <summary>
        /// Adds a handler to the event described by the specified <see cref="EventDescriptor"/>.
        /// </summary>
        /// <param name="event">The <see cref="EventDescriptor"/> describing the event to which to add the handler.</param>
        /// <param name="handler">The event handler to add to the event.</param>
        /// <param name="platforms">The platforms on which the handler should be added.  Platforms that are not specified will not attempt to add the handler.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="event"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="event"/> does not describe a valid property on this instance.</exception>
        public void AddHandler(EventDescriptor @event, Delegate handler, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) != 0)
            {
                AddHandler(@event, handler);
            }
        }

        /// <summary>
        /// Registers a handler to be called when the property described by the specified <see cref="PropertyDescriptor"/> is changed.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> describing the property for which to register the callback.</param>
        /// <param name="callback">The handler to be called when the value of the property is changed.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is <c>null</c> -or- when <paramref name="callback"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="property"/> does not describe a valid property on this instance.</exception>
        public void RegisterPropertyChangedCallback(PropertyDescriptor property, PropertyValueChangedCallback callback)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            if (!property.OwnerType.GetTypeInfo().IsAssignableFrom(GetType().GetTypeInfo()))
            {
                throw new ArgumentException(Resources.Strings.OwnerTypeDoesNotMatchCurrentType, nameof(property));
            }

            PropertyValueChangedCallback pvc;
            var currentCallbacks = changeCallbacks.GetOrCreateValue(this);
            if (currentCallbacks.TryGetValue(property, out pvc))
            {
                callback = pvc + callback;
            }
            currentCallbacks[property] = callback;
        }

        /// <summary>
        /// Removes a handler from the event described by the specified <see cref="EventDescriptor"/>.
        /// </summary>
        /// <param name="event">The <see cref="EventDescriptor"/> describing the event from which to remove the handler.</param>
        /// <param name="handler">The event handler to remove from the event.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="event"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="event"/> does not describe a valid event on this instance.</exception>
        public void RemoveHandler(EventDescriptor @event, Delegate handler)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (!@event.OwnerType.GetTypeInfo().IsAssignableFrom(GetType().GetTypeInfo()))
            {
                throw new ArgumentException(Resources.Strings.OwnerTypeDoesNotMatchCurrentType, nameof(@event));
            }

            @event.RemoveHandler(this, handler);
        }

        /// <summary>
        /// Removes a handler from the event described by the specified <see cref="EventDescriptor"/>.
        /// </summary>
        /// <param name="event">The <see cref="EventDescriptor"/> describing the event from which to remove the handler.</param>
        /// <param name="handler">The event handler to remove from the event.</param>
        /// <param name="platforms">The platforms on which the handler should be removed.  Platforms that are not specified will not attempt to remove the handler.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="event"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="event"/> does not describe a valid event on this instance.</exception>
        public void RemoveHandler(EventDescriptor @event, Delegate handler, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) != 0)
            {
                RemoveHandler(@event, handler);
            }
        }

        /// <summary>
        /// Sets the value of the property described by the specified <see cref="PropertyDescriptor"/> to the specified value.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> describing the property whose value is to be set.</param>
        /// <param name="value">The value to set the property to.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="property"/> does not describe a valid property on this instance.</exception>
        public void SetValue(PropertyDescriptor property, object value)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!property.OwnerType.GetTypeInfo().IsAssignableFrom(GetType().GetTypeInfo()))
            {
                throw new ArgumentException(Resources.Strings.OwnerTypeDoesNotMatchCurrentType, nameof(property));
            }

            property.SetValue(this, value, null);
        }

        /// <summary>
        /// Sets the value of the property described by the specified <see cref="PropertyDescriptor"/> to the specified value.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> describing the property whose value is to be set.</param>
        /// <param name="value">The value to set the property to.</param>
        /// <param name="platforms">The platforms on which the property should be set.  Platforms that are not specified will not attempt to set the property.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="property"/> does not describe a valid property on this instance.</exception>
        public void SetValue(PropertyDescriptor property, object value, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) != 0)
            {
                SetValue(property, value);
            }
        }

        /// <summary>
        /// Sets the value of the property described by the specified <see cref="PropertyDescriptor"/> to the specified value.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> describing the property whose value is to be set.</param>
        /// <param name="value">The value to set the property to when running on one of the specified platforms.</param>
        /// <param name="altValue">The value to set the property to when not running on one of the specified platforms.</param>
        /// <param name="platforms">The platforms on which to set the property to the specified <paramref name="value"/>.  Platforms that are not specified will set the property to the specified <paramref name="altValue"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="property"/> does not describe a valid property on this instance.</exception>
        public void SetValue(PropertyDescriptor property, object value, object altValue, PlatformMask platforms)
        {
            SetValue(property, ((int)platforms & (int)Application.Current.Platform) != 0 ? value : altValue);
        }

        /// <summary>
        /// Sets the value of the property described by the specified <see cref="PropertyPath"/> to the specified value.
        /// </summary>
        /// <param name="propertyPath">The <see cref="PropertyPath"/> describing the property whose value is to be set.</param>
        /// <param name="value">The value to set the property to.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyPath"/> is <c>null</c>.</exception>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exact error types can be unpredictable but should not interfere with execution of the program.  The error is logged to facilitate debugging.")]
        public void SetValue(PropertyPath propertyPath, object value)
        {
            if (propertyPath == null)
            {
                throw new ArgumentNullException(nameof(propertyPath));
            }

            WeakReference[] refs;
            PropertyDescriptor[] descriptors;

            try
            {
                propertyPath.ResolvePath(this, out refs, out descriptors, false);

                var finalDescriptor = descriptors.Last();
                finalDescriptor.SetValue(refs.Last().Target, TypeConverter.Convert(value, finalDescriptor.PropertyType), propertyPath.GetIndexValues(descriptors.Length - 1));
            }
            catch (Exception e)
            {
                Logger.Error(CultureInfo.CurrentCulture, Resources.Strings.FailedToSetValueOnTargetProperty, value, propertyPath.Path, e);
            }
        }

        /// <summary>
        /// Sets the value of the property described by the specified <see cref="PropertyPath"/> to the specified value.
        /// </summary>
        /// <param name="propertyPath">The <see cref="PropertyPath"/> describing the property whose value is to be set.</param>
        /// <param name="value">The value to set the property to.</param>
        /// <param name="platforms">The platforms on which the property should be set.  Platforms that are not specified will not attempt to set the property.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyPath"/> is <c>null</c>.</exception>
        public void SetValue(PropertyPath propertyPath, object value, PlatformMask platforms)
        {
            if (((int)platforms & (int)Application.Current.Platform) != 0)
            {
                SetValue(propertyPath, value);
            }
        }

        /// <summary>
        /// Sets the value of the property described by the specified <see cref="PropertyPath"/> to the specified value.
        /// </summary>
        /// <param name="propertyPath">The <see cref="PropertyPath"/> describing the property whose value is to be set.</param>
        /// <param name="value">The value to set the property to when running on one of the specified platforms.</param>
        /// <param name="altValue">The value to set the property to when not running on one of the specified platforms.</param>
        /// <param name="platforms">The platforms on which to set the property to the specified <paramref name="value"/>.  Platforms that are not specified will set the property to the specified <paramref name="altValue"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyPath"/> is <c>null</c>.</exception>
        public void SetValue(PropertyPath propertyPath, object value, object altValue, PlatformMask platforms)
        {
            SetValue(propertyPath, ((int)platforms & (int)Application.Current.Platform) != 0 ? value : altValue);
        }

        /// <summary>
        /// Unregisters a previously registered callback from the property described by the specified <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> describing the property from which to unregister the callback.</param>
        /// <param name="callback">The handler to be unregistered.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="property"/> is <c>null</c> -or- when <paramref name="callback"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="property"/> does not describe a valid property on this instance.</exception>
        public void UnregisterPropertyChangedCallback(PropertyDescriptor property, PropertyValueChangedCallback callback)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            if (!property.OwnerType.GetTypeInfo().IsAssignableFrom(GetType().GetTypeInfo()))
            {
                throw new ArgumentException(Resources.Strings.OwnerTypeDoesNotMatchCurrentType, nameof(property));
            }

            PropertyValueChangedCallback pvc;
            Dictionary<PropertyDescriptor, PropertyValueChangedCallback> currentCallbacks;
            if (changeCallbacks.TryGetValue(this, out currentCallbacks) && currentCallbacks.TryGetValue(property, out pvc))
            {
                pvc -= callback;
                if (pvc == null)
                {
                    currentCallbacks.Remove(property);
                }
                else
                {
                    currentCallbacks[property] = pvc;
                }
            }
        }

        /// <summary>
        /// Called when a property value is changed.  Derived classes should invoke this method when their own property values are changed.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> describing the property whose value has changed.</param>
        protected void OnPropertyChanged(PropertyDescriptor property)
        {
            if (property == null)
            {
                return;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property.Name));

            var metadata = property.GetMetadata(GetType()) as FrameworkPropertyMetadata;
            metadata?.ValueChangedCallback?.Invoke(this, property);

            PropertyValueChangedCallback pvc;
            Dictionary<PropertyDescriptor, PropertyValueChangedCallback> currentCallbacks;
            if (changeCallbacks.TryGetValue(this, out currentCallbacks) && currentCallbacks.TryGetValue(property, out pvc))
            {
                pvc?.Invoke(this, property);
            }

            if (metadata != null)
            {
                var visual = this as Visual;
                if (visual != null)
                {
                    if (metadata.AffectsParentMeasure)
                    {
                        (visual.Parent as Visual)?.InvalidateMeasure();
                    }
                    else if (metadata.AffectsMeasure)
                    {
                        visual.InvalidateMeasure();
                    }

                    if (metadata.AffectsParentArrange)
                    {
                        (visual.Parent as Visual)?.InvalidateArrange();
                    }
                    else if (metadata.AffectsArrange)
                    {
                        visual.InvalidateArrange();
                    }
                }
            }
        }
    }
}
