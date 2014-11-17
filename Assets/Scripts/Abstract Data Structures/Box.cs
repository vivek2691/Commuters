//------------------------------------------------------------------------------
/// <summary>
/// Box Data Structure.
/// Written by Bryce Summers on 11 - 16 - 2014.
/// 
/// Purpose : This acts as a class that stores a value. Boxes can be passed into functions that query and mutate the value appropiatly.
/// </summary>
//------------------------------------------------------------------------------
using System;
namespace AssemblyCSharp
{
	public class Box<E>
	{
		public E elem;

		public Box (E elem)
		{
			this.elem = elem;
		}
	}
}