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
		public EMRecordCallback recordCallback{ set; get; }
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

		public void SendVoiceMessage (string path, int length, string to, ChatType chattype, EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.sendVoiceMessage (path, length, to, cb.CallbackId,(int)chattype);
		}

		public void SendPictureMessage (string path, bool isSrcImage, string to, ChatType chattype,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.sendPictureMessage (path, isSrcImage, to, cb.CallbackId, (int)chattype);
		}

		public void SendFileMessage (string path, string to,ChatType chattype,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.sendFileMessage (path, to, cb.CallbackId, (int)chattype);
		}

		public void StartRecord(EMRecordCallback cb)
		{
			recordCallback = cb;
			sdk.startRecord ();
		}

		public void StopRecord()
		{
			sdk.stopRecord ();
		}

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
		public int GetAllMsgCount (string username)
		{
			return sdk.getAllMsgCount(username);
		}
		public int GetAllMessagesSize (string username)
		{
			return sdk.getAllMessagesSize(username);
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
		public void inviteUser (string groupId, string[] beInvitedUsernames, string reason,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.inviteUser (cb.CallbackId, groupId, string.Join (",", beInvitedUsernames), reason);
		}
		public void removeUserFromGroup (string groupId, string username, EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.removeUserFromGroup (cb.CallbackId, groupId, username);
		}
		public void joinGroup (string groupId,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.joinGroup (cb.CallbackId, groupId);
		}
		public void applyJoinToGroup (string groupId, string reason,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.applyJoinToGroup (cb.CallbackId, groupId, reason);
		}
		public void leaveGroup (string groupId,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.leaveGroup (cb.CallbackId,groupId);
		}
		public void destroyGroup (string groupId,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.destroyGroup (cb.CallbackId,groupId);
		}
		public void getJoinedGroupsFromServer (EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.getJoinedGroupsFromServer (cb.CallbackId);
		}
		public List<EMGroup> getAllGroups (){
			string jsondata = sdk.getAllGroups ();
			return EMTools.json2grouplist(jsondata);
		}
		public void changeGroupName (string groupId,string groupName,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.changeGroupName (cb.CallbackId,groupId, groupName);
		}
		public EMGroup getGroup (string groupId)
		{
			string jsondata = sdk.getGroup (groupId);
			return EMTools.json2group(jsondata);
		}
		public void blockGroupMessage (string groupId,EMBaseCallback cb)
		{
			AddCallbackToList(cb);
			sdk.blockGroupMessage (cb.CallbackId, groupId);
		}
		public void unblockGroupMessage (string groupId,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.unblockGroupMessage (cb.CallbackId, groupId);
		}
		public void blockUser (string groupId, string username,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.blockUser (cb.CallbackId,groupId, username);
		}
		public void unblockUser(string groupId,string username,EMBaseCallback cb)
		{
			AddCallbackToList (cb);
			sdk.unblockUser (cb.CallbackId, groupId, username);
		}
		public void getBlockedUsers(string groupId,EMGroupCallback cb)
		{
			AddCallbackToList (cb);
			sdk.getBlockedUsers (cb.CallbackId, groupId);
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