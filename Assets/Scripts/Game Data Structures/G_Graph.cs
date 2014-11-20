/// <summary>
/// G_Graph class.
/// Written by Bryce Summers on 11 - 16 - 2014.
/// 
/// Purpose : This overrides the abstract "Graph" class to provide a graph tailor made for the commuters game.
/// 
/// Please look at the the Abstract "Graph" class for more public methods that this class inherits,
/// such as vertex/edge deletion.
/// </summary>
using UnityEngine;
using System.Collections;

using System.Collections.Generic;


public class G_Graph : Graph
{

	// -- Data Structures.
	private HashSet<G_Person> people = new HashSet<G_Person>();
	private int person_index = 0;

	/// <summary>
	/// Constructs and returns a new G_vertice.
	/// </summary>
	/// <returns>The vertex.</returns>
	public G_Vertex newVertex(float x, float y)
	{
		G_Vertex v  = new G_Vertex (nextVertIndex());

		// Give the vertex the correct data.
		v.setPosition ((double)x, (double)y);

		addVertex (v);
		return v;
	}
	
	/// <summary>
	/// 		/// </summary>
	/// <returns>The edge.</returns>
	/// <param name="vertex_index_1">Vertex_index_1.</param>
	/// <param name="vertex_index_2">Vertex_index_2.</param>
	public G_Edge newEdge(G_Vertex v1, G_Vertex v2)
	{
		G_Edge e = new G_Edge (v1, v2, nextEdgeIndex ());
		addEdge (e);
		return e;
	}

	/// <summary>
	/// Constructs a new person and adds them to the graph.
	/// The person starts at home by default.
	/// </summary>
	/// <returns>The person.</returns>
	/// <param name="home">Home.</param>
	/// <param name="work">Work.</param>
	public G_Person newPerson(G_Vertex home, G_Vertex work)
	{
		int index = nextPersonIndex ();
		G_Person p = new G_Person (home, work, index);

		people.Add(p);

		return p;
	}

	protected int nextPersonIndex()
	{
		return person_index++;
	}

	/// <summary>
	/// Removes the person from the graph.
	/// </summary>
	/// <returns><c>true</c>, if person was removed, <c>false</c> otherwise.</returns>
	/// <param name="p">p is the person who will be removed from the graph.</param>
	public bool removePerson(G_Person p)
	{
		return people.Remove (p);
	}

	/// <summary>
	/// Returns an iterator to the set of people in this graph.
	/// </summary>
	/// <returns>An iterator to the set of all people in the graph.</returns>
	public IEnumerable<G_Person> getPeople()
	{
		return people;
	}
}
