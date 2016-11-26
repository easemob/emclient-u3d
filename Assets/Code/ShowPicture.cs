using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowPicture : MonoBehaviour {

	public RawImage image;

	// Use this for initialization
	void Start () {

	}

	public void loadPic(string picUrl)
	{
		StartCoroutine (GetTexture (picUrl));
	}

	IEnumerator GetTexture(string picUrl)
	{
		WWW wwwTexture = new WWW (picUrl);
		yield return wwwTexture;
		if (wwwTexture.error != null) {
			Debug.LogError ("downPic err:" + wwwTexture.error);
		} else {
			image.texture = wwwTexture.texture;
		}
	}
}
