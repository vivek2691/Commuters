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

		// Builtin UBA's do not have generaic type specifications in C SHARP.
		ArrayList vertices = new ArrayList(); // UBA<Vertex>.
		ArrayList edges    = new ArrayList(); // UBA<Edge>.

		int vert_index = 0;
		int edge_index = 0;

		Vertex newVertex()
		{
			int index = vert_index++;
			Vertex v  = new Vertex (index);
			vertices.Add (v);
			return v;
		}

		/// <summary>
		/// 		/// </summary>
		/// <returns>The edge.</returns>
		/// <param name="vertex_index_1">Vertex_index_1.</param>
		/// <param name="vertex_index_2">Vertex_index_2.</param>
		int newEdge(Vertex v1, Vertex v2)
		{
			// Construct the edge.
			int index = edge_index++;
			Edge e = new Edge (v1, v2, index);
			edges.Add (e);

			// Update the states of the two Vertices.
			v1.addEdge (e);
			v2.addEdge (e);

			return e;
		}

	}

}

