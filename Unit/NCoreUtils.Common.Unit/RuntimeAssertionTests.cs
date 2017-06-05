using System;
using System.Collections.Generic;
using Xunit;

namespace NCoreUtils
{
    public class RuntimeAssertionTests
    {
      sealed class ComparableBox<T> : IComparable<ComparableBox<T>>
      {
        public readonly T Value;
        public ComparableBox(T value) => Value = value;
        public int CompareTo(ComparableBox<T> other) => Comparer<T>.Default.Compare(Value, other.Value);
        public override string ToString() => $"c{Value}";
      }
      sealed class Box<T>
      {
        public readonly T Value;
        public Box(T value) => Value = value;
        public override string ToString() => $"b{Value}";
      }
    sealed class BoxComparer<T> : IComparer<Box<T>>
    {
      public int Compare(Box<T> x, Box<T> y) => Comparer<T>.Default.Compare(x.Value, y.Value);
    }
    [Fact]
      public void NotNull()
      {
        // should not throw
        RuntimeAssert.ArgumentNotNull(2, "name");
        RuntimeAssert.ArgumentNotNull(new object(), "name");
        RuntimeAssert.ArgumentNotNull(string.Empty, "name");
        // should throw
        Assert.Throws<ArgumentNullException>(() => RuntimeAssert.ArgumentNotNull((object)null, "name"));
        Assert.Throws<ArgumentNullException>(() => RuntimeAssert.ArgumentNotNull((string)null, "name"));
      }
      [Fact]
      public void Equals()
      {
        // should not throw
        var guid = Guid.NewGuid();
        RuntimeAssert.Equals(2, 2, "name");
        RuntimeAssert.Equals(guid, guid, "name");
        RuntimeAssert.Equals("a", "a", "name");
        RuntimeAssert.Equals("A", "a", StringComparer.OrdinalIgnoreCase, "name");
        RuntimeAssert.Equals("A", "a", (a, b) => StringComparer.OrdinalIgnoreCase.Equals(a, b), "name");
        // should throw
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Equals(2, 3, "name"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Equals(guid, Guid.NewGuid(), "name"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Equals("a", "A", "name"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Equals("a", "A", StringComparer.CurrentCulture, "name"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Equals("a", "A", (a, b) => StringComparer.CurrentCulture.Equals(a, b), "name"));
      }
      [Fact]
      public void Greater()
      {
        // should not throw
        RuntimeAssert.Greater(3, 2, "3");
        RuntimeAssert.Greater(3L, 2L, "3L");
        RuntimeAssert.Greater(new ComparableBox<int>(3), new ComparableBox<int>(2), "c3");
        RuntimeAssert.Greater(new Box<int>(3), new Box<int>(2), new BoxComparer<int>(), "b3");
        RuntimeAssert.Greater(new Box<int>(3), new Box<int>(2), new BoxComparer<int>().Compare, "b3");
        // should throw
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Greater(2, 3, "2"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Greater(2L, 3L, "2L"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Greater(2, 2, "2"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Greater(2L, 2L, "2L"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Greater(new ComparableBox<int>(1), new ComparableBox<int>(2), "c1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Greater(new Box<int>(1), new Box<int>(2), new BoxComparer<int>(), "b1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Greater(new Box<int>(1), new Box<int>(2), new BoxComparer<int>().Compare, "b1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Greater(new ComparableBox<int>(1), new ComparableBox<int>(1), "c1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Greater(new Box<int>(1), new Box<int>(1), new BoxComparer<int>(), "b1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Greater(new Box<int>(1), new Box<int>(1), new BoxComparer<int>().Compare, "b1"));
      }
      [Fact]
      public void GreaterOrEquals()
      {
        // should not throw
        RuntimeAssert.GreaterOrEquals(3, 2, "3");
        RuntimeAssert.GreaterOrEquals(3L, 2L, "3L");
        RuntimeAssert.GreaterOrEquals(new ComparableBox<int>(3), new ComparableBox<int>(2), "c3");
        RuntimeAssert.GreaterOrEquals(new Box<int>(3), new Box<int>(2), new BoxComparer<int>(), "b3");
        RuntimeAssert.GreaterOrEquals(new Box<int>(3), new Box<int>(2), new BoxComparer<int>().Compare, "b3");
        RuntimeAssert.GreaterOrEquals(2, 2, "2");
        RuntimeAssert.GreaterOrEquals(2L, 2L, "2L");
        RuntimeAssert.GreaterOrEquals(new ComparableBox<int>(1), new ComparableBox<int>(1), "c1");
        RuntimeAssert.GreaterOrEquals(new Box<int>(1), new Box<int>(1), new BoxComparer<int>(), "b1");
        RuntimeAssert.GreaterOrEquals(new Box<int>(1), new Box<int>(1), new BoxComparer<int>().Compare, "b1");
        // should throw
        Assert.Throws<ArgumentException>(() => RuntimeAssert.GreaterOrEquals(2, 3, "2"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.GreaterOrEquals(2L, 3L, "2L"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.GreaterOrEquals(new ComparableBox<int>(1), new ComparableBox<int>(2), "c1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.GreaterOrEquals(new Box<int>(1), new Box<int>(2), new BoxComparer<int>(), "b1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.GreaterOrEquals(new Box<int>(1), new Box<int>(2), new BoxComparer<int>().Compare, "b1"));
      }
      [Fact]
      public void Less()
      {
        // should not throw
        RuntimeAssert.Less(3, 4, "3");
        RuntimeAssert.Less(3L, 4L, "3L");
        RuntimeAssert.Less(new ComparableBox<int>(3), new ComparableBox<int>(4), "c3");
        RuntimeAssert.Less(new Box<int>(3), new Box<int>(4), new BoxComparer<int>(), "b3");
        RuntimeAssert.Less(new Box<int>(3), new Box<int>(4), new BoxComparer<int>().Compare, "b3");
        // should throw
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Less(2, 1, "2"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Less(2L, 1L, "2L"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Less(2, 2, "2"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Less(2L, 2L, "2L"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Less(new ComparableBox<int>(1), new ComparableBox<int>(0), "c1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Less(new Box<int>(1), new Box<int>(0), new BoxComparer<int>(), "b1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Less(new Box<int>(1), new Box<int>(0), new BoxComparer<int>().Compare, "b1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Less(new ComparableBox<int>(1), new ComparableBox<int>(1), "c1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Less(new Box<int>(1), new Box<int>(1), new BoxComparer<int>(), "b1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.Less(new Box<int>(1), new Box<int>(1), new BoxComparer<int>().Compare, "b1"));
      }
      [Fact]
      public void LessOrEquals()
      {
        // should not throw
        RuntimeAssert.LessOrEquals(3, 5, "3");
        RuntimeAssert.LessOrEquals(3L, 5L, "3L");
        RuntimeAssert.LessOrEquals(new ComparableBox<int>(3), new ComparableBox<int>(5), "c3");
        RuntimeAssert.LessOrEquals(new Box<int>(3), new Box<int>(5), new BoxComparer<int>(), "b3");
        RuntimeAssert.LessOrEquals(new Box<int>(3), new Box<int>(5), new BoxComparer<int>().Compare, "b3");
        RuntimeAssert.LessOrEquals(2, 2, "2");
        RuntimeAssert.LessOrEquals(2L, 2L, "2L");
        RuntimeAssert.LessOrEquals(new ComparableBox<int>(1), new ComparableBox<int>(1), "c1");
        RuntimeAssert.LessOrEquals(new Box<int>(1), new Box<int>(1), new BoxComparer<int>(), "b1");
        RuntimeAssert.LessOrEquals(new Box<int>(1), new Box<int>(1), new BoxComparer<int>().Compare, "b1");
        // should throw
        Assert.Throws<ArgumentException>(() => RuntimeAssert.LessOrEquals(2, 1, "2"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.LessOrEquals(2L, 1L, "2L"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.LessOrEquals(new ComparableBox<int>(1), new ComparableBox<int>(0), "c1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.LessOrEquals(new Box<int>(1), new Box<int>(0), new BoxComparer<int>(), "b1"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.LessOrEquals(new Box<int>(1), new Box<int>(0), new BoxComparer<int>().Compare, "b1"));
      }
      public void InRange()
      {
        var c0 = new ComparableBox<int>(0);
        var c10 = new ComparableBox<int>(10);
        var c20 = new ComparableBox<int>(20);
        var b0 = new Box<int>(0);
        var b10 = new Box<int>(10);
        var b20 = new Box<int>(20);
        var comparer = new BoxComparer<int>();
        // should not throw
        RuntimeAssert.ArgumentInRange(10, 0, 20, "10");
        RuntimeAssert.ArgumentInRange(10L, 0L, 20L, "10L");
        RuntimeAssert.ArgumentInRange(c10, c0, c20, nameof(c10));
        RuntimeAssert.ArgumentInRange(b10, b0, b20, comparer, nameof(b10));
        RuntimeAssert.ArgumentInRange(b10, b0, b20, comparer.Compare, nameof(b10));
        // should throw
        Assert.Throws<ArgumentException>(() => RuntimeAssert.ArgumentInRange(0, 10, 20, "0"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.ArgumentInRange(0L, 10L, 20L, "0L"));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.ArgumentInRange(c0, c10, c20, nameof(c0)));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.ArgumentInRange(b0, b10, b20, comparer, nameof(b0)));
        Assert.Throws<ArgumentException>(() => RuntimeAssert.ArgumentInRange(b0, b10, b20, comparer.Compare, nameof(b0)));
      }
    }
}