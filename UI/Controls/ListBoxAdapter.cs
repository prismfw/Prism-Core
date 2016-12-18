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


namespace Prism.UI.Controls
{
    /// <summary>
    /// Translates data objects into UI elements for display within a <see cref="ListBox"/>.  This class is abstract.
    /// </summary>
    public abstract class ListBoxAdapter
    {
        /// <summary>
        /// Used to get the <see cref="ListBoxItem"/> for the provided object that will be displayed by the list box.
        /// </summary>
        /// <param name="value">The object in the list box's <see cref="P:Items"/> collection for which to return a <see cref="ListBoxItem"/>.</param>
        /// <param name="reusedItem">A recycled <see cref="ListBoxItem"/> that can be reused instead of creating a new one, or <c>null</c> if nothing was recycled.</param>
        /// <param name="listBox">The <see cref="ListBox"/> instance that will display the item.</param>
        /// <returns>The <see cref="ListBoxItem"/> instance that will be displayed by the list box.</returns>
        public virtual ListBoxItem GetItem(object value, ListBoxItem reusedItem, ListBox listBox)
        {
            return GetDefaultItem(value, reusedItem, listBox);
        }

        /// <summary>
        /// Used to get an identifier for the provided object that is used to determine reuse eligibility.
        /// Objects that share the same identifier are eligible for reuse.
        /// </summary>
        /// <param name="value">The object in the list box's <see cref="P:Items"/> collection for which to return an identifier.</param>
        /// <param name="listBox">The <see cref="ListBox"/> instance containing the object.</param>
        /// <returns>The object identifier as a <see cref="string"/>.</returns>
        public virtual string GetItemId(object value, ListBox listBox)
        {
            return (value is Element) ? value.GetType().FullName : string.Empty;
        }

        /// <summary>
        /// Used to get a <see cref="ListBoxSectionHeader"/> that is displayed above the specified object.
        /// </summary>
        /// <param name="value">The object in the list box's <see cref="P:Items"/> collection that is underneath the <see cref="ListBoxSectionHeader"/>.</param>
        /// <param name="reusedItem">A recycled <see cref="ListBoxSectionHeader"/> that can be reused instead of creating a new one, or <c>null</c> if nothing was recycled.</param>
        /// <param name="listBox">The <see cref="ListBox"/> instance that will display the header.</param>
        /// <returns>The <see cref="ListBoxSectionHeader"/> that will be displayed by the list box.</returns>
        public virtual ListBoxSectionHeader GetSectionHeader(object value, ListBoxSectionHeader reusedItem, ListBox listBox)
        {
            return GetDefaultSectionHeader(value, reusedItem, listBox);
        }

        /// <summary>
        /// Used to get an identifier for the section header associated with the specified object that is used to determine reuse eligibility.
        /// Section headers that share the same identifier are eligible for reuse.
        /// </summary>
        /// <param name="value">The object in the list box's <see cref="P:Items"/> collection for which to return an identifier.</param>
        /// <param name="listBox">The <see cref="ListBox"/> instance containing the object.</param>
        /// <returns>The section header identifier as a <see cref="string"/>.</returns>
        public virtual string GetSectionHeaderId(object value, ListBox listBox)
        {
            return string.Empty;
        }

        internal static ListBoxItem GetDefaultItem(object value, ListBoxItem reusedItem, ListBox listBox)
        {
            var element = value as Element;
            if (element != null)
            {
                var lbi = value as ListBoxItem;
                if (lbi != null)
                {
                    return lbi;
                }

                if (reusedItem == null)
                {
                    reusedItem = new ListBoxItem(ListBoxItemStyle.Empty);
                    if (listBox.Style == ListBoxStyle.Grouped)
                    {
                        reusedItem.SetResourceReference(ListBoxItem.BackgroundProperty, SystemResources.GroupedListBoxItemBackgroundBrushKey);
                    }
                }

                var panel = value as Panel;
                if (panel != null)
                {
                    reusedItem.ContentPanel = panel;
                    return reusedItem;
                }

                if (reusedItem.ContentPanel == null)
                {
                    reusedItem.ContentPanel = new Grid()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                }
                else
                {
                    reusedItem.ContentPanel.Children.Clear();
                }

                reusedItem.ContentPanel.Children.Add(element);
                return reusedItem;
            }

            if (reusedItem == null)
            {
                reusedItem = new ListBoxItem(ListBoxItemStyle.Default);
                if (listBox.Style == ListBoxStyle.Grouped)
                {
                    reusedItem.SetResourceReference(ListBoxItem.BackgroundProperty, SystemResources.GroupedListBoxItemBackgroundBrushKey);
                }
            }

            reusedItem.TextLabel.Text = value == null ? null : value.ToString();
            return reusedItem;
        }

        internal static ListBoxSectionHeader GetDefaultSectionHeader(object value, ListBoxSectionHeader reusedItem, ListBox listBox)
        {
            var obsec = value as IObservableSection;
            if (obsec == null || obsec.HeaderTitle == null)
            {
                return null;
            }

            if (reusedItem == null)
            {
                reusedItem = new ListBoxSectionHeader();
                if (listBox.Style == ListBoxStyle.Grouped)
                {
                    reusedItem.SetResourceReference(ListBoxSectionHeader.BackgroundProperty, SystemResources.GroupedSectionHeaderBackgroundBrushKey);
                    reusedItem.TextLabel.SetResourceReference(Label.ForegroundProperty, SystemResources.GroupedSectionHeaderForegroundBrushKey);
                }
            }

            reusedItem.TextLabel.Text = obsec.HeaderTitle;
            return reusedItem;
        }
    }
}
