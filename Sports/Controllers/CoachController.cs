using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sports.Models;

namespace Sports.Controllers
{

    public class CoachController : Controller
    {
        private SportsEntities db = new SportsEntities();
        // GET: Coach
        public ActionResult Index(string firstname = "",string lastname = "")
        {

            var _coach = db.tbl_coach.Where(x => x.firstname.Contains(firstname) && x.lastname.Contains(lastname)).OrderByDescending(a => a.coach_id).ToList();

            
            return View(_coach);
        }

        public ActionResult Create()
        {

            return View();
        }

        //
 


        [HttpPost]
        public JsonResult Create(tbl_coach coach)
        {



            try
            {
                var coach_exist = db.tbl_coach.Where(x => x.firstname.ToLower().Trim() == coach.firstname.ToLower() && x.lastname.ToLower().Trim() == coach.lastname.ToLower()).Count();

                if (coach_exist > 0)
                {


                    return Json(new { message = "" + coach.firstname + " " + coach.lastname + " is already exist!", success = false, first = coach.firstname, last = coach.lastname }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    coach.created_date = DateTime.Now;
                    db.tbl_coach.Add(coach);

                    db.SaveChanges();
                    return Json(new { message = ""+ coach.firstname + " " + coach.lastname + " successfully inserted!", success = true, first = coach.firstname, last = coach.lastname }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false, first = coach.firstname, last = coach.lastname }, JsonRequestBehavior.AllowGet);
            }




        }


        public ActionResult Delete()
        {

            return View();
        }

        //



        [HttpPost]
        public JsonResult Delete(string id= "")
        {

            int _id = Convert.ToInt32(id);

            var _exist = db.tbl_coach.Where(x => x.coach_id == _id).SingleOrDefault();
            var exist_team = db.tbl_teams.Where(x => x.coach_id == _id).Count();
            if (_exist == null)
            {
       
                return Json(new { message = " "+ _id + " not found!", success = false }, JsonRequestBehavior.AllowGet);
            }
            else if (exist_team > 0)
            {
                return Json(new { message = "Coach " + _exist.firstname + " " + _exist.lastname + " is not allowed to be deleted, this coach connected to the team", success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.tbl_coach.Remove(_exist);
                db.SaveChanges();
                return Json(new { message = "" + _exist.firstname + " " + _exist.lastname + " successfully deleted!", success = true }, JsonRequestBehavior.AllowGet);
            }
       
        }





        public ActionResult Edit(int coach_id = 0)
        {
            var selected_coach = db.tbl_coach.Where(x => x.coach_id == coach_id).SingleOrDefault();

            if(selected_coach == null)
            {
                return HttpNotFound();
            }

          
            return View(selected_coach);
        }

        //



        [HttpPost]
        public ActionResult Edit(tbl_coach coach)
        {
            try {
                var check_name = db.tbl_coach.Where( x => x.firstname.ToLower() == coach.firstname.ToLower() && x.lastname.ToLower() == coach.lastname.ToLower()).Count();

                if(check_name > 0)
                {
                    return Json(new { message = "" + coach.firstname + " " + coach.lastname + " already exist!", success = false }, JsonRequestBehavior.AllowGet);
                }
                else
                { 
                coach.created_date = Convert.ToDateTime(coach.created_date);
                coach.updated_date = DateTime.Now;
                coach.coach_id = Convert.ToInt32(coach.coach_id);
         db.Entry(coach).State = EntityState.Modified;
           db.SaveChanges();
            return Json(new { message = "" + coach.firstname + " " + coach.lastname + " successfully updated!", success = true }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Details(int coach_id = 0)
        {
            var selected_coach = db.tbl_coach.Where(x => x.coach_id == coach_id).SingleOrDefault();

            if (selected_coach == null)
            {
                return HttpNotFound();
            }


            return View(selected_coach);
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