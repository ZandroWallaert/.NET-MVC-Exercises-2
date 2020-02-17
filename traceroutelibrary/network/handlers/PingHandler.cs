using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using lab_traceroute.model;

namespace lab_traceroute.network.handlers
{
    /*
        PingHandler: executes the ping command to a target host.
        The parameters of the ping command are provided through the IcmpAction object.
    */
    public class PingHandler : IIcmpActionHandler
    {
        public IEnumerable<IcmpResult> Handle(IcmpAction action)
        {
            var icmpService = new IcmpService(action.Host);

            // Keep pinging until the amount of wanted echos is exchausted.
            while (action.NbrEcho > 0)
            {
                yield return Ping(action, icmpService);
                action.NbrEcho--;
            }
        }

        private IcmpResult Ping(IcmpAction action, IcmpService icmpService)
        {
            try
            {
                var icmpResult = icmpService.Ping(action.Timeout, action.MaxTTL, action.WeightPacket);
                if (icmpResult.Duration.Equals(TimeSpan.MaxValue))
                {
                    icmpResult.ErrorMessage = "Connection timed out";
                    icmpResult.Success = false;
                }

                // Adding some sleep to prevent icmp attacks on the host.
                if (1000 - icmpResult.Duration.TotalMilliseconds > 0)
                {
                    Thread.Sleep(action.UpdatePeriod - (int)icmpResult.Duration.TotalMilliseconds);
                }
                icmpResult.Host = action.Host;
                return icmpResult;
            }
            catch (Exception e)
            {
                return new IcmpResult
                {
                    Host = action.Host,
                    Weight = action.WeightPacket,
                    Success = false,
                    ErrorMessage = e.Message,
                };
            }

        }
    }
}