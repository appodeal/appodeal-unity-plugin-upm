// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class Error
    {
        private Error(string type, string message)
        {
            Type = type;
            Message = message;
        }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string Type { get; }

        public string Message { get; }

        public static Error Create(string type, string message) => new(type, message);
    }

    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    internal interface IRequest<out T>
    {
        bool IsSuccess { get; }
        T Value { get; }
        Error Error { get; }
    }

    internal class Request<T> : IRequest<T>
    {
        private readonly T _value;
        private readonly Error _error;

        private Request(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _value = value;
        }

        public static implicit operator Request<T>(T value)
        {
            return new Request<T>(value);
        }

        private Request(Error error)
        {
            _error = error ?? throw new ArgumentNullException(nameof(error));
        }

        public static implicit operator Request<T>(Error error)
        {
            return new Request<T>(error);
        }

        public bool IsSuccess
        {
            get => _error == null;
        }

        public T Value
        {
            get => IsSuccess ? _value : throw new InvalidOperationException();
        }

        public Error Error
        {
            get => IsSuccess ? throw new InvalidOperationException() : _error;
        }
    }
}
