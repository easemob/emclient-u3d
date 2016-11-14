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
		public abstract void sendVoiceMessage (string path, int length, string to, int callbackId, int chattype);
		public abstract void sendPictureMessage (string path, bool isSrcImage, string to, int callbackId,int chattype);
		public abstract void sendVideoMessage (string path, string thumbPath,int length, string to, int callbackId,int chattype);
		public abstract void sendLocationMessage (double latitude, double longitude, string locationAddress, string to, int callbackId, int chattype);
		public abstract void sendFileMessage (string path, string to, int callbackId,int chattype);
		public abstract string getAllContactsFromServer();
		public abstract void startRecord();
		public abstract void stopRecord();
		public abstract string getAllConversationMessage(string username);
		public abstract string getConversationMessage(string username, string startMsgId, int pageSize);
		public abstract int getUnreadMsgCount (string username);
		public abstract void markAllMessagesAsRead (string username);
		public abstract void markMessageAsRead (string username, string messageId);
		public abstract void markAllConversationsAsRead ();
		public abstract int getAllMsgCount (string username);
		public abstract int getAllMessagesSize (string username);
		public abstract string getAllConversations ();
		public abstract bool deleteConversation (string username, bool isDeleteHistory);
		public abstract void removeMessage (string username, string msgId);
		public abstract void importMessages (string json);
		//group
		public abstract void createGroup (int callbackId,string groupName, string desc, string strMembers, string reason, int maxUsers, int style);
		public abstract void addUsersToGroup (int callbackId,string groupId, string strMembers);
		public abstract void inviteUser (int callbackId,string groupId, string beInvitedUsernames, string reason);
		public abstract void removeUserFromGroup (int callbackId,string groupId, string username);
		public abstract void joinGroup (int callbackId,string groupId);
		public abstract void applyJoinToGroup (int callbackId,string groupId, string reason);
		public abstract void leaveGroup (int callbackId,string groupId);
		public abstract void destroyGroup (int callbackId,string groupId);
		public abstract void getJoinedGroupsFromServer (int callbackId);
		public abstract string getAllGroups ();
		public abstract void changeGroupName (int callbackId,string groupId,string groupName);
		public abstract string getGroup (string groupId);
		public abstract void blockGroupMessage (int callbackId,string groupId);
		public abstract void unblockGroupMessage (int callbackId,string groupId);
		public abstract void blockUser (int callbackId,string groupId, string username);
		public abstract void unblockUser(int callbackId,string groupId,string username);
		public abstract void getBlockedUsers(int callbackId,string groupId);

	}

}
