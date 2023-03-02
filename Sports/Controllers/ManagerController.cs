using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sports.Models;
namespace Sports.Controllers
{
    public class ManagerController : Controller
    {
        private SportsEntities db = new SportsEntities();
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Get_AllEmployee(string firstname = "", string lastname = "")
        {
            try
            {


                var _manager = db.tbl_manager.Where(x => x.firstname.Contains(firstname) && x.lastname.Contains(lastname)).OrderByDescending(a => a.manager_id).ToList();
                return Json(_manager, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
        }

        public ActionResult Create()
        {

            return View();
        }

        //



        [HttpPost]
        public JsonResult Create(tbl_manager manager)
        {



            try
            {
                var manager_exist = db.tbl_manager.Where(x => x.firstname.ToLower().Trim() == manager.firstname.ToLower() && x.lastname.ToLower().Trim() == manager.lastname.ToLower()).Count();

                if (manager_exist > 0)
                {


                    return Json(new { message = "" + manager.firstname + " " + manager.lastname + " is already exist!", success = false, first = manager.firstname, last = manager.lastname }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    manager.created_date = DateTime.Now;
                    db.tbl_manager.Add(manager);

                    db.SaveChanges();
                    return Json(new { message = "" + manager.firstname + " " + manager.lastname + " successfully inserted!", success = true, first = manager.firstname, last = manager.lastname }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false, first = manager.firstname, last = manager.lastname }, JsonRequestBehavior.AllowGet);
            }




        }


        public ActionResult Delete()
        {

            return View();
        }

        //



        [HttpPost]
        public JsonResult Delete(string id = "")
        {

            int _id = Convert.ToInt32(id);

            var _exist = db.tbl_manager.Where(x => x.manager_id == _id).SingleOrDefault();
            var exist_manager = db.tbl_player.Where(x => x.manager_id == _id).Count();
            if (_exist == null)
            {

                return Json(new { message = " " + _id + " not found!", success = false }, JsonRequestBehavior.AllowGet);
            }
            else if (exist_manager > 0)
            {
                return Json(new { message = "Manager " + _exist.firstname + " " + _exist.lastname + " is not allowed to be deleted, this manager connected to the player", success = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.tbl_manager.Remove(_exist);
                db.SaveChanges();
                return Json(new { message = "" + _exist.firstname + " " + _exist.lastname + " successfully deleted!", success = true }, JsonRequestBehavior.AllowGet);
            }

        }





        public ActionResult Edit(int manager_id = 0)
        {
            var selected_manager= db.tbl_manager.Where(x => x.manager_id == manager_id).SingleOrDefault();

            if (selected_manager == null)
            {
                return HttpNotFound();
            }


            return View(selected_manager);
        }

        //



        [HttpPost]
        public ActionResult Edit(tbl_manager manager)
        {
            try
            {
                var check_name = db.tbl_manager.Where(x => x.firstname.ToLower() == manager.firstname.ToLower() && x.lastname.ToLower() == manager.lastname.ToLower()).Count();

                if (check_name > 0)
                {
                    return Json(new { message = "" + manager.firstname + " " + manager.lastname + " already exist!", success = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    manager.created_date = Convert.ToDateTime(manager.created_date);
                    manager.updated_date = DateTime.Now;
                    manager.manager_id = Convert.ToInt32(manager.manager_id);
                    db.Entry(manager).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { message = "" + manager.firstname + " " + manager.lastname + " successfully updated!", success = true }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Details(int manager_id = 0)
        {
            var selected_manager = db.tbl_manager.Where(x => x.manager_id == manager_id).SingleOrDefault();

            if (selected_manager == null)
            {
                return HttpNotFound();
            }


            return View(selected_manager);
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
