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
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using Prism.Native;
using Prism.UI;
using Prism.Utilities;

namespace Prism.Data
{
    /// <summary>
    /// Represents a connection between the property of a target object and the property of a source object,
    /// allowing their values to be shared as they're updated.
    /// </summary>
    public class Binding : BindingBase
    {
        /// <summary>
        /// Occurs when an error is encountered while resolving a path or updating a value within a data binding.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public static event TypedEventHandler<Binding, BindingFailedEventArgs> BindingFailed;

        /// <summary>
        /// Gets or sets the converter to be used when passing values between the source property and the target property.
        /// </summary>
        public IValueConverter Converter { get; set; }

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
                        if (mode == BindingMode.OneWayToSource)
                        {
                            OnTargetPropertyChanged(targetObjects.Last().Target, new PropertyChangedEventArgs(targetDescriptors.Last().Name));
                        }
                        else
                        {
                            OnSourcePropertyChanged(sourceObjects.Last().Target, new PropertyChangedEventArgs(sourceDescriptors.Last().Name));
                        }
                    }
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BindingMode mode;

        /// <summary>
        /// Gets or sets the <see cref="PropertyPath"/> that describes the source property.
        /// </summary>
        public PropertyPath Path
        {
            get { return sourcePath ?? targetPropertyPath; }
            set
            {
                if (value != sourcePath)
                {
                    sourcePath = value;
                    if (Status != BindingStatus.Inactive && Status != BindingStatus.TargetPathError)
                    {
                        Status = BindingStatus.Active;

                        sourceDescriptors = null;
                        UnregisterSourceListeners();
                        RegisterSourceListeners();

                        if (Status == BindingStatus.Active)
                        {
                            if (Mode == BindingMode.OneWayToSource)
                            {
                                OnTargetPropertyChanged(targetObjects.Last().Target, new PropertyChangedEventArgs(targetDescriptors.Last().Name));
                            }
                            else
                            {
                                OnSourcePropertyChanged(sourceObjects.Last().Target, new PropertyChangedEventArgs(sourceDescriptors.Last().Name));
                            }
                        }
                    }
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PropertyPath sourcePath;

        /// <summary>
        /// Gets or sets the object that contains the source property for the binding.
        /// If <c>null</c>, the target object will also act as the source object.
        /// </summary>
        public object Source
        {
            get
            {
                if (source != null)
                {
                    return source;
                }

                if (targetObjects == null || targetObjects.Length == 0)
                {
                    return null;
                }

                var target = targetObjects[0].Target;
                var dataContext = target as IDataContext;
                if (dataContext?.DataContext == null)
                {
                    dataContext = VisualTreeHelper.GetParent<IDataContext>(target, dc => dc.DataContext != null);
                }

                return dataContext?.DataContext ?? target;
            }
            set
            {
                if (value != source)
                {
                    source = value;
                    if (Status != BindingStatus.Inactive)
                    {
                        UnregisterSourceListeners();
                        RegisterSourceListeners();
                    }
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object source;

        /// <summary>
        /// Gets the current status of the binding.
        /// </summary>
        public BindingStatus Status { get; internal set; }

        internal event TypedEventHandler<Binding, HandledEventArgs> SourceValueUpdated;

        internal event TypedEventHandler<Binding, HandledEventArgs> TargetValueUpdated;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly SynchronizationContext syncContext = SynchronizationContext.Current;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private PropertyDescriptor[] sourceDescriptors;
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private WeakReference[] sourceObjects;
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
        /// Initializes a new instance of the <see cref="Binding"/> class.
        /// </summary>
        public Binding()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Binding"/> class.
        /// </summary>
        /// <param name="sourceProperty">The <see cref="PropertyPath"/> describing the source property.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceProperty"/> is <c>null</c>.</exception>
        public Binding(PropertyPath sourceProperty)
        {
            if (sourceProperty == null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }
            
            Path = sourceProperty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Binding"/> class.
        /// </summary>
        /// <param name="sourceProperty">The <see cref="PropertyDescriptor"/> describing the source property.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sourceProperty"/> is <c>null</c>.</exception>
        public Binding(PropertyDescriptor sourceProperty)
        {
            if (sourceProperty == null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }
            
            Path = new PropertyPath(sourceProperty.Name);
            
            sourceDescriptors = new PropertyDescriptor[] { sourceProperty };
        }
        
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exact error types can be unpredictable but should not interfere with execution of the program.  Status property and BindingFailed event can be used for debugging purposes.")]
        internal sealed override void Activate(object targetObject, PropertyPath targetPath)
        {
            Deactivate();
            Status = BindingStatus.Active;

            if (targetObject == null)
            {
                Status = BindingStatus.TargetPathError;

                var ex = new ArgumentNullException(nameof(targetObject));
                Logger.Error(CultureInfo.CurrentCulture, Resources.Strings.DataBindingError, ex);
                BindingFailed?.Invoke(this, new BindingFailedEventArgs(targetObject, targetPath, ex));
                return;
            }

            targetPropertyPath = targetPath;
            try
            {
                targetPath.ResolvePath(targetObject, out targetObjects, out targetDescriptors, true);
            }
            catch (Exception ex)
            {
                Status = BindingStatus.TargetPathError;
                OnBindingFailed(ex);
                UnregisterTargetListeners();
                UnregisterSourceListeners();
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
                OnBindingFailed(ex);
                UnregisterTargetListeners();
                UnregisterSourceListeners();
                return;
            }

            RegisterSourceListeners();
        }

        internal void Activate(WeakReference[] targets, PropertyPath targetPath, PropertyDescriptor[] targetProperties)
        {
            Deactivate();
            Status = BindingStatus.Active;

            targetObjects = targets;
            targetDescriptors = targetProperties;
            targetPropertyPath = targetPath;

            RegisterSourceListeners();
        }
        
        internal sealed override void Deactivate()
        {
            if (Status != BindingStatus.Inactive)
            {
                UnregisterTargetListeners();
                UnregisterSourceListeners();
                Status = BindingStatus.Inactive;
            }
        }

        internal PropertyDescriptor GetSourceProperty()
        {
            return sourceDescriptors?.LastOrDefault();
        }

        internal object GetSourceValue()
        {
            return GetValue(sourceObjects.Last().Target, sourceDescriptors.Last(), Path.GetIndexValues(sourceDescriptors.Length - 1));
        }

        internal bool OnBindingFailed(Exception ex)
        {
            Logger.Error(CultureInfo.CurrentCulture, Resources.Strings.DataBindingError, ex);

            var args = new BindingFailedEventArgs(targetObjects?.FirstOrDefault()?.Target, targetPropertyPath, ex);
            BindingFailed?.Invoke(this, args);
            return args.Ignore;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exact error types can be unpredictable but should not interfere with execution of the program.  Status property and BindingFailed event can be used for debugging purposes.")]
        internal void UpdateSourceValue(object value)
        {
            if (sourceObjects == null || sourceObjects.Length == 0)
            {
                return;
            }

            try
            {
                var property = sourceDescriptors.Last();
                if (property.IsReadOnly)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.PropertyIsReadOnly, property.Name));
                }

                SetValue(sourceObjects.Last().Target, property, GetConvertedValue(value, property.PropertyType, false), Path.GetIndexValues(sourceDescriptors.Length - 1));
            }
            catch (Exception ex)
            {
                Status = BindingStatus.SourceUpdateError;
                if (OnBindingFailed(ex))
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

        private static object GetValue(object obj, PropertyDescriptor descriptor, object[] indices)
        {
            return obj == null ? null : descriptor.GetValue(obj, indices);
        }

        private object GetConvertedValue(object value, Type targetType, bool isSource)
        {
            if (Converter != null)
            {
                if (isSource)
                {
                    value = Converter.Convert(value, targetType, ConverterParameter, ConverterCulture ?? CultureInfo.CurrentCulture);
                }
                else
                {
                    value = Converter.ConvertBack(value, targetType, ConverterParameter, ConverterCulture ?? CultureInfo.CurrentCulture);
                }
            }

            return value;
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

            descriptor.SetValue(obj, value, indices);
        }

        private void OnDataContextChanged(IDataContext sender, DataContextChangedEventArgs e)
        {
            if (targetObjects == null || targetObjects.Length == 0)
            {
                sender.DataContextChanged -= OnDataContextChanged;
                return;
            }

            if (source != null)
            {
                return;
            }

            UnregisterSourceListeners();
            RegisterSourceListeners();
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exact error types can be unpredictable but should not interfere with execution of the program.  Status property and BindingFailed event can be used for debugging purposes.")]
        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                for (int i = sourceDescriptors.Length - 1; i >= 0; i--)
                {
                    if (sourceDescriptors[i].Name == e.PropertyName && sourceObjects[i].Target == sender)
                    {
                        for (int j = i; j < sourceDescriptors.Length; j++)
                        {
                            var sourceObj = sourceObjects[j].Target;
                            var sourceDescriptor = sourceDescriptors[j];

                            if (j == sourceDescriptors.Length - 1)
                            {
                                if (Mode == BindingMode.OneWayToSource)
                                {
                                    return;
                                }

                                var handler = SourceValueUpdated;
                                if (handler != null)
                                {
                                    var args = new HandledEventArgs();
                                    handler(this, args);
                                    if (args.IsHandled)
                                    {
                                        return;
                                    }
                                }

                                var value = GetValue(sourceObj, sourceDescriptor, Path.GetIndexValues(j));
                                var targetDescriptor = targetDescriptors.Last();

                                if (targetDescriptor.IsReadOnly)
                                {
                                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.PropertyIsReadOnly, targetDescriptor.Name));
                                }

                                SetValue(targetObjects.Last().Target, targetDescriptor, GetConvertedValue(value, targetDescriptor.PropertyType, true),
                                    targetPropertyPath.GetIndexValues(targetDescriptors.Length - 1));

                                if (Mode == BindingMode.OneTimeToTarget)
                                {
                                    Deactivate();
                                }
                            }
                            else
                            {
                                var fwObject = sourceObjects[j + 1].Target as FrameworkObject;
                                if (fwObject != null)
                                {
                                    fwObject.PropertyChanged -= OnSourcePropertyChanged;
                                }
                                else
                                {
                                    var notifier = sourceObjects[j + 1].Target as INotifyPropertyChanged;
                                    if (notifier != null)
                                    {
                                        notifier.PropertyChanged -= OnSourcePropertyChanged;
                                    }
                                }

                                sourceObj = GetValue(sourceObj, sourceDescriptor, Path.GetIndexValues(j));
                                fwObject = sourceObj as FrameworkObject;
                                if (fwObject != null)
                                {
                                    fwObject.PropertyChanged -= OnSourcePropertyChanged;
                                    fwObject.PropertyChanged += OnSourcePropertyChanged;
                                }
                                else
                                {
                                    var notifier = sourceObj as INotifyPropertyChanged;
                                    if (notifier != null)
                                    {
                                        notifier.PropertyChanged -= OnSourcePropertyChanged;
                                        notifier.PropertyChanged += OnSourcePropertyChanged;
                                    }
                                }

                                sourceObjects[j + 1] = new WeakReference(sourceObj);
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Status = BindingStatus.TargetUpdateError;
                if (OnBindingFailed(ex))
                {
                    if (Mode == BindingMode.OneTimeToTarget)
                    {
                        Deactivate();
                    }
                    else
                    {
                        Status = BindingStatus.Active;
                    }
                }
                else
                {
                    UnregisterTargetListeners();
                    UnregisterSourceListeners();
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exact error types can be unpredictable but should not interfere with execution of the program.  Status property and BindingFailed event can be used for debugging purposes.")]
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
                                if (Mode == BindingMode.OneWayToTarget || (Mode == BindingMode.Default && !targetDescriptor.GetMetadata(targetObj.GetType()).BindsTwoWayByDefault) || Mode == BindingMode.OneTimeToTarget)
                                {
                                    return;
                                }

                                var handler = TargetValueUpdated;
                                if (handler != null)
                                {
                                    var args = new HandledEventArgs();
                                    handler(this, args);
                                    if (args.IsHandled)
                                    {
                                        return;
                                    }
                                }

                                var value = GetValue(targetObj, targetDescriptor, targetPropertyPath.GetIndexValues(j));
                                var sourceDescriptor = sourceDescriptors.Last();

                                if (sourceDescriptor.IsReadOnly)
                                {
                                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.PropertyIsReadOnly, sourceDescriptor.Name));
                                }

                                SetValue(sourceObjects.Last().Target, sourceDescriptor, GetConvertedValue(value, sourceDescriptor.PropertyType, false),
                                    Path.GetIndexValues(sourceDescriptors.Length - 1));
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
                if (OnBindingFailed(ex))
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
        private void RegisterSourceListeners()
        {
            if (targetObjects == null || targetObjects.Length == 0)
            {
                return;
            }

            var target = targetObjects[0].Target;
            var dataContext = target as IDataContext ?? VisualTreeHelper.GetParent<IDataContext>(target);
            while (dataContext != null)
            {
                if (dataContext != null)
                {
                    dataContext.DataContextChanged -= OnDataContextChanged;
                    dataContext.DataContextChanged += OnDataContextChanged;
                    if (dataContext.DataContext != null)
                    {
                        break;
                    }
                }

                dataContext = VisualTreeHelper.GetParent<IDataContext>(dataContext);
            }

            if (sourceDescriptors == null)
            {
                try
                {
                    Path.ResolvePath(Source, out sourceObjects, out sourceDescriptors, true);
                }
                catch (Exception ex)
                {
                    Status = BindingStatus.SourcePathError;
                    OnBindingFailed(ex);
                    UnregisterTargetListeners();
                    UnregisterSourceListeners();
                    return;
                }
            }
            else
            {
                sourceObjects = new[] { new WeakReference(Source) };
            }

            try
            {
                for (int i = 0; i < sourceObjects.Length; i++)
                {
                    var sourceObject = ObjectRetriever.GetAgnosticObject(sourceObjects[i].Target);
                    var fwObject = sourceObject as FrameworkObject;
                    if (fwObject != null)
                    {
                        fwObject.PropertyChanged -= OnSourcePropertyChanged;
                        fwObject.PropertyChanged += OnSourcePropertyChanged;
                    }
                    else
                    {
                        var notifier = sourceObject as INotifyPropertyChanged ?? ObjectRetriever.GetNativeObject(sourceObject) as INotifyPropertyChanged;
                        if (notifier != null)
                        {
                            notifier.PropertyChanged -= OnSourcePropertyChanged;
                            notifier.PropertyChanged += OnSourcePropertyChanged;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Status = BindingStatus.SourcePathError;
                OnBindingFailed(ex);
                UnregisterTargetListeners();
                UnregisterSourceListeners();
                return;
            }

            if (Mode == BindingMode.OneWayToSource)
            {
                OnTargetPropertyChanged(targetObjects.Last().Target, new PropertyChangedEventArgs(targetDescriptors.Last().Name));
            }
            else
            {
                OnSourcePropertyChanged(sourceObjects.Last().Target, new PropertyChangedEventArgs(sourceDescriptors.Last().Name));
            }
        }

        private void UnregisterSourceListeners()
        {
            if (targetObjects != null && targetObjects.Length > 0)
            {
                var target = targetObjects[0].Target;
                var dataContext = target as IDataContext ?? VisualTreeHelper.GetParent<IDataContext>(target);
                while (dataContext != null)
                {
                    dataContext.DataContextChanged -= OnDataContextChanged;
                    dataContext = VisualTreeHelper.GetParent<IDataContext>(dataContext);
                }
            }

            if (sourceObjects != null)
            {
                foreach (var weak in sourceObjects)
                {
                    var fwObject = weak?.Target as FrameworkObject;
                    if (fwObject != null)
                    {
                        fwObject.PropertyChanged -= OnSourcePropertyChanged;
                    }
                    else
                    {
                        var notifier = weak?.Target as INotifyPropertyChanged;
                        if (notifier != null)
                        {
                            notifier.PropertyChanged -= OnSourcePropertyChanged;
                        }
                    }
                }
            }
            
            sourceObjects = null;
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
