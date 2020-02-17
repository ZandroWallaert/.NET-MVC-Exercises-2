using System;

namespace lab_traceroute.network
{
    public class IcmpResult
    {
        public int TTL {get;set;}
        public string Host {get; set;}
        public int HopNumber {get; set;}
        public int Weight {get; set;}
		public string IPStartPoint {get;set;}
		public string IPEndPoint {get;set;}
		public TimeSpan Duration {get;set;}
        public Boolean Success {get; set;} = true;
        public string ErrorMessage {get; set;}
    }
}