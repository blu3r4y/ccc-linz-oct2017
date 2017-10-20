using System;
using System.Net.Sockets;
using System.Text;

namespace CCC.ContestUtil.Network
{
    public class TcpConnection
    {
        /// <summary>
        /// tcp client used to connect to a socket
        /// </summary>
        private TcpClient _tcpClient;

        /// <summary>
        /// network stream used to read from the connection
        /// </summary>
        private NetworkStream _stream;

        /// <summary>
        /// connection port
        /// </summary>
        protected readonly int _port;

        /// <summary>
        /// default timeout for read line requests
        /// </summary>
        private static readonly TimeSpan DefaultTimeout = new TimeSpan(0, 0, 0, 1);

        /// <summary>
        /// creates a new instance of the TcpConnection helper and doesn't connect yet
        /// </summary>
        /// <param name="port">port number</param>
        public TcpConnection(int port)
        {
            _port = port;
        }

        /// <summary>
        /// connects to a local tcp port
        /// </summary>
        public void Connect()
        {
            // close an existing stream and client
            _stream?.Close();
            _tcpClient?.Close();

            // start a new client
            _tcpClient = new TcpClient("localhost", _port) { NoDelay = true };
            _stream = _tcpClient.GetStream();
        }

        /// <summary>
        /// disconnects from the local tcp port
        /// </summary>
        public void Disconnect()
        {
            // close an existing stream and client
            _stream?.Close();
            _tcpClient?.Close();
        }

        /// <summary>
        /// checks whether or not data can be read
        /// </summary>
        /// <returns></returns>
        public bool DataAvailable()
        {
            return _stream != null && _stream.DataAvailable;
        }

        /// <summary>
        /// reads from the network stream.
        /// returns null if no data is available.
        /// </summary>
        /// <returns>return the raw data</returns>
        public string Read()
        {
            if (!DataAvailable()) return "";

            var buff = new byte[1024];
            var read = _stream.Read(buff, 0, buff.Length);

            var responseData = Encoding.ASCII.GetString(buff, 0, read);

            return responseData;
        }

        /// <summary>
        /// reads one or more lines from the network stream
        /// </summary>
        /// <param name="timeout">defines how long we should wait for a new line.
        /// default value would be infinite.</param>
        /// <returns>returns a line</returns>
        public string ReadLine(TimeSpan timeout = default(TimeSpan))
        {
            var startTime = DateTime.Now;
            var buffer = "";

            bool hasCrLf;

            do
            {
                // read data until a new line symbol appears
                // and as long there's data to read
                buffer += Read();

                // does the data end with line breaks?
                hasCrLf = buffer.EndsWith("\n") || buffer.EndsWith("\r");

                // is there data available and do we have a timeout (if we set on)
                var hasData = DataAvailable();
                var isTimeout = timeout != default(TimeSpan) && (DateTime.Now - startTime) > timeout;

                // stop if we got cr and lf
                // or we have no more data and a timeout
                if (hasCrLf) break;
                if (!hasData && isTimeout) break;
            }
            while (true);

            // clear line breaks in return string
            if (hasCrLf) buffer = buffer.Substring(0, buffer.Length - 1);

            return buffer;
        }

        /// <summary>
        /// writes a line to the socket
        /// </summary>
        /// <param name="msg">message to write</param>
        public void Write(string msg)
        {
            var bytes = Encoding.ASCII.GetBytes(msg);

            _stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// writes a line to the socket with new line indicators
        /// </summary>
        /// <param name="msg">message to write</param>
        public void WriteLine(string msg)
        {
            Write(msg + "\n");
        }

        /// <summary>
        /// writes a line and returns the response line
        /// </summary>
        /// <param name="msg">line to write</param>
        /// <param name="timeout">should we wait the default timeout to get a response?</param>
        /// <returns>response from the socket</returns>
        public string WriteRead(string msg, bool timeout = true)
        {
            if (msg != null) WriteLine(msg);

            return ReadLine(timeout ? DefaultTimeout : default(TimeSpan));
        }

        /// <summary>
        /// emulates a simple REPL with additional commands
        /// ::START - connect
        /// ::STOP - disconnect
        /// ::EXIT - end the console loop
        /// </summary>
        public void StartConsole()
        {
            while (true)
            {
                var line = Console.ReadLine();

                switch (line)
                {
                    case "::START":
                        Connect();
                        break;

                    case "::STOP":
                        Disconnect();
                        break;

                    case "::EXIT":
                        return;

                    default:
                        Console.WriteLine(WriteRead(line));
                        break;
                }
            }
        }
    }
}
