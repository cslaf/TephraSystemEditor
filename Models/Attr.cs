using System;
using System.Collections.Generic;

namespace tephraSystemEditor.Models
{
    public class Attr
    {
        public int ID {get; set;}
        public String Name {get; set;}
        public String Description {get; set;}
        public List<Skill> Skills {get; set;}

    }
}