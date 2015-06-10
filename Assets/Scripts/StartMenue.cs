using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenue : MonoBehaviour {

	public Button startExperiment;
	public InputField prename;
	public InputField surname;
	public InputField age;
	public InputField participantId;
	public Toggle male;
	public Toggle female;
	public GameObject helper;
	Main main;
	Logger log;

	// Use this for initialization
	void Start ()
	{
		main = helper.GetComponent<Main>();
		log = Logger.Instance;
		startExperiment.onClick.AddListener(() => { main.startExperimentPressed(); });
		prename.onEndEdit.AddListener(preListener);
		surname.onEndEdit.AddListener (surListener);
		age.onEndEdit.AddListener (ageListener);
		participantId.onEndEdit.AddListener (pIDListener);
		male.onValueChanged.AddListener(maleListener);
		female.onValueChanged.AddListener(femaleListener);
	}


	void preListener(string s)
	{
		log.prename = prename.text;
	}


	void surListener(string s)
	{
		log.surname = surname.text;
	}

	void pIDListener(string s)
	{
		log.participantID = participantId.text;
	}

	void ageListener(string s)
	{
		uint a;
		if (uint.TryParse (age.text, out a)) {
			log.age = a;
		} 
		else 
		{
			age.text = "";
			age.placeholder.GetComponent<Text>().text = "Not a number";
		}
	}

	void maleListener(bool v)
	{
		if (v)
		{
			log.gender = Logger.genderEnum.MALE;
		}
		else
		{
			log.gender = Logger.genderEnum.FEMALE;
		}
	}

	void femaleListener(bool v)
	{
		if (v)
		{
			log.gender = Logger.genderEnum.FEMALE;
		}
		else
		{
			log.gender = Logger.genderEnum.MALE;
		}

	}

	public void objectHit(GameObject g)
	{
		EventSystem.current.SetSelectedGameObject(g);
		switch(g.name)
		{
		case "Start Experiment":
			main.startExperimentPressed();
			break;
		case "Female":
			female.isOn = true;
			male.isOn = false;
			femaleListener(true);
			break;
		case "Male":
			female.isOn = false;
			male.isOn = true;
			maleListener(true);
			break;
		}
	}

}
