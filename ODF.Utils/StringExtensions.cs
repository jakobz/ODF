using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODF.Utils
{
	/// <summary>
	/// Common string utility functions made as extension methods.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Extension method variant of the static String.IsNullOrEmpty helper.
		/// </summary>
		public static bool IsNullOrEmpty(this string s)
		{
			return string.IsNullOrEmpty(s);
		}

		/// <summary>
		/// Extension method variant of the static String.IsNullOrEmpty helper.
		/// </summary>
		public static bool IsNullOrWhiteSpace(this string s)
		{
			return string.IsNullOrWhiteSpace(s);
		}
	}
}
