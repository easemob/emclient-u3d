package com.easemob.sdk;

import java.util.List;

import org.json.JSONException;
import org.json.JSONObject;

import com.hyphenate.chat.EMConversation;
import com.hyphenate.chat.EMFileMessageBody;
import com.hyphenate.chat.EMGroup;
import com.hyphenate.chat.EMImageMessageBody;
import com.hyphenate.chat.EMMessage;
import com.hyphenate.chat.EMTextMessageBody;
import com.hyphenate.chat.EMVoiceMessageBody;
import com.hyphenate.chat.EMMessage.Type;

public class EMTools {
	
	public static JSONObject message2json(EMMessage message){
		JSONObject json = new JSONObject();
		if(message != null){
			try {
				json.put("mMsgId", message.getMsgId());
				json.put("mFrom", message.getFrom());
				json.put("mTo", message.getTo());
				json.put("mIsUnread", message.isUnread()?"true":"false");
				json.put("mIsListened", message.isListened()?"true":"false");
				json.put("mIsAcked", message.isAcked()?"true":"false");
				json.put("mIsDelivered", message.isDelivered()?"true":"false");
				json.put("mLocalTime", message.localTime());
				json.put("mServerTime", message.getMsgTime());
				json.put("mChatType", message.getChatType().ordinal());
				json.put("mType", message.getType().ordinal());
				json.put("mStatus", message.status().ordinal());
				json.put("mDirection", message.direct().ordinal());
				try {
					String extjson = message.getStringAttribute("extkey");
					if( extjson != null){
						json.put("mExtJsonStr", extjson);
					}
				} catch (Exception e) {
				}
	
				// set file attribute
				if(message.getType() == Type.FILE 
				        || message.getType() == Type.IMAGE 
				        || message.getType() == Type.VOICE
				        || message.getType() == Type.VIDEO)
				{
				    EMFileMessageBody fileBody = (EMFileMessageBody)message.getBody();
				    json.put("mDisplayName", fileBody.getFileName());
				    json.put("mSecretKey", fileBody.getSecret());
				    json.put("mLocalPath", fileBody.getLocalUrl());
				    json.put("mRemotePath", fileBody.getRemoteUrl());
				}
				
				//set different attribute according type
				switch(message.getType()) {
				case TXT:
				{
				    EMTextMessageBody tbody = (EMTextMessageBody)message.getBody();
		            json.put("mTxt", tbody.getMessage());
				}
				break;
				case IMAGE:
				{
				    EMImageMessageBody ibody = (EMImageMessageBody)message.getBody();
				    json.put("mThumbnailLocalPath", ibody.thumbnailLocalPath());
				    json.put("mThumbnailRemotePath", ibody.getThumbnailUrl());
				    json.put("mThumbnailSecretKey", ibody.getThumbnailSecret());
				    json.put("mWidth", ibody.getWidth());
				    json.put("mHeight", ibody.getHeight());
				}
				break;
				case VOICE:
				{
				    EMVoiceMessageBody voiceBody = (EMVoiceMessageBody)message.getBody();
				    json.put("mDuration", voiceBody.getLength());
				}
				break;
	            default:
	                break;
				}
				
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		return json;
	}
	
	public static JSONObject group2json(EMGroup group)
	{
		JSONObject json =  new JSONObject();
		try {
			json.put("mGroupId", group.getGroupId());
			json.put("mGroupName", group.getGroupName());
			json.put("mDescription", group.getDescription());
			json.put("mOwner", group.getOwner());
			json.put("mMemCount", group.getMemberCount());
			json.put("mMembers", listString2String(group.getMembers()));
			json.put("mIsPublic", group.isPublic()?"true":"false");
			json.put("mIsAllowInvites", group.isAllowInvites()?"true":"false");
			json.put("mIsMsgBlocked", group.isMsgBlocked()?"true":"false");
			json.put("mIsNeedApproval", group.isMembersOnly()?"true":"false");

		} catch (JSONException e) {
			e.printStackTrace();
		}
		return json;
	}
	
	public static JSONObject conversation2json(EMConversation conversation)
	{
		JSONObject json = new JSONObject();
		try {
			json.put("mConversationId", conversation.conversationId());
			json.put("mConversationType", conversation.getType().ordinal());
			json.put("mUnreadMsgCount", conversation.getUnreadMsgCount());
			json.put("mExt", conversation.getExtField());
			json.put("mLatesMsg", message2json(conversation.getLastMessage()).toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return json;
	}
	
	public static String listString2String(List<String> list)
	{
		String retStr = "";
		if(list.size() > 0){
			for (String username : list) {
				if(username.length() > 0)
					retStr += (username + ",");
			}
			retStr = retStr.substring(0, retStr.length()-1);
		}
		return retStr;
	}
}
