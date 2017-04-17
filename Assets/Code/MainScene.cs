using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using EaseMob;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour {
	public InputField toUser;
	public InputField txtContent;
	public Button sendTxtMessageBtn;
	public Button sendGroupTxtMsgBtn;
	public RectTransform logContent;
	public Text logText;
	public Button screenShot;
	public InputField fromUser;
	public InputField msgId;
	public Button getMessageBtn;
	public Button getLastMsgBtn;
	public Button logoutBtn;
	public RawImage rawImage;
	public InputField filePath;
	public Button sendFileMessageBtn;
	public Button sendGroupFileMsgBtn;
	public Button getConversationsBtn;
	public InputField groupName;
	public InputField groupUser;
	public Button createGroupBtn;
	public Dropdown groupStyle;
	public Button leaveGroupBtn;
	public Button getGroupsBtn;
	public Button addToGroupBtn;
	public Button joinGroupBtn;
	public Button GroupInfoBtn;
	public Button sendExtMsgBtn;

	private List<EMGroup> groupList = new List<EMGroup>();

	// Use this for initialization
	void Start () {
		
		rawImage.gameObject.SetActive(false);

		EMClient.Instance.isAutoAcceptGroupInvitation (true);

		setMessageRecvListener ();

		sendTxtMessageBtn.onClick.AddListener (delegate() {
			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				logText.text = "send message success";
			};
			cb.onProgressCallback = (progress,status) => {
				
			};
			cb.onErrorCallback = (code,message) => {
				
			};
			EMClient.Instance.SendTextMessage(txtContent.text, toUser.text, ChatType.Chat, cb);
		});

		screenShot.onClick.AddListener (delegate() {
			StartCoroutine(GenCapture());
		});

		sendFileMessageBtn.onClick.AddListener (delegate() {
			if(filePath.text.Length > 0){
				EMBaseCallback cb = new EMBaseCallback();
				cb.onSuccessCallback = () => {
					logText.text = "send file success";
				};
				cb.onProgressCallback = (progress,status) => {

				};
				cb.onErrorCallback = (code,msg) => {

				};
				EMClient.Instance.SendFileMessage(filePath.text,toUser.text,ChatType.Chat,cb);
			}

		});

		sendGroupTxtMsgBtn.onClick.AddListener (delegate() {
			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				logText.text = "send group message success";
			};
			cb.onProgressCallback = (progress,status) => {

			};
			cb.onErrorCallback = (code,message) => {

			};
			if(txtContent.text.Length == 0)
			{
				txtContent.placeholder.GetComponent<Text>().text = "input here first";
				return;
			}
			if(groupName.text.Length == 0)
			{
				groupName.placeholder.GetComponent<Text>().text = "input here first";
				return;
			}
			EMClient.Instance.SendTextMessage(txtContent.text, groupName.text, ChatType.GroupChat, cb);

		});

		sendGroupFileMsgBtn.onClick.AddListener (delegate() {
			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				logText.text = "send group file success";
			};
			cb.onProgressCallback = (progress,status) => {

			};
			cb.onErrorCallback = (code,message) => {

			};

			if(groupName.text.Length == 0)
			{
				groupName.placeholder.GetComponent<Text>().text = "input here first";
				return;
			}
			EMClient.Instance.SendFileMessage(filePath.text,groupName.text,ChatType.GroupChat,cb);
		});

		createGroupBtn.onClick.AddListener (delegate () {
			if (groupName.text.Length > 0) {

				EMGroupCallback cb = new EMGroupCallback ();
				cb.onSuccessCreateGroupCallback = (group) => {
					logText.text = "create group success";
				};
				cb.onErrorCallback = (code, msg) => {
					logText.text = msg;
				};
				EMClient.Instance.createGroup (groupName.text, "desc:" + groupName.text, new string[0], "reason", 200, GroupStyle.GroupStylePublicOpenJoin, cb);
			}
		});

		joinGroupBtn.onClick.AddListener (delegate () {
			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				logText.text = "join group success";
			};
			cb.onProgressCallback = (progress,status) => {

			};
			cb.onErrorCallback = (code,msg) => {
				logText.text = "join group failure msg=" + msg;
			};
			if(groupName.text.Length > 0)
				EMClient.Instance.joinGroup(groupName.text,cb);
			else
				logText.text = "input group id first";
		});

		leaveGroupBtn.onClick.AddListener (delegate () {
			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				logText.text = "leave group success";
			};
			cb.onProgressCallback = (progress,status) => {

			};
			cb.onErrorCallback = (code,msg) => {
				logText.text = msg;
			};
			if(groupName.text.Length > 0)
				EMClient.Instance.leaveGroup(groupName.text,cb);
			else
				logText.text = "input group id first";
		});
				
		getGroupsBtn.onClick.AddListener (delegate() {
			logText.text = "";
			groupList.Clear();
			EMGroupCallback cb = new EMGroupCallback();
			cb.onSuccessGetGroupListCallback = (groups) => {
				foreach(EMGroup group in groups){
					logText.text += "ID="+group.mGroupId + "," + group.mGroupName + "\n";
					groupList.Add(group);
					groupName.text = group.mGroupId;
				}
				logContent.sizeDelta = new Vector2 (0, logText.preferredHeight+5);
			};
			cb.onErrorCallback = (code,msg) => {
				logText.text = msg;
			};
			EMClient.Instance.getJoinedGroupsFromServer(cb);
		});

		getConversationsBtn.onClick.AddListener (delegate() {
			logText.text = "conversation list:\n";
			List<EMConversation> conversations = EMClient.Instance.GetAllConversations();
			foreach(EMConversation conv in conversations){
				logText.text += conv.mConversationId;
				if(conv.mLastMsg != null)
					logText.text += ",lastmsgId=" + conv.mLastMsg.mMsgId;
				logText.text += "\n";
			}
		});

		addToGroupBtn.onClick.AddListener (delegate() {
			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				logText.text = "add user to group success";
			};
			cb.onProgressCallback = (progress,status) => {

			};
			cb.onErrorCallback = (code,msg) => {
				logText.text = "failed to addUsersToGroup: " + msg;
			};
			string[] users = {groupUser.text};
			EMClient.Instance.addUsersToGroup(groupName.text,users,cb);
		});

		GroupInfoBtn.onClick.AddListener(delegate() {
			if(groupName.text.Length > 0){
				EMGroup group = EMClient.Instance.getGroup(groupName.text);
				if(group != null)
					logText.text = "name="+group.mGroupName+",id="+group.mGroupId;
			}
			else
				logText.text = "input group id first";
			
		});

		getLastMsgBtn.onClick.AddListener (delegate() {
			if(fromUser.text.Length == 0)
			{
				fromUser.placeholder.GetComponent<Text>().text = "input here first";
				return;
			}
			EMMessage message = EMClient.Instance.GetLatestMessage(fromUser.text);
			if(message != null){
				msgId.text = message.mMsgId;
				logText.text = message.mMsgId;
			}

		});

		getMessageBtn.onClick.AddListener (delegate() {
			if(fromUser.text.Length == 0)
			{
				fromUser.placeholder.GetComponent<Text>().text = "input here first";
				return;
			}

			logText.text = "";

			List<EMMessage> list = EMClient.Instance.GetConversationMessage(fromUser.text,msgId.text,20);
				foreach(EMMessage msg in list){
					logText.text += "msg id:"+msg.mMsgId+",from:"+msg.mFrom;
					if(msg.mType == MessageType.TXT)
						logText.text += ",txt:"+msg.mTxt;
					if(msg.mType == MessageType.FILE)
						logText.text += ",path:"+msg.mRemotePath;
					logText.text += "\n";

				logContent.sizeDelta = new Vector2 (0, logText.preferredHeight+5);
			}
		});


		logoutBtn.onClick.AddListener (delegate() {
			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				SceneManager.LoadScene("LoginScene");
				GlobalListener.Instance.listenerInfo = "";
			};
			cb.onProgressCallback = (progress,status) => {
				
			};
			cb.onErrorCallback = (code,message) => {
				
			};
			EMClient.Instance.Logout(true,cb);
		});

		logText.text = GlobalListener.Instance.listenerInfo;

		sendExtMsgBtn.onClick.AddListener (delegate() {
			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				logText.text = "send message success";
			};
			cb.onProgressCallback = (progress,status) => {

			};
			cb.onErrorCallback = (code,message) => {

			};
			string jsonstr = "{\"key\":\"extinfotext\"}";
			EMClient.Instance.SendTextMessage(txtContent.text, toUser.text, ChatType.Chat, cb,jsonstr);

		});
	}
			
	IEnumerator GenCapture()
	{
		yield return new WaitForEndOfFrame();

		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width,height,TextureFormat.RGB24,false);
		tex.ReadPixels(new Rect(0,0,width,height),0,0,true);
		byte[] imagebytes = tex.EncodeToPNG();//转化为png图
		tex.Compress(false);//对屏幕缓存进行压缩
		string imagePath = "";
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			imagePath = Application.persistentDataPath;
		} else if (Application.platform == RuntimePlatform.WindowsPlayer) {
			imagePath = Application.dataPath;  
		} else if (Application.platform == RuntimePlatform.OSXEditor) {  
			imagePath = Application.dataPath; 
			imagePath = imagePath.Replace ("/Assets", null);  
		}

		string path = imagePath + "/" + "screencapture.png";
		File.WriteAllBytes(path,imagebytes);//存储png图
		logText.text = path;
		filePath.text = path;
	}



	private void setMessageRecvListener()
	{
		EMMessageListenerCallback receiveMessageCallback = new EMMessageListenerCallback();
		receiveMessageCallback.onMessageReceivedCallback = (msgs) =>
		{
			logText.text = "from ";
			foreach(EMMessage msg in msgs){
				logText.text += msg.mFrom;
				if(msg.mType == MessageType.TXT){
					logText.text += ",content="+msg.mTxt+",ext="+msg.ext;
				}

				if(msg.mType == MessageType.IMAGE)
				{
					rawImage.gameObject.SetActive(true);
					rawImage.GetComponent<ShowPicture>().loadPic(msg.mRemotePath);
				}else{
					rawImage.gameObject.SetActive(false);
				}

				if(msg.mType == MessageType.VOICE || msg.mType == MessageType.FILE)
				{
					logText.text += msg.mRemotePath;

					EMBaseCallback cb = new EMBaseCallback();
					cb.onSuccessCallback = () => {
						logText.text = "recv file success:" + msg.mLocalPath;
					};
					cb.onProgressCallback = (progress,status) => {

					};
					cb.onErrorCallback = (code,msg1) => {
						logText.text = "recv file failure:" + msg.mLocalPath;
					};
					EMClient.Instance.DownloadAttachment(msg.mFrom, msg.mMsgId, cb);
				}
			}
		};
		receiveMessageCallback.onMessageReadAckReceivedCallback = (msgs) => 
		{

		};
		receiveMessageCallback.onMessageDeliveryAckReceivedCallback = (msgs) => 
		{

		};
		receiveMessageCallback.onMessageChangedCallback = (msgs) => 
		{

		};
		receiveMessageCallback.onCmdMessageReceivedCallback = (msgs) => 
		{

		};
		EMClient.Instance.receiveMessageCallback = receiveMessageCallback;
	}

}
