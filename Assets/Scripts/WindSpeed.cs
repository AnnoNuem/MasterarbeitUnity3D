using UnityEngine;
using System.Collections;
using System;

public sealed class WindSpeed 
{
	// singleton
	private static readonly WindSpeed instance = new WindSpeed();
	
	// Explicit static constructor to tell C# compiler
	// not to mark type as beforefieldinit
	static WindSpeed()
	{
	}
	
	private WindSpeed()
	{
	}
	
	public static WindSpeed Instance
	{
		get
		{
			return instance;
		}
	}
	private float[] GetXY(Vector3 position)
	{
		float x = (position[0] / (Parameters.fieldSizeX * 2f) + Parameters.fieldSizeX/2f) * Parameters.xscale;
		float z = (position[2] / (Parameters.fieldSizeZ * 2f) + Parameters.fieldSizeZ/2f) * Parameters.zscale;
		float[] a = {x,z};
		return a;
	}

	//compute wind forces in x and z direction at given cordinate
	public Vector2 ComputeWindForce(Vector3 position)
	{
		float[] a = GetXY(position);
		return new Vector2(a[0],a[1]);
	}

	// computes wind force and multiplies wwith factor to adjust speed of sphere
	public Vector2 ComputeWindForceForSphere(Vector3 position)
	{
		return ComputeWindForce(position) * Parameters.windForceForSphereFactor;
	}

	public float ComputeWindDirection(Vector3 position)
	{
		float[] a = GetXY(position);
		float hypo = (float)Math.Sqrt(a[0] * a[0] + a[1] * a[1]);
		float direction = (float) Math.Asin(a[1]/hypo);
		return  (float)(-direction * (180.0 / Math.PI) - 90);
	}

	public float ComputeWindSpeed(Vector3 position)
	{
		float[] a = GetXY(position);
		return (float) Math.Sqrt(a[0] * a[0] + a[1] * a[1]);
	}
}
