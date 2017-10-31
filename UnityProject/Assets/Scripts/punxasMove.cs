using UnityEngine;
using System.Collections;

public class punxasMove : MonoBehaviour {
	private bool tb;
	private Vector3 ini;
	private Vector3 fi;	
	private Vector3 posIni, posFin;
	public float timeUp;
	private AudioSource audi;
	float i = 0f;
	// Use this for initialization
	void Start () {
		posIni = new Vector3 (2.67f, 2f, 8.7f);
		posFin = new Vector3 (2.67f, -3f, 8.7f);
		tb = true;
		audi = transform.gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		i += Time.deltaTime;
		if (tb) {
			ini = posIni;
			fi = posFin;
		} else {
			fi = posIni;
			ini = posFin;
		}
		if (i < timeUp)
			transform.localPosition = Vector3.Lerp(ini, fi, i/timeUp);
		else if(i > timeUp) {
			i = 0f;
			tb = !tb;
			//if (tb) audi.Play();
		}
	}
}
