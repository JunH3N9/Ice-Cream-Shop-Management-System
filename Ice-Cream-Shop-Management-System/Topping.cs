using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//==========================================================
// Student Number : S10259970K
// Student Name : Teo Jun Heng
// Partner Name : Tan Ming Xuan
//==========================================================

namespace PRG_assignment
{
    internal class Topping
    {
        private string type { get; set; }
        public string Type { get; set; }

        public Topping() { }

        public Topping(string _type) 
        { 
            Type = _type; 
        }

        public override string ToString() 
        { 
            return type;
        }

        
    }
}
