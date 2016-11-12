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

	}
}
