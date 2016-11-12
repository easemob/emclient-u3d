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
			return list;
		}

		public static List<EMGroup> json2grouplist(string jsondata)
		{
			List<EMGroup> list = new List<EMGroup> ();
			JsonData jd = JsonMapper.ToObject (jsondata);
			for (int i = 0; i < jd.Count; i++) {
				list.Add (json2group(jd[i]));
			}
			return list;
		}

		public static EMGroup json2group(string jsondata)
		{
			JsonData jd = JsonMapper.ToObject (jsondata);
			return json2group (jd);
		}
		public static EMGroup json2group(JsonData jd)
		{
			EMGroup group = new EMGroup ();

			return group;
		}
	}
}
