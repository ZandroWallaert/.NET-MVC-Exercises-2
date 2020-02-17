using System;

namespace lab_traceroute.model.messages {
   public class InfoMessage : IcmpMessage {
		public ushort Id { get; set; } // An id
		public ushort SeqNum { get; set; } // Message sequence number

		// Object serialization into a byte array (the protocol message itself)
		public override byte[] SerializeObject() {
			byte[] msg = new byte[8];
			Array.Copy(base.SerializeObject(), 0, msg, 0, 4); // call the abstract class serialize method
			Array.Copy(BitConverter.GetBytes(Id), 0, msg, 4, 2);
			Array.Copy(BitConverter.GetBytes(SeqNum), 0, msg, 6, 2);

			return msg;
		}
	}
}