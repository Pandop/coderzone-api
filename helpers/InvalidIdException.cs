using System;
using System.Runtime.Serialization;

namespace CsharpReference.Services
{
	[Serializable]
	internal class InvalidIdException : Exception
	{
		public InvalidIdException()
		{
		}

		public InvalidIdException(string message) : base("An invalid id was provided")
		{
		}

		public InvalidIdException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InvalidIdException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}