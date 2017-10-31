using UnityEngine;
using System.Collections;

public class bHoleEat : MonoBehaviour {

	float mass;
	float nextScale;
	float growSpeed = 1;

	public float maxSpeedRotation = 2;

	void Awake(){
		mass = transform.localScale.x * 1000;
		gameObject.GetComponent<Rigidbody>().mass = mass;
	}

	void OnCollisionEnter(Collision c){
		if (c.gameObject.GetComponent<Rigidbody> () != null) {
			Debug.Log ("eat zone");
			mass += c.gameObject.GetComponent<Rigidbody> ().mass;
			nextScale = mass / 1000;
			GetComponent<Rigidbody> ().mass = mass;
			if (c.gameObject.tag == "Player") {
				Debug.Log ("destroy tha body");
				SkinnedMeshRenderer[] skmr = c.gameObject.transform.GetChild (8).GetComponentsInChildren<SkinnedMeshRenderer> ();
				for (int i = 0; i < skmr.Length; ++i) {
					skmr [i].enabled = false;
				}
			}
			else Destroy (c.gameObject);
		}
	}

	void OnCollisionStay(Collision c){
		c.transform.position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0,Time.deltaTime * Random.Range(1, maxSpeedRotation),0);
		if (transform.localScale.x < nextScale) {
			transform.localScale = transform.localScale + new Vector3 (growSpeed, growSpeed, growSpeed) * Time.deltaTime;
		}
	}
}
