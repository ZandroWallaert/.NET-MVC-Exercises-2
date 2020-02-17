using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using lab_traceroute.model.messages;

namespace lab_traceroute.network
{
    class IcmpService
    {
        private const int SPACE_SIGN = 32;
        private const int DEFAULT_PING_TIMEOUT = 1000;
        private const int DEFAULT_TTL = 128;
        private const int DEFAULT_DATA_SIZE = 32;
        public IPAddress TargetIp { get; set; } 
        public Socket NetworkSocket { get; set; } 
        public DateTime StartOfPingCommand { get; set; }
        public Timer PingCommandTimeOut { get; set; }
        public bool HasPingCommandTimedOut { get; set; }

        public IcmpService(string host)
        {
            try
            {
                TargetIp = Dns.GetHostEntry(host).AddressList[0]; // translate host name to ip
            }
            catch
            {}
        }

        // Destructor: method executed when object is destroyed.
        ~IcmpService()
        {
            if (NetworkSocket != null)
            {
                NetworkSocket.Close();
            }
        }

        public IcmpResult Ping()
        {
            return Ping(DEFAULT_PING_TIMEOUT);
        }

        public IcmpResult Ping(int timeout)
        {
            return Ping(timeout, DEFAULT_TTL);
        }

        public IcmpResult Ping(int timeout, int TTL)
        {
            return Ping(timeout, TTL, DEFAULT_DATA_SIZE);
        }

        public IcmpResult Ping(int timeout, int TTL, int weight)
        {
            if (TargetIp == null)
            {
                return new IcmpResult
                {
                    Success = false,
                    ErrorMessage = "Could not resolve host.",
                };
            }

            TimeSpan currentTime;
            EndPoint targetEndPoint = new IPEndPoint(TargetIp, 0);
            int returnTTL = DEFAULT_TTL;
            NetworkSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            byte[] echoMessage = CreateEchoMessage(weight);

            HasPingCommandTimedOut = false;
            PingCommandTimeOut = new Timer(new TimerCallback(OnTimeOut), null, timeout, Timeout.Infinite);
            StartOfPingCommand = DateTime.Now;

            try
            {
                NetworkSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.IpTimeToLive, TTL);

                if (NetworkSocket.SendTo(echoMessage, targetEndPoint) <= 0)
                {
                    // Sending the message failed.
                    throw new SocketException();
                }
                echoMessage = new byte[echoMessage.Length + 20];

                if (NetworkSocket.ReceiveFrom(echoMessage, ref targetEndPoint) <= 0)
                {
                    // Receiving message from the remote failed.
                    throw new SocketException();
                }

                // Get the TTL from returned packet
                returnTTL = Convert.ToInt32(echoMessage[8].ToString());
            }
            catch (SocketException e)
            {
                var icmpResult = new IcmpResult
                {
                    IPStartPoint = TargetIp.ToString(),
                    IPEndPoint = targetEndPoint.ToString().Substring(0, targetEndPoint.ToString().Length - 2),
                    Success = false,
                    ErrorMessage = e.Message,
                    Weight = weight,
                };

                if (HasPingCommandTimedOut)
                {
                    icmpResult.ErrorMessage = "Ping command timed out.";
                    icmpResult.Success = false;
                    icmpResult.TTL =  returnTTL;
                    icmpResult.Duration = TimeSpan.MaxValue;
                }

                return icmpResult;
            }
            finally
            {
                // Cleaning up connections & timers.
                NetworkSocket.Close();
                NetworkSocket = null;
                PingCommandTimeOut.Change(Timeout.Infinite, Timeout.Infinite);
                PingCommandTimeOut.Dispose();
                PingCommandTimeOut = null;
            }

            currentTime = DateTime.Now.Subtract(StartOfPingCommand);
            return new IcmpResult
            {
                Weight = weight,
                TTL = returnTTL,
                IPStartPoint = TargetIp.ToString(),
                IPEndPoint = targetEndPoint.ToString().Substring(0, targetEndPoint.ToString().Length - 2),
                Duration = currentTime
            };
        }

        // Callback when the ping method times out
        private void OnTimeOut(object state)
        {
            HasPingCommandTimedOut = true;
            if (NetworkSocket != null)
            {
                NetworkSocket.Close();
            }
        }

        // Generating the echo message to send
        private byte[] CreateEchoMessage(int weight)
        {
            if (weight < 1)
            {
                weight = 1;
            }

            var msg = new EchoMessage
            {
                Type = 8,
                Data = new System.Byte[1 * weight]
            };

            for (int i = 0; i < weight; i++)
            {
                // Use spaces as data.
                msg.Data[i] = SPACE_SIGN;
            }

            msg.CheckSum = msg.GetCheckSum();
            return msg.SerializeObject();
        }
    }
}