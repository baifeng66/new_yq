using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WS.SearchWeb.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddRole()
        {
            return View();
        }
         public IActionResult ChangeMenu()
        {
            return View();
        }
        


    }
}
