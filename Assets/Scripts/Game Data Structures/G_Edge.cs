//------------------------------------------------------------------------------
/// G_Edge Class.
/// Written by Bryce Summers,
/// 11 - 16 - 2014.
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections;

namespace AssemblyCSharp
{
	public class G_Edge : Edge
	{
		// The set of people travelling along this Edge.
		// Each person has a weight field that represent their position percentage along an edge.
		private HashSet<G_Person> people;

		// -- Properties
		// FIXME : We may want to improve this to be a set of trails, instead of only having one type.
		// All edges start out being unimproved.
		private EdgeType myType = EdgeType.Unimproved;

		private double edge_length;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyCSharp.G_Edge"/> class.
		/// </summary>
		/// <param name="v1">V1.</param>
		/// <param name="v2">V2.</param>
		/// <param name="index">Index.</param>
		public G_Edge (G_Vertex v1, G_Vertex v2, int index) : base(v1, v2, index)
		{

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
			G_Vertex v1 = this.v1;
			G_Vertex v2 = this.v2;

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
		public void setEdgeType(EdgeType type)
		{
			this.myType = type;
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <returns>The type.</returns>
		public EdgeType getType()
		{
			return myType;
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
				return people.Remove (p);
			}			
		}
	}
}