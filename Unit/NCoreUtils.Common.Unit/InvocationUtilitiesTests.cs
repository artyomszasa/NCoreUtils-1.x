using System;
using System.Linq;
using Xunit;

namespace NCoreUtils
{
    public class InvocationUtilitiesTests
    {
        sealed class ValueProvider : IServiceProvider
        {
            readonly object[] _values;
            public ValueProvider(params object[] values)
            {
                _values = values;
            }
            public object GetService(Type serviceType)
            {
                return _values.FirstOrDefault(value => null != value && value.GetType().Equals(serviceType));
            }
        }

        [Fact]
        public void GenericDelegates()
        {
            Func<int, string, string> f0 = (i, s) => $"{s}{i}";
            var valueProvider = new ValueProvider(2, "xxx");
            var result = f0.InvokeWith(valueProvider, new object[0]);
            Assert.NotNull(result);
            var stringResult = result as string;
            Assert.NotNull(stringResult);
            Assert.Equal("xxx2", stringResult);
        }

        [Fact]
        public void ExplicitArguments()
        {
            Func<int, string, string> f0 = (i, s) => $"{s}{i}";
            var valueProvider0 = new ValueProvider();
            var valueProvider1 = new ValueProvider("xxx");
            var valueProvider2 = new ValueProvider(2, "xxx");
            Assert.Throws<InvalidOperationException>(() => f0.InvokeWith(valueProvider0, new object[] { }));
            Assert.Throws<InvalidOperationException>(() => f0.InvokeWith(valueProvider1, new object[] { }));
            var res0 = f0.InvokeWith(valueProvider0, 2, "xxx");
            var res1 = f0.InvokeWith(valueProvider1, 2);
            var res2 = f0.InvokeWith(valueProvider2, new object[] { });
            AssertResult(res0);
            AssertResult(res1);
            AssertResult(res2);

            void AssertResult(object result)
            {
                Assert.NotNull(result);
                var stringResult = result as string;
                Assert.NotNull(stringResult);
                Assert.Equal("xxx2", stringResult);
            }
        }

        [Fact]
        public void Specializations()
        {
            Func<int, string> f0 = i => $"{i}";
            Func<int, string, string> f1 = (i, s) => $"{s}{i}";
            Func<int, string, short, string> f2 = (i, s, sh) => $"{s}{i}{sh}";
            var valueProvider = new ValueProvider(2, "xxx", (short)2);
            var res0 = f0.InvokeWith(valueProvider);
            var res1 = f1.InvokeWith(valueProvider);
            var res2 = f2.InvokeWith(valueProvider);
            Assert.Equal("2", res0);
            Assert.Equal("xxx2", res1);
            Assert.Equal("xxx22", res2);
        }
    }
}