using System;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Represents metadata construction info. Used to compose metadata into single object.
    /// </summary>
    public abstract class MetadataDescriptor
    {
        /// <summary>
        /// Provides functionality to inspect metadata descriptors.
        /// </summary>
        public interface IVisitor<TMetadata, TResult>
        {
            /// <summary>
            /// Invoked if metadata is represented by explicit object instance.
            /// </summary>
            /// <param name="metadata">Metdata instance.</param>
            /// <returns>Returns visitor dependent result.</returns>
            TResult Visit(TMetadata metadata);
            /// <summary>
            /// Invoked if metadata is represented by metadata factory.
            /// </summary>
            /// <param name="factory">Metdata factory.</param>
            /// <returns>Returns visitor dependent result.</returns>
            TResult Visit(Func<IServiceProvider, ICompositeMetadata, TMetadata> factory);
        }
        sealed class ExplicitMetadata<T> : MetadataDescriptor<T> where T : IMetadata
        {
            public T Value { get; private set; }
            public ExplicitMetadata(T value)
            {
                RuntimeAssert.ArgumentNotNull(value, nameof(value));
                Value = value;
            }
            internal override IMetadata Initialize(IServiceProvider serviceProvider, ICompositeMetadata metadata) => Value;
            public override TResult Accept<TResult>(IVisitor<T, TResult> visitor) => visitor.Visit(Value);
        }
        sealed class MetadataFactory<T> : MetadataDescriptor<T> where T : IMetadata
        {
            public Func<IServiceProvider, ICompositeMetadata, T> Factory { get; private set; }
            public MetadataFactory(Func<IServiceProvider, ICompositeMetadata, T> factory)
            {
                RuntimeAssert.ArgumentNotNull(factory, nameof(factory));
                Factory = factory;
            }
            internal override IMetadata Initialize(IServiceProvider serviceProvider, ICompositeMetadata metadata)
                => Factory(serviceProvider, metadata);
            public override TResult Accept<TResult>(IVisitor<T, TResult> visitor) => visitor.Visit(Factory);
        }
        internal sealed class Matcher<TMetadata, TResult> : IVisitor<TMetadata, TResult>
        {
            readonly Func<TMetadata, TResult> _onValue;
            readonly Func<Func<IServiceProvider, ICompositeMetadata, TMetadata>, TResult> _onFactory;
            public Matcher(Func<TMetadata, TResult> onValue, Func<Func<IServiceProvider, ICompositeMetadata, TMetadata>, TResult> onFactory)
            {
                RuntimeAssert.ArgumentNotNull(onValue, nameof(onValue));
                RuntimeAssert.ArgumentNotNull(onFactory, nameof(onFactory));
                _onValue = onValue;
                _onFactory = onFactory;
            }
            public TResult Visit(TMetadata metadata) => _onValue(metadata);
            public TResult Visit(Func<IServiceProvider, ICompositeMetadata, TMetadata> factory) => _onFactory(factory);
        }
        /// <summary>
        /// Initializes new metadata descriptor from explicit metadata instance.
        /// </summary>
        /// <param name="metadata">Metadata instance.</param>
        /// <returns>Created metadata descriptor.</returns>
        public static MetadataDescriptor Create<T>(T metadata) where T : IMetadata
            => new ExplicitMetadata<T>(metadata);
        /// <summary>
        /// Initializes new metadata descriptor with the speciified metadata factory.
        /// </summary>
        /// <param name="factory">Metadata factory.</param>
        /// <returns>Created metadata descriptor.</returns>
        public static MetadataDescriptor Create<T>(Func<IServiceProvider, ICompositeMetadata, T> factory) where T : IMetadata
            => new MetadataFactory<T>(factory);
        /// <summary>
        /// Initializes new metadata descriptor with the speciified metadata factory.
        /// </summary>
        /// <param name="factory">Metadata factory.</param>
        /// <returns>Created metadata descriptor.</returns>
        public static MetadataDescriptor Create<T>(Func<IServiceProvider, T> factory) where T : IMetadata
            => new MetadataFactory<T>((serviceProvider, _) => factory(serviceProvider));
        /// <summary>
        /// Initializes new metadata descriptor with the speciified metadata factory.
        /// </summary>
        /// <param name="factory">Metadata factory.</param>
        /// <returns>Created metadata descriptor.</returns>
        public static MetadataDescriptor Create<T>(Func<ICompositeMetadata, T> factory) where T : IMetadata
            => new MetadataFactory<T>((_, metadata) => factory(metadata));
        /// <summary>
        /// Gets the described metadata type.
        /// </summary>
        public abstract Type MetadataType { get; }
        internal MetadataDescriptor() { }
        internal abstract IMetadata Initialize(IServiceProvider serviceProvider, ICompositeMetadata metadata);
    }
    /// <summary>
    /// Represents typed metadata construction info. Used to compose metadata into single object.
    /// </summary>
    /// <typeparam name="T">Type of the metadata being described.</typeparam>
    public abstract class MetadataDescriptor<T> : MetadataDescriptor where T : IMetadata
    {
        /// <inheritdoc />
        public override Type MetadataType => typeof(T);
        internal MetadataDescriptor() { }
        /// <summary>
        /// Accepts metdata descriptor visitor by invoking appropriate visitor method.
        /// </summary>
        /// <param name="visitor">Visitor to use.</param>
        /// <returns>Visitor dependent result.</returns>
        public abstract TResult Accept<TResult>(MetadataDescriptor.IVisitor<T, TResult> visitor);
        /// <summary>
        /// Performs pattern matching by invoking apporiate callback.
        /// </summary>
        /// <typeparam name="TResult">Result type of the callbacks.</typeparam>
        /// <param name="onValue">Invoked if metdata descriptor represents metadata instance.</param>
        /// <param name="onFactory">Invoked if metdata descriptor represents metadata factory.</param>
        /// <returns>Parameter dependent result.</returns>
        public TResult Match<TResult>(Func<T, TResult> onValue, Func<Func<IServiceProvider, ICompositeMetadata, T>, TResult> onFactory)
            => Accept(new MetadataDescriptor.Matcher<T, TResult>(onValue, onFactory));
    }
}