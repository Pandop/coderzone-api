using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.helpers
{
	public static class CollectionExtensions
	{
		public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				target.Add(item);
			}
		}
	}
}
