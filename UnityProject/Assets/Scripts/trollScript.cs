using UnityEngine;
using System.Collections;

public class trollScript : MonoBehaviour {
	
	private int attackBool;
	private int dieBool;
	private Animator anim;
	private bool atacant;
	private float elapsedH;
	private float durationH;
	private float elapsedS;
	public float durationS;
	private float elapsedD;
	private float durationD;
	private bool hitHerBool;
	private bool setAnimation;
	private bool die;
	private GameObject pl;
//	private Vector3 playerPos;
	private Vector3 ini;
	private Vector3 fi;
	private CardboardHead head;
	public ParticleSystem partisys;
	// Use this for initialization
	void Start () {
		partisys.Stop ();
		die = false;
		transform.FindChild("smdimport").GetComponent<SkinnedMeshRenderer>().enabled = false;
		head = GameObject.FindWithTag ("stereo").GetComponent<StereoController>().Head;
		anim = GetComponent<Animator> ();
		attackBool = Animator.StringToHash("attack");
		dieBool = Animator.StringToHash("die");
		pl = GameObject.FindWithTag("Player");
		atacant = false;
		elapsedH = 0f;
		durationH = 0f;
		hitHerBool = false;
		setAnimation = false;
		fi = pl.transform.position;
		ini = transform.localPosition;
		elapsedS = 0f;
	//	fi = new Vector3 (0f, 0f, -0.02f);
	}
	
	// Update is called once per frame
	void Update () {
		fi = pl.transform.position;
		if (hitHerBool) {
			transform.LookAt (fi);
			hitHer();
		}
		if (die) {
			transform.LookAt (fi);
			Die ();
		}
	}

	void Die(){
		elapsedD += Time.deltaTime;
		if (elapsedD > durationD) {
			pl.GetComponent<playerMovements> ().endQTE ();
			transform.FindChild ("smdimport").GetComponent<SkinnedMeshRenderer> ().enabled = false;
			die = false;
			anim.SetBool (dieBool, false);
			anim.SetBool (attackBool, false);
		}

	}

	void hitHer() {
		if (elapsedH < durationH) {
			RaycastHit hit;
			elapsedH += Time.deltaTime;
			if (durationH - elapsedH < 1.5f & !setAnimation) {
				anim.SetBool (attackBool, true);
				setAnimation = false;
			}
			Debug.Log (ini+" " + fi+" "+Vector3.Distance(transform.position, fi));
			if(Vector3.Distance(transform.position, fi) > 4)transform.position = Vector3.Lerp (ini, fi, elapsedH / durationH);
			if (GameObject.FindWithTag ("trollHead").GetComponent<Collider> ().Raycast (head.Gaze, out hit, Mathf.Infinity)) {
				pl.GetComponent<playerMovements> ().playApuntador = true;
				elapsedS += Time.deltaTime;
	//			Debug.Log ("Head Shoot Duration = " + elapsedS);
			} else if (!GetComponent<Collider> ().Raycast (head.Gaze, out hit, Mathf.Infinity)) {
				elapsedS = 0f;
				pl.GetComponent<playerMovements> ().playApuntador = false;
				pl.GetComponent<playerMovements> ().resetApuntador = true;
			}
			else pl.GetComponent<playerMovements> ().playApuntador = false;
			if(elapsedS > durationS){
				elapsedS = 0f;
				atacant = false;
				die = true;
				hitHerBool = false;
				elapsedH = 0f;
				durationH = 0f;
				elapsedD = 0f;
				durationD = 1.5f;
				anim.SetBool (dieBool,true);
				partisys.Play ();
				pl.GetComponent<playerMovements>().Shoot();
				partisys.Emit (1);
			}
		} else {
			hitHerBool = false;
			elapsedH = 0f;
			durationH = 0f;
			pl.GetComponent<playerMovements>().dieBaby();
		}
	}
	
	public void ataca() {
		if (!atacant) {
			//partisys.si
		//	anim.SetBool (attackBool,true);
			atacant = true;
			hitHerBool = true;
			elapsedH = 0f;
			durationH = 6f;	//1.4
			ini = getPosIni();
			transform.FindChild("smdimport").GetComponent<SkinnedMeshRenderer>().enabled = true;
			//bola.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		}
		
	}

	Vector3 getPosIni(){
		Vector3 vaux;
		vaux = pl.transform.position + new Vector3 (Random.Range(-20, 20), 0,Random.Range(-20, 20));
		while(Vector3.Distance(vaux, fi) < 10)vaux = pl.transform.position + new Vector3 (Random.Range(-20, 20), 0,Random.Range(-20, 20));
		return vaux;
	}
	
	public bool running() {
		return anim.GetCurrentAnimatorStateInfo (0).IsName ("zombie_running") & anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0;
	}
}
