using System.Runtime.InteropServices;

namespace EaseMob{

	public class EMSDKInterfaceIos : EMSDKInterfaceBase {

		[DllImport ("__Internal")]
		private static extern int _createAccount(string username, string password);

		[DllImport ("__Internal")]
		private static extern void _login(string username, string password);

		public override int createAccount (string username, string password)
		{
			return _createAccount(username,password);
		}

		public override void login (string username, string password)
		{
			_login (username, password);
		}

		public override void logout (bool flag)
		{

		}

		public override void sendTextMessage (string content, string to, int callbackId,int chattype)
		{

		}
		public override void sendVoiceMessage (string path, int length, string to, int callbackId,int chattype)
		{

		}
		public override void sendPictureMessage (string path, bool isSrcImage, string to, int callbackId,int chattype)
		{

		}
		public override void sendVideoMessage (string path, string thumbPath,int length, string to, int callbackId,int chattype)
		{

		}
		public override void sendLocationMessage (double latitude, double longitude, string locationAddress, string to, int callbackId,int chattype)
		{

		}
		public override void sendFileMessage (string path, string to, int callbackId,int chattype)
		{

		}
		public override string getAllContactsFromServer()
		{
			return "";
		}
		public override void startRecord(){}
		public override void stopRecord(){}

		public override string getAllConversationMessage(string username)
		{
			return null;
		}

		public override string getConversationMessage(string username, string startMsgId, int pageSize)
		{
			return null;
		}
		public override int getUnreadMsgCount (string username)
		{
			return 0;
		}
		public override void markAllMessagesAsRead (string username)
		{

		}
		public override void markMessageAsRead (string username, string messageId)
		{

		}
		public override void markAllConversationsAsRead ()
		{

		}
		public override int getAllMsgCount (string username)
		{
			return 0;
		}
		public override int getAllMessagesSize (string username)
		{
			return 0;
		}

		public override string getAllConversations ()
		{
			return null;
		}
		public override bool deleteConversation (string username, bool isDeleteHistory)
		{
			return true;
		}
		public override void removeMessage (string username, string msgId)
		{

		}
		public override void importMessages (string json)
		{

		}
		public override void createGroup (int callbackId,string groupName, string desc, string strMembers, string reason, int maxUsers, int style)
		{

		}
		public override void addUsersToGroup (int callbackId,string groupId, string strMembers)
		{

		}
		public override void inviteUser (int callbackId,string groupId, string beInvitedUsernames, string reason)
		{

		}
		public override void removeUserFromGroup (int callbackId,string groupId, string username)
		{

		}
		public override void joinGroup (int callbackId,string groupId)
		{

		}
		public override void applyJoinToGroup (int callbackId,string groupId, string reason)
		{

		}
		public override void leaveGroup (int callbackId,string groupId)
		{

		}
		public override void destroyGroup (int callbackId,string groupId)
		{

		}
		public override void getJoinedGroupsFromServer (int callbackId)
		{

		}
		public override string getAllGroups ()
		{
			return "";
		}
		public override void changeGroupName (int callbackId,string groupId,string groupName)
		{

		}
		public override string getGroup (string groupId)
		{
			return "";
		}
		public override void blockGroupMessage (int callbackId,string groupId)
		{

		}
		public override void unblockGroupMessage (int callbackId,string groupId)
		{

		}
		public override void blockUser (int callbackId,string groupId, string username)
		{

		}
		public override void unblockUser(int callbackId,string groupId,string username)
		{

		}
		public override void getBlockedUsers(int callbackId,string groupId)
		{

		}
	}

}