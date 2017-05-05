using System;
using System.Collections.Generic;
using Xunit;

namespace NCoreUtils.Common
{
    public class CaseInsensitiveTests
    {
        static IEnumerable<(T, T)> PairPermitations<T>(T[] values)
        {
            for (var i = 0; i < values.Length; ++i)
            {
                for (var j = i + 1; j < values.Length; ++j)
                {
                    yield return (values[i], values[j]);
                }
            }
        }
        [Fact]
        public void Empty()
        {
            CaseInsensitive a;
            CaseInsensitive b = CaseInsensitive.Empty;
            CaseInsensitive c = new CaseInsensitive();
            CaseInsensitive d = new CaseInsensitive(null);
            CaseInsensitive e = new CaseInsensitive(string.Empty);
            CaseInsensitive f = CaseInsensitive.Create(null);
            CaseInsensitive g = CaseInsensitive.Create(string.Empty);
            Assert.NotNull(a.Value);
            Assert.NotNull(b.Value);
            Assert.NotNull(c.Value);
            Assert.NotNull(d.Value);
            Assert.NotNull(e.Value);
            Assert.NotNull(f.Value);
            Assert.NotNull(g.Value);
            Assert.Equal(0, a.Length);
            Assert.Equal(0, b.Length);
            Assert.Equal(0, c.Length);
            Assert.Equal(0, d.Length);
            Assert.Equal(0, e.Length);
            Assert.Equal(0, f.Length);
            Assert.Equal(0, g.Length);
            foreach ((var x, var y) in PairPermitations(new [] { a, b, c, d, e, f, g }))
            {
                Assert.True(x.Equals(y));
                Assert.Equal(x, y);
            }
        }
        [Fact]
        public void Create()
        {
            foreach(var value in new [] { "asd", "gba", "cdeeee" })
            {
                var ci = CaseInsensitive.Create(value);
                Assert.Equal(value.Length, ci.Length);
                Assert.True(StringComparer.OrdinalIgnoreCase.Equals(value, ci.Value));
            }
        }
        [Fact]
        public void IndexOf()
        {
            CaseInsensitive value = "abcdefghijklmnopqrstuvwyz";
            foreach (var ch in value.Value)
            {
                Assert.NotEqual(-1, value.IndexOf(ch));
                Assert.NotEqual(-1, value.IndexOf(Char.ToUpper(ch)));
            }
            Assert.Equal(-1, value.IndexOf('x'));
            Assert.Equal(-1, value.IndexOf('X'));
        }
        [Fact]
        public void CaseInsensitiveToString()
        {
            CaseInsensitive value = "ABC";
            Assert.Equal("CI(abc)", value.ToString());
        }
        [Fact]
        public void Equality()
        {
            CaseInsensitive a = "abc";
            CaseInsensitive b = "ABC";
            CaseInsensitive x = "xxx";
            Assert.True(a == b);
            Assert.False(a == x);
            Assert.False(b == x);
            Assert.True(a != x);
            Assert.True(b != x);
            Assert.True(a.Equals(b));
            Assert.False(a.Equals(x));
            Assert.False(b.Equals(x));
            Assert.True(((object)a).Equals((object)b));
            Assert.False(((object)a).Equals((object)x));
            Assert.False(((object)b).Equals((object)x));
            Assert.False(((object)a).Equals(null));
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
            Assert.NotEqual(a.GetHashCode(), x.GetHashCode());
            Assert.NotEqual(b.GetHashCode(), x.GetHashCode());
        }
    }
}
