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
public static class Parameters
{
	// FIELDSIZE
	// limiting the  area of movement also used for wind computation
	public const float fieldSizeX = 0.6f;
	public const float fieldSizeZ = 0.6f;

	//TRIALS
	public const int numberOfIntroTrials = 8;
	public const int numberOfTrainingTrials = 8;
	public const int numberOfTestingTrials = 16;
	// At with degrees spawns the spehre in different type of trials. 
	// 0 for example means north, east, south, west. 45 means northeast, southeast, southwest, northwest
	public const float degreeIntro = 0f;
	public const float degreeTraining = 45f;
	public const float degreeTesting1 = 22.5f;

	// GOAL
	//how height should goal be diplayed over ground
	public const float goal_height = 2.878f;
	// how far away should the goal be from the origin. If 1 the goal is positioned on a unit circle arround the origin. Denotes radius of circle the goal is positioned on
	public const float goal_distance_intro = 0.4f;
	public const float goal_distance_training = 0.4f;
	public const float goal_distance_testing = 0.3f;
	// SPHERE
	public const float startPositionHeight = 3.38f;

	//WIND
	public const float xscale = 0.0f;
	public const float zscale = 1f;
	public const float windForceForSphereFactor = 4f; 

	//how long should the sphere after hitting the ground bebe displayed
	public const float dispayOfHit = 0.7f;
	
	//pause between trials with only ground visible
	public const float pauseBetweenTrials = 0.3f;
}
