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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using Prism.Native;
using Prism.Utilities;

namespace Prism.Data
{
    /// <summary>
    /// Represents a connection between the property of a target object and the properties of multiple source objects,
    /// allowing their values to be shared as they're updated.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi", Justification = "Valid prefix meaning 'multiple'.")]
    public class MultiBinding : BindingBase
    {
        /// <summary>
        /// Occurs when an error is encountered while resolving a path or updating a value within a data binding.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public static event TypedEventHandler<MultiBinding, MultiBindingFailedEventArgs> BindingFailed;

        /// <summary>
        /// Gets a collection of bindings that specify the source objects and properties.
        /// </summary>
        public Collection<Binding> Bindings { get; }

        /// <summary>
        /// Gets or sets the converter to be used when passing values between the source properties and the target property.
        /// </summary>
        public IMultiValueConverter Converter { get; set; }

        /// <summary>
        /// Gets or sets the culture to pass to the <see cref="P:Converter"/>.
        /// If <c>null</c>, the current culture will be passed to the converter instead.
        /// </summary>
        public CultureInfo ConverterCulture { get; set; }

        /// <summary>
        /// Gets or sets an optional parameter to pass to the <see cref="P:Converter"/>.
        /// </summary>
        public object ConverterParameter { get; set; }

        /// <summary>
        /// Gets or sets the direction in which property values are passed.
        /// </summary>
        public BindingMode Mode
        {
            get { return mode; }
            set
            {
                if (value != mode)
                {
                    mode = value;
                    if (Status == BindingStatus.Active)
                    {
                        if (Bindings.Any(b => GetBindingMode(b) != BindingMode.OneWayToSource))
                        {
                            OnSourceValueUpdated(null, new HandledEventArgs());
                        }

                        if (Bindings.Any(b => GetBindingMode(b) == BindingMode.OneWayToSource))
                        {
                            OnTargetPropertyChanged(targetObjects.Last().Target, new PropertyChangedEventArgs(targetDescriptors.Last().Name));
                        }
                    }
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BindingMode mode;

        /// <summary>
        /// Gets the current status of the binding.
        /// </summary>
        public BindingStatus Status { get; private set; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private bool isActivating;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly SynchronizationContext syncContext = SynchronizationContext.Current;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private PropertyDescriptor[] targetDescriptors;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private WeakReference[] targetObjects;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private PropertyPath targetPropertyPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiBinding"/> class.
        /// </summary>
        public MultiBinding()
        {
            Bindings = new ObservableCollection<Binding>();
            ((ObservableCollection<Binding>)Bindings).CollectionChanged += (o, e) =>
            {
                if (Status == BindingStatus.Inactive || Status == BindingStatus.TargetPathError)
                {
                    return;
                }

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        isActivating = true;
                        for (int i = 0; i < e.NewItems.Count; i++)
                        {
                            var binding = e.NewItems[i] as Binding;
                            if (binding != null)
                            {
                                ActivateBinding(binding);
                            }
                        }
                        isActivating = false;

                        if (Bindings.Any(b => GetBindingMode(b) != BindingMode.OneWayToSource))
                        {
                            OnSourceValueUpdated(null, new HandledEventArgs());
                        }

                        if (Bindings.Any(b => GetBindingMode(b) == BindingMode.OneWayToSource))
                        {
                            OnTargetPropertyChanged(targetObjects.Last().Target, new PropertyChangedEventArgs(targetDescriptors.Last().Name));
                        }
                        return;
                    case NotifyCollectionChangedAction.Replace:
                        for (int i = 0; i < e.OldItems.Count; i++)
                        {
                            var binding = e.OldItems[i] as Binding;
                            if (binding != null)
                            {
                                binding.SourceValueUpdated -= OnSourceValueUpdated;
                                binding.TargetValueUpdated -= OnTargetValueChanged;
                                binding.Deactivate();
                            }
                        }

                        if (e.NewItems.Count > 0)
                        {
                            isActivating = true;
                            for (int i = 0; i < e.NewItems.Count; i++)
                            {
                                var binding = e.NewItems[i] as Binding;
                                if (binding != null)
                                {
                                    ActivateBinding(binding);
                                }
                            }
                            isActivating = false;

                            if (Bindings.Any(b => GetBindingMode(b) != BindingMode.OneWayToSource))
                            {
                                OnSourceValueUpdated(null, new HandledEventArgs());
                            }

                            if (Bindings.Any(b => GetBindingMode(b) == BindingMode.OneWayToSource))
                            {
                                OnTargetPropertyChanged(targetObjects.Last().Target, new PropertyChangedEventArgs(targetDescriptors.Last().Name));
                            }
                        }
                        return;
                    case NotifyCollectionChangedAction.Remove:
                    case NotifyCollectionChangedAction.Reset:
                        for (int i = 0; i < e.OldItems.Count; i++)
                        {
                            var binding = e.OldItems[i] as Binding;
                            if (binding != null)
                            {
                                binding.SourceValueUpdated -= OnSourceValueUpdated;
                                binding.TargetValueUpdated -= OnTargetValueChanged;
                                binding.Deactivate();
                            }
                        }
                        return;
                }
            };
        }

        /// <summary>
        /// Activates the binding.
        /// </summary>
        /// <param name="targetObject">The target object of the binding.</param>
        /// <param name="targetProperty">The <see cref="PropertyPath"/> describing the target property of the binding.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exact error types can be unpredictable but should not interfere with execution of the program.  Status property and BindingFailed event can be used for debugging purposes.")]
        internal sealed override void Activate(object targetObject, PropertyPath targetProperty)
        {
            Deactivate();
            Status = BindingStatus.Active;

            if (targetObject == null)
            {
                Status = BindingStatus.TargetPathError;

                var ex = new ArgumentNullException(nameof(targetObject));
                Logger.Error(CultureInfo.CurrentCulture, Resources.Strings.DataBindingError, ex);
                BindingFailed?.Invoke(this, new MultiBindingFailedEventArgs(targetObject, targetProperty, null, ex));
                return;
            }

            targetPropertyPath = targetProperty;
            try
            {
                targetProperty.ResolvePath(targetObject, out targetObjects, out targetDescriptors);
            }
            catch (Exception ex)
            {
                Status = BindingStatus.TargetPathError;

                Logger.Error(CultureInfo.CurrentCulture, Resources.Strings.DataBindingError, ex);
                BindingFailed?.Invoke(this, new MultiBindingFailedEventArgs(targetObject, targetProperty, null, ex));
                UnregisterTargetListeners();
                return;
            }

            try
            {
                for (int i = 0; i < targetObjects.Length; i++)
                {
                    targetObject = ObjectRetriever.GetAgnosticObject(targetObjects[i].Target);
                    var fwObject = targetObject as FrameworkObject;
                    if (fwObject != null)
                    {
                        fwObject.PropertyChanged -= OnTargetPropertyChanged;
                        fwObject.PropertyChanged += OnTargetPropertyChanged;
                    }
                    else
                    {
                        var notifier = targetObject as INotifyPropertyChanged ?? ObjectRetriever.GetNativeObject(targetObject) as INotifyPropertyChanged;
                        if (notifier != null)
                        {
                            notifier.PropertyChanged -= OnTargetPropertyChanged;
                            notifier.PropertyChanged += OnTargetPropertyChanged;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Status = BindingStatus.TargetPathError;

                Logger.Error(CultureInfo.CurrentCulture, Resources.Strings.DataBindingError, ex);
                BindingFailed?.Invoke(this, new MultiBindingFailedEventArgs(targetObjects?.FirstOrDefault()?.Target, targetPropertyPath, null, ex));
                UnregisterTargetListeners();
                return;
            }

            Binding.BindingFailed -= OnBindingFailed;
            Binding.BindingFailed += OnBindingFailed;

            isActivating = true;
            for (int i = 0; i < Bindings.Count; i++)
            {
                ActivateBinding(Bindings[i]);
            }
            isActivating = false;

            if (Bindings.Any(b => GetBindingMode(b) != BindingMode.OneWayToSource))
            {
                OnSourceValueUpdated(null, new HandledEventArgs());
            }
            
            if (Bindings.Any(b => GetBindingMode(b) == BindingMode.OneWayToSource))
            {
                OnTargetPropertyChanged(targetObjects.Last().Target, new PropertyChangedEventArgs(targetDescriptors.Last().Name));
            }
        }

        internal sealed override void Deactivate()
        {
            if (Status != BindingStatus.Inactive)
            {
                UnregisterTargetListeners();

                Binding.BindingFailed -= OnBindingFailed;
                foreach (var binding in Bindings)
                {
                    binding.SourceValueUpdated -= OnSourceValueUpdated;
                    binding.TargetValueUpdated -= OnTargetValueChanged;
                    binding.Deactivate();
                }

                Status = BindingStatus.Inactive;
            }
        }

        private void ActivateBinding(Binding binding)
        {
            binding.SourceValueUpdated -= OnSourceValueUpdated;
            binding.SourceValueUpdated += OnSourceValueUpdated;
            binding.TargetValueUpdated -= OnTargetValueChanged;
            binding.TargetValueUpdated += OnTargetValueChanged;
            binding.Activate(targetObjects, targetPropertyPath, targetDescriptors);

            if (binding.Status == BindingStatus.Active)
            {
                Status = BindingStatus.Active;
            }
        }

        private BindingMode GetBindingMode(Binding binding)
        {
            return binding.Mode == BindingMode.Default ? Mode : binding.Mode;
        }

        private static object GetValue(object obj, PropertyDescriptor descriptor, object[] indices)
        {
            if (indices != null)
            {
                obj = descriptor.GetValue(obj);
                if (descriptor.PropertyType.IsArray)
                {
                    return ((Array)obj).GetValue(indices.Cast<int>().ToArray());
                }
                else
                {
                    return obj.GetType().GetRuntimeProperty("Item").GetValue(obj, indices);
                }
            }

            return descriptor.GetValue(obj);
        }

        private void SetValue(object obj, PropertyDescriptor descriptor, object value, object[] indices)
        {
            if (syncContext != null && SynchronizationContext.Current != syncContext)
            {
                SynchronizationContext.SetSynchronizationContext(syncContext);
                syncContext.Post((state) =>
                {
                    var array = (object[])state;
                    SetValue(array[0], (PropertyDescriptor)array[1], array[2], (object[])array[3]);
                }, new object[] { obj, descriptor, value, indices });

                return;
            }

            if (indices != null)
            {
                obj = descriptor.GetValue(obj);
                if (descriptor.PropertyType.IsArray)
                {
                    ((Array)obj).SetValue(value, indices.Cast<int>().ToArray());
                }
                else
                {
                    obj.GetType().GetRuntimeProperty("Item").SetValue(obj, value, indices);
                }
            }
            else
            {
                descriptor.SetValue(obj, value);
            }
        }

        private void OnBindingFailed(Binding sender, BindingFailedEventArgs e)
        {
            if (Bindings.Contains(sender))
            {
                var args = new MultiBindingFailedEventArgs(targetObjects?.FirstOrDefault()?.Target, targetPropertyPath, sender, e.Exception);
                BindingFailed?.Invoke(this, args);
                e.Ignore = e.Ignore || args.Ignore;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exact error types can be unpredictable but should not interfere with execution of the program.  Status property and BindingFailed event can be used for debugging purposes.")]
        private void OnSourceValueUpdated(Binding binding, HandledEventArgs e)
        {
            e.IsHandled = true;
            if (isActivating)
            {
                return;
            }

            try
            {
                var targetDescriptor = targetDescriptors.Last();
                if (targetDescriptor.IsReadOnly)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.PropertyIsReadOnly, targetDescriptor.Name));
                }

                var affectedBindings = Bindings.Where(b => b.Status == BindingStatus.Active && GetBindingMode(b) != BindingMode.OneWayToSource).ToArray();
                if (affectedBindings.Length == 0)
                {
                    return;
                }

                object[] values = new object[affectedBindings.Length];
                for (int i = 0; i < affectedBindings.Length; i++)
                {
                    binding = affectedBindings[i];
                    if (binding.Converter == null)
                    {
                        values[i] = binding.GetSourceValue();
                    }
                    else
                    {
                        values[i] = binding.Converter.Convert(binding.GetSourceValue(), targetDescriptor.PropertyType,
                            binding.ConverterParameter, binding.ConverterCulture ?? CultureInfo.CurrentCulture);
                    }

                    if (GetBindingMode(binding) == BindingMode.OneTimeToTarget)
                    {
                        binding.Deactivate();
                    }
                }

                object value = values;
                if (Converter != null)
                {
                    value = Converter.Convert(values, targetDescriptor.PropertyType, ConverterParameter, ConverterCulture ?? CultureInfo.CurrentCulture);
                }

                SetValue(targetObjects.Last().Target, targetDescriptor, value, targetPropertyPath.GetIndexValues(targetDescriptors.Length - 1));
            }
            catch (Exception ex)
            {
                Status = BindingStatus.TargetUpdateError;

                Logger.Error(CultureInfo.CurrentCulture, Resources.Strings.DataBindingError, ex);

                var args = new MultiBindingFailedEventArgs(targetObjects?.FirstOrDefault()?.Target, targetPropertyPath, null, ex);
                BindingFailed?.Invoke(this, args);
                if (args.Ignore)
                {
                    Status = BindingStatus.Active;
                }
                else
                {
                    UnregisterTargetListeners();
                    UnregisterSourceListeners();
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exact error types can be unpredictable but should not interfere with execution of the program.  Status property and BindingFailed event can be used for debugging purposes.")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Method is sufficiently maintainable.")]
        private void OnTargetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                for (int i = targetDescriptors.Length - 1; i >= 0; i--)
                {
                    if (targetDescriptors[i].Name == e.PropertyName && targetObjects[i].Target == sender)
                    {
                        for (int j = i; j < targetDescriptors.Length; j++)
                        {
                            var targetObj = targetObjects[j].Target;
                            var targetDescriptor = targetDescriptors[j];

                            if (j == targetDescriptors.Length - 1)
                            {
                                var value = GetValue(targetObj, targetDescriptor, targetPropertyPath.GetIndexValues(j));

                                var metadata = targetDescriptor.GetMetadata(targetObj.GetType());
                                var affectedBindings = Bindings.Where(b =>
                                {
                                    if (b.Status != BindingStatus.Active)
                                    {
                                        return false;
                                    }

                                    var bindingMode = GetBindingMode(b);
                                    if (bindingMode == BindingMode.OneWayToTarget || bindingMode == BindingMode.OneTimeToTarget || (bindingMode == BindingMode.Default && !metadata.BindsTwoWayByDefault))
                                    {
                                        return false;
                                    }

                                    var sourceProperty = b.GetSourceProperty();
                                    if ((sourceProperty?.IsReadOnly).GetValueOrDefault())
                                    {
                                        b.OnBindingFailed(new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.PropertyIsReadOnly, sourceProperty.Name)));
                                        return false;
                                    }

                                    return true;
                                }).ToArray();
                                    
                                if (affectedBindings.Length == 0)
                                {
                                    return;
                                }
                                
                                object[] sourceValues = null;
                                if (Converter != null)
                                {
                                    sourceValues = Converter.ConvertBack(value, affectedBindings.Select(b => b.GetSourceProperty().PropertyType).ToArray(),
                                        ConverterParameter, ConverterCulture ?? CultureInfo.CurrentCulture);
                                }

#if DEBUG
                                if (sourceValues != null && sourceValues.Length > affectedBindings.Length)
                                {
                                    Logger.Debug(CultureInfo.CurrentCulture, "MultiBinding converter returned {0} values for {1} sources.  Excess values will be ignored.", sourceValues.Length, affectedBindings.Length);
                                }
#endif

                                for (int k = 0; k < affectedBindings.Length; k++)
                                {
                                    if (sourceValues.Length > k)
                                    {
                                        affectedBindings[k].UpdateSourceValue(sourceValues?[k] ?? value);
                                    }
                                    else
                                    {
                                        Logger.Debug(CultureInfo.CurrentCulture, "MultiBinding converter returned fewer values than number of sources.  Excess sources will not be updated.");
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                var fwObject = targetObjects[j + 1].Target as FrameworkObject;
                                if (fwObject != null)
                                {
                                    fwObject.PropertyChanged -= OnTargetPropertyChanged;
                                }
                                else
                                {
                                    var notifier = targetObjects[j + 1].Target as INotifyPropertyChanged;
                                    if (notifier != null)
                                    {
                                        notifier.PropertyChanged -= OnTargetPropertyChanged;
                                    }
                                }

                                targetObj = GetValue(targetObj, targetDescriptor, targetPropertyPath.GetIndexValues(j));
                                fwObject = targetObj as FrameworkObject;
                                if (fwObject != null)
                                {
                                    fwObject.PropertyChanged -= OnTargetPropertyChanged;
                                }
                                else
                                {
                                    var notifier = targetObj as INotifyPropertyChanged;
                                    if (notifier != null)
                                    {
                                        notifier.PropertyChanged -= OnTargetPropertyChanged;
                                        notifier.PropertyChanged += OnTargetPropertyChanged;
                                    }
                                }

                                targetObjects[j + 1] = new WeakReference(targetObj);
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Status = BindingStatus.SourceUpdateError;

                Logger.Error(CultureInfo.CurrentCulture, Resources.Strings.DataBindingError, ex);

                var args = new MultiBindingFailedEventArgs(targetObjects?.FirstOrDefault()?.Target, targetPropertyPath, null, ex);
                BindingFailed?.Invoke(this, args);
                if (args.Ignore)
                {
                    Status = BindingStatus.Active;
                }
                else
                {
                    UnregisterTargetListeners();
                    UnregisterSourceListeners();
                }
            }
        }

        private void OnTargetValueChanged(Binding binding, HandledEventArgs e)
        {
            e.IsHandled = true;
            if (!isActivating)
            {
                OnTargetPropertyChanged(targetObjects.Last().Target, new PropertyChangedEventArgs(targetDescriptors.Last().Name));
            }
        }

        private void UnregisterSourceListeners()
        {
            foreach (var binding in Bindings)
            {
                binding.SourceValueUpdated -= OnSourceValueUpdated;
                binding.TargetValueUpdated -= OnTargetValueChanged;
                if (binding.Status == BindingStatus.Active)
                {
                    binding.Deactivate();
                }
            }
        }

        private void UnregisterTargetListeners()
        {
            if (targetObjects != null)
            {
                foreach (var weak in targetObjects)
                {
                    var fwObject = weak?.Target as FrameworkObject;
                    if (fwObject != null)
                    {
                        fwObject.PropertyChanged -= OnTargetPropertyChanged;
                    }
                    else
                    {
                        var notifier = weak?.Target as INotifyPropertyChanged;
                        if (notifier != null)
                        {
                            notifier.PropertyChanged -= OnTargetPropertyChanged;
                        }
                    }
                }
            }

            targetObjects = null;
        }
    }
}
