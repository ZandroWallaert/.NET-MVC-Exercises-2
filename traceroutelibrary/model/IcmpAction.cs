using lab_traceroute.model;

namespace lab_traceroute.model
{
	public class IcmpAction {
		public int UpdatePeriod { get; set; } = 1000;
		public IcmpType IcmpType { get; set; } = IcmpType.traceRoute;
		public string Host { get; set; } = "127.0.0.1";
		public int WeightPacket { get; set; } = 32;
		public int NbrEcho { get; set; } = 4;
		public int Timeout { get; set; } = 1000;
		public int MaxTTL { get; set; } = 128;

		public IcmpAction (string host, IcmpType icmpType, int nbrEcho, int weightPacket, int updatePeriod, int timeout, int maxTTL) {
			if (weightPacket < 1) {
				WeightPacket = 1;
			}

			Host = host;
			IcmpType = icmpType;
			NbrEcho = nbrEcho;
			UpdatePeriod = updatePeriod;
			Timeout = timeout;
			MaxTTL = maxTTL;
		}
	}
}