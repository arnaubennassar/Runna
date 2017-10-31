using UnityEngine;
using System.Collections;

public class putamerda : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(PlayerPrefs.GetInt("isVR",1) == 0)GameObject.Find ("CardboardMain 1").GetComponent<Cardboard> ().VRModeEnabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
