﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Bryce's Test script.
/// Written by Bryce Summers on 11 - 18 - 2014.
/// Purpose : This script is intended to test the behavior of Bryce's data structures.
/// </summary>

public class BryceTestScript : MonoBehaviour {

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
		Debug.Log (G);

		G_Vertex v0 = G.newVertex (0, 0);
		Debug.Log (v0);

		G_Vertex v1 = G.newVertex (100, 0);
		Debug.Log (v1);

		G_Vertex v2 = G.newVertex (100, 100);
		Debug.Log (v2);

		G_Edge e1 = G.newEdge (v0, v1);
		Debug.Log (e1);

		G_Edge e2 = G.newEdge (v1, v2);
		Debug.Log (e2);

		G_Edge e3 = G.newEdge (v0, v2);
		Debug.Log (e3);

		Debug.Log (G);

		// -- Now create a person.
		G_Person p = G.newPerson (v0, v1);
		Debug.Log (p);

		for(int i = 0; i < 20; i++)
		{
			p.MoveTowards (v1, 0);
			Debug.Log (p);
		}
	}
}
