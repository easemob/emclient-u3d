using UnityEngine;
using System.Collections;
using EaseMob;

public class GlobalListener : MonoBehaviour {

	public GameObject obj;

	// Use this for initialization
	void Start () {
	
		UnityEngine.Object.DontDestroyOnLoad(obj);

		setConnectionListener ();
	}


	private void setConnectionListener()
	{
		EMConnListenerCallback connCb = new EMConnListenerCallback ();
		connCb.onConnectionCallback = () => {
			Debug.Log("connected");
		};
		connCb.onDisconnectedCallback = (code) => {
			Debug.Log("disconnect with code="+code);
		};
		EMClient.Instance.connListenerCallback = connCb;
	}
}
