using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;


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
	
	/*****************************************************************************/
	
	void Awake()
	{
		myGenericFunctionsClassScript = transform.GetComponent<GenericFunctionsClass>();
	}
	

	void Start()
	{
		ws = WindSpeed.Instance;
		if(PluginImport.InitHapticDevice())
		{
			Debug.Log("OpenGL Context Launched");
			Debug.Log("Haptic Device Launched");
			
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


		// create 100 different constant effects which are activated depending on the position of the sphere and the cursur
		// 99 is the strongest force and 0 the weakest wind force
		// asuming that the wind is linear
		Vector2 direction = ws.ComputeWindForce(sphere.transform.position);
		direction.Normalize();
		directionArray[0] = direction.x;
		directionArray[2] = direction.y;
		IntPtr directionPtr = ConverterClass.ConvertFloat3ToIntPtr(directionArray);
		IntPtr type = ConverterClass.ConvertStringToByteToIntPtr("constant");
		float[] positionArray = {0f,0f,0f};
		IntPtr positionPtr = ConverterClass.ConvertFloat3ToIntPtr(positionArray);

		for (int i = 0; i < 10; i++)
		{
			magnitude = ((float)i)/10;
			Debug.Log (direction.x + " " + direction.y);
			PluginImport.SetEffect(type , i, gain, magnitude, duration, frequency, positionPtr, directionPtr);
		}


		PluginImport.LaunchHapticEvent();
	}
	

	bool sphere_grabbed = false; 
	int active_event_id = -1;

	void Update()
	{
		if (sphere_grabbed && (main.state == Main.states.INTRO || main.state == Main.states.TRAINING))
		{
			if (active_event_id != -1)
			{
				PluginImport.StopEffect(active_event_id);
			}
			active_event_id = 2;
			PluginImport.StartEffect(active_event_id);
		}
		else if(active_event_id != -1)
		{
			PluginImport.StopEffect(active_event_id);
			active_event_id = -1;
		}


		if(PluginImport.GetMode() == 1)
		{
			ActivatingGrabbedObjectPropperties();
		}

		PluginImport.UpdateWorkspace(myHapticCamera.transform.rotation.eulerAngles.y);

		myGenericFunctionsClassScript.UpdateGraphicalWorkspace();

		PluginImport.RenderHaptic ();
		
		myGenericFunctionsClassScript.GetProxyValues();

		myGenericFunctionsClassScript.GetTouchedObject();
	}

	void OnDisable()
	{
		if (PluginImport.HapticCleanUp())
		{
			Debug.Log("Haptic Context CleanUp");
			Debug.Log("Desactivate Device");
			Debug.Log("OpenGL Context CleanUp");
		}
	}

	bool previousButtonState = false;
	string grabbedObjectName = "";

	// If GameObject is grabbed
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
				}
			}
			previousButtonState = true;
		}
		else if (previousButtonState && !PluginImport.GetButton1State())
		{
			if(grabbedObjectName.Equals("Sphere")){
				sphere_grabbed = false;
				sm.SwitchState(SphereMovement.sphereStates.DROPPING);
				grabbedObjectName = "";
			}
			previousButtonState = false;
		}
	}
}
