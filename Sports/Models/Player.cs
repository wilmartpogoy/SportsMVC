using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sports.Models
{
    public class Player
    {

        public int player_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int jersey_no { get; set; }
        public string fullname { get; set; }
        public string team_name { get; set; }
        public Nullable<DateTime> created_date { get; set; }
        public Nullable<DateTime> updated_date { get; set; }
    }
}