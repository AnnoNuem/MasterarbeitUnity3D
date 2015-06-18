/**
 * ReachOut 3D Experiment
 * Axel Schaffland
 * aschaffland@uos.de
 * SS2015
 * Neuroinformatics
 * Institute of Cognitive Science
 * University of Osnabrueck
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Trials. Creates Queue of trials. Handles current trials and returns new trials to main script
/// </summary>
public sealed class Trials
{
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
		public string text;
		public float displaytime;
	}
	
	private Queue trialQueue;
	private Vector3[] introPositions;
	private Vector3[] trainingPositions;
	private Vector3[] testingPositions;
	private const double radiantIntro = Math.PI * Parameters.degreeIntro / 180.0;
	private const double radiantTraining = Math.PI * Parameters.degreeTraining / 180.0;
	private const double radiantTesting = Math.PI * Parameters.degreeTesting1 / 180.0;

	// singleton variables and functions
	private static readonly Trials instance = new Trials();

	static Trials()
	{
	}
	
	private Trials()
	{
		trialQueue = new Queue();	
		logger = Logger.Instance;
		// compute postion of possible goal positions
		introPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantIntro)*Parameters.goal_distance_intro,Parameters.goal_height, (float)Math.Sin(radiantIntro)*Parameters.goal_distance_intro),
			new Vector3((float)Math.Cos(radiantIntro+Math.PI/2 )*Parameters.goal_distance_intro,Parameters.goal_height, (float)Math.Sin(radiantIntro+Math.PI/2)*Parameters.goal_distance_intro),
			new Vector3((float)Math.Cos(radiantIntro+Math.PI)*Parameters.goal_distance_intro,Parameters.goal_height, (float)Math.Sin(radiantIntro+Math.PI)*Parameters.goal_distance_intro),
			new Vector3((float)Math.Cos(radiantIntro+3*Math.PI/2)*Parameters.goal_distance_intro,Parameters.goal_height, (float)Math.Sin(radiantIntro+3*Math.PI/2)*Parameters.goal_distance_intro)};
		trainingPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantTraining)*Parameters.goal_distance_training,Parameters.goal_height, (float)Math.Sin(radiantTraining)*Parameters.goal_distance_training),
			new Vector3((float)Math.Cos(radiantTraining+Math.PI/2 )*Parameters.goal_distance_training,Parameters.goal_height, (float)Math.Sin(radiantTraining+Math.PI/2)*Parameters.goal_distance_training),
			new Vector3((float)Math.Cos(radiantTraining+Math.PI)*Parameters.goal_distance_training,Parameters.goal_height, (float)Math.Sin(radiantTraining+Math.PI)*Parameters.goal_distance_training),
			new Vector3((float)Math.Cos(radiantTraining+3*Math.PI/2)*Parameters.goal_distance_training,Parameters.goal_height, (float)Math.Sin(radiantTraining+3*Math.PI/2)*Parameters.goal_distance_training)};
		testingPositions = new Vector3[4] {new Vector3((float)Math.Cos(radiantTesting)*Parameters.goal_distance_testing,Parameters.goal_height, (float)Math.Sin(radiantTesting)*Parameters.goal_distance_testing),
			new Vector3((float)Math.Cos(radiantTesting+Math.PI/2 )*Parameters.goal_distance_testing,Parameters.goal_height, (float)Math.Sin(radiantTesting+Math.PI/2)*Parameters.goal_distance_testing),
			new Vector3((float)Math.Cos(radiantTesting+Math.PI)*Parameters.goal_distance_testing,Parameters.goal_height, (float)Math.Sin(radiantTesting+Math.PI)*Parameters.goal_distance_testing),
			new Vector3((float)Math.Cos(radiantTesting+3*Math.PI/2)*Parameters.goal_distance_testing,Parameters.goal_height, (float)Math.Sin(radiantTesting+3*Math.PI/2)*Parameters.goal_distance_testing)};
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
		for (int i = 0; i <= Parameters.numberOfIntroTrials; i++)
		{
			trial t;
			t.text = "Introduction\n\nMove the wooden pointer to the silver sphere.\nPress the lower button to grab the silver sphere.\nMove the silver sphere above the red mark.\n Drop the silver sphere by releasing the button.\n" +
				"Try to hit the red sphere exactly.\n You may notice that the silver sphere is drifting if it is dropped.\nThis is due to wind.\nYou feel a force if you grab the silver sphere.\nIt tells you from where and how strong the wind is.\n\n";
			t.displaytime = float.MaxValue;
			t.type = typeOfTrial.INTRO;
			// randomly select a position from the positions list
			t.position = introPositions[UnityEngine.Random.Range(0, introPositions.Length)];
			trialQueue.Enqueue(t);
		}
		
		//create superblocks of training trials followed by testing trials
		for (int i = 0; i < Parameters.numberOfRepetitions; i++)
		{
			//training trials
			for (int j = 0; j < Parameters.numberOfTrainingTrials; j++)
			{
				trial t;
				t.text = "";
				t.displaytime = 0;
				if (j == 0)
				{
					t.text = "Training trials.\n\nThe force you are feeling is the wind.\nThe wind influences the silver sphere if it is dropped\nTry to hit the red mark exactly.\n\n" +
						"This text will disappear in 20 seconds.";
					t.displaytime = 20;
				}
				t.type = typeOfTrial.TRAINING;
				t.position = trainingPositions[UnityEngine.Random.Range(0, trainingPositions.Length)];
				trialQueue.Enqueue(t);
			}
			
			// testing trials
			for (int j = 0; j < Parameters.numberOfTestingTrials; j++)
			{
				trial t;
				t.text = "";
				t.displaytime = 0;
				if (j == 0)
				{
					t.text = "Testing trials.\n\nNo force is indicating wind speed and direction.\nThe silver sphere is still influenced by the same wind as in previous trials if it is droped.\nTry to hit the red sphere exactly.\n\n" +
						"This text will disappear in 10 seconds.";
					t.displaytime = 20;
				}
				t.type = typeOfTrial.TESTING;
				t.position = testingPositions[UnityEngine.Random.Range(0, testingPositions.Length)];
				trialQueue.Enqueue(t);
			}
		}

		//trial indicating the end of the experiment
		trial tEnd;
		tEnd.type = typeOfTrial.END;
		tEnd.text = "Well Done.\n\nThe experiment is over.\nThank you for your participation.";
		tEnd.displaytime = float.MaxValue;
		tEnd.position = Vector3.zero;
		trialQueue.Enqueue(tEnd);
		// write information about trial creation into log file
		logger.Write(System.DateTime.Now + " Trial List Created\nNumber of IntroTrials: " + Parameters.numberOfIntroTrials +
		             "\nNumber of TrainingTrials: " + Parameters.numberOfTrainingTrials + "\nNumber of Testing Trials: " +
		             Parameters.numberOfTestingTrials + "\n");
	}

	/// <summary>
	/// Advances to the enxt trial
	/// </summary>
	public void NextTrial()
	{
		currentTrial = (trial)trialQueue.Dequeue();
	}
}
