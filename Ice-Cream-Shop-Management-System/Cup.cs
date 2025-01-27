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
    internal class Cup : IceCream
    {
        public Cup() { }

        public Cup(string option, int scoops, List<Flavour> _flavours, List<Topping> _toppings)  
        {
            Option = option;
            Scoops = scoops;
            Flavours = _flavours;
            Toppings = _toppings;
        }
        public override double CalculatePrice()
        {
            double basePrice = 4.00;
            double premiumScoopPrice = 2.00;

            if (Scoops == 2)
            {
                basePrice = 5.50;
            }
            else if (Scoops == 3)
            {
                basePrice = 6.50;
            }

            if (Flavours.Any(flavour => flavour.Premium))
            {
                basePrice += premiumScoopPrice;
            }

            return basePrice;
        }
        public override string ToString()
        {
            return $"{Option,-10}{Scoops,-10}";
        }
    }
}
