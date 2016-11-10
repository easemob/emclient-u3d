public class EMSDKInterfaceIos : EMSDKInterfaceBase {

	public override int createAccount (string username, string password)
	{
		return 0;
	}

	public override void login (string username, string password)
	{

	}

	public override void logout (bool flag)
	{

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
		return "";
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
}
