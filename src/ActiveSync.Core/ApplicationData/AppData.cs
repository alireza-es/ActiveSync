using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace ActiveSync.Core.ApplicationData
{
    public abstract class AppData
    {
        public string ServerId { get; set; }
        public string ClientId { get; set; }
        public abstract XmlNode GetAsXmlNode(XmlDocument xmlDocument);
        protected abstract string[] GetHashKeys();
        public string GenerateHash()
        {
            var hashkeys = GetHashKeys();
            var hashInput = string.Join(",", hashkeys);

            using (var md5Hash = MD5.Create())
            {
                return GetMd5Hash(md5Hash, hashInput);
            }

        }

        public bool IsHashChanged(string oldHash)
        {
            var newHash = GenerateHash();

            var comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(oldHash, newHash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetMd5Hash(HashAlgorithm md5Hash, string input)
        {

            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
