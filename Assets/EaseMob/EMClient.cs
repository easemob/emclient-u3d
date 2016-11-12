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

		public void StartRecord(EMRecordCallback cb)
		{
			recordCallback = cb;
			sdk.startRecord ();
		}

		public void StopRecord()
		{
			sdk.stopRecord ();
		}

		public string GetAllConversationMessage(string username)
		{
			return sdk.getAllConversationMessage(username);
		}

		public string GetConversationMessage(string username, string startMsgId, int pageSize)
		{
			return sdk.getConversationMessage(username,startMsgId,pageSize);
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

		public string GetAllConversations ()
		{
			return sdk.getAllConversations();
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
		public void createGroup (string groupName, string desc, string[] members, string reason, int maxUsers, int style)
		{
			sdk.createGroup (groupName, desc, string.Join (",", members), reason, maxUsers, style);
		}
		public void addUsersToGroup (string groupId, string[] strMembers)
		{
			sdk.addUsersToGroup (groupId, string.Join(",",strMembers));
		}
		public void inviteUser (string groupId, string[] beInvitedUsernames, string reason)
		{
			sdk.inviteUser (groupId, string.Join (",", beInvitedUsernames), reason);
		}
		public void removeUserFromGroup (string groupId, string username)
		{
			sdk.removeUserFromGroup (groupId, username);
		}
		public void joinGroup (string groupId)
		{
			sdk.joinGroup (groupId);
		}
		public void applyJoinToGroup (string groupId, string reason)
		{
			sdk.applyJoinToGroup (groupId, reason);
		}
		public void leaveGroup (string groupId)
		{
			sdk.leaveGroup (groupId);
		}
		public void destroyGroup (string groupId)
		{
			sdk.destroyGroup (groupId);
		}
		public void getJoinedGroupsFromServer ()
		{
			sdk.getJoinedGroupsFromServer ();
		}
		public List<EMGroup> getAllGroups (){
			List<EMGroup> list = new List<EMGroup>();
			string jsondata = sdk.getAllGroups ();
			return EMTools.json2grouplist(jsondata);
		}
		public void changeGroupName (string groupId,string groupName)
		{
			sdk.changeGroupName (groupId, groupName);
		}
		public EMGroup getGroup (string groupId)
		{
			string jsondata = sdk.getGroup (groupId);
			return EMTools.json2group(jsondata);
		}
		public void blockGroupMessage (string groupId)
		{
			sdk.blockGroupMessage (groupId);
		}
		public void unblockGroupMessage (string groupId)
		{
			sdk.unblockGroupMessage (groupId);
		}
		public void blockUser (string groupId, string username)
		{
			sdk.blockUser (groupId, username);
		}
		public void unblockUser(string groupId,string username)
		{
			sdk.unblockUser (groupId, username);
		}
		public void getBlockedUsers(string groupId)
		{
			sdk.getBlockedUsers (groupId);
		}
			
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