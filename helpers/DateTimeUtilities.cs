using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.helpers
{
	public static class DateTimeUtilities
	{
		public static string ToIsoString(this DateTime dateTime) => dateTime.ToString("yyyy-MM-ddTHH\\:mm\\:ss");
	}
}
