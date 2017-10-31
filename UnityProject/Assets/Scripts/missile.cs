using UnityEngine;
using System.Collections;

public class missile : MonoBehaviour {
	public float speed;
	private float timeEla, maxTime;
	public GameObject explosionAnimation;
	// Use this for initialization
	void Start () {
		timeEla = 0;
		maxTime = 10;
	}
	
	// Update is called once per frame
	void Update () {
		timeEla += Time.deltaTime;
		if (timeEla > maxTime)
			Destroy (gameObject);
	//	transform.position -= new Vector3 (0, speed,0); 
	}

	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "serp") {
			Instantiate (explosionAnimation, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}
}
