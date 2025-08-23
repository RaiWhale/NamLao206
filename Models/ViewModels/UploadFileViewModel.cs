using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NamLao206.Models.ViewModels
{
	public class UploadFileViewModel
	{
		public int ProjectId { get; set; }
		public List<IFormFile> Files { get; set; }
		public List<int> DocumentTypeIds { get; set; }
	}
}