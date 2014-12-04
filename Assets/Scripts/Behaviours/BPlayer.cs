using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BPlayer : MonoBehaviour {

	List<BNeighbourhood> allHoods;
	List<BEdge> allEdges;
	public G_Graph gGraph;
	public int money = 100;

	// Use this for initialization
	void Start () {
		gGraph = new G_Graph ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S))
		{
			OnStartGame();
		}

		print (money);
	}

	void OnStartGame()
	{

		RegisterHoods ();
		RegisterEdges ();
	}

	void RegisterEdges()
	{
		allEdges = new List<BEdge>();
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Edge");
		foreach(GameObject obj in objects)
		{
			BEdge edge = obj.GetComponent<BEdge>();
			edge.Spawn(gGraph);
		}

	}

	void RegisterHoods()
	{
	
		allHoods = new List<BNeighbourhood>();
		GameObject [] objects = GameObject.FindGameObjectsWithTag ("Neighbourhood");
		foreach(GameObject obj in objects)
		{
			BNeighbourhood hood = obj.GetComponent<BNeighbourhood>();
			if(hood==null)
			{
				print ("No BNeighbourhood script attached to this GameObject");
			}
			else
			{
				hood.Spawn(obj.transform.position,this,gGraph);
				allHoods.Add(hood);
			}
		}	
		//print (allHoods);
		GeneratePopulation ();

	}

	void GeneratePopulation()
	{
		foreach(BNeighbourhood hood in allHoods)
		{
			hood.GeneratePopulation(gGraph);
		}
	}

	public List<BNeighbourhood> GetAllNeighbourhoods()
	{
		return allHoods;
	}

	public List<BEdge> GetAllEdges()
	{
		return allEdges;
	}


}
