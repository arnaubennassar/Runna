using UnityEngine;
using System.Collections;

public class wayGen : MonoBehaviour {
	public GameObject cami;
	public int nTrosos;
	private GameObject[] fragment;
	private int act;
	private bool kill = false;
	private int killPos = 0;
	private float counter;
	public float delay;
	private bool start = false;
	//private Vector3[] finalObsPos;

	public GameObject blackHole;
	int level = 1;
	int bHIndex1 = 0;
	int bHindex2 = 2;
	public float bHoleFollowSpeed = 1;
	public float marge = 10;
	Vector3 bHTarget = Vector3.zero;
	// Use this for initialization
	public void Awake () {
		act = 0;
		fragment = new GameObject[nTrosos];
		for(int i = 0; i < nTrosos; ++i){
			fragment[i] = Instantiate(cami, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		}
	//	Debug.Log("fragment 0");
		fragment[0].GetComponent<snakeWGenerator> ().addFragment(fragment[0].GetComponent<snakeWGenerator> ().createFirstSeg(), 0.6f, 1, 1);
		for (int i = 1; i < nTrosos; ++i) {
	//		Debug.Log("fragment "+i);
			if(i == nTrosos -1){ 
			//	Debug.Log("is menys 1");
				fragment[i].GetComponent<snakeWGenerator> ().isLast = true;
			}
			++level;
			fragment[i].GetComponent<snakeWGenerator> ().addFragment(fragment[i- 1].GetComponent<snakeWGenerator> ().getLastSeg(),0.8f, 1, level);
		}
		start = true;
		//	rot = way.GetComponent<snakeWGenerator> ().nouse(transform.position.z);
		blackHole = Instantiate(blackHole, new Vector3(0,0,0), Quaternion.identity) as GameObject;
	}
	
	public bool getStart() {
		return start;
	}
	
	public void nouse(){
			kill = true;
			killPos = act;
			act = (act+1)%nTrosos;
			counter = delay;
			GameObject.FindWithTag ("Player").GetComponent<playerMovements> ().gateEvent ();
	}
	
	public bool isFalling(float y){
		return fragment[act].GetComponent<snakeWGenerator> ().isFalling(y);
	}

	public bool isUp(){
		return fragment[act].GetComponent<snakeWGenerator> ().isUp();
	}
	
	public Vector3 inici (){
		return fragment[act].GetComponent<snakeWGenerator> ().inici ();
	}
	public Vector3 posIniTroll(){
		return fragment [act].GetComponent<snakeWGenerator> ().posIniTroll ();
	}
	// Update is called once per frame
	void Update () {
		if (kill) {
			if (counter < 0){
				kill = false;
				fragment [killPos].GetComponent<snakeWGenerator> ().destroySons ();
				Destroy(fragment [killPos]);
				fragment[killPos] = Instantiate(cami, new Vector3(0,0,0), Quaternion.identity) as GameObject;
				fragment[killPos].GetComponent<snakeWGenerator> ().isLast = true;
				++level;
				fragment[killPos].GetComponent<snakeWGenerator> ().addFragment(fragment[(killPos + nTrosos-1)%nTrosos].GetComponent<snakeWGenerator> ().getLastSeg(), 0.8f, 1f, level);
			}
			else counter -= Time.deltaTime;
		}
		if (Vector3.zero == bHTarget || Vector3.Distance (bHTarget, blackHole.transform.position) < 0.1f) {
			bHTarget = fragment [bHIndex1].GetComponent<snakeWGenerator> ().getPosition (bHindex2);
			++bHindex2;
			if (bHTarget == Vector3.zero) {
				bHindex2 = 1;
				bHIndex1 = (bHIndex1 + 1) % nTrosos;
				bHTarget = fragment [bHIndex1].GetComponent<snakeWGenerator> ().getPosition (0);
			}
		}
		blackHole.transform.position = Vector3.Lerp(blackHole.transform.position, bHTarget, Time.deltaTime/(Vector3.Distance(blackHole.transform.position, bHTarget)/bHoleFollowSpeed));
	}
}
