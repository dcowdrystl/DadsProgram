﻿using DadsProgram.Data;
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
            var names = _context.FingerJoints
                    .Select(fj => fj.Name)
                    .Distinct()
                    .AsEnumerable()
                    .OrderBy(name => GetLastName(name))
                    .ToList();
            return View(names);
        }
        public static string GetLastName(string name)
        {
            int index = name.LastIndexOf(" ");
            if (index != -1)
            {
                return name.Substring(index + 1);
            }

            return name;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,ActiveExtension,PassiveExtension,ActiveFlexion,PassiveFlexion,Finger,Date,Description")] FingerJoint fingerJoint)
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
                extensionData.Add(fingerJoint.PassiveExtension);
                flexionData.Add(fingerJoint.ActiveFlexion);
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
            var fingerJoints = _context.FingerJoints.Where(fj => fj.Name == selectedName).OrderBy(fj => fj.Date).ThenBy(fj => fj.Finger).ToList();

            // Add search functionality for selectedName
            if (!string.IsNullOrEmpty(selectedName))
            {
                fingerJoints = fingerJoints.Where(fj => fj.Name.Contains(selectedName)).ToList();
            }

            var fingerNames = fingerJoints.Select(fj => fj.Finger).Distinct().ToList();
            ViewData["fingerNames"] = fingerNames;
            foreach (var fingerJoint in fingerJoints)
            {
                extensionData.Add(fingerJoint.PassiveExtension);
                flexionData.Add(fingerJoint.ActiveFlexion);
                fingerDataWithDates.Add($"{fingerJoint.Finger} ({fingerJoint.Date.ToShortDateString()})");
            }
            HttpContext.Session.SetString("selectedName", selectedName);
            ViewData["selectedName"] = selectedName;
            string nameFilter = HttpContext.Session.GetString("selectedName");

            return View(fingerJoints);
        }

        /*  [HttpGet]
          public IActionResult GetDataForSelectedNameJson(string nameFilter, string fingerFilter)
          {
              var data = _context.FingerJoints
                  .Where(fj => (string.IsNullOrEmpty(nameFilter) || fj.Name.Contains(nameFilter)) && (string.IsNullOrEmpty(fingerFilter) || fj.Finger == fingerFilter))
                  .OrderBy(fj => fj.Date)
                  .ToList();

              var pextensionData = data.Select(fj => fj.PassiveExtension).ToList();
              var aextensionData = data.Select(fj => fj.ActiveExtension).ToList();
              var aflexionData = data.Select(fj => fj.ActiveFlexion).ToList();
              var pflexionData = data.Select(fj => fj.PassiveFlexion).ToList();
              var fingerDataWithDates = data.Select(fj => fj.Date.ToString("yyyy-MM-dd")).ToList();

              return Json(new { pextensionData, aextensionData, aflexionData, pflexionData, fingerDataWithDates });
          }*/
        [HttpGet]
        public IActionResult GetDataForSelectedNameJson(string nameFilter, string fingerFilter, bool pExtensionChecked, bool aExtensionChecked, bool pFlexionChecked, bool aFlexionChecked)
        {
            var data = _context.FingerJoints
                .Where(fj => (string.IsNullOrEmpty(nameFilter) || fj.Name.Contains(nameFilter)) && (string.IsNullOrEmpty(fingerFilter) || fj.Finger == fingerFilter))
                .OrderBy(fj => fj.Date)
                .ToList();

            List<int> pextensionData = pExtensionChecked ? data.Select(fj => fj.PassiveExtension).ToList() : new List<int>();
            List<int> aextensionData = aExtensionChecked ? data.Select(fj => fj.ActiveExtension).ToList() : new List<int>();
            List<int> pflexionData = pFlexionChecked ? data.Select(fj => fj.PassiveFlexion).ToList() : new List<int>();
            List<int> aflexionData = aFlexionChecked ? data.Select(fj => fj.ActiveFlexion).ToList() : new List<int>();
            var fingerDataWithDates = data.Select(fj => fj.Date.ToString("yyyy-MM-dd")).ToList();

            return Json(new { pextensionData, aextensionData, aflexionData, pflexionData, fingerDataWithDates });
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
