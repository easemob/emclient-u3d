using System.Runtime.InteropServices;

namespace EaseMob{

	public class EMSDKInterfaceIos : EMSDKInterfaceBase {

		[DllImport ("__Internal")]
		private static extern int _createAccount(string username, string password);

		[DllImport ("__Internal")]
		private static extern void _login(string username, string password);

		[DllImport ("__Internal")]
		private static extern void _logout(bool flag);

		[DllImport ("__Internal")]
		private static extern void _sendTextMessage(string content, string to, int callbackId,int chattype);

		[DllImport ("__Internal")]
		private static extern void _sendFileMessage(string path, string to, int callbackId,int chattype);

		[DllImport ("__Internal")]
		private static extern string _getAllContactsFromServer ();

		[DllImport ("__Internal")]
		private static extern string _getAllConversations ();

		[DllImport ("__Internal")]
		private static extern void _createGroup (int callbackId, string groupName, string desc, string strMembers, string reason, int maxUsers, int style);

		[DllImport ("__Internal")]
		private static extern void _destroyGroup (int callbackId,string groupId);

		[DllImport ("__Internal")]
		private static extern void _changeGroupName (int callbackId, string groupId, string groupName);

		[DllImport ("__Internal")]
		private static extern void _inviteUser (int callbackId,string groupId, string beInvitedUsernames, string reason);

		[DllImport ("__Internal")]
		private static extern void _leaveGroup (int callbackId, string groupId);

		[DllImport ("__Internal")]
		private static extern void _getJoinedGroupsFromServer (int callbackId);

		[DllImport ("__Internal")]
		private static extern string _getJoinedGroups ();

		[DllImport ("__Internal")]
		private static extern string _getGroup (string groupId);

		[DllImport ("__Internal")]
		private static extern void _blockGroupMessage (int callbackId,string groupId);

		[DllImport ("__Internal")]
		private static extern void _unblockGroupMessage (int callbackId,string groupId);

		[DllImport ("__Internal")]
		private static extern void _blockUser (int callbackId, string groupId, string username);

		[DllImport ("__Internal")]
		private static extern void _unblockUser (int callbackId, string groupId, string username);

		[DllImport ("__Internal")]
		private static extern void _getBlockedUsers (int callbackId, string groupId);

		[DllImport ("__Internal")]
		private static extern void _applyJoinToGroup (int callbackId,string groupId, string reason);

		[DllImport ("__Internal")]
		private static extern void _removeUserFromGroup (int callbackId, string groupId, string username);

		[DllImport ("__Internal")]
		private static extern void _addUsersToGroup (int callbackId, string groupId, string strMembers);

		[DllImport ("__Internal")]
		private static extern void _joinGroup (int callbackId, string groupId);

		[DllImport ("__Internal")]
		private static extern string _getConversationMessage (string username, string startMsgId, int pageSize);

		[DllImport ("__Internal")]
		private static extern int _getUnreadMsgCount (string username);

		[DllImport ("__Internal")]
		private static extern void _markAllMessagesAsRead (string username);

		[DllImport ("__Internal")]
		private static extern bool _deleteConversation (string username, bool isDeleteHistory);

		[DllImport ("__Internal")]
		private static extern void _removeMessage (string username, string msgId);

		public override int createAccount (string username, string password)
		{
			return _createAccount(username,password);
		}

		public override void login (string username, string password)
		{
			_login(username, password);
		}

		public override void logout (bool flag)
		{
			_logout(flag);
		}

		public override void sendTextMessage (string content, string to, int callbackId,int chattype)
		{
			_sendTextMessage(content, to, callbackId, chattype);
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
			_sendFileMessage (path, to, callbackId, chattype);
		}
		public override string getAllContactsFromServer()
		{
			return _getAllContactsFromServer();
		}
		public override void startRecord(){}
		public override void stopRecord(){}

		public override string getAllConversationMessage(string username)
		{
			return null;
		}

		public override string getConversationMessage(string username, string startMsgId, int pageSize)
		{
			return _getConversationMessage(username,startMsgId,pageSize);
		}
		public override int getUnreadMsgCount (string username)
		{
			return _getUnreadMsgCount(username);
		}
		public override void markAllMessagesAsRead (string username)
		{
			_markAllMessagesAsRead (username);
		}
		public override void markMessageAsRead (string username, string messageId)
		{
			
		}
		public override void markAllConversationsAsRead ()
		{

		}

		//not use
		public override int getAllMsgCount (string username)
		{
			return 0;
		}
		//not use
		public override int getAllMessagesSize (string username)
		{
			return 0;
		}

		public override string getAllConversations ()
		{
			return _getAllConversations();
		}

		public override bool deleteConversation (string username, bool isDeleteHistory)
		{
			return _deleteConversation(username,isDeleteHistory);
		}
		public override void removeMessage (string username, string msgId)
		{
			_removeMessage (username, msgId);
		}
		public override void importMessages (string json)
		{

		}
		public override void createGroup (int callbackId,string groupName, string desc, string strMembers, string reason, int maxUsers, int style)
		{
			_createGroup (callbackId, groupName, desc, strMembers, reason, maxUsers, style);
		}

		public override void addUsersToGroup (int callbackId,string groupId, string strMembers)
		{
			_addUsersToGroup (callbackId, groupId, strMembers);
		}
		public override void inviteUser (int callbackId,string groupId, string beInvitedUsernames, string reason)
		{
			_inviteUser (callbackId, groupId, beInvitedUsernames, reason);
		}
		public override void removeUserFromGroup (int callbackId,string groupId, string username)
		{
			_removeUserFromGroup (callbackId, groupId, username);
		}
		public override void joinGroup (int callbackId,string groupId)
		{
			_joinGroup (callbackId, groupId);
		}
		public override void applyJoinToGroup (int callbackId,string groupId, string reason)
		{
			_applyJoinToGroup(callbackId,groupId,reason);
		}
		public override void leaveGroup (int callbackId,string groupId)
		{
			_leaveGroup (callbackId, groupId);
		}
		public override void destroyGroup (int callbackId,string groupId)
		{
			_destroyGroup (callbackId, groupId);
		}
		public override void getJoinedGroupsFromServer (int callbackId)
		{
			_getJoinedGroupsFromServer (callbackId);
		}
		public override string getAllGroups ()
		{
			return _getJoinedGroups();
		}
		public override void changeGroupName (int callbackId,string groupId,string groupName)
		{
			_changeGroupName (callbackId, groupId, groupName);
		}
		public override string getGroup (string groupId)
		{
			return _getGroup(groupId);
		}
		public override void blockGroupMessage (int callbackId,string groupId)
		{
			_blockGroupMessage (callbackId, groupId);
		}
		public override void unblockGroupMessage (int callbackId,string groupId)
		{
			_unblockGroupMessage (callbackId, groupId);
		}
		public override void blockUser (int callbackId,string groupId, string username)
		{
			_blockUser (callbackId, groupId, username);
		}
		public override void unblockUser(int callbackId,string groupId,string username)
		{
			_unblockUser (callbackId, groupId, username);
		}
		public override void getBlockedUsers(int callbackId,string groupId)
		{
			_getBlockedUsers (callbackId, groupId);
		}
	}

}