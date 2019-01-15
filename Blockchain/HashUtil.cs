using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain
{
    public static class HashUtil
    {
        public static string Sha256(params string[] data)
        {
            var hash = string.Empty;
            var crypto = new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(string.Join("",data)));
            return crypto.Aggregate(hash, (current, theByte) => current + theByte.ToString("x2"));
        }
    }
}
