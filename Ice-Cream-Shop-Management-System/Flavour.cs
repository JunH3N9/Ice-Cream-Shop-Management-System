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
    internal class Flavour
    {
        private string type { get; set; }

        public string Type { get; set; }

        private bool premium { get; set; }

        public bool Premium { get; set; }

        private int quantity { get; set; }

        public int Quantity { get; set; }
        
        public Flavour() { }

        public Flavour(string type, bool premium, int quantity) 
        {
            Type = type;
            Premium = premium;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return type;
        }

    }
}
