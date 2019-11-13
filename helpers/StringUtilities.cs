using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.helpers
{
	public static class StringUtilities
	{
		public static string ConvertToCamelCase(this string field) => char.ToLowerInvariant(field[0]) + field.Substring(1);
		public static string ConvertToPascalCase(this string field) => char.ToUpperInvariant(field[0]) + field.Substring(1);
	}
}
