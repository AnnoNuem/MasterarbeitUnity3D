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
	private const double radiantIntro = Math.PI * degreeIntro / 180.0;
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
		introPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantIntro),0.0001f, (float)Math.Sin(radiantIntro)),
			new Vector3(-(float)Math.Cos(radiantIntro),0.0001f, -(float)Math.Sin(radiantIntro)),
			new Vector3((float)Math.Sin(radiantIntro),0.0001f, (float)Math.Cos(radiantIntro)),
			new Vector3(-(float)Math.Sin(radiantIntro),0.0001f, -(float)Math.Cos(radiantIntro))};
		trainingPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantTraining),0.0001f, (float)Math.Sin(radiantTraining)),
			new Vector3(-(float)Math.Cos(radiantTraining),0.0001f, -(float)Math.Sin(radiantTraining)),
			new Vector3((float)Math.Sin(radiantTraining),0.0001f, (float)Math.Cos(radiantTraining)),
			new Vector3(-(float)Math.Sin(radiantTraining),0.0001f, -(float)Math.Cos(radiantTraining))};
		testingPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantTesting),0.0001f, (float)Math.Sin(radiantTesting)),
			new Vector3(-(float)Math.Cos(radiantTesting),0.0001f, -(float)Math.Sin(radiantTesting)),
			new Vector3((float)Math.Sin(radiantTesting),0.0001f, (float)Math.Cos(radiantTesting)),
			new Vector3(-(float)Math.Sin(radiantTesting),0.0001f, -(float)Math.Cos(radiantTesting))};
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
