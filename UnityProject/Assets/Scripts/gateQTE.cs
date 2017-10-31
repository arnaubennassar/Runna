using UnityEngine;
using System.Collections;

public class gateQTE : MonoBehaviour {
	// Use this for initialization
	public bool enable = false;
	bool itsOnRange = false;
	private bool open = false;
	private float elapsed, maxElapsed, maxDuration;
	public float duration = 2;
	private Quaternion ini1, ini2;
	private Transform porta1, porta2;
	private GameObject pl;
	private CardboardHead head;
	void Start () {
		switch (PlayerPrefs.GetString ("current weapon", "shit")) {
		case "escopet":
			duration = 1.5f;
			break;
		case "revolver":
			duration = 2f;
			break;
		case "deserteagle":
			duration = 1f;
			break;
		case "metralleta":
			duration = 0.5f;
			break;
		}
		elapsed = 0;
		maxElapsed = 0;
		maxDuration = 6;
		pl = GameObject.FindWithTag("Player");
		porta1 = transform.parent.FindChild ("pivotD1");
		porta2 = transform.parent.FindChild ("pivotD2");
		head = GameObject.FindWithTag ("stereo").GetComponent<StereoController>().Head;
	}

	void OnTriggerEnter(Collider c) {
		if (c.name == "partygirlSnake" & itsOnRange == false) {
			itsOnRange = true;
			GameObject.FindWithTag ("serp").GetComponent<wayGen> ().nouse ();
		}
	}


	void openGates(){
		elapsed += Time.deltaTime;
		if (elapsed < duration) {
			porta1.localRotation = Quaternion.Lerp (ini1, Quaternion.Euler (0, -70, 0), elapsed / duration);
			porta2.localRotation = Quaternion.Lerp (ini2, Quaternion.Euler (0, 70, 0), elapsed / duration);
		} else {
			porta1.localRotation =  Quaternion.Euler (0, -70, 0);
			porta2.localRotation = Quaternion.Euler (0, 70, 0);
			pl.GetComponent<playerMovements>().endQTE();
			Destroy (gameObject);
		}
	}
	void checkShoot(){
		RaycastHit hit;
		if (GetComponent<BoxCollider> ().Raycast (head.Gaze, out hit, Mathf.Infinity)) {
			if (elapsed == 0) pl.GetComponent<playerMovements> ().playApuntador = true;
			elapsed += Time.deltaTime;
			if (elapsed > duration) {
				pl.GetComponent<playerMovements>().Shoot();
				open = true;
				elapsed = 1;
				pl.GetComponent<playerMovements> ().playApuntador = false;
				pl.GetComponent<playerMovements> ().resetApuntador = true;
				transform.GetComponent<MeshRenderer> ().enabled = false;
			}
		} else {
			elapsed = 0;
			pl.GetComponent<playerMovements> ().playApuntador = false;
			pl.GetComponent<playerMovements> ().resetApuntador = true;

		}
	}

	// Update is called once per frame
	void Update () {
		if (!open) {
			if (itsOnRange) {
				checkShoot ();
				ini1 = porta1.rotation;
				ini2 = porta2.rotation;
			}
		}
		else openGates ();
	}
}
