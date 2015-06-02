using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public enum sphereStates
	{
		HIDDEN,
		MOVING,
		DROPPING,
		COLLIDED,
	}
	public sphereStates state = sphereStates.HIDDEN;

	private List<Vector3> positions;

	// Use this for initialization
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
		if (state == sphereStates.DROPPING)
		{
			Vector2 force = windSpeed.ComputeWindForce(sphere.transform.position);
			sphere.rigidbody.AddForce(new Vector3(force.x, 0, force.y));
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
//		Debug.Log(sphere.collider.gameObject);

//		Debug.Log (state);
		switch (state)
		{
		case sphereStates.MOVING:
			positions.Add(sphere.transform.position);
			float x = Input.GetAxis ("L_XAxis_1"); 
			float z = -Input.GetAxis ("L_YAxis_1");
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
			break;
		}
	}

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

	IEnumerator OnCollisionEnter(Collision col)
	{	
		if (state == sphereStates.DROPPING && col.gameObject == ground)
		{
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
