using UnityEngine;
using System.Collections;

public class bonus : MonoBehaviour {
	private GameObject player; 
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (player.transform);
	}
}
