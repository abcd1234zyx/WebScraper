using System;
using Xunit;
using Program;
using Scrape;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ScrapeTest
{
    public class UnitTest1
    {
        [Fact]
       public async Task Test_PostFunForHtmlText()
        {
            Scraping obj = new Scraping();
            string from = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
            string to = DateTime.Now.ToString("yyyy-MM-dd");
            var payload = new Dictionary<string, string>
                {
                    { "erectDate",from},
                    {"nothing",to},
                    {"pjname","USD"},
                    {"page","1" }
            };
            await obj.PostFun(new FormUrlEncodedContent(payload));
            PrivateObject po = new PrivateObject(obj);
            string Htmltext=(string)po.GetProperty("HtmlText");
            string checkString = File.ReadAllText(@"C:\Users\Administrator\source\repos\Scrape\ScrapeTest\HtmlTest(USD).txt");
            Xunit.Assert.Equal(Htmltext, checkString);
        }
        [Fact]
        public void Test_currencyCreate()
        {
            string checkString = File.ReadAllText(@"C:\Users\Administrator\source\repos\Scrape\ScrapeTest\HtmlTest(USD).txt");
            Scraping obj = new Scraping();
            obj.CurrencyCreate(checkString);
            PrivateObject po = new PrivateObject(obj);
            List<string> lis = po.GetFieldOrProperty("currency") as List<string>;
            List<string> mylist = new List<string>(new string[] { "GBP" , "HKD" ,"USD", "CHF", "DEM","FRF", "SGD", "SEK", "DKK", "NOK", "JPY", "CAD", "AUD", "EUR", "MOP", "PHP", "THB", "NZD", "KRW", "RUB", "MYR", "TWD", "ESP", "ITL", "NLG", "BEF", "FIM" , "IDR" , "BRL" , "AED" , "INR" , "ZAR" , "SAR" , "TRY" });
            String expected = "", actual = "";
            foreach(var value in lis) actual += value;
            foreach (var value in lis) expected += value;
            Xunit.Assert.Equal(actual,expected);
        }
        [Fact]
        public async Task Test_GetAsync()
        {
            Scraping obj = new Scraping();
            await obj.GetRequest();
            PrivateObject po = new PrivateObject(obj);
            List<string> lis = po.GetFieldOrProperty("currency") as List<string>;
            List<string> mylist = new List<string>(new string[] { "GBP", "HKD", "USD", "CHF", "DEM", "FRF", "SGD", "SEK", "DKK", "NOK", "JPY", "CAD", "AUD", "EUR", "MOP", "PHP", "THB", "NZD", "KRW", "RUB", "MYR", "TWD", "ESP", "ITL", "NLG", "BEF", "FIM", "IDR", "BRL", "AED", "INR", "ZAR", "SAR", "TRY" });
            String expected = "", actual = "";
            foreach (var value in lis) actual += value;
            foreach (var value in lis) expected += value;
            Xunit.Assert.Equal(actual, expected);
        }

    }
}
