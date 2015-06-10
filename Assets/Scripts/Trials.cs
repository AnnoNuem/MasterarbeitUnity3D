using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class Trials
{

	public const int degreeIntro = 0;
	public const int degreeTesting1 = 45;
	public const int degreeTraining = 45;
	public trial currentTrial;
	static public Logger logger;

	
	public enum typeOfTrial{
		INTRO,
		TRAINING,
		TESTING,
		END,
	}
	
	public struct trial{
		public typeOfTrial type;
		public Vector3 position;
	}
	
	private Queue trialQueue;
	private Vector3[] introPositions;
	private Vector3[] trainingPositions;
	private Vector3[] testingPositions;
	private const double radiantIntro = 0;//Math.PI * degreeIntro / 180.0;
	private const double radiantTraining = Math.PI * degreeTraining / 180.0;
	private const double radiantTesting = Math.PI * degreeTesting1 / 180.0;


	// singleton
	private static readonly Trials instance = new Trials();
	
	// Explicit static constructor to tell C# compiler
	// not to mark type as beforefieldinit
	static Trials()
	{
	}
	
	private Trials()
	{
		trialQueue = new Queue();	
		logger = Logger.Instance;
		introPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantIntro)*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantIntro)*Parameters.goal_scale),
			new Vector3((float)Math.Cos(radiantIntro+Math.PI/2 )*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantIntro+Math.PI/2)*Parameters.goal_scale),
			new Vector3((float)Math.Cos(radiantIntro+Math.PI)*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantIntro+Math.PI)*Parameters.goal_scale),
			new Vector3((float)Math.Cos(radiantIntro+3*Math.PI/2)*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantIntro+3*Math.PI/2)*Parameters.goal_scale)};
		trainingPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantTraining)*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantTraining)*Parameters.goal_scale),
			new Vector3((float)Math.Cos(radiantTraining+Math.PI/2 )*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantTraining+Math.PI/2)*Parameters.goal_scale),
			new Vector3((float)Math.Cos(radiantTraining+Math.PI)*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantTraining+Math.PI)*Parameters.goal_scale),
			new Vector3((float)Math.Cos(radiantTraining+3*Math.PI/2)*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantTraining+3*Math.PI/2)*Parameters.goal_scale)};
		testingPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantTesting)*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantTesting)*Parameters.goal_scale),
			new Vector3((float)Math.Cos(radiantTesting+Math.PI/2 )*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantTesting+Math.PI/2)*Parameters.goal_scale),
			new Vector3((float)Math.Cos(radiantTesting+Math.PI)*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantTesting+Math.PI)*Parameters.goal_scale),
			new Vector3((float)Math.Cos(radiantTesting+3*Math.PI/2)*Parameters.goal_scale,Parameters.goal_height, (float)Math.Sin(radiantTesting+3*Math.PI/2)*Parameters.goal_scale)};

		foreach (Vector3 v in introPositions){
			Debug.Log (v);
		}
		foreach (Vector3 v in trainingPositions){
			Debug.Log (v);
		}
		foreach (Vector3 v in testingPositions){
			Debug.Log (v);
		}
	}
	
	public static Trials Instance
	{
		get
		{
			return instance;
		}
	}



	public void CreateTrials()
	{
		//intro trials
		for (int i = 0; i < Parameters.numberOfIntroTrials; i++)
		{
			trial t;
			t.type = typeOfTrial.INTRO;
			t.position = introPositions[UnityEngine.Random.Range(0, introPositions.Length)];
			trialQueue.Enqueue(t);
		}

		//training trials
		for (int i = 0; i < Parameters.numberOfTrainingTrials; i++)
		{
			trial t;
			t.type = typeOfTrial.TRAINING;
			t.position = trainingPositions[UnityEngine.Random.Range(0, trainingPositions.Length)];
			trialQueue.Enqueue(t);
		}

		// testing trials
		for (int i = 0; i < Parameters.numberOfTestingTrials; i++)
		{
			trial t;
			t.type = typeOfTrial.TESTING;
			t.position = testingPositions[UnityEngine.Random.Range(0, testingPositions.Length)];
			trialQueue.Enqueue(t);
		}

		//trial indicating the end of the experiment
		trial tEnd;
		tEnd.type = typeOfTrial.END;
		tEnd.position = Vector3.zero;
		trialQueue.Enqueue(tEnd);
		logger.Write(System.DateTime.Now + " Trial List Created\nNumber of IntroTrials: " + Parameters.numberOfIntroTrials +
		             "\nNumber of TrainingTrials: " + Parameters.numberOfTrainingTrials + "\nNumber of Testing Trials: " +
		             Parameters.numberOfTestingTrials + "\n");
	}

	public void NextTrial()
	{
		currentTrial = (trial)trialQueue.Dequeue();
	}
}
