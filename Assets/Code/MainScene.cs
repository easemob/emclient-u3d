using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using LitJson;
using EaseMob;

public class MainScene : MonoBehaviour {

	public Button friendListBtn;
	public InputField txtContent;
	public Button sendTxtMessageBtn;
	public Text logText;
	public Dropdown friendListDd;
	public Button screenShot;
	public Dropdown picComprass;
	public Button sendPicMessageBtn;
	public Button startRecord;
	public Button stopRecord;
	public Button sendVoiceMessageBtn;
	public InputField voicePath;
	public InputField fromUser;
	public Button getMessageBtn;
	public Button logoutBtn;
	public RawImage rawImage;
	public InputField filePath;
	public Button sendFileMessageBtn;

	private string picFilePath = "";
	private List<Dropdown.OptionData> friendList = new List<Dropdown.OptionData>();

	// Use this for initialization
	void Start () {
		
		rawImage.gameObject.SetActive(false);

		setConnectionListener ();

		setMessageRecvListener ();

		setGroupListener ();


		friendListBtn.onClick.AddListener (delegate {
			friendList.Clear();
			string names = EMClient.Instance.GetAllContactsFromServer();
			Dropdown.OptionData od = new Dropdown.OptionData();
			od.text = "choose";
			friendList.Add(od);
			if(names.Length > 0){
				string[] namearr = names.Split(new char[] { ',' });
				foreach(string name in namearr){
					Dropdown.OptionData dod = new Dropdown.OptionData();
					dod.text = name;
					friendList.Add(dod);
				}
			}
			friendListDd.AddOptions(friendList);
		});

		sendTxtMessageBtn.onClick.AddListener (delegate() {
			if(friendListDd.value == 0){
				logText.text = "not choose";
				logText.color = new Color(255,0,0);
				return;
			}
			Dropdown.OptionData dod = friendList[friendListDd.value];

			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				logText.text = "send message success";
			};
			cb.onProgressCallback = (progress,status) => {
				
			};
			cb.onErrorCallback = (code,message) => {
				
			};
			EMClient.Instance.SendTextMessage(txtContent.text, dod.text, ChatType.Chat, cb);
		});

		screenShot.onClick.AddListener (delegate() {
			StartCoroutine(GenCapture());
		});

		sendPicMessageBtn.onClick.AddListener (delegate() {
			if(picFilePath!=""){
				Dropdown.OptionData dod = friendList[friendListDd.value];
				EMBaseCallback cb = new EMBaseCallback();
				cb.onSuccessCallback = () => {
					logText.text = "send Picture success";
				};
				cb.onProgressCallback = (progress,status) => {

				};
				cb.onErrorCallback = (code,message) => {

				};
				EMClient.Instance.SendPictureMessage(picFilePath, picComprass.value==1 ? true : false, dod.text, ChatType.Chat, cb);
			}
		});

		sendVoiceMessageBtn.onClick.AddListener (delegate() {
			if(voicePath.text.Length>0)
			{
				Dropdown.OptionData dod = friendList[friendListDd.value];
				EMBaseCallback cb = new EMBaseCallback();
				cb.onSuccessCallback = () => {
					logText.text = "send Voice success";
				};
				cb.onProgressCallback = (progress,status) => {

				};
				cb.onErrorCallback = (code,message) => {

				};
				EMClient.Instance.SendVoiceMessage(voicePath.text,10,dod.text,ChatType.Chat,cb);
			}
		});

		sendFileMessageBtn.onClick.AddListener (delegate() {
			if(filePath.text.Length > 0){
				Dropdown.OptionData dod = friendList[friendListDd.value];
				EMBaseCallback cb = new EMBaseCallback();
				cb.onSuccessCallback = () => {
					logText.text = "send file success";
				};
				cb.onProgressCallback = (progress,status) => {

				};
				cb.onErrorCallback = (code,msg) => {

				};
				EMClient.Instance.SendFileMessage(filePath.text,dod.text,ChatType.Chat,cb);
			}

		});
		/*
		startRecord.onClick.AddListener (delegate() {
			EMRecordCallback rcb = new EMRecordCallback();
			rcb.onStopRecordCallback=(path,length)=>{
				Dropdown.OptionData dod = friendList[friendListDd.value];

				EMBaseCallback cb = new EMBaseCallback();
				cb.onSuccessCallback = () => {
					logText.text = "send message success";
				};
				cb.onProgressCallback = (p,s) => {

				};
				cb.onErrorCallback = (c,m) => {

				};
				EMClient.Instance.SendVoiceMessage(path,length,dod.text,0,cb);
			};
			EMClient.Instance.StartRecord(rcb);
		});

		stopRecord.onClick.AddListener (delegate() {
			EMClient.Instance.StopRecord();
		});
		*/

		getMessageBtn.onClick.AddListener (delegate() {
			string fromuser = fromUser.text;
			if(fromuser.Length > 0){
				string json = EMClient.Instance.GetAllConversationMessage(fromuser);
				logText.text = json;
			}
		});


		logoutBtn.onClick.AddListener (delegate() {
			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				Application.LoadLevel("LoginScene");
			};
			cb.onProgressCallback = (progress,status) => {
				
			};
			cb.onErrorCallback = (code,message) => {
				
			};
			EMClient.Instance.Logout(true,cb);
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
		picFilePath = path;
		filePath.text = path;
	}

	private void setConnectionListener()
	{
		EMConnListenerCallback connCb = new EMConnListenerCallback ();
		connCb.onConnectionCallback = () => {

		};
		connCb.onDisconnectedCallback = (code) => {
			logText.color = new Color (255, 0, 0);
			logText.text = "Disconnected! code=" + code;
		};
		EMClient.Instance.connListenerCallback = connCb;
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
					logText.text += ",content="+msg.mTxt;
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
					voicePath.text = msg.mLocalPath;
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

	private void setGroupListener()
	{
		
		EMGroupListenerCallback groupListenerCallback = new EMGroupListenerCallback ();
		groupListenerCallback.onUserRemovedCallback = (groupId,groupName) => {

		};
		groupListenerCallback.onInvitationAccepted = (groupId, inviter, reason) => {

		};
		groupListenerCallback.onInvitationDeclined = (groupId, invitee, reason) => {
		
		};
		groupListenerCallback.onInvitationAccepted = (groupId, inviter, reason) => {

		};
		groupListenerCallback.onGroupDestroyed = (groupId, groupName) => {

		};
		groupListenerCallback.onAutoAcceptInvitationFromGroup = (groupId, inviter, inviteMessage) => {

		};
		groupListenerCallback.onApplicationReceived = (groupId, groupName, applicant, reason) => {

		};
		groupListenerCallback.onApplicationDeclined = (groupId, groupName, decliner, reason) => {

		};
		groupListenerCallback.onApplicationAccept = (groupId, groupName, accepter) => {

		};
		EMClient.Instance.groupListenerCallback = groupListenerCallback;

	}
}
