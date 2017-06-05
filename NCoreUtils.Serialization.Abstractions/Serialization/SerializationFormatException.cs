using System;
#if NET45
using System.Runtime.Serialization;
#endif

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Thrown if input value has invalid format.
    /// </summary>
    #if NET45
    [Serializable]
    #endif
    public class SerializationFormatException : SerializationException
    {
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.SerializationFormatException" /> with the
        /// specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SerializationFormatException(string message) : base(message) { }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.SerializationFormatException" /> with the
        /// specified inner exception.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public SerializationFormatException(FormatException innerException) : base(innerException?.Message, innerException) { }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.SerializationFormatException" /> with the
        /// specified message and inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public SerializationFormatException(string message, FormatException innerException) : base(message, innerException) { }
        #if NET45
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.SerializationFormatException" /> during
        /// deserialization.
        /// </summary>
        /// <param name="info">
        /// The <c>SerializationInfo</c> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <c>StreamingContext</c> that contains contextual information about the source or destination.
        /// </param>
        protected SerializationFormatException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endif
    }
}