using System.Diagnostics.CodeAnalysis;

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
            var status = _adapter.WriteAndReadLine("I");
            return int.TryParse(ExCommand.ParseResponseFragment(status, "Vid"), out var result)
                ? result
                : 0;
        }

        public void SetVideoInput(int input)
        {
            _adapter.WriteAndReadLine($"{input:D1}&");
        }

        public int GetAudioInput()
        {
            var status = _adapter.WriteAndReadLine("I");
            return int.TryParse(ExCommand.ParseResponseFragment(status, "Aud"), out var result)
                ? result
                : 0;
        }

        public void SetAudioInput(int input)
        {
            _adapter.WriteAndReadLine($"{input:D1}$");
        }

        public void SetInput(int input)
        {
            _adapter.WriteAndReadLine($"{input:D1}!");
        }

        public Format GetVideoFormat(int input)
        {
            _adapter.WriteLine($"{input:D1}\\");
            _adapter.WaitForLine();
            var status = _adapter.ReadLine();
            return (Format)(int.TryParse(status, out var result)
                ? result
                : 0);
        }

        public void SetVideoFormat(int input, Format format)
        {
            _adapter.WriteLine($"{input:D1}*{(int)format:D1}\\");
            _adapter.WaitForLine();
            _adapter.ReadLine();
        }

        public int GetHorizontalStart()
        {
            _adapter.WriteLine(")");
            _adapter.WaitForLine();
            var status = _adapter.ReadLine();
            return int.TryParse(status, out var result)
                ? result
                : 0;
        }

        public void SetHorizontalStart(int value)
        {
            _adapter.WriteLine($"{value})");
            _adapter.WaitForLine();
            _adapter.ReadLine();
        }

        public int GetVerticalStart()
        {
            _adapter.WriteLine("(");
            _adapter.WaitForLine();
            var status = _adapter.ReadLine();
            return int.TryParse(status, out var result)
                ? result
                : 0;
        }

        public void SetVerticalStart(int value)
        {
            _adapter.WriteLine($"{value}(");
            _adapter.WaitForLine();
            _adapter.ReadLine();
        }
        
        public int GetPixelPhase()
        {
            _adapter.WriteLine("U");
            _adapter.WaitForLine();
            var status = _adapter.ReadLine();
            return int.TryParse(status, out var result)
                ? result
                : 0;
        }

        public void SetPixelPhase(int value)
        {
            _adapter.WriteLine($"{value}U");
            _adapter.WaitForLine();
            _adapter.ReadLine();
        }
        
        public int GetActivePixels()
        {
            _adapter.WriteLine("12#");
            _adapter.WaitForLine();
            var status = _adapter.ReadLine();
            return int.TryParse(status, out var result)
                ? result
                : 0;
        }

        public void SetActivePixels(int value)
        {
            _adapter.WriteLine($"12*{value}#");
            _adapter.WaitForLine();
            _adapter.ReadLine();
        }

        public OutputRate GetOutputRate()
        {
            var status = _adapter.WriteAndReadLine("=").Split("*");
            return new OutputRate
            {
                Resolution = (Resolution)int.Parse(status[0]),
                RefreshRate = (RefreshRate)int.Parse(status[1])
            };
        }

        public void SetOutputRate(OutputRate outputRate)
        {
            _adapter.WriteAndReadLine($"{(int)outputRate.Resolution:D2}*{(int)outputRate.RefreshRate:D2}=");
        }

        public TestPattern GetTestPattern()
        {
            _adapter.WriteLine($"J");
            _adapter.WaitForLine();
            var status = _adapter.ReadLine();
            return (TestPattern)(int.TryParse(status, out var result)
                ? result
                : 0);
        }

        public void SetTestPattern(TestPattern testPattern)
        {
            _adapter.WriteLine($"{(int)testPattern:D1}J");
            _adapter.WaitForLine();
            _adapter.ReadLine();
        }

        public void PerformAutoImage()
        {
            _adapter.WriteLine($"55*2#");
            _adapter.WaitForLine();
            _adapter.ReadLine();
        }

        public struct OutputRate
        {
            public OutputRate(Resolution resolution, RefreshRate refreshRate)
            {
                Resolution = resolution;
                RefreshRate = refreshRate;
            }
            
            public Resolution Resolution { get; init; }
            public RefreshRate RefreshRate { get; init; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum TestPattern
        {
            Crop = 1,
            AlternatingPixels = 2,
            ColorBars = 3,
            Off = 0
        }
        
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Format
        {
            Composite = 1,
            SVideo = 2,
            RGBcvS = 3,
            YUVi = 4,
            YUVp = 5,
            RGBScaled = 6,
            RGBPass = 7,
            AutoDetect = 8,
            SDI = 9
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Resolution
        {
            Res640x480 = 1,
            Res800x600 = 2,
            Res852x480 = 3,
            Res1024x768 = 4,
            Res1024x852 = 5,
            Res1024x1024 = 6,
            Res1280x768 = 7,
            Res1280x1024 = 8,
            Res1360x765 = 9,
            Res1365x768 = 10,
            Res1365x1024 = 11,
            Res1366x768 = 12,
            Res1400x1050 = 13,
            Res1600x1200 = 14,
            Res480p = 15,
            Res576p = 16,
            Res720p = 17,
            Res1080i = 18,
            Res1080p = 19,
            Res1440x900 = 20,
            Res1680x1050 = 21,
            Res1280x800 = 22,
            Res1080pSharp = 23,
            Res1920x1200 = 24,
            Res1080pCvt = 25
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum RefreshRate
        {
            Rate50 = 1,
            Rate60 = 2,
            Rate72 = 3, // all 3 rates are represented by the same value, apparently
            Rate75 = 3,
            Rate24 = 3,
            Rate96 = 4,
            Rate100 = 5,
            Rate120 = 6,
            Rate59_94 = 7
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Edid
        {
            Res640x480Rate50 = 10,
            Res640x480Rate60 = 11,
            Res640x480Rate72 = 12,
            Res640x480Rate96 = 13,
            Res640x480Rate100 = 14,
            Res640x480Rate120 = 15,
            Res800x600Rate50 = 16,
            Res800x600Rate60 = 17,
            Res800x600Rate72 = 18,
            Res800x600Rate96 = 19,
            Res800x600Rate100 = 20,
            Res800x600Rate120 = 21,
            Res852x480Rate50 = 22,
            Res852x480Rate60 = 23,
            Res1024x768Rate50 = 24,
            Res1024x768Rate60 = 25,
            Res1024x768Rate72 = 26,
            Res1024x768Rate96 = 27,
            Res1024x852Rate50 = 28,
            Res1024x852Rate60 = 29,
            Res1024x852Rate72 = 30,
            Res1024x852Rate96 = 31,
            Res1024x1024Rate50 = 32,
            Res1024x1024Rate60 = 33,
            Res1024x1024Rate72 = 34,
            Res1280x768Rate50 = 35,
            Res1280x768Rate60 = 36,
            Res1280x768Rate72 = 37,
            Res1280x768Rate96 = 38,
            Res1280x1024Rate50 = 39,
            Res1280x1024Rate60 = 40,
            Res1280x1024Rate72 = 41,
            Res1360x765Rate50 = 42,
            Res1360x765Rate60 = 43,
            Res1360x765Rate72 = 44,
            Res1365x768Rate50 = 45,
            Res1365x768Rate60 = 46,
            Res1365x768Rate72 = 47,
            Res1365x1024Rate50 = 48,
            Res1365x1024Rate60 = 49,
            Res1366x768Rate50 = 50,
            Res1366x768Rate60 = 51,
            Res1366x768Rate72 = 52,
            Res1400x1050Rate50 = 53,
            Res1400x1050Rate60 = 54,
            Res1600x1200Rate50 = 55,
            Res1600x1200Rate60 = 56,
            Res480pRate59_94 = 57,
            Res480pRate60 = 58,
            Res576pRate50 = 59,
            Res576pRate100 = 60,
            Res720pRate50 = 61,
            Res720pRate59_94 = 62,
            Res720pRate60 = 63,
            Res1080iRate50 = 64,
            Res1080iRate59_94 = 65,
            Res1080iRate60 = 66,

            // 67 is absent from the manual
            Res1080pRate50 = 68,
            Res1080pRate59_94 = 69,
            Res1080pRate60 = 70,
            Res1440x900Rate60 = 71,
            Res1440x900Rate75 = 72,
            Res1680x1050Rate60 = 73,
            Res1280x800Rate50 = 74,
            Res1280x800Rate60 = 75,
            Res1080pSharpRate60 = 76,
            Res1920x1200Rate60 = 77,
            Res1080pCvtRate60 = 78
        }
    }
}