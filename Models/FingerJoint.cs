using System;

namespace DadsProgram.Models
{
    public class FingerJoint
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Extension { get; set; }
        public int Flexion { get; set; }
        public int ActiveValue { get; set; }
        public int PassiveValue { get; set; }
        public string Finger { get; set; }
        public DateTime Date { get; set; }
        public FingerJoint() { }


    }
}
