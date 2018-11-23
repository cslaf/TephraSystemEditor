using System;
using System.Collections.Generic;
namespace tephraSystemEditor.Models
{
    public class Specialty
    {
        public Specialty(){
            Bonuses = new List<Bonus>(){
                new Bonus(),
                new Bonus(),
                new Bonus()
            };
            
        }
        public int ID {get; set;}
        public int SpecialtyID {get; set;}
        public int CharacterID {get; set;}
        public int SkillID {get; set;}
        public String Name {get; set;}
        public String Description {get; set;}
        public List<Bonus> Bonuses {get; set;}
        public String Notes {get; set;}
    }
}