using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace EaseMob{
	
	public class EMTools {

		public static List<EMMessage> json2messagelist(string jsonParam)
		{
			Debug.LogError ("receive List:" + jsonParam);
			List<EMMessage> list = new List<EMMessage>();
			JsonData jd = JsonMapper.ToObject (jsonParam);
			if(jd.IsArray){
				for (int i = 0; i < jd.Count; i++) {
					EMMessage message = new EMMessage ();
					message.mMsgId = (string)jd [i] ["mMsgId"];
					message.mFrom = (string)jd [i] ["mFrom"];
					message.mTo = (string)jd [i] ["mTo"];
					message.mIsUnread = ((string)jd [i] ["mIsUnread"]).Equals("true");
					message.mIsListened = ((string)jd [i] ["mIsListened"]).Equals("true");
					message.mIsAcked = ((string)jd [i] ["mIsAcked"]).Equals ("true");
					message.mIsDelivered = ((string)jd [i] ["mIsDelivered"]).Equals("true");
					message.mLocalTime = (long)jd [i] ["mLocalTime"];
					message.mServerTime = (long)jd [i] ["mServerTime"];
					message.mDirection = (int)jd [i] ["mDirection"];
					message.mStatus = (int)jd [i] ["mStatus"];
					message.mChatType = (int)jd [i] ["mChatType"];
					int mType = (int)jd [i] ["mType"];
					MessageType type = (MessageType)mType;
					message.mType = type;

					if (type == MessageType.VIDEO || type == MessageType.FILE || type == MessageType.IMAGE || type == MessageType.VOICE) {
						message.mDisplayName = (string)jd [i] ["mDisplayName"];
						message.mSecretKey = (string)jd [i] ["mSecretKey"];
						message.mLocalPath = (string)jd [i] ["mLocalPath"];
						message.mRemotePath = (string)jd [i] ["mRemotePath"];
						Debug.LogError ("image");
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
			}
			return list;
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
			group.mIsPublic = ((string)jd ["mIsPublic"]).Equals("true");
			group.mIsAllowInvites = ((string)jd ["mIsAllowInvites"]).Equals("true");
			group.mIsMsgBlocked = ((string)jd ["mIsMsgBlocked"]).Equals("true");
			group.mOwner = (string)jd ["mOwner"];
			group.mMembers = (string)jd ["mMembers"];
			group.mDescription = (string)jd ["mDescription"];

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
			conversation.mUsername = (string)jd ["mUsername"];
			conversation.mIsGroup = ((string)jd ["mIsGroup"]).Equals("true");
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
