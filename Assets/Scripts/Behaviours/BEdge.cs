using UnityEngine;
using System.Collections;


/// <summary>
/// Behaviour equivalent of Bryce's G_Edge class
/// Author : Vivek Kotecha
/// </summary>
public class BEdge : MonoBehaviour {

	public BNeighbourhood vertex1,vertex2;

	public G_Edge gEdge;

	public BPlayer player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Spawn(G_Graph graph,BPlayer player)
	{
		if(vertex1.GetVertex () == null || vertex2.GetVertex () == null)
			print("A vertex is null for this edge");
		this.gEdge = graph.newEdge(vertex1.GetVertex(),vertex2.GetVertex());
		this.player = player;
//		print (gEdge);

	}

	public void AddUpgrade(EdgeType type)
	{
		switch(type)
		{
		case EdgeType.Footpath : player.money -= PublicConstants.COST_BUY_FOOTPATH;
			break;
		case EdgeType.Biking_Trail : player.money -= PublicConstants.COST_BUY_BIKETRAIL;
			break;
		case EdgeType.Road : player.money -= PublicConstants.COST_BUY_ROAD;
			break;
		case EdgeType.Boulevard : player.money -= PublicConstants.COST_BUY_BOULEVARD;
			break;
		case EdgeType.Rail : player.money -= PublicConstants.COST_BUY_RAIL;
			break;
		default : break;
		}
		gEdge.addImprovement (type);
	}

	public bool IsVertex(BNeighbourhood vertex)
	{
		return (vertex == vertex1 || vertex == vertex2);
	}


}
