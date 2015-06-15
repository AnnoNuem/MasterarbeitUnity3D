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
/// Statistics. Class to compute statistics for trials and trialblocks
/// </summary>
public sealed class Statistics {

	// singleton functions and variables
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
	private double sumAccuracyInBlock = 0;
	private double sumVarianceInBlock = 0;


	/// <summary>
	/// Computes the euklidean distance of two 3D Points projected on the x-z plane
	/// </summary>
	/// <returns>The accuracy.</returns>
	/// <param name="a">Position a.</param>
	/// <param name="b">Position b.</param>
	private double computeDistance(Vector3 a, Vector3 b)
	{
		return (Math.Sqrt(Math.Pow ((a.x - b.x),2) + Math.Pow((a.z - b.z),2)));
	}

	/// <summary>
	/// compute variance between straig line from start to drop position and actual path in order to define explorer exploider behaviour
	/// </summary>
	/// <returns>The variance.</returns>
	/// <param name="dropPosition">Drop position.</param>
	/// <param name="positions">Positions.</param>
	private double computeVariance(Vector3 dropPosition, List<Vector3>positions)
	{
		//compute straight line between start and drop position
		//start is [0,0,0] thus y-intercept is zero and line can be described with drop Position vector only
		double sumDistances = 0;
		uint i = 0;
		//prevent division by zero
		if ( dropPosition.x == 0 && dropPosition.z == 0)
		{
			dropPosition.x = 0.00001f;
		}
		foreach (Vector3 position in positions)
		{
			double distanceStartDrop = computeDistance(Vector3.zero, dropPosition);
			double distance = (Math.Abs(dropPosition.z * position.x - dropPosition.x * position.z) / distanceStartDrop);
			sumDistances+= distance;
			i++;
		}
		return sumDistances/i;
	}

	public void computeTrialStatistics(Vector3 dropPosition, Vector3 hitPosition, Vector3 goalPosition, List<Vector3> positions)
	{
		// compute distance between start position and goal position in order to normalize variance and accuracy
		// start postion is 0,0
		double distanceStartGoal = computeDistance(Vector3.zero, goalPosition);
		numberOfTrialsInBlock++;
		double accuracy = computeDistance(hitPosition, goalPosition) / distanceStartGoal;
		double variance = computeVariance(dropPosition, positions) / distanceStartGoal;
		sumAccuracyInBlock+=accuracy;
		sumVarianceInBlock+=variance;
		logger.Write("Accuracy: " + accuracy + "\nVariance: " + variance + "\n");
	}

	public void computeBlockStatistics()
	{
		double blockAccuracy = sumAccuracyInBlock/numberOfTrialsInBlock;
		double blockVariance = sumVarianceInBlock/numberOfTrialsInBlock;
		logger.Write("Block Accuracy: " + blockAccuracy + "\nBlock Variance: " + blockVariance + "\n");
		numberOfTrialsInBlock = 0;
		sumAccuracyInBlock = 0;
		sumVarianceInBlock = 0;
	}
}
