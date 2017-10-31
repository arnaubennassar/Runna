
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class slideVR : MonoBehaviour {

	private CardboardHead head;
	private float delay = 0.0f; 
	private GameObject player;

	void Start() {
		head = GameObject.FindWithTag ("stereo").GetComponent<StereoController>().Head;
		player = GameObject.FindWithTag ("Player");
	}

	void Update() {
		RaycastHit hit;
		bool isLookedAt = GetComponent<Collider>().Raycast(head.Gaze, out hit, Mathf.Infinity);
		if (isLookedAt) {
			player.GetComponent<playerMovements> ().makeMeSlide ();
		}

	}

}