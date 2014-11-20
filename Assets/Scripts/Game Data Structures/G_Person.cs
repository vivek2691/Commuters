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
using System.Text;


public class G_Person
{
	private int index;
	
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
	
	private double percentage_change_per_frame = 0.0;
	
	
	private HashSet<Vehicle> vehicles = new HashSet<Vehicle> ();
	
	public G_Person(G_Vertex v_home, G_Vertex v_work, int index)
	{
		this.v_home = v_home;
		this.v_work = v_work;
		
		// Every person starts at home by default.
		myState  = PersonState.Vertex;
		myVertex = v_home;
		
		// Let the home vertex know this person is there.
		v_home.addPerson (this);
		
		this.index = index;
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
			myVertex = v_new;
			return;
		}
		
		if(myState == PersonState.Edge)
		{
			myEdge.removePerson(this);
			myState = PersonState.Vertex;
			v_new.addPerson (this);
			myVertex = v_new;
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
	/// <returns>-1 if no path was found that can be satisfied with the given money alowance.
	/// returns a cost >= 0 if a path was found and the player has been moved.
	/// The return value is the cost of any payments the player may have had to make.
	/// </returns>
	public int MoveTowards(G_Vertex v_new, int money)
	{
		// Compute a new shortest path if the person is given a new destination.
		if(v_new != v_current_goal || money < path_money_cost)
		{
			if(computeShortestPath(v_new, money) == false)
			{
				return -1;
			}
		}
		
		// -- Movement code.
		
		// Handle moving along an edge.
		if(myState == PersonState.Edge)
		{
			weight += percentage_change_per_frame;
			
			if(weight < 0)
			{
				G_Vertex v_percentage1 = myEdge.getV1() as G_Vertex;
				teleportToVertex(v_percentage1);
				return 0;
			}
			
			if(weight > 1)
			{
				G_Vertex v_percentage2 = myEdge.getV2() as G_Vertex;
				teleportToVertex(v_percentage2);
				return 0;
			}
			
			return 0;
		}

		// No Path --> no policy for leaving neighborhoods.
		if(path == null || path.Count == 0)
		{
			return -1;
		}

		// -- Handle in vertex movement.
		
		// Retrive relevant topological information.
		G_Vertex next = path.Pop();

		// No movement if we are headed to the neighborhood we are already in.
		if (next == myVertex)
		{
			return 0;
		}

		G_Edge edge = myVertex.getEdgeTo (next) as G_Edge;
		Box<int> amount_left = new Box<int> (money);
		
		double speed = computePathSpeed (edge, myVertex, next, amount_left);
		
		int next_vert_index = edge.getVerticeIndex (next);
		
		double length = edge.getLength();
		
		// Now change the state of this person to allow him to embark on an edge.
		if(next_vert_index == 1)
		{
			// Case where the person travels from v2 to v1 --> 1.0 -> 0.0
			weight = 1.0;
			percentage_change_per_frame = -speed/length;// Percentage, not time.
		}
		else
		{
			// Case where the person travels from v1 to v2 --> 0.0 -> 1.0
			weight = 0.0;
			percentage_change_per_frame = speed/length;// Percentage, not time.
		}
		
		myEdge = edge;
		edge.addPerson (this);
		myState = PersonState.Edge;
		
		// Return the cost of embarking on this edge.
		return money - amount_left.elem;
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
		
		while (!PQ.isEmpty())
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
			
			// -- Expand (Visit) the node.
			
			Visited.Add (vert);
			
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
				double speed = computePathSpeed (e, vert, v_dest, money);
				
				double time = e.getLength() / speed;
				
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
	public double computePathSpeed(G_Edge edge, G_Vertex src, G_Vertex dest, Box<int> money)
	{
		HashSet<EdgeType> types = edge.getImprovements ();
		
		if (src.train_stop && dest.train_stop && types.Contains (EdgeType.Rail) &&
		    money.elem >= PublicConstants.COST_TRAIN_TICKET)
		{
			money.elem -= PublicConstants.COST_TRAIN_TICKET;
			return PublicConstants.SPEED_RAIL;
		}
		
		// Handle Riding in an owned car.
		if(vehicles.Contains(Vehicle.Car))
		{
			if(types.Contains (EdgeType.Boulevard))
			{
				return PublicConstants.SPEED_BOULEVARD;
			}
			
			if(types.Contains (EdgeType.Road))
			{
				return PublicConstants.SPEED_ROAD;
			}
		}
		
		// Handle Riding in a rental car.
		if(src.car_rental && money.elem >= PublicConstants.COST_RENT_CAR)
		{
			if(types.Contains (EdgeType.Boulevard))
			{
				money.elem -= PublicConstants.COST_RENT_CAR;
				return PublicConstants.SPEED_BOULEVARD;
			}
			
			if(types.Contains (EdgeType.Road))
			{
				money.elem -= PublicConstants.COST_RENT_CAR;
				return PublicConstants.SPEED_ROAD;
			}
		}
		
		// Handle taking a bus.
		if (src.bus_stop && dest.bus_stop && money.elem >= PublicConstants.COST_BUS_TICKET)
		{
			
			if(types.Contains (EdgeType.Boulevard))
			{
				money.elem -= PublicConstants.COST_BUS_TICKET;
				return PublicConstants.SPEED_BOULEVARD;
			}
			
			if(types.Contains (EdgeType.Road))
			{
				money.elem -= PublicConstants.COST_BUS_TICKET;
				return PublicConstants.SPEED_ROAD;
			}
		}
		
		// Ride an owned bike.
		if(vehicles.Contains(Vehicle.Bike) && types.Contains(EdgeType.Biking_Trail))
		{
			return PublicConstants.SPEED_BIKE_TRAIL;
		}
		
		// Rent a bike.
		if(src.bike_rental && types.Contains(EdgeType.Biking_Trail) && money.elem >= PublicConstants.COST_RENT_BIKE)
		{
			money.elem -= PublicConstants.COST_RENT_BIKE;
			return PublicConstants.SPEED_BIKE_TRAIL;
		}
		
		// Walk along a footpath.
		if(types.Contains (EdgeType.Footpath))
		{
			return PublicConstants.SPEED_FOOTPATH;
		}
		
		
		// Walk along a nasty hunk of ground,
		// hopefully the person will get to work without too many bruises!!!
		return PublicConstants.SPEED_UNIMPROVED;
	}
	
	/// <summary>
	/// Extracts the shortest path found so far from the given goal_node from the location where the search began.
	/// </summary>
	/// <returns>Returns a stack that represents the vertices the person will have to travel to in order.</returns>
	/// <param name="end_node">The last node in the path. Naturally, this should never be null.</param>
	private Stack<G_Vertex> extractPath(Priority_Node<G_Vertex> end_node)
	{
		Stack<G_Vertex> S = new Stack<G_Vertex> ();

		while (end_node != null)
		{
			S.Push(end_node.elem);
			end_node = end_node.last;
		}

		// Don't keep the current vertex in the path, because it will cause a 1 frame delay.
		if (myState == PersonState.Vertex && S.Peek () == myVertex)
		{
			S.Pop();
		}
		
		return S;
	}
	
	
	// -- Geometric interface functions.
	
	/// <summary>
	/// Gets the x coordinate of this person.
	/// </summary>
	/// <returns>The x.</returns>
	public float getX()
	{
		switch(myState)
		{

		case PersonState.Vertex:
			return (float)myVertex.getX();
		case PersonState.Edge:
			G_Vertex v1 = myEdge.getV1() as G_Vertex;
			G_Vertex v2 = myEdge.getV2() as G_Vertex;
			double x1 = v1.getX();
			double x2 = v2.getX();
			return (float)(x1*(1 - weight) + x2*weight);
		}
		
		throw new Exception("We should never get here.");
	}
	
	/// <summary>
	/// Gets the y coordinate of this person.
	/// </summary>
	/// <returns>The y.</returns>
	public float getY()
	{
		switch(myState)
		{
		case PersonState.Vertex:
			return (float)myVertex.getY();
		case PersonState.Edge:
			G_Vertex v1 = myEdge.getV1() as G_Vertex;
			G_Vertex v2 = myEdge.getV2() as G_Vertex;
			double y1 = v1.getY();
			double y2 = v2.getY();
			return (float)(y1*(1 - weight) + y2*weight);
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
	
	
	public G_Vertex getHomeVertex()
	{
		return v_home;
	}
	
	public G_Vertex getWorkVertex()
	{
		return v_work;
	}
	
	public void setHomeVertex(G_Vertex v)
	{
		if(v == null)
		{
			throw new Exception("Null vertex input");
		}
		v_home = v;
	}
	
	public void setWorkVertex(G_Vertex v)
	{
		if(v == null)
		{
			throw new Exception("Null vertex input");
		}
		v_work = v;
	}
	
	public override string ToString ()
	{
		StringBuilder s = new StringBuilder ();
		s.AppendLine ("G_Person");
		s.AppendLine ("index = " + index);
		if(myState == PersonState.Edge)
		{
			s.AppendLine ("Edge: " + myEdge.ToString());
			s.AppendLine ("Weight:" + weight);
		}
		
		if(myState == PersonState.Vertex)
		{
			s.AppendLine ("Vertex: " + myVertex.ToString());
		}

		s.AppendLine ("X = " + getX () + ", Y = " + getY ());
		
		s.AppendLine ("");
		
		return s.ToString ();
	}
	
	public override int GetHashCode()
	{
		return index;
	}
}
