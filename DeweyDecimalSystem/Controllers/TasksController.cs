﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DeweyDecimalSystem.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult ReplacingBooks()
        {

            return View();
        }

        public IActionResult IdentifyingAreas()
        {

            return View();
        }

        public IActionResult FindingCallNumbers()
        {

            return View();
        }
    }
}