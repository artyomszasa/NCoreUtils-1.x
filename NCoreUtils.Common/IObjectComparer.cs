using System.Collections.Generic;

namespace NCoreUtils
{
  /// <summary>
  /// Provides generic functionality for full comparison (both order and equality) of an object.
  /// </summary>
  public interface IObjectComparer<T> : IEqualityComparer<T>, IComparer<T> { }
}