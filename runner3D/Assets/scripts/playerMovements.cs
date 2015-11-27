using UnityEngine;
using System.Collections;

public class playerMovements : MonoBehaviour {

	public float turn = 20f;
	public float speed = 5f;
	private Animator anim;
	private bool salta;
	private bool derrapa;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		salta = false;
		derrapa = false;
	}

	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.UpArrow) && !salta) {
			anim.SetBool ("Jump",true);
			salta = true;
		}
		else if (!Input.GetKey(KeyCode.UpArrow)) {
			anim.SetBool ("Jump",false);
			salta = false;
		}
		if (Input.GetKey(KeyCode.Space) && !derrapa) {
			anim.SetBool ("Slide",true);
			derrapa = true;
		}
		else if (!Input.GetKey(KeyCode.Space)) {
			anim.SetBool ("Slide",false);
			derrapa = false;
		}
		if(Input.GetKey(KeyCode.A))
			transform.Rotate(Vector3.up, -turn * Time.deltaTime);
		if(Input.GetKey(KeyCode.D))
			transform.Rotate(Vector3.up, turn * Time.deltaTime);
		if(Input.GetKey(KeyCode.LeftArrow))
			transform.Translate(Vector3.left * speed * Time.deltaTime);
		if(Input.GetKey(KeyCode.RightArrow))
			transform.Translate(Vector3.right * speed * Time.deltaTime);

		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}
