using UnityEngine;
using System.Collections;

public class avioneta : MonoBehaviour {
	public GameObject missile;
	private bool atacant;
	private float elapsedH, elapsedS;
	private float durationH;
	private float elapsedIn;
	private float durationIn;
	private float elapsedOut;
	private float durationOut;
	private GameObject pl;
	//	private Vector3 playerPos;
	private Vector3 ini;
	private Vector3 fi;
	private bool dispara, ves, surt;
	private Transform fi2;
	// Use this for initialization
	void Start () {
		pl = GameObject.FindWithTag ("Player").gameObject;
		transform.rotation = Quaternion.Euler (270, 90, 0);
		GetComponent<MeshRenderer>().enabled = false;
		dispara = false;
		ves = false;
		surt = false;
		atacant = false;
		elapsedH = 0f;
		durationH = 0f;
		elapsedS = 0;
	}

	// Update is called once per frame
	void Update () {
	/*	if (ves) {
			Debug.Log ("Ves-hi");
		//	transform.LookAt (fi);
			Ves ();
		}*/
		 if (dispara) {
		//	Debug.Log ("dispara");
	//		transform.LookAt (fi);
			hitHer();
		}
		else if (surt) {
		//	Debug.Log ("SURT");
	//		transform.LookAt (fi);
			Surt();
		}
	}
		
/*	void Ves(){
		elapsedIn += Time.deltaTime;
		if (elapsedIn < durationIn) {
			transform.position = Vector3.Lerp (ini, fi, elapsedIn / durationIn );
		} else {
			ves = false;
			dispara = true;
			elapsedIn = 0;
			durationIn = 0;
			elapsedH = 0f;
			durationH = 2f;
			ini = transform.position;
			fi = GameObject.FindWithTag ("Player").transform.position;
			fi += new Vector3 (0, 10, 0);
			fi += fi - ini;
		}
	}*/
	void hitHer() {
		if (elapsedH < durationH) {
			elapsedH += Time.deltaTime;
			elapsedS += Time.deltaTime;
			transform.position = Vector3.Lerp (ini, fi2.position + fi2.forward*100 + new Vector3(0,30,0), elapsedH / (durationH));
			transform.LookAt (fi2.position + fi2.forward*110 + new Vector3(0,10,0));
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3 (270, 90, 0));
			if (elapsedS > 0.5f) {
				Instantiate (missile, transform.position, missile.transform.rotation);
				elapsedS = 0;
			}
		} else {
			dispara = false;
			elapsedH = 0f;
			durationH = 0f;

			surt = true;
			elapsedOut = 0;
			durationOut = 2;
			fi = fi2.position + fi2.forward*300 + new Vector3(0,100,0);
			ini = transform.position;
		}
	}
	void Surt(){
		if (elapsedOut < durationOut) {
			elapsedOut += Time.deltaTime;
			transform.position = Vector3.Lerp (ini, fi, elapsedOut / (durationOut));
			transform.LookAt (fi);
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3 (270, 90, 0));
		} else {
			surt = false;
			atacant = false;
			elapsedOut = 0f;
			durationOut = 0f;
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<AudioSource> ().Stop();
	//		Vector3 vaux =  GameObject.FindWithTag ("Player").transform.position;
	//		transform.position = vaux + (vaux - transform.position); 
		}
	}

	public void ataca() {
		if (!atacant) {
			atacant = true;
			dispara = true;
		//	elapsedIn = 0f;
		//	durationIn = 1;	
		//	ini = transform.position;
			elapsedH = 0f;
			durationH = 3f;
			ini = pl.transform.position -30*pl.transform.forward + new Vector3(0,30,0);
			ini += new Vector3 (0, 30, 0);
			fi2 = pl.transform;
			GetComponent<MeshRenderer>().enabled = true;
			GetComponent<AudioSource> ().Play ();
			//transform.LookAt (fi);
		}
	}
}
