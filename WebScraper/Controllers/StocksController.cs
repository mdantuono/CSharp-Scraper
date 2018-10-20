using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebScraper.Models;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace WebScraper.Controllers
{
    public class StocksController : Controller
    {
        private StockDBEntities db = new StockDBEntities();

        // GET: Stocks
        public ActionResult Index()
        {
            return View(db.Stocks.ToList());
        }

        public ActionResult Scrape()
        {
            //// Show loading animation -- TODO
            //myLoader.Style["visibility"] = visible;

            Scraper myScraper = new Scraper();
            
            // Run scraper and save data to list stockList
            List<Stock> stockList = myScraper.Scrape();
            
            // Open SQL connection
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();

            foreach (Stock stock in stockList)
            {
                db.Stocks.Add(stock);
            }

            db.SaveChanges();
            conn.Close();

            //// Stop loading animation -- TODO
            //myLoader.Style["visibility"] = hidden;

            return RedirectToAction("Index");
        }

        private static string connString = @"data source=MIKEPC;initial catalog=StockDB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";


        // Below are some test routes used in production

        public ActionResult TestScrape()
        {
            
            Scraper testScraper = new Scraper();
            Stock newStock = testScraper.TestScrape();

            SqlConnection conn = new SqlConnection(connString);

            conn.Open();
            db.Stocks.Add(newStock);
            db.SaveChanges();
            conn.Close();

            return RedirectToAction("Index"); 
            
            
        }

        public ActionResult DeleteAllStocks()
        {
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand deleteAll = new SqlCommand("DELETE FROM Stock", conn);
            deleteAll.ExecuteNonQuery();
            db.SaveChanges();
            conn.Close();
            

            return RedirectToAction("Index");
        }
    }
}
