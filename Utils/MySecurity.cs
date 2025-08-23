using QRCoder;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NamLao206.Utils
{
    public class MySecurity
    {	
        public static string Encrypt(string str)
        {
            SHA256 sha = SHA256Managed.Create();
            string signdata = "fhe8w89jkvnh899thdkfhs";
            byte[] result = sha.ComputeHash(Encoding.Unicode.GetBytes(str + signdata));
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

        public static string str_replace(string key)
        {
            string keys = key.ToString();
            keys = keys.Replace(" ", "");
            keys = keys.Replace(",", "");
            keys = keys.Replace("@", "");
            keys = keys.Replace("#", "");
            keys = keys.Replace("/", "");
            keys = keys.Replace("|", "");
            keys = keys.Replace("*", "");
            keys = keys.Replace("-", "");
            keys = keys.Replace("+", "");
            keys = keys.Replace(".", "");
            keys = keys.Replace("!", "");
            keys = keys.Replace("'", "");
            keys = keys.Replace("?", "");
            keys = keys.Replace("$", "");
            keys = keys.Replace("%", "");
            keys = keys.Replace("^", "");
            keys = keys.Replace("&", "");
            keys = keys.Replace("(", "");
            keys = keys.Replace(")", "");
            return keys;
        }       

        public static string GetQRCode(string ticketNumber, string filename)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode("{\"uuid\":\"" + ticketNumber + "\"}", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (Bitmap bitMap = qrCode.GetGraphic(20, Color.Black, Color.White, (Bitmap)Bitmap.FromFile(filename)))
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    var QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    return QRCodeImage;
                }
            }
        }

        public static string GetSrcPicture(int topicId)
        {
            string srcPic = "";
			switch (topicId)
			{
                case 1: srcPic = "/Content/EponaTheme/assets/images/thongbao.jpeg";
					break;
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					srcPic = "/Content/EponaTheme/assets/images/tin-tuc.jpg";
					break;
				case 9:
					srcPic = "/Content/EponaTheme/assets/images/chuyendoiso.jpeg";
					break;
				default:
					// Xử lý trường hợp không nằm trong các case trên
					srcPic = "/Content/EponaTheme/assets/images/thongbao.jpeg";
					break;
			}	             
            return srcPic;
        }
        public static string RemoveDiacritics(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // Normalize the string to decompose characters into base letters and diacritics
            string normalizedString = input.Normalize(NormalizationForm.FormKD);

            // Filter out diacritic marks and rebuild the string
            var stringBuilder = new StringBuilder();
            foreach (char c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            // Rebuild the string and normalize to FormC to ensure consistency
            string result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            // Replace special characters and multiple spaces/hyphens with a single hyphen
            result = Regex.Replace(result, "[^a-zA-Z0-9-]", "-");
            result = Regex.Replace(result, "-{2,}", "-"); // Replace multiple hyphens with a single one
            result = result.Trim('-'); // Remove leading/trailing hyphens

            return result;
        }
    }
}