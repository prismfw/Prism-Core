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
    /// Represents a UI element that can be used to enter search query text.
    /// </summary>
    public class SearchBox : Control
    {
        #region Event Descriptors
        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:QueryChanged"/> event.
        /// </summary>
        public static EventDescriptor QueryChangedEvent { get; } = EventDescriptor.Create(nameof(QueryChanged), typeof(TypedEventHandler<SearchBox, QueryChangedEventArgs>), typeof(SearchBox));

        /// <summary>
        /// Gets an <see cref="EventDescriptor"/> describing the <see cref="E:QuerySubmitted"/> event.
        /// </summary>
        public static EventDescriptor QuerySubmittedEvent { get; } = EventDescriptor.Create(nameof(QuerySubmitted), typeof(TypedEventHandler<SearchBox, QuerySubmittedEventArgs>), typeof(SearchBox));
        #endregion

        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:ActionKeyType"/> property.
        /// </summary>
        public static PropertyDescriptor ActionKeyTypeProperty { get; } = PropertyDescriptor.Create(nameof(ActionKeyType), typeof(ActionKeyType), typeof(SearchBox));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Placeholder"/> property.
        /// </summary>
        public static PropertyDescriptor PlaceholderProperty { get; } = PropertyDescriptor.Create(nameof(Placeholder), typeof(string), typeof(SearchBox));

        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:QueryText"/> property.
        /// </summary>
        public static PropertyDescriptor QueryTextProperty { get; } = PropertyDescriptor.Create(nameof(QueryText), typeof(string), typeof(SearchBox), new PropertyMetadata(PropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        /// <summary>
        /// Occurs when the value of the <see cref="P:QueryText"/> property has changed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<SearchBox, QueryChangedEventArgs> QueryChanged;

        /// <summary>
        /// Occurs when the user submits a search query.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Event handler provides a strongly-typed sender for easier use.")]
        public event TypedEventHandler<SearchBox, QuerySubmittedEventArgs> QuerySubmitted;

        /// <summary>
        /// Gets or sets the type of action key to use for the soft keyboard when the control has focus.
        /// </summary>
        public ActionKeyType ActionKeyType
        {
            get { return nativeObject.ActionKeyType; }
            set { nativeObject.ActionKeyType = value; }
        }

        /// <summary>
        /// Gets or sets the text to display when the control does not have a value.
        /// </summary>
        public string Placeholder
        {
            get { return nativeObject.Placeholder; }
            set { nativeObject.Placeholder = value; }
        }

        /// <summary>
        /// Gets or sets the query text value of the control.
        /// </summary>
        public string QueryText
        {
            get { return nativeObject.QueryText; }
            set { nativeObject.QueryText = value; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativeSearchBox nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBox"/> class.
        /// </summary>
        public SearchBox()
            : this(typeof(INativeSearchBox), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBox"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> does not resolve to an <see cref="INativeSearchBox"/> instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected SearchBox(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativeSearchBox;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativeSearchBox).FullName), nameof(resolveType));
            }

            nativeObject.QueryChanged += (o, e) => OnQueryChanged(e);
            nativeObject.QuerySubmitted += (o, e) => OnQuerySubmitted(e);

            BorderWidth = (double)Application.Current.Resources[SystemResources.SearchBoxBorderWidthKey];
            FontSize = (double)Application.Current.Resources[SystemResources.SearchBoxFontSizeKey];
            FontStyle = (FontStyle)Application.Current.Resources[SystemResources.SearchBoxFontStyleKey];
            HorizontalAlignment = HorizontalAlignment.Stretch;

            SetResourceReference(BackgroundProperty, SystemResources.SearchBoxBackgroundBrushKey);
            SetResourceReference(BorderBrushProperty, SystemResources.SearchBoxBorderBrushKey);
            SetResourceReference(ForegroundProperty, SystemResources.SearchBoxForegroundBrushKey);
        }

        /// <summary>
        /// Called when the value of <see cref="P:QueryText"/> is changed and raises the <see cref="QueryChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnQueryChanged(QueryChangedEventArgs e)
        {
            QueryChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the user submits a search query and raises the <see cref="QuerySubmitted"/> event.
        /// </summary>
        /// <param name="e">The event arguments for the event.</param>
        protected virtual void OnQuerySubmitted(QuerySubmittedEventArgs e)
        {
            QuerySubmitted?.Invoke(this, e);
        }
    }
}
