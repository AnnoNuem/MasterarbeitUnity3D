using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public GameObject sphere;
	public GameObject helper;
	public GameObject goal;
	public GameObject ground;
	public GameObject planeForPointer;
	public GameObject planeForPointer2;
	public Camera mainCamera;
	public GameObject oculusCamera;
	public GameObject environment;
	public Canvas startscreen;
	public states state;
	static SphereMovement sphereScript;
	static Logger logger;
	static Trials trials;
	static Statistics statistics;


	public enum states
	{
		STARTSCREEN,
		INTRO,
		TRAINING,
		TESTING,
		PAUSE,
		END
	}

	// Use this for initialization
	void Start () {
		logger = Logger.Instance;
		sphereScript = sphere.GetComponent<SphereMovement>();
		trials = Trials.Instance;
		statistics = Statistics.Instance;
		switchState(states.STARTSCREEN);
		//DEBUG ONLY TODO 
		startExperimentPressed();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && state == states.END)
		{
			Application.Quit();
		}	
	}

	void switchState(states newState)
	{
		switch (newState)
		{
		case states.PAUSE:
			startscreen.enabled = false;
			goal.renderer.enabled = false;
			ground.renderer.enabled = true;
			sphereScript.SwitchState(SphereMovement.sphereStates.HIDDEN);
				sphere.SetActive(false);
			planeForPointer.SetActive(false);
			planeForPointer2.SetActive(false);
			Screen.showCursor = false;
			break;
		case states.INTRO:
			startscreen.enabled = false;
			goal.renderer.enabled = true;
			ground.renderer.enabled = true;
			environment.SetActive(true);
			sphereScript.SwitchState(SphereMovement.sphereStates.MOVING);
			sphere.SetActive(true);
			planeForPointer.SetActive(true);
			planeForPointer2.SetActive(true);
			Screen.showCursor = false;
			break;
		case states.STARTSCREEN:
			Debug.Log("Startscreen");
			startscreen.enabled = true;
			goal.renderer.enabled = false;
			ground.renderer.enabled = false;
			environment.SetActive(false);
			sphereScript.SwitchState(SphereMovement.sphereStates.HIDDEN);
			sphere.SetActive(false);
			planeForPointer.SetActive(false);
			planeForPointer2.SetActive(false);
			Screen.showCursor = true;
			break;
		case states.TESTING:
			startscreen.enabled = false;
			goal.renderer.enabled = true;
			ground.renderer.enabled = true;
			environment.SetActive(true);
			sphereScript.SwitchState(SphereMovement.sphereStates.MOVING);
			sphere.SetActive(true);
			planeForPointer.SetActive(true);
			planeForPointer2.SetActive(true);
			Screen.showCursor = false;
			break;
		case states.TRAINING:
			startscreen.enabled = false;
			goal.renderer.enabled = true;
			ground.renderer.enabled = true;
			environment.SetActive(true);
			sphereScript.SwitchState(SphereMovement.sphereStates.MOVING);
			sphere.SetActive(true);
			planeForPointer.SetActive(true);
			planeForPointer2.SetActive(true);
			Screen.showCursor = false;
			break;
		case states.END:
			startscreen.enabled = false;
			goal.renderer.enabled = false;
			ground.renderer.enabled = false;
			sphereScript.SwitchState(SphereMovement.sphereStates.HIDDEN);
			logger.CloseLogFile();
			Screen.showCursor = false;
			break;
		}
		state = newState;
	}	

	public IEnumerator newTrial()
	{
		Trials.typeOfTrial oldType = trials.currentTrial.type;
		switchState(states.PAUSE);
		yield return new WaitForSeconds(Parameters.pauseBetweenTrials);
		trials.NextTrial();
		if (trials.currentTrial.type != Trials.typeOfTrial.END)
		{
			if (oldType != trials.currentTrial.type)
			{
				statistics.computeBlockStatistics();
				logger.Write("\n" + System.DateTime.Now + " New Block of " + trials.currentTrial.type + " trials.\n");
			}
			logger.Write(System.DateTime.Now + " New " + trials.currentTrial.type + " trial.\n");  
			goal.transform.position = trials.currentTrial.position;
		}
		switch (trials.currentTrial.type){
		case Trials.typeOfTrial.INTRO:
			switchState(states.INTRO);
			break;
		case Trials.typeOfTrial.TESTING:
			switchState(states.TESTING);
			break;
		case Trials.typeOfTrial.TRAINING:
			switchState(states.TRAINING);
			break;
		case Trials.typeOfTrial.END:
			statistics.computeBlockStatistics();
			logger.Write("\n" + System.DateTime.Now + " Experimend ended");
			switchState(states.END);
			break;
		}
	}

	public void startExperimentPressed()
	{
		logger.CreateLogFile();
		trials.CreateTrials();
		logger.Write("\n" + System.DateTime.Now + " New Block of " + trials.currentTrial.type + " trials.\n");
		logger.Write(System.DateTime.Now + " New " + trials.currentTrial.type + " trial.\n");  
		switchState (states.INTRO);
	}
}
