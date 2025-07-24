using System.Security.Cryptography;
using System.Text;

namespace XPlan.Utility
{
    public static class Utils
    {
        /// <summary>
        /// 計算輸入字串與 salt 混合後的 SHA256 雜湊值
        /// </summary>
        /// <param name="rawData">原始字串</param>
        /// <param name="salt">加鹽字串</param>
        /// <returns>SHA256 雜湊值的十六進位字串</returns>
        public static string ComputeSha256Hash(string rawData, string salt)
        {
            // 將原始資料與 salt 合併
            string saltedData = rawData + salt;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes            = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(saltedData));
                StringBuilder builder   = new StringBuilder();

                // 將雜湊的 byte 陣列轉成十六進位字串
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
