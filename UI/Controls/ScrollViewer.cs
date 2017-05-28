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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Prism.Native;
using Prism.Resources;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a scrollable area for visual content.
    /// </summary>
    [Resolve(typeof(INativeScrollViewer))]
    public class ScrollViewer : Element, IScrollable
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:Scrolled"/> event.
        /// </summary>
        public static EventDescriptor ScrolledEvent { get; } = EventDescriptor.Create(nameof(Scrolled), typeof(TypedEventHandler<ScrollViewer>), typeof(ScrollViewer));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:CanScrollHorizontally"/> property.
        /// </summary>
        public static PropertyDescriptor CanScrollHorizontallyProperty { get; } = PropertyDescriptor.Create(nameof(CanScrollHorizontally), typeof(bool), typeof(ScrollViewer), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:CanScrollVertically"/> property.
        /// </summary>
        public static PropertyDescriptor CanScrollVerticallyProperty { get; } = PropertyDescriptor.Create(nameof(CanScrollVertically), typeof(bool), typeof(ScrollViewer), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Content"/> property.
        /// </summary>
        public static PropertyDescriptor ContentProperty { get; } = PropertyDescriptor.Create(nameof(Content), typeof(object), typeof(ScrollViewer), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ContentOffset"/> property.
        /// </summary>
        public static PropertyDescriptor ContentOffsetProperty { get; } = PropertyDescriptor.Create(nameof(ContentOffset), typeof(Point), typeof(ScrollViewer), true);

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ContentSize"/> property.
        /// </summary>
        public static PropertyDescriptor ContentSizeProperty { get; } = PropertyDescriptor.Create(nameof(ContentSize), typeof(Size), typeof(ScrollViewer), true);
        #endregion

        /// <summary>
        /// Occurs when the contents of the scroll viewer has been scrolled.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<ScrollViewer> Scrolled;

        /// <summary>
        /// Gets or sets a value indicating whether the content of the scroll viewer can be scrolled horizontally.
        /// </summary>
        public bool CanScrollHorizontally
        {
            get { return nativeObject.CanScrollHorizontally; }
            set { nativeObject.CanScrollHorizontally = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the content of the scroll viewer can be scrolled vertically.
        /// </summary>
        public bool CanScrollVertically
        {
            get { return nativeObject.CanScrollVertically; }
            set { nativeObject.CanScrollVertically = value; }
        }

        /// <summary>
        /// Gets or sets the content of the scroll viewer.
        /// </summary>
        public object Content
        {
            get { return content; }
            set
            {
                if (value != content)
                {
                    content = value;
                    if (content is INativeElement)
                    {
                        nativeObject.Content = content;
                    }
                    else
                    {
                        var element = content as Element;
                        if (element == null && content != null)
                        {
                            element = new Label()
                            {
                                Text = content.ToString(),
                                TextAlignment = TextAlignment.Center
                            };
                        }

                        nativeObject.Content = ObjectRetriever.GetNativeObject(element);
                    }

                    OnPropertyChanged(ContentProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object content;

        /// <summary>
        /// Gets the distance that the content has been scrolled.
        /// </summary>
        public Point ContentOffset
        {
            get { return nativeObject.ContentOffset; }
        }

        /// <summary>
        /// Gets the size of the scrollable area within the scroll viewer.
        /// </summary>
        public Size ContentSize
        {
            get { return nativeObject.ContentSize; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeScrollViewer nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        public ScrollViewer()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected ScrollViewer(INativeScrollViewer nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeScrollViewer"/> instance.</exception>
        protected ScrollViewer(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeScrollViewer;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeScrollViewer).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Scrolls the content within the scroll viewer to the specified offset.
        /// </summary>
        /// <param name="offset">The position to which to scroll the content.</param>
        /// <param name="animate">Whether to animate the scrolling.</param>
        public void ScrollTo(Point offset, Animate animate)
        {
            nativeObject.ScrollTo(offset, animate);
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        protected override Size ArrangeOverride(Size constraints)
        {
            constraints = base.ArrangeOverride(constraints);

            var currentContent = (Content as Element) ?? VisualTreeHelper.GetChild<Element>(this);
            if (currentContent != null)
            {
                currentContent.Arrange(new Rectangle(0, 0, Math.Max(currentContent.DesiredSize.Width, constraints.Width),
                    Math.Max(currentContent.DesiredSize.Height, constraints.Height)));
            }

            return constraints;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        protected override Size MeasureOverride(Size constraints)
        {
            constraints = base.MeasureOverride(constraints);

            var currentContent = (Content as Element) ?? VisualTreeHelper.GetChild<Element>(this);
            if (currentContent != null)
            {
                currentContent.Measure(constraints);
                constraints.Width = Math.Min(currentContent.DesiredSize.Width, constraints.Width);
                constraints.Height = Math.Min(currentContent.DesiredSize.Height, constraints.Height);
            }

            return constraints;
        }

        /// <summary>
        /// Called when the contents of the scroll viewer is scrolled and raises the <see cref="Scrolled"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnScrolled(EventArgs e)
        {
            Scrolled?.Invoke(this, e);
        }

        private void Initialize()
        {
            nativeObject.Scrolled += (o, e) => OnScrolled(e);

            CanScrollHorizontally = false;
            CanScrollVertically = true;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
        }
    }
}
