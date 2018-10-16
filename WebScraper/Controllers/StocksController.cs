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

        // GET: Stocks/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: Stocks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Symbol,Price,Change,PChange,Volume")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Stocks.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stock);
        }

        // GET: Stocks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Symbol,Price,Change,PChange,Volume")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Stock stock = db.Stocks.Find(id);
            db.Stocks.Remove(stock);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
    }
}
