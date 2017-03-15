using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace EaseMob{
	
	public class EMTools {

		public static EMMessage json2message(string jsonParam)
		{
			if(jsonParam == null || jsonParam.Length <= 3)
				return null;
			JSONNode jd = JSON.Parse (jsonParam);
			return json2message (jd);
		}

		public static List<EMMessage> json2messagelist(string jsonParam)
		{
			List<EMMessage> list = new List<EMMessage>();
			JSONNode node = JSON.Parse (jsonParam);
			JSONArray jd = node.AsArray;
			for (int i = 0; i < jd.Count; i++) {
								
				list.Add (json2message(jd[i]));
			}

			return list;
		}

		public static EMMessage json2message(JSONNode jd)
		{
			EMMessage message = new EMMessage ();
			message.mMsgId = jd ["mMsgId"].Value;
			message.mFrom = jd ["mFrom"].Value;
			message.mTo = jd ["mTo"].Value;
			message.mIsUnread = (jd ["mIsUnread"].Value).Equals("true");
			message.mIsListened = (jd ["mIsListened"].Value).Equals("true");
			message.mIsAcked = (jd ["mIsAcked"].Value).Equals ("true");
			message.mIsDelivered = (jd ["mIsDelivered"].Value).Equals("true");
			message.mLocalTime = (long)jd ["mLocalTime"].AsInt;
			message.mServerTime = (long)jd ["mServerTime"].AsInt;
			message.mDirection = jd ["mDirection"].AsInt;
			message.mStatus = jd ["mStatus"].AsInt;
			message.mChatType = jd ["mChatType"].AsInt;
			int mType = jd ["mType"].AsInt;
			MessageType type = (MessageType)mType;
			message.mType = type;
			message.ext = jd ["mExtJsonStr"].Value;

			if (type == MessageType.VIDEO || type == MessageType.FILE || type == MessageType.IMAGE || type == MessageType.VOICE) {
				message.mDisplayName = jd ["mDisplayName"].Value;
				message.mSecretKey = jd ["mSecretKey"].Value;
				message.mLocalPath = jd ["mLocalPath"].Value;
				message.mRemotePath = jd ["mRemotePath"].Value;
			} 
			if (type == MessageType.TXT) { 
				message.mTxt = jd ["mTxt"];
			} else if (type == MessageType.IMAGE) {
				message.mThumbnailLocalPath = jd ["mThumbnailLocalPath"].Value;
				message.mThumbnailRemotePath = jd ["mThumbnailRemotePath"].Value;
				message.mThumbnailSecretKey = jd ["mThumbnailSecretKey"].Value;
				message.mWidth = jd ["mWidth"].AsInt;
				message.mHeight = jd ["mHeight"].AsInt;
			} else if (type == MessageType.VOICE) {
				message.mDuration = jd ["mDuration"].AsInt;
			}
			return message;
		}

		public static List<EMGroup> json2grouplist(string jsondata)
		{
			List<EMGroup> list = new List<EMGroup> ();
			JSONArray jd = JSON.Parse(jsondata).AsArray;
				for (int i = 0; i < jd.Count; i++) {
					list.Add (json2group (jd [i]));
				}

			return list;
		}

		public static EMGroup json2group(string jsondata)
		{
			if (jsondata == null || jsondata.Length <= 3)
				return null;
			JSONNode jd = JSON.Parse (jsondata);
			return json2group (jd);
		}
		public static EMGroup json2group(JSONNode jd)
		{
			EMGroup group = new EMGroup ();
			group.mGroupId = jd ["mGroupId"].Value;
			group.mGroupName = jd ["mGroupName"].Value;
			group.mDescription = jd ["mDescription"].Value;
			group.mOwner = jd ["mOwner"].Value;
			group.mIsPublic = (jd ["mIsPublic"].Value).Equals("true");
			group.mIsMsgBlocked = (jd ["mIsMsgBlocked"].Value).Equals("true");
			group.mMembers = jd ["mMembers"].Value;
			group.mIsAllowInvites = (jd ["mIsAllowInvites"].Value).Equals ("true");
			group.mIsNeedApproval = (jd ["mIsNeedApproval"].Value).Equals ("true");

			return group;
		}

		public static List<EMConversation> json2conversationlist(string jsondata)
		{
			List<EMConversation> list = new List<EMConversation> ();
			JSONNode jd = JSON.Parse (jsondata);
				for (int i = 0; i < jd.Count; i++) {
					list.Add (json2conversation (jd [i]));
				}

			return list;
		}

		public static EMConversation json2conversation(string jsondata)
		{
			if(jsondata == null || jsondata.Length <= 3)
				return null;
			JSONNode jd = JSON.Parse (jsondata);
			return json2conversation (jd);
		}

		public static EMConversation json2conversation(JSONNode jd)
		{
			EMConversation conversation = new EMConversation ();
			conversation.mConversationId = jd ["mConversationId"].Value;
			int type = jd ["mConversationType"].AsInt;
			ConversationType ctype = (ConversationType)type;
			conversation.mConversationType = ctype;
			conversation.mUnreadMsgCount = jd ["mUnreadMsgCount"].AsInt;
			conversation.mExtInfo = jd ["mExt"].Value;
			string data = jd ["mLatesMsg"].Value;
			conversation.mLastMsg = json2message (data);
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
