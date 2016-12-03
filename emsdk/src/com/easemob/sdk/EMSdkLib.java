package com.easemob.sdk;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.Properties;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.Application;
import android.util.Log;

import com.hyphenate.EMCallBack;
import com.hyphenate.EMConnectionListener;
import com.hyphenate.EMGroupChangeListener;
import com.hyphenate.EMMessageListener;
import com.hyphenate.EMValueCallBack;
import com.hyphenate.chat.EMClient;
import com.hyphenate.chat.EMConversation;
import com.hyphenate.chat.EMGroup;
import com.hyphenate.chat.EMConversation.EMConversationType;
import com.hyphenate.chat.EMGroupManager.EMGroupOptions;
import com.hyphenate.chat.EMGroupManager.EMGroupStyle;
import com.hyphenate.chat.EMMessage;
import com.hyphenate.chat.EMMessage.ChatType;
import com.hyphenate.chat.EMOptions;
import com.hyphenate.exceptions.HyphenateException;
import com.unity3d.player.UnityPlayer;

public class EMSdkLib {
	private static EMSdkLib instance = null;
	public static Application mApplication;
	private static final String TAG = "EMSdkLib";

	private final String UnityObjectName = "emsdk_cb_object";
		
	public static EMSdkLib getInstance()
	{
		if(instance == null)
			instance = new EMSdkLib();
		return instance;
	}
	
	private EMSdkLib()
	{
		
	}
	
	public void onApplicationCreate(Application application)
	{
		mApplication = application;
		
		//TODO options未补全
		EMOptions options = new EMOptions();
		Properties properties = PropertiesUtil.getProperties(mApplication, "emconfig.properties");
		if(PropertiesUtil.keyExist(properties, "acceptInvitationAlways"))
			options.setAcceptInvitationAlways(PropertiesUtil.getBoolean(properties, "acceptInvitationAlways"));
		
		if(PropertiesUtil.keyExist(properties, "autoLogin"))
			options.setAutoLogin(PropertiesUtil.getBoolean(properties, "autoLogin"));
		
		EMClient.getInstance().init(mApplication, options);
		
		EMClient.getInstance().setDebugMode(PropertiesUtil.getBoolean(properties, "debugModel"));
		
		addListeners();
	}
	
	public void addListeners()
	{
		EMClient.getInstance().addConnectionListener(connectionListener);
		EMClient.getInstance().chatManager().addMessageListener(messageListener);
		EMClient.getInstance().groupManager().addGroupChangeListener(groupChangeListener);
	}
	
	public void removeListeners()
	{
		EMClient.getInstance().removeConnectionListener(connectionListener);
		EMClient.getInstance().chatManager().removeMessageListener(messageListener);
		EMClient.getInstance().groupManager().removeGroupChangeListener(groupChangeListener);
	}
	
   //【API】开放注册
  	public int createAccount(String username,String pwd)
  	{
  		try {
  			EMClient.getInstance().createAccount(username, pwd);
  		} catch (HyphenateException e) {
  			return e.getErrorCode();
  		} catch(Exception e){
  			return -1;	//unknown error
  		}
  		return 0;	//no error
  	}
	  	
  	//【API】
  	public void login(String username,String pwd)
  	{
  		final String cbName = "LoginCallback";
  		EMClient.getInstance().login(username, pwd, new EMCallBack() {
			
			@Override
			public void onSuccess() {
		        sendSuccCallback(cbName);
				EMClient.getInstance().groupManager().loadAllGroups();
		        EMClient.getInstance().chatManager().loadAllConversations();
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(cbName, progress, status);
			}
			
			@Override
			public void onError(final int code, final String message) {
				sendErrorCallbac(cbName, code, message);
			}
		});
  	}
  	
  	
  	//【API】
  	public void logout(boolean flag)
  	{
  		final String cbName = "LogoutCallback";
  		EMClient.getInstance().logout(flag,new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(cbName);				
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(cbName, code, message);
			}
		});
  	}
  	
  	private void setMessageType(EMMessage message, int chatType)
  	{
  		if(0 == chatType)
  			message.setChatType(ChatType.Chat);
  		if(1 == chatType)
  			message.setChatType(ChatType.GroupChat);
  		
  	}
  	
  	private void setMessageStatusCallback(EMMessage message ,final int callbackId)
  	{
  		final String cbName = "SendMessageCallback";
  		message.setMessageStatusCallback(new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);	
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);				
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
  	}
  	
  	//【API】
  	public void sendTextMessage(final String content,final String toChatUsername, final int callbackId,final int chatType)
  	{
  		EMMessage message = EMMessage.createTxtSendMessage(content, toChatUsername);
  		setMessageType(message, chatType);
  		setMessageStatusCallback(message, callbackId);
  		EMClient.getInstance().chatManager().sendMessage(message);
  	}
  	
  	//【API】
  	public void sendVoiceMessage(final String filePath, final int length, final String toChatUsername, final int callbackId,final int chatType)
  	{
  		EMMessage message = EMMessage.createVoiceSendMessage(filePath, length, toChatUsername);
  		setMessageType(message, chatType);
  		setMessageStatusCallback(message, callbackId);
  		EMClient.getInstance().chatManager().sendMessage(message);
  	}
  	
  	//【API】
  	public void sendPictureMessage(final String imagePath,final boolean isSrcImage,final String toChatUsername, final int callbackId,final int chatType)
  	{
  		EMMessage message = EMMessage.createImageSendMessage(imagePath, isSrcImage, toChatUsername);
  		setMessageType(message, chatType);
  		setMessageStatusCallback(message, callbackId);
  		EMClient.getInstance().chatManager().sendMessage(message);
  	}
  	
  	//【API】
  	public void sendVideoMessage(final String videoPath, final String thumbPath, final int videoLength,final String toChatUsername, final int callbackId,final int chatType)
  	{
  		EMMessage message = EMMessage.createVideoSendMessage(videoPath, thumbPath, videoLength, toChatUsername);
  		setMessageType(message, chatType);
  		setMessageStatusCallback(message, callbackId);
  		EMClient.getInstance().chatManager().sendMessage(message);
  	}
  	//【API】
  	public void sendLocationMessage(final double latitude,final double longitude,final String locationAddress,final String toChatUsername, final int callbackId,final int chatType)
  	{
  		EMMessage message = EMMessage.createLocationSendMessage(latitude, longitude, locationAddress, toChatUsername);
  		setMessageType(message, chatType);
  		setMessageStatusCallback(message, callbackId);
  		EMClient.getInstance().chatManager().sendMessage(message);
  	}
  	//【API】
  	public void sendFileMessage(final String filePath,final String toChatUsername, final int callbackId,final int chatType)
  	{
  		EMMessage message = EMMessage.createFileSendMessage(filePath, toChatUsername);
  		setMessageType(message, chatType);
  		setMessageStatusCallback(message, callbackId);
  		EMClient.getInstance().chatManager().sendMessage(message);
  	}
  	
  	//【API】TODO 透传消息
  	public void sendExtraMessage()
  	{
  		
  	}
  	
  	//【API】TODO expand
  	public void sendExpandMessage()
  	{
  		
  	}

	EMConnectionListener connectionListener = new EMConnectionListener() {
		@Override
		public void onConnected() {
			sendCallback("ConnectedCallback", "" );
		}

		@Override
		public void onDisconnected(int code) {
			sendCallback("DisconnectedCallback", "" + code );
		}
	};

  	EMMessageListener messageListener = new EMMessageListener() {
  		
  		//收到消息
		@Override
		public void onMessageReceived(List<EMMessage> messageList) {
			JSONArray jsonArray = new JSONArray();
			for (EMMessage message : messageList) {
				jsonArray.put(EMTools.message2json(message));
			}
			sendCallback("MessageReceivedCallback", jsonArray.toString());
		}
		
		//收到已读回执
		@Override
		public void onMessageReadAckReceived(List<EMMessage> messageList) {
			JSONArray jsonArray = new JSONArray();
			for (EMMessage message : messageList) {
				jsonArray.put(EMTools.message2json(message));
			}
			sendCallback("MessageReadAckReceivedCallback", jsonArray.toString());
		}
		
		//收到已送达回执
		@Override
		public void onMessageDeliveryAckReceived(List<EMMessage> messageList) {
			JSONArray jsonArray = new JSONArray();
			for (EMMessage message : messageList) {
				jsonArray.put(EMTools.message2json(message));
			}
			sendCallback("MessageDeliveryAckReceivedCallback", jsonArray.toString());
		}
		
		//消息状态变动
		@Override
		public void onMessageChanged(EMMessage arg0, Object arg1) {
			// TODO Auto-generated method stub
			
		}
		
		//收到透传消息
		@Override
		public void onCmdMessageReceived(List<EMMessage> messageList) {
			JSONArray jsonArray = new JSONArray();
			for (EMMessage message : messageList) {
				jsonArray.put(EMTools.message2json(message));
			}
			sendCallback("CmdMessageReceivedCallback", jsonArray.toString());
		}
	};
	
	//【API】获取联系人列表
	public String getAllContactsFromServer()
	{
		try {
			List<String> usernames = EMClient.getInstance().contactManager().getAllContactsFromServer();
			return EMTools.listString2String(usernames);
		} catch (HyphenateException e) {
			e.printStackTrace();
			return "";
		}
	}
	
	public String getConversation(String cid, int type, boolean createIfNotExists)
	{
		EMConversation conversation = EMClient.getInstance().chatManager().getConversation(cid, EMConversationType.values()[type], createIfNotExists);
		if(conversation != null)
			return EMTools.conversation2json(conversation).toString();
		return "";
	}
	
	//【API】获取指定会话聊天记录
	public String getAllConversationMessage(String username)
	{
		EMConversation conversation = EMClient.getInstance().chatManager().getConversation(username);
		List<EMMessage> messages = conversation.getAllMessages();
		JSONArray jsonArray = new JSONArray();
		for (EMMessage message : messages) {
			jsonArray.put(EMTools.message2json(message));
		}
		return jsonArray.toString();
	}
	
	//【API】获取指定会话startMsgId之前的pageSize条消息
	public String getConversationMessage(String username,String startMsgId, int pageSize)
	{
		EMConversation conversation = EMClient.getInstance().chatManager().getConversation(username);
		List<EMMessage> messages = conversation.loadMoreMsgFromDB(startMsgId, pageSize);
		JSONArray jsonArray = new JSONArray();
		for (EMMessage message : messages) {
			jsonArray.put(EMTools.message2json(message));
		}
		return jsonArray.toString();
	}
	
	//【API】
	public String getLatestMessage(String username)
	{
		EMConversation conversation = EMClient.getInstance().chatManager().getConversation(username);
		if(conversation != null && conversation.getLastMessage() != null){
			return EMTools.message2json(conversation.getLastMessage()).toString();
		}
		return "";
	}
	
	//【API】获取指定会话未读消息数量
	public int getUnreadMsgCount(String username)
	{
		EMConversation conversation = EMClient.getInstance().chatManager().getConversation(username);
		return conversation.getUnreadMsgCount();
	}
	
	//【API】指定会话未读消息数清零
	public void markAllMessagesAsRead(String username)
	{
		EMConversation conversation = EMClient.getInstance().chatManager().getConversation(username);
		conversation.markAllMessagesAsRead();
	}
	//【API】指定会话特定消息置为已读
	public void markMessageAsRead(String username,String messageId)
	{
		EMConversation conversation = EMClient.getInstance().chatManager().getConversation(username);
		conversation.markMessageAsRead(messageId);
	}
	
	//【API】所有未读消息数清零
	public void markAllConversationsAsRead()
	{
		EMClient.getInstance().chatManager().markAllConversationsAsRead();
	}
	
	//【API】获取此会话在本地的所有的消息数量
	public int getAllMsgCount(String username)
	{
		EMConversation conversation = EMClient.getInstance().chatManager().getConversation(username);
		return conversation.getAllMsgCount();
	}
	
	//【API】获取当前在内存的消息数量
	public int getAllMessagesSize(String username)
	{
		EMConversation conversation = EMClient.getInstance().chatManager().getConversation(username);
		return conversation.getAllMessages().size();
	}
	
	//【API】获取所有会话
	public String getAllConversations()
	{
		JSONArray array = new JSONArray();
		Map<String, EMConversation> conversations = EMClient.getInstance().chatManager().getAllConversations();
		for(Map.Entry<String, EMConversation> entry:conversations.entrySet()){    
		     array.put(EMTools.conversation2json(entry.getValue()));
		}   
		return array.toString();
	}
	
	//【API】删除和某个user会话，如果需要保留聊天记录，传false
	public boolean deleteConversation(String username, boolean isDeleteHistory)
	{
		return EMClient.getInstance().chatManager().deleteConversation(username, isDeleteHistory);
	}
	
	//【API】删除当前会话的某条聊天记录
	public void removeMessage(String username, String msgId)
	{
		EMConversation conversation = EMClient.getInstance().chatManager().getConversation(username);
		conversation.removeMessage(msgId);
	}
	
	//【API】导入消息到数据库 
	public void importMessages(String json)
	{
		List<EMMessage> list = new ArrayList<EMMessage>();
		//TODO json转化成EMMessage列表添加到list中
		
		EMClient.getInstance().chatManager().importMessages(list);
	}
	
	//【API】
	public void createGroup(final int callbackId, final String groupName, final String desc,final String strMembers,final String reason, final int maxUsers,final int style)
	{
		Log.d(TAG, "createGroup style="+style);
		EMGroupOptions options = new EMGroupOptions();
		options.maxUsers = maxUsers;
		switch (style) {
		case 0:
			options.style = EMGroupStyle.EMGroupStylePrivateOnlyOwnerInvite;
			break;
		case 1:
			options.style = EMGroupStyle.EMGroupStylePrivateMemberCanInvite;
			break;
		case 2:
			options.style = EMGroupStyle.EMGroupStylePublicJoinNeedApproval;
			break;
		case 3:
			options.style = EMGroupStyle.EMGroupStylePublicOpenJoin;
			break;
		default:
			break;
		}
		String[] allMembers = strMembers.split(",");
		
		final String cbName = "CreateGroupCallback";
		EMClient.getInstance().groupManager().asyncCreateGroup(groupName, desc, allMembers, reason, options,new EMValueCallBack<EMGroup>() {
			
			@Override
			public void onSuccess(EMGroup group) {
				sendSuccCallback(callbackId, cbName, EMTools.group2json(group).toString());
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(cbName, code, message);
			}
		});
	}
	
	public void addUsersToGroup (final int callbackId,String groupId, String strMembers) 
	{
		String[] members = strMembers.split(",");
		final String cbName = "AddUsersToGroupCallback";
		EMClient.getInstance().groupManager().asyncAddUsersToGroup(groupId, members, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void inviteUser(final int callbackId, String groupId, String beInvitedUsernames, String reason)
	{
		String[] members = beInvitedUsernames.split(",");
		final String cbName = "InviteUserCallback";
		EMClient.getInstance().groupManager().asyncInviteUser(groupId, members, reason, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void removeUserFromGroup(final int callbackId, String groupId,String username)
	{
		final String cbName = "RemoveUserFromGroupCallback";

		EMClient.getInstance().groupManager().asyncRemoveUserFromGroup(groupId, username, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void joinGroup(final int callbackId, String groupId)
	{
		final String cbName = "JoinGroupCallback";

		EMClient.getInstance().groupManager().asyncJoinGroup(groupId, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId,cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void applyJoinToGroup(final int callbackId,String groupId,String reason)
	{
		final String cbName = "ApplyJoinToGroupCallback";

		EMClient.getInstance().groupManager().asyncApplyJoinToGroup(groupId, reason, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void leaveGroup(final int callbackId,String groupId)
	{
		final String cbName = "LeaveGroupCallback";

		EMClient.getInstance().groupManager().asyncLeaveGroup(groupId,new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId,cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void destroyGroup(final int callbackId, String groupId)
	{
		final String cbName = "DestroyGroupCallback";

		EMClient.getInstance().groupManager().asyncDestroyGroup(groupId, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void getJoinedGroupsFromServer(final int callbackId) 
	{
		final String cbName = "GetJoinedGroupsFromServerCallback";

		EMClient.getInstance().groupManager().asyncGetJoinedGroupsFromServer(new EMValueCallBack<List<EMGroup>>() {

			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}

			@Override
			public void onSuccess(List<EMGroup> groups) {
				JSONArray array = new JSONArray();
				for (EMGroup group : groups) {
					array.put(EMTools.group2json(group));
				}	
				sendSuccCallback(callbackId, cbName, array.toString());
			}
		});
	}
	
	public String getAllGroups()
	{
		List<EMGroup> groups = EMClient.getInstance().groupManager().getAllGroups();
		JSONArray array = new JSONArray();
		for (EMGroup group : groups) {
			array.put(EMTools.group2json(group));
		}
		return array.toString();
	}
	
	public void changeGroupName(final int callbackId, String groupId,String groupName)
	{
		final String cbName = "ChangeGroupNameCallback";

		EMClient.getInstance().groupManager().asyncChangeGroupName(groupId, groupName, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId,cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public String getGroup(String groupId)
	{
		EMGroup group = EMClient.getInstance().groupManager().getGroup(groupId);
		if(group == null)
			try {
				group = EMClient.getInstance().groupManager().getGroupFromServer(groupId);
			} catch (HyphenateException e) {
				e.printStackTrace();
			}
		return EMTools.group2json(group).toString();
	}
	
	public void blockGroupMessage(final int callbackId, String groupId)
	{
		final String cbName = "BlockGroupMessageCallback";

		EMClient.getInstance().groupManager().asyncBlockGroupMessage(groupId, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void unblockGroupMessage(final int callbackId, String groupId)
	{
		final String cbName = "UnblockGroupMessageCallback";

		EMClient.getInstance().groupManager().asyncUnblockGroupMessage(groupId, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void blockUser(final int callbackId, String groupId,String username)
	{
		final String cbName = "BlockUserCallback";

		EMClient.getInstance().groupManager().asyncBlockUser(groupId, username, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void unblockUser(final int callbackId, String groupId,String username)
	{
		final String cbName = "UnblockUserCallback";

		EMClient.getInstance().groupManager().asyncUnblockUser(groupId, username, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void getBlockedUsers(final int callbackId, String groupId)
	{
		final String cbName = "GetBlockedUsersCallback";

		EMClient.getInstance().groupManager().asyncGetBlockedUsers(groupId, new EMValueCallBack<List<String>>() {

			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}

			@Override
			public void onSuccess(List<String> users) {		
				sendSuccCallback(callbackId, cbName, EMTools.listString2String(users));
			}
		});
	}
	
	EMGroupChangeListener groupChangeListener = new EMGroupChangeListener() {
		
		@Override
		public void onUserRemoved(String groupId, String groupName) {
			try {
				JSONObject json = new JSONObject();
				json.put("groupId", groupId);
				json.put("groupName", groupName);
				sendCallback("UserRemovedCallback", json.toString());
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		
		@Override
		public void onInvitationReceived(String groupId, String groupName, String inviter,
				String reason) {
			try {
				JSONObject json = new JSONObject();
				json.put("groupId", groupId);
				json.put("groupName", groupName);
				json.put("inviter", inviter);
				json.put("reason", reason);
				sendCallback("InvitationReceivedCallback", json.toString());
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		
		@Override
		public void onInvitationDeclined(String groupId, String invitee, String reason) {
			try {
				JSONObject json = new JSONObject();
				json.put("groupId", groupId);
				json.put("invitee", invitee);
				json.put("reason", reason);
				sendCallback("InvitationDeclinedCallback", json.toString());
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		
		@Override
		public void onInvitationAccepted(String groupId, String inviter, String reason) {
			try {
				JSONObject json = new JSONObject();
				json.put("groupId", groupId);
				json.put("inviter", inviter);
				json.put("reason", reason);
				sendCallback("InvitationAcceptedCallback", json.toString());
			} catch (JSONException e) {
				e.printStackTrace();
			}			
		}
		
		@Override
		public void onGroupDestroyed(String groupId, String groupName) {
			try {
				JSONObject json = new JSONObject();
				json.put("groupId", groupId);
				json.put("groupName", groupName);
				sendCallback("GroupDestroyedCallback", json.toString());
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		
		@Override
		public void onAutoAcceptInvitationFromGroup(String groupId, String inviter,
				String inviteMessage) {
			try {
				JSONObject json = new JSONObject();
				json.put("groupId", groupId);
				json.put("inviter", inviter);
				json.put("inviteMessage", inviteMessage);
				sendCallback("AutoAcceptInvitationFromGroupCallback", json.toString());
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		
		@Override
		public void onApplicationReceived(String groupId, String groupName, String applicant,
				String reason) {
			try {
				JSONObject json = new JSONObject();
				json.put("groupId", groupId);
				json.put("groupName", groupName);
				json.put("applicant", applicant);
				json.put("reason", reason);
				sendCallback("ApplicationReceivedCallback", json.toString());
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		
		@Override
		public void onApplicationDeclined(String groupId, String groupName, String decliner,
				String reason) {
			try {
				JSONObject json = new JSONObject();
				json.put("groupId", groupId);
				json.put("groupName", groupName);
				json.put("decliner", decliner);
				json.put("reason", reason);
				sendCallback("ApplicationDeclinedCallback", json.toString());
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
		
		@Override
		public void onApplicationAccept(String groupId, String groupName, String accepter) {
			try {
				JSONObject json = new JSONObject();
				json.put("groupId", groupId);
				json.put("groupName", groupName);
				json.put("accepter", accepter);
				sendCallback("ApplicationAcceptCallback", json.toString());
			} catch (JSONException e) {
				e.printStackTrace();
			}
		}
	};
	
	public void downloadAttachment(final int callbackId,String username,String msgId)
	{
		final String cbName = "DownloadAttachmentCallback";
		EMMessage message = EMClient.getInstance().chatManager().getMessage(msgId);
		message.setMessageStatusCallback(new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(cbName, code, message);
			}
		});
		EMClient.getInstance().chatManager().downloadAttachment(message);
	}
	
	public void approveJoinGroupRequest(final int callbackId,String groupId,String username)
	{
		final String cbName = "ApproveJoinGroupRequestCallback";
		EMClient.getInstance().groupManager().asyncAcceptApplication(username, groupId, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void declineJoinGroupRequest(final int callbackId,String groupId,String username,String reason)
	{
		final String cbName = "DeclineJoinGroupRequestCallback";
		EMClient.getInstance().groupManager().asyncDeclineApplication(username, groupId, reason,new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(cbName, code, message);
			}
		});
	}
	
	public void acceptInvitationFromGroup(final int callbackId,String groupId,String username)
	{
		final String cbName = "AcceptInvitationFromGroupCallback";
		EMClient.getInstance().groupManager().asyncAcceptInvitation(groupId, username, new EMValueCallBack<EMGroup>() {

			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(cbName, code, message);
			}

			@Override
			public void onSuccess(EMGroup group) {
				sendSuccCallback(callbackId, cbName, EMTools.group2json(group).toString());
			}
		});
	}
	
	public void declineInvitationFromGroup(final int callbackId,String groupId,String username,String reason)
	{
		final String cbName = "DeclineInvitationFromGroupCallback";
		EMClient.getInstance().groupManager().asyncDeclineInvitation(groupId, username,reason, new EMCallBack() {
			
			@Override
			public void onSuccess() {
				sendSuccCallback(callbackId, cbName);
			}
			
			@Override
			public void onProgress(int progress, String status) {
				sendInProgressCallback(callbackId, cbName, progress, status);
			}
			
			@Override
			public void onError(int code, String message) {
				sendErrorCallbac(callbackId, cbName, code, message);
			}
		});
	}
	
	public void deleteMessagesAsExitGroup(boolean del)
	{
		EMOptions options = EMClient.getInstance().getOptions();
		options.setDeleteMessagesAsExitGroup(del);
	}
	
	public void isAutoAcceptGroupInvitation(boolean isAuto)
	{
		EMOptions options = EMClient.getInstance().getOptions();
		options.setAutoAcceptGroupInvitation(isAuto);
	}
	
	public void isSortMessageByServerTime(boolean isSort)
	{
		EMOptions options = EMClient.getInstance().getOptions();
		options.setSortMessageByServerTime(isSort);
	}
	
	public void requireDeliveryAck(boolean isReq)
	{
		EMOptions options = EMClient.getInstance().getOptions();
		options.setRequireDeliveryAck(isReq);
	}
	
	public void sendCallback(String objName, String cbName, final String jsonParams)
	{
		Log.d(TAG,"return objName="+ objName+",cbName="+cbName+",data="+jsonParams);
		UnityPlayer.UnitySendMessage(objName, cbName, jsonParams);
	}
	public void sendCallback(String cbName, String jsonParams)
	{
		if (jsonParams == null) {
			jsonParams = "";
	    }
	    sendCallback(UnityObjectName, cbName, jsonParams);
	}
	
	private void sendSuccCallback(final int callbackId,final String cbName,final String data)
	{
		try {
			JSONObject jo = new JSONObject();
			jo.put("on", "success");
			jo.put("callbackid", callbackId);
			jo.put("data", data);
			sendCallback(cbName, jo.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}
	
	private void sendSuccCallback(final String cbName)
	{
		try {
			JSONObject jo = new JSONObject();
			jo.put("on", "success");
			sendCallback(cbName, jo.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}
	
	private void sendSuccCallback(final int callbackId,final String cbName)
	{
		try {
			JSONObject jo = new JSONObject();
			jo.put("on", "success");
			jo.put("callbackid", callbackId);
			sendCallback(cbName, jo.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}
	
	private void sendInProgressCallback(final int callbackId,final String cbName,final int progress,final String status)
	{
		try {
			JSONObject jo = new JSONObject();
			jo.put("on", "progress");
			jo.put("callbackid", callbackId);
			jo.put("progress", progress);
			jo.put("status", status);
			sendCallback(cbName, jo.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}
	
	private void sendInProgressCallback(final String cbName,final int progress,final String status)
	{
		try {
			JSONObject jo = new JSONObject();
			jo.put("on", "progress");
			jo.put("progress", progress);
			jo.put("status", status);
			sendCallback(cbName, jo.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}
	
	private void sendErrorCallbac(final int callbackId,final String cbName,final int code,final String message)
	{
		try {
			JSONObject jo = new JSONObject();
			jo.put("on", "error");
			jo.put("callbackid", callbackId);
			jo.put("code", code);
			jo.put("message", message);
			sendCallback(cbName, jo.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}
	
	private void sendErrorCallbac(final String cbName,final int code,final String message)
	{
		try {
			JSONObject jo = new JSONObject();
			jo.put("on", "error");
			jo.put("code", code);
			jo.put("message", message);
			sendCallback(cbName, jo.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}

}
