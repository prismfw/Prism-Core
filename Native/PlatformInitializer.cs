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
using System.Globalization;
using System.Linq;
using System.Reflection;
using Prism.IO;
using Prism.Systems;
using Prism.Utilities;

namespace Prism.Native
{
    /// <summary>
    /// Represents the base class for native platform initializers.  This class is abstract.
    /// </summary>
    public abstract class PlatformInitializer
    {
        /// <summary>
        /// Gets a value indicating whether the platform initializer has successfully initialized.
        /// </summary>
        protected static bool HasInitialized { get; private set; }

        /// <summary>
        /// Initializes the platform and loads the specified <see cref="Application"/> instance.
        /// </summary>
        /// <param name="appInstance">The application instance to be loaded.</param>
        /// <param name="appAssemblies">Every loaded assembly that is not a system assembly.  These are scanned for types that should be registered with the IoC container.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="appInstance"/> is <c>null</c>.</exception>
        protected static void Initialize(Application appInstance, Assembly[] appAssemblies)
        {
            if (appInstance == null)
            {
                throw new ArgumentNullException(nameof(appInstance));
            }

            if (appAssemblies != null)
            {
                foreach (var type in appAssemblies.SelectMany(a => a.ExportedTypes))
                {
                    var info = type.GetTypeInfo();
                    var regAtts = info.GetCustomAttributes<RegisterAttribute>(false);
                    foreach (var att in regAtts)
                    {
                        if (att.IsSingleton)
                        {
                            TypeManager.Default.RegisterSingleton(att.RegisterType, att.Name, type, att.InitializeMethod,
                                att.IsProtected ? TypeRegistrationOptions.Protect : TypeRegistrationOptions.None);
                        }
                        else
                        {
                            TypeManager.Default.Register(att.RegisterType, att.Name, type, att.InitializeMethod,
                                att.IsProtected ? TypeRegistrationOptions.Protect : TypeRegistrationOptions.None);
                        }
                    }

                    ControllerManager.Default.Register(info, type);
                    ViewManager.Default.Register(info, type);
                }

                if (Logger.LogFilePath == null)
                {
                    Logger.LogFilePath = System.IO.Path.Combine(Directory.DataDirectory, "Logs", DateTime.Today.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture) + ".log");
                }

#if DEBUG
                Logger.SetMinimumConsoleOutputLevel(LoggingLevel.Trace);
#endif

                Logger.Info(CultureInfo.CurrentCulture, Resources.Strings.PlatformInitialized, Device.Current.OperatingSystem, Device.Current.OSVersion, Device.Current.Model);
                Logger.Debug(CultureInfo.CurrentCulture, "Running in DEBUG mode.");
                Logger.Info(CultureInfo.CurrentCulture, TypeManager.Default.RegisteredCount == 1 ? Resources.Strings.RegisteredTypesToIoCContainerSingular : Resources.Strings.RegisteredTypesToIoCContainerPlural, TypeManager.Default.RegisteredCount);
                Logger.Info(CultureInfo.CurrentCulture, ControllerManager.Default.RegisteredCount == 1 ? Resources.Strings.FoundControllersWithNavAttributesSingular : Resources.Strings.FoundControllersWithNavAttributesPlural, ControllerManager.Default.RegisteredCount);
                Logger.Info(CultureInfo.CurrentCulture, ViewManager.Default.RegisteredCount == 1 ? Resources.Strings.FoundViewsWithPerspectivesSingular : Resources.Strings.FoundViewsWithPerspectivesPlural, ViewManager.Default.RegisteredCount);
            }

            Application.Initialize(appInstance);
            HasInitialized = true;
        }
    }
}