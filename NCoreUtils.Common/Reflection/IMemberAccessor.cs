using System.Reflection;

namespace NCoreUtils.Reflection
{
  /// <summary>
  /// Defines accessor extensions when the accessor represents a member - either field or property.
  /// </summary>
  public interface IMemberAccessor : IAccessor
  {
    /// <summary>
    /// Underlying member.
    /// </summary>
    MemberInfo MemberInfo { get; }
  }
}