using Newtonsoft.Json;
using PagedList;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TestApplication.Models;

namespace TestApplication.Controllers
{
    public class ReadJsonController : Controller
    {
        /// <summary>
        /// Controller for search person
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="currentMaleFilter"></param>
        /// <param name="currentFemaleFilter"></param>
        /// <param name="IsMale"></param>
        /// <param name="IsFemale"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ViewResult Index(string searchString, string currentFilter, bool currentMaleFilter=false, bool currentFemaleFilter = false, bool IsMale = false, bool IsFemale = false, int? pageNumber = null)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
                IsMale = currentMaleFilter;
                IsFemale = currentFemaleFilter;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.IsMaleFilter = IsMale;
            ViewBag.IsFemaleFilter = IsFemale;

            //load json file data
            string file = Server.MapPath("~/App_Data/data_large.json");
            using (WebClient client = new WebClient())
            using (Stream stream = client.OpenRead(file))
            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonTextReader reader = new JsonTextReader(streamReader))
            {
                reader.SupportMultipleContent = true;
                var serializer = new JsonSerializer();
                var personlist = new Data();
                var personDetailList = new List<PersonDetail>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        personlist = serializer.Deserialize<Data>(reader);
                    }
                }

                if (personlist != null)
                {
                    var newData = from x in personlist.People
                                  join p in personlist.Places on x.place_id equals p.Id
                                  select new
                                  {
                                      Id = x.id,
                                      Name = x.name,
                                      Gender = (x.gender == "M") ? "Male" : "Female",
                                      BirthPlace = p.Name
                                  };

                    foreach (var x in newData)
                    {
                        var data = new PersonDetail
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Gender = x.Gender,
                            BirthPlace = x.BirthPlace
                        };
                        personDetailList.Add(data);
                    }

                    //apply search filter
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        personDetailList = personDetailList.Where(e => e.Name.ToLower().Contains(searchString.ToLower())).ToList();
                    }
                    if (IsMale && !IsFemale)
                    {
                        personDetailList = personDetailList.Where(e => e.Gender.Equals("Male")).ToList();
                    }
                    if (!IsMale && IsFemale)
                    {
                        personDetailList = personDetailList.Where(e => e.Gender.Equals("Female")).ToList();
                    }
                    
                    //set pagination
                    int pageSize = 10;
                    int pageNumberValue = (pageNumber ?? 1);
                    return View(personDetailList.ToList().ToPagedList(pageNumberValue, pageSize));
                }
            }
            return View();
        }
    }
}