using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sports.Models;


namespace Sports.Controllers
{
    public class TeamController : Controller
    {

        private SportsEntities db = new SportsEntities();
        // GET: Team
        public ActionResult Index(string search = "")
        {  

            var _team = db.tbl_teams.Where(x => x.team_name.Contains(search)).OrderByDescending(a => a.team_id).ToList();

            return View(_team.ToList());
        }

        //
       

        public ActionResult Create()
        {
            ViewBag.Coach = new SelectList(db.vw_coach.ToList(), "coach_id", "fullname");

            return View();
        }

        //
       


        [HttpPost]
        public ActionResult Create(tbl_teams team)
        {


      
            try
                {
                var team_exist = db.tbl_teams.Where(x => x.team_name.ToLower().Trim() == team.team_name.ToLower()).Count();
         
                if(team_exist > 0)
                {

                    return Json(new { message = "" + team.team_name + " is already exist!", success = false, name = team.team_name }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    team.created_date = DateTime.Now;
                    team.coach_id = Convert.ToInt32(team.coach_id);
                    db.tbl_teams.Add(team);

                    db.SaveChanges();
                    return Json(new { message = "" + team.team_name + " successfully saved!", success = true, name = team.team_name }, JsonRequestBehavior.AllowGet);
                }

                }
               catch(Exception ex)
            {
                return Json(new { message = ex.Message, success = false, name = team.team_name }, JsonRequestBehavior.AllowGet);
            }
         

   
           
        }

        public ActionResult Details(int team_id = 0)
        {
            var selected_team = db.tbl_teams.Where(x => x.team_id == team_id).SingleOrDefault();

            if (selected_team == null)
            {
                return HttpNotFound();
            }


            return View(selected_team);
        }
        public ActionResult Edit(int team_id = 0)
        {
         
            var selected_team = db.tbl_teams.Where(x => x.team_id == team_id).SingleOrDefault();

            if (selected_team == null)
            {
                return HttpNotFound();
            }

            ViewBag.Coach = new SelectList(db.vw_coach.ToList(), "coach_id", "fullname");
            return View(selected_team);
        }
        [HttpPost]
        public ActionResult Edit(tbl_teams team)
        {
            try
            {
                var check_name = db.tbl_teams.Where(x => x.team_name.ToLower() == team.team_name.ToLower()).Count();

                if (check_name > 0)
                {
                    return Json(new { message = "" + team.team_name +  " already exist!", success = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    team.created_date = Convert.ToDateTime(team.created_date);
                    team.updated_date = DateTime.Now;
                    team.coach_id = Convert.ToInt32(team.coach_id);
                    team.team_id = Convert.ToInt32(team.team_id);
                    db.Entry(team).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { message = "" + team.team_name + " successfully updated!", success = true }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete()
        {

            return View();
        }

        //
       


        [HttpPost]
        public ActionResult Delete(string id = "")
        {
            int _id = Convert.ToInt32(id);

            var _exist = db.tbl_teams.Where(x => x.team_id == _id).SingleOrDefault();
        
            if (_exist == null)
            {

                return Json(new { message = " " + _id + " not found!", success = false }, JsonRequestBehavior.AllowGet);
            }
         
            else
            {
                db.tbl_teams.Remove(_exist);
                db.SaveChanges();
                return Json(new { message = "" + _exist.team_name + " successfully deleted!", success = true }, JsonRequestBehavior.AllowGet);
            }


        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}