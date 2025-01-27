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
    internal abstract class IceCream
    {
        private string option;
        public string Option { get; set; }

        private int scoops;
        public int Scoops { get; set; }

        private List<Flavour> flavours;
        public List<Flavour> Flavours { get; set; }

        private List<Topping> toppings;
        public List<Topping> Toppings { get; set; }

        public IceCream() { }

        public IceCream(string Option, int Scoops, List<Flavour> Flavours, List<Topping> Toppings)
        {
            option = Option;
            scoops = Scoops;
            flavours = Flavours;
            toppings = Toppings;
        }

        public abstract double CalculatePrice();
        public abstract string ToString();
        
    }
}