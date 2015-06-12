/**
 * ReachOut 2D Experiment
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

/// <summary>
/// Sphere movement. Handles the sphere. State machine for different sphere states. Checks if sphere is moved by user, dropped, colides with ground. Logs the position the sphere is at
/// </summary>
public class SphereMovement : MonoBehaviour {

	Vector3 startPosition; 
	public Vector3 dropPosition;
	public GameObject sphere;
	public GameObject helper;
	public GameObject ground;
	public GameObject goal;
	public WindSpeed windSpeed;
	public Trials trials;
	public Statistics statistics;
	public SphereAudio sphereAudio;
	public enum sphereStates
	{
		HIDDEN,
		MOVING,
		DROPPING,
		COLLIDED,
	}
	public sphereStates state = sphereStates.HIDDEN;

	private List<Vector3> positions;
	
	void  Awake() 
	{
		windSpeed = WindSpeed.Instance;
		trials = Trials.Instance;
		statistics = Statistics.Instance;
		positions = new List<Vector3>();
	}

	void Start()
	{
		startPosition = new Vector3(0,Parameters.startPositionHeight,0);
		sphere.transform.position = startPosition;
	}

	void FixedUpdate()
	{		
		// apply windforce to sphere if it is dropped
				if (state == sphereStates.DROPPING)
		{
			Vector2 force = windSpeed.ComputeWindForceForSphere(sphere.transform.position);
			sphere.rigidbody.AddForce(new Vector3(force.x, 0, force.y));
		}
	}
	
	void Update () 
	{
		switch (state)
		{
		case sphereStates.MOVING:
			Vector3 v = sphere.transform.position;
			if ( v.x < -Parameters.fieldSizeX )
			{
				v.x = -Parameters.fieldSizeX;
			}
			else if ( v.x > Parameters.fieldSizeX)
			{
				v.x = Parameters.fieldSizeX;
			}
			if ( v.z < -Parameters.fieldSizeZ )
			{
				v.z = -Parameters.fieldSizeZ;
			}
			else if ( v.z > Parameters.fieldSizeZ)
			{
				v.z = Parameters.fieldSizeZ;
			}
			sphere.transform.position = v;
			positions.Add(sphere.transform.position);
			break;
		}
	}

	/// <summary>
	/// Switchs state of sphere to "newState"
	/// </summary>
	/// <param name="newState">New state.</param>
	public void SwitchState(sphereStates newState)
	{
		this.state = newState;
		switch (newState)
		{
			case sphereStates.DROPPING:
				dropPosition =sphere.transform.position;
				sphere.rigidbody.useGravity = true;
				sphere.rigidbody.isKinematic = false;
				sphere.renderer.enabled = true;
				break;
			case sphereStates.HIDDEN:
				sphere.renderer.enabled = false;
				sphere.rigidbody.useGravity = false;
				sphere.rigidbody.isKinematic = false;
				break;
			case sphereStates.MOVING:
				sphere.transform.position = startPosition;
				sphere.renderer.enabled = true;
				sphere.rigidbody.useGravity = false;
				sphere.rigidbody.isKinematic = true;
				break;
		}
	}

	/// <summary>
	/// Raises the collision enter event. Compute statistics of the trial. Display sphere at hit position to allow subject to process where sphere hit the ground. Start new trial.
	/// </summary>
	/// <param name="col">Col.</param>
	IEnumerator OnCollisionEnter(Collision col)
	{	
		if (state == sphereStates.DROPPING && col.gameObject == ground)
		{
			sphereAudio.Collission();
			statistics.computeTrialStatistics(dropPosition, sphere.transform.position, goal.transform.position, positions);
			positions.Clear();
			sphere.rigidbody.useGravity = false;
			sphere.rigidbody.velocity = Vector3.zero;
			sphere.rigidbody.angularVelocity = Vector3.zero;
			SwitchState(sphereStates.COLLIDED);
			yield return new WaitForSeconds(Parameters.dispayOfHit);
			sphere.renderer.enabled = false;
			helper.SendMessage("newTrial");
		}
	}
}
