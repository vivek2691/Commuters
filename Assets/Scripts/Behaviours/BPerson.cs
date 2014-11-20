using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// This class controls the behaviour and statistics of each NPC. This is my equivalen of Bryce's G_Person class
/// Author : Vivek Kotecha
/// </summary>
public class BPerson : MonoBehaviour {
	
	int wealth=1000;
	int health;
	int happiness;
	
	float lastCommute;
	float averageCommute;
	float leaveHomeAt; //Variable that tells the NPC when to leave home for work
	float leaveWorkAt; //Variable that tells the NPC when to leave work for home
	
	bool outForWork = false;
	bool canMove = false;


	BNeighbourhood home,destination,current;
	G_Person gPerson;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(leaveHomeAt == PublicClock.clock.GetTime() && !outForWork){
			TravelToWork();
		}
		*/

		if(Input.GetKeyDown(KeyCode.M))
		{
			TravelToWork();
			transform.position = new Vector3(gPerson.getX(),transform.position.y,gPerson.getY());
		}

	}

	public void Spawn(BNeighbourhood home,BNeighbourhood destination,G_Graph graph)
	{
		this.home = home;
		this.destination = destination;
		print (home.GetVertex() + " " + destination.GetVertex());
		this.gPerson = graph.newPerson (home.GetVertex (), destination.GetVertex ());
		//print (gPerson);
		this.leaveHomeAt = Random.Range(PublicConstants.MIN_WAKEUP_TIME,PublicConstants.MAX_WAKEUP_TIME);
		this.leaveWorkAt = leaveHomeAt + Random.Range(PublicConstants.MIN_WORKING_HOURS,PublicConstants.MAX_WORKING_HOURS);

	}

	public G_Person GetGPerson()
	{
		return gPerson;
	}

	void TravelToWork()
	{
		print ("Travel TO Work");
		outForWork = true;
		canMove = true;
		//print (destination);
		gPerson.MoveTowards (destination.GetVertex(), wealth);

	}
	
	void TravelToHome()
	{
		outForWork = false;
	}

	public int GetWealth()
	{
		return wealth;
	}

	public int GetHealth()
	{
		return health;
	}

	public int GetHappiness()
	{
		return happiness;
	}
	public float GetLastCommute()
	{
		return lastCommute;
	}
	public float GetAvgCommute()
	{
		return averageCommute;
	}
	public bool hasVehicle(Vehicle v)
	{
		return this.gPerson.hasVehicle (v);
	}




}


