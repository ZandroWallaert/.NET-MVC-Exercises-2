using System.Collections.Generic;
using lab_traceroute.model;
using System;
using System.Net;
using System.Threading;

namespace lab_traceroute.network.handlers
{
    // TraceRouteHandler: executes the traceRoute command to a certain host.
    public class TraceRouteHandler : IIcmpActionHandler
    {
        public IEnumerable<IcmpResult> Handle(IcmpAction icmpAction)
        {
            var icmpService = new IcmpService(icmpAction.Host);
            bool isTargetFound = false;
            bool isTimeout = false;
            int startTTL = 1;
            int hopNumber = 1;
            int nbrErr = 0;

            while (!isTargetFound & !isTimeout)
            {
                IcmpResult icmpResult  = new IcmpResult();
                try
                {
                    startTTL += 1; // With each attempt allow a higher TTL.
                    icmpResult = icmpService.Ping(icmpAction.Timeout, startTTL);
                    icmpResult.HopNumber = hopNumber;
                    nbrErr = 0; // New hop was reached, reset error count to 0;
                    try
                    {
                        icmpResult.Host = Dns.GetHostEntry(icmpResult.IPEndPoint).HostName.ToString();
                    }
                    catch { }

                    if (icmpResult.Duration.Equals(TimeSpan.MaxValue))
                    {
                        icmpResult.Success = false;
                        icmpResult.ErrorMessage = "Timed out";
                    }
                    else 
                    {
                        hopNumber++;
                    }

                    if (icmpResult.IPEndPoint.Equals(icmpService.TargetIp.ToString()))
                    {
                        isTargetFound = true;
                    }

                    if (startTTL >= icmpAction.MaxTTL)
                    {
                        isTimeout = true;
                    }

                    hopNumber++;
                }
                catch
                {
                    nbrErr++;
                    if (nbrErr > 10)
                    {
                        isTimeout = true;
                    }
                }
                yield return icmpResult;
            }
        }
    }
}