using UnityEngine;
using System.Collections;

/// <summary>
/// Bryce's Test script.
/// Written by Bryce Summers on 11 - 18 - 2014.
/// Purpose : This script is intended to test the behavior of Bryce's data structures.
/// </summary>
using System;

public class BryceTestScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		testCode ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void testCode()
	{
		// Test the creation of Graph entities.
		G_Graph G = new G_Graph ();
		ASSERT (G.getNumVertices () == 0);
		ASSERT (G.getNumEdges () == 0);

		G_Vertex v0 = G.newVertex (18, 0);
		ASSERT (G.getNumVertices () == 1);
		ASSERT (G.getNumEdges () == 0);
		ASSERT (v0.GetHashCode() == 0);

		G_Vertex v1 = G.newVertex (-17, 0);
		ASSERT (G.getNumVertices () == 2);
		ASSERT (G.getNumEdges () == 0);
		ASSERT (v1.GetHashCode() == 1);

		G_Vertex v2 = G.newVertex (100, 100);
		ASSERT (G.getNumVertices () == 3);
		ASSERT (G.getNumEdges () == 0);
		ASSERT (v2.GetHashCode() == 2);

		G_Edge e1 = G.newEdge (v0, v1);
		ASSERT (G.getNumVertices () == 3);
		ASSERT (G.getNumEdges () == 1);
		ASSERT (e1.getV1() == v0 && e1.getV2 () == v1 && e1.getOther(v0) == v1 &&
		        e1.getOther(v1) == v0 && e1.getVerticeIndex(v0) == 1 && e1.getVerticeIndex(v1) == 2 && e1.getVerticeIndex(v2) == -1 && 
		        e1.getVertex(1) == v0 && e1.getVertex (2) == v1 && e1.getVertex (0) == null && e1.getVertex (3) == null);

		e1.addImprovement (EdgeType.Rail);
		ASSERT (e1.hasImprovement(EdgeType.Rail));
		e1.removeImprovement (EdgeType.Rail);
		ASSERT (!e1.hasImprovement(EdgeType.Rail));
		ASSERT(!e1.removeImprovement (EdgeType.Rail));

		v0.train_stop = true;
		//v1.train_stop = true;

		G_Edge e2 = G.newEdge (v1, v2);
		Debug.Log (e2);

		G_Edge e3 = G.newEdge (v0, v2);
		Debug.Log (e3);

		Debug.Log (G);

		// -- Now create a person.
		G_Person p = G.newPerson (v0, v1);
		Debug.Log (p);

		p.MoveTowards (v1, 20);
		Debug.Log (p);

		for(int i = 0; i < 100; i++)
		{
			ASSERT(e1.hasPerson (p));
			p.MoveTowards (v1, 20);
			Debug.Log (p);
		}

		ASSERT(!e1.hasPerson (p));

		Debug.Log ("Player should now be at v1!");
		Debug.Log (p);
	}

	public void ASSERT(bool b)
	{
		if(!b)
		{
			throw new Exception("Assertion Failed!");
		}
	}
}
