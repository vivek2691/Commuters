using UnityEngine;
using System.Collections;
using AssemblyCSharp;

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

	public void Spawn()
	{
		this.gEdge = new G_Edge(vertex1.GetVertex(),vertex2.GetVertex (),0);
	}

	public bool IsVertex(BNeighbourhood vertex)
	{
		return (vertex == vertex1 || vertex == vertex2);
	}


}
