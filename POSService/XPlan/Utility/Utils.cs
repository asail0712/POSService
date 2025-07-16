using System.Security.Cryptography;
using System.Text;

namespace XPlan.Utility
{
    public static class Utils
    {
        public static string ComputeSha256Hash(string rawData)
        {
            // 建立 SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // 把字串轉成 byte[]
                byte[] bytes            = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                // 轉成十六進位字串
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
