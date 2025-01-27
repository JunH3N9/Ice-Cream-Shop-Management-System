using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
//==========================================================
// Student Number : S1025970K
// Student Name : Teo Jun Heng
// Partner Name : Tan Ming Xuan
//==========================================================

namespace PRG_assignment
{
    internal class Order : IComparable<Order>
    {
        private int id;
        public int Id { get; set; }

        private DateTime timeReceived;
        public DateTime TimeReceived { get; set; }

        private DateTime? timeFulfilled;
        public DateTime? TimeFulfilled { get; set; }

        private List<IceCream> iceCreamList;
        public List<IceCream> IceCreamList { get; set; }

        public Order() { }

        public Order(int _id, DateTime _timeReceived)
        {
            Id = _id;
            TimeReceived = _timeReceived;
            IceCreamList = new List<IceCream>();
        }

        string GetIceCreamOption()
        {
            string iceCreamOption = " ";
            while (true)
            {
                Console.WriteLine("Options: \n1. Waffle \n2. Cone \n3. Cup");
                Console.Write("Enter the new ice cream option: ");
                try
                {
                    iceCreamOption = Console.ReadLine().ToLower();
                    if (iceCreamOption == null) //if input is null
                    {
                        throw new ArgumentNullException();
                    }
                    else if (char.IsDigit(iceCreamOption[0])) //If option is not string
                    {
                        throw new FormatException();
                    }
                    else if (iceCreamOption != "waffle" && iceCreamOption != "cone" && iceCreamOption != "cup") //check if input is valid
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                catch (FormatException) //input is not string
                {
                    Console.WriteLine("Invalid option. Please input the name of the option. \n");
                }
                catch (ArgumentOutOfRangeException) //input is not valid
                {
                    Console.WriteLine("Please choose from the list of ice cream options. \n");
                }
                catch (ArgumentNullException) //input is null
                {
                    Console.WriteLine("Input is null");
                }

                if (iceCreamOption == "waffle")
                {
                    string waffleFlavour = " ";
                    while (true)
                    {
                        Console.WriteLine("0. Exit \n1. Red velvet \n2. Charcoal \n3. Pandan \n4. Plain");
                        Console.Write("Enter waffle flavour: ");
                        try
                        {
                            waffleFlavour = Console.ReadLine().ToLower();
                            if (waffleFlavour == null) //check if input is not null
                            {
                                throw new ArgumentNullException();
                            }
                            else if (waffleFlavour == "0")
                            {
                                break;
                            }
                            else if (char.IsDigit(waffleFlavour[0])) // check if input is not int
                            {
                                throw new FormatException();
                            }
                            else if (waffleFlavour != "red velvet" && waffleFlavour != "charcoal" && waffleFlavour != "pandan" && waffleFlavour != "plain") //check if input is valid
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                            return waffleFlavour;
                        }
                        catch (FormatException) //input is not string
                        {
                            Console.WriteLine("Invalid option. Please input the name of the option. \n");
                        }
                        catch (ArgumentOutOfRangeException) // input is not valid
                        {
                            Console.WriteLine("Please choose from the list of waffle flavour. \n");
                        }
                        catch (ArgumentNullException) //input is null
                        {
                            Console.WriteLine("Input is null");
                        }
                    }
                }
                else if (iceCreamOption == "cone")
                {
                    while (true)
                    {
                        Console.Write("Do you want to dip your ice cream? (Y/N) ");
                        try
                        {
                            string coneDipped = Console.ReadLine().ToLower();
                            if (coneDipped == null) //check if input is not null
                            {
                                throw new ArgumentNullException();
                            }
                            else if (char.IsDigit(iceCreamOption[0])) //check if input is not string
                            {
                                throw new FormatException();
                            }
                            else if (coneDipped != "y" && coneDipped != "n") //check if input is valid
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                            return coneDipped;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid option. Please input a letter. \n");
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            Console.WriteLine("Please input y or n only. \n");
                        }
                        catch (ArgumentNullException) //input is null
                        {
                            Console.WriteLine("Input is null");
                        }
                    }
                }             
                else if (iceCreamOption == "cup")
                {
                    return "cup";
                }
            }
        }
        int GetScoops()
        {
            while (true)
            {
                int scoops = 0;
                Console.Write("Enter the new number of scoops: ");
                try
                {
                    scoops = Convert.ToInt32(Console.ReadLine());
                    if (scoops == null) //check if input is not null
                    {
                        throw new ArgumentNullException();
                    }
                    else if (scoops < 1 || scoops > 3) //check if input is valid
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    return scoops;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid option. Please input an integer. \n");
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Ice cream scoops only can be between 1 and 3. \n");
                }
                catch (ArgumentNullException) //input is null
                {
                    Console.WriteLine("Input is null");
                }
            }            
        }
        List<Flavour>GetFlavours(int scoops)
        {
            while (true)
            {
                List<Flavour> flavourList = new List<Flavour>();
                Console.WriteLine("\nRegular Flavours:\nVanilla\nChocolate\nStrawberry\n" +
                    "\nPremium Flavours:\nDurian\nUbe\nSea salt\n");
                Console.Write("Enter new flavours (Seperated by commas): ");
                try
                {
                    string[] newFlavours = Console.ReadLine().Split(",");
                    foreach (var _flavour in newFlavours)
                    {
                        string flavour = _flavour.ToLower().Trim();
                        if (flavour == null) //if input is null
                        {
                            throw new ArgumentNullException();
                        }
                        else if (char.IsDigit(flavour[0])) //if input is not string
                        {
                            throw new FormatException();
                        }
                        bool premiumFlavour = false;
                        if (flavour == "durian" || flavour == "ube" || flavour == "sea salt")
                        {
                            premiumFlavour = true;
                        }
                        if (flavour != "durian" && flavour != "ube" && flavour != "sea salt" && flavour != "vanilla" && flavour != "chocolate" && flavour != "strawberry")
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        Flavour flavours = new Flavour(flavour, premiumFlavour, scoops); //create new flavours
                        flavourList.Add(flavours);                        
                    }
                    return flavourList;
                }
                catch (FormatException) //input is not correct type
                {
                    Console.WriteLine("Invalid option. Please input the names, seperated by commas. \n");
                }
                catch (ArgumentOutOfRangeException) //input is not valid
                {
                    Console.WriteLine("Please choose from the list of flavours. \n");
                }
                catch (ArgumentNullException) //input is null
                {
                    Console.WriteLine("Input is null");
                }
            }
        }
        List<Topping> GetToppings()
        {
            while (true)
            {
                List<Topping>toppingList = new List<Topping>();
                Console.WriteLine("\nToppings:\nSprinkles\nMochi\nSago\nOreos\n");
                Console.Write("Enter the new toppings (Separated by commas): ");
                try
                {
                    string[] newToppings = Console.ReadLine().Split(",");
                    foreach (var _topping in newToppings)
                    {
                        string topping = _topping.ToLower().Trim();
                        if (topping == null) //if input is null
                        {
                            throw new ArgumentNullException();
                        }
                        else if (char.IsDigit(topping[0])) //if input is not string
                        {
                            throw new FormatException();
                        }
                        else if (topping != "sprinkles" && topping != "mochi" && topping != "sago" && topping != "oreos")
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        Topping newTopping = new Topping(topping);
                        toppingList.Add(newTopping);
                    }
                    return toppingList;
                }
                catch (FormatException) //input is not correct type
                {
                    Console.WriteLine("Invalid option. Please input the names, seperated by commas. ");
                }
                catch (ArgumentOutOfRangeException) //input is not valid
                {
                    Console.WriteLine("Please choose from the list of flavours. ");
                }
                catch (ArgumentNullException) //input is null
                {
                    Console.WriteLine("Input is null");
                }
            }
        }   
        public void ModifyIceCream(int option)
        {
            if (option == 1)
            {
                int index = -1;
                while (true)
                {
                    Console.Write("Enter ice cream number: "); //Choose ice cream
                    try
                    {
                        index = Convert.ToInt32(Console.ReadLine()) - 1;
                        if (index == null) //check if input is not null
                        {
                            throw new ArgumentNullException();
                        }
                        else if (index + 1 > IceCreamList.Count()) //check if input is within limit
                        {
                            throw new ArgumentOutOfRangeException("Please choose from the list of ice cream shown.");
                        }
                        break;
                    }

                    //Catching
                    catch (FormatException) //input is not int
                    {
                        Console.WriteLine("Invalid option. Please input an integer. \n");
                    }
                    catch (ArgumentOutOfRangeException) //input is not valid
                    {
                        Console.WriteLine("Please choose from the list of ice cream. \n");
                    }
                    catch (ArgumentNullException) //input is null
                    {
                        Console.WriteLine("Input is null");
                    }
                }
                IceCream iceCream = IceCreamList[index]; //Get ice cream to modify

                int optionSelected = -1;
                while (true)
                {
                    Console.WriteLine("0. Exit \n1. Option \n2. Scoops \n3. Flavours \n4. Toppings"); //show options
                    if (iceCream is Waffle) //Extra option for waffle
                    {
                        Console.WriteLine("5. Waffle flavour");
                    }
                    else if (iceCream is Cone) //Extra option for cone
                    {
                        Console.WriteLine("5. Dipped cone");
                    }
                    Console.Write("Select information to modify: ");
                    try
                    {
                        optionSelected = Convert.ToInt32(Console.ReadLine());
                        if (optionSelected == null) //check if input is not null
                        {
                            throw new ArgumentNullException();
                        }
                        else if (optionSelected == 0)
                        {
                            break;
                        }
                        else if (optionSelected < 0 || optionSelected > 5) //check if input is valid(for waffle and cone)
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        else if (optionSelected < 0 || optionSelected > 4)//check if input is valid (for cone)
                        {
                            if (!(iceCream is Waffle || iceCream is Cone))
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                        }
                        break;
                    }

                    //Catching
                    catch (FormatException) //input is not integer
                    {
                        Console.WriteLine("Invalid option. Please input an integer. \n");
                    }
                    catch (ArgumentOutOfRangeException) //input is not valid
                    {
                        Console.WriteLine("Please choose from the list of option. \n");
                    }
                    catch (ArgumentNullException) //input is null
                    {
                        Console.WriteLine("Input is null");
                    }
                }

                if (optionSelected == 0) { return; }
                else if (optionSelected == 1)
                {
                    string iceCreamType = GetIceCreamOption();
                    if (iceCreamType != "red velvet" && iceCreamType != "charcoal" && iceCreamType != "pandan" && iceCreamType != "plain") //if not waffle
                    {
                        if (iceCreamType== "y" || iceCreamType == "n") // if is cone
                        {
                            bool dipped = false;
                            if (iceCreamType == "y") { dipped = true; }
                            Cone newIceCream = new Cone("Cone", iceCream.Scoops, iceCream.Flavours, iceCream.Toppings, dipped); //create new ice cream                            
                            IceCreamList[index] = newIceCream; //Add new ice cream
                        }
                        else if (iceCreamType == "cup") //if is cup
                        {
                            Cup newIceCream = new Cup("Cup", iceCream.Scoops, iceCream.Flavours, iceCream.Toppings);
                            IceCreamList[index] = newIceCream; //Add new ice cream
                        }
                    }
                    else
                    {
                        Waffle newIceCream = new Waffle("Waffle", iceCream.Scoops, iceCream.Flavours, iceCream.Toppings, iceCreamType); //Create waffle
                        IceCreamList[index] = newIceCream; //Add new ice cream
                    }                 
                }                                
                                
                else if (optionSelected == 2)
                {
                    int newScoops = GetScoops(); //Get number of scoops
                    iceCream.Scoops = newScoops; //Update the number of scoops
                }
                else if (optionSelected == 3)
                {
                    List<Flavour> flavourList = GetFlavours(iceCream.Scoops); //Get flavours
                    iceCream.Flavours = flavourList; //Update the flavours
                }
                else if (optionSelected == 4)
                {
                    List<Topping> toppingList = GetToppings(); //Get toppings
                    iceCream.Toppings = toppingList; //Update toppings
                }
                else if (optionSelected == 5)
                {
                    while (true)
                    {
                        if (iceCream is Waffle)
                        {
                            string waffleFlavour = " ";
                            Waffle IceCream = (Waffle)iceCream;
                            Console.WriteLine("0. Exit \n1.Red velvet \n2. Charcoal \n3. Pandan \n4. Plain");
                            Console.Write("Enter waffle flavour: ");
                            try
                            {
                                waffleFlavour = Console.ReadLine().ToLower();
                                if (waffleFlavour == null) //check if input is not null
                                {
                                    throw new ArgumentNullException();
                                }
                                else if (char.IsDigit(waffleFlavour[0])) //check if input is not string
                                {
                                    throw new FormatException();
                                }
                                else if (waffleFlavour != "red velvet" && waffleFlavour != "charcoal" && waffleFlavour != "pandan" && waffleFlavour != "plain") //check if input is valid
                                {
                                    throw new ArgumentOutOfRangeException();
                                }
                                IceCream.WaffleFlavour = waffleFlavour; //change the waffle flavour
                                break;
                            }

                            catch (FormatException) //input not string
                            {
                                Console.WriteLine("Invalid option. Please input the name of the option. \n");
                            }
                            catch (ArgumentOutOfRangeException) //input not valid
                            {
                                Console.WriteLine("Please choose from the list of waffle flavour. \n");
                            }
                            catch (ArgumentNullException) //input is null
                            {
                                Console.WriteLine("Input is null");
                            }
                        }

                        else if (iceCream is Cone)
                        {
                            string coneDipped = " ";
                            Cone IceCream = (Cone)iceCream;
                            Console.Write("Do you want your ice cream dipped? (Y/N): ");
                            try
                            {
                                coneDipped = Console.ReadLine().ToLower();
                                if (coneDipped == null) //check if input is not null
                                {
                                    throw new ArgumentNullException();
                                }
                                else if (char.IsDigit(coneDipped[0])) //check if input is not string
                                {
                                    throw new FormatException();
                                }
                                else if (coneDipped != "y" && coneDipped != "n") //check if input is valid
                                {
                                    throw new ArgumentOutOfRangeException();
                                }

                                //update whether cone is dipped
                                if (coneDipped == "y")
                                {
                                    IceCream.Dipped = true;
                                }
                                else if (coneDipped == "n")
                                {
                                    IceCream.Dipped = false;
                                }
                                break;
                            }

                            catch (FormatException) //input not string
                            {
                                Console.WriteLine("Invalid option. Please input the name of the option. \n");
                            }
                            catch (ArgumentOutOfRangeException) //input not valid
                            {
                                Console.WriteLine("Please input y or n only. \n");
                            }
                            catch (ArgumentNullException) //input is null
                            {
                                Console.WriteLine("Input is null");
                            }
                        }
                    }
                    Console.WriteLine("Ice Cream order modified succesfully.");
                }
            }

            if (option == 2)
            {
                string iceCreamOption = " ";
                string iceCreamType = GetIceCreamOption();
                bool dipped = false;

                //get the type of ice cream
                if (iceCreamType != "red velvet" && iceCreamType != "charcoal" && iceCreamType != "pandan" && iceCreamType != "plain") //is not waffle
                {
                    if (iceCreamType == "y" || iceCreamType == "n") // is cone
                    {                        
                        if (iceCreamType == "y") { dipped = true; }
                        iceCreamOption = "cone";
                    }
                    else if (iceCreamType == "cup") //is cup
                    {
                        iceCreamOption = "cup";
                    }
                }
                else // is waffle
                {
                    iceCreamOption = "waffle"; 
                }

                int scoops = GetScoops(); //get scoops

                List<Flavour> flavourList = GetFlavours(scoops); //get flavours

                List<Topping> toppingList = GetToppings(); //get toppings

                //create the ice cream accordingly and add to current order
                if (iceCreamOption == "waffle")
                {
                    IceCream iceCream = new Waffle("Waffle", scoops, flavourList, toppingList, iceCreamType);
                    IceCreamList.Add(iceCream);
                }
                else if (iceCreamOption == "cone")
                {
                    IceCream iceCream = new Cone("Cone", scoops, flavourList, toppingList, dipped);
                    IceCreamList.Add(iceCream);
                    Console.WriteLine(1);
                }
                else if (iceCreamOption == "cup")
                {
                    IceCream iceCream = new Cup("Cup", scoops, flavourList, toppingList);
                    IceCreamList.Add(iceCream);
                }                
            }

            if (option == 3)
            {
                int num = 0;
                while (true)
                {                    
                    try
                    {
                        if (IceCreamList.Count <= 1) //Order cannot have no ice cream
                        {
                            Console.WriteLine("Order cannot have zero ice creams \n");
                            break;
                        }
                        Console.Write("Which ice cream do you want to delete (Number): ");
                        num = Convert.ToInt32(Console.ReadLine()) - 1;
                        if (num == null) //check if input is not null
                        {
                            throw new ArgumentNullException();
                        }                                            
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid option. Please input an integer. \n");
                    }
                    catch (ArgumentNullException) //input is null
                    {
                        Console.WriteLine("Input is null");
                    }
                    IceCreamList.RemoveAt(num);
                    Console.WriteLine(" ");
                    break;
                }
            }
        }
        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }

        public int CompareTo(Order date)
        {
            if (date == null)
            {
                return 1;
            }

            return TimeReceived.CompareTo(date.TimeReceived);
        }

        public double CalculateTotal()
        {
            double total = 0.0;

            foreach (IceCream iceCream in IceCreamList)
            {

                total += iceCream.CalculatePrice();
            }
            return total;
        }
        public override string ToString()
        {
            return "MMM";
        }
    }   
}
