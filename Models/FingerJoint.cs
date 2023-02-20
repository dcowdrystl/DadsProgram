using System;
using System.ComponentModel.DataAnnotations;

namespace DadsProgram.Models
{
    public class FingerJoint
    {
        public int ID { get; set; }
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Please enter only alphabets for the name field")]
        public string Name { get; set; }
        public string Description { get; set; }

        public int ActiveExtension { get; set; }
        public int PassiveExtension { get; set; }
        public int ActiveFlexion { get; set; }
        public int PassiveFlexion { get; set; }

        public string Finger { get; set; }
        public DateTime Date { get; set; }
        public FingerJoint()
        {
            if (Date == DateTime.MinValue)
                Date = DateTime.Now;
        }


    }
}