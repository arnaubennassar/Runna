using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class vrMenuControl : MonoBehaviour {
	private string lastHitName;
	private float elapsed, duration, ePS, dPS;
	private int index, highscore, isVR, is1st, show;
	private string runner, weapon;
	private float xLW, yLW, zLW, xLP, yLP, zLP;
	public int money, level;
	public GameObject personLight;
	public GameObject weaponLight;
	public GameObject challanges;
	public AudioClip coinClip;
	public GameObject coinLose;

	// Use this for initialization
	void Start () {
	//	PlayerPrefs.DeleteAll ();
		elapsed = 0;
		duration = 2;
		ePS = 0;
		dPS = 2;
		index = 4;

		GetComponentInChildren<spriteAC> ().manualIndex = true;
		weapon = PlayerPrefs.GetString("current weapon", "ballesta");
		runner = PlayerPrefs.GetString("current runner","partyGirl");
		money = PlayerPrefs.GetInt("money",10) + PlayerPrefs.GetInt ("last score", 0);
		level = PlayerPrefs.GetInt("level",1);
		isVR = PlayerPrefs.GetInt("isVR",0);
		is1st = PlayerPrefs.GetInt("is1st",0); 
		show = PlayerPrefs.GetInt("show",0); 
		highscore = PlayerPrefs.GetInt("Player Score1",0);
		xLW = PlayerPrefs.GetFloat("Light Weapon x",0);
		yLW = PlayerPrefs.GetFloat("Light Weapon y",4.5f);
		zLW = PlayerPrefs.GetFloat("Light Weapon z",0);
		xLP = PlayerPrefs.GetFloat("Light Player x",0);
		yLP = PlayerPrefs.GetFloat("Light Player y",4.5f);
		zLP = PlayerPrefs.GetFloat("Light Player z",0);

		personLight.transform.position = new Vector3 (xLP, yLP, zLP);
		weaponLight.transform.position = new Vector3 (xLW, yLW, zLW);
		GameObject.Find ("score").GetComponent<Text> ().text = money.ToString () + " $ \n level "+ level.ToString();
		if (isVR == 1)
			GameObject.Find ("CardboardMain").GetComponent<Cardboard> ().VRModeEnabled = true;
		else {
			GameObject.Find ("CardboardMain").GetComponent<Cardboard> ().VRModeEnabled = false;
			Transform vrSwitchLook = GameObject.Find ("VRswitch").GetComponent<Transform> ();
			vrSwitchLook.rotation = Quaternion.Euler (vrSwitchLook.eulerAngles.x, vrSwitchLook.eulerAngles.y + 180, vrSwitchLook.eulerAngles.z);
		}
		if (is1st == 0) {
			Transform camSwitchLook = GameObject.Find ("1or3Cam").GetComponent<Transform>();
			camSwitchLook.rotation = Quaternion.Euler (camSwitchLook.eulerAngles.x, camSwitchLook.eulerAngles.y + 180, camSwitchLook.eulerAngles.z);
		}
		if (show == 0) {
			Transform camSwitchLook = GameObject.Find ("showControls").GetComponent<Transform>();
			camSwitchLook.rotation = Quaternion.Euler (camSwitchLook.eulerAngles.x, camSwitchLook.eulerAngles.y + 180, camSwitchLook.eulerAngles.z);
		}
		GameObject.FindWithTag ("pantalla").GetComponent<challanges> ().isOut = false;
		GameObject.FindWithTag ("pantalla").GetComponent<challanges> ().target = GameObject.Find ("CardboardMain").transform.GetChild(0).GetComponent<Transform> ();

	}
	
	// Update is called once per frame
	void Update () {
			if (ePS > 0)
				ePS += Time.deltaTime;
			if (ePS > dPS) {
				Destroy (FindObjectOfType<ParticleSystem> ());
				Debug.Log ("stoping PS");
			}
			RaycastHit hit;
			if (Physics.Raycast (transform.position, transform.forward, out hit, 100f)) {
				//	Debug.Log ("raycast hit to : " + hit.transform.name);
				string currentHitTag = hit.transform.tag;
				if (currentHitTag == lastHitName & currentHitTag != "buyBlocker") {
					elapsed += Time.deltaTime;
					index = 4 + (int)((elapsed / duration) * 15);
					if (index > 15)
						index -= 16;
				} else {
					index = 4;
					elapsed = 0;
				}
				if (elapsed > duration) {
					index = 3;
					elapsed = 0;


					//
					if (currentHitTag == "Player" || currentHitTag == "weapon") {
					
						int selected = hit.transform.GetComponent<characterMenu> ().selectHit ();
						if (selected != 0) {
							Debug.Log ("raycast hit to : " + hit.transform.name);
							if (selected < 0) {
								money += selected;
								PlayerPrefs.SetInt ("money", money);
								GameObject.Find ("score").GetComponent<Text> ().text = money.ToString () + " $";
								AudioSource.PlayClipAtPoint (coinClip, transform.position);
								Instantiate (coinLose, hit.transform.position, Quaternion.identity);
								coinLose.GetComponent<ParticleSystem> ().Play ();
							}
							if (currentHitTag == "Player") {
							
								runner = hit.transform.GetComponent<characterMenu> ().characterName;
								PlayerPrefs.SetString ("current runner", runner);
								xLP = hit.transform.position.x;
								yLP = 4.5f;
								zLP = hit.transform.position.z;
								PlayerPrefs.SetFloat ("Light Player x", xLP);
								PlayerPrefs.SetFloat ("Light Player y", yLP);
								PlayerPrefs.SetFloat ("Light Player z", zLP);
								personLight.transform.position = new Vector3 (xLP, yLP, zLP);
							} else {
								weapon = hit.transform.GetComponent<characterMenu> ().characterName;
								PlayerPrefs.SetString ("current weapon", weapon);
								xLW = hit.transform.position.x;
								yLW = 4.5f;
								zLW = hit.transform.position.z;
								PlayerPrefs.SetFloat ("Light Weapon x", xLW);
								PlayerPrefs.SetFloat ("Light Weapon y", yLW);
								PlayerPrefs.SetFloat ("Light Weapon z", zLW);
								weaponLight.transform.position = new Vector3 (xLW, yLW, zLW);
							}
						}
					}

					string currentHitName = hit.transform.name;
					switch (currentHitName) {
					//
					/*
				case "partygirlMenu":
					hit.transform.GetComponent<Animator> ().SetBool ("selected", true);
				//	hit.transform.position += hit.transform.forward * 1.05f;
					GameObject.Find ("GoblinMenu").GetComponent<Animator> ().SetBool ("selected", false);
					GameObject.Find ("gamoraMenu").GetComponent<Animator> ().SetBool ("selected", false);
					personLight.transform.position = new Vector3 (hit.transform.position.x, personLight.transform.position.y, hit.transform.position.z);
					elapsed = 0;
					break;
				case "GoblinMenu":
					hit.transform.GetComponent<Animator> ().SetBool ("selected", true);
				//	hit.transform.position += hit.transform.forward * 1.05f;
					GameObject.Find ("gamoraMenu").GetComponent<Animator> ().SetBool ("selected", false);
					GameObject.Find ("partygirlMenu").GetComponent<Animator> ().SetBool ("selected", false);
					personLight.transform.position = new Vector3 (hit.transform.position.x, personLight.transform.position.y, hit.transform.position.z);
					elapsed = 0;
					break;
				case "gamoraMenu":
					hit.transform.GetComponent<Animator> ().SetBool ("selected", true);
				//	hit.transform.position += hit.transform.forward * 1.05f;
					GameObject.Find ("partygirlMenu").GetComponent<Animator> ().SetBool ("selected", false);
					GameObject.Find ("GoblinMenu").GetComponent<Animator> ().SetBool ("selected", false);
					personLight.transform.position = new Vector3 (hit.transform.position.x, personLight.transform.position.y, hit.transform.position.z);
					elapsed = 0;
					break;
				case "buy":
					ePS = 0.01f;
					AudioSource.PlayClipAtPoint (coinClip, transform.position);
					GameObject.Find ("gamoraMenu").GetComponent<Animator> ().SetBool ("selected", false);
					GameObject.Find ("score").GetComponent<Text> ().text = "1280$";
					Instantiate (coinLose, hit.transform.position, Quaternion.identity);
					coinLose.GetComponent<ParticleSystem> ().Play();
					Destroy (hit.transform.gameObject);
					break;

				case "ballesta":
					weaponLight.transform.position = new Vector3 (hit.transform.position.x, personLight.transform.position.y, hit.transform.position.z);
					elapsed = 0;
					break;
				case "Final Revolver_OBJ":
					weaponLight.transform.position = new Vector3 (hit.transform.position.x, personLight.transform.position.y, hit.transform.position.z);
					elapsed = 0;
					break;
				case "Scifi_shotgun_OBJ":
					weaponLight.transform.position = new Vector3 (hit.transform.position.x, personLight.transform.position.y, hit.transform.position.z);
					elapsed = 0;
					break;
*/
				case "1or3Cam":
						if (is1st == 1) {
							PlayerPrefs.SetInt ("is1st", 0);
							is1st = 0;
						} else {
							PlayerPrefs.SetInt ("is1st", 1);
							is1st = 1;
						}
						Quaternion rotAux = Quaternion.Euler (hit.transform.rotation.eulerAngles.x, hit.transform.rotation.eulerAngles.y + 180, hit.transform.rotation.eulerAngles.z);
					//	GetComponentInParent<Cardboard> ().VRModeEnabled = !GetComponentInParent<Cardboard> ().VRModeEnabled;
						hit.transform.rotation = rotAux;
						elapsed = 0;
						break;
				case "VRswitch":
					if (isVR == 1) {
						PlayerPrefs.SetInt ("isVR", 0);
						isVR = 0;
					} else {
						PlayerPrefs.SetInt ("isVR", 1);
						isVR = 1;
					}
					Quaternion rotAux1 = Quaternion.Euler (hit.transform.rotation.eulerAngles.x, hit.transform.rotation.eulerAngles.y + 180, hit.transform.rotation.eulerAngles.z);
					GetComponentInParent<Cardboard> ().VRModeEnabled = !GetComponentInParent<Cardboard> ().VRModeEnabled;
					hit.transform.rotation = rotAux1;
					elapsed = 0;
					break;
				case "CanLogo":
					GameObject.FindWithTag ("pantalla").GetComponent<challanges> ().toTheEnd (1);
					break;
				case "showControls":
					if (show == 1) {
						PlayerPrefs.SetInt ("show", 0);
						show = 0;
					} else {
						PlayerPrefs.SetInt ("show", 1);
						show = 1;
					}
					Quaternion rotAux2 = Quaternion.Euler (hit.transform.rotation.eulerAngles.x, hit.transform.rotation.eulerAngles.y + 180, hit.transform.rotation.eulerAngles.z);
					hit.transform.rotation = rotAux2;
					elapsed = 0;
					break;
					}
				}
				lastHitName = currentHitTag;
			} else {
				index = 4;
				elapsed = 0;
			}
			GetComponentInChildren<spriteAC> ()._index = index;

	}
		
}

