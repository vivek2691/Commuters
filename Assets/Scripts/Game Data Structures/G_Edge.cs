//------------------------------------------------------------------------------
/// G_Edge Class.
/// Written by Bryce Summers,
/// 11 - 16 - 2014.
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections;

public class G_Edge : Edge
{
	// The set of people travelling along this Edge.
	// Each person has a weight field that represent their position percentage along an edge.
	private HashSet<G_Person> people = new HashSet<G_Person>();
	
	// -- Properties
	// Each Edge has a set of improvement properties.
	private HashSet<EdgeType> myImprovements = new HashSet<EdgeType> ();
	
	private double edge_length;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="AssemblyCSharp.G_Edge"/> class.
	/// </summary>
	/// <param name="v1">V1.</param>
	/// <param name="v2">V2.</param>
	/// <param name="index">Index.</param>
	public G_Edge (G_Vertex v1, G_Vertex v2, int index) : base(v1, v2, index)
	{
		
		if (v1 == v2)
		{
			throw new Exception("This game should not have edges with identical vertices!!!");
		}
		
		// Every edge starts out being unimproved.
		myImprovements.Add(EdgeType.Unimproved);// This may be superfluous, because it is implied.
		
		/*
			 * Compute the length of the edge using the Euclidean metric.
			 */
		edge_length = v1.distanceTo (v2);
	}
	
	
	/// <summary>
	/// Derives the location weight of a person standing at the given vertex.
	/// </summary>
	/// <returns>The weight.</returns>
	/// <param name="v">V. The vertex a person is currently on.</param>
	public double getWeight(G_Vertex v)
	{
		G_Vertex v1 = this.v1 as G_Vertex;
		G_Vertex v2 = this.v2 as G_Vertex;
		
		if(v == v1)
		{
			return 0.0;
		}
		
		if(v == v2)
		{
			return 1.0;
		}
		
		throw new Exception("The input vertex is not related to this G_Edge.");
	}
	
	/// <summary>
	/// Returns the length of this G_Edge using the Euclidean metric on the 
	/// displacement of the two edge endpoint vertices.
	/// </summary>
	/// <returns>The length.</returns>
	public double getLength()
	{
		return edge_length;
	}
	
	/// <summary>
	/// Allows edges to be upgraded and downgraded.
	/// </summary>
	/// <param name="myType">My type.</param>
	/// <returns> Returns true iff the input type was not already an improvment of this edge.</returns>
	public bool addImprovement(EdgeType type)
	{
		return myImprovements.Add(type);
	}

	/// <summary>
	/// Removes the input improvement.
	/// </summary>
	/// <returns><c>true</c> iff the given improvement was in this edge's improvement set and it was removed.</returns>
	/// <param name="type">Type.</param>
	public bool removeImprovement(EdgeType type)
	{
		return myImprovements.Remove (type);
	}
	
	/// <summary>
	/// Gets the type.
	/// </summary>
	/// <returns>The type.</returns>
	public HashSet<EdgeType> getImprovements()
	{
		return myImprovements;
	}

	/// <summary>
	/// Returns true iff the given edge has the requested improvement.
	/// </summary>
	/// <param name="e">The edgetype we are querying.</param>
	public bool hasImprovement(EdgeType e)
	{
		return myImprovements.Contains (e);
	}

	/// <summary>
	/// Adds a person to this Neighborhood.
	/// </summary>
	/// <returns><c>true</c>, if person was added, <c>false</c> otherwise (i.e. the person is already in the set.</returns>
	/// <param name="p">P.</param>
	public bool addPerson(G_Person p)
	{
		if(p == null)
		{
			throw new Exception("G_Vertex addPerson(): Null Person");
		}
		
		return people.Add (p);
	}
	
	/// <summary>
	/// Removes the person from this neighborhood.
	/// </summary>
	/// <returns><c>true</c>, if person was removed, <c>false</c> otherwise.</returns>
	/// <param name="p">P.</param>
	public bool removePerson(G_Person p)
	{
		if(p == null)
		{
			throw new Exception("G_Vertex removePerson(): Null person");
		}			
		
		return people.Remove (p);
	}

	public bool hasPerson(G_Person p)
	{
		return people.Contains (p);
	}

	public IEnumerable getPeople()
	{
		return people;
	}

}	
