using System;
using System.Collections.Generic;
using lab_traceroute.model;

namespace lab_traceroute.network.handlers
{
    public interface IIcmpActionHandler
    {
        public IEnumerable<IcmpResult> Handle(IcmpAction icmpAction);
    }
}