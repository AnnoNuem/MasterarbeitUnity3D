public static class Parameters
{
	//fieldsize limitin area of movement and used for wind computation
	public const int fieldSizeX = 2;
	public const int fieldSizeZ = 2;

	//speed of object moved by joystick
	public const float moveSpeed = 0.05f;

	//number of trials
	public const int numberOfIntroTrials = 2;
	public const int numberOfTrainingTrials = 2;
	public const int numberOfTestingTrials = 2;

	//parameters for wind
	public const float xscale = 0.0f;
	public const float zscale = 1f;
	public const float xbias = 0.0f;
	public const float zbias = 0.1f;

	//scale of the arrow indicating wind direction and force
	public const float arrowScale = 0.005f;
	public const float arrowMinSize = 0.01f;
	public const float arrowY = 1.4f;

	//how long should the sphere laying on the floor been displayed
	public const float dispayOfHit = 0.7f;
	
	//pause between trials with onl ground visible
	public const float pauseBetweenTrials = 0.3f;
}
