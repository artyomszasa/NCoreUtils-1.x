using System;
using System.Linq.Expressions;
using Xunit;

namespace NCoreUtils.Linq
{
    public class Embed
    {
        [Fact]
        public void EmbedValue()
        {
            var value = Expression.Constant(2, typeof(int));
            Expression<Func<int, int>> expr0 = i => i + value.EmbedAs<int>();
            var expr = expr0.Splice();
            var func = expr.Compile();
            Assert.Equal(3, func(1));
            Assert.Equal(5, func(3));
        }

        [Fact]
        public void EmbedFunc()
        {
            Expression<Func<int, int>> inc = i => i + 1;
            Expression<Func<int, int>> add2Source = i => inc.EmbedCall(i) + 1;
            var add2 = add2Source.Splice();
            var func = add2.Compile();
            Assert.Equal(3, func(1));
            Assert.Equal(5, func(3));
        }

        [Fact]
        public void EmbedNestedFunc()
        {
            Expression<Func<int, int>> inc = i => i + 1;
            Expression<Func<int, Func<int, int>, int>> apply = (i, f) => f(i);
            Expression<Func<int, int>> add2Source = i => apply.EmbedCall(i, j => inc.EmbedCall(j) + 1);
            var add2 = add2Source.Splice();
            var func = add2.Compile();
            Assert.Equal(3, func(1));
            Assert.Equal(5, func(3));
        }
    }
}