#if UNITY_ANDROID
using UnityEngine;

namespace EaseMob{

	public class EMSDKInterfaceAndroid : EMSDKInterfaceBase {

		private AndroidJavaObject activity;
		private AndroidJavaObject application;


		private AndroidJavaObject jo;

		public EMSDKInterfaceAndroid(){

			using (AndroidJavaClass ajc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
				activity = ajc.GetStatic<AndroidJavaObject> ("currentActivity");
				application = activity.Call<AndroidJavaObject>("getApplicationContext");
			}

			using (AndroidJavaClass aj = new AndroidJavaClass ("com.easemob.sdk.EMSdkLib")) {
				aj.CallStatic<AndroidJavaObject> ("getInstance");
				jo = aj.GetStatic<AndroidJavaObject> ("instance");
				jo.Call ("onApplicationCreate", application);
			}
		}

		private T SDKCall<T>(string method, params object[] param)
		{
			try {
				return jo.Call<T> (method, param);
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
			return default(T);
		}

		private void SDKCall(string method, params object[] param)
		{
			try {
				jo.Call(method, param);
			} catch (System.Exception e) {
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

		public override void sendTextMessage (string content, string to, int callbackId, int chattype, string extjson = "")
		{
			SDKCall ("sendTextMessage", content, to,callbackId, chattype, extjson);
		}

//		public override void sendVoiceMessage (string path, int length, string to, int callbackId, int chattype)
//		{
//			SDKCall ("sendVoiceMessage", path, length, to,callbackId, chattype);
//		}
//		public override void sendPictureMessage (string path, bool isSrcImage, string to, int callbackId, int chattype)
//		{
//			SDKCall ("sendPictureMessage", path, isSrcImage, to,callbackId, chattype);
//		}
//		public override void sendVideoMessage (string path, string thumbPath,int length, string to, int callbackId, int chattype)
//		{
//			SDKCall ("sendVideoMessage", path, thumbPath, length, to,callbackId, chattype);
//		}
//		public override void sendLocationMessage (double latitude, double longitude, string locationAddress, string to, int callbackId, int chattype)
//		{
//			SDKCall ("sendLocationMessage", latitude, longitude, locationAddress, to,callbackId, chattype);
//		}
		public override void sendFileMessage (string path, string to, int callbackId, int chattype, string extjson = "")
		{
			SDKCall ("sendFileMessage", path, to,callbackId, chattype, extjson);
		}

		public override string getAllContactsFromServer()
		{
			return SDKCall<string> ("getAllContactsFromServer");
		}
		public override string getAllConversationMessage(string username)
		{
			return SDKCall<string>("getAllConversationMessage",username);
		}

		public override string getConversationMessage(string username, string startMsgId, int pageSize)
		{
			return SDKCall<string>("getConversationMessage",username,startMsgId,pageSize);
		}

		public override string getLatestMessage (string username)
		{
			return SDKCall<string> ("getLatestMessage",username);
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
		public override void createGroup (int callbackId,string groupName, string desc, string strMembers, string reason, int maxUsers, int style)
		{
			SDKCall ("createGroup",callbackId, groupName, desc, strMembers, reason, maxUsers, style);
		}
		public override void addUsersToGroup (int callbackId, string groupId, string strMembers)
		{
			SDKCall ("addUsersToGroup",callbackId, groupId, strMembers);
		}
		public override void joinGroup (int callbackId,string groupId)
		{
			SDKCall ("joinGroup",callbackId, groupId);
		}
		public override void leaveGroup (int callbackId,string groupId)
		{
			SDKCall ("leaveGroup",callbackId, groupId);
		}
		public override void getJoinedGroupsFromServer (int callbackId)
		{
			SDKCall ("getJoinedGroupsFromServer",callbackId);
		}
		public override string getGroup (string groupId)
		{
			return SDKCall<string> ("getGroup", groupId);
		}

		public override void downloadAttachment(int callbackId,string username,string msgId)
		{
			SDKCall ("downloadAttachment",callbackId,username,msgId);
		}

		public override string getConversation (string cid, int type, bool createIfNotExists)
		{
			return SDKCall<string> ("getConversation",cid,type,createIfNotExists);
		}
		public override void deleteMessagesAsExitGroup (bool del)
		{
			SDKCall ("deleteMessagesAsExitGroup",del);
		}
		public override void isAutoAcceptGroupInvitation(bool isAuto)
		{
			SDKCall ("isAutoAcceptGroupInvitation",isAuto);
		}
		public override void isSortMessageByServerTime(bool isSort)
		{
			SDKCall ("isSortMessageByServerTime",isSort);
		}
		public override void requireDeliveryAck(bool isReq)
		{
			SDKCall ("requireDeliveryAck",isReq);
		}
	}

}
#endif