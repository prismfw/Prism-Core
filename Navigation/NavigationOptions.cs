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


namespace Prism
{
    /// <summary>
    /// Represents optional settings for customizing a navigation.
    /// </summary>
    public class NavigationOptions
    {
        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, to wait before displaying the load indicator.
        /// A negative or infinity value will disable the indicator for the navigation.
        /// </summary>
        public double LoadIndicatorDelay { get; set; }

        /// <summary>
        /// Gets or sets the title text for the indicator.
        /// </summary>
        public string LoadIndicatorTitle { get; set; }

        /// <summary>
        /// Gets any additional parameters that should be passed to the navigated controller.
        /// </summary>
        public NavigationParameterDictionary Parameters { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationOptions"/> class.
        /// </summary>
        public NavigationOptions()
        {
            LoadIndicatorDelay = UI.LoadIndicator.DefaultDelay;
            Parameters = new NavigationParameterDictionary();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public NavigationOptions Clone()
        {
            var clone = (NavigationOptions)MemberwiseClone();
            foreach (var key in Parameters.Keys)
            {
                clone.Parameters[key] = Parameters[key];
            }

            return clone;
        }
    }
}
