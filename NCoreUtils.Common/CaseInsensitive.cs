using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NCoreUtils
{
  /// <summary>
  /// Represents case-insensitive string.
  /// </summary>
  public struct CaseInsensitive : IComparable, IConvertible, IEquatable<CaseInsensitive>, IComparable<CaseInsensitive>
  {
    const string KeyValue = "Value";

    static readonly CultureInfo _invCulture = CultureInfo.InvariantCulture;
    static readonly StringComparer _invComparer;
    static readonly Dictionary<Type, Func<CaseInsensitive, IFormatProvider, object>> _toType = new Dictionary<Type, Func<CaseInsensitive, IFormatProvider, object>>();

    static CaseInsensitive()
    {
      var cci = CultureInfo.CurrentCulture;
      #if NET_CORE
      CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
      _invComparer = StringComparer.CurrentCultureIgnoreCase;
      if (null != cci)
      {
        CultureInfo.CurrentCulture = cci;
      }
      #else
      _invComparer = StringComparer.InvariantCultureIgnoreCase;
      #endif
      _toType.Add(typeof(bool), (ci, provider) => ((IConvertible)ci).ToBoolean(provider));
      _toType.Add(typeof(sbyte), (ci, provider) => ((IConvertible)ci).ToSByte(provider));
      _toType.Add(typeof(byte), (ci, provider) => ((IConvertible)ci).ToByte(provider));
      _toType.Add(typeof(short), (ci, provider) => ((IConvertible)ci).ToInt16(provider));
      _toType.Add(typeof(ushort), (ci, provider) => ((IConvertible)ci).ToUInt16(provider));
      _toType.Add(typeof(int), (ci, provider) => ((IConvertible)ci).ToInt32(provider));
      _toType.Add(typeof(uint), (ci, provider) => ((IConvertible)ci).ToUInt32(provider));
      _toType.Add(typeof(long), (ci, provider) => ((IConvertible)ci).ToInt64(provider));
      _toType.Add(typeof(ulong), (ci, provider) => ((IConvertible)ci).ToUInt64(provider));
      _toType.Add(typeof(float), (ci, provider) => ((IConvertible)ci).ToSingle(provider));
      _toType.Add(typeof(double), (ci, provider) => ((IConvertible)ci).ToDouble(provider));
      _toType.Add(typeof(decimal), (ci, provider) => ((IConvertible)ci).ToDecimal(provider));
      _toType.Add(typeof(DateTime), (ci, provider) => ((IConvertible)ci).ToDateTime(provider));
      _toType.Add(typeof(string), (ci, provider) => ((IConvertible)ci).ToString(provider));
    }

    /// <summary>
    /// Empty case-insensitive string.
    /// </summary>
    public static CaseInsensitive Empty { get; } = new CaseInsensitive(string.Empty);

    /// <summary>
    /// Preferred way to create case-insensitive strings.
    /// </summary>
    /// <param name="value">Underlying case-sensitive value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CaseInsensitive Create(string value) => new CaseInsensitive(value ?? string.Empty);

    /// <summary>
    /// Concatenates the members of a constructed <see cref="System.Collections.Generic.IEnumerable{T}"/> collection
    /// of type <see cref="T:NCoreUtils.CaseInsensitive"/>.
    /// </summary>
    public static CaseInsensitive Concat(IEnumerable<CaseInsensitive> values)
    {
      return CaseInsensitive.Create(string.Concat(values.Select(v => v.Value)));
    }

    /// Read-only case-sensitive value;
    readonly string _value;

    /// <summary>
    /// Underlying case-sensitive string. Ensures non-null value returned.
    /// </summary>
    public string Value
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get { return _value ?? string.Empty; }
    }

    /// <summary>
    /// Gets the number of characters in the current <see cref="T:NCoreUtils.CaseInsensitive"/> object.
    /// </summary>
    public int Length
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get { return Value.Length; }
    }

    /// <summary>
    /// Gets the <see cref="T:System.Char"/> object at a specified position in the current <see cref="T:NCoreUtils.CaseInsensitive"/> object.
    /// </summary>
    [IndexerName("Chars")]
    public char this[int index]
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get { return Value[index]; }
    }

    /// <summary>
    /// Constructs new case-insensitive string.
    /// </summary>
    /// <param name="value">Value.</param>
    public CaseInsensitive(string value)
    {
      _value = value;
    }

    /// <summary>
    /// Returns a value indicating whether a specified case-insensitive substring occurs within this case-insensitive
    /// string.
    /// </summary>
    public bool Contains(CaseInsensitive value)
    {
      return -1 != Value.IndexOf(value.Value, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    /// Reports the zero-based index of the first occurrence of the specified Unicode character in this string.
    /// Comparison performed in case-insensitive manner.
    /// </summary>
    /// <param name="value">A Unicode character to seek.</param>
    /// <returns>The zero-based index position of value if that character is found, or  <c>-1</c> if it is not.</returns>
    public int IndexOf(char value)
    {
      // Ötlet: https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/String.Comparison.cs
      var i = 0;
      var uvalue = (uint)(value - 'a') <= (uint)('z' - 'a') ? (uint)value - 0x20 : (uint)value;
      foreach (var ch in Value)
      {
        var uch = (uint)(ch - 'a') <= (uint)('z' - 'a') ? (uint)ch - 0x20 : (uint)ch;
        if (uch == uvalue)
        {
          return i;
        }
        ++i;
      }
      return -1;
    }

    /// <summary>
    /// Returns the lowercased underlying string.
    /// </summary>
    /// <param name="cultureInfo">Culture info to use.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToLowerString(CultureInfo cultureInfo)
    {
      return cultureInfo.TextInfo.ToLower(Value);
    }

    /// <summary>
    /// Returns the uppercased underlying string.
    /// </summary>
    /// <param name="cultureInfo">Culture info to use.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToUpperString(CultureInfo cultureInfo)
    {
      return cultureInfo.TextInfo.ToUpper(Value);
    }

    /// <summary>
    /// Returns the lowercased underlying string (invariant culture).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToLowerString() => ToLowerString(_invCulture);

    /// <summary>
    /// Returns the uppercased underlying string (invariant culture).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToUpperString() => ToUpperString(_invCulture);

    /// <summary>
    /// Implicit string conversion operator.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator CaseInsensitive(string value) => Create(value);

    #region stringify

    /// <summary>
    /// Customized ToString method.
    /// </summary>
    public override string ToString() => $"CI({ToLowerString()})";

    #endregion

    #region equality

    /// <summary>
    /// Determines whether the actual instance of <see cref="T:NCoreUtils.CaseInsensitive"/> is
    /// equal to the specified instance of <see cref="T:NCoreUtils.CaseInsensitive"/>. Two
    /// instances are considered equal if underlying values of the instances are equal with
    /// no respect for case-sensitivity or culture.
    /// </summary>
    /// <param name="that">
    /// Instance of <see cref="T:NCoreUtils.CaseInsensitive"/> to be compared against the actual one.
    /// </param>
    /// <returns>
    /// <c>true</c> if the instances are equal; <c>false</c> otherwise.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(CaseInsensitive that) => _invComparer.Equals(Value, that.Value);

    /// <summary>
    /// Determines whether the actual instance of <see cref="T:NCoreUtils.CaseInsensitive"/> is
    /// equal to the specified object. Two instances are considered equal if underlying values of
    /// the instances are equal with no respect for case-sensitivity or culture. If the specified
    /// object is not an instance of <see cref="T:NCoreUtils.CaseInsensitive"/> the result of
    /// the comparison is always <c>false</c>.
    /// </summary>
    /// <param name="obj">
    /// Object to be compared against the actual one.
    /// </param>
    /// <returns>
    /// <c>true</c> if the instances are equal; <c>false</c> otherwise.
    /// </returns>
    public override bool Equals(object obj)
    {
      if (null == obj)
      {
        return false;
      }
      if (obj is CaseInsensitive)
      {
        var that = (CaseInsensitive)obj;
        return Equals(that);
      }
      return false;
    }

    /// <summary>
    /// Provides hasing function for the actual instance of <see cref="T:NCoreUtils.CaseInsensitive"/>.
    /// Hash is computed  with no respect for case-sensitivity or culture.
    /// </summary>
    public override int GetHashCode() => _invComparer.GetHashCode(Value);

    /// <summary>
    /// Determines whether the first instance of <see cref="T:NCoreUtils.CaseInsensitive"/> is
    /// equal to the second instance of <see cref="T:NCoreUtils.CaseInsensitive"/>. Two
    /// instances are considered equal if underlying values of the instances are equal with
    /// no respect for case-sensitivity or culture.
    /// </summary>
    /// <param name="a">
    /// Instance of <see cref="T:NCoreUtils.CaseInsensitive"/> to be compared against.
    /// </param>
    /// <param name="b">
    /// Instance of <see cref="T:NCoreUtils.CaseInsensitive"/> to be compared against the first one.
    /// </param>
    /// <returns>
    /// <c>true</c> if the instances are equal; <c>false</c> otherwise.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(CaseInsensitive a, CaseInsensitive b) => a.Equals(b);

    /// <summary>
    /// Determines whether the first instance of <see cref="T:NCoreUtils.CaseInsensitive"/> is
    /// inequal to the second instance of <see cref="T:NCoreUtils.CaseInsensitive"/>. Two
    /// instances are considered equal if underlying values of the instances are equal with
    /// no respect for case-sensitivity or culture.
    /// </summary>
    /// <param name="a">
    /// Instance of <see cref="T:NCoreUtils.CaseInsensitive"/> to be compared against.
    /// </param>
    /// <param name="b">
    /// Instance of <see cref="T:NCoreUtils.CaseInsensitive"/> to be compared against the first one.
    /// </param>
    /// <returns>
    /// <c>false</c> if the instances are equal; <c>true</c> otherwise.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(CaseInsensitive a, CaseInsensitive b) => !a.Equals(b);

    #endregion

    #region Comparison

    /// <summary>
    /// Compares the actual instance of <see cref="T:NCoreUtils.CaseInsensitive"/> to
    /// the specified instance of <see cref="T:NCoreUtils.CaseInsensitive"/>. Compares the underlying
    /// values of the instances with no respect for case-sensitivity or culture.
    /// </summary>
    /// <param name="that">
    /// Instance of <see cref="T:NCoreUtils.CaseInsensitive"/> to be compared against the actual one.
    /// </param>
    /// <returns>
    /// <c>1</c> if the specfied instances are greater than the actual one; <c>-1</c> if less; <c>0</c> otherwise.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int CompareTo(CaseInsensitive that) => _invComparer.Compare(Value, that.Value);

    /// <summary>
    /// Compares the actual instance of <see cref="T:NCoreUtils.CaseInsensitive"/> to
    /// the specified object. Compares the underlying values of the instances with no respect
    /// for case-sensitivity or culture. Throws if the specified object is not an instance of
    /// <see cref="T:NCoreUtils.CaseInsensitive"/>.
    /// </summary>
    /// <param name="other">
    /// Object to be compared against the actual one.
    /// </param>
    /// <returns>
    /// <c>1</c> if the specfied instances are greater than the actual one; <c>-1</c> if less; <c>0</c> otherwise.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">
    /// Thrown if the specified object is not an instance of <see cref="T:NCoreUtils.CaseInsensitive"/>.
    /// </exception>
    public int CompareTo(object other)
    {
      if (!(other is CaseInsensitive))
      {
        throw new InvalidOperationException("Uncomparable");
      }
      return CompareTo((CaseInsensitive)other);
    }

    #endregion

    #region IConvertible

    /// <summary>
    /// Returns object type code.
    /// </summary>
    /// <returns>Object type code.</returns>
    TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

    /// <summary>
    /// Converts actual instance to boolean if possible.
    /// </summary>
    /// <param name="provider">Format provider to use.</param>
    /// <returns>Converted boolean value.</returns>
    bool IConvertible.ToBoolean(IFormatProvider provider)
    {
      bool result;
      if (!bool.TryParse(Value, out result))
      {
        throw new InvalidCastException($"{this} cannot be converted to bool");
      }
      return result;
    }

    /// <summary>
    /// Converts actual instance to char if possible.
    /// </summary>
    /// <param name="provider">Format provider to use.</param>
    /// <returns>Converted char value.</returns>
    char IConvertible.ToChar(IFormatProvider provider)
    {
      if (1 == Value.Length)
      {
        return char.ToLowerInvariant(Value[0]);
      }
      throw new InvalidCastException($"{this} cannot be converted to char");
    }

    sbyte IConvertible.ToSByte(IFormatProvider provider)
    {
      sbyte result;
      if (!sbyte.TryParse(Value, NumberStyles.Integer, provider, out result))
      {
        throw new InvalidCastException($"{this} cannot be converted to sbyte");
      }
      return result;
    }

    byte IConvertible.ToByte(IFormatProvider provider)
    {
      byte result;
      if (!byte.TryParse(Value, NumberStyles.Integer, provider, out result))
      {
        throw new InvalidCastException($"{this} cannot be converted to byte");
      }
      return result;
    }

    short IConvertible.ToInt16(IFormatProvider provider)
    {
      short result;
      if (!short.TryParse(Value, NumberStyles.Integer, provider, out result))
      {
        throw new InvalidCastException($"{this} cannot be converted to short");
      }
      return result;
    }

    ushort IConvertible.ToUInt16(IFormatProvider provider)
    {
      ushort result;
      if (!ushort.TryParse(Value, NumberStyles.Integer, provider, out result))
      {
        throw new InvalidCastException($"{this} cannot be converted to ushort");
      }
      return result;
    }

    int IConvertible.ToInt32(IFormatProvider provider)
    {
      int result;
      if (!int.TryParse(Value, NumberStyles.Integer, provider, out result))
      {
        throw new InvalidCastException($"{this} cannot be converted to int");
      }
      return result;
    }

    uint IConvertible.ToUInt32(IFormatProvider provider)
    {
      uint result;
      if (!uint.TryParse(Value, NumberStyles.Integer, provider, out result))
      {
        throw new InvalidCastException($"{this} cannot be converted to uint");
      }
      return result;
    }

    long IConvertible.ToInt64(IFormatProvider provider)
    {
      long result;
      if (!long.TryParse(Value, NumberStyles.Integer, provider, out result))
      {
        throw new InvalidCastException(string.Format("{0} cannot be converted to long", this));
      }
      return result;
    }

    ulong IConvertible.ToUInt64(IFormatProvider provider)
    {
      ulong result;
      if (!ulong.TryParse(Value, NumberStyles.Integer, provider, out result))
      {
        throw new InvalidCastException(string.Format("{0} cannot be converted to ulong", this));
      }
      return result;
    }

    float IConvertible.ToSingle(IFormatProvider provider)
    {
      float result;
      if (!float.TryParse(Value, NumberStyles.Float, provider, out result))
      {
        throw new InvalidCastException(string.Format("{0} cannot be converted to float", this));
      }
      return result;
    }

    double IConvertible.ToDouble(IFormatProvider provider)
    {
      double result;
      if (!double.TryParse(Value, NumberStyles.Float, provider, out result))
      {
        throw new InvalidCastException(string.Format("{0} cannot be converted to double", this));
      }
      return result;
    }

    decimal IConvertible.ToDecimal(IFormatProvider provider)
    {
      decimal result;
      if (!decimal.TryParse(Value, NumberStyles.Float, provider, out result))
      {
        throw new InvalidCastException(string.Format("{0} cannot be converted to decimal", this));
      }
      return result;
    }

    DateTime IConvertible.ToDateTime(IFormatProvider provider)
    {
      DateTime result;
      if (!DateTime.TryParse(Value, provider, DateTimeStyles.AllowWhiteSpaces, out result))
      {
        throw new InvalidCastException(string.Format("{0} cannot be converted to DateTime", this));
      }
      return result;
    }

    string IConvertible.ToString(IFormatProvider provider) => ToLowerString();

    object IConvertible.ToType(Type conversionType, IFormatProvider provider)
    {
      Func<CaseInsensitive, IFormatProvider, object> converter;
      if (_toType.TryGetValue(conversionType, out converter))
      {
        return converter(this, provider);
      }
      throw new InvalidCastException(string.Format("{0} cannot be converted to {1}", this, conversionType));
    }

    #endregion
  }
}