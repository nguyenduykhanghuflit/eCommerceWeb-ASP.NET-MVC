﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClothesWebNET.Models;
using ClothesWebNET.Pattern.ModelsView;
using static ClothesWebNET.Models.Product;

namespace ClothesWebNET.Controllers
{
    public class CollectionsController : Controller
    {
        private CLOTHESEntities db = new CLOTHESEntities();

        //API GET CATEGORY
        [HttpGet]
        public JsonResult GetCategory()
        {
            var category = from type in db.Types
                           select new Category()
                           {
                               nameType = type.nameType,
                               idType = type.idType,
                           };

            return Json(category.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: Collections

        public ActionResult Index(string id,int? page)
        {
            ProductVM productVM = new ProductVM();

            if (id == null)
            {
                int count = db.Products.Count();
                var data = productVM.GetAllProduct(page).ToList();

                ViewBag.list = data;
                ViewBag.MaxPage = (count / 12) - (count % 12 == 0 ? 1 : 0);
                ViewBag.Page = page==null? 1 : page;

                return View(data);

            }
            else
            {
                int count = db.Products.Where(p=>p.idType==id).Count();
                var data = productVM.GetProductByType(page,id).ToList();

                ViewBag.list = data;
                ViewBag.MaxPage = (count / 12) - (count % 12 == 0 ? 1 : 0);
                ViewBag.Page = page == null ? 1 : page;

                return View(data);

            }

        }

        // GET: Collections/Details/5
  
        public ActionResult Details(string id)
        {
    
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = from el in db.Products
                          where el.idProduct == id
                          select el;

            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {

                var data = from p in product
                           select p;

                data.Include("ImageProducts")
                    .Include("Type");
                var datarelateto = (from p in db.Products
                                    join t in data on p.idType equals t.idType
                                    select p);
                datarelateto.Include("ImageProducts").Include("Type");
                var subData = (datarelateto.ToList()).Skip(3).Take(4);
                ViewBag.datarelateto = subData.ToList();
                ViewBag.List = data;
                return View(data.ToList());
            };
        }


    }
}
