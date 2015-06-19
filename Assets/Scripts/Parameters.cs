/**
 * ReachOut 3D Experiment
 * Axel Schaffland
 * aschaffland@uos.de
 * SS2015
 * Neuroinformatics
 * Institute of Cognitive Science
 * University of Osnabrueck
 **/

/// <summary>
/// Parameters. Class to set important paramteres like number of trials
/// </summary>
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public static class Parameters
{
	// FIELDSIZE
	// limiting the  area of movement also used for wind computation
	public static float fieldSizeX;
	public static float fieldSizeZ;

	//TRIALS
	public static int numberOfRepetitions;
	public static int numberOfIntroTrials;
	public static int numberOfTrainingTrials;
	public static int numberOfTestingTrials;
	// At with degrees spawns the spehre in different type of trials. 
	// 0 for example means north, east, south, west. 45 means northeast, southeast, southwest, northwest
	public static float degreeIntro;
	public static float degreeTraining;
	public static float degreeTesting1;

	// GOAL
	//how height should goal be diplayed over ground
	public static float goal_height;
	// how far away should the goal be from the origin. If 1 the goal is positioned on a unit circle arround the origin. Denotes radius of circle the goal is positioned on
	public static float goal_distance_intro;
	public static float goal_distance_training;
	public static float goal_distance_testing;
	// SPHERE
	public static float startPositionHeight;

	//WIND
	public static float xscale;
	public static float zscale;
	public static float windForceForSphereFactor;

	//how long should the sphere after hitting the ground bebe displayed
	public static float dispayOfHit;
	
	//pause between trials with only ground visible
	public static float pauseBetweenTrials;

	static Dictionary<string, string> dic;

	public static void readParameters()
	{
		dic = File.ReadAllLines("CenterOut3DParameters.txt")
		.Select(l => l.Split(new[] { '=' }))
			.ToDictionary( s => s[0].Trim(), s => s[1].Trim());

		fieldSizeX = float.Parse(dic["fieldSizeX"]);
		fieldSizeZ = float.Parse(dic["fieldSizeZ"]);

		numberOfRepetitions = int.Parse(dic["numberOfRepetitions"]);
		numberOfIntroTrials = int.Parse(dic["numberOfIntroTrials"]);
		numberOfTrainingTrials = int.Parse(dic["numberOfTrainingTrials"]);
		numberOfTestingTrials = int.Parse(dic["numberOfTestingTrials"]);

		degreeIntro = float.Parse(dic["degreeIntro"]);
		degreeTraining = float.Parse(dic["degreeTraining"]);
		degreeTesting1 = float.Parse(dic["degreeTesting1"]);

		goal_height = float.Parse(dic["goal_height"]);

		goal_distance_intro = float.Parse(dic["goal_distance_intro"]);
		goal_distance_training = float.Parse(dic["goal_distance_training"]);
		goal_distance_testing = float.Parse(dic["goal_distance_testing"]);

		startPositionHeight = float.Parse(dic["startPositionHeight"]);

		xscale = float.Parse(dic["xscale"]);
		zscale = float.Parse(dic["zscale"]);
		windForceForSphereFactor = float.Parse(dic["windForceForSphereFactor"]);

		dispayOfHit = float.Parse(dic["dispayOfHit"]);

		pauseBetweenTrials = float.Parse(dic["pauseBetweenTrials"]);
	}
}
