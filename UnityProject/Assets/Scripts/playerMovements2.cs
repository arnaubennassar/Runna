using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerMovements2 : MonoBehaviour {
	
	public float turn = 50f;
	public float speed = 10f;
	public float slidePower = 20f;
	public AudioClip deadClip;
	public AudioClip coinClip;
	public AudioClip jumpClip;
	public AudioClip ouchClip;
	public Text countText;
	private Animator anim;
	private int JumpingState;
	private int JumpBool;
	private int SlideBool;
	private int FallingBool;
	private int EstampaoBool;
	private int RodolaBool;
	private bool derrapa;
	private bool salta;
	private bool noaug;
	private bool controlaRodola;
	private float h;
	private BoxCollider bxcol;
	private Rigidbody rb;
	private float elapsedS;
	private float durationS;
	private float elapsedSG;
	private float durationSG;
	private float elapsedSR;
	private float durationSR;
	private float elapsedB;
	private float durationB;
	private float elapsedCR;
	private float durationCR;
	private float elapsedGM;
	private float durationGM;
	private float elapsedGR;
	private float durationGR;
	private bool shakeBool;
	private bool bloodBool;
	private bool SlideCamBoolG;
	private bool SlideCamBoolR;
	private GameObject bloody;
	private Material matB;
	private AudioSource audi;
	private GameObject way;
	private GameObject gohan;
	private Quaternion rot;
	private bool ini;
	private bool rotBool;
	private Quaternion rotAux;
	private int count;
	public ParticleSystem partisys;
	public ParticleSystem partisys2;
	public ParticleSystem partisys3;
	private bool GohanMoveBool;
	private bool GohanReturnBool;
	private bool readyToHit;
	private float onDanger;
	private float tempoFade;
	private bool fadifade;
	private bool over;
	private int highScore;
	//private int runningState;
	// Use this for initialization
	void Start () {
		ini = false;
		way = GameObject.FindWithTag ("serp");
		matB = Resources.Load("sang", typeof(Material)) as Material;
		bloody =  GameObject.FindWithTag("Bloody");
		gohan = GameObject.FindWithTag("enemy");
		anim = GetComponent<Animator> ();
		//runningState = Animator.StringToHash("Base Layer.running");
		JumpBool = Animator.StringToHash("Jump");
		EstampaoBool = Animator.StringToHash("Estampao");
		SlideBool = Animator.StringToHash("Slide");
		FallingBool = Animator.StringToHash("Falling");
		RodolaBool = Animator.StringToHash("Rodola");
		bxcol = transform.gameObject.GetComponent<BoxCollider>();
		rb = transform.gameObject.GetComponent<Rigidbody>();
		audi = transform.gameObject.GetComponent<AudioSource>();
		derrapa = false;
		salta = false;
		controlaRodola = false;
		noaug = true;
		elapsedS = 0.0f;
		durationS = 0.0f;
		elapsedSG = 0.0f;
		durationSG = 0.0f;
		elapsedSR = 0.0f;
		durationSR = 0.0f;
		elapsedB = 0.0f;
		durationB = 0.0f;
		elapsedCR = 0.0f;
		durationCR = 0.0f;
		elapsedGM = 0.0f;
		durationGM = 0.0f;
		tempoFade = 0f;
		fadifade = false;
		shakeBool = false;
		bloodBool = false;
		rotBool = false;
		SlideCamBoolG = false;
		SlideCamBoolR = false;	
		GohanMoveBool = false;
		readyToHit = false;
		elapsedGR = 0f;
		durationGR = 0f;
		GohanReturnBool = false;
		count = 0;
		onDanger = 0f;
		over = false;
		highScore = PlayerPrefs.GetInt("Player Score2",0);
	}
	
	void SetCountText ()
	{
		countText.text = "Score " + count.ToString ()+"\nHighScore " + highScore;
	}
	
	bool isGrounded () {
		return Physics.Raycast(transform.position, -transform.up, 0.1f);
	}
	
	bool Falling () {
		return way.GetComponent<wayGenL2> ().isFalling(transform.position.y); //|| (Physics.Raycast(transform.position, -transform.up, 200f) & Physics.Raycast(transform.position, -transform.up, -200f) & transform.position.z > 40f & !IsJumping);
		//Debug.Log (Physics.Raycast(transform.position, -transform.up, 200f) || Physics.Raycast(transform.position, -transform.up, -200f));
		//return Physics.Raycast(transform.position, -transform.up, 200f) & Physics.Raycast(transform.position, -transform.up, -200f);
	}
	
	bool rodolaTime() {
		return Physics.Raycast(transform.position, -transform.up, 1.75f);
	}
	
	void OnTriggerEnter(Collider c) {
		//if (elapsed > duration) duration = elapsed + 3f;
		if (c.tag == "pinxos" || c.tag == "foc") {
			AudioSource.PlayClipAtPoint(ouchClip, transform.position);
			elapsedS = 0.0f;
			durationS = 1f;
			elapsedB = 0.0f;
			durationB = 1.5f;
			shakeBool = true;
			bloodBool = true;
			if (!readyToHit & !GohanMoveBool) {
				elapsedGM = 0f;
				durationGM = 1.5f;
				GohanMoveBool = true;
			}
			else {
				gohan.GetComponent<trollScript> ().ataca();
			}
			speed -= (speed - 8) / 8;
			AudioSource.PlayClipAtPoint(coinClip, transform.position);
			count -= 50;
			partisys3.Play();
			SetCountText ();
		} 
		else if (c.tag == "moneda") {
			AudioSource.PlayClipAtPoint(coinClip, transform.position);
			count += 20;
			partisys.Play();
			SetCountText ();
			Destroy(c.gameObject);
		}
		else if (c.tag == "diamant") {
			AudioSource.PlayClipAtPoint(coinClip, transform.position);
			count += 200;
			partisys2.Play();
			SetCountText ();
			Destroy(c.gameObject);
		}
	}
	
	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "obstacle") {
			//audi.Play();
			elapsedS = 0.0f;
			durationS = 1f;
			shakeBool = true;
			anim.SetBool (EstampaoBool,true);
			speed = 0;
			if (audi.isPlaying) audi.Stop();
			AudioSource.PlayClipAtPoint(deadClip, transform.position);
			if (count > highScore) PlayerPrefs.SetInt("Player Score2", count);
			GameObject.FindWithTag ("pantalla").GetComponent<fadeScript>().toTheEnd(0);
		}
		else if (c.gameObject.tag == "limitD") {
			AudioSource.PlayClipAtPoint(ouchClip, transform.position);
			elapsedS = 0.0f;
			durationS = 0.5f;
			shakeBool = true;
			speed -= (speed-8)/8;
			transform.Translate (Vector3.left*4);
			if (!readyToHit & !GohanMoveBool) {
				elapsedGM = 0f;
				durationGM = 1.5f;
				GohanMoveBool = true;
			}
			else if (!GohanReturnBool){
				gohan.GetComponent<trollScript> ().ataca();
			}
			AudioSource.PlayClipAtPoint(coinClip, transform.position);
			count -= 50;
			partisys3.Play();
			SetCountText ();
			//rb.AddForce(transform.right * -750 * 100);
			
		}
		else if (c.gameObject.tag == "limitE") {
			AudioSource.PlayClipAtPoint(ouchClip, transform.position);
			elapsedS = 0.0f;
			durationS = 0.5f;
			shakeBool = true;
			speed -= (speed-8)/8;
			transform.Translate (Vector3.right *4);
			if (!readyToHit & !GohanMoveBool) {
				elapsedGM = 0f;
				durationGM = 1.5f;
				GohanMoveBool = true;
			}
			else if (!GohanReturnBool){
				gohan.GetComponent<trollScript> ().ataca();
			}
			AudioSource.PlayClipAtPoint(coinClip, transform.position);
			count -= 50;
			partisys3.Play();
			SetCountText ();
			//rb.AddForce(transform.right * 750 * 100);
			
		}
	}
	
	public void dieBaby() {
		elapsedS = 0.0f;
		durationS = 1f;
		shakeBool = true;
		anim.SetBool (EstampaoBool,true);
		speed = 0;
		if (audi.isPlaying) audi.Stop();
		AudioSource.PlayClipAtPoint(deadClip, transform.position);
		elapsedGR = 0f;
		durationGR = 1.5f;
		GohanReturnBool = true;
		readyToHit = false;
		onDanger = 0f;
		fadifade = true;
		if (count > highScore) PlayerPrefs.SetInt("Player Score2", count);
		GameObject.FindWithTag ("pantalla").GetComponent<fadeScript>().toTheEnd(0);
		//GameObject.FindWithTag ("pantalla").transform.gameObject.GetComponent<fadeScenesScript> ().toPrincipalMenu ();
	}
	
	void gohanMove() {
		if (elapsedGM < durationGM) {
			elapsedGM += Time.deltaTime;
			Vector3 ini = new Vector3(0f,0f,-0.1f);
			Vector3 fi = new Vector3(0f,0f,-0.06f);
			gohan.transform.localPosition = Vector3.Lerp (ini, fi, elapsedGM / durationGM);
		} else {
			elapsedGM = 0f;
			durationGM = 0f;
			GohanMoveBool = false;
			readyToHit = true;
			onDanger = 0f;
		}
	}
	
	void gohanReturn() {
		if (elapsedGR < durationGR) {
			elapsedGR += Time.deltaTime;
			Vector3 fi = new Vector3(0f,0f,-0.1f);
			Vector3 ini = new Vector3(0f,0f,-0.06f);
			gohan.transform.localPosition = Vector3.Lerp (ini, fi, elapsedGR / durationGR);
		} else {
			elapsedGR = 0f;
			durationGR = 0f;
			GohanReturnBool = false;
		}
	}
	
	void bloodSplat() {
		if (elapsedB < durationB) {     
			elapsedB += Time.deltaTime;          
			bloody.transform.localPosition = new Vector3(0f,0f,0.3f);
			Color originalColour = matB.color;
			float newAlpha;
			if (elapsedB < durationB) newAlpha = 1 - (elapsedB/durationB);
			else newAlpha = 0f;
			matB.color = new Color(originalColour.r, originalColour.g, originalColour.b, newAlpha);
		}
		if (elapsedB >= durationB) {
			bloodBool = false;
			bloody.transform.localPosition = new Vector3(0f,0f,-0.3f);
		}
	}
	
	
	void CameraSlideGo() {
		if (elapsedSG < durationSG) {     
			elapsedSG += Time.deltaTime; 
			float y = (0.07278f - 0.01f) * (elapsedSG/durationSG);
			float z = (0.12f - 0.03f)*(elapsedSG/durationSG);
			//float y = Mathf.Clamp((elapsedSG/durationSG), Camera.main.transform.localPosition.y, 0.01f );
			//float z = Mathf.Clamp((elapsedSG/durationSG), -Camera.main.transform.localPosition.z, 0.03f);
			Camera.main.transform.localPosition = new Vector3 (0f, 0.07278f - y, -0.12f+z);    
		}
		if (elapsedSG >= durationSG) {
			SlideCamBoolG = false;
			Camera.main.transform.localPosition = new Vector3(0f,0.01f,-0.03f);
			elapsedSR = 0.0f;
			durationSR = 0.7f;
			SlideCamBoolR = true;
		}
	}
	
	void CameraSlideReturn() {
		if (elapsedSR < durationSR) {              
			elapsedSR += Time.deltaTime; 
			float y = (0.07278f - 0.01f) * (elapsedSR/durationSR);
			float z = (0.12f - 0.03f)*(elapsedSR/durationSR);
			Camera.main.transform.localPosition = new Vector3 (0f, 0.01f + y, -0.03f-z);
		}
		else if (elapsedSR >= durationSR) {
			SlideCamBoolR = false;
			Camera.main.transform.localPosition = new Vector3(0, 0.07278f, -0.12f);
		}
	}
	
	void CameraShake() {
		
		float magnitudeX = 0.5f;	
		float magnitudeY = 0.01f;	
		float magnitudeZ = 0.04f;
		if (elapsedS < durationS) {
			elapsedS += Time.deltaTime;          
			float percentComplete = elapsedS / durationS;         
			float damper = 1.0f - Mathf.Clamp (4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			float z = Random.value * 2.0f - 1.0f;
			x *= magnitudeX * damper;
			y *= magnitudeY * damper;
			z *= magnitudeZ * damper;
			Camera.main.transform.position += new Vector3 (x, y, z);
		}
		if (elapsedS >= durationS) {
			shakeBool = false;
			Camera.main.transform.localPosition = new Vector3(0, 0.07278f, -0.12f);
		}
	}
	
	void CameraRot(Quaternion q) {
		if (elapsedCR < durationCR) {
			elapsedCR += Time.deltaTime;         
			transform.rotation = Quaternion.Slerp (rot, rotAux, elapsedCR / durationCR);
			transform.Rotate(Vector3.right, -90);
		} 
		else if (elapsedCR >= durationCR) {
			rotBool = false;
			rot = rotAux;
			transform.rotation =  rot;
			transform.Rotate(Vector3.right, -90);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (readyToHit)
			onDanger += Time.deltaTime;
		if (onDanger > 10f & gohan.GetComponent<trollScript> ().running ()) {
			elapsedGR = 0f;
			durationGR = 1.5f;
			GohanReturnBool = true;
			readyToHit = false;
			onDanger = 0f;
		}
		SetCountText ();
		if (!ini) {
			//for(float i = 0; i < 1000000; ++i);
			while(!way.GetComponent<wayGenL2> ().getStart());
			Vector3 vini = way.GetComponent<wayGenL2> ().inici();
			transform.position = vini;
			rot = way.GetComponent<wayGenL2> ().nouse(transform.position.z);
			transform.rotation =  rot;
			transform.Rotate(Vector3.right, -90);
			ini = true;
		}
		if (!rotBool) rotAux = way.GetComponent<wayGenL2> ().nouse(transform.position.z);
		if (rotAux != rot & !rotBool) {
			elapsedCR = 0.0f;
			durationCR = 0.5f;
			rotBool = true;
			//rotAux.eulerAngles = new Vector3(rotAux.eulerAngles.x + Quaternion.Euler(0,-90,0).eulerAngles.x,rotAux.eulerAngles.y,rotAux.eulerAngles.z);
			
		}
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("falling_back_death") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0) anim.SetBool (EstampaoBool,false);
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("falling") & anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0 & !over) {
			AudioSource.PlayClipAtPoint(deadClip, transform.position);
			anim.SetBool (FallingBool, false);
			over = true;
			if (audi.isPlaying) audi.Stop();
			GameObject.FindWithTag ("pantalla").GetComponent<fadeScript>().toTheEnd(0);
		}
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("jumping") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0 & salta) {
			salta = false;
			AudioSource.PlayClipAtPoint(jumpClip, transform.position);
			speed -= (speed-15)/32;
			rb.AddForce(transform.up * 550 * 100);
			//bxcol.center = new Vector3(bxcol.center.x,bxcol.center.y*2f,bxcol.center.z);
		}
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("falling_idle") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0 & rodolaTime()) {
			anim.SetBool (RodolaBool,true);
			controlaRodola = true;
			//bxcol.center = new Vector3(bxcol.center.x,bxcol.center.y/2f,bxcol.center.z);
		}
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("jumping 2") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0 & controlaRodola) {
			anim.SetBool (RodolaBool,false);
			anim.SetBool (JumpBool,false);
			controlaRodola = false;
			
		}
		if (Falling () & !anim.GetCurrentAnimatorStateInfo (0).IsName ("falling")) {
			anim.SetBool (FallingBool,true);
			speed = 0f;
		}
		/*
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("jumping") & anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.26 & anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.27) 
		{
			rb.velocity+=transform.up * speed;
			//transform.position += transform.up * jumpPower * Time.deltaTime;
		}
		/*
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("jumping") & anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.38 & anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.50) 
		{
			transform.position -= transform.up * jumpPower * Time.deltaTime;
		}*/
		
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("running_slide") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.90 & derrapa) {
			anim.SetBool (SlideBool,false);
			derrapa = false;
			bxcol.center = new Vector3(bxcol.center.x,bxcol.center.y*4,bxcol.center.z/20);
			bxcol.size = new Vector3(bxcol.size.x/2,bxcol.size.y*4,bxcol.size.z/3);
			speed = slidePower;
			slidePower = 25f;
		}
		
		if ((Input.GetKey(KeyCode.Space) | Input.GetKey(KeyCode.UpArrow)) & !salta & anim.GetCurrentAnimatorStateInfo (0).IsName ("running") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0) {
			anim.SetBool (JumpBool,true);
			salta = true;
		}
		
		if (Input.GetKey(KeyCode.DownArrow) & !derrapa &  anim.GetCurrentAnimatorStateInfo (0).IsName ("running") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0) {
			anim.SetBool (SlideBool,true);
			bxcol.center = new Vector3(bxcol.center.x,bxcol.center.y/4,bxcol.center.z*20);
			bxcol.size = new Vector3(bxcol.size.x*2,bxcol.size.y/4,bxcol.size.z*3);
			derrapa = true;
			elapsedSG = 0.0f;
			durationSG = 0.7f;
			SlideCamBoolG = true;
			slidePower = speed;
			speed = 25f;
		}
		if (Input.GetKey (KeyCode.A)) {
			GameObject.FindWithTag ("pantalla").GetComponent<fadeScript>().toTheEnd(0);
		}
		/*
		if (Input.GetKey (KeyCode.A)) {
			transform.Rotate (Vector3.up, -turn * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (Vector3.up, turn * Time.deltaTime);
		}*/
		h = Input.GetAxis ("Horizontal");
		transform.Translate (Vector3.right * h * speed * Time.deltaTime);
		transform.position += transform.forward * speed * Time.deltaTime;
		if (!(Mathf.Round (transform.position.z) % 50 == 0))
			noaug = true;
		if (Mathf.Round (transform.position.z) % 50 == 0 & noaug) {
			if (speed < 25f) speed += 0.5f;
			noaug = false;
		}
		if (GohanReturnBool)
			gohanReturn ();
		if (GohanMoveBool)
			gohanMove ();
		if (rotBool)
			CameraRot (rotAux);
		if (SlideCamBoolG)
			CameraSlideGo ();
		if (SlideCamBoolR)
			CameraSlideReturn ();
		if (bloodBool)
			bloodSplat ();
		if (shakeBool) CameraShake();
	}
}
