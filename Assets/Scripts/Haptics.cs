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
using UnityEngine.EventSystems;
using System.Collections;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Haptics. Manages input of SensAble Haptic Device
/// </summary>
// sensable does not support linear forces or a force changing over time. thus different bins 
// which get activate depending on position of pointer
public class Haptics : HapticClassScript {


	//Generic Haptic Functions
	private GenericFunctionsClass myGenericFunctionsClassScript;

	public SphereMovement sm;
	public Main main;
	public GameObject sphere;
	public WindSpeed ws;
	public string Type;
	private const int effect_index = 0;
	public float gain;
	public float magnitude;
	public float[] directionArray = new float[3];
	public float duration;
	public float frequency;
	public GameObject cursor;
	public StartMenue startMenue;
	public SphereAudio sphereAudio;
	public LineRenderer l;
	//number of bins of different constant wind forces
	public const uint wind_bins = 100;
		
	void Awake()
	{
		myGenericFunctionsClassScript = transform.GetComponent<GenericFunctionsClass>();
	}
	

	public void Start()
	{
		ws = WindSpeed.Instance;
		if(PluginImport.InitHapticDevice())
		{
			myGenericFunctionsClassScript.SetHapticWorkSpace();
			myGenericFunctionsClassScript.GetHapticWorkSpace();

			PluginImport.UpdateWorkspace(myHapticCamera.transform.rotation.eulerAngles.y);
			
			PluginImport.SetMode(ModeIndex);

			myGenericFunctionsClassScript.IndicateMode();

			PluginImport.SetTouchableFace(ConverterClass.ConvertStringToByteToIntPtr(TouchableFace));
			
		}
		else
		{
			Debug.Log("Haptic Device cannot be launched");
		}

		myGenericFunctionsClassScript.SetHapticGeometry();

		// create different constant effects which are activated depending on the position of the sphere and the cursor
		// higher numbers correspond to higer forces
		// assumes wind is linear and wind direction is the same at all positions
		Vector2 direction = ws.ComputeWindForce(sphere.transform.position);
		direction.Normalize();
		directionArray[0] = -direction.x;
		directionArray[2] = -direction.y;
		IntPtr directionPtr = ConverterClass.ConvertFloat3ToIntPtr(directionArray);
		IntPtr type = ConverterClass.ConvertStringToByteToIntPtr("constant");
		float[] positionArray = {0f,0f,0f};
		IntPtr positionPtr = ConverterClass.ConvertFloat3ToIntPtr(positionArray);

		for (int i = 0; i < wind_bins; i++)
		{
			magnitude = ((float)i)/wind_bins;
			PluginImport.SetEffect(type , i, gain, magnitude, duration, frequency, positionPtr, directionPtr);
		}


		PluginImport.LaunchHapticEvent();
	}
	

	bool sphere_grabbed = false; 
	int active_event_id = -1;
	bool prev_button_state_start = false;
	void Update()
	{				
		PluginImport.UpdateWorkspace(myHapticCamera.transform.rotation.eulerAngles.y);
		
		myGenericFunctionsClassScript.UpdateGraphicalWorkspace();
		
		PluginImport.RenderHaptic ();

		myGenericFunctionsClassScript.GetProxyValues();
		
		myGenericFunctionsClassScript.GetTouchedObject();


		// if startmenu active use haptic device as laser pointer pointing to the UI
		if ( main.state == Main.states.STARTSCREEN)
		{	
			Vector3 positionV = cursor.transform.position;
			double[] orientation = ConverterClass.ConvertIntPtrToDouble3(PluginImport.GetProxyDirection());
			Vector3 orientationV = new Vector3((float)orientation[0], (float)orientation[1], (float)orientation[2]);
			orientationV.Normalize();
			l.enabled = true;
			l.SetPosition(0, positionV);
			l.SetPosition(1, positionV + 400 * orientationV);
			// use raycasts to detect to which object the poiner pointed when button was pressed
			if ( (prev_button_state_start != PluginImport.GetButton1State()) && PluginImport.GetButton1State())
			{
				RaycastHit hit;
				if(Physics.Raycast(positionV, orientationV, out hit))
				{
					startMenue.objectHit(hit.collider.gameObject);
				}
			}
			prev_button_state_start = PluginImport.GetButton1State();
		}
		else
		{
			l.enabled = false;
		}		

		// apply wind force to sphere if it is grabbed and the current trial is an intro or training trial
		if (sphere_grabbed && (main.state == Main.states.INTRO || main.state == Main.states.TRAINING))
		{
			int old_active_event_id = active_event_id;
			float windSpeed = ws.ComputeWindSpeed(sphere.transform.position);
			active_event_id = (int) (windSpeed * wind_bins);
			if (active_event_id != old_active_event_id)
			{
				if (old_active_event_id != -1)
				{
					PluginImport.StopEffect(old_active_event_id);
				}
				PluginImport.StartEffect(active_event_id);
			}
		}
		else if(active_event_id != -1)
		{
			PluginImport.StopEffect(active_event_id);
			active_event_id = -1;
		}


		if(PluginImport.GetMode() == 1 && main.state != Main.states.STARTSCREEN)
		{
			ActivatingGrabbedObjectPropperties();
		}
	}

	void OnDisable()
	{
		if (PluginImport.HapticCleanUp())
		{
		}
	}

	bool previousButtonState = false;
	string grabbedObjectName = "";
	/// <summary>
	/// If GameObject is grabbed
	/// </summary>
	void ActivatingGrabbedObjectPropperties(){
		
		GameObject grabbedObject;
		string myObjStringName;
		
		if (!previousButtonState && PluginImport.GetButton1State())
		{
			myObjStringName = ConverterClass.ConvertIntPtrToByteToString(PluginImport.GetTouchedObjectName());
			if(!myObjStringName.Equals("null")){
				grabbedObject = GameObject.Find(myObjStringName);
				if(grabbedObject.rigidbody != null)
				{
					grabbedObject.rigidbody.isKinematic=true;
					grabbedObject.rigidbody.useGravity=false;
				}
				grabbedObjectName = myObjStringName;
				if (grabbedObjectName.Equals("Sphere"))
				{
					sphere_grabbed = true;
					sphereAudio.Grab();
				}
			}
			previousButtonState = true;
		}
		else if (previousButtonState && !PluginImport.GetButton1State())
		{
			if(grabbedObjectName.Equals("Sphere")){
				sphere_grabbed = false;
				sphereAudio.Ungrap();
				sm.SwitchState(SphereMovement.sphereStates.DROPPING);
				grabbedObjectName = "";
			}
			previousButtonState = false;
		}
	}
}
