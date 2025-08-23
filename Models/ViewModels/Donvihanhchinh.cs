using System.Collections.Generic;

namespace NamLao206.Models.ViewModels
{
	public class DataHanhChinh
	{
		public ListHanhChinh objects { get; set; }
	}
	public class ListHanhChinh
	{
		public List<Donvihanhchinh> @object { get; set; }
	}
	public class Donvihanhchinh
	{
		public string ma { get; set; }
		public string chaId { get; set; }
		public string id { get; set; }
		public string ten { get; set; }
		public string capId { get; set; }

	}
}