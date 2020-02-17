using System;

namespace lab_traceroute.model.messages {
    public class EchoMessage : InfoMessage {
		public byte[] Data {get;set;} 

		// Serialization into a byte array (the protocol message itself).
		public override byte[] SerializeObject() {
			int length = 8;

			if (Data != null) {
				length += Data.Length;
			}

			byte[] msg = new byte[length];
			Array.Copy(base.SerializeObject(), 0, msg, 0, 8);

			if (Data != null) {
				Array.Copy(Data, 0, msg, 8, Data.Length);
			}

			return msg;
		}
	}

}