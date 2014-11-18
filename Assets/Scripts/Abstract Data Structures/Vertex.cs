//------------------------------------------------------------------------------
/// <summary>
/// Abstract Vertex class.
/// 
/// Written by Bryce Summers, 11 - 16 - 2014.
/// </summary>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Vertex
	{
		// -- Private Data Fields.
		private int index = 0;
		private HashSet<Edge> edges = new HashSet<Edge> ();

		/// <summary>
		///  Default empty constructor.
		/// </summary>
		/// <param name="index">Index.</param>
		public Vertex (int index)
		{
			this.index = index;
		}

		/// <summary>
		/// Adds the edge.
		/// </summary>
		/// <returns><c>true</c>, if edge was added, <c>false</c> otherwise.</returns>
		/// <param name="e">E.</param>
		public bool addEdge(Edge e)
		{
			return edges.Add (e);
		}

		/// <summary>
		/// Removes the edge.
		/// </summary>
		/// <returns><c>true</c>, if edge was removed, <c>false</c> otherwise.</returns>
		/// <param name="e">E.</param>
		public bool removeEdge(Edge e)
		{
			return edges.Remove (e);
		}

		/// <summary>
		/// Retrieves the index of this vertex that is unique with regards to the graph that created it.
		/// </summary>
		/// <returns>The index.</returns>
		public int getIndex()
		{
			return index;
		}



		// -- Standard overrides such as equality checks and a properly implemented hash code.

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="AssemblyCSharp.Vertex"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="AssemblyCSharp.Vertex"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="AssemblyCSharp.Vertex"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals (object obj)
		{
			Vertex n = obj as Vertex;

			// Not a Vertex object.
			if (n == null)
			{
				return false;
			}
			
			return Equals (n);
		}

		/// <summary>
		/// Equality Check for Vertices. We can use the trivial reference equality check,
		/// because we should never create duplicate vertices that should be interpreted as the same.
		/// The propoper graph search function should be sufficient to find the exact copies of vertices that are needed.
		/// </summary>
		/// <param name="v">The <see cref="AssemblyCSharp.Vertex"/> to compare with the current <see cref="AssemblyCSharp.Vertex"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="AssemblyCSharp.Vertex"/> is equal to the current
		/// <see cref="AssemblyCSharp.Vertex"/>; otherwise, <c>false</c>.</returns>
		public bool Equals(Vertex v)
		{
			return this == v;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode()
		{
			return index;
		}

		// Allow iteration over this vertice's edge elements.
		public IEnumerable<Edge> getEdges()
		{
			return edges;
		}

		/// <summary>
		/// Retrieves an edge that connects this vertex to the input vertex.
		/// </summary>
		/// <returns>The edge that connects this vertex to the given vertex.</returns>
		/// <param name="v">V.</param>
		public Edge getEdgeTo(Vertex v)
		{
			foreach (Edge e in edges)
			{
				if(e.getOther(this) == v)
				{
					return e;
				}
			}

			throw new Exception("This vertex does not contain an edge to the given vertex.");
		}
	}
}

