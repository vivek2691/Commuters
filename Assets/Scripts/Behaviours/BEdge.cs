using UnityEngine;
using System.Collections;


/// <summary>
/// Behaviour equivalent of Bryce's G_Edge class
/// Author : Vivek Kotecha
/// </summary>
public class BEdge : MonoBehaviour {

	public BNeighbourhood vertex1,vertex2;

	public G_Edge gEdge;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Spawn(G_Graph graph)
	{
		if(vertex1.GetVertex () == null || vertex2.GetVertex () == null)
			print("A vertex is null for this edge");
		this.gEdge = graph.newEdge(vertex1.GetVertex(),vertex2.GetVertex());
//		print (gEdge);

	}

	public bool IsVertex(BNeighbourhood vertex)
	{
		return (vertex == vertex1 || vertex == vertex2);
	}


}
