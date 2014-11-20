using UnityEngine;
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


	G_Vertex vertex;
	public GameObject personPrefab;

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
		print (vertex);
	}

	BNeighbourhood RandomDestination()
	{
		int rand = Random.Range (0, player.GetAllNeighbourhoods ().Count-1);
		//print (player.GetAllNeighbourhoods().Count);
		List<BNeighbourhood> allHoods = player.GetAllNeighbourhoods();
		return allHoods[rand];
	}

	public void GeneratePopulation(G_Graph graph)
	{
		//Add Random number of people to this neighbourhood
		for(int i=0; i <1;i++)
		{
			GameObject personObject =  (GameObject)Instantiate (personPrefab,this.transform.position,Quaternion.identity);
			BPerson person = personObject.GetComponent<BPerson>();
			person.Spawn (this,RandomDestination(),graph);
		}
	}

	public G_Vertex GetVertex()
	{
		return vertex;
	}
}
