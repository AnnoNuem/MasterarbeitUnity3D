using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class Statistics {

	// singleton
	private static readonly Statistics instance = new Statistics();

	static Statistics()
	{
	}
	
	private Statistics()
	{
		logger = Logger.Instance;
	}
	
	public static Statistics Instance
	{
		get
		{
			return instance;
		}
	}
	
	private static Logger logger;
	private uint numberOfTrialsInBlock = 0;
	private float sumAccuracyInBlock = 0;
	private float sumVarianceInBlock = 0;

	private float computeAccuracy(Vector3 a, Vector3 b)
	{
		return (float)(Math.Abs(a.x-b.x) + Math.Abs(a.z-b.z))/2;
	}

	//compute variance between straig line from start to drop position and actual path in order to define explorer exploider behaviour
	private float computeVariance(Vector3 dropPosition, List<Vector3>positions)
	{
		//compute straight line between start and drop position
		//start is [0,0,0] thus y-intercept is zero and line can be described with drop Position vector only
		float sumDistances = 0;
		uint i = 0;
		//prevent division by zero
		if ( dropPosition.x == 0 && dropPosition.z == 0)
		{
			dropPosition.x = 0.00001f;
		}
		foreach (Vector3 position in positions)
		{
			float k = (float)((position.x * dropPosition.x + position.z * dropPosition.z)/(Math.Pow(dropPosition.x,2) + Math.Pow(dropPosition.z,2)));
			float distance = (float)(Math.Sqrt(Math.Pow((k*dropPosition.x - position.x),2) + Math.Pow((k*dropPosition.z - position.z),2)));
			sumDistances+= distance;
			i++;
		}
		return sumDistances/i;
	}

	public void computeTrialStatistics(Vector3 dropPosition, Vector3 hitPosition, Vector3 goalPosition, List<Vector3> positions)
	{
		numberOfTrialsInBlock++;
		float accuracy = computeAccuracy(hitPosition, goalPosition);
		float variance = computeVariance(dropPosition, positions);
		sumAccuracyInBlock+=accuracy;
		sumVarianceInBlock+=variance;
		logger.Write("Accuracy: " + accuracy + "\nVariance: " + variance + "\n");

	}

	public void computeBlockStatistics()
	{
		float blockAccuracy = sumAccuracyInBlock/numberOfTrialsInBlock;
		float blockVariance = sumVarianceInBlock/numberOfTrialsInBlock;
		logger.Write("Block Accuracy: " + blockAccuracy + "\nBlock Variance: " + blockVariance + "\n");
		numberOfTrialsInBlock = 0;
		sumAccuracyInBlock = 0;
		sumVarianceInBlock = 0;
	}
}
