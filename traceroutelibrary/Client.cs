using System;
using System.Collections.Generic;
using lab_traceroute.model;
using lab_traceroute.network;
using lab_traceroute.network.handlers;

namespace lab_traceroute 
{
    public class Client 
    {
        public IEnumerable<IcmpResult> TraceRoute(string destination)
        {
            return HandleIcmpAction(destination, IcmpType.traceRoute, 4, 32, 1000, 1000, 128);
        }

        public IEnumerable<IcmpResult> Ping(string destination)
        {
            return HandleIcmpAction(destination, IcmpType.ping, 4, 32, 1000, 1000, 128);
        }

        private IEnumerable<IcmpResult> HandleIcmpAction(string host, IcmpType icmpType, int nbrEcho, int weightPacket, int updatePeriod, int timeout, int maxTTL) 
        {
            IcmpAction icmpAction = new IcmpAction(host, icmpType, nbrEcho, weightPacket, updatePeriod, timeout, maxTTL);
            return GetIcmpActionHandler(icmpType).Handle(icmpAction);
        }

        private IIcmpActionHandler GetIcmpActionHandler(IcmpType icmpType)
        {
            switch (icmpType) {
                case IcmpType.traceRoute:
                    return new TraceRouteHandler();
                case IcmpType.ping:
                    return new PingHandler();
                default:
                    return new PingHandler();
            }
        }

    }
}