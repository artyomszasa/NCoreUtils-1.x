namespace NCoreUtils.Reflection
{
  /// <summary>
  /// Defines visitor for <see cref="T:NCoreUtils.Reflection.AccessorType" />.
  /// </summary>
  public interface IAccessorTypeVisitor
  {
    /// <summary>
    /// Invoked if the accepting object is instance of
    /// <see cref="T:NCoreUtils.Reflection.AccessorType+PropertyAccessor" />.
    /// </summary>
    void VisitProperty();
    /// <summary>
    /// Invoked if the accepting object is instance of
    /// <see cref="T:NCoreUtils.Reflection.AccessorType+FieldAccessor" />.
    /// </summary>
    void VisitField();
    /// <summary>
    /// Invoked if the accepting object is instance of
    /// <see cref="T:NCoreUtils.Reflection.AccessorType+CustomAccessor" />.
    /// </summary>
    void VisitCustom();
  }

  /// <summary>
  /// Defines value generating visitor for <see cref="T:NCoreUtils.Reflection.AccessorType" />.
  /// </summary>
  public interface IAccessorTypeVisitor<T>
  {
    /// <summary>
    /// Invoked if the accepting object is instance of
    /// <see cref="T:NCoreUtils.Reflection.AccessorType+PropertyAccessor" />.
    /// </summary>
    T VisitProperty();
    /// <summary>
    /// Invoked if the accepting object is instance of
    /// <see cref="T:NCoreUtils.Reflection.AccessorType+FieldAccessor" />.
    /// </summary>
    T VisitField();
    /// <summary>
    /// Invoked if the accepting object is instance of
    /// <see cref="T:NCoreUtils.Reflection.AccessorType+CustomAccessor" />.
    /// </summary>
    T VisitCustom();
  }
}