using NCoreUtils.Reflection;
#if NET45
using System;
using System.Runtime.Serialization;
#endif

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Thrown when some non-optinal value is missing during deserialization.
    /// </summary>
    #if NET45
    [Serializable]
    #endif
    public class MissingValueException : SerializationException
    {
        #if NET45
        const string KeyAccessor = "MissingValueException.Accessor";
        #endif
        /// <summary>
        /// Gets the related accessor.
        /// </summary>
        public IAccessor Accessor { get; private set; }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.MissingValueException" /> with specified
        /// accessor and message.
        /// </summary>
        /// <param name="accessor">Related accessor.</param>
        /// <param name="message">The message that describes the error.</param>
        public MissingValueException(IAccessor accessor, string message)
            : base(message)
        {
            RuntimeAssert.ArgumentNotNull(accessor, nameof(accessor));
            Accessor = accessor;
        }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.MissingValueException" /> with specified
        /// accessor and default message.
        /// </summary>
        /// <param name="accessor">Related accessor.</param>
        public MissingValueException(IAccessor accessor) : this(accessor, $"Missing value for {accessor}") { }
        #if NET45
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.MissingValueException" /> during
        /// deserialization.
        /// </summary>
        /// <param name="info">
        /// The <c>SerializationInfo</c> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <c>StreamingContext</c> that contains contextual information about the source or destination.
        /// </param>
        protected MissingValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Accessor = (IAccessor)info.GetValue(KeyAccessor, typeof(IAccessor));
        }
        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(KeyAccessor, Accessor, typeof(IAccessor));
        }
        #endif
    }
}