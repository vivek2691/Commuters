﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Neighbourhood Class. Represents a neighbourhood in the game.
/// Keeps track of all residents of that neighbourhood.
/// Equivalent of Bryce's Vertex class
/// Author : Vivek Kotecha
/// </summary>
/// 
public class BNeighbourhood : MonoBehaviour {

	public GameObject personPrefab;

	G_Vertex vertex;
	List<BPerson> residents = new List<BPerson> ();
	BPlayer player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Spawn(Vector3 position,BPlayer player,G_Graph graph)
	{
		this.player = player;
		this.vertex = graph.newVertex (gameObject.transform.position.x, gameObject.transform.position.z);
	}

	BNeighbourhood RandomDestination()
	{	
		int rand=0;
		rand = Random.Range (0, player.GetAllNeighbourhoods ().Count);
		while(rand== player.GetAllNeighbourhoods().IndexOf(this))
		{
			rand = Random.Range(0,player.GetAllNeighbourhoods().Count);
		}
		List<BNeighbourhood> allHoods = player.GetAllNeighbourhoods();
		return allHoods[rand];
	}

	public void GeneratePopulation(G_Graph graph)
	{
		//Add Random number of people to this neighbourhood
		for(int i=0; i <Random.Range(3,6);i++)
		{
			GameObject personObject =  (GameObject)Instantiate (personPrefab,this.transform.position,Quaternion.identity);
			BPerson person = personObject.GetComponent<BPerson>();
			person.Spawn (this,RandomDestination(),graph);
			residents.Add(person);
		}
	}

	public G_Vertex GetVertex()
	{
		return vertex;
	}

	public int GetAverageWealth()
	{
		return GetTotalWealth () / residents.Count;

	}
	public int GetTotalWealth()
	{
		int result = 0;
		foreach(BPerson resident in residents)
		{
			result += resident.GetWealth();
		}
		return result;
	}

	public int GetTotalHealth()
	{
		int result = 0;
		foreach(BPerson resident in residents)
		{
			result += resident.GetHealth();
		}
		return result;
	}
	public int GetAverageHealth()
	{
		return GetTotalHealth()/residents.Count;
	}
	public int GetTotalHappines()
	{
		int result = 0;
		foreach(BPerson resident in residents)
		{
			result += resident.GetHappiness();
		}
		return result;
	}
	public int GetAverageHappiness()
	{
		return GetTotalHappines()/residents.Count;
	}
}
