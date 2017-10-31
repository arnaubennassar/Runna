using UnityEngine;
using System.Collections;

public class followMagnet : MonoBehaviour {
	public Transform target;
	public Vector3 offset;
	public bool enable = false;
	// Use this for initialization
	void Start () {
		if (target == null) {
			target = GameObject.FindWithTag ("Player").transform;
		}
	}

	// Update is called once per frame
	void Update () {
		transform.position = target.position + offset;
	}

	void OnTriggerEnter(Collider c) {
		if ((c.tag == "moneda" ||  c.tag == "diamant") & enable) {
			c.transform.position = Vector3.Lerp (c.transform.position, transform.position, 0.2f);
		}
	}
	void OnTriggerStay(Collider c) {
		if ((c.tag == "moneda" || c.tag == "diamant") & enable) {
			c.transform.position = Vector3.Lerp (c.transform.position, transform.position, 0.2f);
		}
	}
}
