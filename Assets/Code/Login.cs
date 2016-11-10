using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
public class Login : MonoBehaviour {

	public InputField username;
	public InputField password;
	public Button regBtn;
	public Button loginBtn;
	public Text logText;

	void Awake ()
	{
		EMClient.Instance.Init ();
	}

	// Use this for initialization
	void Start () {

		username.text = "user1";
		password.text = "123";


		EMConnListenerCallback connCb = new EMConnListenerCallback ();
		connCb.onConnectionCallback = () => {
			logText.text = "Connected!";

		};
		connCb.onDisconnectedCallback = (code) => {
			logText.color = new Color (255, 0, 0);
			logText.text = "Disconnected! code=" + code;
		};
		EMClient.Instance.connListenerCallback = connCb;

	
		regBtn.onClick.AddListener (delegate() {
			string nametext = username.text;
			string pwdtext = password.text;
			int ret = EMClient.Instance.CreateAccount(nametext,pwdtext);
			if(ret == 0){
				logText.text = "reg ok";
//				Application.LoadLevel("MainScene");
			}else{
				logText.text = "reg error,code="+ret;
			}
		});

		loginBtn.onClick.AddListener (delegate() {
			string nametext = username.text;
			string pwdtext = password.text;
			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				logText.text = "login success";
				Application.LoadLevel("MainScene");
			};
			cb.onProgressCallback = (p,s) => {
				logText.text = ("p="+p+",s="+s);
			};
			cb.onErrorCallback = (c,m) => {
				logText.text = ("c="+c+",m="+m);
			};
			Debug.LogError("login username="+nametext);
			EMClient.Instance.Login(nametext,pwdtext,cb);
		});
	}
	
	public void regBtnClick()
	{

	}

	public void loginBtnClick()
	{

	}
}
