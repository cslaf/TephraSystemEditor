using System;
using System.Collections.Generic;



namespace tephraSystemEditor.Models
{
    public enum Modifier {Acc, Eva, Stk, Def, Pri, Spd, Aug, Diy, Wnd, Hp}
    public class Bonus
    {
        public int ID {get; set;}
        public int specialtyID {get; set;}
        public Modifier mod {get; set;}
        public int Value {get; set;}

    }
}