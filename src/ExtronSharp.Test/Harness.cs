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
            device.SetVideoInput(1);
            device.SetVideoInput(2);
            device.SetVideoInput(3);
            device.SetVideoInput(4);
            adapter.Stop();
        }
    }
}