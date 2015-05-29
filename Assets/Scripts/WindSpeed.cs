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
	private Vector2 GetXY(Vector3 position)
	{
		float x = (position[0] + Parameters.fieldSizeX ) * Parameters.xscale + Parameters.xbias;
		float z = (position[2] + Parameters.fieldSizeZ ) * Parameters.zscale + Parameters.zbias;
		return new Vector2(x,z);
	}

	//compute wind forces in x and z direction at given cordinate
	public Vector2 ComputeWindForce(Vector3 position)
	{
			return GetXY(position);
	}

	public float ComputeWindDirection(Vector3 position)
	{
		Vector2 v = GetXY(position);
		float hypo = (float)Math.Sqrt(v.x * v.x + v.y * v.y);
		float direction = (float) Math.Asin(v.y/hypo);
		return  (float)(-direction * (180.0 / Math.PI) - 90);
	}

	public float ComputeWindSpeed(Vector3 position)
	{
		Vector2 v = GetXY(position);
		return (float) Math.Sqrt(v.x*v.x+v.y*v.y);
	}
}
