using UnityEngine;
using System.Collections;


public class GameRunning : MonoBehaviour {

	private readonly static int RED = 0;
	private readonly static int GREEN = 1;
	private readonly static int BLUE = 2;
	private readonly static int LAMP = 0;
	private readonly static int SWORD = 1;
	private readonly static int CHAIR = 2;

	private int gameState = 0;

	private int w, a, d, up, left, right;
	private int pill, action;
	private int choice1, choice2;
	private int barValue = 3;

	public float animSpeed = 0.5F;
	private float timeStamp = 0.0F;
	
	void Start() {
		SetKeysEasy ();
	}

	void Update() {
		if (Input.GetButtonDown ("W") && gameState == 0) {
			Debug.Log ("Press W Key");
			choice1 = w;
			hidePillmanIdle();
			showPillmanPills(w);
			gameState = 1;
		} else if (Input.GetButtonDown ("A") && gameState == 0) {
			Debug.Log ("Press A Key");
			choice1 = a;
			hidePillmanIdle();
			showPillmanPills(a);
			gameState = 1;
		} else if (Input.GetButtonDown ("D") && gameState == 0) {
			Debug.Log ("Press D Key");
			choice1 = d;
			hidePillmanIdle();
			showPillmanPills(d);
			gameState = 1;
		} else if (Input.GetButtonDown ("Up") && gameState == 1) {
			Debug.Log ("Press Up Key");
			choice2 = up;
			gameState = 2;
			CheckWin();
		} else if (Input.GetButtonDown ("Left") && gameState == 1) {
			Debug.Log ("Press Left Key");
			choice2 = left;
			gameState = 2;
			CheckWin();
		} else if (Input.GetButtonDown ("Right") && gameState == 1) {
			Debug.Log ("Press Right Key");
			choice2 = right;
			gameState = 2;
			CheckWin();
		}
		if (gameState == 0 && Time.time - timeStamp > animSpeed) {
			timeStamp = Time.time;
			TogglePillmanIdle ();
		}
			
	}

	private void SetKeys() {
		w = Random.Range (0, 3);
		int temp = Random.Range (0, 2);
		a = (w + temp) % 3;
		d = (w - temp) % 3;
		up = Random.Range (0, 3);
		temp = Random.Range (0, 2);
		left = (up + temp) % 3;
		right = (up - temp) % 3;
	}

	private void SetKeysEasy() {
		w = RED; a = GREEN; d = BLUE;
		up = LAMP; left = SWORD; right = CHAIR;
	}

	private void CheckWin() {

	}

	// Usage: AdjustBar(1) or AdjustBar(-1)
	private void AdjustBar(int addValue) {
		if (barValue + addValue < 0 || barValue + addValue > 6)
			return;
		Vector3 v = GameObject.Find ("bar" + barValue).transform.localPosition;
		v.z = 0.0F;
		GameObject.Find ("bar" + barValue).transform.localPosition = v;
		barValue += addValue;
		v = GameObject.Find ("bar" + barValue).transform.localPosition;
		v.z = -1.0F;
		GameObject.Find ("bar" + barValue).transform.localPosition = v;

	}

	private void TogglePillmanIdle() {
		Vector3 v1 = GameObject.Find ("pillman idle 1").transform.localPosition;
		Vector3 v2 = GameObject.Find ("pillman idle 2").transform.localPosition;
		GameObject.Find ("pillman idle 1").transform.localPosition = v2;
		GameObject.Find ("pillman idle 2").transform.localPosition = v1;
	}
	private void showPillmanIdle() {
		Vector3 v = GameObject.Find ("pillman idle 1").transform.localPosition;
		v.z = 0.0F;
		GameObject.Find ("pillman idle 1").transform.localPosition = v;
	}
	private void hidePillmanIdle() {
		Vector3 v = GameObject.Find ("pillman idle 1").transform.localPosition;
		v.z = -15.0F;
		GameObject.Find ("pillman idle 1").transform.localPosition = v;
//		v = GameObject.Find ("pillman idle 2").transform.localPosition;
//		v.z = -15.0F;
		GameObject.Find ("pillman idle 2").transform.localPosition = v;
	}

	private void showPillmanPills(int color) {
		string name;
		if (color == BLUE)
			name = "pillman blue";
		else if (color == GREEN)
			name = "pillman green";
		else if (color == RED)
			name = "pillman red";
		else
			return;
		Vector3 v = GameObject.Find(name).transform.localPosition;
		v.z = 0.0F;
		GameObject.Find(name).transform.localPosition = v;
	}
	private void hidePillmanPills() {
		Vector3 v = GameObject.Find("pillman blue").transform.localPosition;
		v.z = -15.0F;
		GameObject.Find("pillman blue").transform.localPosition = v;
		GameObject.Find("pillman green").transform.localPosition = v;
		GameObject.Find("pillman red").transform.localPosition = v;
	}


//	public int sceneToStart = 1;										//Index number in build settings of scene to load if changeScenes is true
//	public bool changeScenes = true;									//If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
//	public bool changeMusicOnStart = true;								//Choose whether to continue playing menu music or start a new music clip
//	public int musicToChangeTo = 0;										//Array index in array MusicClips to change to if changeMusicOnStart is true.
//
//
//	[HideInInspector] public bool inMainMenu = true;					//If true, pause button disabled in main menu (Cancel in input manager, default escape key)
//	[HideInInspector] public Animator animColorFade; 					//Reference to animator which will fade to and from black when starting game.
//	[HideInInspector] public Animator animMenuAlpha;					//Reference to animator that will fade out alpha of MenuPanel canvas group
//	[HideInInspector] public AnimationClip fadeColorAnimationClip;		//Animation clip fading to color (black default) when changing scenes
//	[HideInInspector] public AnimationClip fadeAlphaAnimationClip;		//Animation clip fading out UI elements alpha
//
//
//	private PlayMusic playMusic;										//Reference to PlayMusic script
//	private float fastFadeIn = .01f;									//Very short fade time (10 milliseconds) to start playing music immediately without a click/glitch
//	private ShowPanels showPanels;										//Reference to ShowPanels script on UI GameObject, to show and hide panels
//
//	
//	void Awake()
//	{
//		//Get a reference to ShowPanels attached to UI object
//		showPanels = GetComponent<ShowPanels> ();
//
//		//Get a reference to PlayMusic attached to UI object
//		playMusic = GetComponent<PlayMusic> ();
//	}
//
//
//	public void StartButtonClicked()
//	{
//		//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
//		//To change fade time, change length of animation "FadeToColor"
//		if (changeMusicOnStart) 
//		{
//			playMusic.FadeDown(fadeColorAnimationClip.length);
//			Invoke ("PlayNewMusic", fadeAlphaAnimationClip.length);
//		}
//
//		//If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
//		if (changeScenes) 
//		{
//			//Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
//			Invoke ("LoadDelayed", fadeColorAnimationClip.length * .5f);
//
//			//Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
//			animColorFade.SetTrigger ("fade");
//		} 
//
//		//If changeScenes is false, call StartGameInScene
//		else 
//		{
//			//Call the StartGameInScene function to start game without loading a new scene.
//			StartGameInScene();
//		}
//
//	}
//
//
//	public void LoadDelayed()
//	{
//		//Pause button now works if escape is pressed since we are no longer in Main menu.
//		inMainMenu = false;
//
//		//Hide the main menu UI element
//		showPanels.HideMenu ();
//
//		//Load the selected scene, by scene index number in build settings
//		Application.LoadLevel (sceneToStart);
//	}
//
//
//	public void StartGameInScene()
//	{
//		//Pause button now works if escape is pressed since we are no longer in Main menu.
//		inMainMenu = false;
//
//		//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
//		//To change fade time, change length of animation "FadeToColor"
//		if (changeMusicOnStart) 
//		{
//			//Wait until game has started, then play new music
//			Invoke ("PlayNewMusic", fadeAlphaAnimationClip.length);
//		}
//		//Set trigger for animator to start animation fading out Menu UI
//		animMenuAlpha.SetTrigger ("fade");
//
//		//Wait until game has started, then hide the main menu
//		Invoke("HideDelayed", fadeAlphaAnimationClip.length);
//
//		Debug.Log ("Game started in same scene! Put your game starting stuff here.");
//
//
//	}
//
//
//	public void PlayNewMusic()
//	{
//		//Fade up music nearly instantly without a click 
//		playMusic.FadeUp (fastFadeIn);
//		//Play music clip assigned to mainMusic in PlayMusic script
//		playMusic.PlaySelectedMusic (musicToChangeTo);
//	}
//
//	public void HideDelayed()
//	{
//		//Hide the main menu UI element
//		showPanels.HideMenu();
//	}
}
