using UnityEngine;
using System.Collections;
using System.IO;


public sealed class Logger
{
	// singleton
	private static readonly Logger instance = new Logger();
	
	// Explicit static constructor to tell C# compiler
	// not to mark type as beforefieldinit
	static Logger()
	{
	}
	
	private Logger()
	{
		//ONLY DEBUG
	//	this.CreateLogFile();
	}
	
	public static Logger Instance
	{
		get
		{
			return instance;
		}
	}

	public string filepath = "";//Application.absoluteURL;
	public string filename;
	public string surname = "NA";
	public string prename = "NA";
	public string participantID = "NA";
	public enum genderEnum{
		FEMALE,
		MALE,
		NA
	};
	public genderEnum gender = genderEnum.NA;
	public GameObject helper;
	public uint age;

	private static StreamWriter sw;

	public void CreateLogFile()
	{
		filename = "ReachOut3D " + participantID + "_" + System.DateTime.Now.ToString("dd_MM_yyyy__HH_mm_ss") + ".txt";
		sw = new StreamWriter(filename);
		string s = "ReachOut 3D Experiment\nSurname: " + surname + "\nPrename: " + prename + "\nAge: " + age + "" +
			"\nGender: " + gender + "\nparticipantID: " + participantID
		+ "\nDateTime: " + System.DateTime.Now + "\n\n";
		this.Write(s);
	}

	public void CloseLogFile()
	{
		Debug.Log("log closed");
		sw.Close();
	}

	public void Write(string s)
	{
		sw.Write(s);
		sw.Flush();
	}

}
