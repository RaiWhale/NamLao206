using QRCoder;
using System;
using System.Drawing;
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
            if (topicId == 2)
            {
                srcPic = "/Content/TrangChu/assets/images/thongbao.jpeg";
            }
            else if (topicId == 9)
            {
                srcPic = "/Content/TrangChu/assets/images/dao-tao.jpg";
            }
            else if (topicId == 10)
            {
                srcPic = "/Content/TrangChu/assets/images/tuyen-dung.jpg";
            }
            else if (topicId == 16)
            {
                srcPic = "/Content/TrangChu/assets/images/tin-tuc.jpg";
            }
            else if (topicId == 22)
            {
                srcPic = "/Content/TrangChu/assets/images/moi-thau.png";
            }
            else if (topicId == 13)
            {
                srcPic = "/Content/TrangChu/assets/images/chat-luong.jpg";
            }
            else
            {
                srcPic = "/Content/TrangChu/assets/images/thongbao.jpeg";
            }



            return srcPic;
        }
		public static string RemoveDiacritics(string input)
		{
			string[] VietnameseSigns = new string[]
		  {
			"aAeEoOuUiIdDyY",
			"áàạảãâấầậẩẫăắằặẳẵ",
			"ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
			"éèẹẻẽêếềệểễ",
			"ÉÈẸẺẼÊẾỀỆỂỄ",
			"óòọỏõôốồộổỗơớờợởỡ",
			"ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
			"úùụủũưứừựửữ",
			"ÚÙỤỦŨƯỨỪỰỬỮ",
			"íìịỉĩ",
			"ÍÌỊỈĨ",
			"đ",
			"Đ",
			"ýỳỵỷỹ",
			"ÝỲỴỶỸ"
		  };

			// Loại bỏ ký tự đặc biệt và chuyển các ký tự tiếng Việt có dấu thành không dấu
			for (int i = 1; i < VietnameseSigns.Length; i++)
			{
				for (int j = 0; j < VietnameseSigns[i].Length; j++)
				{
					input = input.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
				}
			}
			// Loại bỏ các ký tự không nằm trong bảng chữ cái tiếng Anh
			//Regex regex = new Regex("[^a-zA-Z0-9]");
			//input = regex.Replace(input, "");


			input = input.Replace(" ", "-");
			input = input.Replace(" ", "-");
			input = input.Replace("  ", " ");
			input = input.Replace("    ", " ");
			input = input.Replace("     ", " ");
			input = input.Replace("--", "-");
			input = input.Replace("---", "-");
			input = input.Replace("----", "-");
			input = input.Replace("@", "-");
			input = input.Replace("#", "-");
			input = input.Replace("/", "-");
			input = input.Replace("|", "-");
			input = input.Replace("*", "-");
			input = input.Replace("-", "-");
			input = input.Replace("+", "-");
			input = input.Replace(".", "-");
			input = input.Replace("!", "-");
			input = input.Replace("'", "-");
			input = input.Replace("?", "-");
			input = input.Replace("$", "-");
			input = input.Replace("%", "-");
			input = input.Replace("^", "-");
			input = input.Replace("&", "-");
			input = input.Replace("(", "-");
			input = input.Replace(")", "-");						

			return input;
		}
	}
}