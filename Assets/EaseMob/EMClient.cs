using UnityEngine;
using System.Collections.Generic;
namespace EaseMob{

	public class EMClient {
		
		private static EMClient _instance;
		private static int callbackId; 
		private static Dictionary<int,EMBaseCallback> requestedCallbackList;
		private static object lockObject = new UnityEngine.Object();


		private EMSDKInterfaceBase sdk;

		public EMBaseCallback loginCallback{ set; get; }
		public EMBaseCallback regCallback{ set; get; }
		public EMBaseCallback logoutCallback{ set; get; }
		public EMMessageListenerCallback receiveMessageCallback{ set; get; }
		public EMConnListenerCallback connListenerCallback{ set; get; }
		public EMGroupListenerCallback groupListenerCallback{ set; get; }

		public static EMClient Instance{
			get{
				if (_instance == null) {
					_instance = new EMClient ();
				}
				return _instance;
			}
		}

		private EMClient()
		{
			sdk = EMSDKInterfaceBase.Instance;
			requestedCallbackList = new Dictionary<int,EMBaseCallback> ();

		}

		public void Init()
		{
			EMSDKCallback.InitCallback ();
		}

		public int CreateAccount(string username,string password)
		{
			return sdk.createAccount (username, password);
		}

		public void Login(string username,string password,EMBaseCallback cb)
		{
			loginCallback = cb;
			sdk.login (username, password);
		}

		public void Logout (bool flag, EMBaseCallback cb)
		{
			logoutCallback = cb;
			sdk.logout (flag);
		}

		public string GetAllContactsFromServer()
		{
			return sdk.getAllContactsFromServer ();
		}

		public void SendTextMessage (string content, string to, ChatType chattype, EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.sendTextMessage (content, to, cb.CallbackId,(int)chattype);
		}

//		public void SendVoiceMessage (string path, int length, string to, ChatType chattype, EMBaseCallback cb)
//		{
//			AddCallbackToList (cb);
//			sdk.sendVoiceMessage (path, length, to, cb.CallbackId,(int)chattype);
//		}
//
//		public void SendPictureMessage (string path, bool isSrcImage, string to, ChatType chattype,EMBaseCallback cb)
//		{
//			AddCallbackToList (cb);
//			sdk.sendPictureMessage (path, isSrcImage, to, cb.CallbackId, (int)chattype);
//		}

		public void SendFileMessage (string path, string to,ChatType chattype,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.sendFileMessage (path, to, cb.CallbackId, (int)chattype);
		}

		[System.Obsolete("This method not in Use.",true)]
		public List<EMMessage> GetAllConversationMessage(string username)
		{
			string jsonData = sdk.getAllConversationMessage (username);
			return EMTools.json2messagelist (jsonData);
		}

		public List<EMMessage> GetConversationMessage(string username, string startMsgId, int pageSize)
		{
			string jsonData = sdk.getConversationMessage(username,startMsgId,pageSize);
			return EMTools.json2messagelist (jsonData);
		}

		public EMMessage GetLatestMessage (string username)
		{
			string retData = sdk.getLatestMessage (username);
			return EMTools.json2message (retData);
		}
		public int GetUnreadMsgCount (string username)
		{
			return sdk.getUnreadMsgCount(username);
		}
		public void MarkAllMessagesAsRead (string username)
		{
			sdk.markAllMessagesAsRead (username);
		}
		public void MarkMessageAsRead (string username, string messageId)
		{
			sdk.markMessageAsRead (username, messageId);
		}
		public void MarkAllConversationsAsRead ()
		{
			sdk.markAllConversationsAsRead ();
		}

		public List<EMConversation> GetAllConversations ()
		{
			string jsonData = sdk.getAllConversations();
			return EMTools.json2conversationlist (jsonData);
		}
		public bool DeleteConversation (string username, bool isDeleteHistory)
		{
			return sdk.deleteConversation(username,isDeleteHistory);
		}
		public void RemoveMessage (string username, string msgId)
		{
			sdk.removeMessage (username, msgId);
		}
		public void ImportMessages (string json)
		{
			sdk.importMessages (json);
		}

		public void DownloadAttachment(string username,string msgId,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.downloadAttachment (cb.CallbackId,username,msgId);
		}

		#region
		public void createGroup (string groupName, string desc, string[] members, string reason, int maxUsers, GroupStyle style,EMGroupCallback cb)
		{
			AddCallbackToList (cb);
			sdk.createGroup (cb.CallbackId, groupName, desc, string.Join (",", members), reason, maxUsers, (int)style);
		}
		public void addUsersToGroup (string groupId, string[] strMembers,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.addUsersToGroup (cb.CallbackId, groupId, string.Join(",",strMembers));
		}
		public void joinGroup (string groupId,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.joinGroup (cb.CallbackId, groupId);
		}
		public void leaveGroup (string groupId,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.leaveGroup (cb.CallbackId,groupId);
		}

		public void getJoinedGroupsFromServer (EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.getJoinedGroupsFromServer (cb.CallbackId);
		}
			
		public EMGroup getGroup (string groupId)
		{
			string jsondata = sdk.getGroup (groupId);
			return EMTools.json2group(jsondata);
		}
		#endregion

		#region
		public EMConversation getConversation (string cid, ConversationType type, bool createIfNotExists)
		{
			string data = sdk.getConversation (cid, (int)type, createIfNotExists);
			return EMTools.json2conversation (data);
		}

		public void deleteMessagesAsExitGroup (bool del)
		{
			sdk.deleteMessagesAsExitGroup (del);
		}
		public void isAutoAcceptGroupInvitation(bool isAuto)
		{
			sdk.isAutoAcceptGroupInvitation (isAuto);
		}
		public void isSortMessageByServerTime(bool isSort)
		{
			sdk.isSortMessageByServerTime (isSort);
		}
		public void requireDeliveryAck(bool isReq)
		{
			sdk.requireDeliveryAck (isReq);
		}
		#endregion
			
		public EMBaseCallback GetCallbackById(int callbackId)
		{
			if (requestedCallbackList.ContainsKey (callbackId)) 
			{
				return (EMBaseCallback)requestedCallbackList [callbackId];
			} 
			else 
			{
				return null;
			}
		}
			
		public void RemoveCallbackById(int callbackId)
		{
			if (requestedCallbackList.ContainsKey (callbackId)) 
			{
				requestedCallbackList.Remove(callbackId);
				Debug.LogError ("remove callbackId from list:"+callbackId);
			}
		}

		private int AddCallbackToList(EMBaseCallback callback)
		{
			lock (lockObject) 
			{
				callbackId++;
				callback.CallbackId = callbackId;
				requestedCallbackList.Add (callback.CallbackId, callback);
				return callback.CallbackId;
			}
		}


	}

}