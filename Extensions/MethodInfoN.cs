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


using System.Diagnostics.CodeAnalysis;

#if !DEBUG
using System.Diagnostics;
#endif

namespace System.Reflection
{
    /// <summary>
    /// Discovers the attributes of a method and provides access to method metadata.
    /// </summary>
    public sealed class MethodInfoN : MemberInfoN
    {
        /// <summary>
        /// Gets the attributes associated with this method.
        /// </summary>
        public MethodAttributes Attributes
        {
            get { return methodInfo.Attributes; }
        }

        /// <summary>
        /// Gets a value indicating the calling conventions for this method.
        /// </summary>
        public CallingConventions CallingConvention
        {
            get { return methodInfo.CallingConvention; }
        }

        /// <summary>
        /// Gets a value indicating whether the generic method contains unassigned generic type parameters.
        /// </summary>
        public bool ContainsGenericParameters
        {
            get { return methodInfo.ContainsGenericParameters; }
        }

        /// <summary>
        /// Gets a value indicating whether the method is abstract.
        /// </summary>
        public bool IsAbstract
        {
            get { return methodInfo.IsAbstract; }
        }

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this method or constructor
        /// is described by <see cref="MethodAttributes.Assembly"/>; that is, the method
        /// or constructor is visible at most to other types in the same assembly, and is
        /// not visible to derived types outside the assembly.
        /// </summary>
        public bool IsAssembly
        {
            get { return methodInfo.IsAssembly; }
        }

        /// <summary>
        /// Gets a value indicating whether the method is a constructor.
        /// </summary>
        public bool IsConstructor
        {
            get { return methodInfo.IsConstructor; }
        }

        /// <summary>
        /// Gets a value indicating whether the visibility of this method or constructor
        /// is described by <see cref="MethodAttributes.Family"/>; that is, the method
        /// or constructor is visible only within its class and derived classes.
        /// </summary>
        public bool IsFamily
        {
            get { return methodInfo.IsFamily; }
        }

        /// <summary>
        /// Gets a value indicating whether the visibility of this method or constructor
        /// is described by <see cref="MethodAttributes.FamANDAssem"/>; that is, the
        /// method or constructor can be called by derived classes, but only if they are
        /// in the same assembly.
        /// </summary>
        public bool IsFamilyAndAssembly
        {
            get { return methodInfo.IsFamilyAndAssembly; }
        }

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this method or constructor
        /// is described by <see cref="MethodAttributes.FamORAssem"/>; that is, the method
        /// or constructor can be called by derived classes wherever they are, and by classes
        /// in the same assembly.
        /// </summary>
        public bool IsFamilyOrAssembly
        {
            get { return methodInfo.IsFamilyOrAssembly; }
        }

        /// <summary>
        /// Gets a value indicating whether this method is final.
        /// </summary>
        public bool IsFinal
        {
            get { return methodInfo.IsFinal; }
        }

        /// <summary>
        /// Gets a value indicating whether the method is generic.
        /// </summary>
        public bool IsGenericMethod
        {
            get { return methodInfo.IsGenericMethod; }
        }

        /// <summary>
        /// Gets a value indicating whether the method is a generic method definition.
        /// </summary>
        public bool IsGenericMethodDefinition
        {
            get { return methodInfo.IsGenericMethodDefinition; }
        }

        /// <summary>
        /// Gets a value indicating whether only a member of the same kind with exactly the same signature is hidden in the derived class.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sig", Justification = "Matching API with .NET class.")]
        public bool IsHideBySig
        {
            get { return methodInfo.IsHideBySig; }
        }

        /// <summary>
        /// Gets a value indicating whether this member is private.
        /// </summary>
        public bool IsPrivate
        {
            get { return methodInfo.IsPrivate; }
        }

        /// <summary>
        /// Gets a value indicating whether this is a public method.
        /// </summary>
        public bool IsPublic
        {
            get { return methodInfo.IsPublic; }
        }

        /// <summary>
        /// Gets a value indicating whether the method is static.
        /// </summary>
        public bool IsStatic
        {
            get { return methodInfo.IsStatic; }
        }

        /// <summary>
        /// Gets a value indicating whether the method is virtual.
        /// </summary>
        public bool IsVirtual
        {
            get { return methodInfo.IsVirtual; }
        }

        /// <summary>
        /// Gets the <see cref="MethodImplAttributes"/> flags that specify the attributes of a method implementation.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Justification = "Matching API with .NET class.")]
        public MethodImplAttributes MethodImplementationFlags
        {
            get { return methodInfo.MethodImplementationFlags; }
        }

        /// <summary>
        /// Gets a <see cref="ParameterInfo"/> object that contains information about
        /// the return type of the method, such as whether the return type has custom modifiers.
        /// </summary>
        public ParameterInfo ReturnParameter
        {
            get { return methodInfo.ReturnParameter; }
        }

        /// <summary>
        /// Gets the return type of this method.
        /// </summary>
        public Type ReturnType
        {
            get { return methodInfo.ReturnType; }
        }

#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private readonly MethodInfo methodInfo;

        internal MethodInfoN(MethodInfo methodInfo)
            : base(methodInfo)
        {
            this.methodInfo = methodInfo;
        }

        /// <summary>
        /// Creates a delegate of the specified type from this method.
        /// </summary>
        /// <param name="delegateType">The type of the delegate to create.</param>
        /// <returns>The delegate for this method.</returns>
        public Delegate CreateDelegate(Type delegateType)
        {
            return methodInfo.CreateDelegate(delegateType);
        }

        /// <summary>
        /// Creates a delegate of the specified type with the specified target from this method.
        /// </summary>
        /// <param name="delegateType">The type of the delegate to create.</param>
        /// <param name="target">The object targeted by the delegate.</param>
        /// <returns>The delegate for this method.</returns>
        public Delegate CreateDelegate(Type delegateType, object target)
        {
            return methodInfo.CreateDelegate(delegateType, GetObject(target));
        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance, or <c>null</c>.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> equals the type and value of this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return methodInfo.Equals(obj);
        }

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the type arguments of
        /// a generic method or the type parameters of a generic method definition.
        /// </summary>
        /// <returns>
        /// An array of <see cref="Type"/> objects that represent the type arguments of a generic
        /// method or the type parameters of a generic method definition. Returns an empty
        /// array if the current method is not a generic method.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// The current object is a constructor. Generic constructors are not supported in the .NET Framework version 2.0.
        /// This exception is the default behavior if this method is not overridden in a derived class.
        /// </exception>
        public Type[] GetGenericArguments()
        {
            return methodInfo.GetGenericArguments();
        }

        /// <summary>
        /// Returns a <see cref="MethodInfoN"/> object that represents a generic method definition from which the current method can be constructed.
        /// </summary>
        /// <returns>A <see cref="MethodInfoN"/> object representing a generic method definition from which the current method can be constructed</returns>
        /// <exception cref="InvalidOperationException">The current method is not a generic method.</exception>
        /// <exception cref="NotSupportedException">The method is not supported.</exception>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Matching API with .NET class.")]
        public MethodInfoN GetGenericMethodDefinition()
        {
            return new MethodInfoN(methodInfo.GetGenericMethodDefinition());
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return methodInfo.GetHashCode();
        }

        /// <summary>
        /// Retrieves an object that represents the specified method on the direct or indirect base class where the method was first declared.
        /// </summary>
        /// <returns>An object that represents the specified method's initial declaration on a base class.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Matching API with .NET class.")]
        public MethodInfoN GetRuntimeBaseDefinition()
        {
            var baseDef = methodInfo.GetRuntimeBaseDefinition();
            return baseDef == methodInfo ? this : new MethodInfoN(baseDef);
        }

        /// <summary>
        /// When overridden in a derived class, gets the parameters of the specified method or constructor.
        /// </summary>
        /// <returns>
        /// An array of type <see cref="ParameterInfo"/> containing information that matches the signature
        /// of the method (or constructor) reflected by this instance.
        /// </returns>
        public ParameterInfo[] GetParameters()
        {
            return methodInfo.GetParameters();
        }

        /// <summary>
        /// Invokes the method or constructor represented by the current instance, using the specified parameters.
        /// </summary>
        /// <param name="obj">
        /// The object on which to invoke the method or constructor. If a method is static, this argument is ignored.
        /// If a constructor is static, this argument must be null or an instance of the class that defines the constructor.
        /// </param>
        /// <param name="parameters">
        /// An argument list for the invoked method or constructor. This is an array of objects with the same number,
        /// order, and type as the parameters of the method or constructor to be invoked. If there are no parameters,
        /// <paramref name="parameters"/> should be null.If the method or constructor represented by this instance takes
        /// a ref parameter (ByRef in Visual Basic), no special attribute is required for that parameter in order to invoke
        /// the method or constructor using this function. Any object in this array that is not explicitly initialized with
        /// a value will contain the default value for that object type. For reference-type elements, this value is <c>null</c>.
        /// For value-type elements, this value is <c>0</c>, <c>0.0</c>, or <c>false</c>, depending on the specific element type.
        /// </param>
        /// <returns>
        /// An object containing the return value of the invoked method, or null in the case of a constructor.
        /// Caution: Elements of the parameters array that represent parameters declared with the ref or out keyword may also be modified.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The elements of the <paramref name="parameters"/> array do not match the signature of the method or constructor reflected
        /// by this instance.
        /// </exception>
        /// <exception cref="Exception">
        /// The <paramref name="obj"/> parameter is <c>null</c> and the method is not static -or- the method is not declared
        /// or inherited by the class of <paramref name="obj"/> -or- a static constructor is invoked, and <paramref name="obj"/>
        /// is neither <c>null</c> nor an instance of the class that declared the constructor.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The type that declares the method is an open generic type. That is, the <see cref="TypeInfo.ContainsGenericParameters"/>
        /// property returns true for the declaring type.
        /// </exception>
        /// <exception cref="MemberAccessException">
        /// The caller does not have permission to execute the method or constructor that is represented by the current instance.
        /// </exception>
        /// <exception cref="TargetInvocationException">
        /// The invoked method or constructor throws an exception.
        /// </exception>
        /// <exception cref="TargetParameterCountException">
        /// The <paramref name="parameters"/> array does not have the correct number of arguments.
        /// </exception>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "obj", Justification = "Matching parameter name with .NET method.")]
        public object Invoke(object obj, object[] parameters)
        {
            return methodInfo.Invoke(GetObject(obj), parameters);
        }

        /// <summary>
        /// Substitutes the elements of an array of types for the type parameters of the current generic method definition,
        /// and returns a <see cref="MethodInfoN"/> object representing the resulting constructed method.
        /// </summary>
        /// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic method definition.</param>
        /// <returns>
        /// A <see cref="MethodInfoN"/> object that represents the constructed method formed by substituting the elements
        /// of <paramref name="typeArguments"/> for the type parameters of the current generic method definition.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// The number of elements in <paramref name="typeArguments"/> is not the same as the number of type
        /// parameters of the current generic method definition -or- an element of <paramref name="typeArguments"/>
        /// does not satisfy the constraints specified for the corresponding type parameter of the current generic method definition.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="typeArguments"/> is <c>null</c> -or- any element of <paramref name="typeArguments"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The current <see cref="MethodInfoN"/> does not represent a generic method definition.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The method is not supported.
        /// </exception>
        public MethodInfoN MakeGenericMethod(params Type[] typeArguments)
        {
            return new MethodInfoN(methodInfo.MakeGenericMethod(typeArguments));
        }
    }
}
