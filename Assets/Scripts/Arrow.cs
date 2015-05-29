using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public GameObject arrow;
	public GameObject sphere;
	static WindSpeed windSpeed;


	// Use this for initialization
	void Start () {
		windSpeed = WindSpeed.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		if (arrow.renderer.enabled)
		{
			arrow.transform.rotation = Quaternion.AngleAxis(
				windSpeed.ComputeWindDirection(transform.position), Vector3.up);
			arrow.transform.localScale = new Vector3(
				windSpeed.ComputeWindSpeed(transform.position)*Parameters.arrowScale + Parameters.arrowMinSize,
				1, 
				windSpeed.ComputeWindSpeed(transform.position)*Parameters.arrowScale + Parameters.arrowMinSize);
			Vector3 pos = sphere.transform.position;
			pos.y = Parameters.arrowY;
			arrow.transform.position = pos;
		}
	}
}
