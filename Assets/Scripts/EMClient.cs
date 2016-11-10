using UnityEngine;
using System.Collections.Generic;

public class EMClient {
	
	private static EMClient _instance;
	private static int callbackId; 
	private static Dictionary<int,EMBaseCallback> requestedCallbackList;
	private static object lockObject = new UnityEngine.Object();


	private EMSDKInterfaceBase sdk;

	public EMBaseCallback loginCallback{ set; get; }
	public EMBaseCallback regCallback{ set; get; }
	public EMBaseCallback logoutCallback{ set; get; }
	public EMRecordCallback recordCallback{ set; get; }
	public EMMessageCallback receiveMessageCallback{ set; get; }
	public EMConnListenerCallback connListenerCallback{ set; get; }

	public static EMClient Instance{
		get{
			if (_instance == null) {
				_instance = new EMClient ();
			}
			return _instance;
		}
	}

	private EMClient()
	{
		sdk = EMSDKInterfaceBase.Instance;
		requestedCallbackList = new Dictionary<int,EMBaseCallback> ();

	}

	public void Init()
	{
		EMSDKCallback.InitCallback ();
	}

	public int CreateAccount(string username,string password)
	{
		return sdk.createAccount (username, password);
	}

	public void Login(string username,string password,EMBaseCallback cb)
	{
		loginCallback = cb;
		sdk.login (username, password);
	}

	public void Logout (bool flag, EMBaseCallback cb)
	{
		logoutCallback = cb;
		sdk.logout (flag);
	}

	public string GetAllContactsFromServer()
	{
		return sdk.getAllContactsFromServer ();
	}

	public void SendTextMessage (string content, string to, int chattype, EMBaseCallback cb)
	{
		AddCallbackToList (cb);
		sdk.sendTextMessage (content, to, cb.CallbackId,chattype);
	}

	public void SendVoiceMessage (string path, int length, string to, int chattype, EMBaseCallback cb)
	{
		AddCallbackToList (cb);
		sdk.sendVoiceMessage (path, length, to, cb.CallbackId,chattype);
	}

	public void SendPictureMessage (string path, bool isSrcImage, string to, int chattype,EMBaseCallback cb)
	{
		AddCallbackToList (cb);
		sdk.sendPictureMessage (path, isSrcImage, to, cb.CallbackId, chattype);
	}

	public void StartRecord(EMRecordCallback cb)
	{
		recordCallback = cb;
		sdk.startRecord ();
	}

	public void StopRecord()
	{
		sdk.stopRecord ();
	}

	public string GetAllConversationMessage(string username)
	{
		return sdk.getAllConversationMessage(username);
	}

	public string GetConversationMessage(string username, string startMsgId, int pageSize)
	{
		return sdk.getConversationMessage(username,startMsgId,pageSize);
	}
	public int GetUnreadMsgCount (string username)
	{
		return sdk.getUnreadMsgCount(username);
	}
	public void MarkAllMessagesAsRead (string username)
	{
		sdk.markAllMessagesAsRead (username);
	}
	public void MarkMessageAsRead (string username, string messageId)
	{
		sdk.markMessageAsRead (username, messageId);
	}
	public void MarkAllConversationsAsRead ()
	{
		sdk.markAllConversationsAsRead ();
	}
	public int GetAllMsgCount (string username)
	{
		return sdk.getAllMsgCount(username);
	}
	public int GetAllMessagesSize (string username)
	{
		return sdk.getAllMessagesSize(username);
	}

	public string GetAllConversations ()
	{
		return sdk.getAllConversations();
	}
	public bool DeleteConversation (string username, bool isDeleteHistory)
	{
		return sdk.deleteConversation(username,isDeleteHistory);
	}
	public void RemoveMessage (string username, string msgId)
	{
		sdk.removeMessage (username, msgId);
	}
	public void ImportMessages (string json)
	{
		sdk.importMessages (json);
	}

	public EMBaseCallback GetCallbackById(int callbackId)
	{
		if (requestedCallbackList.ContainsKey (callbackId)) 
		{
			return (EMBaseCallback)requestedCallbackList [callbackId];
		} 
		else 
		{
			return null;
		}
	}
		
	public void RemoveCallbackById(int callbackId)
	{
		if (requestedCallbackList.ContainsKey (callbackId)) 
		{
			requestedCallbackList.Remove(callbackId);
		}
	}

	private int AddCallbackToList(EMBaseCallback callback)
	{
		lock (lockObject) 
		{
			callbackId++;
			callback.CallbackId = callbackId;
			requestedCallbackList.Add (callback.CallbackId, callback);
			return callback.CallbackId;
		}
	}
}
