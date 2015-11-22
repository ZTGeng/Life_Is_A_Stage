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
	private bool isWin = false;
	private float seconds = 3.0F;
	private float countDownStamp = 0.0F;
	private int currentSec = 3;
	private int actionFrame = 1;
	
	private int w, a, d, up, left, right;
	private int pill, action;
	private int choice1, choice2;
	private int barValue = 3;
	
	public float animSpeed = 0.5F;
	private float timeStamp = 0.0F;
	
	void Start() {
		SetKeysEasy ();
		reset ();
	}
	
	void Update() {
		// Check key press
		if (Input.GetButtonDown ("W") && gameState == 1) {
			Debug.Log ("Press W Key");
			choice1 = w;
			hidePillmanIdle();
			showPillmanPills(w);
			gameState = 2;
		} else if (Input.GetButtonDown ("A") && gameState == 1) {
			Debug.Log ("Press A Key");
			choice1 = a;
			hidePillmanIdle();
			showPillmanPills(a);
			gameState = 2;
		} else if (Input.GetButtonDown ("D") && gameState == 1) {
			Debug.Log ("Press D Key");
			choice1 = d;
			hidePillmanIdle();
			showPillmanPills(d);
			gameState = 2;
		} else if (Input.GetButtonDown ("Up") && gameState == 2) {
			Debug.Log ("Press Up Key");
			choice2 = up;
			CheckWin();
			hidePillmanPills();
			gameState = 3;
		} else if (Input.GetButtonDown ("Left") && gameState == 2) {
			Debug.Log ("Press Left Key");
			choice2 = left;
			CheckWin();
			hidePillmanPills();
			gameState = 3;
		} else if (Input.GetButtonDown ("Right") && gameState == 2) {
			Debug.Log ("Press Right Key");
			choice2 = right;
			CheckWin();
			hidePillmanPills();
			gameState = 3;
		}
		
		// First count down
		if (gameState == 0) {
			int secondNum = (int) Mathf.Ceil(seconds - Time.time + countDownStamp);
			if (secondNum > 0) {
				// Update cound down on screen
				if (secondNum < currentSec) {
					Debug.Log("show " + secondNum);
					showCountDown(secondNum);
					currentSec = secondNum;
				}

			} else {
				Debug.Log("First count down end");
				gameState = 1;
				showCountDown(3);
				currentSec = 3;
				countDownStamp = Time.time;
			}
		}
		
		// Second count down
		if (gameState == 1 || gameState == 2) {
			int secondNum = (int) Mathf.Ceil(seconds - Time.time + countDownStamp);
			if (secondNum > 0) {
				// Update cound down on screen
				if (secondNum < currentSec) {
					Debug.Log("show " + secondNum);
					showCountDown(secondNum);
					currentSec = secondNum;
				}
			} else {
				CheckWin();
				hidePillmanIdle();
				hidePillmanPills();
				gameState = 3;
				hideThing ("timer countdown1");
				showThing ("timer end");
				countDownStamp = Time.time;
			}
		}
		
		// Idle Animation
		if (gameState < 2 && Time.time - timeStamp > animSpeed) {
			timeStamp = Time.time;
			TogglePillmanIdle ();
		}
		
		// Action Animation
		if (gameState == 3) {
			if (isWin) {
				if (Time.time - timeStamp > animSpeed) {
					timeStamp = Time.time;
					TogglePillmanAction (choice2);
					ToggleAudience();
					// check when to go to state 4
				}
			} else {
				gameState = 4;
			}
		}
		
	}
	
	private void reset() {
		isWin = false;
		pill = Random.Range (0, 3);
		action = Random.Range (0, 3);
		choice1 = -1;
		choice2 = -1;
		currentSec = 3;
		showCountDown (3);
		actionFrame = 1;
		countDownStamp = Time.time;
		timeStamp = Time.time;
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
		if (choice1 == pill && choice2 == action) {
			// win
			Debug.Log("Win this round");
			isWin = true;
		} else {
			// lose
			Debug.Log("Lose this round");
			isWin = false;
		}
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

	private void TogglePillmanAction(int action) {
	
	}
	
	private void showPillmanAction(int action) {
		Vector3 v = GameObject.Find(name).transform.localPosition;
		v.z = 0.0F;
		GameObject.Find(name).transform.localPosition = v;
	}
	private void showPillmanLamp(int action) {
		timeStamp = Time.time;
		
	}

	private void ToggleAudience() {

	}

	private void showCountDown(int sec) {
		hideThing ("timer countdown" + currentSec);
		showThing ("timer countdown" + sec);
	}






	private void showThing(string name) {
		Vector3 v = GameObject.Find(name).transform.localPosition;
		v.z = 0.0F;
		GameObject.Find(name).transform.localPosition = v;
	}
	private void hideThing(string name) {
		Vector3 v = GameObject.Find(name).transform.localPosition;
		v.z = -15.0F;
		GameObject.Find(name).transform.localPosition = v;
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
