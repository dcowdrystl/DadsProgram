using DadsProgram.Data;
using DadsProgram.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            /*            var allData = _context.FingerJoints.ToList();
                        var names = allData.Select(x => x.Name).Distinct().ToList();
                        ViewData["names"] = new SelectList(names);
                        var fingers = allData.Select(x => x.Finger).Distinct().ToList();
                        ViewData["fingers"] = new SelectList(fingers);
                        return View(allData);*/
            var names = _context.FingerJoints.Select(fj => fj.Name).Distinct().ToList();
            return View(names);
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
            string nameFilter = HttpContext.Session.GetString("selectedName");

            return View(fingerJoints);

        }

        [HttpGet]
        public IActionResult GetDataForSelectedNameJson(string nameFilter, string fingerFilter)
        {
            var data = _context.FingerJoints
                .Where(fj => (string.IsNullOrEmpty(nameFilter) || fj.Name.Contains(nameFilter)) && (string.IsNullOrEmpty(fingerFilter) || fj.Finger == fingerFilter))
                .OrderBy(fj => fj.Date)
                .ToList();

            var extensionData = data.Select(fj => fj.Extension).ToList();
            var flexionData = data.Select(fj => fj.Flexion).ToList();
            var fingerDataWithDates = data.Select(fj => fj.Date.ToString("yyyy-MM-dd")).ToList();

            return Json(new { extensionData, flexionData, fingerDataWithDates });
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
