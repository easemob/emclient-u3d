using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EaseMob;
using UnityEngine.SceneManagement;
 
public class Login : MonoBehaviour {

	public InputField username;
	public InputField password;
	public Button regBtn;
	public Button loginBtn;
	public Button logoutBtn;
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
			logText.text = "Disconnected! code=" + code;
		};
		EMClient.Instance.connListenerCallback = connCb;

		regBtn.onClick.AddListener (delegate() {
			string nametext = username.text;
			string pwdtext = password.text;
			int ret = EMClient.Instance.CreateAccount(nametext,pwdtext);
			if(ret == 0){
				logText.text = "reg success";
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
				SceneManager.LoadScene("MainScene");
			};
			cb.onProgressCallback = (p,s) => {
				logText.text = ("prograss="+p+",status="+s);
			};
			cb.onErrorCallback = (c,m) => {
				logText.text = ("Err code="+c+",msg="+m);
			};
			EMClient.Instance.Login(nametext,pwdtext,cb);
		});

		logoutBtn.onClick.AddListener (delegate() {
			EMBaseCallback cb = new EMBaseCallback();
			cb.onSuccessCallback = () => {
				logText.text = "logout success";
			};
			cb.onProgressCallback = (p,s) => {
				logText.text = ("prograss="+p+",status="+s);
			};
			cb.onErrorCallback = (c,m) => {
				logText.text = ("Err code="+c+",msg="+m);
			};
			EMClient.Instance.Logout(true,cb);
		});
	}
	
	public void regBtnClick()
	{

	}

	public void loginBtnClick()
	{

	}
}
