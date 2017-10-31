using UnityEngine;
using System.Collections;

public class fadeAnim : MonoBehaviour {
	private Material mate;
	private Color originalColor;
	private float fadeSpeed;
	private float elapsed;
	private int sentit;
	private bool goinUp;
	// Use this for initialization
	void Start () {
		sentit = 1;
		fadeSpeed = 20;
		elapsed = 0;
		mate = GetComponent<MeshRenderer> ().material;
		originalColor = mate.color;
		goinUp = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (elapsed < fadeSpeed & goinUp) {
			mate.color = Color.Lerp (originalColor, Color.clear, elapsed / fadeSpeed);
			sentit = 1;
		} else {
			sentit = -1;
			mate.color = Color.Lerp (Color.clear, originalColor, elapsed / fadeSpeed);
			if (elapsed > 0.1)
				goinUp = false;
			else
				goinUp = true;
		}
		elapsed += Time.deltaTime*sentit;
	}
}
