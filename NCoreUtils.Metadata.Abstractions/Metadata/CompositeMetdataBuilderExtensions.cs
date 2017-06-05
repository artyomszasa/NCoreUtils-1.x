using System;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Composite metadata builder extensions.
    /// </summary>
    public static class CompositeMetdataBuilderExtensions
    {
        sealed class EmptyServiceProvider : IServiceProvider
        {
            public static EmptyServiceProvider SharedInstance { get; } = new EmptyServiceProvider();
            public object GetService(Type serviceType) => null;
        }
        /// <summary>
        /// Initializes composite metadata using "empty" service provider.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Composed metadata type.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <returns>Initialized composite metadata.</returns>
        public static TMetadata Initialize<TMetadata, TBuilder>(this TBuilder builder)
            where TMetadata : ICompositeMetadata
            where TBuilder : ICompositeMetdataBuilder<TMetadata>
        {
            RuntimeAssert.ArgumentNotNull(builder, nameof(builder));
            return builder.Initialize(EmptyServiceProvider.SharedInstance);
        }
        /// <summary>
        /// Appends metadata object to the composite metadata being composed.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="metadata">Metadata object instance.</param>
        /// <returns>Target builder.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// Thrown if metadata with same type has already been appended.
        /// </exception>
        public static TBuilder AddMetadata<TMetadata, TBuilder>(this TBuilder builder, TMetadata metadata)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
        {
            builder.Add(typeof(TMetadata), MetadataDescriptor.Create(metadata));
            return builder;
        }
        /// <summary>
        /// Appends metadata object represented by its factory to the composite metadata being composed.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <returns>Target builder.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// Thrown if metadata with same type has already been appended.
        /// </exception>
        public static TBuilder AddMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<IServiceProvider, ICompositeMetadata, TMetadata> factory)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
        {
            builder.Add(typeof(TMetadata), MetadataDescriptor.Create(factory));
            return builder;
        }
        /// <summary>
        /// Appends metadata object represented by its factory to the composite metadata being composed.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <returns>Target builder.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// Thrown if metadata with same type has already been appended.
        /// </exception>
        public static TBuilder AddMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<IServiceProvider, TMetadata> factory)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
        {
            builder.Add(typeof(TMetadata), MetadataDescriptor.Create(factory));
            return builder;
        }
        /// <summary>
        /// Appends metadata object represented by its factory to the composite metadata being composed.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <returns>Target builder.</returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// Thrown if metadata with same type has already been appended.
        /// </exception>
        public static TBuilder AddMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<ICompositeMetadata, TMetadata> factory)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
        {
            builder.Add(typeof(TMetadata), MetadataDescriptor.Create(factory));
            return builder;
        }
        /// <summary>
        /// Either appends metadata object to the composite metadata being composed, or updates existing metadata
        /// descriptor.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="metadata">Metadata object instance.</param>
        /// <param name="update">
        /// Function that is invoked if metadata descriptor has already been appended for the specified metadata type.
        /// </param>
        /// <returns>Target builder.</returns>
        public static TBuilder AddOrUpdateMetadata<TMetadata, TBuilder>(this TBuilder builder, TMetadata metadata, Func<MetadataDescriptor, TMetadata, MetadataDescriptor> update)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
        {
            builder[typeof(TMetadata)] = builder.TryGetValue(typeof(TMetadata), out var desc) ? update(desc, metadata) : MetadataDescriptor.Create(metadata);
            return builder;
        }
        /// <summary>
        /// Either appends metadata object represented by its factory to the composite metadata being composed, or
        /// updates existing metadata descriptor.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <param name="update">
        /// Function that is invoked if metadata descriptor has already been appended for the specified metadata type.
        /// </param>
        /// <returns>Target builder.</returns>
        public static TBuilder AddOrUpdateMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<IServiceProvider, ICompositeMetadata, TMetadata> factory, Func<MetadataDescriptor, Func<IServiceProvider, ICompositeMetadata, TMetadata>, MetadataDescriptor> update)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
        {
            builder[typeof(TMetadata)] = builder.TryGetValue(typeof(TMetadata), out var desc) ? update(desc, factory) : MetadataDescriptor.Create(factory);
            return builder;
        }
        /// <summary>
        /// Either appends metadata object represented by its factory to the composite metadata being composed, or
        /// updates existing metadata descriptor.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <param name="update">
        /// Function that is invoked if metadata descriptor has already been appended for the specified metadata type.
        /// </param>
        /// <returns>Target builder.</returns>
        public static TBuilder AddOrUpdateMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<ICompositeMetadata, TMetadata> factory, Func<MetadataDescriptor, Func<ICompositeMetadata, TMetadata>, MetadataDescriptor> update)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
        {
            builder[typeof(TMetadata)] = builder.TryGetValue(typeof(TMetadata), out var desc) ? update(desc, factory) : MetadataDescriptor.Create(factory);
            return builder;
        }
        /// <summary>
        /// Either appends metadata object represented by its factory to the composite metadata being composed, or
        /// updates existing metadata descriptor.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <param name="update">
        /// Function that is invoked if metadata descriptor has already been appended for the specified metadata type.
        /// </param>
        /// <returns>Target builder.</returns>
        public static TBuilder AddOrUpdateMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<IServiceProvider, TMetadata> factory, Func<MetadataDescriptor, Func<IServiceProvider, TMetadata>, MetadataDescriptor> update)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
        {
            builder[typeof(TMetadata)] = builder.TryGetValue(typeof(TMetadata), out var desc) ? update(desc, factory) : MetadataDescriptor.Create(factory);
            return builder;
        }
        /// <summary>
        /// Appends metadata object to the composite metadata being composed overriding any previous assignment of
        /// metadata of the specified type.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="metadata">Metadata object instance.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder SetMetadata<TMetadata, TBuilder>(this TBuilder builder, TMetadata metadata)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
            => builder.AddOrUpdateMetadata(metadata, (_, md) => MetadataDescriptor.Create(md));
        /// <summary>
        /// Appends metadata object represented by its factory to the composite metadata being composed overriding any
        /// previous assignment of metadata of the specified type.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder SetMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<IServiceProvider, ICompositeMetadata, TMetadata> factory)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
            => builder.AddOrUpdateMetadata(factory, (_, f) => MetadataDescriptor.Create(f));
        /// <summary>
        /// Appends metadata object represented by its factory to the composite metadata being composed overriding any
        /// previous assignment of metadata of the specified type.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder SetMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<IServiceProvider, TMetadata> factory)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
            => builder.AddOrUpdateMetadata(factory, (_, f) => MetadataDescriptor.Create(f));
        /// <summary>
        /// Appends metadata object represented by its factory to the composite metadata being composed overriding any
        /// previous assignment of metadata of the specified type.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder SetMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<ICompositeMetadata, TMetadata> factory)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
            => builder.AddOrUpdateMetadata(factory, (_, f) => MetadataDescriptor.Create(f));
        /// <summary>
        /// Appends metadata object to the composite metadata being composed if no metadata descriptor has been already
        /// assigned for the specified metadata type.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="metadata">Metadata object instance.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder TryAddMetadata<TMetadata, TBuilder>(this TBuilder builder, TMetadata metadata)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
            => builder.AddOrUpdateMetadata(metadata, (desc, _) => desc);
        /// <summary>
        /// Appends metadata object represented by its factory to the composite metadata being composed if no metadata
        /// descriptor has been already assigned for the specified metadata type.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder TryAddMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<IServiceProvider, ICompositeMetadata, TMetadata> factory)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
            => builder.AddOrUpdateMetadata(factory, (desc, _) => desc);
        /// <summary>
        /// Appends metadata object represented by its factory to the composite metadata being composed if no metadata
        /// descriptor has been already assigned for the specified metadata type.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder TryAddMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<IServiceProvider, TMetadata> factory)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
            => builder.AddOrUpdateMetadata(factory, (desc, _) => desc);
        /// <summary>
        /// Appends metadata object represented by its factory to the composite metadata being composed if no metadata
        /// descriptor has been already assigned for the specified metadata type.
        /// </summary>
        /// <typeparam name="TBuilder">Builder type.</typeparam>
        /// <typeparam name="TMetadata">Metadata object type to append or update.</typeparam>
        /// <param name="builder">Target builder.</param>
        /// <param name="factory">Metadata object factory.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder TryAddMetadata<TMetadata, TBuilder>(this TBuilder builder, Func<ICompositeMetadata, TMetadata> factory)
            where TMetadata : IMetadata
            where TBuilder : ICompositeMetdataBuilder
            => builder.AddOrUpdateMetadata(factory, (desc, _) => desc);
    }
}