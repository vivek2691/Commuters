﻿using UnityEngine;
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
	float happinessFactor;
	
	float lastCommute;
	float averageCommute;
	float leaveHomeAt; //Variable that tells the NPC when to leave home for work
	float leaveWorkAt; //Variable that tells the NPC when to leave work for home
	
	bool outForWork = false;
	bool outForHome = false;
	bool canMove = false;

	int finalCost;


	BNeighbourhood home,destination,current;
	G_Person gPerson;

	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(PublicClock.clock.GetTime()==leaveHomeAt)
		{
			outForWork = true;
		}
		else if(PublicClock.clock.GetTime()==leaveWorkAt)
		{
			outForHome = true;
		}

		if(Input.GetKeyDown(KeyCode.M))
		{
			print (GetHappiness());
		}

		if(outForWork)
		{
			TravelToWork();
		}
		if(outForHome)
		{
			TravelToHome();
		}

	}

	public void Spawn(BNeighbourhood home,BNeighbourhood destination,G_Graph graph)
	{
		this.home = home;
		this.destination = destination;
		this.wealth = Random.Range (20, 50);
		this.health = Random.Range (70,90);
		this.happinessFactor = Random.Range (0f,1f);
		//print (happinessFactor);
		this.gPerson = graph.newPerson (home.GetVertex (), destination.GetVertex ());
		this.leaveHomeAt = Random.Range(PublicConstants.MIN_WAKEUP_TIME,PublicConstants.MAX_WAKEUP_TIME);
		this.leaveWorkAt = leaveHomeAt + Random.Range(PublicConstants.MIN_WORKING_HOURS,PublicConstants.MAX_WORKING_HOURS);
	}

	public G_Person GetGPerson()
	{
		return gPerson;
	}

	void TravelToWork()
	{
		int cost = gPerson.MoveTowards (destination.GetVertex(), wealth/2,false);
		if(cost!=-1)
		{
			finalCost = cost;
			transform.position = new Vector3(gPerson.getX(),transform.position.y,gPerson.getY());
		}
			
		else
		{
			outForWork = false;
			wealth -= finalCost;
			print (finalCost);
			lastCommute = gPerson.getCurrentCommutetime();
			wealth += (int)lastCommute;
		}
			
	}
	
	void TravelToHome()
	{
		int cost = gPerson.MoveTowards (home.GetVertex(),wealth,false);
		if(cost!=-1)
			transform.position = new Vector3(gPerson.getX(),transform.position.y,gPerson.getY());
		else
		{
			outForHome = false;
			lastCommute = gPerson.getCurrentCommutetime(); 
			OnIncome();
		}
			
	}

	void OnIncome()
	{

		//If person is wealth oriented
		if(happinessFactor<0.5f)
		{		
			if((home.IsRich() && wealth<PublicConstants.AVERAGE_WEALTH_RICH) ||
			   (!home.IsRich() && wealth<PublicConstants.AVERAGE_WEALTH_POOR))
			{
				OnGetMoreWealth();
			}
			else
			{
				OnGetMoreHealth();
			}
		}
		//If person is health oriented
		else
		{
			if((home.IsRich() && health<PublicConstants.AVERAGE_HEALTH_RICH) ||
			   (!home.IsRich() && health<PublicConstants.AVERAGE_HEALTH_POOR))
			{
				OnGetMoreHealth();
			}
			else
			{
				OnGetMoreWealth();
			}

		}
	}



	void OnGetMoreWealth()
	{
		if(!gPerson.hasVehicle(Vehicle.Car))
		{
			if(wealth/2 >= PublicConstants.COST_BUY_CAR)
			{
				gPerson.AddVehicle(Vehicle.Car);
				wealth -= PublicConstants.COST_BUY_CAR;
			}

		}
	}

	void OnGetMoreHealth()
	{
		if(!gPerson.hasVehicle(Vehicle.Bike))
		{
			if(wealth/2 >= PublicConstants.COST_BUY_BIKE)
			{
				gPerson.AddVehicle(Vehicle.Bike);
				wealth -= PublicConstants.COST_BUY_BIKE;
			}		
		}
	}

	public void ChooseModeOfTransport(){
		//Meaning Person wants to be healthy
		if(happinessFactor<0.5f){
			//Transport preference = health;
		}
		//Person wants to be wealthy
		else{
			//transport preference = wealth;
		}
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
		return (int)((wealth*happinessFactor)+(health*(1-happinessFactor)));
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


