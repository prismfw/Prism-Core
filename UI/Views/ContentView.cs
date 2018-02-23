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
using System.Diagnostics;
using System.Globalization;
using Prism.Native;
using Prism.Resources;
using Prism.UI.Controls;
using Prism.UI.Media;

namespace Prism.UI
{
    /// <summary>
    /// Represents a platform-agnostic view that can display a single piece of content.
    /// The generic version implements <see cref="IView&lt;T&gt;"/> for specifying a model type.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="P:Model"/>.</typeparam>
    public class ContentView<T> : ContentView, IView<T>
    {
        /// <summary>
        /// Gets or sets the model containing the information that is displayed by the view.
        /// </summary>
        public new T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentView&lt;T&gt;"/> class.
        /// </summary>
        public ContentView()
        {
        }
    }

    /// <summary>
    /// Represents a platform-agnostic view that can display a single piece of content.
    /// </summary>
    [Resolve(typeof(INativeContentView))]
    public class ContentView : View, IViewStackChild
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(ContentView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Content"/> property.
        /// </summary>
        public static PropertyDescriptor ContentProperty { get; } = PropertyDescriptor.Create(nameof(Content), typeof(object), typeof(ContentView), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsBackButtonEnabled"/> property.
        /// </summary>
        public static PropertyDescriptor IsBackButtonEnabledProperty { get; } = PropertyDescriptor.Create(nameof(IsBackButtonEnabled), typeof(bool), typeof(ContentView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:IsValidBackTarget"/> property.
        /// </summary>
        public static PropertyDescriptor IsValidBackTargetProperty { get; } = PropertyDescriptor.Create(nameof(IsValidBackTarget), typeof(bool), typeof(ContentView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Menu"/> property.
        /// </summary>
        public static PropertyDescriptor MenuProperty { get; } = PropertyDescriptor.Create(nameof(Menu), typeof(ActionMenu), typeof(ContentView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:StackId"/> property.
        /// </summary>
        public static PropertyDescriptor StackIdProperty { get; } = PropertyDescriptor.Create(nameof(StackId), typeof(string), typeof(ContentView));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Title"/> property.
        /// </summary>
        public static PropertyDescriptor TitleProperty { get; } = PropertyDescriptor.Create(nameof(Title), typeof(string), typeof(ContentView));
        #endregion

        /// <summary>
        /// Gets or sets the background for the view.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets or sets the content to be displayed by the view.
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
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center
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
        /// Gets or sets a value indicating whether the back button of a <see cref="ViewStack"/>
        /// is enabled when this view is the visible view of the stack.
        /// </summary>
        public bool IsBackButtonEnabled
        {
            get { return isBackButtonEnabled; }
            set
            {
                if (value != isBackButtonEnabled)
                {
                    isBackButtonEnabled = value;
                    (Parent as ViewStack)?.UpdateBackButtonState();
                    OnPropertyChanged(IsBackButtonEnabledProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isBackButtonEnabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can be popped to by pressing the back button of a <see cref="ViewStack"/>.
        /// A value of <c>false</c> means that the back button should be disabled when this instance is next in the stack.
        /// </summary>
        public bool IsValidBackTarget
        {
            get { return isValidBackTarget; }
            set
            {
                if (value != isValidBackTarget)
                {
                    isValidBackTarget = value;
                    (Parent as ViewStack)?.UpdateBackButtonState();
                    OnPropertyChanged(IsValidBackTargetProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isValidBackTarget = true;

        /// <summary>
        /// Gets or sets the action menu for the view.
        /// </summary>
        public ActionMenu Menu
        {
            get { return (ActionMenu)ObjectRetriever.GetAgnosticObject(nativeObject.Menu); }
            set { nativeObject.Menu = (INativeActionMenu)ObjectRetriever.GetNativeObject(value); }
        }

        /// <summary>
        /// Gets or sets an identifier that determines the position in which this instance is placed in a <see cref="ViewStack"/>.
        /// Objects with the same identifier value will replace each other within the same <see cref="ViewStack"/>, and
        /// objects with different identifiers will be assigned different positions even if they are of the same type.
        /// </summary>
        public string StackId
        {
            get { return stackId; }
            set
            {
                if (value != stackId)
                {
                    stackId = value;
                    OnPropertyChanged(StackIdProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string stackId;

        /// <summary>
        /// Gets or sets the title of the view.
        /// </summary>
        public string Title
        {
            get { return nativeObject.Title; }
            set { nativeObject.Title = value; }
        }
        
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeContentView nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentView"/> class.
        /// </summary>
        public ContentView()
            : this(ResolveParameter.EmptyParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentView"/> class and pairs it with the specified native object.
        /// </summary>
        /// <param name="nativeObject">The native object with which to pair this instance.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="nativeObject"/> doesn't match the type specified by the topmost <see cref="ResolveAttribute"/> in the inheritance chain.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="nativeObject"/> is <c>null</c>.</exception>
        protected ContentView(INativeContentView nativeObject)
            : base(nativeObject)
        {
            this.nativeObject = nativeObject;

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentView"/> class and pairs it with a native object that is resolved from the IoC container.
        /// </summary>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the native type.</param>
        /// <exception cref="TypeResolutionException">Thrown when the native object does not resolve to an <see cref="INativeContentView"/> instance.</exception>
        protected ContentView(ResolveParameter[] resolveParameters)
            : base(resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeContentView;
            if (nativeObject == null)
            {
                throw new TypeResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType,
                    ObjectRetriever.GetNativeObject(this).GetType().FullName, typeof(INativeContentView).FullName));
            }

            Initialize();
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            var currentContent = (Content as Visual) ?? VisualTreeHelper.GetChild<Visual>(this);
            if (currentContent != null)
            {
                double inset;
                if (currentContent is IScrollable)
                {
                    inset = 0;
                }
                else
                {
                    var viewStack = Parent as ViewStack;
                    inset = (viewStack == null || viewStack.IsHeaderHidden || !viewStack.Header.IsInset) ? 0 : viewStack.Header.RenderSize.Height;
                }

                currentContent.Arrange(new Rectangle(0, inset, Math.Max(constraints.Width, 0), Math.Max(constraints.Height - inset, 0)));
            }

            return constraints;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size MeasureOverride(Size constraints)
        {
            var currentContent = (Content as Visual) ?? VisualTreeHelper.GetChild<Visual>(this);
            if (currentContent != null)
            {
                currentContent.Measure(GetChildConstraints(currentContent, constraints, false));
            }

            return nativeObject.Measure(constraints);
        }

        internal override Size GetChildConstraints(Visual child)
        {
            return GetChildConstraints(child, RenderSize, true);
        }

        private Size GetChildConstraints(Visual child, Size constraints, bool useRenderSize)
        {
            double inset;
            if (child is IScrollable)
            {
                inset = 0;
            }
            else
            {
                var viewStack = Parent as ViewStack;
                inset = (viewStack == null || viewStack.IsHeaderHidden || !viewStack.Header.IsInset) ? 0 :
                    (useRenderSize ? viewStack.Header.RenderSize : viewStack.Header.DesiredSize).Height;
            }

            return new Size(Math.Max(constraints.Width, 0), Math.Max(constraints.Height - inset, 0));
        }

        private void Initialize()
        {
            IsBackButtonEnabled = true;

            SetResourceReference(BackgroundProperty, SystemResources.ViewBackgroundBrushKey);
        }
    }
}
