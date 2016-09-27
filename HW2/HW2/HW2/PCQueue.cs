using System;

namespace CS422
{
	public class PCQueue
	{
		private PCNode front;
		private PCNode back;
		private PCNode nullNode;

		public PCQueue ()
		{
			nullNode = new PCNode ();
			front = nullNode;
			back = nullNode;
		}

		public PCNode Front
		{
			get {
				return front;
			}
			set {
				front = value;
			}
		}

		public PCNode Back
		{
			get {
				return back;
			}
			set {
				back = value;
			}
		}

		public void Enqueue(int dataValue)
		{
			// Empty queue
			if (object.ReferenceEquals (Front, nullNode)) { // If front is null node, queue must be empty
				PCNode newNode = new PCNode(dataValue);		// Create a new node 
				Front = newNode;							// Set front and back to the new node
				Back = newNode;
				return;
			}

			// Queue size 1
			if (object.ReferenceEquals (Front, Back)) { 	// If front is equal to back, queue must be one node
				PCNode newNode = new PCNode (dataValue);	// Create a new node
				Front.Next = newNode;						// Set Front.Next to new node
				Back = newNode;								// Set Back to new node
				return;
			}

			// Queue size >1
			else {
				PCNode newNode = new PCNode(dataValue);		// Create a new node
				Back.Next = newNode;						// Set the previous back's next to new node
				Back = newNode;								// Set the new node to back
			}
		}

		public bool Dequeue(ref int out_value)
		{
			// Empty queue
			if (object.ReferenceEquals (Front, nullNode))	// If front is null node, queue must be empty
				return false;
			
			// Queue size 1
			if (object.ReferenceEquals (Front, Back)) {		// If front is equal to back, queue must be one node
				out_value = Front.Data;						// Output front's data
				nullNode = Front;							// Make the node the new null node
				return true;
			}

			// Queue size >1
			else {
				out_value = Front.Data;
				Front = Front.Next;
				return true;
			}
		}
	}

	public class PCNode
	{
		private int data;
		private PCNode next;

		#region Constructors

		/// <summary>
		/// Constructor initializes only data, sets next node reference to null
		/// </summary>
		/// <param name="Data">Data.</param>
		public PCNode(int Data)
		{
			data = Data;
			next = null;
		}

		/// <summary>
		/// Constructor initializes both data and next node reference
		/// </summary>
		/// <param name="Data">Data.</param>
		/// <param name="Next">Next.</param>
		public PCNode(int Data, PCNode Next)
		{
			data = Data;
			next = Next;
		}

		/// <summary>
		/// Empty Constructor, sets data and next node reference to null. For making nullNodes only
		/// </summary>
		public PCNode()
		{
			next = null;
		}

		#endregion

		#region Properties

		public int Data
		{
			get {
				return data;
			}
			set {
				data = value;
			}
		}

		public PCNode Next
		{
			get {
				return next;
			}
			set {
				next = value;
			}
		}

		#endregion
	}
}

