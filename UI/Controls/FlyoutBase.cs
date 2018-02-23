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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.Resources;
using Prism.UI.Media;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents the base class for flyout objects such as <see cref="Flyout"/> and <see cref="MenuFlyout"/>.
    /// </summary>
    public abstract class FlyoutBase : Visual
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Closed"/> event.
        /// </summary>
        public static EventDescriptor ClosedEvent { get; } = EventDescriptor.Create(nameof(Closed), typeof(TypedEventHandler<FlyoutBase>), typeof(FlyoutBase));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Opened"/> event.
        /// </summary>
        public static EventDescriptor OpenedEvent { get; } = EventDescriptor.Create(nameof(Opened), typeof(TypedEventHandler<FlyoutBase>), typeof(FlyoutBase));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(FlyoutBase));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Placement"/> property.
        /// </summary>
        public static PropertyDescriptor PlacementProperty { get; } = PropertyDescriptor.Create(nameof(Placement), typeof(FlyoutPlacement), typeof(FlyoutBase));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:PlacementTarget"/> property.
        /// </summary>
        public static PropertyDescriptor PlacementTargetProperty { get; } = PropertyDescriptor.Create(nameof(PlacementTarget), typeof(object), typeof(FlyoutBase));
        #endregion

        /// <summary>
        /// Occurs when the flyout is closed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<FlyoutBase> Closed;

        /// <summary>
        /// Occurs when the flyout is opened.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<FlyoutBase> Opened;

        /// <summary>
        /// Gets or sets the background for the flyout.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets or sets the placement of the flyout in relation to its placement target.
        /// </summary>
        public FlyoutPlacement Placement
        {
            get { return nativeObject.Placement; }
            set { nativeObject.Placement = value; }
        }

        /// <summary>
        /// Gets the object that is being used for the placement target of the flyout.
        /// </summary>
        public object PlacementTarget { get; private set; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeFlyoutBase nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlyoutBase"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected FlyoutBase(INativeFlyoutBase nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlyoutBase"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeFlyoutBase"/> instance.</exception>
        protected FlyoutBase(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeFlyoutBase;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeFlyoutBase).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Dismisses the flyout.
        /// </summary>
        public void Hide()
        {
            nativeObject.Hide();

            if (null != PlacementTarget)
            {
                PlacementTarget = null;
                OnPropertyChanged(PlacementTargetProperty);
            }
        }

        /// <summary>
        /// Presents the flyout and positions it relative to the specified placement target.
        /// </summary>
        /// <param name="placementTarget">The object to use as the flyout's placement target.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="placementTarget"/> is <c>null</c>.</exception>
        public void ShowAt(Element placementTarget)
        {
            if (placementTarget == null)
            {
                throw new ArgumentNullException(nameof(placementTarget));
            }

            if (placementTarget != PlacementTarget)
            {
                PlacementTarget = placementTarget;
                OnPropertyChanged(PlacementTargetProperty);
            }

            nativeObject.ShowAt(ObjectRetriever.GetNativeObject(placementTarget));
        }

        /// <summary>
        /// Presents the flyout and positions it relative to the specified placement target.
        /// The button that is specified cannot be in an overflow menu, or this will not work.
        /// </summary>
        /// <param name="placementTarget">The object to use as the flyout's placement target.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="placementTarget"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="placementTarget"/> is not inside of an <see cref="ActionMenu"/> -or- when <paramref name="placementTarget"/> is inside of an overflow menu.</exception>
        public void ShowAt(MenuButton placementTarget)
        {
            if (placementTarget == null)
            {
                throw new ArgumentNullException(nameof(placementTarget));
            }

            var owner = VisualTreeHelper.GetChild<ActionMenu>(Window.Current.Content, a => a.Items.Contains(placementTarget));
            if (owner == null && Window.Current.PresentedPopup != null)
            {
                var popup = Window.Current.PresentedPopup;
                do
                {
                    owner = VisualTreeHelper.GetChild<ActionMenu>(popup, a => a.Items.Contains(placementTarget));
                    popup = popup.PresentedPopup;
                }
                while (owner == null && popup != null);
            }

            if (owner == null || owner.Items.IndexOf(placementTarget) >= owner.MaxDisplayItems)
            {
                throw new ArgumentException(Strings.MenuButtonCannotBeInOverflow);
            }

            if (placementTarget != PlacementTarget)
            {
                PlacementTarget = placementTarget;
                OnPropertyChanged(PlacementTargetProperty);
            }

            nativeObject.ShowAt(ObjectRetriever.GetNativeObject(placementTarget));
        }

        /// <summary>
        /// Called when the flyout is closed and raises the <see cref="Closed"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnClosed(EventArgs e)
        {
            Closed?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the flyout is opened and raises the <see cref="Opened"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnOpened(EventArgs e)
        {   
            Opened?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.Closed += (o, e) => OnClosed(e);
            nativeObject.Opened += (o, e) => OnOpened(e);

            Placement = FlyoutPlacement.Auto;

            SetResourceReference(BackgroundProperty, SystemResources.FlyoutBackgroundBrushKey);
        }
    }
}
