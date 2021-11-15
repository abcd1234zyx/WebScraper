using System;
using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
namespace Scrape
{
    public class Scraping
    {
        private  string HtmlText { get; set; }
        private readonly List<string> currency = new List<string>();
        /// <summary>
        /// Absolute path shold be given in both windows and linux within permission or access
        /// </summary>
        private readonly string filepath = "C:/Users/Administrator/source/repos/Scrape/Scrape/";
        private string filename = "";
        private readonly string url = "https://srh.bankofchina.com/search/whpj/searchen.jsp";


        //Helps in getting All the html components into string by doing post request to url
        /// <summary>
        /// parameter include dictionory of items converrted to Urlencoded for form submission
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public async Task PostFun(FormUrlEncodedContent queryString)
        {
           HttpClient client = new HttpClient();
            var response = await client.PostAsync(url, queryString);
            var Text = await response.Content.ReadAsStringAsync();
            HtmlText = Text;
        }
        //Add files into list of currency dynamically present as list on url
        /// <summary>
        /// Text contains html file in string
        /// </summary>
        /// <param name="Text"></param>
        public void CurrencyCreate(string Text)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(Text);
            int index = 2;
            while (true)
            {
                var e = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"pjname\"]/option[" + index.ToString() + "]");
                if (e == null)
                    break;
                else
                    currency.Add(e.InnerText);
                index++;
            }
        }
        //DO get request to url for collecting html components
        public async Task GetRequest()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var Text = await response.Content.ReadAsStringAsync();
            CurrencyCreate(Text);
        }

        //Create FormUrlEncodedContent and calls post function and then scraper for every list of elements
        public async Task PostStart()
        {
            string from = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
            string to = DateTime.Now.ToString("yyyy-MM-dd");
            filename = "(" + from + ") - " + "(" + to + ")";
            var payload = new Dictionary<string, string>
                {
                    { "erectDate",from},
                    {"nothing",to},
                    {"pjname",""},
                    {"page","1" }
            };

            int index = 0;
            foreach (string curr in currency)
            {
                HtmlText = "";
                payload["pjname"] = "USD";
                var content = new FormUrlEncodedContent(payload);
                await PostFun(content);
                Scraper(index);
                index++;
            }

        }
        //Scrapes the web after post request for collecting table into the text file(sets the name of text file "($from)-($to)$currency" format
        public void Scraper(int index)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HtmlText);
            int outIndex = 1;
            string file = filepath + filename + currency[index]+".txt";
            while (true)
            {
                string input = "//html/body/table[2]/tr[" + outIndex.ToString() + "]";
                var k = htmlDoc.DocumentNode.SelectSingleNode(input);
                if (k == null) break;
                int innerIndex = 1;
                string d = "";
                while (true)
                {
                    var b = htmlDoc.DocumentNode.SelectSingleNode(input + "/td[" + innerIndex.ToString() + "]");
                    if (b == null) break;
                    else d += b.InnerText+",";
                    innerIndex++;
                }
                if (d.Length >= 1) d = d.Remove(d.Length - 1);
                outIndex++;
                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine(d);
                }
            }
            using (StreamWriter sw = File.AppendText(file))
            {
                sw.WriteLine("--------------------------------------------------------------------------------------------------------------------");
            }
            Console.WriteLine("done");
        }
    }
}
