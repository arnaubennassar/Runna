using UnityEngine;
using System.Collections;

public class wayGenL2 : MonoBehaviour {
	public GameObject cami;
	public int nTrosos;
	private GameObject[] fragment;
	private int act;
	private bool kill = false;
	private int killPos = 0;
	private float counter;
	public float delay;
	private bool start = false;
	// Use this for initialization
	public void Awake () {
		act = 0;
		fragment = new GameObject[nTrosos];
		for(int i = 0; i < nTrosos; ++i){
			fragment[i] = Instantiate(cami, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		}
		fragment[0].GetComponent<iceWGenerator> ().addFragment(fragment[0].GetComponent<iceWGenerator> ().createFirstSeg(), 0.0f, 1, 0);
		for (int i = 1; i < nTrosos; ++i) {
			if(i == nTrosos -1){ 
				Debug.Log("is menys 1");
				fragment[i].GetComponent<iceWGenerator> ().isLast = true;
			}
			fragment[i].GetComponent<iceWGenerator> ().addFragment(fragment[i- 1].GetComponent<iceWGenerator> ().getLastSeg(),0.5f, 1, (int)Mathf.Min (i,2));
		}
		start = true;
	//	rot = way.GetComponent<iceWGenerator> ().nouse(transform.position.z);
	}

	public bool getStart() {
		return start;
	}

	public Quaternion nouse(float z){
		if (fragment [act].GetComponent<iceWGenerator> ().isOver ()) {
			kill = true;
			killPos = act;
			Debug.Log ("fragment acabat " +act);
			act = (act+1)%nTrosos;
			counter = delay;
			Debug.Log ("fragment actual "+ act);
		}
		if (kill) {
			if (counter < 0){
				Debug.Log ("fragment "+ killPos+ " esborrat");
				kill = false;
				fragment [killPos].GetComponent<iceWGenerator> ().destroySons ();
				Destroy(fragment [killPos]);
				fragment[killPos] = Instantiate(cami, new Vector3(0,0,0), Quaternion.identity) as GameObject;
				fragment[killPos].GetComponent<iceWGenerator> ().isLast = true;
				fragment[killPos].GetComponent<iceWGenerator> ().addFragment(fragment[(killPos + nTrosos-1)%nTrosos].GetComponent<iceWGenerator> ().getLastSeg(), 0.8f, 1f, 2);
				Debug.Log ("fragment "+ killPos+ "  redibuixat");

			}
			else counter -= Time.deltaTime;
		}
		return fragment[act].GetComponent<iceWGenerator> ().nouse(z);
	}
	
	public bool isFalling(float y){
		return fragment[act].GetComponent<iceWGenerator> ().isFalling(y);
	}

	public Vector3 inici (){
		return fragment[act].GetComponent<iceWGenerator> ().inici ();
	}
	// Update is called once per frame
	void Update () {

	}
}
