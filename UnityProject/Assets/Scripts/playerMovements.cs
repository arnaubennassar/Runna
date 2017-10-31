using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerMovements : MonoBehaviour {

	public float turn = 50f;
	public float speed = 10f;
	public float slidePower = 20f;
	public AudioClip deadClip;
	public AudioClip winClip;
	public AudioClip coinClip;
	private AudioClip jumpClip;
	private AudioClip ouchClip;
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
	private int highScore;
	public ParticleSystem partisys;
	public ParticleSystem partisys2;
	public ParticleSystem partisys3;
	private bool GohanMoveBool;
	private bool GohanReturnBool;
	private bool readyToHit;
	private float onDanger;
	private float tempoFade;
	private bool fadifade;

	//private int runningState;
	// Use this for initialization


	//afegit arnau:

	private GameObject troll;
	private GameObject avioneta;
	private GameObject vrCam;
	private int loadShoot, shoot, multiplier, indexApuntador;
	private GameObject quickGO, shieldFX, magnetFX, magneto;
	public bool quickTE, quickEnd;
	private float elapsedQT;
	private AudioClip shootClip;
	public AudioClip slideClip;
	public AudioClip shieldClip;
	public AudioClip magnetClip;
	public AudioClip pointsClipUp, pointsClipDown;
	public int enemyAct;
	public float maxSpeed;
	public bool resetApuntador, playApuntador;
	private bool shield, x2points, magnet;
	private float eShield, dShield, ePoints, dPoints, eMagnet, dMagnet, eApuntador;
	private float dApuntador = 2;
	private int shockDis, coinsCol, powersCol, coinDis, powerDis;
	private float runnedM;
	private Transform playerArm;
	bool deadTroll = false;
	float eEnemy = 0;
	float dEnemy = 3;
	float eFalling = 0;
	float dFalling = 6;
	Quaternion newRotAux;
	bool gateE = false;
	float eGate = 0;
	float dGate = 5;


	void Start () {
		troll = GameObject.FindWithTag("troll");
		loadPrefs ();
		ini = false;
		way = GameObject.FindWithTag ("serp");
		matB = Resources.Load("sang", typeof(Material)) as Material;
		bloody =  GameObject.FindWithTag("Bloody");
		quickGO = GameObject.FindWithTag ("qFX");
		shieldFX = GameObject.FindWithTag ("sFX");
		magnetFX = GameObject.FindWithTag ("mFx");
		gohan = GameObject.FindWithTag("enemy");
		avioneta = GameObject.FindWithTag("avioneta");

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
		bloody.transform.localPosition = new Vector3(0f,0f,-0.5f);
		rotBool = false;
		SlideCamBoolG = false;
		SlideCamBoolR = false;	
		GohanMoveBool = false;
		readyToHit = false;
		elapsedGR = 0f;
		durationGR = 0f;
		GohanReturnBool = false; 
		count = 0;
		highScore = PlayerPrefs.GetInt("Player Score1",0);
		onDanger = 0f;
		//ARNAU
		vrCam = GameObject.FindWithTag ("stereo");
	//	enemyAct = 1;
		quickTE = false;
		shield = false;
		eShield = 0f;
		dShield = 10f;
		ePoints = 0f;
		dPoints = 15f;
		multiplier = 1;
		magneto = GameObject.FindWithTag ("magneto");
		magnet = false;
		shieldFX.transform.localPosition = new Vector3 (0f, 0, -3);
		magnetFX.transform.localPosition = new Vector3 (0f, 0, -3);
		magneto.GetComponent<followMagnet> ().enable = false;
		eMagnet = 0;
		dMagnet = 15;
		eApuntador = 0;
		indexApuntador = 4;
		quickGO.GetComponent<spriteAC> ().manualIndex = true;
		quickGO.GetComponent<spriteAC> ()._index = 4;
		runnedM = shockDis = coinsCol = powersCol = coinDis = powerDis = 0;
	} 

	void loadPrefs(){
		if(PlayerPrefs.GetInt("isVR",0) == 0)GameObject.Find ("CardboardMain").GetComponent<Cardboard> ().VRModeEnabled = false;
		if (PlayerPrefs.GetInt ("show", 0) == 1) {
			GameObject.Find ("Salta").GetComponent<MeshRenderer> ().enabled = false;
			GameObject.Find ("Slide").GetComponent<MeshRenderer> ().enabled = false;
		}
		// LOAD PERSON 

		Transform taux = transform.GetChild (6);  
		GameObject go = null;
		string charName = PlayerPrefs.GetString ("current runner", "partyGirl");
		switch (charName) {
		//

		case "partyGirl":
			jumpClip = Resources.Load ("sounds/girJump") as AudioClip;
			ouchClip = Resources.Load ("sounds/girOuch") as AudioClip;
			go = Instantiate (Resources.Load ("persons/partygirl", typeof(GameObject))) as GameObject; 
			go.transform.parent = transform;
			go.transform.localScale = new Vector3 (1, 1, 1);
			go.transform.localPosition = Vector3.zero;
			maxSpeed = 15;
			speed = maxSpeed / 2f;
			taux.localPosition = new Vector3 (0.0f, 0.01379f, 0.002f);

			break;

		case "ggggamora":
			jumpClip = Resources.Load ("sounds/gamJump") as AudioClip;
			ouchClip = Resources.Load ("sounds/gamOuch") as AudioClip;
			go = Instantiate (Resources.Load ("persons/gamora", typeof(GameObject))) as GameObject; 
			go.transform.parent = transform;
			go.transform.localScale = new Vector3 (0.40f, 0.40f, 0.40f);
			go.transform.localPosition = new Vector3 (0, 0.0057f, 0);
			maxSpeed = 20;
			speed = maxSpeed / 2f;
			taux.localPosition = new Vector3 (0.0f, 0.013f, 0.0013f);

			break;

		case "aragon":
			jumpClip = Resources.Load ("sounds/araJump") as AudioClip;
			ouchClip = Resources.Load ("sounds/araOuch") as AudioClip;
			go = Instantiate (Resources.Load ("persons/aragon", typeof(GameObject))) as GameObject; 
			go.transform.parent = transform;
			go.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
			go.transform.localPosition = new Vector3 (0, 0.009f, 0);
			taux.localPosition = new Vector3 (0.0f, 0.015f, 0.001f);
			maxSpeed = 25;
			speed = maxSpeed / 2f;
			break;

		}

		go.transform.localRotation =  Quaternion.identity;
		anim = GetComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load("girl") as RuntimeAnimatorController;
		//runningState = Animator.StringToHash("Base Layer.running");
		JumpBool = Animator.StringToHash("Jump");
		EstampaoBool = Animator.StringToHash("Estampao");
		SlideBool = Animator.StringToHash("Slide");
		FallingBool = Animator.StringToHash("Falling");
		RodolaBool = Animator.StringToHash("Rodola");
		loadShoot = Animator.StringToHash ("loadSh");
		shoot = Animator.StringToHash ("shoot");

		GameObject weap = null;
		Debug.Log (PlayerPrefs.GetString ("current weapon", "shit"));
		Vector3 offset = Vector3.zero;
		Vector3 rotOff = Vector3.zero;
		switch (PlayerPrefs.GetString ("current weapon", "revolver")) {

		case "escopet":
			shootClip = Resources.Load ("sounds/escopet") as AudioClip;
			dApuntador = 1.5f;
			troll.GetComponent<trollScript> ().durationS = dApuntador;
			weap = Instantiate (Resources.Load ("weapons/escopet", typeof(GameObject))) as GameObject; 
			weap.transform.localScale = new Vector3 (0.0025f, 0.0025f, 0.0025f);
			offset = new Vector3 (0.00033f, 0.00397f, 0.00003f);
			rotOff = new Vector3 (0, 90, 90);
			if (charName == "aragon") {
				rotOff = new Vector3 (90, 0, 0);
				offset = new Vector3 (0.083f, 0.288f, 0.078f);
			} else if (charName == "ggggamora") {
				offset = new Vector3 (-0.00003f, 0.0084f, 0.00016f);
				rotOff = new Vector3 (0, 90, 90);
			}
			break;
		case "revolver":
			shootClip = Resources.Load ("sounds/revolver") as AudioClip;
			dApuntador = 2f;
			troll.GetComponent<trollScript> ().durationS = dApuntador;
			weap = Instantiate (Resources.Load ("weapons/revolver", typeof(GameObject))) as GameObject; 
			offset = new Vector3 (-0.0014f, 0.00424f, 0.00034f);
			rotOff = new Vector3 (90,300,-65);
			if (charName == "aragon") {
				rotOff = new Vector3 (0, 270, -90);
				offset = new Vector3 (-0.028f, 0.123f, 0.029f);
			}
			else if (charName == "partyGirl") {
				offset = new Vector3 (-0.0006f, 0.00214f, 0.00024f);
			}

			break;
		case "deserteagle":
			shootClip = Resources.Load ("sounds/dEagle") as AudioClip;
			dApuntador = 1f;
			troll.GetComponent<trollScript> ().durationS = dApuntador;
			weap = Instantiate (Resources.Load ("weapons/deserteagle", typeof(GameObject))) as GameObject; 
			weap.transform.localScale = new Vector3 (2, 2, 2);
			offset = new Vector3 (0.0011f, 0.00171f, 0.0004f);
			rotOff = new Vector3 (180,180,0);
			if (charName == "aragon") rotOff += new Vector3 (0,0,-90);
			else if (charName == "partyGirl") {
				offset = new Vector3 (0.00019f, 0.00096f, 0);
			}
			break;
		case "metralleta":
			shootClip = Resources.Load ("sounds/metrallet") as AudioClip;
			dApuntador = 0.5f;
			troll.GetComponent<trollScript> ().durationS = dApuntador;
			weap = Instantiate (Resources.Load ("weapons/metralleta", typeof(GameObject))) as GameObject; 
			rotOff = new Vector3 (90,0,0);
			offset = new Vector3 (-0.0008f, 0.00411f, 0f);
			if (charName == "ggggamora") {
				rotOff = new Vector3 (90,0,0);
				offset = new Vector3 (-0.0025f, 0.0082f, -0.0001f);
			} else if (charName == "aragon") {
				rotOff = new Vector3 (0, -90, -90);
				offset = new Vector3 (-0.033f, 0.303f, 0.042f);
			}
			break;
		}

		playerArm = go.transform.FindChild ("mixamorig:Hips").FindChild ("mixamorig:Spine").FindChild ("mixamorig:Spine1").FindChild ("mixamorig:Spine2").FindChild ("mixamorig:RightShoulder").FindChild ("mixamorig:RightArm").FindChild ("mixamorig:RightForeArm");
		weap.transform.parent = playerArm.transform.FindChild ("mixamorig:RightHand").GetComponent<Transform> ();
		weap.transform.localPosition = Vector3.zero + offset;
		weap.transform.rotation = Quaternion.Euler (rotOff);

	}

	//ARNAU{
	public void makeMeJump(){
		if (!salta & anim.GetCurrentAnimatorStateInfo (0).IsName ("running") & anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0 & !quickTE & ini) {
			salta = true;
			anim.SetBool (JumpBool, true);
		}
	}

	public void makeMeSlide(){
		if (!derrapa &  anim.GetCurrentAnimatorStateInfo (0).IsName ("running") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0 & !quickTE & ini) {
			anim.SetBool (SlideBool,true);
			AudioSource.PlayClipAtPoint(slideClip, transform.position);
			bxcol.center = new Vector3(bxcol.center.x,bxcol.center.y/4,bxcol.center.z*4);
			bxcol.size = new Vector3(bxcol.size.x*2,bxcol.size.y/4,bxcol.size.z*3);
			derrapa = true;
			elapsedSG = 0.0f;
			durationSG = 0.7f;
			SlideCamBoolG = true;
			slidePower = speed;
			speed = 25f;
		}
	}

	//}ARNAU
	void SetCountText ()
	{
		countText.text = "Score " + count.ToString ()+"      HighScore " + highScore+"\nMultiplier " + multiplier;

	}

	bool isGrounded () {
		return Physics.Raycast(transform.position, -transform.up, 0.1f);
	}

	bool Falling () {
		eFalling += Time.deltaTime;
		if (eFalling > dFalling)
			return true;
		else
			return false;
		//return way.GetComponent<wayGen> ().isFalling(transform.position.y); //|| (Physics.Raycast(transform.position, -transform.up, 200f) & Physics.Raycast(transform.position, -transform.up, -200f) & transform.position.z > 40f & !IsJumping);
		//Debug.Log (Physics.Raycast(transform.position, -transform.up, 200f) || Physics.Raycast(transform.position, -transform.up, -200f));
		//return Physics.Raycast(transform.position, -transform.up, 200f) & Physics.Raycast(transform.position, -transform.up, -200f);
	}

	bool rodolaTime() {
		return Physics.Raycast(transform.position, -transform.up, 1.75f);
	}

	void OnTriggerEnter(Collider c) {
		Debug.Log (c.name);
		//if (elapsed > duration) duration = elapsed + 3f;
		if (c.tag == "pinxos" || c.tag == "foc" & !shield /*|| c.tag == "limitE" || c.tag == "limitD" */) {
			if (shockDis == 0)
				shockDis = (int) runnedM;
			AudioSource.PlayClipAtPoint (ouchClip, transform.position);
			elapsedS = 0.0f;
			durationS = 1f;
			elapsedB = 0.0f;
			durationB = 1.5f;
			shakeBool = true;

			bloodBool = true;
			if (!readyToHit & !GohanMoveBool & !shield) {
				elapsedGM = 0f;
				durationGM = 1.5f;
				GohanMoveBool = true;
				if (enemyAct == 0) {
					gohan.transform.FindChild ("Mesh.125_DrawCall_63").GetComponent<SkinnedMeshRenderer> ().enabled = true;
					gohan.transform.FindChild ("aura").GetComponent<MeshRenderer> ().enabled = true;
				}
				if (enemyAct == 1) {
					troll.GetComponent<trollScript> ().ataca ();
					deadTroll = true;
				}
				if (enemyAct == 2)
					avioneta.GetComponent<avioneta> ().ataca ();
			} else {
				if (enemyAct == 0 & !shield)
					gohan.GetComponent<gohanScript> ().ataca ();
				//if(enemyAct == 1) troll.GetComponent<trollScript> ().ataca();
			}
			speed -= (speed - 8) / 8;
			AudioSource.PlayClipAtPoint (coinClip, transform.position);
			count -= 50;
			partisys3.Play ();
			SetCountText ();
		} else if (c.tag == "moneda") {
			if (coinDis == 0)
				coinDis = (int) runnedM;
			++coinsCol;
			AudioSource.PlayClipAtPoint (coinClip, transform.position);
			count += 20 * multiplier;
			partisys.Play ();
			SetCountText ();
			Destroy (c.gameObject);
		} else if (c.tag == "diamant") {
			if (coinDis == 0)
				coinDis = (int) runnedM;
			++coinsCol;
			AudioSource.PlayClipAtPoint (coinClip, transform.position);
			count += 200 * multiplier;
			partisys2.Play ();
			SetCountText ();
			Destroy (c.gameObject);
		} else if (c.tag == "diamant2") {
			if (coinDis == 0)
				coinDis = (int) runnedM;
			++coinsCol;
			AudioSource.PlayClipAtPoint (winClip, transform.position);
			count += 500 * multiplier;
			partisys2.Play ();
			SetCountText ();
			Destroy (c.gameObject);
			if (audi.isPlaying)
				audi.Stop ();
			checkSuperatedChallanges ();
			PlayerPrefs.SetInt ("last score", count);
			GameObject.FindWithTag ("pantalla").GetComponent<challanges> ().toTheEnd (0);
		} else if (c.tag == "shield") {
			if (powerDis == 0)
				powerDis = (int) runnedM;
			++powersCol;
			shield = true;
			eShield = 0;
			shieldFX.transform.localPosition = new Vector3 (0f, 0, 0.8f);
			AudioSource.PlayClipAtPoint (shieldClip, transform.position);
		} else if (c.tag == "double points") {
			if (powerDis == 0)
				powerDis = (int) runnedM;
			++powersCol;
			x2points = true;
			ePoints = 0;
			multiplier *= 2;
			AudioSource.PlayClipAtPoint (pointsClipUp, transform.position);
		} else if (c.tag == "magnet") {
			if (powerDis == 0)
				powerDis = (int) runnedM;
			++powersCol;
			magnet = true;
			magnetFX.transform.localPosition = new Vector3 (0f, 0, 0.8f);
			eMagnet = 0;
			magneto.GetComponent<followMagnet> ().enable = true;
			AudioSource.PlayClipAtPoint (magnetClip, transform.position);
		}
		if (shield & (c.tag == "pinxos" || c.tag == "foc")) {
			if (shockDis == 0)
				shockDis = (int) runnedM;
			shield = false;
			shieldFX.transform.localPosition = new Vector3 (0f, 0, -3);
		}
	}

	void OnCollisionStay(Collision c) {
		if (c.gameObject.tag == "serp") {
			eFalling = 0;
			newRotAux = Quaternion.FromToRotation(transform.up, c.contacts[0].normal) * transform.rotation;
		}
	}

	void OnCollisionEnter(Collision c) {
		Debug.Log ("has chocat cobtra "+c.gameObject.name);
		if (c.gameObject.tag == "serp") {
			eFalling = 0;
		//	Debug.Log ("neew rotation");
			newRotAux = Quaternion.FromToRotation(transform.up, c.contacts[0].normal) * transform.rotation;
		}
		if (c.gameObject.tag == "obstacle") {
			if (shockDis == 0)
				shockDis = (int) runnedM;
			//audi.Play();
			elapsedS = 0.0f;
			durationS = 1f;
			shakeBool = true;
			anim.SetBool (EstampaoBool,true);
			speed = 0;
			if (audi.isPlaying) audi.Stop();
			AudioSource.PlayClipAtPoint(deadClip, transform.position);
			if (count > highScore) PlayerPrefs.SetInt("Player Score1", count);
			checkSuperatedChallanges ();
			PlayerPrefs.SetInt ("last score", count);
			GameObject.FindWithTag ("pantalla").GetComponent<challanges>().toTheEnd(0);
		}
		else if (c.gameObject.tag == "limitD") {
			if (shockDis == 0)
				shockDis = (int) runnedM;
			elapsedB = 0.0f;
			durationB = 1.5f;
			if(!shield)bloodBool = true;
			AudioSource.PlayClipAtPoint(ouchClip, transform.position);
			elapsedS = 0.0f;
			durationS = 0.5f;
			shakeBool = true;
			speed -= (speed-8)/8;
			transform.Translate (Vector3.left*4);
			if (!readyToHit & !GohanMoveBool & !shield) {
				elapsedGM = 0f;
				durationGM = 1.5f;
				GohanMoveBool = true;
				if (enemyAct == 0) {
					gohan.transform.FindChild ("Mesh.125_DrawCall_63").GetComponent<SkinnedMeshRenderer> ().enabled = true;
					gohan.transform.FindChild ("aura").GetComponent<MeshRenderer> ().enabled = true;
				}
				if (enemyAct == 1) {
					troll.GetComponent<trollScript> ().ataca ();
					deadTroll = true;
				}
				else if(enemyAct == 2) avioneta.GetComponent<avioneta> ().ataca();
			}
			else if (!GohanReturnBool & !shield){
				if (enemyAct == 0)
					gohan.GetComponent<gohanScript> ().ataca ();
				else if (enemyAct == 1) {
					troll.GetComponent<trollScript> ().ataca ();
					deadTroll = true;
				}
				else if(enemyAct == 2) avioneta.GetComponent<avioneta> ().ataca();
			}
			if (shield) {
				shield = false;
				shieldFX.transform.localPosition = new Vector3 (0f, 0, -3);
			}
			AudioSource.PlayClipAtPoint(coinClip, transform.position);
			count -= 50;
			partisys3.Play();
			SetCountText ();
			//rb.AddForce(transform.right * -750 * 100);

		}
		else if (c.gameObject.tag == "limitE") {
			if (shockDis == 0)
				shockDis = (int) runnedM;

			elapsedB = 0.0f;
			durationB = 1.5f;
			if(!shield) bloodBool = true;
			AudioSource.PlayClipAtPoint(ouchClip, transform.position);
			elapsedS = 0.0f;
			durationS = 0.5f;
			shakeBool = true;
			speed -= (speed-8)/8;
			transform.Translate (Vector3.right *4);
			if (!readyToHit & !GohanMoveBool & !shield) {
				elapsedGM = 0f;
				durationGM = 1.5f;
				GohanMoveBool = true;
				if (enemyAct == 0) {
					gohan.transform.FindChild ("Mesh.125_DrawCall_63").GetComponent<SkinnedMeshRenderer> ().enabled = true;
					gohan.transform.FindChild ("aura").GetComponent<MeshRenderer> ().enabled = true;
				}
				if (enemyAct == 1) {
					troll.GetComponent<trollScript> ().ataca ();
					deadTroll = true;
				}
				else if(enemyAct == 2) avioneta.GetComponent<avioneta> ().ataca();
			}
			else if (!GohanReturnBool & !shield){
				if (enemyAct == 0)
					gohan.GetComponent<gohanScript> ().ataca ();
				else if (enemyAct == 1) {
					troll.GetComponent<trollScript> ().ataca ();
					deadTroll = true;
				}
				else if(enemyAct == 2) avioneta.GetComponent<avioneta> ().ataca();
			}
			AudioSource.PlayClipAtPoint(coinClip, transform.position);
			count -= 50;
			partisys3.Play();
			SetCountText ();
			//rb.AddForce(transform.right * 750 * 100);
			if (shield) shield = false;

		}

	}

/*	public void bHoled(){
	
	}*/
	//		runnedM = shockDis = coinsCol = powersCol = coinDis = powerDis = 0;
	void checkSuperatedChallanges(){
		if (coinDis == 0)
			coinDis = (int) runnedM;
		if (shockDis == 0)
			shockDis = (int) runnedM;
		if (powerDis == 0)
			powerDis = (int) runnedM;
		Debug.Log ("estadisitiques de la partida : "+runnedM+" correguts, "+shockDis+" distancia de xoc, "+coinsCol+" monedes agafades, "+powersCol+" powerups agafats, "+coinDis+" metres sense monedes, "+powerDis+" metres sense poders, "+count+" punts");
		int i = 0;
		for (int k = 1; k < 4; ++k){
			int chScore = PlayerPrefs.GetInt ("current challangeScore " + k.ToString (), k - 1);
			int chId = PlayerPrefs.GetInt ("current challangeId " + k.ToString (), k - 1);
	//		Debug.Log (chScore +" "+chId);
			switch(chId){
			case 0:
				if (chScore <= runnedM) {
					PlayerPrefs.SetInt ("current challangeId " + k.ToString (), -1);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "sprtd", 0);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "lvl", 1 + PlayerPrefs.GetInt ("ch" + chId.ToString () + "lvl", 1));

				}
				break;
			case 1:
				Debug.Log ("japuuu");
				if (chScore <= shockDis) {
					PlayerPrefs.SetInt ("current challangeId " + k.ToString (), -1);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "sprtd", 0);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "lvl", 1 + PlayerPrefs.GetInt ("ch" + chId.ToString () + "lvl", 1));

				}
				break;
			case 2:
				if (chScore <= count) {
					PlayerPrefs.SetInt ("current challangeId " + k.ToString (), -1);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "sprtd", 0);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "lvl", 1 + PlayerPrefs.GetInt ("ch" + chId.ToString () + "lvl", 1));

				}
				break;
			case 3:
				if (chScore <= coinsCol) {
					PlayerPrefs.SetInt ("current challangeId " + k.ToString (), -1);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "sprtd", 0);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "lvl", 1 + PlayerPrefs.GetInt ("ch" + chId.ToString () + "lvl", 1));

				}
				break;
			case 4:
				if (chScore <= powersCol) {
					PlayerPrefs.SetInt ("current challangeId " + k.ToString (), -1);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "sprtd", 0);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "lvl", 1 + PlayerPrefs.GetInt ("ch" + chId.ToString () + "lvl", 1));

				}
				break;
			case 5:
				if (chScore <= coinDis) {
					PlayerPrefs.SetInt ("current challangeId " + k.ToString (), -1);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "sprtd", 0);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "lvl", 1 + PlayerPrefs.GetInt ("ch" + chId.ToString () + "lvl", 1));

				}
				break;
			case 6:
				if (chScore <= powerDis) {
					PlayerPrefs.SetInt ("current challangeId " + k.ToString (), -1);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "sprtd", 0);
					PlayerPrefs.SetInt ("ch" + chId.ToString () + "lvl", 1 + PlayerPrefs.GetInt ("ch" + chId.ToString () + "lvl", 1));

				}
				break;
			}
		}
	}

	public void dieBaby() {
		elapsedS = 0.0f;
		durationS = 1f;
		shakeBool = true;
		if(!anim.GetCurrentAnimatorStateInfo (0).IsName ("falling"))anim.SetBool (EstampaoBool,true);
		speed = 0;
		if (audi.isPlaying) audi.Stop();
		AudioSource.PlayClipAtPoint(deadClip, transform.position);
		elapsedGR = 0f;
		durationGR = 1.5f;
		GohanReturnBool = true;
		readyToHit = false;
		onDanger = 0f;
		fadifade = true;
		if (count > highScore) PlayerPrefs.SetInt("Player Score1", count);
		checkSuperatedChallanges ();
		PlayerPrefs.SetInt ("last score", count);
		GameObject.FindWithTag ("pantalla").GetComponent<challanges>().toTheEnd(0);

	//	GameObject.FindWithTag ("pantalla").transform.gameObject.GetComponent<fadeScenesScript> ().toPrincipalMenu ();
	}

	void gohanMove() {
		if (elapsedGM < durationGM) {
			if (enemyAct == 0) {
				elapsedGM += Time.deltaTime;
			//	Vector3 ini = new Vector3 (-0.05f, 0.05f, 0.01f);
			//	Vector3 fi = new Vector3 (-0.05f, 0.05f, 0.1f);
			//	gohan.transform.localPosition = Vector3.Lerp (ini, fi, elapsedGM / durationGM);
			} else if (enemyAct == 1) {
				quickGO .transform.localPosition = new Vector3 (0f, 0, 3f);
				anim.SetBool (loadShoot, true);
				quickTE = true;
				deadTroll = true;
			//	vrCam.transform.GetComponentInParent<followOS> ().offset = new Vector3 (0f, 0.2f, -0.18f);
			//	transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y +20, transform.rotation.eulerAngles.z);
				elapsedGM += Time.deltaTime;
				Vector3 ini = new Vector3(0f,0f,-0.1f);
				Vector3 fi = new Vector3(0f,0f,-0.06f);
		//		troll.transform.localPosition = Vector3.Lerp (ini, fi, elapsedGM / durationGM);
			}
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
			if (enemyAct == 0) {
				elapsedGR += Time.deltaTime;
			//	Vector3 fi = new Vector3 (-0.05f, 0.05f, 0.01f);
			//	Vector3 ini = new Vector3 (-0.05f, 0.05f, 0.1f);
			//	gohan.transform.localPosition = Vector3.Lerp (ini, fi, elapsedGR / durationGR);

			}/*else if (enemyAct == 1) {
				if(quickTE) quickGO .transform.localPosition = new Vector3 (0f, 0, -100f);
				quickTE = false;
				elapsedGR += Time.deltaTime;
				Vector3 fi = new Vector3(0f,0f,-0.1f);
				Vector3 ini = new Vector3(0f,0f,-0.06f);
				troll.transform.localPosition = Vector3.Lerp (ini, fi, elapsedGR / durationGR);
			}*/
		} else {
			elapsedGR = 0f;
			durationGR = 0f;
			GohanReturnBool = false;
			gohan.transform.FindChild("Mesh.125_DrawCall_63").GetComponent<SkinnedMeshRenderer>().enabled = false;
			gohan.transform.FindChild("aura").GetComponent<MeshRenderer>().enabled = false;
		}
	}

	public void endQTE(){
		elapsedQT = 0f;
		quickEnd = true;
		anim.SetBool (loadShoot, false);
		anim.SetBool (shoot, false);
		quickGO .transform.localPosition = new Vector3 (0f, 0, -100f);
	}

	public void Shoot(){
		anim.SetBool (loadShoot, false);
		anim.SetBool (shoot, true);
		AudioSource.PlayClipAtPoint(shootClip, transform.position);
	}

	void qTEnd(){
		elapsedQT += Time.deltaTime;
	//	vrCam.transform.rotation = Quaternion.Lerp (vrCam.transform.rotation, way.GetComponent<wayGen> ().nouse (transform.position.z, transform.position.x), elapsedQT);
		if(elapsedQT > 1){
		//	vrCam.transform.GetComponentInParent<followOS> ().offset = new Vector3 (0f, 0.1f, -0.18f);
			quickTE = false;
			quickEnd = false;
		}
	}

	public void gateEvent(){
	//	Debug.Log ("new mofokin method");
		gateE = true;
		eGate += Time.deltaTime;
		quickTE = true;
		quickGO .transform.localPosition = new Vector3 (0f, 0, 3f);
		//playApuntador = true;
		anim.SetBool (loadShoot, true);
		if (eGate > dGate) {
			endQTE();
			quickTE = false;
			playApuntador = false;
			resetApuntador = true;
			eGate = 0;
			gateE = false;
		}
	}

	public void qtStart(){
		Debug.Log (" qtStart !!!!!!!!");
		quickGO .transform.localPosition = new Vector3 (0f, 0, 3f);
		anim.SetBool (loadShoot, true);
		quickTE = true;
	}

	void bloodSplat() {
		
		if (elapsedB < durationB) { 
	
			elapsedB += Time.deltaTime;          
			bloody.transform.localPosition = new Vector3(0f,0f,0.5f);
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
	/*
	void CameraRot(Quaternion q) {
		if (elapsedCR < durationCR) {
			elapsedCR += Time.deltaTime;         
			transform.rotation = Quaternion.Slerp (rot, rotAux, elapsedCR / durationCR);
			transform.Rotate(Vector3.right, -90);
		} 
		else if (elapsedCR >= durationCR) {
			rotBool = false;
			rot = rotAux;
	//		transform.rotation =  rot;
		//	transform.Rotate(Vector3.right, -90);
		}
	} */
	void animacioApuntador(){
		if (playApuntador) {
			eApuntador += Time.deltaTime;
			indexApuntador =4+ (int)((eApuntador / dApuntador) * 15);
			if (indexApuntador > 15)
				indexApuntador -= 16;
			if (eApuntador > dApuntador)
				indexApuntador = 3;
		} 
		if (resetApuntador) {
			indexApuntador = 4;
			eApuntador = 0;
			resetApuntador = false;
		}
		quickGO.GetComponent<spriteAC> ()._index = indexApuntador;

	}



	// Update is called once per frame
	void FixedUpdate () {
	//	way.GetComponent<wayGen> ().nouse (transform.position);
		if (!ini) {
			//for(float i = 0; i < 1000000; ++i);
			while (!way.GetComponent<wayGen> ().getStart ());
			Vector3 vini = way.GetComponent<wayGen> ().inici ();
			transform.position = vini;
		//	rot = way.GetComponent<wayGen> ().nouse (transform.position);
			transform.rotation = Quaternion.identity;
			transform.Rotate (Vector3.right, -90);
			if (eEnemy > dEnemy) {
				ini = true;
				eEnemy = 0;
			}
			eEnemy += Time.deltaTime;
		} else {

			eEnemy += Time.deltaTime;
			Debug.Log ("elapsedGm = "+elapsedGM+" elapsedGR = "+elapsedGR+" gohanMoveBool = "+GohanMoveBool+" read2hit  = "+readyToHit);
			if (enemyAct == 2)
				GohanMoveBool = false;
			if (eEnemy > dEnemy & !(quickTE || elapsedGM != 0f || elapsedGR != 0f || GohanMoveBool || readyToHit) ) {
				//elapsedGM = 0f;
				eEnemy = 1;
				enemyAct = (enemyAct+1)%3;
			}
		}
		if (readyToHit)
			onDanger += Time.deltaTime;
		if (shield) {
			eShield += Time.deltaTime;
			if (eShield > dShield) {
				shield = false;
				eShield = 0;
				shieldFX .transform.localPosition = new Vector3 (0f, 0, -3f);
				AudioSource.PlayClipAtPoint (shieldClip, transform.position);
			}
		}
		else shieldFX .transform.localPosition = new Vector3 (0f, 0, -3f);
		if (x2points) {
			ePoints += Time.deltaTime;
			if (ePoints > dPoints) {
				x2points = false;
				multiplier = 1;
				AudioSource.PlayClipAtPoint (pointsClipDown, transform.position);
			}
		}

		if (magnet) {
			eMagnet += Time.deltaTime;
			if (eMagnet > dMagnet) {
				magnet = false;
				magnetFX.transform.localPosition = new Vector3 (0f, 0, -3);
				magneto.GetComponent<followMagnet> ().enable = false;
				AudioSource.PlayClipAtPoint (magnetClip, transform.position);
			}
		}
		if (onDanger > 10f) {
			if (enemyAct == 0 & gohan.GetComponent<gohanScript> ().flying ()) {
				elapsedGR = 0f;
				durationGR = 1.5f;
				GohanReturnBool = true;
				readyToHit = false;
				onDanger = 0f;
			}
			else if (enemyAct == 1 & troll.GetComponent<trollScript> ().running ()) {
				elapsedGR = 0f;
				durationGR = 1.5f;
				GohanReturnBool = true;
				readyToHit = false;
				onDanger = 0f;
				deadTroll = true;
			}
		}
		SetCountText ();

	//	if (!rotBool) {
			//modificat ARNAU : noems sagafa la rotacio en l'eix x del segment actual del cami


		//SMOOTH IT?



	//	if(way.GetComponent<wayGen> ().isUp()) rotAux = Quaternion.Euler (way.GetComponent<wayGen> ().nouse (transform.position).eulerAngles.x-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
	//	else rotAux = Quaternion.Euler (360-(way.GetComponent<wayGen> ().nouse (transform.position.z, transform.position.x).eulerAngles.x-90), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		if (!quickTE)
			transform.rotation = newRotAux;
		else
			animacioApuntador ();

	//	}
	/*	if (rotAux != rot & !rotBool) {
			elapsedCR = 0.0f;
			durationCR = 0.5f;
			rotBool = true;
			//rotAux.eulerAngles = new Vector3(rotAux.eulerAngles.x + Quaternion.Euler(0,-90,0).eulerAngles.x,rotAux.eulerAngles.y,rotAux.eulerAngles.z);

		}*/
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("falling_back_death") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0) anim.SetBool (EstampaoBool,false);
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("falling") & anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0) anim.SetBool (FallingBool,false);
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("jumping") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0 & salta) {
			salta = false;
			AudioSource.PlayClipAtPoint(jumpClip, transform.position);
			speed -= (speed-15)/32;
			rb.AddForce(transform.up * 550 * 10000000);
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
		if (Falling ()) {
			anim.SetBool (FallingBool,true);
			speed -= 2f;
			if (speed < 0) {
				dieBaby ();
				speed = 10000;
			}
		}

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
			bxcol.center = new Vector3(bxcol.center.x,bxcol.center.y*4,bxcol.center.z/4);
			bxcol.size = new Vector3(bxcol.size.x/2,bxcol.size.y*4,bxcol.size.z/3);
			speed = slidePower;
			slidePower = 25f;
		}
			
		if ((Input.GetKey(KeyCode.Space) | Input.GetKey(KeyCode.UpArrow)) & !salta & anim.GetCurrentAnimatorStateInfo (0).IsName ("running") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0) {
			anim.SetBool (JumpBool,true);
			salta = true;
		}

		if (Input.GetKey(KeyCode.DownArrow) & !derrapa &  anim.GetCurrentAnimatorStateInfo (0).IsName ("running") & anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0) {
			AudioSource.PlayClipAtPoint(slideClip, transform.position);
			anim.SetBool (SlideBool,true);
			bxcol.center = new Vector3(bxcol.center.x,bxcol.center.y/4,bxcol.center.z*4);
			bxcol.size = new Vector3(bxcol.size.x*2,bxcol.size.y/4,bxcol.size.z*3);
			derrapa = true;
			elapsedSG = 0.0f;
			durationSG = 0.7f;
			SlideCamBoolG = true;
			slidePower = speed;
			speed = 25f;
		}

		if (Input.GetKey (KeyCode.A)) {
			checkSuperatedChallanges ();
			PlayerPrefs.SetInt ("last score", count);
			GameObject.FindWithTag ("pantalla").GetComponent<challanges>().toTheEnd(0);

		}


		//afegit ARNAU: aconseguir que la pava miri a on la camera de VR (nomes en l'eix y)
	/*	if (!quickTE)  */ 
		transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x, vrCam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
	//	if (quickTE) playerArm.l
	/*	else {
			transform.rotation = Quaternion.Euler (vrCam.transform.rotation.eulerAngles.x, vrCam.transform.rotation.eulerAngles.y +20, vrCam.transform.rotation.eulerAngles.z);
		}	*/


		/*
		if (Input.GetKey (KeyCode.D)) {
			transform.Rotate (Vector3.up, turn * Time.deltaTime);
		}*/
		h = Input.GetAxis ("Horizontal");
		if(!quickTE & ini)transform.Translate (Vector3.right * h * speed * Time.deltaTime);
		if(!quickTE & ini)transform.position += transform.forward * speed * Time.deltaTime;
		if(!quickTE & ini) runnedM += (speed * Time.deltaTime)/10;
		if (!(Mathf.Round (transform.position.z) % 50 == 0))
			noaug = true;
		if (Mathf.Round (runnedM) % 50 == 0 & noaug) {
			if (speed < maxSpeed) speed += 0.5f;
			noaug = false;
		}
		if (GohanReturnBool)
			gohanReturn ();
		if (GohanMoveBool)
			gohanMove ();
		if (quickEnd)
			qTEnd ();
	//	if (rotBool)
	//		CameraRot (rotAux);
	//	if (SlideCamBoolG)
	//		CameraSlideGo ();
	//	if (SlideCamBoolR)
	//		CameraSlideReturn ();
	//		Debug.Log("sangra mala puta " +bloodBool);
		if (bloodBool)
			bloodSplat ();
		if (gateE)
			gateEvent ();
	//	if (shakeBool) CameraShake();
	}
}
