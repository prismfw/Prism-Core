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
    /// Represents the base class for elements that contain and lay out other elements.  This class is abstract.
    /// </summary>
    public abstract class Panel : Element
    {
        #region Property Descriptors
        /// <summary>
        /// Gets a <see cref="PropertyDescriptor"/> describing the <see cref="P:Background"/> property.
        /// </summary>
        public static PropertyDescriptor BackgroundProperty { get; } = PropertyDescriptor.Create(nameof(Background), typeof(Brush), typeof(Panel));
        #endregion

        /// <summary>
        /// Gets or sets the background for the panel.
        /// </summary>
        public Brush Background
        {
            get { return nativeObject.Background; }
            set { nativeObject.Background = value; }
        }

        /// <summary>
        /// Gets a collection of the UI elements that reside within the panel.
        /// </summary>
        public ElementCollection Children { get; }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        // this field is to avoid casting
        private readonly INativePanel nativeObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        protected Panel()
            : this(typeof(INativePanel), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="resolveType">The type to pass to the IoC container in order to resolve the native object.</param>
        /// <param name="resolveName">An optional name to use when resolving the native object.</param>
        /// <param name="resolveParameters">Any parameters to pass along to the constructor of the resolve type.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolveType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="resolveType"/> is not a valid native instance.</exception>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "resolveType is validated in base constructor.")]
        protected Panel(Type resolveType, string resolveName, params ResolveParameter[] resolveParameters)
            : base(resolveType, resolveName, resolveParameters)
        {
            nativeObject = ObjectRetriever.GetNativeObject(this) as INativePanel;
            if (nativeObject == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.TypeMustResolveToType, resolveType.FullName, typeof(INativePanel).FullName), nameof(resolveType));
            }

            Children = new ElementCollection(this);
        }

        /// <summary>
        /// Retrieves the first child element found that is of type <typeparamref name="T"/> and that has the specified name.
        /// If no such element is found, a new one is created using the default constructor.  The new element is then assigned
        /// the specified name and added to the panel as a child.
        /// </summary>
        /// <typeparam name="T">The type of element to get or create.</typeparam>
        /// <param name="name">The name of the element to get or create.</param>
        /// <returns>The retrieved or newly created child element.</returns>
        /// <exception cref="MissingMemberException">Thrown when <typeparamref name="T"/> does not have a default constructor defined.</exception>
        public T GetOrCreateChild<T>(string name)
            where T : Element
        {
            foreach (var child in Children)
            {
                var t = child as T;
                if (t != null && t.Name == name)
                {
                    return t;
                }
            }

            var newChild = Activator.CreateInstance<T>();
            newChild.Name = name;
            Children.Add(newChild);
            return newChild;
        }

        /// <summary>
        /// Retrieves the first child element found that is of type <typeparamref name="T"/> and that has the specified name.
        /// If no such element is found, the specified creation method is invoked to create a new element.  The new element is
        /// then assigned the specified name and added to the panel as a child.
        /// </summary>
        /// <typeparam name="T">The type of element to get or create.</typeparam>
        /// <param name="name">The name of the element to get or create.</param>
        /// <param name="createMethod">The method to invoke for creating the element when one isn't found.</param>
        /// <returns>The retrieved or newly created child element.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="createMethod"/> is <c>null</c>.</exception>
        public T GetOrCreateChild<T>(string name, Func<T> createMethod)
            where T : Element
        {
            if (createMethod == null)
            {
                throw new ArgumentNullException(nameof(createMethod));
            }

            foreach (var child in Children)
            {
                var t = child as T;
                if (t != null && t.Name == name)
                {
                    return t;
                }
            }

            var newChild = createMethod();
            if (newChild != null)
            {
                newChild.Name = name;
                Children.Add(newChild);
            }

            return newChild;
        }
    }
}