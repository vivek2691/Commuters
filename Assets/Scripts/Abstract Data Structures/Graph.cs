//------------------------------------------------------------------------------
/// <summary>
/// Bryce Graph Class.
/// 
/// Written by Bryce Summers on 11 - 16 - 2014.
/// 
/// This class controls the representation of undirected graphs where every vertice and every edge is represented as a separate object.
/// 
/// The Graph, Vertex, and Edge components should be overriden with application specific objects.
/// This allows specific representations to be joined by this abstract code.
/// </summary>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections;

namespace AssemblyCSharp
{
	public class Graph
	{
		// -- Data Fields.

		// Builtin UBA's do not have generic type specifications in C SHARP.
		HashSet<Vertex> vertices = new HashSet<Vertex>(); // UBA<Vertex>.
		HashSet<Edge> edges    = new HashSet<Edge>(); // UBA<Edge>.

		private int vert_index = 0;
		private int edge_index = 0;

		// FIXME : Use a dictionary.
		protected bool addVertex(Vertex v)
		{
			return vertices.Add (v);
		}

		protected int nextVertIndex()
		{
			return vert_index++;
		}

		/// <summary>
		/// Removes the given vertex from this graph.
		/// </summary>
		/// <param name="v">v is the vertex to be removed.</param>
		public void removeVertex(Vertex v)
		{
			throw new Exception ("Not yet implemented, I need to write some dictionary augmented searching code.");
		}

		/// <summary>
		/// Returns an iterator for all of the vertices.
		/// </summary>
		/// <returns>The vertices.</returns>
		public IEnumerable<Vertex> getVertices()
		{
			return vertices;
		}

		/// <summary>
		/// Constructs a new edge.
		/// </summary>
		/// <returns>The edge.</returns>
		/// <param name="vertex_index_1">Vertex_index_1.</param>
		/// <param name="vertex_index_2">Vertex_index_2.</param>
		// FIXME : Use a dictionary.
		protected bool addEdge(Edge e)
		{
			if (edges.Add (e) == false)
			{
				return false;
			}

			// Update the states of the two Vertices.
			e.getV1().addEdge (e);
			e.getV2().addEdge (e);

			return true;
		}

		protected int nextEdgeIndex()
		{
			return edge_index++;
		}





		/// <summary>
		/// Removes the given edge from this graph.
		/// </summary>
		/// <param name="e">E.</param>
		public void removeEdge(Edge e)
		{
			throw new Exception ("Not yet implemented, I need to write some dictionary augmented searching code.");
		}


		public IEnumerable<Edge> getEdges()
		{
			return edges;
		}


	}

}

