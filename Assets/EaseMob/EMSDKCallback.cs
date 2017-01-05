using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;

namespace EaseMob{

	public class EMSDKCallback : MonoBehaviour {

		private static readonly string CB_OBJ = "emsdk_cb_object";

		private static EMSDKCallback _instance;

		private static object _lock = new object();

		//初始化回调对象
		public static void InitCallback()
		{
			lock (_lock)
			{
				if (_instance == null)
				{
					GameObject callback = GameObject.Find(CB_OBJ);
					if (callback == null)
					{
						callback = new GameObject(CB_OBJ);
						UnityEngine.Object.DontDestroyOnLoad(callback);
						_instance = callback.AddComponent<EMSDKCallback>();
					}
					else
					{
						_instance = callback.GetComponent<EMSDKCallback>();
					}

				}
			}
		}

		public static EMSDKCallback Instance
		{
			get{
				if (_instance == null) {
					InitCallback ();
					//throw new System.Exception ("instance is null");
				}
				return _instance;
			}
		}

		public void LoginCallback(string jsonParam)
		{
			if (EMClient.Instance.loginCallback == null) {
				throw new System.Exception ("NOT set login callback");
			}
			JSONNode jsonData = JSON.Parse (jsonParam);
			string on = jsonData ["on"].Value;
			if (on.Equals ("success")) {
				EMClient.Instance.loginCallback.onSuccessCallback ();
			} else if (on.Equals ("progress")) {
				EMClient.Instance.loginCallback.onProgressCallback (jsonData ["progress"].AsInt, jsonData ["status"].Value);
			} else if (on.Equals ("error")) {
				EMClient.Instance.loginCallback.onErrorCallback (jsonData ["code"].AsInt, jsonData ["message"].Value);
			} else {
				EMClient.Instance.loginCallback.onErrorCallback (-999999, "unknown error");
			}
		}

		public void LogoutCallback(string jsonParam)
		{
			if (EMClient.Instance.logoutCallback == null) {
				throw new System.Exception ("NOT set logout callback");
			}
			JSONNode jsonData = JSON.Parse (jsonParam);
			string on = jsonData ["on"].Value;
			if (on.Equals ("success")) {
				EMClient.Instance.logoutCallback.onSuccessCallback ();
			} else if (on.Equals ("progress")) {
				EMClient.Instance.logoutCallback.onProgressCallback (jsonData ["progress"].AsInt, jsonData ["status"].Value);
			} else if (on.Equals ("error")) {
				EMClient.Instance.logoutCallback.onErrorCallback (jsonData ["code"].AsInt, jsonData ["message"].Value);
			} else {
				EMClient.Instance.logoutCallback.onErrorCallback (-999999, "unknown error");
			}
		}

		//TODO 成功接收消息回到后应该从消息列表中删除消息【 invoke EMClient.Instance.RemoveCallbackById（）】
		public void SendMessageCallback(string jsonParam)
		{
			JSONNode jsonData = JSON.Parse (jsonParam);
			int callbackId = jsonData ["callbackid"].AsInt;
			Debug.LogError ("callbackId=" + callbackId);
			EMBaseCallback cb = EMClient.Instance.GetCallbackById (callbackId);
			if (cb != null) {
				string on = jsonData ["on"].Value;
				if (on.Equals ("success")) {
					cb.onSuccessCallback ();
					EMClient.Instance.RemoveCallbackById (callbackId);
				} else if (on.Equals ("progress")) {
					cb.onProgressCallback (jsonData ["progress"].AsInt, jsonData ["status"].Value);
				} else if (on.Equals ("error")) {
					cb.onErrorCallback (jsonData ["code"].AsInt, jsonData ["message"].Value);
				} else {
					cb.onErrorCallback (-999999, "unknown error");
				}
			}
		}

//		public void StopRecordCallback(string jsonParam)
//		{
//			if (EMClient.Instance.recordCallback == null) {
//				throw new System.Exception ("NOT set record callback");
//			}
//			JsonData jsonData = JsonMapper.ToObject (jsonParam);
//			string path = (string)jsonData ["path"];
//			int length = (int)jsonData ["length"];
//			EMClient.Instance.recordCallback.onStopRecordCallback (path, length);
//		}

		public void MessageReceivedCallback(string jsonParam)
		{
			if (EMClient.Instance.receiveMessageCallback != null) {
				EMClient.Instance.receiveMessageCallback.onMessageReceivedCallback(EMTools.json2messagelist(jsonParam));
			}
		}

		public void MessageReadAckReceivedCallback(string jsonParam)
		{
			if (EMClient.Instance.receiveMessageCallback != null) {
				EMClient.Instance.receiveMessageCallback.onMessageReadAckReceivedCallback (EMTools.json2messagelist(jsonParam));
			}
		}

		public void MessageDeliveryAckReceivedCallback(string jsonParam)
		{
			if (EMClient.Instance.receiveMessageCallback != null) {
				EMClient.Instance.receiveMessageCallback.onMessageDeliveryAckReceivedCallback (EMTools.json2messagelist(jsonParam));
			}
		}

		public void MessageChangedCallback(string jsonParam)
		{
			if (EMClient.Instance.receiveMessageCallback != null) {
				EMClient.Instance.receiveMessageCallback.onMessageChangedCallback (EMTools.json2messagelist(jsonParam));
			}
		}

		public void CmdMessageReceivedCallback(string jsonParam)
		{
			if (EMClient.Instance.receiveMessageCallback != null) {
				EMClient.Instance.receiveMessageCallback.onCmdMessageReceivedCallback (EMTools.json2messagelist(jsonParam));
			}
				
		}

		public void ConnectedCallback(string jsonParam)
		{
			if (EMClient.Instance.connListenerCallback != null)
				EMClient.Instance.connListenerCallback.onConnectionCallback ();
		}

		public void DisconnectedCallback(string code)
		{
			if (EMClient.Instance.connListenerCallback != null)
				EMClient.Instance.connListenerCallback.onDisconnectedCallback (code);
		}

		//group listener callback
		public void UserRemovedCallback(string jsonParam)
		{
			if (EMClient.Instance.groupListenerCallback == null) {
				throw new System.Exception ("NOT set group callback");
			}
			JSONNode jsonData = JSON.Parse (jsonParam);
			string groupId = (string)jsonData ["groupId"];
			string groupName = (string)jsonData ["groupName"];
			EMClient.Instance.groupListenerCallback.onUserRemovedCallback (groupId, groupName);
		}

		public void InvitationReceivedCallback(string jsonParam)
		{
			if (EMClient.Instance.groupListenerCallback == null) {
				throw new System.Exception ("NOT set group callback");
			}
			JSONNode jsonData = JSON.Parse (jsonParam);
			string groupId = (string)jsonData ["groupId"];
			string groupName = (string)jsonData ["groupName"];
			string inviter = (string)jsonData ["inviter"];
			string reason = (string)jsonData ["reason"];
			EMClient.Instance.groupListenerCallback.onInvitationReceived (groupId, groupName, inviter, reason);
		}

		public void InvitationDeclinedCallback(string jsonParam)
		{
			if (EMClient.Instance.groupListenerCallback == null) {
				throw new System.Exception ("NOT set group callback");
			}
			JSONNode jsonData = JSON.Parse (jsonParam);
			string groupId = (string)jsonData ["groupId"];
			string invitee = (string)jsonData ["invitee"];
			string reason = (string)jsonData ["reason"];
			EMClient.Instance.groupListenerCallback.onInvitationDeclined (groupId, invitee, reason);
		}

		public void InvitationAcceptedCallback(string jsonParam)
		{
			if (EMClient.Instance.groupListenerCallback == null) {
				throw new System.Exception ("NOT set group callback");
			}
			JSONNode jsonData = JSON.Parse (jsonParam);
			string groupId = (string)jsonData ["groupId"];
			string inviter = (string)jsonData ["inviter"];
			string reason = (string)jsonData ["reason"];
			EMClient.Instance.groupListenerCallback.onInvitationAccepted (groupId, inviter, reason);
		}

		public void GroupDestroyedCallback(string jsonParam)
		{
			if (EMClient.Instance.groupListenerCallback == null) {
				throw new System.Exception ("NOT set group callback");
			}
			JSONNode jsonData = JSON.Parse (jsonParam);
			string groupId = jsonData ["groupId"].Value;
			string groupName = jsonData ["groupName"].Value;
			EMClient.Instance.groupListenerCallback.onGroupDestroyed (groupId, groupName);
		}

		public void AutoAcceptInvitationFromGroupCallback(string jsonParam)
		{
			if (EMClient.Instance.groupListenerCallback == null) {
				throw new System.Exception ("NOT set group callback");
			}
			JSONNode jsonData = JSON.Parse (jsonParam);
			string groupId = jsonData ["groupId"].Value;
			string inviter = jsonData ["inviter"].Value;
			string inviteMessage = jsonData ["inviteMessage"].Value;
			EMClient.Instance.groupListenerCallback.onAutoAcceptInvitationFromGroup (groupId, inviter, inviteMessage);
		}

		public void ApplicationReceivedCallback(string jsonParam)
		{
			if (EMClient.Instance.groupListenerCallback == null) {
				throw new System.Exception ("NOT set group callback");
			}
			JSONNode jsonData = JSON.Parse (jsonParam);
			string groupId = jsonData ["groupId"].Value;
			string groupName = jsonData ["groupName"].Value;
			string applicant = jsonData ["applicant"].Value;
			string reason = jsonData ["reason"].Value;
			EMClient.Instance.groupListenerCallback.onApplicationReceived (groupId, groupName, applicant, reason);
		}

		public void ApplicationDeclinedCallback(string jsonParam)
		{
			if (EMClient.Instance.groupListenerCallback == null) {
				throw new System.Exception ("NOT set group callback");
			}
			JSONNode jsonData = JSON.Parse (jsonParam);
			string groupId = jsonData ["groupId"].Value;
			string groupName = jsonData ["groupName"].Value;
			string decliner = jsonData ["decliner"].Value;
			string reason = jsonData ["reason"].Value;
			EMClient.Instance.groupListenerCallback.onApplicationDeclined (groupId, groupName, decliner, reason);
		}

		public void ApplicationAcceptCallback(string jsonParam)
		{
			if (EMClient.Instance.groupListenerCallback == null) {
				throw new System.Exception ("NOT set group callback");
			}
			JSONNode jsonData = JSON.Parse (jsonParam);
			string groupId = jsonData ["groupId"].Value;
			string groupName = jsonData ["groupName"].Value;
			string accepter = jsonData ["accepter"].Value;
			EMClient.Instance.groupListenerCallback.onApplicationAccept (groupId, groupName, accepter);
		}

		//
		public void CreateGroupCallback(string jsonParam)
		{
			JSONNode jsonData = JSON.Parse (jsonParam);
			int callbackId = jsonData ["callbackid"].AsInt;
			string on = jsonData ["on"].Value;
			EMGroupCallback cb = (EMGroupCallback)EMClient.Instance.GetCallbackById (callbackId);

			if (on.Equals ("success")) {
				EMGroup group = EMTools.json2group (jsonData ["data"].Value);
				cb.onSuccessCreateGroupCallback (group);
				EMClient.Instance.RemoveCallbackById (callbackId);
			} else if (on.Equals ("error")) {
				cb.onErrorCallback (jsonData ["code"].AsInt, jsonData ["message"].Value);
			}
		}

		public void AddUsersToGroupCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void InviteUserCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void RemoveUserFromGroupCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void JoinGroupCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void ApplyJoinToGroupCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void LeaveGroupCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void DestroyGroupCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void GetJoinedGroupsFromServerCallback(string jsonParam)
		{
			JSONNode jsonData = JSON.Parse (jsonParam);
			int callbackId = jsonData ["callbackid"].AsInt;
			string on = jsonData ["on"].Value;
			EMGroupCallback cb = (EMGroupCallback)EMClient.Instance.GetCallbackById (callbackId);

			if (on.Equals ("success")) {
				List<EMGroup> groups = EMTools.json2grouplist (jsonData ["data"].Value);
				cb.onSuccessGetGroupListCallback (groups);
				EMClient.Instance.RemoveCallbackById (callbackId);
			} else if (on.Equals ("error")) {
				cb.onErrorCallback (jsonData ["code"].AsInt, jsonData ["message"].Value);
			}
		}

		public void ChangeGroupNameCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void BlockGroupMessageCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void UnblockGroupMessageCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void BlockUserCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void UnblockUserCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void GetBlockedUsersCallback(string jsonParam)
		{
			JSONNode jsonData = JSON.Parse (jsonParam);
			int callbackId = jsonData ["callbackid"].AsInt;
			string on = jsonData ["on"].Value;
			EMGroupCallback cb = (EMGroupCallback)EMClient.Instance.GetCallbackById (callbackId);

			if (on.Equals ("success")) {
				string strUsers = jsonData ["data"].Value;
				cb.onSuccessGetBlockedUsers(EMTools.string2list(strUsers));
				EMClient.Instance.RemoveCallbackById (callbackId);
			} else if (on.Equals ("error")) {
				cb.onErrorCallback (jsonData ["code"].AsInt, jsonData ["message"].Value);
			}
		}

		public void DownloadAttachmentCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void ApproveJoinGroupRequestCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void DeclineJoinGroupRequestCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}

		public void AcceptInvitationFromGroupCallback(string jsonParam)
		{
			JSONNode jsonData = JSON.Parse (jsonParam);
			int callbackId = jsonData ["callbackid"].AsInt;
			string on = jsonData ["on"].Value;
			EMGroupCallback cb = (EMGroupCallback)EMClient.Instance.GetCallbackById (callbackId);

			if (on.Equals ("success")) {
				EMGroup group = EMTools.json2group (jsonData ["data"].Value);
				cb.onSuccessJoinGroupCallback (group);
				EMClient.Instance.RemoveCallbackById (callbackId);
			} else if (on.Equals ("error")) {
				cb.onErrorCallback (jsonData ["code"].AsInt, jsonData ["message"].Value);
			}
		}

		public void DeclineInvitationFromGroupCallback(string jsonParam)
		{
			doBaseCallback (jsonParam);
		}
			
		private void doBaseCallback(string jsonParam)
		{
			JSONNode jsonData = JSON.Parse (jsonParam);
			int callbackId = jsonData ["callbackid"].AsInt;
			string on = (string)jsonData ["on"];
			EMBaseCallback cb = EMClient.Instance.GetCallbackById (callbackId);
			if (on.Equals ("success")) {
				cb.onSuccessCallback ();
				EMClient.Instance.RemoveCallbackById (callbackId);
			} else if (on.Equals ("progress")) {
				cb.onProgressCallback (jsonData ["progress"].AsInt, jsonData ["status"].Value);
			} else if (on.Equals ("error")) {
				cb.onErrorCallback (jsonData ["code"].AsInt, jsonData ["message"].Value);
				EMClient.Instance.RemoveCallbackById (callbackId);
			} else {
				cb.onErrorCallback (-999999, "unknown error");
				EMClient.Instance.RemoveCallbackById (callbackId);
			}
		}
			

	}
}
