using System;

namespace ExtronSharp.Devices
{
    public class ExMatrix<TIn, TOut>
    {
        private readonly Func<TOut, TIn> _inGetter;
        private readonly Func<TIn, TOut[]> _outGetter;
        private readonly Action<TIn, TOut[]> _setter;

        public ExMatrix(Func<TOut, TIn> inGetter, Func<TIn, TOut[]> outGetter, Action<TIn, TOut[]> setter)
        {
            _inGetter = inGetter;
            _outGetter = outGetter;
            _setter = setter;
        }

        public TOut[] GetOutputs(TIn input) => _outGetter(input);
        public TIn GetInput(TOut output) => _inGetter(output);
        public void Route(TIn input, params TOut[] output) => _setter(input, output);
    }
}