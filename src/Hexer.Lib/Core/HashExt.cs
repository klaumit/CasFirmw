using System;
using System.Linq;
using System.Security.Cryptography;
using Hexer.Tools;

namespace Hexer.Core
{
	public static class HashExt
	{
		private static readonly Lazy<SHA256> Sha256 = new(SHA256.Create);

		public static string GetSha256(this byte[] array)
		{
			var hash = Sha256.Value.ComputeHash(array);
			var hexes = TextExt.ToHex(hash).Split([' '], 4);
			var hex = string.Join("", hexes.Take(3));
			return hex;
		}
	}
}