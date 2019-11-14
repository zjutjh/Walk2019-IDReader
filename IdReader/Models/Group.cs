using Newtonsoft.Json.Linq;

namespace IdReader.Models
{
    public class Group
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string No { get; set; }
        public string Route { get; set; }
        public string Caption { get; set; }
        public string isSubmit { get; set; }

        public string GroupId { get; set; }



        public Group(JToken json)
        {
            Name = json["name"].ToString();
            Description = json["description"].ToString();
            No = json["No"].ToString();
            Route = json["route"].ToString();
            Caption = json["captain_name"].ToString();
            isSubmit = json["is_submit"].ToString();

        }

  
     

        public Group() { }


    }
}
