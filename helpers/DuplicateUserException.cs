using System;
using System.Runtime.Serialization;

namespace CsharpReference.Services
{
	[Serializable]
	internal class DuplicateUserException : Exception
	{
		public DuplicateUserException()
		{
		}

		public DuplicateUserException(string message) : base("This user already exists")
		{
		}

		public DuplicateUserException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected DuplicateUserException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}