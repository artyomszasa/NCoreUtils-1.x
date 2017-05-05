using System;
using System.ComponentModel;
using System.Globalization;

namespace NCoreUtils
{
  /// <summary>
  /// Custom converter for converting instances of <see cref="NCoreUtils.CaseInsensitive"/> to/from string.
  /// </summary>
  public sealed class CaseInsensitiveTypeConverer : TypeConverter
  {
    /// <summary>
    /// Returns whether this converter can convert an object of the given type to the type of this converter. 
    /// </summary>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    /// <summary>
    /// Returns whether this converter can convert the object to the specified type. 
    /// </summary>
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      return destinationType == typeof(CaseInsensitive) || base.CanConvertTo(context, destinationType);
    }

    /// <summary>
    /// Converts the given object to the type of this converter, using the specified context and culture information.
    /// </summary>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      if (null == value)
      {
        return CaseInsensitive.Empty;
      }
      var svalue = value as string;
      if (null != svalue)
      {
        return new CaseInsensitive(svalue);
      }
      return base.ConvertFrom(context, culture, value);
    }

    /// <summary>
    /// Converts the given value object to the specified type, using the specified context and culture information.
    /// </summary>
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (typeof(string) == destinationType && value is CaseInsensitive)
      {
        return ((CaseInsensitive)value).ToLowerString(culture);
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }
  }
}