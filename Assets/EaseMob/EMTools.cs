using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace EaseMob{
	
	public class EMTools {

		public static EMMessage json2message(string jsonParam)
		{
			JsonData jd = JsonMapper.ToObject (jsonParam);
			return json2message (jd);
		}

		public static List<EMMessage> json2messagelist(string jsonParam)
		{
			List<EMMessage> list = new List<EMMessage>();
			JsonData jd = JsonMapper.ToObject (jsonParam);
			if(jd.IsArray){
				for (int i = 0; i < jd.Count; i++) {
									
					list.Add (json2message(jd[i]));
				}
			}
			return list;
		}

		public static EMMessage json2message(JsonData jd)
		{
			EMMessage message = new EMMessage ();
			message.mMsgId = (string)jd ["mMsgId"];
			message.mFrom = (string)jd ["mFrom"];
			message.mTo = (string)jd ["mTo"];
			message.mIsUnread = ((string)jd ["mIsUnread"]).Equals("true");
			message.mIsListened = ((string)jd ["mIsListened"]).Equals("true");
			message.mIsAcked = ((string)jd ["mIsAcked"]).Equals ("true");
			message.mIsDelivered = ((string)jd ["mIsDelivered"]).Equals("true");
			message.mLocalTime = (long)jd ["mLocalTime"];
			message.mServerTime = (long)jd ["mServerTime"];
			message.mDirection = (int)jd ["mDirection"];
			message.mStatus = (int)jd ["mStatus"];
			message.mChatType = (int)jd ["mChatType"];
			int mType = (int)jd ["mType"];
			MessageType type = (MessageType)mType;
			message.mType = type;

			if (type == MessageType.VIDEO || type == MessageType.FILE || type == MessageType.IMAGE || type == MessageType.VOICE) {
				message.mDisplayName = (string)jd ["mDisplayName"];
				message.mSecretKey = (string)jd ["mSecretKey"];
				message.mLocalPath = (string)jd ["mLocalPath"];
				message.mRemotePath = (string)jd ["mRemotePath"];
				Debug.LogError ("image");
			} 
			if (type == MessageType.TXT) { 
				message.mTxt = (string)jd ["mTxt"];
			} else if (type == MessageType.IMAGE) {
				message.mThumbnailLocalPath = (string)jd ["mThumbnailLocalPath"];
				message.mThumbnailRemotePath = (string)jd ["mThumbnailRemotePath"];
				message.mThumbnailSecretKey = (string)jd ["mThumbnailSecretKey"];
				message.mWidth = (int)jd ["mWidth"];
				message.mHeight = (int)jd ["mHeight"];
			} else if (type == MessageType.VOICE) {
				message.mDuration = (int)jd ["mDuration"];
			}
			return message;
		}

		public static List<EMGroup> json2grouplist(string jsondata)
		{
			List<EMGroup> list = new List<EMGroup> ();
			JsonData jd = JsonMapper.ToObject (jsondata);
			if (jd.IsArray) {
				for (int i = 0; i < jd.Count; i++) {
					list.Add (json2group (jd [i]));
				}
			}
			return list;
		}

		public static EMGroup json2group(string jsondata)
		{
			if (jsondata.Length == 0)
				return null;
			JsonData jd = JsonMapper.ToObject (jsondata);
			return json2group (jd);
		}
		public static EMGroup json2group(JsonData jd)
		{
			EMGroup group = new EMGroup ();
			group.mGroupId = (string)jd ["mGroupId"];
			group.mGroupName = (string)jd ["mGroupName"];
			group.mDescription = (string)jd ["mDescription"];
			group.mOwner = (string)jd ["mOwner"];
			group.mIsPublic = ((string)jd ["mIsPublic"]).Equals("true");
			group.mIsMsgBlocked = ((string)jd ["mIsMsgBlocked"]).Equals("true");
			group.mMembers = (string)jd ["mMembers"];

			return group;
		}

		public static List<EMConversation> json2conversationlist(string jsondata)
		{
			List<EMConversation> list = new List<EMConversation> ();
			JsonData jd = JsonMapper.ToObject (jsondata);
			if (jd.IsArray) {
				for (int i = 0; i < jd.Count; i++) {
					list.Add (json2conversation (jd [i]));
				}
			}
			return list;
		}

		public static EMConversation json2conversation(JsonData jd)
		{
			EMConversation conversation = new EMConversation ();
			conversation.mConversationId = (string)jd ["mConversationId"];
			int type = (int)jd ["mConversationType"];
			ConversationType ctype = (ConversationType)type;
			conversation.mConversationType = ctype;
			conversation.mUnreadMsgCount = (int)jd ["mUnreadMsgCount"];
			conversation.mExtInfo = (string)jd ["mExt"];
			string data = (string)jd ["mLatesMsg"];
			conversation.mLastMsg = json2message (data);
			if (jd ["key"] != null) {
				conversation.mKey = (string)jd ["key"];
			}
			return conversation;
		}

		public static List<string> string2list(string str)
		{
			List<string> list = new List<string> ();
			if (str != null && str.Length > 0) {
				string[] array = str.Split (',');
				foreach (string val in array) {
					list.Add (val);
				}
			}
			return list;
		}
	}
}
