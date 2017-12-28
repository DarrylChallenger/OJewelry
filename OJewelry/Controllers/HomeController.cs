using OJewelry.Models;
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
            OJewelryDBEntities dc = new OJewelryDBEntities();
            var newClient = dc.Clients.Join(dc.Companies, cli => cli.Id, com => com.Id,
                (cli, com) => new
                {
                    cli.Id,
                    ClientName = cli.Name,
                    ClientPhone = cli.Phone,
                    ClientEmail = cli.Email,
                    CompanyName = com.Name
                }).ToList().Select(n => new ClientViewModel()
                { 
                    Id = n.Id,
                    Name = n.ClientName,
                    Phone = n.ClientPhone,
                    Email = n.ClientEmail,
                    CompanyName = n.CompanyName
                 }
                ).ToList();
            return View("ClientList", newClient);
        }
    }
}