//------------------------------------------------------------------------------
/// Person Class. This represents a person in the game world.
/// Written by Bryce Summers, 11 - 16 - 2014.
/// 
/// 
/// FIXME : I should handle a player carrying / storing their vehicles at their home.
/// FIXME : Think of better heuristics, because the Euclidean heursitic is not a very tight lower bound on the time it will take to travel.
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;


namespace AssemblyCSharp
{
	public class G_Person
	{
		// Represents the state of this person.
		// myState = Edge --> this person is traveling along an edge right now.
		// myState = Vertex --> This person is inside of a city right now.
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
		private Stack<G_Vertex> path = null;
		private int path_money_cost = 0;

		private HashSet<Vehicle> vehicles = new HashSet<Vehicle> ();

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
		/// 
		/// WARNING : This function should not be called with differing G_Vertices willy nilly,
		/// because then it will compute the shortest path each frame. Please try to maintain a consistent destination.
		/// 
		/// WARNING : This returns the shortest path assuming the player can pay all tolls.
		/// 
		/// </summary>
		/// <param name="v_new"> The destination vertice this person should be moving to.
		/// <param name="money"> The total amount of money this person will be willing to spend on this trip.
		/// <returns><c>true</c>, if a path exists that costs less than the input money allowance and the person was moved, 
		/// <c>false</c> if no path could be found and the person has not been moved.</returns>
		public bool MoveTowards(G_Vertex v_new, int money)
		{
			// Compute a new shortest path if the person is given a new destination.
			if(v_new != v_current_goal || money < path_money_cost)
			{
				if(computeShortestPath(v_new, money) == false)
				{
					return false;
				}
			}

			// Movement code.
			throw new Exception ("Not yet implemented!");
		}

		/// <summary>
		/// Computes a shortest path from scratch using a custom A*STAR search function.
		/// 
		/// WARNING : This algorithm assumes that the player is always willing to pay more to travel faster.
		/// Because of this design choice, it is possible for people to spend money on short edges instead of longer edges.
		/// 
		/// 
		/// </summary>
		/// <param name="goal">Goal.</param>
		/// <returns> Returns true iff  a path with cost less than the given money amount
		/// was found and loaded into this G_Person.</returns>
		/// 
		/// 
		private bool computeShortestPath(G_Vertex goal, int max_money_cost)
		{
			// -- Initialization functions.

			// Rev up the gool ol' Priority Queue engine.
			MinHeap<Priority_Node<G_Vertex>> PQ = new MinHeap<Priority_Node<G_Vertex>>();

			// Add the initial states to the PQ.
			if (myState == PersonState.Vertex)
			{
				G_Vertex v = myVertex;
				double distance = v.distanceTo (goal);
				Priority_Node<G_Vertex> node = new Priority_Node<G_Vertex> (v, max_money_cost, distance);
				PQ.Add (node);
			}
			else if(myState == PersonState.Edge)// The player starts in the middle of an edge.
			{
				List<Vertex> verts = myEdge.getVerticeList();
				foreach(Vertex v_abstract in verts)
				{
					G_Vertex v = v_abstract as G_Vertex;
					double distance = v.distanceTo (getX(), getY()) + v.distanceTo (goal);
					Priority_Node<G_Vertex> node = new Priority_Node<G_Vertex> (v, max_money_cost, distance);
					PQ.Add (node);
				}
			}
			else
			{
				throw new Exception("This case is not yet supported: " + myState);
			}

			// -- Special time + money based ASTAR Search.

			HashSet<G_Vertex> Visited = new HashSet<G_Vertex>();

			while (PQ.Count > 0)
			{
				Priority_Node<G_Vertex> node = PQ.ExtractDominating();
				G_Vertex vert = node.elem;

				// Do not re expand nodes that have already been visited.
				if(Visited.Contains (vert))
				{
					continue;
				}

				// hallelujah! The shortest path has been found!!!
				if(vert == goal)
				{
					path = extractPath(node);
					path_money_cost = node.money;
					v_current_goal = goal;
					return true;
				}

				// -- Expand node.

				IEnumerable<Edge> iter = vert.getEdges();
				foreach(Edge e_abstract in iter)
				{
					G_Edge e = e_abstract as G_Edge;

					G_Vertex v_dest = e.getOther(vert) as G_Vertex;

					Box<int> money = new Box<int>(node.money);

					// Compute the transversal time,
					// while updating the money value to be the moeny left after the edge is traversed.
					// WARNING : This algorithm assumes that the player is always willing to pay more to travel faster.
					// Because of this design choice, it is possible for people to spend money on short edges instead of longer edges.
					double time = timeLength (e, vert, v_dest, money);

					// Compute the estimated distance value for this node using the euclidean heuristic.
					double estimate_val = node.priority + time + v_dest.distanceTo(goal);

					Priority_Node<G_Vertex> node_new = new Priority_Node<G_Vertex>(v_dest, money.elem, estimate_val);
					node_new.last = node;

					PQ.Add(node_new);
				}
			}

			// No shortest path exists.
			return false;

		}


		/// <summary>
		/// Returns the time it will take the person to walk down this edge.
		/// </summary>
		/// <returns>The length. The money box's value will be update to the amount of money left after this edge has been traversed.
		/// </returns>
		/// <param name="edge">Edge.</param>
		// FIXME : I need to handle bus fares and train fares. (Basically, I need to handle the money.);
		public double timeLength(G_Edge edge, G_Vertex src, G_Vertex dest, Box<int> money)
		{
			HashSet<EdgeType> types = edge.getImprovements ();

			double length = edge.getLength ();

			if (src.train_stop && dest.train_stop && types.Contains (EdgeType.Rail))
			{
				return length/PublicConstants.SPEED_RAIL;
			}

			if(vehicles.Contains(Vehicle.Car))
			{
				if(types.Contains (EdgeType.Boulevard))
				{
					return length/PublicConstants.SPEED_BOULEVARD;
				}

				if(types.Contains (EdgeType.Road))
			    {
					return length/PublicConstants.SPEED_ROAD;
				}
			}

			// Handle taking a bus.
			if (src.bus_stop && dest.bus_stop)
			{
				if(types.Contains (EdgeType.Boulevard))
				{
					return length/PublicConstants.SPEED_BOULEVARD;
				}
				
				if(types.Contains (EdgeType.Road))
				{
					return length/PublicConstants.SPEED_ROAD;
				}
			}

			// Ride an owned bike.
			if(vehicles.Contains(Vehicle.Bike) && types.Contains(EdgeType.Biking_Trail))
			{
				return length/PublicConstants.SPEED_BIKE_TRAIL;
			}

			// Walk along a footpath.
			if(types.Contains (EdgeType.Footpath))
			{
				return length/PublicConstants.SPEED_FOOTPATH;
			}


			// Walk along a nasty hunk of ground,
			// hopefully the person will get to work without too many bruises!!!
			return length/PublicConstants.SPEED_UNIMPROVED;
		}

		/// <summary>
		/// Extracts the shortest path found so far from the given goal_node from the location where the search began.
		/// </summary>
		/// <returns>Returns a stack that represents the vertices the person will have to travel to in order.</returns>
		/// <param name="end_node">The last node in the path. Naturally, this should never be null.</param>
		private Stack<G_Vertex> extractPath(Priority_Node<G_Vertex> end_node)
		{
			Stack<G_Vertex> S = new Stack<G_Vertex> ();

			while (end_node == null)
			{
				S.Push(end_node.elem);
				end_node = end_node.last;
			}

			return S;
		}


		// -- Geometric interface functions.

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


		// -- Vehicle interface functions.


		/// <summary>
		/// Adds the vehicle.
		/// </summary>
		/// <returns><c>true</c>, iff the person did not already own the vehicle.
		/// <param name="v">V.</param>
		public bool AddVehicle(Vehicle v)
		{
			return vehicles.Add (v);
		}

		/// <summary>
		/// Removes the vehicle.
		/// </summary>
		/// <returns><c>true</c>, iff the person had the indicated vehicle.
		/// <param name="v">V.</param>
		public bool RemoveVehicle(Vehicle v)
		{
			return vehicles.Remove (v);
		}

		/// <summary>
		/// Queries a vehicle property for this person.
		/// </summary>
		/// <returns><c>true</c>, iff this person has the indicated vehicle on their person, <c>false</c> otherwise.</returns>
		/// <param name="v">V.</param>
		public bool hasVehicle(Vehicle v)
		{
			return vehicles.Contains (v);
		}

	}
}