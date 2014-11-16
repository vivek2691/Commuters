//------------------------------------------------------------------------------
/// Person Class. This represents a person in the game world.
/// Written by Bryce Summers, 11 - 16 - 2014.
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;


namespace AssemblyCSharp
{
	public class G_Person
	{
		// Represents the state of this person.
		// myState = Edge --> 
		private PersonState myState;

		// -- Vertice weights for when a person is travelling along an edge.
		// weight = 0 --> the person is at the first vertex of the edge.
		// weight = 1 --> the person is at the second vertex of the edge.
		private double weight;

		// Used if myState = Edge;
		private G_Edge myEdge;

		// Used if myState = Vertex;
		private G_Vertex myVertex;

		// Important Vertices in a person's life.
		private G_Vertex v_home, v_work;

		// -- Path finding and commuting information.
		// The current goal that this person is striving to get to.
		private G_Vertex v_current_goal = null;
		List<G_Vertex> path = null;

		public G_Person(G_Vertex v_home, G_Vertex v_work)
		{
			this.v_home = v_home;
			this.v_work = v_work;

			// Every person starts at home by default.
			myState  = PersonState.Vertex;
			myVertex = v_home;

			// Let the home vertex know this person is there.
			v_home.addPerson (this);
		}

		// -- Person movement functions.

		/// <summary>
		/// Teleports this person to the given vertex.
		/// </summary>
		/// <param name="v_new">V_new.</param>
		public void teleportToVertex(G_Vertex v_new)
		{
			if (myState == PersonState.Vertex)
			{
				myVertex.removePerson(this);
				v_new.addPerson(this);
				return;
			}

			if(myState == PersonState.Edge)
			{
				myEdge.removePerson(this);
				myState = PersonState.Vertex;
				v_new.addPerson (this);
			}
		}

		/// <summary>
		/// Instructs the person to move at maximum speed towards the given vertice,
		/// using shortest path graph algorithms.
		/// </summary>
		/// <param name="v_new">V_new.</param>
		/// <returns><c>true</c>, if a path exists and the person was moved, 
		/// <c>false</c> if no path could be found and the person has not been moved.</returns>
		public bool MoveTowards(G_Vertex v_new)
		{
			// Compute a new shortest path if the person is given a new destination.
			if(v_new != v_current_goal)
			{
				computeShortestPath(v_new);
			}

			throw new Exception ("Not yet implemented!");
		}

		/// <summary>
		/// Computes a shortest path from scratch using a custom A*STAR search function.
		/// </summary>
		/// <param name="goal">Goal.</param>
		private void computeShortestPath(G_Vertex goal)
		{
			// Setup the internal information functions.
			this.v_current_goal = goal;
			path = new List<G_Vertex> ();

			// Rev up the gool ol' Priority Queue engine.
			MinHeap<Priority_Node<G_Vertex>> PQ = new MinHeap<Priority_Node<G_Vertex>>();

			// Add the initial states to the PQ.
			if (myState == PersonState.Vertex)
			{
				G_Vertex v = myVertex;
				double distance = v.distanceTo (goal);
				Priority_Node<G_Vertex> node = new Priority_Node<G_Vertex> (v, distance);
				PQ.Add (node);
			}
			else if(myState == PersonState.Edge)// The player starts in the middle of an edge.
			{
				List<Vertex> verts = myEdge.getVerticeList();
				foreach(Vertex v_abstract in verts)
				{
					G_Vertex v = v_abstract as G_Vertex;
					double distance = v.distanceTo (getX(), getY()) + v.distanceTo (goal);
					Priority_Node<G_Vertex> node = new Priority_Node<G_Vertex> (v, distance);
					PQ.Add (node);
				}
			}
			else
			{
				throw new Exception("This case is not yet supported: " + myState);
			}

			// Now perform the ASTAR Search.
			HashSet<G_Vertex> Visited = new HashSet<G_Vertex>();

			while (PQ.Count > 0)
			{
				throw new Exception("A STAR Search not yet implemented.");
			}

		}

		/// <summary>
		/// Gets the x coordinate of this person.
		/// </summary>
		/// <returns>The x.</returns>
		public int getX()
		{
			switch(myState)
			{
				case PersonState.Vertex:
					return myVertex.getX();
				case PersonState.Edge:
					G_Vertex v1 = myEdge.getV1() as G_Vertex;
					G_Vertex v2 = myEdge.getV2() as G_Vertex;
					int x1 = v1.getX();
					int x2 = v2.getX();
				return (int)(x1*(1 - weight) + x2*weight);
			}

			throw new Exception("We should never get here.");
		}

		/// <summary>
		/// Gets the y coordinate of this person.
		/// </summary>
		/// <returns>The y.</returns>
		public int getY()
		{
			switch(myState)
			{
			case PersonState.Vertex:
				return myVertex.getX();
			case PersonState.Edge:
				G_Vertex v1 = myEdge.getV1() as G_Vertex;
				G_Vertex v2 = myEdge.getV2() as G_Vertex;
				int y1 = v1.getY();
				int y2 = v2.getY();
				return (int)(y1*(1 - weight) + y2*weight);
			}
			
			throw new Exception("We should never get here.");
		}

	}
}