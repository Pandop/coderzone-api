using System;
using System.Runtime.Serialization;

namespace CsharpReference.Services
{
	[Serializable]
	internal class InvalidGrantTypeException : Exception
	{
		public InvalidGrantTypeException()
		{
		}

		public InvalidGrantTypeException(string message) : base(message)
		{
		}

		public InvalidGrantTypeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InvalidGrantTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}