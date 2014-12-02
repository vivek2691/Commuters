//------------------------------------------------------------------------------
/// <summary>
/// Public constants.
/// 
/// Written By Bryce Summers on 11 - 16 - 2014.
/// </summary>
//------------------------------------------------------------------------------
using System;

	public class PublicConstants
	{
		// Speed constants used in path finding to find the shortest path.
		// These represent the distance that each person travels in an update step.
		
		// WARNING : Never make a speed less than 1.0!!!
		
		// WARNING : Do not change the ordering of the speeds, e.g footpaths <= bike trail <= road <= boulevard <= rail.
		//			 Let Bryce know if you change the ordering.
		
		// Please feel free to modify these values.
		
		public static double SPEED_UNIMPROVED = 0.1;
		public static double SPEED_FOOTPATH   = 0.3;
		public static double SPEED_BIKE_TRAIL = 1.0;
		public static double SPEED_ROAD       = 1.0;
		public static double SPEED_BOULEVARD  = 7;
		public static double SPEED_RAIL       = 10;
		
		
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
		
		// Used for Game Clock
		
		public static int MINUTES_PER_DAY = 1440;
		public static int MIN_WORKING_HOURS = 6;
		public static int MAX_WORKING_HOURS = 12;
		public static int MIN_WAKEUP_TIME = 6;
		public static int MAX_WAKEUP_TIME = 11;


		// Healthiness values for edge types.
		public static double health_footpath = 10.0;
		public static double health_biking_trail = 6.0;
		public static double health_unimproved = 1.0;
		public static double health_rail = .3;
		public static double health_road = .2;
		public static double health_boulevard = .1;

		//Used for BPerson
		
		public static int AVERAGE_WEALTH_RICH = 200;
		public static int AVERAGE_WEALTH_POOR = 75;
		public static int AVERAGE_HEALTH_RICH = 150;
		public static int AVERAGE_HEALTH_POOR = 100;
		
		//used for BNeighbourhood
		public static int RICHNESS_THRESHOLD = 150;

}

