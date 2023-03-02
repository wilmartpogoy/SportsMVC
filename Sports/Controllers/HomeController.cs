using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sports.Models;
namespace Sports.Controllers
{
    public class HomeController : Controller
    {
        private SportsEntities db = new SportsEntities();
        public ActionResult Home()
        {

            ViewBag.Player = db.tbl_player.Count();
            ViewBag.Manager = db.tbl_manager.Count();
            ViewBag.Coach = db.tbl_coach.Count();
            ViewBag.Team = db.tbl_teams.Count();
            return View();
        }

      
    }
}