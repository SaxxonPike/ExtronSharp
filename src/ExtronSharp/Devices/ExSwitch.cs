using System;

namespace ExtronSharp.Devices
{
    public class ExSwitch<T>
    {
        private readonly Func<T> _getter;
        private readonly Action<T> _setter;

        public ExSwitch(Func<T> getter, Action<T> setter)
        {
            _getter = getter;
            _setter = setter;
        }

        public T Selected
        {
            get => _getter();
            set => _setter(value);
        }
    }
}