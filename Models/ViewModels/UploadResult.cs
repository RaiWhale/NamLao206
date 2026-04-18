using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NamLao206.Models.ViewModels
{
    public class UploadResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public string RelativePath { get; set; }

        // Helper methods
        public static UploadResult Failed(string message)
        {
            return new UploadResult
            {
                Success = false,
                Message = message
            };
        }

        public static UploadResult SuccessResult(string fileName, string fullPath, string relativePath)
        {
            return new UploadResult
            {
                Success = true,
                FileName = fileName,
                FullPath = fullPath,
                RelativePath = relativePath,
                Message = "Upload file thành công"
            };
        }
    }
}