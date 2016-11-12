using UnityEngine;
using System;

namespace EaseMob{

	public class EMSDKInterfaceAndroid : EMSDKInterfaceBase {

		private AndroidJavaObject jo;

		public EMSDKInterfaceAndroid(){

			using (AndroidJavaClass aj = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
				jo = aj.GetStatic<AndroidJavaObject> ("currentActivity");
			}
		}

		private T SDKCall<T>(string method, params object[] param)
		{
			try {
				return jo.Call<T> (method, param);
			} catch (Exception e) {
				Debug.LogError (e);
			}
			return default(T);
		}

		private void SDKCall(string method, params object[] param)
		{
			try {
				jo.Call(method, param);
			} catch (Exception e) {
				Debug.LogError (e);
			}
		}

		public override int createAccount (string username, string password)
		{
			return SDKCall<int> ("createAccount",username,password);
		}

		public override void login (string username, string password)
		{
			SDKCall ("login",username,password);
		}

		public override void logout (bool flag)
		{
			SDKCall ("logout", flag);
		}

		public override void sendTextMessage (string content, string to, int callbackId, int chattype)
		{
			SDKCall ("sendTextMessage", content, to,callbackId, chattype);
		}
		public override void sendVoiceMessage (string path, int length, string to, int callbackId, int chattype)
		{
			SDKCall ("sendVoiceMessage", path, length, to,callbackId, chattype);
		}
		public override void sendPictureMessage (string path, bool isSrcImage, string to, int callbackId, int chattype)
		{
			SDKCall ("sendPictureMessage", path, isSrcImage, to,callbackId, chattype);
		}
		public override void sendVideoMessage (string path, string thumbPath,int length, string to, int callbackId, int chattype)
		{
			SDKCall ("sendVideoMessage", path, thumbPath, length, to,callbackId, chattype);
		}
		public override void sendLocationMessage (double latitude, double longitude, string locationAddress, string to, int callbackId, int chattype)
		{
			SDKCall ("sendLocationMessage", latitude, longitude, locationAddress, to,callbackId, chattype);
		}
		public override void sendFileMessage (string path, string to, int callbackId, int chattype)
		{
			SDKCall ("sendFileMessage", path, to,callbackId, chattype);
		}
		public override string getAllContactsFromServer()
		{
			return SDKCall<string> ("getAllContactsFromServer");
		}
		public override void startRecord()
		{
			SDKCall ("startRecord");
		}
		public override void stopRecord()
		{
			SDKCall ("stopRecord");
		}

		public override string getAllConversationMessage(string username)
		{
			return SDKCall<string>("getAllConversationMessage",username);
		}

		public override string getConversationMessage(string username, string startMsgId, int pageSize)
		{
			return SDKCall<string>("getConversationMessage",username,startMsgId,pageSize);
		}
		public override int getUnreadMsgCount (string username)
		{
			return SDKCall<int>("getUnreadMsgCount",username);
		}
		public override void markAllMessagesAsRead (string username)
		{
			SDKCall ("markAllMessagesAsRead", username);
		}
		public override void markMessageAsRead (string username, string messageId)
		{
			SDKCall ("markMessageAsRead", username, messageId);
		}
		public override void markAllConversationsAsRead ()
		{
			SDKCall ("markAllConversationsAsRead");
		}
		public override int getAllMsgCount (string username)
		{
			return SDKCall<int>("getAllMsgCount",username);
		}
		public override int getAllMessagesSize (string username)
		{
			return SDKCall<int> ("getAllMessagesSize", username);
		}
		public override string getAllConversations ()
		{
			return SDKCall<string>("getAllConversations");
		}
		public override bool deleteConversation (string username, bool isDeleteHistory)
		{
			return SDKCall<bool>("deleteConversation",username,isDeleteHistory);
		}
		public override void removeMessage (string username, string msgId)
		{
			SDKCall ("removeMessage", username, msgId);
		}
		public override void importMessages (string json)
		{
			SDKCall ("importMessages", json);
		}
	}

}
