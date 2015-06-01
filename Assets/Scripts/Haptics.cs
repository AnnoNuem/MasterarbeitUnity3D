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
	public int effect_index;
	public float gain;
	public float magnitude;
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
			
			//Update Workspace as function of camera
			PluginImport.UpdateWorkspace(myHapticCamera.transform.rotation.eulerAngles.y);
			
			//Set Mode of Interaction
			/*
			 * Mode = 0 Contact
			 * Mode = 1 Manipulation - So objects will have a mass when handling them
			 * Mode = 2 Custom Effect - So the haptic device simulate vibration and tangential forces as power tools
			 * Mode = 3 Puncture - So the haptic device is a needle that puncture inside a geometry
			 */
			PluginImport.SetMode(ModeIndex);
			//Show a text descrition of the mode
			myGenericFunctionsClassScript.IndicateMode();

			//Set the touchable face(s)
			PluginImport.SetTouchableFace(ConverterClass.ConvertStringToByteToIntPtr(TouchableFace));
			
		}
		else
			Debug.Log("Haptic Device cannot be launched");

		/***************************************************************/
		//Set Environmental Haptic Effect
		/***************************************************************/
			// Viscous Force Example
			//myGenericFunctionsClassScript.SetEnvironmentViscosity ();

			// Constant Force Example - We use this environmental force effect to simulate the weight of the cursor


			// Friction Force Example
			//myGenericFunctionsClassScript.SetEnvironmentFriction();

			// Spring Force Example
			//myGenericFunctionsClassScript.SetEnvironmentSpring();

		
		/***************************************************************/
		//Setup the Haptic Geometry in the OpenGL context 
		//And read haptic characteristics
		/***************************************************************/
		myGenericFunctionsClassScript.SetHapticGeometry();

		//Get the Number of Haptic Object
		//Debug.Log ("Total Number of Haptic Objects: " + PluginImport.GetHapticObjectCount());

		/***************************************************************/
		//Launch the Haptic Event for all different haptic objects
		/***************************************************************/
		PluginImport.LaunchHapticEvent();
	}
	

	void Update()
	{
		if (main.state == Main.states.INTRO || main.state == Main.states.TRAINING)
		{
			double[] _temp = myGenericFunctionsClassScript.getProxyPosition();
			Vector3 position = new Vector3((float)_temp[0],(float)_temp[1],(float)_temp[2]);
			Vector2 force = ws.ComputeWindForce(position);
			//convert String to IntPtr
			IntPtr type = ConverterClass.ConvertStringToByteToIntPtr("const");
			//Convert float[3] to intptr
			float[] _temp2 = {0f,0f,0f};
			IntPtr p = ConverterClass.ConvertFloat3ToIntPtr(_temp2);
			//Convert float[3] to intptr
			float[] _temp3 = {force.x, 0, force.y};
			IntPtr direction = ConverterClass.ConvertFloat3ToIntPtr(_temp3);

			//PluginImport.SetEffect(type , effect_index, gain, magnitude, duration, frequency, p, direction);
			//PluginImport.StartEffect(effect_index);
		}


		/***************************************************************/
		//Act on the rigid body of the Manipulated object
		// if Mode = Manipulation Mode
		/***************************************************************/
		if(PluginImport.GetMode() == 1)
			ActivatingGrabbedObjectPropperties();

		/***************************************************************/
		//Update Workspace as function of camera
		/***************************************************************/
		PluginImport.UpdateWorkspace(myHapticCamera.transform.rotation.eulerAngles.y);

		/***************************************************************/
		//Update cube workspace
		/***************************************************************/
		myGenericFunctionsClassScript.UpdateGraphicalWorkspace();

		/***************************************************************/
		//Haptic Rendering Loop
		/***************************************************************/
		PluginImport.RenderHaptic ();
		
		myGenericFunctionsClassScript.GetProxyValues();

		myGenericFunctionsClassScript.GetTouchedObject();

		//Debug.Log ("Button 1: " + PluginImport.GetButton1State());
		//Debug.Log ("Button 2: " + PluginImport.GetButton2State());
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

	/*****************************************************************************************/
	/* Act on Characteristics of Rigid Body of the manipulated object*/
	/*****************************************************************************************/
	
	//Deactivating gravity and enabled kinematics when a object is grabbed
	bool previousButtonState = false;
	string grabbedObjectName = "";
	
	void ActivatingGrabbedObjectPropperties(){
		
		GameObject grabbedObject;
		string myObjStringName;
		
		if (!previousButtonState && PluginImport.GetButton1State())
		{
			//If the object is grabbed, the gravity is deactivated and kinematic is enabled
			myObjStringName = ConverterClass.ConvertIntPtrToByteToString(PluginImport.GetTouchedObjectName());
			
			if(!myObjStringName.Equals("null")){
				grabbedObject = GameObject.Find(myObjStringName);
				
				//If there is a rigid body
				if(grabbedObject.rigidbody != null)
				{
					grabbedObject.rigidbody.isKinematic=true;
					grabbedObject.rigidbody.useGravity=false;
				}
				grabbedObjectName = myObjStringName;
			}
			previousButtonState = true;
		}
		
		else if (previousButtonState && !PluginImport.GetButton1State())
		{
			//If the object is dropped, the grabity is enabled again and kinematic is deactivated
			if(grabbedObjectName.Equals("Sphere")){
				sm.SwitchState(SphereMovement.sphereStates.DROPPING);
				grabbedObjectName = "";
			}
			previousButtonState = false;
		}
		
	}

}
