using System;
#if NET45
using System.Runtime.Serialization;
#endif

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Thrown when serialization encounters value that supposed to have different type.
    /// </summary>
    #if NET45
    [Serializable]
    #endif
    public class InvalidValueException : SerializationException
    {
        #if NET45
        const string KeyExpected = "InvalidValueException.Expected";
        const string KeyActual = "InvalidValueException.Actual";
        #endif
        /// <summary>
        /// Expected type of the value.
        /// </summary>
        public Type Expected { get; private set; }
        /// <summary>
        /// Actual type of the value or <c>null</c> if the value is <c>null</c>.
        /// </summary>
        public Type Actual { get; private set; }
        /// <inheritdoc />
        public override string Message
            => $"Value expected to be {Expected.FullName} but was {Actual?.FullName ?? "<null>"}";
        /// <summary>
        /// Initilaizes new instance of <see cref="T:NCoreUtils.Serialization.InvalidValueException" /> using the
        /// specified expected type and actual type.
        /// </summary>
        /// <param name="expected">Expected type.</param>
        /// <param name="actual">Actual type.</param>
        public InvalidValueException(Type expected, Type actual)
        {
            RuntimeAssert.ArgumentNotNull(expected, nameof(expected));
            Expected = expected;
            Actual = actual;
        }
        /// <summary>
        /// Initilaizes new instance of <see cref="T:NCoreUtils.Serialization.InvalidValueException" /> using the
        /// specified expected type and actual value.
        /// </summary>
        /// <param name="expected">Expected type.</param>
        /// <param name="actualObject">Actual value.</param>
        /// <returns>Initialized exception.</returns>
        public InvalidValueException(Type expected, object actualObject)
            : this(expected, actualObject?.GetType())
        { }
        #if NET45
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.InvalidValueException" /> during
        /// deserialization.
        /// </summary>
        /// <param name="info">
        /// The <c>SerializationInfo</c> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <c>StreamingContext</c> that contains contextual information about the source or destination.
        /// </param>
        protected InvalidValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            var actual = info.GetString(KeyActual);
            Actual = null == actual ? null : Type.GetType(actual);
            Expected = Type.GetType(info.GetString(KeyExpected));
        }
        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(KeyActual, Actual?.AssemblyQualifiedName);
            info.AddValue(KeyExpected, Expected.AssemblyQualifiedName);
        }
        #endif
    }
}