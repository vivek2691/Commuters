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
		OnStartGame ();
	}
	
	// Update is called once per frame
	void Update () {


		//print (money);
	}

	bool CheckWin()
	{
		if(GetGlobalAverageHappiness()>=5000 && PublicClock.clock.GetDays()<15)
		{
			return true;
		}
		else 
		{
			return false;
		}
	}

	public bool CheckLose()
	{
		if(PublicClock.clock.GetDays()>=15)
		{
			if(GetGlobalAverageHappiness()<5000)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
			return false;
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
			edge.Spawn(gGraph,this);
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

	public int GetGlobalAverageHappiness()
	{
		int result = 0;
		foreach(BNeighbourhood hood in allHoods)
		{
			result+= hood.GetAverageHappiness();
		}

		if(allHoods.Count!=0)
			return result/allHoods.Count;
		else
			return 0;

	}


}
