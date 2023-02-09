using DadsProgram.Data;
using DadsProgram.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DadsProgram.Controllers
{
    public class FingerJointController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FingerJointController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var allData = _context.FingerJoints.ToList();
            //var names = allData.Select(x => x.Name).Distinct().ToList();
            //ViewData["names"] = new SelectList(names);
            return View(allData);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Extension,Flexion,ActiveValue,PassiveValue,Finger,Date,Description")] FingerJoint fingerJoint)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fingerJoint);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(fingerJoint);
        }

        /*       public JsonResult GetData()
               {
                   var extensionData = new List<int>();
                   var flexionData = new List<int>();
                   var fingerData = new List<string>();
                   var dates = new List<string>();

                   // Retrieve data from database
                   var fingerJoints = _context.FingerJoints.ToList();
                   foreach (var fingerJoint in fingerJoints)
                   {
                       extensionData.Add(fingerJoint.Extension);
                       flexionData.Add(fingerJoint.Flexion);
                       fingerData.Add(fingerJoint.Finger);
                       dates.Add(fingerJoint.Date.ToString("yyyy-MM-dd")); // Add the corresponding date for each finger joint
                   }

                   return Json(new { extensionData, flexionData, fingerData, dates });
               }*/
        public JsonResult GetData()
        {
            var extensionData = new List<int>();
            var flexionData = new List<int>();
            var fingerData = new List<string>();
            var fingerDataWithDates = new List<string>();

            // Retrieve data from database
            var fingerJoints = _context.FingerJoints.ToList();
            foreach (var fingerJoint in fingerJoints)
            {
                extensionData.Add(fingerJoint.Extension);
                flexionData.Add(fingerJoint.Flexion);
                fingerData.Add(fingerJoint.Finger);
                fingerDataWithDates.Add($"{fingerJoint.Finger} ({fingerJoint.Date.ToShortDateString()})");
            }

            return Json(new { extensionData, flexionData, fingerDataWithDates });
        }

        /*
                public IActionResult SortByName(string name)
                {
                    var allData = _context.FingerJoints.Where(p => p.Name == name).Distinct().ToList();
                    return View("Index", allData);
                }*/
        [HttpPost]
        public IActionResult SortByName(string selectedName)
        {
            // Fetch the names from the database
            var names = GetNames();

            // Store the names in the ViewData dictionary
            // ViewData["names"] = new SelectList(names);
            ViewData["names"] = new SelectList((System.Collections.IEnumerable)names, selectedName);
            // Store the selected name in the ViewData dictionary
            ViewData["selectedName"] = selectedName;

            // Get the data related to the selected name
            var data = GetDataForSelectedName(selectedName);

            return View("SortByName", data);
        }
        public async Task<IActionResult> GetNames()
        {
            var names = await _context.FingerJoints.Select(x => x.Name).Distinct().ToListAsync();
            return Json(names);
        }

        /*   public IActionResult GetDataForSelectedName(string selectedName)
           {

               var extensionData = new List<int>();
               var flexionData = new List<int>();
               var fingerData = new List<string>();
               var fingerDataWithDates = new List<string>();

               // Retrieve data from database
               var fingerJoints = _context.FingerJoints.Where(f => f.Name == selectedName).ToList();
               foreach (var fingerJoint in fingerJoints)
               {
                   extensionData.Add(fingerJoint.Extension);
                   flexionData.Add(fingerJoint.Flexion);
                   fingerData.Add(fingerJoint.Finger);
                   fingerDataWithDates.Add($"{fingerJoint.Finger} ({fingerJoint.Date.ToShortDateString()})");
               }

               return View("GetDataForSelectedName", fingerJoints);
           }*/

        /*        public IActionResult GetDataForSelectedName(string selectedName)
                {
                    var viewModel = new FingerJointDataViewModel
                    {
                        ExtensionData = new List<int>(),
                        FlexionData = new List<int>(),
                        FingerDataWithDates = new List<string>()
                    };

                    // Retrieve data from database
                    var fingerJoints = _context.FingerJoints.Where(f => f.Name == selectedName).ToList();
                    foreach (var fingerJoint in fingerJoints)
                    {
                        viewModel.ExtensionData.Add(fingerJoint.Extension);
                        viewModel.FlexionData.Add(fingerJoint.Flexion);
                        viewModel.FingerDataWithDates.Add($"{fingerJoint.Finger} ({fingerJoint.Date.ToShortDateString()})");
                    }

                    return View("GetDataForSelectedName", fingerJoints);
                }*/
        /*    public IActionResult GetDataForSelectedName(string selectedName)
            {
                var extensionData = new List<int>();
                var flexionData = new List<int>();
                var fingerData = new List<string>();
                var fingerDataWithDates = new List<string>();

                // Retrieve data from database
                var fingerJoints = _context.FingerJoints.Where(f => f.Name == selectedName).ToList();
                foreach (var fingerJoint in fingerJoints)
                {
                    extensionData.Add(fingerJoint.Extension);
                    flexionData.Add(fingerJoint.Flexion);
                    fingerData.Add(fingerJoint.Finger);
                    fingerDataWithDates.Add($"{fingerJoint.Finger} ({fingerJoint.Date.ToShortDateString()})");
                }

                *//*return View("GetDataForSelectedName", new { extensionData, flexionData, fingerDataWithDates });*//*
                return View(fingerJoints);
            }*/
        /*  [Route("/index")]*/
        /*      public JsonResult GetDataForSelectedName(string selectedName)
              {
                  var extensionData = new List<int>();
                  var flexionData = new List<int>();
                  var fingerData = new List<string>();
                  var fingerDataWithDates = new List<string>();

                  // Retrieve data from database
                  var fingerJoints = _context.FingerJoints.Where(f => f.Name == selectedName).ToList();
                  foreach (var fingerJoint in fingerJoints)
                  {
                      extensionData.Add(fingerJoint.Extension);
                      flexionData.Add(fingerJoint.Flexion);
                      fingerData.Add(fingerJoint.Finger);
                      fingerDataWithDates.Add($"{fingerJoint.Finger} ({fingerJoint.Date.ToShortDateString()})");
                  }

                  return Json(new { extensionData, flexionData, fingerDataWithDates });
              }*/
        /*        public IActionResult GetDataForSelectedName(string selectedName)
                {
                    var allData = _context.FingerJoints.Where(f => f.Name == selectedName).ToList();
                    //var names = allData.Select(x => x.Name).Distinct().ToList();
                    //ViewData["names"] = new SelectList(names);
                    return View(allData);
                }*/
        /*   public IActionResult GetDataForSelectedName(string selectedName)
           {
               var extensionData = new List<int>();
               var flexionData = new List<int>();
               var fingerData = new List<string>();
               var fingerDataWithDates = new List<string>();

               // Retrieve data from database
               var fingerJoints = _context.FingerJoints.Where(f => f.Name == selectedName).ToList();
               foreach (var fingerJoint in fingerJoints)
               {
                   extensionData.Add(fingerJoint.Extension);
                   flexionData.Add(fingerJoint.Flexion);
                   fingerData.Add(fingerJoint.Finger);
                   fingerDataWithDates.Add($"{fingerJoint.Finger} ({fingerJoint.Date.ToShortDateString()})");
               }

               return View("GetDataForSelectedName", new { extensionData, flexionData, fingerDataWithDates });
           }*/
        /*    [HttpGet]
            public JsonResult GetDataForSelectedName(string selectedName)
            {
                // Get the data for the selected name from the database
                var fingerJointData = _context.FingerJoints.Where(x => x.Name == selectedName).OrderBy(x => x.Date).ToList();

                // Initialize lists to store the extension and flexion data
                var extensionData = new List<double>();
                var flexionData = new List<double>();
                var fingerDataWithDates = new List<string>();

                // Loop through the data for the selected name and add the extension and flexion data to the lists
                foreach (var data in fingerJointData)
                {
                    extensionData.Add(data.Extension);
                    flexionData.Add(data.Flexion);
                    fingerDataWithDates.Add(data.Date.ToString());
                }

                // Return the data as a JSON object
                return Json(new { extensionData, flexionData, fingerDataWithDates });
            }*/
        /*[HttpGet]
        public IActionResult GetDataForSelectedName(string selectedName)
        {
            var fingerJointData = _context.FingerJoints.Where(fj => fj.Name == selectedName).ToList();

            *//*     var fingerDataWithDates = fingerJointData.Select(fj => fj.Date).ToList();
                 var extensionData = fingerJointData.Select(fj => fj.Extension).ToList();
                 var flexionData = fingerJointData.Select(fj => fj.Flexion).ToList();*//*
            var fingerDataWithDates = fingerJointData.Select(fj => fj.Date.ToString("yyyy-MM-dd")).ToList();
            var extensionData = fingerJointData.Select(fj => fj.Extension).ToList();
            var flexionData = fingerJointData.Select(fj => fj.Flexion).ToList();

            var result = new { fingerDataWithDates, extensionData, flexionData };

            //return Json(new { extensionData, flexionData, fingerDataWithDates });
            return Json(result);
            //return View("GetDataForSelectedName", Json(result));
        }*/
        public IActionResult GetDataForSelectedName(string selectedName)
        {
            var extensionData = new List<int>();
            var flexionData = new List<int>();
            var fingerDataWithDates = new List<string>();

            // Retrieve data from database
            var fingerJoints = _context.FingerJoints.Where(fj => fj.Name == selectedName).ToList();
            foreach (var fingerJoint in fingerJoints)
            {
                extensionData.Add(fingerJoint.Extension);
                flexionData.Add(fingerJoint.Flexion);
                fingerDataWithDates.Add($"{fingerJoint.Finger} ({fingerJoint.Date.ToShortDateString()})");
            }
            HttpContext.Session.SetString("selectedName", selectedName);

            return View(fingerJoints);

        }
        /*        [HttpGet]
                public IActionResult GetDataForSelectedNameJson()
                {
                    var selectedName = HttpContext.Session.GetString("selectedName");

                    var extensionData = new List<int>();
                    var flexionData = new List<int>();
                    var fingerDataWithDates = new List<string>();

                    // Retrieve data from database
                    var fingerJoints = _context.FingerJoints.Where(fj => fj.Name == selectedName).ToList();
                    foreach (var fingerJoint in fingerJoints)
                    {
                        extensionData.Add(fingerJoint.Extension);
                        flexionData.Add(fingerJoint.Flexion);
                        fingerDataWithDates.Add($"{fingerJoint.Finger} ({fingerJoint.Date.ToShortDateString()})");
                    }

                    var result = new
                    {
                        ExtensionData = extensionData,
                        FlexionData = flexionData,
                        FingerDataWithDates = fingerDataWithDates
                    };

                    return Json(result);
                }*/
        [HttpGet]
        public IActionResult GetDataForSelectedNameJson()
        {
            var selectedName = HttpContext.Session.GetString("selectedName");

            var extensionData = new List<int>();
            var flexionData = new List<int>();
            var fingerDataWithDates = new List<string>();

            // Retrieve data from database
            var fingerJoints = _context.FingerJoints.Where(fj => fj.Name == selectedName).ToList();
            foreach (var fingerJoint in fingerJoints)
            {
                extensionData.Add(fingerJoint.Extension);
                flexionData.Add(fingerJoint.Flexion);
                fingerDataWithDates.Add($"{fingerJoint.Finger} ({fingerJoint.Date.ToShortDateString()})");
            }

            var result = new
            {
                ExtensionData = extensionData,
                FlexionData = flexionData,
                FingerDataWithDates = fingerDataWithDates
            };

            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int fid)
        {
            FingerJoint fingerJoint = _context.FingerJoints.Find(fid);
            _context.FingerJoints.Remove(fingerJoint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
