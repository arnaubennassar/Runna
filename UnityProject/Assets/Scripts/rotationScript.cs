using UnityEngine;
using System.Collections;

public class rotationScript : MonoBehaviour {
	public float rotationSpeed = 90;
	public float delay = 0.0f;
	public bool localRot = false;
	public Vector3 onAxis = new Vector3(0, 1, 0);

	// Update is called once per frame
	void Update () {
		if (delay <= 0) {
			if (!localRot) {
				Quaternion rotAux = Quaternion.Euler (transform.rotation.eulerAngles.x + (Time.deltaTime * rotationSpeed) * onAxis.x, transform.rotation.eulerAngles.y + (Time.deltaTime * rotationSpeed) * onAxis.y, transform.rotation.eulerAngles.z + (Time.deltaTime * rotationSpeed) * onAxis.z);
				transform.rotation = rotAux;
				delay -= Time.deltaTime;
			}
			else {
				Quaternion rotAux = Quaternion.Euler (transform.localRotation.eulerAngles.x + (Time.deltaTime * rotationSpeed) * onAxis.x, transform.localRotation.eulerAngles.y + (Time.deltaTime * rotationSpeed) * onAxis.y, transform.localRotation.eulerAngles.z + (Time.deltaTime * rotationSpeed) * onAxis.z);
				transform.localRotation = rotAux;
				delay -= Time.deltaTime;
			}
		}
	}
}
