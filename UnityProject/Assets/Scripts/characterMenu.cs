using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class characterMenu : MonoBehaviour {

	public string characterName;
	public int cost, unlockLevel;
	public GameObject buy, blocked;
	public bool isWeapon = false;

	public bool unblocked = false;
	public int owned = 0;
	// Use this for initialization
	void Start () {
	//	transform.LookAt(GameObject.Find("CardboardMain").GetComponent<Transform>().position);
		owned = PlayerPrefs.GetInt (characterName + " Owned", 0);
		if ( PlayerPrefs.GetInt ("level", 1) >= unlockLevel)
			unblocked = true;
		if(!unblocked){
			GameObject aux = Instantiate (blocked, Vector3.Lerp(transform.position, GameObject.Find("CardboardMain").transform.position, 0.2f), Quaternion.identity) as GameObject;
			if (!isWeapon)
				aux.transform.position += new Vector3 (0,1,0);
			aux.transform.LookAt(GameObject.Find("CardboardMain").GetComponent<Transform>().position);
		}
		else if (owned != 1){
			Debug.Log ("buy me :" + characterName);
			buy = Instantiate (buy, Vector3.Lerp(transform.position, GameObject.Find("CardboardMain").transform.position, 0.2f), Quaternion.identity) as GameObject;
			if (!isWeapon)
				buy.transform.position += new Vector3 (0,1,0);
			buy.transform.GetChild(0).GetComponent<Text>().text = cost.ToString() + " $";
			buy.transform.LookAt(GameObject.Find("CardboardMain").GetComponent<Transform>().position);
		}
	}

	// Update is called once per frame


	public int selectHit(){ 
		Debug.Log (unblocked + " " + owned);
		if (unblocked & owned == 0 & GameObject.FindGameObjectWithTag("troll").GetComponent<vrMenuControl>().money >= cost) {
			owned = 1;
			PlayerPrefs.SetInt (characterName + " Owned", 1);
			if(!isWeapon)GetComponent<Animator> ().SetBool ("selected", false);
			if(!isWeapon)GameObject.Find ("gamoraMenu").GetComponent<Animator> ().SetBool ("selected", false);
			Destroy (buy);
			return -cost;
		}
		else if (owned == 0 || !unblocked)
			return 0;
		return 1;
	}
}
