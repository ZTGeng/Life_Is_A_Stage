using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


public class StartOptions : MonoBehaviour {



	public int sceneToStart = 1;										//Index number in build settings of scene to load if changeScenes is true
	public bool changeScenes = true;									//If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
	public bool changeMusicOnStart = true;								//Choose whether to continue playing menu music or start a new music clip
	public int musicToChangeTo = 0;										//Array index in array MusicClips to change to if changeMusicOnStart is true.


	[HideInInspector] public bool inMainMenu = true;					//If true, pause button disabled in main menu (Cancel in input manager, default escape key)
	[HideInInspector] public Animator animColorFade; 					//Reference to animator which will fade to and from black when starting game.
	[HideInInspector] public Animator animMenuAlpha;					//Reference to animator that will fade out alpha of MenuPanel canvas group
	[HideInInspector] public AnimationClip fadeColorAnimationClip;		//Animation clip fading to color (black default) when changing scenes
	[HideInInspector] public AnimationClip fadeAlphaAnimationClip;		//Animation clip fading out UI elements alpha


	private PlayMusic playMusic;										//Reference to PlayMusic script
	private float fastFadeIn = .01f;									//Very short fade time (10 milliseconds) to start playing music immediately without a click/glitch
	private ShowPanels showPanels;										//Reference to ShowPanels script on UI GameObject, to show and hide panels

	private int moveCurtain = 0;
	private Transform leftCurtain;
	private Transform rightCurtain;
	private Vector3 leftCurtainTarget;
	private Vector3 rightCurtainTarget;
	private readonly static float target = 8.5F;
	private readonly static float speed = 8.0F;
	
	void Awake()
	{
		//Get a reference to ShowPanels attached to UI object
		showPanels = GetComponent<ShowPanels> ();

		//Get a reference to PlayMusic attached to UI object
		playMusic = GetComponent<PlayMusic> ();

		leftCurtain  = GameObject.Find("left curtain").transform;
		rightCurtain = GameObject.Find("right curtain").transform;
		leftCurtainTarget  = leftCurtain.transform.position;
		rightCurtainTarget = rightCurtain.transform.position;
		leftCurtainTarget.x  = -target;
		rightCurtainTarget.x = target;
	}

	void Update() {
		if (moveCurtain == 1) {
			leftCurtain.position  = Vector3.MoveTowards(leftCurtain.position,  leftCurtainTarget,  speed * Time.deltaTime);
			rightCurtain.position = Vector3.MoveTowards(rightCurtain.position, rightCurtainTarget, speed * Time.deltaTime);
			if (rightCurtain.position.x == target) {
				moveCurtain = 0;
				GameStart();
			}
		}
	}


	public void StartButtonClicked()
	{
		moveCurtain = 1;
	}

	private void GameStart() {

		//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
		//To change fade time, change length of animation "FadeToColor"
		if (changeMusicOnStart) 
		{
			playMusic.FadeDown(fadeColorAnimationClip.length);
			Invoke ("PlayNewMusic", fadeAlphaAnimationClip.length);
		}

		//If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
		if (changeScenes) 
		{
			//Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
			Invoke ("LoadDelayed", fadeColorAnimationClip.length * .5f);

			//Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
			animColorFade.SetTrigger ("fade");
		} 

		//If changeScenes is false, call StartGameInScene
		else 
		{
			//Call the StartGameInScene function to start game without loading a new scene.
			StartGameInScene();
		}

	}


	public void LoadDelayed()
	{
		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;

		//Hide the main menu UI element
		showPanels.HideMenu ();

		//Load the selected scene, by scene index number in build settings
		Application.LoadLevel (sceneToStart);
	}


	public void StartGameInScene()
	{
		//Pause button now works if escape is pressed since we are no longer in Main menu.
		inMainMenu = false;

		//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
		//To change fade time, change length of animation "FadeToColor"
		if (changeMusicOnStart) 
		{
			//Wait until game has started, then play new music
			Invoke ("PlayNewMusic", fadeAlphaAnimationClip.length);
		}
		//Set trigger for animator to start animation fading out Menu UI
		animMenuAlpha.SetTrigger ("fade");

		//Wait until game has started, then hide the main menu
		Invoke("HideDelayed", fadeAlphaAnimationClip.length);

		Debug.Log ("Game started in same scene! Put your game starting stuff here.");


	}


	public void PlayNewMusic()
	{
		//Fade up music nearly instantly without a click 
		playMusic.FadeUp (fastFadeIn);
		//Play music clip assigned to mainMusic in PlayMusic script
		playMusic.PlaySelectedMusic (musicToChangeTo);
	}

	public void HideDelayed()
	{
		//Hide the main menu UI element
		showPanels.HideMenu();
	}
}
