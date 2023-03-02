using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sports.Models;
namespace Sports.Controllers
{
    public class PlayerController : Controller
    {
        private SportsEntities db = new SportsEntities();
        // GET: Player
        public ActionResult Index(string firstname = "", string lastname = "", string team = "", string manager = "", string jersey = "")
        {

            List<Player> list_play = new List<Player>();

            var _player = (from player in db.tbl_player
                          join
              teams in db.tbl_teams on player.team_id equals teams.team_id
                          join managers in db.vw_manager on player.manager_id equals managers.manager_id
                          select new {
                              player_id= player.player_id,
                              firstname =  player.firstname,
                              lastname =    player.lastname,
                              jersey_no = player.jersey_no,
                              fullname =  managers.fullname,
                              team_name = teams.team_name,
                              created_date = player.created_date,
                              updated_date = player.updated_date
                          }).ToList();

            //   var player_list = _player.ToList();
   
                var player_filter = _player.Where(x => x.firstname.Contains(firstname) && x.lastname.Contains(lastname)
             && x.team_name.Contains(team) && x.fullname.Contains(manager)
            ).OrderByDescending(a => a.player_id).ToList();
            

            if(jersey != "")
            {
                int _jer = Convert.ToInt32(jersey);
                player_filter = player_filter.Where(x => x.jersey_no == _jer).ToList();
            }

          
           foreach ( var z in player_filter)
            {
                Player play = new Player
                {
                    player_id = z.player_id,
                    firstname = z.firstname,
                    lastname = z.lastname,
                    jersey_no = (int) z.jersey_no,
                    fullname = z.fullname,
                    team_name = z.team_name,
                    created_date = z.created_date,
                    updated_date = z.updated_date



                };
                list_play.Add(play);
         
            }
          
            return View(list_play);
        }


        public ActionResult Create()
        {
            ViewBag.Team = new SelectList(db.tbl_teams.ToList(), "team_id", "team_name");
            ViewBag.Manager = new SelectList(db.vw_manager.ToList(), "manager_id", "fullname");
            return View();
        }

        //



        [HttpPost]
        public ActionResult Create(tbl_player player)
        {


            try
            {
                player.manager_id = Convert.ToInt32(player.manager_id);
                player.jersey_no = Convert.ToInt32(player.jersey_no);
                player.team_id = Convert.ToInt32(player.team_id);
                var player_exist = db.tbl_player.Where(x => x.firstname.ToLower().Trim() == player.firstname.ToLower() && x.lastname.ToLower().Trim() == player.lastname.ToLower()).Count();
                var team_player_jersey = db.tbl_player.Where(x => x.jersey_no == player.jersey_no && x.team_id == player.team_id).SingleOrDefault();
                if (player_exist > 0)
                {


                    return Json(new { message = "" + player.firstname + " " + player.lastname + " is already exist!", success = false}, JsonRequestBehavior.AllowGet);
                }
                if (team_player_jersey !=null)
                {
                    return Json(new { message = " Sorry, " + team_player_jersey.firstname + " " + team_player_jersey.lastname + " from your team already wearing jersey " + team_player_jersey.jersey_no + "", success = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    player.created_date = DateTime.Now;
                    db.tbl_player.Add(player);

                    db.SaveChanges();
                    return Json(new { message = "" + player.firstname + " " + player.lastname + " successfully inserted!", success = true }, JsonRequestBehavior.AllowGet);
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
        public JsonResult Delete(string id = "")
        {

            int _id = Convert.ToInt32(id);

            var _exist = db.tbl_player.Where(x => x.player_id == _id).SingleOrDefault();
          
            if (_exist == null)
            {

                return Json(new { message = " " + _id + " not found!", success = false }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                db.tbl_player.Remove(_exist);
                db.SaveChanges();
                return Json(new { message = "" + _exist.firstname + " " + _exist.lastname + " successfully deleted!", success = true }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Details(int player_id = 0)
        {
            var selected_player = db.tbl_player.Where(x => x.player_id == player_id).SingleOrDefault();

            if (selected_player == null)
            {
                return HttpNotFound();
            }


            return View(selected_player);
        }
        public ActionResult Edit(int player_id = 0)
        {
            var selected_player = db.tbl_player.Where(x => x.player_id == player_id).SingleOrDefault();

            if (selected_player == null)
            {
                return HttpNotFound();
            }

            ViewBag.Team = new SelectList(db.tbl_teams.ToList(), "team_id", "team_name");
            ViewBag.Manager = new SelectList(db.vw_manager.ToList(), "manager_id", "fullname");
            return View(selected_player);
        }
        [HttpPost]
        public ActionResult Edit(tbl_player player)
        {
            try
            {
                player.manager_id = Convert.ToInt32(player.manager_id);
                player.jersey_no = Convert.ToInt32(player.jersey_no);
                player.team_id = Convert.ToInt32(player.team_id);
                player.updated_date = DateTime.Now;
                player.created_date = Convert.ToDateTime(player.created_date);
                var player_exist = db.tbl_player.Where(x => x.firstname.ToLower().Trim() == player.firstname.ToLower() && x.lastname.ToLower().Trim() == player.lastname.ToLower()).Count();
                var team_player_jersey = db.tbl_player.Where(x => x.jersey_no == player.jersey_no && x.team_id == player.team_id).SingleOrDefault();
                if (player_exist > 0)
                {


                    return Json(new { message = "" + player.firstname + " " + player.lastname + " is already exist!", success = false }, JsonRequestBehavior.AllowGet);
                }
                if (team_player_jersey != null)
                {
                    return Json(new { message = " Sorry, " + team_player_jersey.firstname + " " + team_player_jersey.lastname + " from your team already wearing jersey " + team_player_jersey.jersey_no + "", success = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {


                    db.Entry(player).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { message = "" + player.firstname + " " + player.lastname + " successfully updated!", success = true }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}