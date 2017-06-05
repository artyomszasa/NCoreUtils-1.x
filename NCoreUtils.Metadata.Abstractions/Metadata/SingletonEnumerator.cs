using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NCoreUtils.Metadata
{
    static class SingletonEnumerator
    {
        public static IEnumerator<T> Create<T>(T value) => new SingletonEnumerator<T>(value);
    }
    sealed class SingletonEnumerator<T> : IEnumerator<T>
    {
        const int NotStarted = 0;
        const int Active = 1;
        const int Finished = 2;
        const int Disposed = 3;
        static readonly ImmutableDictionary<int, int> _moveNext;
        static SingletonEnumerator()
        {
            var builder = ImmutableDictionary.CreateBuilder<int, int>();
            builder.Add(NotStarted, Active);
            builder.Add(Active, Finished);
            builder.Add(Finished, Finished);
            builder.Add(Disposed, Disposed);
            _moveNext = builder.ToImmutable();
        }
        readonly T _value;
        int _state = NotStarted;
        T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [DebuggerStepThrough]
            get
            {
                ThrowIfDisposed();
                return Active == _state ? _value : default(T);
            }
        }
        T IEnumerator<T>.Current
        {
            [DebuggerStepThrough]
            get => Current;
        }
        object IEnumerator.Current
        {
            [DebuggerStepThrough]
            get => Current;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        public SingletonEnumerator(T value) => _value = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        void ThrowIfDisposed()
        {
            if (Disposed == _state)
            {
                throw new ObjectDisposedException(nameof(SingletonEnumerator<T>));
            }
        }
        [DebuggerStepThrough]
        void IDisposable.Dispose() => _state = Disposed;
        [DebuggerStepThrough]
        bool IEnumerator.MoveNext()
        {
            ThrowIfDisposed();
            var done = false;
            do
            {
                var currentState = _state;
                if (_moveNext.TryGetValue(currentState, out var nextState))
                {
                    done = currentState == Interlocked.CompareExchange(ref _state, nextState, currentState);
                }
                else
                {
                    throw new InvalidOperationException("State has been corrupted");
                }
            }
            while (!done);
            return Active == _state;
        }
        [DebuggerStepThrough]
        void IEnumerator.Reset()
        {
            ThrowIfDisposed();
            _state = NotStarted;
        }
    }
}