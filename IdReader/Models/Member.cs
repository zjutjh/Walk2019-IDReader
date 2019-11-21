using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IdReader.Models
{
    public class Member
    {
        public IDCard IdCard { get; set; }

        public string name{ get; set; }
        public string msg { get; set; }

        public string sex { get; set; }
        public string idNumber { get; set; }
        public Group GroupInfo { get; set; }

        public bool isOk { get; set; }

        public Member(IDCard i)
        {
            IdCard = i;
            sex = i.sex;
            name = i.name;
            idNumber = i.number;
        }

        public async Task SetStart()
        {
            HttpClient httpClient = new HttpClient();

            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
            pairs.Add(new KeyValuePair<string, string>("pass", "QWERT"));
            pairs.Add(new KeyValuePair<string, string>("iid",idNumber));
            pairs.Add(new KeyValuePair<string, string>("code", "1"));

            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(pairs);
            var res = await httpClient.PostAsync(Util.serverUrl, formUrlEncodedContent);

            var json = await res.Content.ReadAsStringAsync();
            fullInfo(json);

        }

        public async Task SetBack(bool isNormal)
        {


            HttpClient httpClient = new HttpClient();

            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
            pairs.Add(new KeyValuePair<string, string>("pass", "QWERT"));
            pairs.Add(new KeyValuePair<string, string>("iid", idNumber));
            if (isNormal)
            {
                pairs.Add(new KeyValuePair<string, string>("code", "3"));
            }
            else
            {
                pairs.Add(new KeyValuePair<string, string>("code", "2"));
            }
  

            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(pairs);
            var res= await httpClient.PostAsync(Util.serverUrl, formUrlEncodedContent);

            var json = await res.Content.ReadAsStringAsync();
            fullInfo(json);
            
        }


        private void fullInfo(string json)
        {
            try
            {
                JObject jo = JObject.Parse(json);
                var data = jo.GetValue("data");
                var code = jo.GetValue("code").ToString();


                msg = jo.GetValue("msg").ToString();

                if (code == "1")
                {
                    isOk = true;
                }
                var group = data["group"];


                if (name == null)
                {
                    var user = data["user"];
                    name = user["name"].ToString();

                }
                if (group.HasValues != false)
                {
                    GroupInfo = new Group(group);
                }
            }
            catch (Exception)
            {

       
            }
           
           
        }

        public Member(string id) {
            idNumber = id;
        }

        public Member()
        {
        }
    }
}
