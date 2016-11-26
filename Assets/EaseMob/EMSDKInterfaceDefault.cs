using UnityEngine;

namespace EaseMob{

	public class EMSDKInterfaceDefault : EMSDKInterfaceBase {

		public override int createAccount (string username, string password)
		{
			Debug.Log ("createAccount:" + username + "," + password);
			return 0;
		}

		public override void login (string username, string password)
		{
			Debug.Log ("login:" + username + "," + password);
			string ret = "{\"on\":\"success\"}";
	//		string ret = "{\"on\":\"error\",\"code\":100,\"message\":\"err message\"}";

			EMSDKCallback.Instance.LoginCallback (ret);
		}

		public override void logout (bool flag)
		{
			string ret = "{\"on\":\"success\"}";
			EMSDKCallback.Instance.LogoutCallback (ret);
		}
		public override void sendTextMessage (string content, string to, int callbackId,int chattype)
		{
			
		}
		public override void sendVoiceMessage (string path, int length, string to, int callbackId,int chattype)
		{
			
		}
		public override void sendPictureMessage (string path, bool isSrcImage, string to, int callbackId,int chattype)
		{
			
		}
		public override void sendVideoMessage (string path, string thumbPath,int length, string to, int callbackId,int chattype)
		{
			
		}
		public override void sendLocationMessage (double latitude, double longitude, string locationAddress, string to, int callbackId,int chattype)
		{
			
		}
		public override void sendFileMessage (string path, string to, int callbackId,int chattype)
		{
			
		}
		public override string getAllContactsFromServer()
		{
			return "user2,user3,user4";
		}

		public override void startRecord(){}
		public override void stopRecord(){}

		public override string getAllConversationMessage(string username)
		{
			return null;
		}

		public override string getConversationMessage(string username, string startMsgId, int pageSize)
		{
			return null;
		}
		public override string getLatestMessage (string username)
		{
			return "";
		}
		public override int getUnreadMsgCount (string username)
		{
			return 0;
		}
		public override void markAllMessagesAsRead (string username)
		{

		}
		public override void markMessageAsRead (string username, string messageId)
		{

		}
		public override void markAllConversationsAsRead ()
		{

		}
		public override int getAllMsgCount (string username)
		{
			return 0;
		}
		public override int getAllMessagesSize (string username)
		{
			return 0;
		}

		public override string getAllConversations ()
		{
			return null;
		}
		public override bool deleteConversation (string username, bool isDeleteHistory)
		{
			return true;
		}
		public override void removeMessage (string username, string msgId)
		{

		}
		public override void importMessages (string json)
		{

		}

		public override void createGroup (int callbackId,string groupName, string desc, string strMembers, string reason, int maxUsers, int style)
		{
			string json = "{\"on\":\"success\",\"callbackid\":1,\"data\":\"{\\\"mGroupId\\\":\\\"1479014387570\\\",\\\"mGroupName\\\":\\\"eee\\\",\\\"mIsPublic\\\":true,\\\"mIsAllowInvites\\\":false,\\\"mIsMsgBlocked\\\":false,\\\"mOwner\\\":\\\"user1\\\",\\\"mMembers\\\":\\\"[user1, ]\\\",\\\"mDescription \\\":\\\"desc:eee\\\"}\"}";
			json = "{\"callbackid\":1,\"data\":\"{\\\"mIsPublic\\\":\\\"true\\\",\\\"mOwner\\\":\\\"user1\\\",\\\"mGroupName\\\":\\\"21\\\",\\\"mMemCount\\\":1,\\\"mMembers\\\":\\\"\\\",\\\"mIsMsgBlocked\\\":\\\"false\\\",\\\"mDescription\\\":\\\"desc:21\\\",\\\"mGroupId\\\":\\\"1479557332062\\\"}\",\"on\":\"success\"}";
			EMSDKCallback.Instance.CreateGroupCallback (json);
		}
		public override void addUsersToGroup (int callbackId,string groupId, string strMembers)
		{
			
		}
		public override void inviteUser (int callbackId,string groupId, string beInvitedUsernames, string reason)
		{
			
		}
		public override void removeUserFromGroup (int callbackId,string groupId, string username)
		{
			
		}
		public override void joinGroup (int callbackId,string groupId)
		{
			
		}
		public override void applyJoinToGroup (int callbackId,string groupId, string reason)
		{
			
		}
		public override void leaveGroup (int callbackId,string groupId)
		{
			
		}
		public override void destroyGroup (int callbackId,string groupId)
		{
			
		}
		public override void getJoinedGroupsFromServer (int callbackId)
		{
			string json = "{\"callbackid\":"+callbackId+",\"data\":\"[{\\\"mIsPublic\\\":\\\"true\\\",\\\"mOwner\\\":\\\"user1\\\",\\\"mGroupName\\\":\\\"Qq\\\",\\\"mMemCount\\\":0,\\\"mMembers\\\":\\\"\\\",\\\"mIsMsgBlocked\\\":\\\"false\\\",\\\"mDescription\\\":\\\"desc:Qq\\\",\\\"mGroupId\\\":\\\"1479573183219\\\"},{\\\"mIsPublic\\\":\\\"true\\\",\\\"mOwner\\\":\\\"user1\\\",\\\"mGroupName\\\":\\\"Ww\\\",\\\"mMemCount\\\":0,\\\"mMembers\\\":\\\"\\\",\\\"mIsMsgBlocked\\\":\\\"false\\\",\\\"mDescription\\\":\\\"desc:Ww\\\",\\\"mGroupId\\\":\\\"1479573487295\\\"},{\\\"mIsPublic\\\":\\\"true\\\",\\\"mOwner\\\":\\\"user1\\\",\\\"mGroupName\\\":\\\"A\\\",\\\"mMemCount\\\":0,\\\"mMembers\\\":\\\"\\\",\\\"mIsMsgBlocked\\\":\\\"false\\\",\\\"mDescription\\\":\\\"desc:A\\\",\\\"mGroupId\\\":\\\"1479574181703\\\"},{\\\"mIsPublic\\\":\\\"true\\\",\\\"mOwner\\\":\\\"user1\\\",\\\"mGroupName\\\":\\\"B\\\",\\\"mMemCount\\\":0,\\\"mMembers\\\":\\\"\\\",\\\"mIsMsgBlocked\\\":\\\"false\\\",\\\"mDescription\\\":\\\"desc:B\\\",\\\"mGroupId\\\":\\\"1479574291702\\\"},{\\\"mIsPublic\\\":\\\"true\\\",\\\"mOwner\\\":\\\"user1\\\",\\\"mGroupName\\\":\\\"C\\\",\\\"mMemCount\\\":0,\\\"mMembers\\\":\\\"\\\",\\\"mIsMsgBlocked\\\":\\\"false\\\",\\\"mDescription\\\":\\\"desc:C\\\",\\\"mGroupId\\\":\\\"1479574419203\\\"},{\\\"mIsPublic\\\":\\\"true\\\",\\\"mOwner\\\":\\\"user1\\\",\\\"mGroupName\\\":\\\"D\\\",\\\"mMemCount\\\":0,\\\"mMembers\\\":\\\"\\\",\\\"mIsMsgBlocked\\\":\\\"false\\\",\\\"mDescription\\\":\\\"desc:D\\\",\\\"mGroupId\\\":\\\"1479574867173\\\"},{\\\"mIsPublic\\\":\\\"true\\\",\\\"mOwner\\\":\\\"user1\\\",\\\"mGroupName\\\":\\\"E\\\",\\\"mMemCount\\\":0,\\\"mMembers\\\":\\\"\\\",\\\"mIsMsgBlocked\\\":\\\"false\\\",\\\"mDescription\\\":\\\"desc:E\\\",\\\"mGroupId\\\":\\\"1479575988653\\\"},{\\\"mIsPublic\\\":\\\"true\\\",\\\"mOwner\\\":\\\"user1\\\",\\\"mGroupName\\\":\\\"F\\\",\\\"mMemCount\\\":2,\\\"mMembers\\\":\\\"user8\\\",\\\"mIsMsgBlocked\\\":\\\"false\\\",\\\"mDescription\\\":\\\"desc:F\\\",\\\"mGroupId\\\":\\\"1479576316799\\\"}]\",\"on\":\"success\"}";
			EMSDKCallback.Instance.GetJoinedGroupsFromServerCallback (json);
		}
		public override string getAllGroups (){
			return "";
		}
		public override void changeGroupName (int callbackId,string groupId,string groupName)
		{
			
		}
		public override string getGroup (string groupId)
		{
			return "";
		}
		public override void blockGroupMessage (int callbackId,string groupId)
		{
			
		}
		public override void unblockGroupMessage (int callbackId,string groupId)
		{
			
		}
		public override void blockUser (int callbackId,string groupId, string username)
		{
			
		}
		public override void unblockUser(int callbackId,string groupId,string username)
		{
			
		}
		public override void getBlockedUsers(int callbackId,string groupId)
		{
			
		}

		public override void downloadAttachment(int callbackId,string username,string msgId)
		{

		}


	}

}