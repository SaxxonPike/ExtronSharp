using ExtronSharp.Devices;
using NUnit.Framework;

namespace ExtronSharp.Test
{
    [TestFixture]
    public class Harness
    {
        [Test]
        [TestCase("dvs-304-series-02-83-ec")]
        public void Run(string host)
        {
            using var adapter = new ExTelnet(host, ExTelnet.DefaultPort, 3000)
            {
                Log = TestContext.Out
            };
            adapter.Start();
            foreach (var _ in adapter.ReadLines())
            {
            }

            var device = new ExDvs304(adapter);

            device.SetOutputRate(new ExDvs304.OutputRate(
                ExDvs304.Resolution.Res1080p, 
                ExDvs304.RefreshRate.Rate60));
            //device.SetActivePixels(500);
            
            
            adapter.Stop();
        }
    }
}