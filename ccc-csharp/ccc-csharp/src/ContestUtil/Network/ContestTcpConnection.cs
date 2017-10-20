using System;
using System.Diagnostics;
using CCC.ContestUtil.Paths;

namespace CCC.ContestUtil.Network
{
    /// <summary>
    /// special catalysts coding contest implementation
    /// </summary>
    public class ContestTcpConnection : TcpConnection
    {
        private int level;
        private int mode;

        public ContestTcpConnection(int port, int level, int mode) : base(port)
        {
            this.level = level;
            this.mode = mode;
        }

        public new void Connect()
        {
            StartJar();

            base.Connect();
        }

        public new void Disconnect()
        {
            base.Disconnect();

            // kill jar file
            foreach (var proc in Process.GetProcessesByName("java")) proc.Kill();
        }

        /// <summary>
        /// starts the contest jar file
        /// <param name="killExisting">should we kill an existing java instance</param>
        /// </summary>
        public void StartJar(bool killExisting = true)
        {
            // kill jar file
            if (killExisting) foreach (var proc in Process.GetProcessesByName("java")) proc.Kill();

            // start a jar file
            string path = Environment.CurrentDirectory + @"\simulator.jar";

            var processInfo = new ProcessStartInfo(PathFinder.Java, "-jar \"" + path + $"\" {level} {mode} {_port}");

            if ((Process.Start(processInfo)) == null) throw new InvalidOperationException("couldn't start jar");
        }
    }
}