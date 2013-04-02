using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ODF.Utils;

namespace ODF.Tests.Utils
{
	[TestFixture]
	public class StringExtensions
	{
		[Test]
		public void TestStringIsNullOrEmpty()
		{
			string nullString = null;
			Assert.IsTrue("".IsNullOrEmpty());
			Assert.IsFalse("a".IsNullOrEmpty());
			Assert.IsFalse(" ".IsNullOrEmpty());
			Assert.IsTrue(nullString.IsNullOrEmpty());

			Assert.IsTrue("".IsNullOrWhiteSpace());
			Assert.IsTrue("   ".IsNullOrWhiteSpace());
			Assert.IsFalse("  a ".IsNullOrWhiteSpace());
			Assert.IsTrue(nullString.IsNullOrWhiteSpace());
		}
	}
}
