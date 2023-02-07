using DadsProgram.Data;
using DadsProgram.Models;
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
            var names = allData.Select(x => x.Name).Distinct().ToList();
            ViewData["names"] = new SelectList(names);
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

        public IActionResult GetDataForSelectedName(string selectedName)
        {
            //Get data from database based on the selected name
            var data = _context.FingerJoints.Where(f => f.Name == selectedName).ToList();

            //Transform the data into required format
            var extensionData = data.Select(f => f.Extension).ToList();
            var flexionData = data.Select(f => f.Flexion).ToList();

            //Return the data as a JSON object
            // return Json(new { extensionData, flexionData });
            return View("Index", data);
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
