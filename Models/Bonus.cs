using System;
using System.Collections.Generic;



namespace tephraSystemEditor.Models
{
    public enum  Mod {Acc, Eva, Stk, Def, Pri, Spd, Aug, Diy, Wnd, Hp}
    public class Bonus
    {
        public int ID {get; set;}
        public int specialtyID {get; set;}
        public Mod mod {get; set;}
        public int Value {get; set;}

    }
}