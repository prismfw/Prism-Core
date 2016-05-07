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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Prism.UI.Controls
{
    /// <summary>
    /// Represents a panel that arranges its children either vertically or horizontally, relative to each other.
    /// </summary>
    public class DockPanel : Panel
    {
        #region Property Descriptors
        /// <summary>
        /// Describes the <see cref="P:LastChildFill"/> property.  This field is read-only.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "PropertyDescriptor is immutable.")]
        public static readonly PropertyDescriptor LastChildFillProperty = PropertyDescriptor.Create(nameof(LastChildFill), typeof(bool), typeof(DockPanel), new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.AffectsArrange));
        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether the last child element should be stretched to fill any remaining space.
        /// </summary>
        public bool LastChildFill
        {
            get { return lastChildFill; }
            set
            {
                if (value != lastChildFill)
                {
                    lastChildFill = value;
                    OnPropertyChanged(LastChildFillProperty);
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool lastChildFill = true;

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static readonly ConditionalWeakTable<Element, DockPosition> elements = new ConditionalWeakTable<Element, DockPosition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DockPanel"/> class.
        /// </summary>
        public DockPanel()
        {
        }

        /// <summary>
        /// Gets the <see cref="Dock"/> value of the specified element.
        /// </summary>
        /// <param name="element">The element from which to get the <see cref="Dock"/> value.</param>
        /// <returns>The dock value as a <see cref="Dock"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        public static Dock GetDock(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            DockPosition position;
            return elements.TryGetValue(element, out position) ? position.Dock : Dock.Left;
        }

        /// <summary>
        /// Sets the <see cref="Dock"/> value for the specified element.
        /// </summary>
        /// <param name="element">The element for which to set the <see cref="Dock"/> value.</param>
        /// <param name="value">The <see cref="Dock"/> value to set.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        public static void SetDock(Element element, Dock value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var position = elements.GetOrCreateValue(element);
            if (position.Dock != value)
            {
                position.Dock = value;

                var parent = element.Parent as DockPanel;
                if (parent != null)
                {
                    parent.InvalidateMeasure();
                    parent.InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// Sets the <see cref="Dock"/> value for the specified element.
        /// </summary>
        /// <param name="element">The element for which to set the <see cref="Dock"/> value.</param>
        /// <param name="value">The <see cref="Dock"/> value to set.</param>
        /// <param name="platforms">The platforms on which the value should be set.  Platforms that are not specified will not attempt to set the value.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="element"/> is <c>null</c>.</exception>
        public static void SetDock(Element element, Dock value, PlatformMask platforms)
        {
            if (platforms.HasFlag(Application.Current.Platform))
            {
                SetDock(element, value);
            }
        }

        /// <summary>
        /// Called when this instance is ready to arrange its children and returns the final rendering size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The final rendering size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size ArrangeOverride(Size constraints)
        {
            var renderSize = base.ArrangeOverride(constraints);
            var insets = new Thickness();

            var lastChild = Children.LastOrDefault();
            foreach (var child in Children)
            {
                if (lastChildFill && child == lastChild)
                {
                    child.Arrange(new Rectangle(insets.Left, insets.Top, renderSize.Width - (insets.Left + insets.Right),
                        renderSize.Height - (insets.Top + insets.Bottom)));

                    continue;
                }

                DockPosition position;
                elements.TryGetValue(child, out position);

                Dock dock = position == null ? Dock.Left : position.Dock;
                switch (dock)
                {
                    case Dock.Bottom:
                        child.Arrange(new Rectangle(insets.Left, renderSize.Height - (insets.Bottom + child.DesiredSize.Height),
                            renderSize.Width - (insets.Left + insets.Right), child.DesiredSize.Height));
                        
                        insets.Bottom += child.RenderSize.Height + child.Margin.Top + child.Margin.Bottom;
                        break;
                    case Dock.Right:
                        child.Arrange(new Rectangle(renderSize.Width - (insets.Right + child.DesiredSize.Width), insets.Top,
                            child.DesiredSize.Width, renderSize.Height - (insets.Top + insets.Bottom)));

                        insets.Right += child.RenderSize.Width + child.Margin.Left + child.Margin.Right;
                        break;
                    case Dock.Top:
                        child.Arrange(new Rectangle(insets.Left, insets.Top, renderSize.Width - (insets.Left + insets.Right), child.DesiredSize.Height));
                        insets.Top += child.RenderSize.Height + child.Margin.Top + child.Margin.Bottom;
                        break;
                    default:
                        child.Arrange(new Rectangle(insets.Left, insets.Top, child.DesiredSize.Width, renderSize.Height - (insets.Top + insets.Bottom)));
                        insets.Left += child.RenderSize.Width + child.Margin.Left + child.Margin.Right;
                        break;
                }
            }

            return renderSize;
        }

        /// <summary>
        /// Called when this instance is ready to be measured and returns the desired size of the object.
        /// </summary>
        /// <param name="constraints">The width and height that this instance should not exceed.</param>
        /// <returns>The desired size of the object as a <see cref="Size"/> instance.</returns>
        protected override Size MeasureOverride(Size constraints)
        {
            constraints = base.MeasureOverride(constraints);

            double verticalInset = 0, horizontalInset = 0;
            Size desiredSize = new Size();
            foreach (var child in Children)
            {
                child.Measure(constraints);
                desiredSize.Width = Math.Min(Math.Max(desiredSize.Width, child.DesiredSize.Width + horizontalInset), constraints.Width);
                desiredSize.Height = Math.Min(Math.Max(desiredSize.Height, child.DesiredSize.Height + verticalInset), constraints.Height);

                DockPosition position;
                elements.TryGetValue(child, out position);
                if (position == null || position.Dock == Dock.Left || position.Dock == Dock.Right)
                {
                    constraints.Width = Math.Max(constraints.Width - child.DesiredSize.Width, 0);
                    horizontalInset += child.DesiredSize.Width;
                }
                else
                {
                    constraints.Height = Math.Max(constraints.Height - child.DesiredSize.Height, 0);
                    verticalInset += child.DesiredSize.Height;
                }
            }

            return desiredSize;
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Class is instantiated through ConditionalWeakTable.GetOrCreateValue method.")]
        private class DockPosition
        {
            public Dock Dock = Dock.Left;
        }
    }
}
