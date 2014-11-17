//------------------------------------------------------------------------------
/// <summary>
/// Public constants.
/// 
/// Written By Bryce Summers on 11 - 16 - 2014.
/// </summary>
//------------------------------------------------------------------------------
using System;
namespace AssemblyCSharp
{
	public class PublicConstants
	{
		// Speed constants used in path finding to find the shortest path.
		// These represent the distance that each person travels in an update step.

		// WARNING : Never make a speed less than 1.0!!!

		// WARNING : Do not change the ordering of the speeds, e.g footpaths <= bike trail <= road <= boulevard <= rail.
		//			 Let Bryce know if you change the ordering.

		// Please feel free to modify these values.

		public static double SPEED_UNIMPROVED = 1.0;
		public static double SPEED_FOOTPATH   = 1.5;
		public static double SPEED_BIKE_TRAIL = 3;
		public static double SPEED_ROAD       = 10;
		public static double SPEED_BOULEVARD  = 15;
		public static double SPEED_RAIL       = 20;


		// Please change these to your heart's content, hopefully with the goal of improving gameplay.
		// These represent the cost values.
		// If you would like to use some other representation other than an integer for money,
		// please let Bryce Know.

		public static int COST_BUS_TICKET = 2;// arbitrary values have been chosen thus far.
		public static int COST_TRAIN_TICKET = 3;

		public static int COST_RENT_BIKE = 1;
		public static int COST_RENT_CAR  = 2;
		public static int COST_BUY_BIKE  = 3;
		public static int COST_BUY_CAR   = 4;
	}
}

