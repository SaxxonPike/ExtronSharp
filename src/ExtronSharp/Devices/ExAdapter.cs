using System.Collections.Generic;
using System.Threading;

namespace ExtronSharp.Devices
{
    public abstract class ExAdapter
    {
        protected abstract bool DataReady { get; }
        
        public abstract int Timeout { get; }
        
        public void WaitForLine() => 
            SpinWait.SpinUntil(() => DataReady, Timeout);

        public abstract string ReadLine();

        public IEnumerable<string> ReadLines()
        {
            while (DataReady)
                yield return ReadLine();
        }

        public abstract void WriteLine(string line);
    }
}