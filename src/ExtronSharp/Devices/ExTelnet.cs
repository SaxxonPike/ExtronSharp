using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ExtronSharp.Devices
{
    public class ExTelnet : ExAdapter, IDisposable
    {
        public const int DefaultPort = 23;
        
        private readonly string _hostname;
        private readonly int _port;
        private Thread _thread;
        private readonly ConcurrentQueue<string> _readBuffer;
        private readonly ConcurrentQueue<string> _writeBuffer;
        private bool _stop;
        
        public TextWriter Log { get; set; }
        
        public ExTelnet(string hostname, int port, int timeout)
        {
            Timeout = timeout;
            _hostname = hostname;
            _port = port;
            _readBuffer = new ConcurrentQueue<string>();
            _writeBuffer = new ConcurrentQueue<string>();
            _stop = false;
        }
        
        protected override bool DataReady => !_readBuffer.IsEmpty;
        
        public override int Timeout { get; }

        public void Start()
        {
            if (_stop)
                throw new Exception("Still stopping..");
            
            if (_thread is { IsAlive: true })
                throw new Exception("Already running..");

            _readBuffer.Clear();
            _writeBuffer.Clear();
            
            _thread = new Thread(Background);
            _thread.Start();
            WaitForLine();
        }
        
        private void Background()
        {
            using var client = new TcpClient(_hostname, _port);
            using var stream = client.GetStream();
            var reader = new BinaryReader(stream);
            var writer = new StreamWriter(stream, Encoding.ASCII);
            while (true)
            {
                SpinWait.SpinUntil(() => _stop || stream.DataAvailable || !_writeBuffer.IsEmpty);
                if (_stop)
                {
                    _stop = false;
                    break;
                }

                while (stream.DataAvailable)
                {
                    var read = reader.ReadBytes(stream.Socket.Available);
                    var text = Encoding.ASCII.GetString(read);
                    foreach (var inLine in text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Log?.WriteLine($"{DateTime.Now:O} In:   {inLine}");
                        _readBuffer.Enqueue(inLine);
                    }
                }

                while (!_writeBuffer.IsEmpty && _writeBuffer.TryDequeue(out var outLine))
                {
                    Log?.WriteLine($"{DateTime.Now:O} Out:  {outLine}");
                    writer.Write(outLine);
                    writer.Write("\x0D\x0A");
                    writer.Flush();
                }
            }
        }

        public override string ReadLine() => 
            _thread is { IsAlive: true }
                ? !_readBuffer.TryDequeue(out var s) ? null : s
                : throw new Exception("Not started..");

        public override void WriteLine(string line)
        {
            if (_thread is { IsAlive: false })
                throw new Exception("Not started..");
            _writeBuffer.Enqueue(line);
        }

        public void Dispose()
        {
            _stop = true;
        }

        public void Stop() =>
            _stop = true;
    }
}