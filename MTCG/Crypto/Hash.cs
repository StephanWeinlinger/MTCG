using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Crypto {
	public static class Hash {
		public static string HashString(string value) {
			byte[] salt;
			using var csp = new RNGCryptoServiceProvider();
			csp.GetBytes(salt = new byte[16]);
			using var pbkdf2 = new Rfc2898DeriveBytes(value, salt, 100000);
			byte[] hash = pbkdf2.GetBytes(20);
			byte[] hashBytes = new byte[36];
			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);
			string hashedValue = Convert.ToBase64String(hashBytes);
			return hashedValue;
		}

		public static bool CompareString(string value, string hashToCompare) {
			byte[] hashBytes = Convert.FromBase64String(hashToCompare);
			byte[] salt = new byte[16];
			Array.Copy(hashBytes, 0, salt, 0, 16);
			using var pbkdf2 = new Rfc2898DeriveBytes(value, salt, 100000);
			byte[] hash = pbkdf2.GetBytes(20);
			for(int i = 0; i < 20; i++) {
				if(hashBytes[i + 16] != hash[i]) {
					return false;
				}
			}
			return true;
		}
	}
}
