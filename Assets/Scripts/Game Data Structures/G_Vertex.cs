using UnityEngine;
using System.Collections;

using AssemblyCSharp;
using System.Collections.Generic;
using System;

/// <summary>
/// G_ vertex class.
/// Written by Bryce Summers on 11 - 16 - 2014.
/// 
/// These vertices represent neighborhoods inside of the game.
/// 
/// This class overrides the standard Vertex class, so it can be used with standard Bryce Graphs.
/// </summary>

namespace AssemblyCSharp
{
	
	public class G_Vertex : Vertex
	{
		// Geometric location on the screen / in the world.
		private int x, y;
		
		// A set of the people currently inside of this vertex.
		private HashSet<G_Person> people = new HashSet<G_Person>();

		// -- Properties.
		// Every Neighborhood may contain various shops and stops.
		public bool bus_stop    = false;
		public bool train_stop  = false;
		public bool bike_shop   = false;
		public bool bike_rental = false;
		public bool car_rental  = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyCSharp.G_Vertex"/> class.
		/// </summary>
		/// <param name="index">Index.</param>
		public G_Vertex(int index) : base(index)
		{
			/* Trivial Constructor. */
		}

		
		/// <summary>
		/// Sets the position to the given geometric coordinates.
		/// </summary>
		/// <param name="x_new">X_new.</param>
		/// <param name="y_new">Y_new.</param>
		public void setPosition(int x_new, int y_new)
		{
			x = x_new;
			y = y_new;
		}

		public int getX()
		{
			return x;
		}

		public int getY()
		{
			return y;
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
				throw new UnityException("G_Vertex addPerson(): Null Person");
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
				throw new UnityException("G_Vertex removePerson(): Null person");
			}	

			return people.Remove (p);
		}

		// -- Distance computation functions.

		/// <summary>
		/// Euclidean Distance calculation.
		/// </summary>
		/// <returns> Returns the Euclidean distance from this vertex to the input vertex.</returns>
		/// <param name="v">V.</param>
		public double distanceTo(G_Vertex v)
		{
			int x1 = getX ();
			int x2 = v.getX ();
			int y1 = getY ();
			int y2 = v.getY ();
			
			return distance (x1, y1, x2, y2);
		}

		public double distanceTo(int x, int y)
		{
			return distance (x, y, getX (), getY ());
		}

		public static double distance(int x1, int y1, int x2, int y2)
		{
			int dx = x1 - x2;
			int dy = y1 - y2;
			
			return Math.Sqrt (dx * dx + dy * dy);
		}
	}
}
