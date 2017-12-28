﻿using OJewelry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OJewelry.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return ClientList();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ClientList()
        {
            ViewBag.Message = "Client List";
            OJewelryDBEntities1 dc = new OJewelryDBEntities1();
            var aClient = dc.Clients.Join(dc.Companies, cli => cli.CompanyID, com => com.Id,
                (cli, com) => new
                {
                    cli.Id,
                    ClientName = cli.Name,
                    ClientPhone = cli.Phone,
                    ClientEmail = cli.Email,
                    CompanyName = com.Name,
                    CompanyId = com.Id
                }).ToList().Select(n => new ClientViewModel()
                { 
                    Id = n.Id,
                    Name = n.ClientName,
                    Phone = n.ClientPhone,
                    Email = n.ClientEmail,
                    CompanyName = n.CompanyName,
                    CompanyId = n.CompanyId
                 }
                ).ToList();
            return View("ClientList", aClient);
        }
        public ActionResult CollectionListByCompany(int id)
        {
            ViewBag.Message = "Collection List for company";
            
            OJewelryDBEntities1 dc = new OJewelryDBEntities1();
            CollectionViewModel m = new CollectionViewModel();
            Company co = dc.Companies.Find(id);
            m.CompanyId = co.Id;
            m.CompanyName = co.Name;
            m.Collections = new List<CollectionModel>();
            foreach (Collection coll in  co.Collections)
            {
                CollectionModel collM = new CollectionModel()
                {
                    Id = coll.Id,
                    CompanyId = coll.CompanyId,
                    Name = coll.Name
                };
                collM.Styles = new List<StyleModel>();
                foreach (Style sty in coll.Styles)
                {
                    StyleModel styM = new StyleModel()
                    {
                        Id = sty.Id,
                        Name = sty.StyleName,
                        Num = sty.StyleNum,
                        Qty = sty.Quantity
                        // Cost is the sum of the component prices
                        //Retail Price is the cost * retail ratio
                    };
                    collM.Styles.Add(styM);
                }
                m.Collections.Add(collM);
            }
            return View(m);
        }


    }
}