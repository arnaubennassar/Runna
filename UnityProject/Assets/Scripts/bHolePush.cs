using UnityEngine;
using System.Collections;

public class bHolePush : MonoBehaviour {

	void OnTriggerStay(Collider c){
		if (c.GetComponent<Rigidbody> () != null) {
			
			Vector3 direction = transform.GetComponentInParent<Transform>().position - c.transform.position;
			float distance = Vector3.Distance (transform.GetComponentInParent<Transform> ().position, c.transform.position);
			//direction = Vector3.Scale (direction.normalized, c.transform.localScale);
			direction =  transform.GetComponentInParent<Rigidbody>().mass * direction * (1/(distance*distance*distance));
			c.GetComponent<Rigidbody> ().useGravity = false;
			c.GetComponent<Rigidbody> ().AddForce (direction, ForceMode.Acceleration);
		}
	}

	void OnTriggerExit(Collider c){
		if (c.GetComponent<Rigidbody> () != null) {
			c.GetComponent<Rigidbody> ().useGravity = true;
		}
	}
}
