using Scrape;
using System;
using System.Threading.Tasks;
namespace Program
{
    public class Main1
    {
        //Calls Scraping and print done on console after every successful written text(due to Scrape.Scrapping.scraper) and AllDone when all files are written
        public static async Task Main(string[] args)
        {
            Scraping scraping = new Scraping();
            try
            {
                await scraping.GetRequest();
                await scraping.PostStart();
            }
            catch(System.Net.Http.HttpRequestException)
            {
                Console.WriteLine("Internet not working or maybe site is unreahable!!!!!");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("AllDone");
            Console.ReadLine();
        }
        
    }
}