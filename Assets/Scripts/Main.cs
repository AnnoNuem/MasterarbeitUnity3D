/**
 * ReachOut 2D Experiment
 * Axel Schaffland
 * aschaffland@uos.de
 * SS2015
 * Neuroinformatics
 * Institute of Cognitive Science
 * University of Osnabrueck
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// Main. Handles the experiment. State Machine for different states of the game.
/// </summary>
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
	public GameObject planeForCollissionWithSphere;
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


	void Start () {
		logger = Logger.Instance;
		sphereScript = sphere.GetComponent<SphereMovement>();
		trials = Trials.Instance;
		statistics = Statistics.Instance;
		// begin experiment with displaying the startscreen
		switchState(states.STARTSCREEN);
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && state == states.END)
		{
			Application.Quit();
		}	
	}

	/// <summary>
	/// Switchs the state of the experiment to "newState"
	/// </summary>
	/// <param name="newState">New state.</param>
	void switchState(states newState)
	{
		Debug.Log(newState.ToString());
		switch (newState)
		{
		case states.PAUSE:
			startscreen.enabled = false;
			goal.renderer.enabled = false;
			ground.renderer.enabled = true;
			sphereScript.SwitchState(SphereMovement.sphereStates.HIDDEN);
			Screen.showCursor = false;
			break;
		case states.INTRO:
			goal.renderer.enabled = true;
			ground.renderer.enabled = true;
			environment.SetActive(true);
			sphereScript.SwitchState(SphereMovement.sphereStates.MOVING);
			sphere.SetActive(true);
			planeForPointer.SetActive(true);
			planeForPointer2.SetActive(true);
			planeForCollissionWithSphere.SetActive(true);
			Screen.showCursor = false;
			startscreen.enabled = false;
			break;
		case states.STARTSCREEN:
			startscreen.enabled = true;
			goal.renderer.enabled = false;
			ground.renderer.enabled = false;
			environment.SetActive(false);
			sphereScript.SwitchState(SphereMovement.sphereStates.HIDDEN);
			planeForCollissionWithSphere.SetActive(false);
			Screen.showCursor = false;
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
			planeForCollissionWithSphere.SetActive(true);
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
			planeForCollissionWithSphere.SetActive(true);
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
			//if new block of traisl of other type compute statistics for previous trial block
			if (oldType != trials.currentTrial.type)
			{
				statistics.computeBlockStatistics();
				logger.Write("\n" + System.DateTime.Now + " New Block of " + trials.currentTrial.type + " trials.\n");
			}
			logger.Write(System.DateTime.Now + " New " + trials.currentTrial.type + " trial.\n");
			//set position of the goal defined in the curernt trial
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
		trials.NextTrial();
		StartCoroutine("newTrial");
	}
}
