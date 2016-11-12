using UnityEngine;
using System.Collections;
using LitJson;
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
			JsonData jsonData = JsonMapper.ToObject (jsonParam);
			string on = (string)jsonData ["on"];
			if (on.Equals ("success")) {
				EMClient.Instance.loginCallback.onSuccessCallback ();
			} else if (on.Equals ("progress")) {
				EMClient.Instance.loginCallback.onProgressCallback ((int)jsonData ["progress"], (string)jsonData ["status"]);
			} else if (on.Equals ("error")) {
				EMClient.Instance.loginCallback.onErrorCallback ((int)jsonData ["code"], (string)jsonData ["message"]);
			} else {
				EMClient.Instance.loginCallback.onErrorCallback (-999999, "unknown error");
			}
		}

		public void LogoutCallback(string jsonParam)
		{
			if (EMClient.Instance.logoutCallback == null) {
				throw new System.Exception ("NOT set logout callback");
			}
			JsonData jsonData = JsonMapper.ToObject (jsonParam);
			string on = (string)jsonData ["on"];
			if (on.Equals ("success")) {
				EMClient.Instance.logoutCallback.onSuccessCallback ();
			} else if (on.Equals ("progress")) {
				EMClient.Instance.logoutCallback.onProgressCallback ((int)jsonData ["progress"], (string)jsonData ["status"]);
			} else if (on.Equals ("error")) {
				EMClient.Instance.logoutCallback.onErrorCallback ((int)jsonData ["code"], (string)jsonData ["message"]);
			} else {
				EMClient.Instance.logoutCallback.onErrorCallback (-999999, "unknown error");
			}
		}

		//TODO 成功接收消息回到后应该从消息列表中删除消息【 invoke EMClient.Instance.RemoveCallbackById（）】
		public void SendMessageCallback(string jsonParam)
		{
			JsonData jsonData = JsonMapper.ToObject (jsonParam);
			int callbackId = (int)jsonData ["callbackid"];
			Debug.LogError ("callbackId=" + callbackId);
			EMBaseCallback cb = EMClient.Instance.GetCallbackById (callbackId);
			if (cb != null) {
				string on = (string)jsonData ["on"];
				if (on.Equals ("success")) {
					cb.onSuccessCallback ();
				} else if (on.Equals ("progress")) {
					cb.onProgressCallback ((int)jsonData ["progress"], (string)jsonData ["status"]);
				} else if (on.Equals ("error")) {
					cb.onErrorCallback ((int)jsonData ["code"], (string)jsonData ["message"]);
				} else {
					cb.onErrorCallback (-999999, "unknown error");
				}
			}
		}

		public void StopRecordCallback(string jsonParam)
		{
			if (EMClient.Instance.recordCallback == null) {
				throw new System.Exception ("NOT set record callback");
			}
			JsonData jsonData = JsonMapper.ToObject (jsonParam);
			string path = (string)jsonData ["path"];
			int length = (int)jsonData ["length"];
			EMClient.Instance.recordCallback.onStopRecordCallback (path, length);
		}

		public void MessageReceivedCallback(string jsonParam)
		{
			if (EMClient.Instance.receiveMessageCallback != null) {
				EMClient.Instance.receiveMessageCallback.onMessageReceivedCallback(json2messagelist(jsonParam));
			}
		}

		public void MessageReadAckReceivedCallback(string jsonParam)
		{
			if (EMClient.Instance.receiveMessageCallback != null) {
				EMClient.Instance.receiveMessageCallback.onMessageReadAckReceivedCallback (json2messagelist(jsonParam));
			}
		}

		public void MessageDeliveryAckReceivedCallback(string jsonParam)
		{
			if (EMClient.Instance.receiveMessageCallback != null) {
				EMClient.Instance.receiveMessageCallback.onMessageDeliveryAckReceivedCallback (json2messagelist(jsonParam));
			}
		}

		public void MessageChangedCallback(string jsonParam)
		{
			if (EMClient.Instance.receiveMessageCallback != null) {
				EMClient.Instance.receiveMessageCallback.onMessageChangedCallback (json2messagelist(jsonParam));
			}
		}

		public void CmdMessageReceivedCallback(string jsonParam)
		{
			if (EMClient.Instance.receiveMessageCallback != null) {
				EMClient.Instance.receiveMessageCallback.onCmdMessageReceivedCallback (json2messagelist(jsonParam));
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

		private List<EMMessage> json2messagelist(string jsonParam)
		{
			Debug.LogError ("receive List:" + jsonParam);
			List<EMMessage> list = new List<EMMessage>();
			JsonData jd = JsonMapper.ToObject (jsonParam);
			for (int i = 0; i < jd.Count; i++) {
				EMMessage message = new EMMessage ();
				message.mMsgId = (string)jd [i] ["mMsgId"];
				message.mFrom = (string)jd [i] ["mFrom"];
				message.mTo = (string)jd [i] ["mTo"];
				message.mIsUnread = (bool)jd [i] ["mIsUnread"];
				message.mIsListened = (bool)jd [i] ["mIsListened"];
				message.mIsAcked = (bool)jd [i] ["mIsAcked"];
				message.mIsDelivered = (bool)jd [i] ["mIsDelivered"];
				message.mLocalTime = (long)jd [i] ["mLocalTime"];
				message.mServerTime = (long)jd [i] ["mServerTime"];
				message.mDirection = (int)jd [i] ["mDirection"];
				message.mStatus = (int)jd [i] ["mStatus"];
				message.mChatType = (int)jd [i] ["mChatType"];
				int  mType = (int) jd[i] ["mType"];
				MessageType type = (MessageType)mType;
				if (type == MessageType.VIDEO || type == MessageType.FILE || type == MessageType.IMAGE || type == MessageType.VOICE) {
					message.mDisplayName = (string)jd [i] ["mDisplayName"];
					message.mSecretKey = (string)jd [i] ["mSecretKey"];
					message.mLocalPath = (string)jd [i] ["mLocalPath"];
					message.mRemotePath = (string)jd [i] ["mRemotePath"];
				} 
				if (type == MessageType.TXT) { 
					message.mTxt = (string)jd [i] ["mTxt"];
				} else if (type == MessageType.IMAGE) {
					message.mThumbnailLocalPath = (string)jd [i] ["mThumbnailLocalPath"];
					message.mThumbnailRemotePath = (string)jd [i] ["mThumbnailRemotePath"];
					message.mThumbnailSecretKey = (string)jd [i] ["mThumbnailSecretKey"];
					message.mWidth = (int)jd [i] ["mWidth"];
					message.mHeight = (int)jd [i] ["mHeight"];
				} else if (type == MessageType.VOICE) {
					message.mDuration = (int)jd [i] ["mDuration"];
				}				
				list.Add (message);
			}
			return list;
		}

	}
}
