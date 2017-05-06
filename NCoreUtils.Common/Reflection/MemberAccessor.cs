using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NCoreUtils.Reflection
{
  /// <summary>
  /// Member accessor wrapper around property or field.
  /// </summary>
  public abstract class MemberAccessor : IMemberAccessor, IEquatable<MemberAccessor>
  {
    /// <summary>
    /// Member accessor wrapper around property.
    /// </summary>
    public class PropertyAccessor : MemberAccessor
    {
      /// <summary>
      /// Underlying property info.
      /// </summary>
      public PropertyInfo PropertyInfo { get; private set; }
      /// <summary>
      /// Initializes new instance of <see cref="T:NCoreUtils.Reflection.MemberAccessor+PropertyAccessor" />.
      /// </summary>
      /// <param name="propertyInfo">Target property info.</param>
      public PropertyAccessor(PropertyInfo propertyInfo)
      {
        RuntimeAssert.ArgumentNotNull(propertyInfo, nameof(propertyInfo));
        PropertyInfo = propertyInfo;
      }
      /// <summary>
      /// Gets a value indicating whether the underlying property and thus accessor can be read.
      /// </summary>
      public override bool CanRead => PropertyInfo.CanRead;
      /// <summary>
      /// Gets a value indicating whether the underlying property and thus accessor can be written to.
      /// </summary>
      public override bool CanWrite => PropertyInfo.CanWrite;
      /// <summary>
      /// Gets the accessor type of the actual accessor. Necessarily returns property option.
      /// </summary>
      public override AccessorType AccessorType => AccessorType.Property;
      /// <summary>
      /// Gets the downcasted value of <see cref="P:NCoreUtils.Reflection.MemberAccessor+PropertyAccessor.PropertyInfo" />.
      /// </summary>
      public override MemberInfo MemberInfo => PropertyInfo;
      /// <summary>
      /// Gets the underlying property type.
      /// </summary>
      public override Type TargetType => PropertyInfo.PropertyType;
      /// <summary>
      /// Gets the underlying property value of the specified object.
      /// </summary>
      /// <param name="instance">Object to use.</param>
      /// <returns>Underlying property value oth the specified object.</returns>
      public override object GetValue(object instance) => PropertyInfo.GetValue(instance, null);
      /// <summary>
      /// Sets the underlying property value of the specified object.
      /// </summary>
      /// <param name="instance">Object to use.</param>
      /// <param name="value">Value to set.</param>
      public override void SetValue(object instance, object value) => PropertyInfo.SetValue(instance, value, null);
    }
    /// <summary>
    /// Member accessor wrapper around field.
    /// </summary>
    public class FieldAccessor : MemberAccessor
    {
      /// <summary>
      /// Underlying property info.
      /// </summary>
      public FieldInfo FieldInfo { get; private set; }
      /// <summary>
      /// Initializes new instance of <see cref="T:NCoreUtils.Reflection.MemberAccessor+FieldAccessor" />.
      /// </summary>
      /// <param name="fieldInfo">Target field info.</param>
      public FieldAccessor(FieldInfo fieldInfo)
      {
        RuntimeAssert.ArgumentNotNull(fieldInfo, nameof(fieldInfo));
        FieldInfo = fieldInfo;
      }
      /// <summary>
      /// Gets a value indicating whether the underlying field and thus accessor can be read. Always <c>true</c>.
      /// </summary>
      public override bool CanRead => true;
      /// <summary>
      /// Gets a value indicating whether the underlying field and thus accessor can be written to. Field is considered
      /// writable if none of InitOnly and Literal field attributes are set.
      /// </summary>
      public override bool CanWrite
        => (FieldInfo.Attributes & (FieldAttributes.InitOnly | FieldAttributes.Literal)) == (FieldAttributes)0;
      /// <summary>
      /// Gets the accessor type of the actual accessor. Necessarily returns field option.
      /// </summary>
      public override AccessorType AccessorType => AccessorType.Field;
      /// <summary>
      /// Gets the downcasted value of <see cref="P:NCoreUtils.Reflection.MemberAccessor+FieldAccessor.FieldInfo" />.
      /// </summary>
      public override MemberInfo MemberInfo => FieldInfo;
      /// <summary>
      /// Gets the underlying field type.
      /// </summary>
      public override Type TargetType => FieldInfo.FieldType;
      /// <summary>
      /// Gets the underlying field value of the specified object.
      /// </summary>
      /// <param name="instance">Object to use.</param>
      /// <returns>Underlying field value oth the specified object.</returns>
      public override object GetValue(object instance) => FieldInfo.GetValue(instance);
      /// <summary>
      /// Sets the underlying field value of the specified object.
      /// </summary>
      /// <param name="instance">Object to use.</param>
      /// <param name="value">Value to set.</param>
      public override void SetValue(object instance, object value) => FieldInfo.SetValue(instance, value);
    }
    /// <summary>
    /// Creates new accessor from the specified field.
    /// </summary>
    /// <param name="fieldInfo">Field to use.</param>
    /// <returns>Accessor representing a field.</returns>
    public static MemberAccessor Create(FieldInfo fieldInfo) => new FieldAccessor(fieldInfo);
    /// <summary>
    /// Creates new accessor from the specified property.
    /// </summary>
    /// <param name="propertyInfo">Property to use.</param>
    /// <returns>Accessor representing a property.</returns>
    public static MemberAccessor Create(PropertyInfo propertyInfo) => new PropertyAccessor(propertyInfo);
    /// <summary>
    /// Gets the accessor type of the actual accessor.
    /// </summary>
    public abstract AccessorType AccessorType { get; }
    /// <summary>
    /// Gets a value indicating whether the accessor can be read.
    /// </summary>
    public abstract bool CanRead { get; }
    /// <summary>
    /// Gets a value indicating whether the accessor can be written to.
    /// </summary>
    public abstract bool CanWrite { get; }
    /// <summary>
    /// Gets the class that declares the represented member.
    /// </summary>
    public Type DeclaringType => MemberInfo.DeclaringType;
    /// <summary>
    /// Gets the default value for the represented member, if any.
    /// </summary>
    public object DefaultValue
      => GetCustomAttributes(typeof(DefaultValueAttribute), true)
          .OfType<DefaultValueAttribute>()
          .Select(a => a.Value).FirstOrDefault();
    /// <summary>
    /// Gets whether the represented member has default value.
    /// </summary>
    public bool HasDefaultValue => IsDefined(typeof(DefaultValueAttribute), true);
    /// <summary>
    /// Gets the underlying member.
    /// </summary>
    public abstract MemberInfo MemberInfo { get; }
    /// <summary>
    /// Gets the name of the underlying member.
    /// </summary>
    public string Name => MemberInfo.Name;
    /// <summary>
    /// Gets the type of the represented member.
    /// </summary>
    public abstract Type TargetType { get; }
    /// <summary>
    /// Determines whether two instances of <see cref="T:NCoreUtils.Reflection.MemberAccessor" /> are equal.
    /// </summary>
    /// <param name="that">Instance to compare to the actual.</param>
    /// <returns>
    /// <c>true</c> if both instances represent the same member accessor, <c>false</c> otherwise.
    /// </returns>
    public bool Equals(MemberAccessor that) => MemberInfo.Equals(that.MemberInfo);
    /// <summary>
    /// Determines whether the specified object and the actual instance of
    /// <see cref="T:NCoreUtils.Reflection.MemberAccessor" /> are equal.
    /// </summary>
    /// <param name="other">Object to compare to the actual.</param>
    /// <returns>
    /// <c>true</c> if both instances represent the same member accessor, <c>false</c> otherwise.
    /// </returns>
    public override bool Equals(object other)
    {
      var that = other as MemberAccessor;
      return that != null && Equals(that);
    }
    /// <summary>
    /// Returns the hash code for the current <see cref="T:NCoreUtils.Reflection.MemberAccessor" /> instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => MemberInfo.GetHashCode();
    /// <summary>
    /// Returns an array of all custom attributes applied to this member.
    /// </summary>
    /// <param name="inherit">
    /// Normally <c>true</c> to search this member's inheritance chain to find the attributes; otherwise, <c>false</c>.
    /// This parameter is ignored as it is ignored both on fields and properties.
    /// </param>
    /// <returns>
    /// An array that contains all the custom attributes applied to this member, or an array with zero elements if no
    /// attributes are defined.
    /// </returns>
    public object[] GetCustomAttributes(bool inherit)
      => ((ICustomAttributeProvider)MemberInfo).GetCustomAttributes(inherit);
    /// <summary>
    /// Returns an array of custom attributes applied to this member and identified by the specified type.
    /// </summary>
    /// <param name="attributeType">
    /// The type of attribute to search for. Only attributes that are assignable to this type are returned.
    /// </param>
    /// <param name="inherit">
    /// Normally <c>true</c> to search this member's inheritance chain to find the attributes; otherwise, <c>false</c>.
    /// This parameter is ignored as it is ignored both on fields and properties.
    /// </param>
    /// <returns>
    /// An array of custom attributes applied to this member, or an array with zero elements if no attributes
    /// assignable to <paramref name="attributeType" /> have been applied.
    /// </returns>
    public object[] GetCustomAttributes(Type attributeType, bool inherit)
      => ((ICustomAttributeProvider)MemberInfo).GetCustomAttributes(attributeType, inherit);
    /// <summary>
    /// Indicates whether one or more attributes of the specified type or of its derived types is applied to this
    /// member.
    /// </summary>
    /// <param name="attributeType">
    /// The type of custom attribute to search for. The search includes derived types.
    /// </param>
    /// <param name="inherit">
    /// Normally <c>true</c> to search this member's inheritance chain to find the attributes; otherwise, <c>false</c>.
    /// This parameter is ignored as it is ignored both on fields and properties.
    /// </param>
    /// <returns>
    /// <c>true</c> if one or more instances of <paramref name="attributeType" /> or any of its derived types is
    /// applied to this member; otherwise, <c>false</c>.
    /// </returns>
    public bool IsDefined(Type attributeType, bool inherit)
      => ((ICustomAttributeProvider)MemberInfo).IsDefined(attributeType, inherit);
    /// <summary>
    /// Gets the underlying property or field value of the specified object.
    /// </summary>
    /// <param name="instance">Object to use.</param>
    /// <returns>Underlying property or field value oth the specified object.</returns>
    public abstract object GetValue(object instance);
    /// <summary>
    /// Sets the underlying property or field value of the specified object.
    /// </summary>
    /// <param name="instance">Object to use.</param>
    /// <param name="value">Value to set.</param>
    public abstract void SetValue(object instance, object value);
  }
}