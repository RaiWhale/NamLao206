using NamLao206.Models;
using NamLao206.Models.ViewModels;
using Newtonsoft.Json.Linq;
using QRCoder;
using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

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
        public static async Task<UploadResult> HandleFileUpload(HoSoPhapLy hoSoPhapLy, HttpPostedFileBase file, string UploadPath)
        {
            if (file == null || file.ContentLength == 0)
                return new UploadResult { Success = false, Message = "Chưa chọn file!" };

            // Check file extension
            var allowedExts = new[] { ".xls", ".xlsx", ".pdf", ".doc", ".docx" };
            string ext = Path.GetExtension(file.FileName)?.ToLower();

            if (string.IsNullOrEmpty(ext) || !allowedExts.Contains(ext))
                return new UploadResult { Success = false, Message = $"Định dạng không hỗ trợ. Cho phép: {string.Join(", ", allowedExts)}" };

            // QUÉT FILE VỚI VIRUSTOTAL
            var virusScanResult = await ScanFileWithVirusTotalAsync(file);
            if (!virusScanResult.IsSafe)
            {
                return new UploadResult
                {
                    Success = false,
                    Message = $"File không an toàn: {virusScanResult.Message}"
                };
            }

            // Generate safe filename
            string safeFileName = GenerateSafeFileName(file.FileName);

            // Xác định đường dẫn lưu file
            string savePath = GetSavePath(hoSoPhapLy, UploadPath);

            // Tạo thư mục nếu chưa tồn tại
            Directory.CreateDirectory(savePath);

            // Tạo đường dẫn file duy nhất
            string fullFilePath = Path.Combine(savePath, safeFileName);

            // Kiểm tra và đổi tên nếu file đã tồn tại
            fullFilePath = GetUniqueFilePath(fullFilePath);

            // Save file
            using (var stream = new FileStream(fullFilePath, FileMode.CreateNew))
            {
                await file.InputStream.CopyToAsync(stream);
            }

            return new UploadResult
            {
                Success = true,
                FileName = Path.GetFileName(fullFilePath),
                FullPath = fullFilePath,
                RelativePath = GetRelativePath(fullFilePath, UploadPath)
            };
        }

        private static string GetSavePath(HoSoPhapLy hoSoPhapLy, string baseUploadPath)
        {
            // Nếu có ProjectID: kết hợp với AddBangId
            if (hoSoPhapLy.ProjectID.HasValue)
            {
                return Path.Combine(baseUploadPath,
                    "Projects",
                    hoSoPhapLy.ProjectID.Value.ToString(),
                    hoSoPhapLy.AddBangId?.ToString() ?? "0");
            }
            // Nếu có NhapBanMu_Id
            else if (hoSoPhapLy.NhapBanMu_Id.HasValue)
            {
                return Path.Combine(baseUploadPath,
                    "NhapBanMus",
                    hoSoPhapLy.NhapBanMu_Id.Value.ToString());
            }
            // Nếu có XayDung_Id
            else if (hoSoPhapLy.XayDung_Id.HasValue)
            {
                return Path.Combine(baseUploadPath,
                    "XayDungs",
                    hoSoPhapLy.XayDung_Id.Value.ToString());
            }
            // Nếu có ThietBi_Id
            else if (hoSoPhapLy.ThietBi_Id.HasValue)
            {
                return Path.Combine(baseUploadPath,
                    "ThietBis",
                    hoSoPhapLy.ThietBi_Id.Value.ToString());
            }
            // Mặc định (chung)
            else
            {
                return Path.Combine(baseUploadPath, "General");
            }
        }

        private static async Task<VirusScanResult> ScanFileWithVirusTotalAsync(HttpPostedFileBase file)
        {
            try
            {
                // Lấy API key từ config
                string apiKey = ConfigurationManager.AppSettings["VirusTotalApiKey"];
                bool scanEnabled = ConfigurationManager.AppSettings["VirusTotalScanEnabled"] == "true";

                // Nếu không có API key hoặc scan không enabled
                if (string.IsNullOrEmpty(apiKey) || !scanEnabled)
                {
                    // Có thể log warning
                    System.Diagnostics.Debug.WriteLine("VirusTotal scan is disabled or API key not configured");
                    return new VirusScanResult { IsSafe = true, Message = "Scan disabled" };
                }

                // Tính hash SHA256 của file
                string fileHash = await CalculateFileHashAsync(file);

                // 1. Kiểm tra hash với VirusTotal (nếu đã quét trước đó)
                var hashResult = await CheckHashWithVirusTotalAsync(fileHash, apiKey);
                if (hashResult.IsSafe.HasValue)
                {
                    return new VirusScanResult
                    {
                        IsSafe = hashResult.IsSafe.Value,
                        Message = hashResult.IsSafe.Value ? "File đã được quét và an toàn" : "File đã được quét và phát hiện mã độc"
                    };
                }

                // 2. Nếu hash chưa có trong database, upload file để quét
                return await UploadAndScanFileAsync(file, apiKey);
            }
            catch (Exception ex)
            {
                // Log lỗi
                System.Diagnostics.Debug.WriteLine($"Lỗi khi quét VirusTotal: {ex.Message}");

                // Tuỳ chính sách: cho phép hoặc từ chối khi lỗi
                bool allowOnError = ConfigurationManager.AppSettings["VirusTotalAllowOnError"] == "true";

                return new VirusScanResult
                {
                    IsSafe = allowOnError,
                    Message = allowOnError ? "Lỗi quét, file được cho phép" : "Lỗi quét, file bị từ chối"
                };
            }
        }

        private static async Task<string> CalculateFileHashAsync(HttpPostedFileBase file)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            using (var stream = file.InputStream)
            {
                // Reset stream position
                stream.Position = 0;

                byte[] hashBytes = sha256.ComputeHash(stream);

                // Reset stream position lại
                stream.Position = 0;

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private static async Task<HashCheckResult> CheckHashWithVirusTotalAsync(string hash, string apiKey)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-apikey", apiKey);
                client.Timeout = TimeSpan.FromSeconds(30);

                string url = $"https://www.virustotal.com/api/v3/files/{hash}";

                try
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var json = JObject.Parse(jsonResponse);

                        var stats = json["data"]?["attributes"]?["last_analysis_stats"];

                        if (stats != null)
                        {
                            int malicious = stats["malicious"]?.Value<int>() ?? 0;
                            int suspicious = stats["suspicious"]?.Value<int>() ?? 0;
                            int undetected = stats["undetected"]?.Value<int>() ?? 0;

                            // File an toàn nếu không có engine nào detect là malicious/suspicious
                            bool isSafe = malicious == 0 && suspicious == 0;

                            return new HashCheckResult
                            {
                                IsSafe = isSafe,
                                Message = isSafe ? $"An toàn ({undetected} engine không phát hiện)" : $"Phát hiện {malicious} malicious, {suspicious} suspicious"
                            };
                        }
                    }

                    return new HashCheckResult { IsSafe = null, Message = "Hash không tồn tại trong database" };
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi check hash: {ex.Message}");
                    return new HashCheckResult { IsSafe = null, Message = $"Lỗi khi check hash: {ex.Message}" };
                }
            }
        }

        private static async Task<VirusScanResult> UploadAndScanFileAsync(HttpPostedFileBase file, string apiKey)
        {
            // Tạo file tạm
            string tempFilePath = Path.GetTempFileName();

            try
            {
                // Lưu file tạm
                file.SaveAs(tempFilePath);

                using (var client = new HttpClient())
                using (var formData = new MultipartFormDataContent())
                {
                    client.DefaultRequestHeaders.Add("x-apikey", apiKey);
                    client.Timeout = TimeSpan.FromSeconds(60);

                    // Thêm file vào form data
                    var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(tempFilePath));
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    formData.Add(fileContent, "file", Path.GetFileName(tempFilePath));

                    // Upload file lên VirusTotal
                    var uploadResponse = await client.PostAsync(
                        "https://www.virustotal.com/api/v3/files",
                        formData
                    );

                    if (!uploadResponse.IsSuccessStatusCode)
                    {
                        return new VirusScanResult
                        {
                            IsSafe = false,
                            Message = $"Lỗi upload file: {uploadResponse.StatusCode}"
                        };
                    }

                    // Lấy ID của scan
                    string uploadJson = await uploadResponse.Content.ReadAsStringAsync();
                    var uploadData = JObject.Parse(uploadJson);
                    string analysisId = uploadData["data"]?["id"]?.Value<string>();

                    if (string.IsNullOrEmpty(analysisId))
                    {
                        return new VirusScanResult { IsSafe = false, Message = "Không lấy được analysis ID" };
                    }

                    // Chờ và lấy kết quả scan (với timeout)
                    return await WaitForScanResultAsync(analysisId, apiKey, 60); // Timeout 60 giây
                }
            }
            finally
            {
                // Xóa file tạm
                if (System.IO.File.Exists(tempFilePath))
                {
                    System.IO.File.Delete(tempFilePath);
                }
            }
        }

        private static async Task<VirusScanResult> WaitForScanResultAsync(string analysisId, string apiKey, int timeoutSeconds)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-apikey", apiKey);

                string url = $"https://www.virustotal.com/api/v3/analyses/{analysisId}";

                DateTime startTime = DateTime.Now;

                while ((DateTime.Now - startTime).TotalSeconds < timeoutSeconds)
                {
                    await Task.Delay(5000); // Chờ 5 giây giữa các lần check

                    try
                    {
                        var response = await client.GetAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            var json = JObject.Parse(jsonResponse);

                            string status = json["data"]?["attributes"]?["status"]?.Value<string>();

                            if (status == "completed")
                            {
                                var stats = json["data"]?["attributes"]?["stats"];
                                if (stats != null)
                                {
                                    int malicious = stats["malicious"]?.Value<int>() ?? 0;
                                    int suspicious = stats["suspicious"]?.Value<int>() ?? 0;
                                    int undetected = stats["undetected"]?.Value<int>() ?? 0;

                                    bool isSafe = malicious == 0 && suspicious == 0;

                                    return new VirusScanResult
                                    {
                                        IsSafe = isSafe,
                                        Message = isSafe ?
                                            $"Quét hoàn tất: An toàn ({undetected} engine không phát hiện)" :
                                            $"Quét hoàn tất: Phát hiện {malicious} malicious, {suspicious} suspicious"
                                    };
                                }
                            }
                            else if (status == "queued" || status == "in-progress")
                            {
                                continue; // Tiếp tục chờ
                            }
                            else
                            {
                                return new VirusScanResult { IsSafe = false, Message = $"Trạng thái scan không xác định: {status}" };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy kết quả scan: {ex.Message}");
                    }
                }

                return new VirusScanResult { IsSafe = false, Message = "Timeout khi chờ kết quả scan" };
            }
        }

        private static string GenerateSafeFileName(string originalFileName)
        {
            // Lấy tên file an toàn
            string fileName = Path.GetFileName(originalFileName);

            // Loại bỏ ký tự không hợp lệ
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                fileName = fileName.Replace(c.ToString(), "");
            }

            // Giới hạn độ dài
            string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            if (nameWithoutExt.Length > 50)
            {
                nameWithoutExt = nameWithoutExt.Substring(0, 50);
            }

            // Thêm timestamp và guid để tránh trùng
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string guid = Guid.NewGuid().ToString("N").Substring(0, 8);

            return $"{timestamp}_{nameWithoutExt}_{guid}{extension}";
        }

        private static string GetUniqueFilePath(string proposedFilePath)
        {
            if (!File.Exists(proposedFilePath))
                return proposedFilePath;

            string directory = Path.GetDirectoryName(proposedFilePath);
            string fileName = Path.GetFileNameWithoutExtension(proposedFilePath);
            string extension = Path.GetExtension(proposedFilePath);

            int counter = 1;
            string newFilePath;

            do
            {
                newFilePath = Path.Combine(directory, $"{fileName}_{counter}{extension}");
                counter++;
            } while (File.Exists(newFilePath));

            return newFilePath;
        }

        private static string GetRelativePath(string fullPath, string basePath)
        {
            Uri fullUri = new Uri(fullPath);
            Uri baseUri = new Uri(basePath + Path.DirectorySeparatorChar);

            return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fullUri).ToString());
        }
    }
}