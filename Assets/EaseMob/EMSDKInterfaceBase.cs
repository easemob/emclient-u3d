namespace EaseMob{

	public abstract class EMSDKInterfaceBase {

		private static EMSDKInterfaceBase _instance;

		public static EMSDKInterfaceBase Instance
		{
			get {
				if (_instance == null) {
					#if UNITY_EDITOR || UNITY_STANDLONE
					_instance = new EMSDKInterfaceDefault ();
					#elif UNITY_ANDROID
					_instance = new EMSDKInterfaceAndroid ();
					#elif UNITY_IOS
					_instance = new EMSDKInterfaceIos();
					#else
					_instance = new EMSDKInterfaceDefault();
					#endif
				}
				return _instance;
			}
		}
		
		public abstract int createAccount(string username, string password);
		public abstract void login(string username, string password);
		public abstract void logout (bool flag);
		public abstract void sendTextMessage (string content, string to, int callbackId, int chattype);
//		public abstract void sendVoiceMessage (string path, int length, string to, int callbackId, int chattype);
//		public abstract void sendPictureMessage (string path, bool isSrcImage, string to, int callbackId,int chattype);
//		public abstract void sendVideoMessage (string path, string thumbPath,int length, string to, int callbackId,int chattype);
//		public abstract void sendLocationMessage (double latitude, double longitude, string locationAddress, string to, int callbackId, int chattype);
		public abstract void sendFileMessage (string path, string to, int callbackId,int chattype);
		public abstract string getAllContactsFromServer();
		public abstract string getAllConversationMessage(string username);
		public abstract string getConversationMessage(string username, string startMsgId, int pageSize);
		public abstract string getLatestMessage (string username);
		public abstract int getUnreadMsgCount (string username);
		public abstract void markAllMessagesAsRead (string username);
		public abstract void markMessageAsRead (string username, string messageId);
		public abstract void markAllConversationsAsRead ();
		public abstract string getAllConversations ();
		public abstract bool deleteConversation (string username, bool isDeleteHistory);
		public abstract void removeMessage (string username, string msgId);
		public abstract void importMessages (string json);
		//group
		public abstract void createGroup (int callbackId,string groupName, string desc, string strMembers, string reason, int maxUsers, int style);
		public abstract void addUsersToGroup (int callbackId,string groupId, string strMembers);
		public abstract void joinGroup (int callbackId,string groupId);
		//主动退群
		public abstract void leaveGroup (int callbackId,string groupId);
		//从服务器获取自己加入的和创建的群组列表，此api获取的群组sdk会自动保存到内存和db
		public abstract void getJoinedGroupsFromServer (int callbackId);
		//获取群详情
		public abstract string getGroup (string groupId);
		public abstract void downloadAttachment(int callbackId,string username,string msgId);
		public abstract string getConversation (string cid, int type, bool createIfNotExists);
		public abstract void deleteMessagesAsExitGroup (bool del);
		public abstract void isAutoAcceptGroupInvitation(bool isAuto);
		public abstract void isSortMessageByServerTime(bool isSort);
		public abstract void requireDeliveryAck(bool isReq);

	}

}
