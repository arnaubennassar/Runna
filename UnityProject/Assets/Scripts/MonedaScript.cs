using UnityEngine;
using System.Collections;

public class MonedaScript : MonoBehaviour {
	public float turn;
	// Use this for initialization
	void Start () {
		turn = 140f;
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate (Vector3.forward, turn * Time.deltaTime);
	}
}
