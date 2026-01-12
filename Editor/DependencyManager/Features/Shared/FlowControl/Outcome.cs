using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class Outcome<T>
    {
        private readonly T _value;
        private readonly Failure _failure;

        private Outcome(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _value = value;
        }

        public static implicit operator Outcome<T>(T value)
        {
            return new Outcome<T>(value);
        }

        private Outcome(Failure failure)
        {
            _failure = failure ?? throw new ArgumentNullException(nameof(failure));
        }

        public static implicit operator Outcome<T>(Failure failure)
        {
            return new Outcome<T>(failure);
        }

        public bool IsSuccess => _failure == null;

        public T Value => IsSuccess ? _value : throw new InvalidOperationException();

        public Failure Failure => IsSuccess ? throw new InvalidOperationException() : _failure;
    }
}
