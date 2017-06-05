using System;
using System.Collections.Concurrent;

namespace NCoreUtils.Metadata
{
    abstract class BoxedMatcher
    {
        sealed class Impl<T> : BoxedMatcher where T : IMetadata
        {
            public override Maybe<IMetadata> TryGetValueImpl(MetadataDescriptor descriptor)
            {
                var desc = (MetadataDescriptor<T>)descriptor;
                return desc.Match(
                    onValue: v => ((IMetadata)v).AsMaybe(),
                    onFactory: _ => Maybe.Empty
                );
            }
            public override Maybe<IMetadata> TryInitializeImpl(MetadataDescriptor descriptor, IServiceProvider serviceProvider, ICompositeMetadata target)
            {
                var desc = (MetadataDescriptor<T>)descriptor;
                return desc.Match(
                    onFactory: factory => ((IMetadata)factory(serviceProvider, target)).AsMaybe(),
                    onValue: _ => Maybe.Empty
                );
            }
        }
        static readonly ConcurrentDictionary<Type, BoxedMatcher> _cache = new ConcurrentDictionary<Type, BoxedMatcher>();
        public static Maybe<IMetadata> TryGetValue(MetadataDescriptor descriptor)
            => _cache.GetOrAdd(descriptor.MetadataType, mtype => (BoxedMatcher)Activator.CreateInstance(typeof(Impl<>).MakeGenericType(mtype), true))
                .TryGetValueImpl(descriptor);
        public static Maybe<IMetadata> TryInitialize(MetadataDescriptor descriptor, IServiceProvider serviceProvider, ICompositeMetadata target)
            => _cache.GetOrAdd(descriptor.MetadataType, mtype => (BoxedMatcher)Activator.CreateInstance(typeof(Impl<>).MakeGenericType(mtype), true))
                .TryInitializeImpl(descriptor, serviceProvider, target);
        public abstract Maybe<IMetadata> TryGetValueImpl(MetadataDescriptor descriptor);
        public abstract Maybe<IMetadata> TryInitializeImpl(MetadataDescriptor descriptor, IServiceProvider serviceProvider, ICompositeMetadata target);
    }
}