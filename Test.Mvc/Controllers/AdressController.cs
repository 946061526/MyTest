using Chloe;
using Chloe.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Mvc.Models;
using System.Configuration;


namespace Test.Mvc.Controllers
{
    public class AdressController : Controller
    {
        // GET: Adress
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddAddress()
        {
            return View();
        }
        public ActionResult test()
        {
            return View();
        }
        // GET: Adress/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Adress/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Adress/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public IDbContext dbContext = new MsSqlContext(ConfigurationManager.ConnectionStrings["local"].ToString());

        public ActionResult Edit()
        {
            //从数据库中读取
            var listProvince = dbContext.SqlQuery<ProvinceCityArea>("SELECT [ID],[Name],[ParentID] FROM [dbo].[JingdongRegion] WHERE [ParentID] IS NULL").ToList();
            ViewBag.ListProvince = listProvince;
            return View();
        }

        [HttpPost]
        public JsonResult GetCityByPid(int pid)
        {
            //从数据库中读取
            var listCity = dbContext.SqlQuery<ProvinceCityArea>("SELECT [ID],[Name] FROM [dbo].[JingdongRegion] WHERE [ParentID]=" + pid).ToList();
            return Json(listCity);
        }

        //// GET: Adress/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Adress/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Adress/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Adress/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
