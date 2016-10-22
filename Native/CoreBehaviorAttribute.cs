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

namespace Prism.Native
{
    /// <summary>
    /// Indicates the amount of work that the core framework library does with regards to a native API.
    /// This is for informational purposes only and is meant to assist developers implementing native interfaces by conveying
    /// what behaviors their implementations are responsible for versus what behaviors are already provided for them.
    /// It should be assumed that any behavior not explicitly specified is the responsibility of the implementation.  See Remarks.
    /// </summary>
    /// <remarks>
    /// When implementing a native interface, it is important to know what behaviors are expected of an implementation and what
    /// behaviors are provided automatically by the agnostic class that resides in the core library.  Some behaviors are always
    /// provided by the core library, some are only occasionally provided, and some are always expected to be a part of the native
    /// implementation.  Behaviors that are only occasionally provided are marked by this attribute.  If an interface member is
    /// decorated with this attribute, careful attention should be paid to the particular behaviors that are specified so as to
    /// avoid duplication of logic and ensure consistency across platforms.  Behaviors that are always provided by the core library
    /// or are always expected of an implementation are not explicitly marked, but developers should still keep them in mind when
    /// creating implementations.  An inexhaustive list of such behaviors is as follows:
    /// <para>
    /// - Native implementations should not throw exceptions or leave any thrown exceptions unhandled unless
    /// those exceptions are specifically mentioned by the interface through &lt;exception&gt; tags.
    /// </para>
    /// <para>
    /// - Methods or properties that accept instances of framework objects will always be given the native component of those objects.
    /// </para>
    /// <para>
    /// - Methods and property getters that return instances of framework objects can return the native component instead of having
    /// to retrieve the agnostic object.  The core library will ensure that the agnostic object is used when appropriate.
    /// </para>
    /// <para>
    /// - Unless specified, values given to property setters are not checked for inequality before being passed to the native implementation.
    /// The native implementation should check for inequality to avoid performing unnecessary logic or triggering inappropriate change notifications.
    /// </para>
    /// <para>
    /// - Any property that has a corresponding PropertyDescriptor in the agnostic class is expected to be bindable.
    /// Such properties should always trigger a change notification unless the notification is already triggered by the core library.
    /// This includes properties that only have getters.
    /// </para>
    /// <para>
    /// - Unless specified, change notifications should be triggered after any and all effects of the property change have been applied.
    /// Exceptions to this rule are redrawing and re-laying out content as well as applying any effects asynchronously, such as
    /// when loading images from an ImageBrush.  The implementation should not wait for asynchronous operations to complete before triggering
    /// the change notification, but rather it should trigger the notification as soon as all synchronous operations are completed.
    /// </para>
    /// <para>
    /// - Any property without a corresponding PropertyDescriptor in the agnostic class should not trigger a change notification.
    /// </para>
    /// <para>
    /// - Event handlers are provided and delegate values are assigned in the constructor of the agnostic class.
    /// Null checks for these members are unnecessary unless invoked prior to the completion of the agnostic constructor.
    /// </para>
    /// <para>
    /// - The Image of an ImageBrush will always be the agnostic object.  Native implementations that use brushes and are given an
    /// ImageBrush value need to retrieve the native component of the ImageSource object before using it.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class CoreBehaviorAttribute : Attribute
    {
        /// <summary>
        /// Gets the behaviors that are provided by the core library.
        /// </summary>
        public CoreBehaviors ProvidedBehaviors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreBehaviorAttribute"/> class.
        /// </summary>
        /// <param name="providedBehaviors">The behaviors that are provided by the core library.</param>
        public CoreBehaviorAttribute(CoreBehaviors providedBehaviors)
        {
            ProvidedBehaviors = providedBehaviors;
        }
    }

    /// <summary>
    /// Describes behaviors provided by the core library that are beyond what is typically provided.
    /// </summary>
    [Flags]
    public enum CoreBehaviors
    {
        /// <summary>
        /// No behaviors beyond what is normally provided.
        /// </summary>
        None = 0,
        /// <summary>
        /// The implementation is expected to be registered as a singleton.
        /// Failure to register the implementation as a singleton could lead to performance degradation.
        /// </summary>
        ExpectsSingleton = 1,
        /// <summary>
        /// Property values are checked for inequality before being passed to the native object.
        /// </summary>
        ChecksInequality = 2,
        /// <summary>
        /// Property values/method parameters are checked to ensure that they are not <c>null</c> before being passed to the native object.
        /// </summary>
        ChecksNullity = 4,
        /// <summary>
        /// Property values/method parameters are checked to ensure that they fall within a valid range before being passed to the native object.
        /// This includes checking for NaN and infinity values for properties and parameters of type <see cref="Double"/> and <see cref="Single"/>.
        /// </summary>
        ChecksRange = 8,
        /// <summary>
        /// Notifications for changes to the property value are triggered.
        /// </summary>
        TriggersChangeNotification = 16,
        /// <summary>
        /// Change notifications are expected to be triggered before any effects of the change are applied.
        /// </summary>
        ExpectsEarlyChangeNotification = 32,
        /// <summary>
        /// The visual object measures itself based on its content.
        /// Implementations of the <see cref="INativeVisual.Measure(Size)"/> method should simply return the constraints that are given to it.
        /// </summary>
        MeasuresByContent = 64
    }
}
