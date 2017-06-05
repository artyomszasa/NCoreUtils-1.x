using System;

namespace NCoreUtils.Serialization.Meta
{
    /// <summary>
    /// Overrides member inheritance for the target class or struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class MemberInheritanceAttribute : Attribute
    {
        /// <summary>
        /// Overridden member inheritance.
        /// </summary>
        public readonly MemberInheritance MemberInheritance;
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.Meta.MemberInheritanceAttribute" />.
        /// </summary>
        /// <param name="memberInheritance">Member inheritance to use.</param>
        public MemberInheritanceAttribute(MemberInheritance memberInheritance)
        {
            MemberInheritance = memberInheritance;
        }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.Meta.MemberInheritanceAttribute" />
        /// with default member inheritance.
        /// </summary>
        public MemberInheritanceAttribute() : this(MemberInheritance.Default) { }
    }
}