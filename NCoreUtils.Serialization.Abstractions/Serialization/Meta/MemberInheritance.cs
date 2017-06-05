namespace NCoreUtils.Serialization.Meta
{
    /// <summary>
    /// Defines how to handle inherited members.
    /// </summary>
    public enum MemberInheritance
    {
        /// Serialization dependant functionality (default).
        Default = 0,
        /// <summary>
        /// Members are selected through the selector associated with
        /// the target type. Selecting inherited members may be controlled
        /// by using <see cref="T:NCoreUtils.Serialization.DefaultAccessSelector" />.
        /// </summary>
        None = 1,
        /// <summary>
        /// Members are selected both through the selector associated with
        /// the target type and its base type. Same accessors are safe to be
        /// selected multiple times.
        /// </summary>
        Inherited = 2
    }
}