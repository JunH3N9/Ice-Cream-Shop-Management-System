using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
//==========================================================
// Student Number : S10259970K
// Student Name : Teo Jun Heng
// Partner Name : Tan Ming Xuan
//==========================================================

namespace PRG_assignment
{
    internal class Waffle: IceCream
    {
        private string waffleFlavour;
        public string WaffleFlavour { get; set; }

        public Waffle() { }
        public Waffle(string _option, int _scoops, List<Flavour> _flavours, List<Topping> _toppings, string _waffle)
        {
            Option = _option;
            Scoops = _scoops;
            Flavours = _flavours;
            Toppings = _toppings;
            WaffleFlavour = _waffle;
        }
        public override double CalculatePrice()
        {
            double basePrice = 7.00;
            double premiumScoopPrice = 2.00;

            if (Scoops == 2)
            {
                basePrice = 8.50;
            }
            else if (Scoops == 3)
            {
                basePrice = 9.50;
            }

            if (Flavours.Any(flavour => flavour.Premium))
            {
                basePrice += premiumScoopPrice;
            }

            return basePrice;
        }
        public override string ToString()
        {
            return "waffle";
        }
    }
}
