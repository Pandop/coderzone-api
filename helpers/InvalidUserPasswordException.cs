using System;
using System.Runtime.Serialization;

namespace CsharpReference.Services
{
	[Serializable]
	internal class InvalidUserPasswordException : Exception
	{
		public InvalidUserPasswordException()
		{
		}

		public InvalidUserPasswordException(string message) : base("The username/password couple is invalid.")
		{
		}

		public InvalidUserPasswordException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InvalidUserPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}