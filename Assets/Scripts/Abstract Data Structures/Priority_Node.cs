//------------------------------------------------------------------------------
/// <summary>
/// Priority_ node. Class.
/// 
/// Written by Bryce Summers, 11 - 16 - 2014.
/// 
/// Purpose: used to store priority values in a min heap or a max heap.
/// 
/// </summary>
//------------------------------------------------------------------------------
using System;
namespace AssemblyCSharp
{
	public class Priority_Node<E> : IComparable<Priority_Node<E>>
	{

		// -- Data values.
		public double priority;
		public E elem;

		// Used for retrieving paths after a graph search.
		public Priority_Node<E> last;

		public int money;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyCSharp.Priority_Node`1"/> class.
		/// </summary>
		/// <param name="elem">Element.</param>
		/// <param name="priority">Priority.</param>
		public Priority_Node (E elem, int money, double priority)
		{
			this.elem = elem;
			this.priority = priority;
			this.money = money;
		}

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <returns>The to.</returns>
		/// <param name="other">Other.</param>
		public int CompareTo(Priority_Node<E> other)
		{
			if(other == null)
			{
				return 1;
			}
			
			// Return the difference in priorities.
			return Math.Sign(priority - other.priority);
		}
	}
}