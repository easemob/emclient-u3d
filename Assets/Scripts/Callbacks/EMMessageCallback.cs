using System.Collections.Generic;

namespace EaseMob{

	public class EMMessageCallback {

		public delegate void MessageReceivedCallback(List<EMMessage> msgs);
		public delegate void CmdMessageReceivedCallback(List<EMMessage> msgs);
		public delegate void MessageReadAckReceivedCallback(List<EMMessage> msgs);
		public delegate void MessageDeliveryAckReceivedCallback(List<EMMessage> msgs);
		public delegate void MessageChangedCallback(List<EMMessage> msgs);

		public MessageReceivedCallback onMessageReceivedCallback;
		public CmdMessageReceivedCallback onCmdMessageReceivedCallback;
		public MessageReadAckReceivedCallback onMessageReadAckReceivedCallback;
		public MessageDeliveryAckReceivedCallback onMessageDeliveryAckReceivedCallback;
		public MessageChangedCallback onMessageChangedCallback;
	}

}