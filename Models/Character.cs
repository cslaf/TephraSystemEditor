using System;
using System.Collections.Generic;

namespace tephraSystemEditor.Models
{
    public class Character
    {
        public Character(){
            Specialties = new List<Specialty>();
        }
        public int ID {get; set;}
        public string UserID {get; set;}
        public string Name {get; set;}
        public List<Specialty> Specialties {get; set;}
        public string Description { get; set;}
        public int Level { get; set;}
        //Add equipment and inventory later

    }

}