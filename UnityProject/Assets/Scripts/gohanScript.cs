using UnityEngine;
using System.Collections;

public class gohanScript : MonoBehaviour {

	private int attackBool;
	private Animator anim;
	private bool atacant;
	private GameObject bola;
	private float elapsedG;
	private float durationG;
	private float elapsedH;
	private float durationH;
	private bool bolaGrowBool;
	private bool hitHerBool;
	private GameObject pl;
	private Vector3 playerPos;
	public Vector3 offset;
	public int multi;
	// Use this for initialization
	void Start () {
		transform.FindChild("Mesh.125_DrawCall_63").GetComponent<SkinnedMeshRenderer>().enabled = false;
		transform.FindChild("aura").GetComponent<MeshRenderer>().enabled = false;
		anim = GetComponent<Animator> ();
		attackBool = Animator.StringToHash("attack");
		pl = GameObject.FindWithTag("Player");
		atacant = false;
		bola = transform.Find("hameka").gameObject;
		elapsedG = 0f;
		durationG = 0f;
		elapsedH = 0f;
		durationH = 0f;
		bolaGrowBool = false;
		hitHerBool = false;
		playerPos = pl.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		playerPos = pl.transform.position;
		transform.position = Vector3.Lerp (transform.position, playerPos + offset + multi * pl.transform.forward, 0.1f); 
		if (hitHerBool)
			hitHer();
		if (bolaGrowBool)
			bolaGrow ();
		if(bolaGrowBool || hitHerBool)transform.LookAt (playerPos);
		else transform.LookAt (playerPos + offset + (multi+5) * pl.transform.forward);
	}

	void hitHer() {
		
		if (elapsedH < durationH) {
			elapsedH += Time.deltaTime;
			Vector3 ini = transform.position;
			Vector3 fi = playerPos;
			Vector3 iniS = new Vector3(0.15f,0.15f,0.15f);
			Vector3 fiS = new Vector3(0.5f,0.5f,0.5f);
			bola.transform.localScale = Vector3.Lerp (iniS, fiS, elapsedH / durationH);
			bola.transform.position = Vector3.Lerp (ini, fi, elapsedH / durationH);
		} else {
			hitHerBool = false;
			elapsedH = 0f;
			durationH = 0f;
		//	bola.GetComponent<ParticleSystem>().Stop();
			bola.transform.localScale = new Vector3(0.0f,0.0f,0.0f);
			pl.GetComponent<playerMovements>().dieBaby();

		}
	}


	void bolaGrow() {
		if (elapsedG < durationG) {
			elapsedG += Time.deltaTime;
			Vector3 ini = new Vector3(0.01f,0.01f,0.01f);
			Vector3 fi = new Vector3(0.15f,0.15f,0.15f);
			bola.transform.localScale = Vector3.Lerp (ini, fi, elapsedG / durationG);
		} else {
			elapsedG = 0f;
			durationG = 0f;
			bolaGrowBool = false;
			hitHerBool = true;
			elapsedH = 0f;
			durationH = 0.4f;
			anim.SetBool (attackBool,false);
		}
	}

	public void ataca() {
		if (!atacant) {
			anim.SetBool (attackBool,true);
			atacant = true;
		//	bola.GetComponent<ParticleSystem>().Play();
			elapsedG = 0f;
			durationG = 2.7f;
			bolaGrowBool = true;
			//bola.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		}

	}

	public bool flying() {
		return anim.GetCurrentAnimatorStateInfo (0).IsName ("flying") & anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0;
	}
}