using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

/// <summary>
/// Neighbourhood Class. Represents a neighbourhood in the game.
/// Keeps track of all residents of that neighbourhood.
/// Equivalent of Bryce's Vertex class
/// Author : Vivek Kotecha
/// </summary>
/// 
public class BNeighbourhood : MonoBehaviour {


	AssemblyCSharp.G_Vertex vertex;
	public GameObject personPrefab;

	BPlayer player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Spawn(Vector3 position,BPlayer player)
	{
		this.player = player;
		this.vertex = new G_Vertex (0);
		this.vertex.setPosition ((int)position.x,(int)position.z);

	}

	BNeighbourhood RandomDestination()
	{
		int rand = Random.Range (0, player.GetAllNeighbourhoods ().Count-1);
		//print (player.GetAllNeighbourhoods().Count);
		List<BNeighbourhood> allHoods = player.GetAllNeighbourhoods();
		return allHoods[rand];
	}

	public void GeneratePopulation()
	{
		GameObject personObject =  (GameObject)Instantiate (personPrefab,this.transform.position,Quaternion.identity);
		BPerson person = personObject.GetComponent<BPerson>();
		person.Spawn (this,RandomDestination());
		vertex.addPerson (person.GetGPerson());
	}

	public G_Vertex GetVertex()
	{
		return vertex;
	}
}
