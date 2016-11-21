namespace EaseMob{

	public class EMConversation {
		public string mConversationId{ set; get; }
		public ConversationType mConversationType{ set; get; } 
		public int mUnreadMsgCount { set; get; }
		public string mExtInfo{ set; get; }
		public EMMessage mLastMsg{ set; get; }
	}
}
