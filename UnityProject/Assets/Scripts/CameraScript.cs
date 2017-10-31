using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private GUITexture gt;
	// Use this for initialization
	void Start () {
		gt = GetComponent<GUITexture> ();
		gt.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
		gt.color = Color.Lerp(gt.color, Color.red, 1.5f * Time.deltaTime);
	}


}
