#if UNITY_IOS
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
		private static extern string _getAllConversationMessage (string username);

		[DllImport ("__Internal")]
		private static extern string _getAllConversations ();

		[DllImport ("__Internal")]
		private static extern void _createGroup (int callbackId, string groupName, string desc, string strMembers, string reason, int maxUsers, int style);

		[DllImport ("__Internal")]
		private static extern void _leaveGroup (int callbackId, string groupId);

		[DllImport ("__Internal")]
		private static extern void _getJoinedGroupsFromServer (int callbackId);

		[DllImport ("__Internal")]
		private static extern string _getGroup (string groupId);

		[DllImport ("__Internal")]
		private static extern void _addUsersToGroup (int callbackId, string groupId, string strMembers);

		[DllImport ("__Internal")]
		private static extern void _joinGroup (int callbackId, string groupId);

		[DllImport ("__Internal")]
		private static extern string _getConversationMessage (string username, string startMsgId, int pageSize);

		[DllImport ("__Internal")]
		private static extern string _getLatestMessage(string username);

		[DllImport ("__Internal")]
		private static extern int _getUnreadMsgCount (string username);

		[DllImport ("__Internal")]
		private static extern void _markAllMessagesAsRead (string username);

		[DllImport ("__Internal")]
		private static extern bool _deleteConversation (string username, bool isDeleteHistory);

		[DllImport ("__Internal")]
		private static extern void _removeMessage (string username, string msgId);

		[DllImport ("__Internal")]
		private static extern void _downloadAttachment(int callbackId,string username,string msgId);

		[DllImport ("__Internal")]
		private static extern string _getConversation (string cid, int type, bool createIfNotExists);

		[DllImport ("__Internal")]
		private static extern void _deleteMessagesAsExitGroup (bool del);

		[DllImport ("__Internal")]
		private static extern void _isAutoAcceptGroupInvitation (bool isAuto);

		[DllImport ("__Internal")]
		private static extern void _isSortMessageByServerTime(bool isSort);

		[DllImport ("__Internal")]
		private static extern void _requireDeliveryAck(bool isReq);


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
//		public override void sendVoiceMessage (string path, int length, string to, int callbackId,int chattype)
//		{
//
//		}
//		public override void sendPictureMessage (string path, bool isSrcImage, string to, int callbackId,int chattype)
//		{
//
//		}
//		public override void sendVideoMessage (string path, string thumbPath,int length, string to, int callbackId,int chattype)
//		{
//
//		}
//		public override void sendLocationMessage (double latitude, double longitude, string locationAddress, string to, int callbackId,int chattype)
//		{
//
//		}
		public override void sendFileMessage (string path, string to, int callbackId,int chattype)
		{
			_sendFileMessage (path, to, callbackId, chattype);
		}
		public override string getAllContactsFromServer()
		{
			return _getAllContactsFromServer();
		}

		public override string getAllConversationMessage(string username)
		{
			return _getAllConversationMessage(username);
		}

		public override string getConversationMessage(string username, string startMsgId, int pageSize)
		{
			return _getConversationMessage(username,startMsgId,pageSize);
		}
		public override string getLatestMessage (string username)
		{
			return _getLatestMessage (username);
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
		public override void joinGroup (int callbackId,string groupId)
		{
			_joinGroup (callbackId, groupId);
		}
		public override void leaveGroup (int callbackId,string groupId)
		{
			_leaveGroup (callbackId, groupId);
		}

		public override void getJoinedGroupsFromServer (int callbackId)
		{
			_getJoinedGroupsFromServer (callbackId);
		}

		public override string getGroup (string groupId)
		{
			return _getGroup(groupId);
		}

		public override void downloadAttachment(int callbackId,string username,string msgId)
		{
			_downloadAttachment(callbackId,username, msgId);
		}

		public override string getConversation (string cid, int type, bool createIfNotExists)
		{
			return _getConversation (cid, type, createIfNotExists);
		}
		public override void deleteMessagesAsExitGroup (bool del)
		{
			_deleteMessagesAsExitGroup (del);
		}
		public override void isAutoAcceptGroupInvitation(bool isAuto)
		{
			_isAutoAcceptGroupInvitation (isAuto);
		}
		public override void isSortMessageByServerTime(bool isSort)
		{
			_isSortMessageByServerTime (isSort);
		}
		public override void requireDeliveryAck(bool isReq)
		{
			_requireDeliveryAck (isReq);
		}
	}

}
#endif