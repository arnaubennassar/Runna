using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class fadeScript : MonoBehaviour
{
	public Image FadeImg;
	public float fadeSpeed = 30f;
	public bool sceneStarting = true;
	public bool isSceneTransition = false;
	public int nextScene = 1;
	private bool end;
	private int SceneNumber;
	private float elapsed, duration;
	
	void Awake()
	{
		end = false;
		FadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
		elapsed = 0;
		duration = 3;
	}
	
	void Update()
	{
		// If the scene is starting...
		if (sceneStarting)
			// ... call the StartScene function.
			StartScene();
		if (end)
			EndScene ();
		if (isSceneTransition) {
			elapsed += Time.deltaTime;
			if (elapsed > duration) {
				toTheEnd (nextScene);
			}
		}
	}
	
	
	void FadeToClear()
	{
		// Lerp the colour of the image between itself and transparent.
		FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	
	void FadeToBlack()
	{
		// Lerp the colour of the image between itself and black.
		FadeImg.color = Color.Lerp(FadeImg.color, Color.black, fadeSpeed/3 * Time.deltaTime);
	}
	
	
	void StartScene()
	{
		// Fade the texture to clear.
		//FadeToClear();
		FadeImg.color = Color.clear;
		// If the texture is almost clear...
		if (FadeImg.color.a <= 0.05f)
		{
			// ... set the colour to clear and disable the RawImage.
			FadeImg.color = Color.clear;
			FadeImg.enabled = false;
			
			// The scene is no longer starting.
			sceneStarting = false;
		}
	}
	public void toTheEnd(int sn) {
		end = true;
		SceneNumber = sn;
		FadeImg.enabled = true;	
	}
	
	public void EndScene()
	{
		// Start fading towards black.
		//FadeToBlack();
		FadeImg.color = Color.black;
		// If the screen is almost black...
		if (FadeImg.color.a >= 0.95) {
			end = false;
			FadeImg.color = Color.black;
			Application.LoadLevel (SceneNumber);
		}
	}
}   
