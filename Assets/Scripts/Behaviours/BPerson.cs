using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// This class controls the behaviour and statistics of each NPC. This is my equivalen of Bryce's G_Person class
/// Author : Vivek Kotecha
/// </summary>
public class BPerson : MonoBehaviour {
	
	int wealth;
	int health;
	int happiness;
	
	float lastCommute;
	float averageCommute;


	BNeighbourhood home,destination,current;
	G_Person gPerson;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Spawn(BNeighbourhood home,BNeighbourhood destination)
	{
		this.home = home;
		this.destination = destination;
		this.gPerson = new G_Person (this.home.GetVertex (),this.destination.GetVertex(),0);
		//this.destination = 

	}

	public G_Person GetGPerson()
	{
		return gPerson;
	}

	void Travel()
	{

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


