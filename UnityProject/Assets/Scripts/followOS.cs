using UnityEngine;
using System.Collections;

public class followOS : MonoBehaviour {
	public Transform target;
	public Vector3 offset;
	public bool enable = true;
	float treshold = 10;
	public float percentatge = 0.5f;
	public float thepoint;
	public float multiTresh = 1;
	public bool isPlaying = false;
	private bool enableSmooth = false;
	// Use this for initialization
	void Start () {
		if (target == null) {
			if (0 == PlayerPrefs.GetInt ("is1st", 0)) {
				target = GameObject.FindWithTag ("Player").transform.GetChild (5).GetComponent<Transform> ();
				enableSmooth = true;
			}
			else
				target = GameObject.FindWithTag ("Player").transform.GetChild(6).GetComponent<Transform>();
		}
		transform.position = target.position;
		if (PlayerPrefs.GetInt ("isVR", 0) == 1 & isPlaying) {
			GetComponentInChildren <Canvas> ().renderMode = RenderMode.WorldSpace;
			GetComponentInChildren <Canvas> ().transform.localScale = new Vector3 (0.005f,0.005f,1);
			GetComponentInChildren <Canvas> ().transform.localPosition = new Vector3 (0,0,thepoint*GetComponentInChildren <Canvas> ().GetComponent<RectTransform>().sizeDelta.x);

		}
	}
	
	// Update is called once per frame
	void Update () {
	//	GetComponentInChildren <Canvas> ().transform.localPosition = new Vector3 (0,0,thepoint*GetComponentInChildren <Canvas> ().GetComponent<RectTransform>().sizeDelta.x);
		if (enable){
			if (treshold < Vector3.Distance (transform.position, target.transform.position))
				transform.position = Vector3.Lerp (transform.position, target.transform.position, percentatge);//target.transform.TransformPoint (target.localPosition + offset);
			else {
			/*	if(enableSmooth)	transform.position = Vector3.Lerp (transform.position, target.transform.position, percentatge * multiTresh);
				else */ transform.position = target.position;
			}
		}
	}
}
