using System;
using System.Collections.Generic;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Provides generic composite metadata builder functionality.
    /// </summary>
    public interface ICompositeMetdataBuilder : IDictionary<Type, MetadataDescriptor>
    {
        /// <summary>
        /// Initializes the composite metadata.
        /// </summary>
        /// <param name="serviceProvider">Service provider to use in factory based initializations.</param>
        /// <returns>Composite metdata.</returns>
        ICompositeMetadata Initialize(IServiceProvider serviceProvider);
        /// <summary>
        /// Checks whether metadata of the specified metadata type has already been appended.
        /// </summary>
        /// <typeparam name="TMetadata">Metdata type to check for.</typeparam>
        /// <returns>
        /// <c>true</c> if metadata of the specified metadata type has already been appended, <c>false</c> otherwise.
        /// </returns>
        bool ContainsMetadata<TMetadata>() where TMetadata : IMetadata;
    }
    /// <summary>
    /// Provides specific composite metadata builder functionality.
    /// </summary>
    /// <typeparam name="T">Composite metadata type.</typeparam>
    public interface ICompositeMetdataBuilder<T> : ICompositeMetdataBuilder where T : ICompositeMetadata
    {
        /// <summary>
        /// Initializes the composite metadata.
        /// </summary>
        /// <param name="serviceProvider">Service provider to use in factory based initializations.</param>
        /// <returns>Composite metdata.</returns>
        new T Initialize(IServiceProvider serviceProvider);
    }
}