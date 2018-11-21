using System;
using System.Collections.Generic;

namespace tephraSystemEditor.Models
{
    public class Skill
    {
        public int ID {get; set;}
        public String Description {get; set;}
        public int attributeID {get; set;}
        public String Name {get; set;}
        public IEnumerable<Specialty> Specialties {get; set;}
    }
}