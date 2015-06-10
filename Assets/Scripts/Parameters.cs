public static class Parameters
{
	//fieldsize limitin area of movement and used for wind computation
	public const float fieldSizeX = 0.8f;
	public const float fieldSizeZ = 0.8f;

	//number of trials
	public const int numberOfIntroTrials = 4;
	public const int numberOfTrainingTrials = 4;
	public const int numberOfTestingTrials = 8;


	//parameters goal
	public const float goal_height = 2.89f;
	public const float goal_scale = 0.4f;

	// start position of ball
	public const float startPositionHeight = 3.38f;

	//parameters for wind
	public const float xscale = 0.0f;
	public const float zscale = 1f;
	public const float xbias = 0.0f;
	public const float zbias = 0.8f;

	public const float windForceForSphereFactor = 4f; 


	//how long should the sphere laying on the floor been displayed
	public const float dispayOfHit = 0.7f;
	
	//pause between trials with onl ground visible
	public const float pauseBetweenTrials = 0.3f;
}
