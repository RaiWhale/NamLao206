using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NamLao206.Models.ViewModels
{
	public class DataQuocTich
	{
		public ListQuocTich objects { get; set; }
	}
	public class ListQuocTich
	{
		public List<QuocTich> @object { get; set; }
	}
	public class QuocTich
	{
		public string ma { get; set; }
		public string id { get; set; }
		public string ten { get; set; }
		public string tenKhac { get; set; }

	}
}