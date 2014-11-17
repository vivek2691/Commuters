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
/// 
/// FIXME : Add the genaric abstract vertice and edge creation capabilities back in, although they will not need to be used in the commuters project.
/// 
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
		HashSet<Edge>   edges    = new HashSet<Edge>(); // UBA<Edge>.

		private int vert_index = 0;
		private int edge_index = 0;


		// --  Abstract functions that should be overriden in sub classes.

		/// <summary>
		/// Constructs and returns a new Vertex.
		/// </summary>
		/// <returns>The vertex.</returns>
		public Vertex newVertex()
		{
			Vertex v  = new Vertex (nextVertIndex());
			addVertex (v);
			return v;
		}

		/// <summary>
		/// 		/// </summary>
		/// <returns>The edge.</returns>
		/// <param name="vertex_index_1">Vertex_index_1.</param>
		/// <param name="vertex_index_2">Vertex_index_2.</param>
		Edge newEdge(Vertex v1, Vertex v2)
		{
			Edge e = new Edge (v1, v2, nextEdgeIndex ());
			addEdge (e);
			return e;
		}


		// -- The various helper functions.



		/// <summary>
		/// Adds the vertex to this graph.
		/// </summary>
		/// <returns><c>true</c>, if vertex was added, <c>false</c> otherwise.</returns>
		/// <param name="v">V.</param>
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
		public bool removeVertex(Vertex v)
		{
			if (!vertices.Contains (v))
			{
				return false;
			}

			// -- First remove all of the edges.
			IEnumerable<Edge> edges = v.getEdges ();

			foreach (Edge e in v.getEdges())
			{
				removeEdge(e);
			}

			// Now remove the vertex.
			vertices.Remove (v);

			return true;
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

		/// <summary>
		/// Returns a unique ID for a newly created edge. this should be used by subclasses.
		/// </summary>
		/// <returns>The edge index.</returns>
		protected int nextEdgeIndex()
		{
			return edge_index++;
		}


		/// <summary>
		/// Removes the given edge from the graph.
		/// </summary>
		/// <returns><c>true</c>, if edge was removed, <c>false</c> otherwise.</returns>
		/// <param name="e">E.</param>
		public bool removeEdge(Edge e)
		{
			if (edges.Remove (e) == false)
			{
				return false;
			}

			e.getV1().removeEdge (e);
			e.getV2().removeEdge (e);

			return true;
		}

		/// <summary>
		/// Returns an iterator for the set of all edges in this graph.
		/// </summary>
		/// <returns>The edges.</returns>
		public IEnumerable<Edge> getEdges()
		{
			return edges;
		}


	}

}

