using System;
using System.Runtime.Serialization;

namespace CsharpReference.Services
{
	[Serializable]
	internal class UserNotFoundException : Exception
	{
		public UserNotFoundException()
		{
		}

		public UserNotFoundException(string message) : base("The user was not found")
		{
		}

		public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}