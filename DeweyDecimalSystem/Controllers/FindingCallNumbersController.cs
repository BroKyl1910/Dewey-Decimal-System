using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeweyDecimalSystem.Helpers;
using DeweyDecimalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DeweyDecimalSystem.Controllers
{
    public class FindingCallNumbersController : Controller
    {
        public string Initialise()
        {
            CallNumberJSONModel fileJSON = GetTreeJson();
            return "";
        }

        private static CallNumberJSONModel GetTreeJson()
        {
            CallNumberJSONModel fileJSON;
            using (StreamReader sr = new StreamReader(FileURLHelper.CALL_NUMBERS_TREE_JSON_FILE))
            {
                fileJSON = JsonConvert.DeserializeObject<CallNumberJSONModel>(sr.ReadToEnd());
            }
            return fileJSON;
        }
    }
}