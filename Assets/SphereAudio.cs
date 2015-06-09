using UnityEngine;
using System.Collections;

public class SphereAudio : MonoBehaviour {

	public AudioSource grab;
	public AudioSource ungrab;
	public AudioSource whileGrabbed;
	public AudioSource collission;


	public void Grab()
	{
		grab.Play();
		whileGrabbed.Play();
	}

	public void Ungrap()
	{
		ungrab.Play();
		whileGrabbed.Stop();
	}

	public void Collission()
	{
		collission.Play();
	}
}
