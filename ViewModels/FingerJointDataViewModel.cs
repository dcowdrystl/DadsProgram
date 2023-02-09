using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DadsProgram.ViewModels
{
    public class FingerJointDataViewModel
    {
        public List<int> ExtensionData { get; set; }
        public List<int> FlexionData { get; set; }
        public List<string> FingerDataWithDates { get; set; }
    }
}
