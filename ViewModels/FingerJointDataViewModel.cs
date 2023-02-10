using DadsProgram.Models;
using System.Collections.Generic;

namespace DadsProgram.ViewModels
{
    public class FingerJointDataViewModel
    {
        public string selectedName { get; set; }
        public string Finger { get; set; }
        public List<FingerJoint> fingerJoints { get; set; }
    }
}
