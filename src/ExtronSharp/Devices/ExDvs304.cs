using System;
using System.Text.RegularExpressions;

namespace ExtronSharp.Devices
{
    public class ExDvs304
    {
        private readonly ExAdapter _adapter;

        public ExDvs304(ExAdapter adapter)
        {
            _adapter = adapter;
        }

        public int GetVideoInput()
        {
            _adapter.WriteLine("I");
            _adapter.WaitForLine();
            var status = _adapter.ReadLine();
            return int.TryParse(ExCommand.ParseResponseFragment(status, "Vid"), out var result)
                ? result
                : 0;
        }

        public void SetVideoInput(int input)
        {
            _adapter.WriteLine($"{input:D1}&");
            _adapter.WaitForLine();
            _adapter.ReadLine();
        }
        
        public int GetAudioInput()
        {
            _adapter.WriteLine("I");
            _adapter.WaitForLine();
            var status = _adapter.ReadLine();
            return int.TryParse(ExCommand.ParseResponseFragment(status, "Aud"), out var result)
                ? result
                : 0;
        }

        public void SetAudioInput(int input)
        {
            _adapter.WriteLine($"{input:D1}$");
            _adapter.WaitForLine();
            _adapter.ReadLine();
        }

        public void SetInput(int input)
        {
            _adapter.WriteLine($"{input:D1}!");
            _adapter.WaitForLine();
            _adapter.ReadLine();
        }

        public int GetVideoType(int input)
        {
            _adapter.WriteLine($"{input:D1}");
            _adapter.WaitForLine();
            var status = _adapter.ReadLine();
            return int.TryParse(status, out var result)
                ? result
                : 0;
        }
    }
}