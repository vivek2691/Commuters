using UnityEngine;
using System.Collections;

public enum EdgeType
{
	Unimproved,
	Footpath,
	Biking_Trail,
	Road,
	Boulevard,
	Rail,
}

public enum Vehicle
{
	Bike,
	Car
}

public enum VertexUpgrades
{
    BusStop,
    TrainStation,
    BikeShop,
    CarShop
}








































public enum VertexType
{
	Rich,
	Poor
}

/// <summary>
/// Person location enum.
/// 
/// This enum represents the state a G_Person is in. People are either inside a neighborhood (vertex) or they are travelling along and edge.
/// </summary>
public enum PersonState
{
	Edge,
	Vertex
}