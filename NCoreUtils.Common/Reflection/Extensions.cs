using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NCoreUtils.Reflection
{
  /// <summary>
  /// Common reflection extensions.
  /// </summary>
  public static class Extensions
  {
    /// <summary>
    /// Tries to retrieve first attribute of the specified type.
    /// </summary>
    /// <param name="type">Attribute source.</param>
    /// <param name="attribute">Variable tp store retrieved attribute.</param>
    /// <returns>
    /// <c>true</c> if attribute has been retrieved and stored in the specified variable, <c>false</c> otherwise.
    /// </returns>
    public static bool TryGetAttribute<T>(this Type type, out T attribute) where T : Attribute
    {
      attribute = type.GetTypeInfo().GetCustomAttributes<T>().FirstOrDefault();
      return null != attribute;
    }
    /// <summary>
    /// Tries to retrieve first attribute of the specified type.
    /// </summary>
    /// <param name="member">Attribute source.</param>
    /// <param name="attribute">Variable tp store retrieved attribute.</param>
    /// <returns>
    /// <c>true</c> if attribute has been retrieved and stored in the specified variable, <c>false</c> otherwise.
    /// </returns>
    public static bool TryGetAttribute<T>(this MemberInfo member, out T attribute) where T : Attribute
    {
      attribute = member.GetCustomAttributes<T>().FirstOrDefault();
      return null != attribute;
    }
    /// <summary>
    /// Tries to retrieve first attribute of the specified type.
    /// </summary>
    /// <param name="accessor">Attribute source.</param>
    /// <param name="attribute">Variable tp store retrieved attribute.</param>
    /// <returns>
    /// <c>true</c> if attribute has been retrieved and stored in the specified variable, <c>false</c> otherwise.
    /// </returns>
    public static bool TryGetAttribute<T>(this IAccessor accessor, out T attribute) where T : Attribute
    {
      attribute = accessor.GetCustomAttributes<T>().FirstOrDefault();
      return null != attribute;
    }
    /// <summary>
    /// Retrieves all member accessors for specified type with respect to <paramref name="bindingAttr" />.
    /// </summary>
    /// <param name="type">Member source.</param>
    /// <param name="bindingAttr">Binding flags.</param>
    /// <returns>Sequence of retrieved member accessors.</returns>
    public static IEnumerable<MemberAccessor> GetAccessors(this Type type, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
    {
      return type.GetTypeInfo()
        .GetMembers(bindingAttr)
        .MaybeChoose(member => {
          switch (member)
          {
            case PropertyInfo property:
              return MemberAccessor.Create(property).AsMaybe();
            case FieldInfo field:
              return MemberAccessor.Create(field).AsMaybe();
            default:
              return Maybe.Empty;
          }
        });
    }
  }
}