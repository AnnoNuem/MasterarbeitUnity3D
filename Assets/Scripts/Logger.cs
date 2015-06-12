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
using System.IO;
using System;

/// <summary>
/// Logger. Singleton with handles log file and writes data from other classses into the log file.
/// </summary>
public sealed class Logger
{
	// singleton functions
	private static readonly Logger instance = new Logger();
	
	static Logger()
	{
	}
	
	private Logger()
	{
	}
	
	public static Logger Instance
	{
		get
		{
			return instance;
		}
	}

	public string filepath = "Results/";
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

		try 
		{
			//creates only directory if not already exists
			Directory.CreateDirectory(filepath);

		} 
		catch (Exception e) 
		{
			Debug.Log (e);
		} 

		filename = "ReachOut3D " + participantID + "_" + System.DateTime.Now.ToString("dd_MM_yyyy__HH_mm_ss") + ".txt";
		sw = new StreamWriter(filepath + filename);
		string s = "ReachOut 3D Experiment\nSurname: " + surname + "\nPrename: " + prename + "\nAge: " + age + "" +
			"\nGender: " + gender + "\nparticipantID: " + participantID
		+ "\nDateTime: " + System.DateTime.Now + "\n\n";
		this.Write(s);
	}

	public void CloseLogFile()
	{
		sw.Close();
		Debug.Log("log closed");
	}

	public void Write(string s)
	{
		sw.Write(s);
		sw.Flush();
	}

}
