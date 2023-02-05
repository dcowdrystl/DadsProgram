using DadsProgram.Data;
using DadsProgram.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
            return View(_context.FingerJoints.ToList());
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

            // Retrieve data from database
            var fingerJoints = _context.FingerJoints.ToList();
            foreach (var fingerJoint in fingerJoints)
            {
                extensionData.Add(fingerJoint.Extension);
                flexionData.Add(fingerJoint.Flexion);
                fingerData.Add(fingerJoint.Finger);
            }

            return Json(new { extensionData, flexionData, fingerData });
        }

    }
}
