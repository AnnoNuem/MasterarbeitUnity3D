/**
 * ReachOut 3D Experiment
 * Axel Schaffland
 * aschaffland@uos.de
 * 
 * Adapted from AdjustOVR.cs 
 * by Leon Rene Suetfeld
 * lsuetfel@uos.de
 * 
 * SS2015
 * Neuroinformatics
 * Institute of Cognitive Science
 * University of Osnabrueck
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// Allows to adjust the viewpoint of the OVR in the experiment to match real view direction and rotation
/// </summary>
public class AdjustOVR : MonoBehaviour {

	public Main main;

	void Start () 
	{
	}
	
	void Update () 
	{
		//if (main.state == Main.states.INTRO)
		{
			if (Input.GetKey("d")) {
				transform.localPosition = new Vector3(transform.localPosition.x + 0.01f,transform.localPosition.y,transform.localPosition.z);
			}
			if (Input.GetKey("a")) {
				transform.localPosition = new Vector3(transform.localPosition.x - 0.01f,transform.localPosition.y,transform.localPosition.z);
			}
			if (Input.GetKey("q")) {
				transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y + 0.01f,transform.localPosition.z);
			}
			if (Input.GetKey("e")) {
				transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y - 0.01f,transform.localPosition.z);
			}
			if (Input.GetKey("w")) {
				transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z + 0.01f);
			}
			if (Input.GetKey("s")) {
				transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z - 0.01f);
			}
			
			if (Input.GetKey ("up")) {
				transform.localRotation = new Quaternion(transform.localRotation.x + 0.01f,transform.localRotation.y,transform.localRotation.z,transform.localRotation.w);
			}
			if (Input.GetKey ("down")) {
				transform.localRotation = new Quaternion(transform.localRotation.x - 0.01f,transform.localRotation.y,transform.localRotation.z,transform.localRotation.w);
			}
			if (Input.GetKey ("left")) {
				transform.localRotation = new Quaternion(transform.localRotation.x,transform.localRotation.y - 0.01f,transform.localRotation.z,transform.localRotation.w);
			}
			if (Input.GetKey ("right")) {
				transform.localRotation = new Quaternion(transform.localRotation.x,transform.localRotation.y + 0.01f,transform.localRotation.z,transform.localRotation.w);
			}
			if (Input.GetKey (",")) {
				transform.localRotation = new Quaternion(transform.localRotation.x,transform.localRotation.y,transform.localRotation.z + 0.01f,transform.localRotation.w);
			}
			if (Input.GetKey (".")) {
				transform.localRotation = new Quaternion(transform.localRotation.x,transform.localRotation.y,transform.localRotation.z - 0.01f,transform.localRotation.w);
			}
		}
	}
}
