using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class challanges : MonoBehaviour {

	public bool isOut = false;
	bool enabled = false;
	private float elapsed, duration;
	bool first = true;
	bool isVr = false;
	private int sn;
	public Transform target;
	private struct challange{
		public string text1, text2;
		public int mult;
	}
		
	private challange[] challangeList = new challange[] {
		new challange() {text1 = "Run ", text2 = " meters", mult = 50},
		new challange() {text1 = "Run ", text2 =  " meters with no collisions", mult = 30},
		new challange() {text1 = "Score ", text2 = " points",  mult =500},
		new challange() {text1 = "Pick up ", text2 = " coins or diamonds", mult = 30},
		new challange() {text1 = "Pick up ", text2 = " powerUp", mult = 2},
		new challange() {text1 = "Run ", text2 = " meters with no coins", mult = 30},
		new challange() {text1 = "Run ", text2 = " meters with no powerUp", mult = 30}
	};
	// Use this for initialization
	void Start () {
		elapsed = 0;
		duration = 4;
		GetComponent<Canvas> ().enabled = false;
		updateChallanges ();
		transform.FindChild("challange line 1").GetComponentInChildren<Text>().text = PlayerPrefs.GetString ("current challange 1", challangeList[0].text1 + challangeList[0].mult.ToString() + challangeList[0].text2);
		transform.FindChild("challange line 2").GetComponentInChildren<Text>().text = PlayerPrefs.GetString ("current challange 2", challangeList[1].text1 + challangeList[1].mult.ToString() + challangeList[1].text2);
		transform.FindChild("challange line 3").GetComponentInChildren<Text>().text = PlayerPrefs.GetString ("current challange 3", challangeList[2].text1 + challangeList[2].mult.ToString() + challangeList[2].text2);
		int levels2up = PlayerPrefs.GetInt ("level", 1);
		levels2up = levels2up * levels2up - PlayerPrefs.GetInt ("challanges superated", 0);
		transform.FindChild("challUp_text").GetComponentInChildren<Text>().text = levels2up.ToString()+" challanges to level up ";
	}

	void updateChallanges(){
		int auxSuperated, auxLevel;
		auxSuperated = PlayerPrefs.GetInt("challanges superated",0);
		auxLevel = PlayerPrefs.GetInt("level",1);
		for(int k = 1; k < 4; ++k){
			if (PlayerPrefs.GetInt ("current challangeId "+k.ToString(), 0) == -1) {
				++auxSuperated;
				int nextId = nextChallangeId ();
				int score = (challangeList [nextId].mult * PlayerPrefs.GetInt ("ch" + nextId.ToString () + "lvl", 1));
				string chText = challangeList [nextId].text1 + score.ToString()  + challangeList [nextId].text2;
				transform.FindChild ("challange line "+k.ToString()).GetComponentInChildren<Text> ().text = "+ 1000$";
				transform.FindChild ("challange line " + k.ToString ()).GetComponent<Image> ().color = Color.green;
				PlayerPrefs.SetString ("current challange "+k.ToString(), chText);
				PlayerPrefs.SetInt ("current challangeId "+k.ToString(), nextId);
				PlayerPrefs.SetInt ("current challangeScore "+k.ToString(), score);
			}
		}
		PlayerPrefs.SetInt("challanges superated",auxSuperated);
		if (auxSuperated > auxLevel * auxLevel) PlayerPrefs.SetInt("level",auxLevel+1);
	}

	int nextChallangeId(){
		int lowestLevelId = 0;
		int lowestLevel = 100000;
		for (int i = 0; i < challangeList.Length; ++i){
			if (lowestLevel > PlayerPrefs.GetInt ("ch" + i.ToString () + "lvl", 1) & PlayerPrefs.GetInt ("ch" + i.ToString () + "sprtd", 0) == 0) {
				lowestLevelId = i;
			}
		}
		PlayerPrefs.SetInt ("ch" + lowestLevelId.ToString () + "sprtd", 2);
		return lowestLevelId;
	}
	// Update is called once per frame
	void Update () {
		if (enabled & isVr) {
			transform.position = target.position + target.forward/10;
			transform.LookAt (target);
		}
		if (enabled) {
			elapsed += Time.deltaTime;
			if (duration < elapsed) {
				if (elapsed * 3 > duration & isOut) GetComponent<Canvas> ().enabled = enabled;
				Application.LoadLevel (sn);
			}
		}
	}

	public void toTheEnd(int SceneNumber){
		GetComponent<Canvas> ().enabled = enabled;
		if (PlayerPrefs.GetInt ("isVR", 0) == 1) {
			isVr = true;
			GetComponent<Canvas> ().renderMode = RenderMode.WorldSpace;
			GetComponent<Canvas> ().transform.localScale = new Vector3 (0.0001f,0.0001f,0.0001f);
			
		}
		sn = SceneNumber;
		enabled = true;
		if (!isOut) {
			GetComponent<Canvas> ().enabled = enabled;
		}
	}
}
