using NamLao206.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NamLao206.Controllers.API
{
    public class ApiController
    {
		private static string END_POINT_URL = "https://lgsp.danang.gov.vn/dldc/1.0.0/";
		// GET: Api
		public static async Task<string> GetTokenDVHC()
		{
			string token = "";
			using (var client = new HttpClient())
			{
				string data = "client_id=6aSoqSHvNPFhTOb_gl8b0nSl1iAa&client_secret=TlJ7bVslKKIEX8G0yGP42kfgLDca&grant_type=client_credentials";
				using (var response = await client.PostAsync("https://lgsp.danang.gov.vn/token", new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded")))
				{
					string json = await response.Content.ReadAsStringAsync();
					if (json.Contains("access_token"))
					{
						int pos = json.IndexOf("access_token") + 15;
						token = json.Substring(pos, json.IndexOf("\"", pos) - pos);
					}
				}
			}
			return token;
		}
		public static async Task<List<Donvihanhchinh>> GetDVHC()
		{
			List<Donvihanhchinh> result = new List<Donvihanhchinh>();
			string token = await GetTokenDVHC();

			if (!string.IsNullOrEmpty(token))
			{
				try
				{
					using (var client = new HttpClient())
					{
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
																														 //client.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=utf-8");
						client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

						string url = "donvihanhchinh";


						string json = await client.GetStringAsync(END_POINT_URL + url);

						result = JsonConvert.DeserializeObject<DataHanhChinh>(json).objects.@object;
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
				}
			}
			return result;
		}
		public static async Task<List<QuocTich>> GetQT()
		{
			List<QuocTich> result = new List<QuocTich>();
			string token = await GetTokenDVHC();

			if (!string.IsNullOrEmpty(token))
			{
				try
				{
					using (var client = new HttpClient())
					{
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
																														 //client.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=utf-8");
						client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

						string url = "dantoc";


						string json = await client.GetStringAsync(END_POINT_URL + url);

						result = JsonConvert.DeserializeObject<DataQuocTich>(json).objects.@object;
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
				}
			}
			return result;
		}
	}
}