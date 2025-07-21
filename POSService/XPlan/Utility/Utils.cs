using System.Security.Cryptography;
using System.Text;

namespace XPlan.Utility
{
    public static class Utils
    {
        public static string ComputeSha256Hash(string rawData, string salt)
        {
            // 將原始資料與 salt 合併
            string saltedData = rawData + salt;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes            = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(saltedData));
                StringBuilder builder   = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
