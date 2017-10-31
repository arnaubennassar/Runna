using UnityEngine;
using System.Collections;

public class upNdown : MonoBehaviour {
	public float top, bott, speed;
	private bool direction = false;

	void Update () {
		if (direction) {
			if (transform.localPosition.y < top)
				transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (speed * Time.deltaTime), transform.localPosition.z);
			else
				direction = false;
		} else {
			if (transform.localPosition.y > bott)
				transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - (speed * Time.deltaTime), transform.localPosition.z);
			else
				direction = true;
		}
	}
}
