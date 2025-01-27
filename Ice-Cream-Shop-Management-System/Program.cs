// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic.FileIO;
using PRG_assignment;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.WebSockets;
using System.Xml.Serialization;
//==========================================================
// Student Number : S10259970K
// Student Name : Teo Jun Heng
// Partner Name : Tan Ming Xuan
//==========================================================
void displayMenu()
{
    Console.WriteLine("Menu \n=====================\n[1] List all customer \n[2] List all current orders \n[3] Register a new customer \n" +
        "[4] Create an order \n[5] Display order details of customer \n[6] Modify order details \n[7] Process an order and checkout" +
        "\n[8] Display monthly charged amounts breakdown & total charged amounts for the year \n[0] Exit \n===================== ");
}

//Read the customers.csv
List<Customer> customerList = new List<Customer>(); //List of all the customers 
using (StreamReader sr = new StreamReader("customers.csv"))
{
    string? s = sr.ReadLine();

    while ((s = sr.ReadLine()) != null)
    {
        string[] split = s.Split(",");
        string name = split[0]; //Get customer name
        int memberID = Convert.ToInt32(split[1]); //Get customer's ID
        DateTime dob = DateTime.Parse(split[2]); //Get customer's date of birth
        PointCard pointCard = new PointCard(Convert.ToInt32(split[4]), Convert.ToInt32(split[5])); //Create new point card
        pointCard.Tier = split[3];

        Customer customer = new Customer(name, memberID, dob) //Create new customer object
        {
            Rewards = pointCard
        };
        customerList.Add(customer); //add newly created customer to list
    }
}
//Create icecreams
List<IceCream> createIceCream(string[] details, List<IceCream>iceCreams)
{
    //Create flavour and at them to a list
    List<Flavour> flavourList = new List<Flavour>();
    for (int i = 8; i < 11; i++)
    {
        if (string.IsNullOrWhiteSpace(details[i])) //Check if it blank
        {
            break;
        }
        else if (!string.IsNullOrWhiteSpace(details[i]))
        {
            bool flavourType = false;
            if (details[i] == "Durian" || details[i] == "Ube" || details[i] == "Sea Salt")
            {
                flavourType = true;
            }
            Flavour flavour = new Flavour(details[i], flavourType, Convert.ToInt32(details[5])); //Create new flavour
            flavourList.Add(flavour); //Add new flavour to list
        }
    }

    //Create topping and add to list
    List<Topping> toppingsList = new List<Topping>();
    for (int i = 11; i < 15; i++)
    {
        if (string.IsNullOrWhiteSpace(details[i])) { break; }
        else if (!string.IsNullOrWhiteSpace(details[i]))
        {
            Topping topping = new Topping(details[i]); //Create new topping
            toppingsList.Add(topping); //Add new topping to list
        }
    }

    if (details[4] == "Waffle") //waffle ice cream
    {
        //Create new waffle
        Waffle waffle = new Waffle(details[4], Convert.ToInt32(details[5]), flavourList, toppingsList, details[7]);
        iceCreams.Add(waffle);
        waffle.Flavours = flavourList;
        waffle.Toppings = toppingsList;
    }
    else if (details[4] == "Cone") //cone ice cream
    {
        bool flavourType = false;
        if (details[6] == "True") { flavourType = true; }

        //Create new cone
        Cone cone = new Cone(details[4], Convert.ToInt32(details[5]), flavourList, toppingsList, flavourType);
        iceCreams.Add(cone);
        cone.Flavours = flavourList;
        cone.Toppings = toppingsList;
    }
    else if (details[4] == "Cup") //cup ice cream
    {
        //Create new cup
        Cup cup = new Cup(details[4], Convert.ToInt32(details[5]), flavourList, toppingsList);
        iceCreams.Add(cup);
        cup.Flavours = flavourList;
        cup.Toppings = toppingsList;
    }
    return iceCreams;
}


//Reading the orders.csv
using (StreamReader sr = new StreamReader("orders.csv"))
{
    string? s = sr.ReadLine();

    while ((s = sr.ReadLine()) != null)
    {
        string[] details = s.Split(',');

        List<IceCream> iceCreams = new List<IceCream>(); //List of ice creams in the order
        int memberID = Convert.ToInt32(details[1]);
        foreach (Customer customer in customerList)
        {
            if (customer.MemberId == memberID) //To get wanted customer
            {
                int orderID = Convert.ToInt32(details[0]);
                bool found = false;

                //Create order history if not created yet
                if (customer.OrderHistory == null)
                {
                    List<Order> orderHistory = new List<Order>();
                    customer.OrderHistory = orderHistory;
                }
                else
                {
                    foreach(Order previousOrder in customer.OrderHistory)
                    {
                        if (previousOrder.Id == orderID)//If order already exists
                        {
                            found = true;
                            Order order = previousOrder;
                            iceCreams = order.IceCreamList;
                            List<IceCream> iceCreamList = createIceCream(details, iceCreams); //Call method to create ice cream
                            order.IceCreamList = iceCreamList; //Update order ice cream list
                            break;
                        }
                    }
                }
                if (found == false) //If order was not made yet
                {
                    Order order = customer.MakeOrder(Convert.ToInt32(details[0]), Convert.ToDateTime(details[2]));
                    if (!string.IsNullOrEmpty(details[3]))
                    {
                        order.TimeFulfilled = Convert.ToDateTime(details[3]);
                    }
                    else
                    {
                        continue;
                    }
                    List<IceCream> iceCreamList = createIceCream(details, iceCreams); //Call method to create ice cream
                    order.IceCreamList = iceCreamList; //Update order ice cream list     
                }               
            }
        }  
    }
}

List<Order> normalList = new List<Order>();
List<Order> goldList = new List<Order>();
foreach(Customer customer in customerList)
{
    if(customer.CurrentOrder != null)
    {
        if (customer.CurrentOrder.TimeFulfilled == null) //Queue order should not be fulfilled
        {
            if (customer.Rewards.Tier == "Gold")
            {
                goldList.Add(customer.CurrentOrder); //List of all orders that are in the gold queue
            }
            else
            {
                normalList.Add(customer.CurrentOrder); //List of all orders that are in the normal queue
            }
        }
        else
        {
            customer.CurrentOrder = null;
        }
    }
}

//Sort the lists by time
goldList.Sort(); 
normalList.Sort();

//Create the queue
Queue<Order> normalQueue = new Queue<Order>(normalList);
Queue<Order> goldQueue = new Queue<Order>(goldList);

//Display Ice cream details
void DisplayIceCream(Order order)
{
    int i = 1;
    foreach (IceCream iceCream in order.IceCreamList)
    {
        Console.WriteLine($"Order ID: {order.Id}({i})" +
            $"\nTime received: {order.TimeReceived,-18:dd/MM/yyyy hh:MM}" +
            $"\nTime fulfilled: {order.TimeFulfilled,-18:dd/MM/yyyy hh:MM}" +
            $"\nOption: {iceCream.Option,-8}" +
            $"\nNo. of scoops: {iceCream.Scoops,-7}");
        i++;

        if (iceCream.Option == "Waffle") //If ice cream is waffle
        {
            Waffle waffle = (Waffle)iceCream; //Downcast to waffle
            string waffleFlavour = waffle.WaffleFlavour;
            Console.WriteLine($"Waffle flavour: {waffleFlavour}"); //Display waffle flavour
        }
        else if (iceCream.Option == "Cone") //If ice cream is cone
        {
            string dipped = "No";
            Cone cone = (Cone)iceCream; //Downcast to cone
            if (cone.Dipped == true) { dipped = "Yes"; }
            Console.WriteLine($"Dipped: {dipped}"); //Display if cone is dipped or not
        }

        //Display all the flavours
        Console.Write("Ice cream flavours: ");        
        int flavourCount = iceCream.Flavours.Count;
        foreach (Flavour flavour in iceCream.Flavours)
        {
            Console.Write(flavour.Type);

            if (--flavourCount > 0) //If flavour is not the last one, add a comma behind
            {
                Console.Write(", ");
            }
        }      

        Console.Write("\nToppings: ");
        if (iceCream.Toppings != null) //if ice cream has toppings
        {
            int toppingCount = iceCream.Toppings.Count;
            foreach (Topping topping in iceCream.Toppings)
            {
                Console.Write(topping.Type);

                if (--toppingCount > 0) //If toppings is not the last one, add a comma behind
                {
                    Console.Write(", ");
                }
            }
        }
        else
        {
            Console.Write("No toppings available.");
        }
        Console.WriteLine("\n"); //leave a line between each order displayed
    }
}

void DisplayFlavours()
{
    Console.WriteLine("{0,-20} {1,-20}", "Regular Flavours", "Premium Flavours");
    Console.WriteLine("{0,-20} {1,-20}", "----------------", "----------------");
    Console.WriteLine("{0,-20} {1,-20}", "Vanilla", "Durian");
    Console.WriteLine("{0,-20} {1,-20}", "Chocolate", "Ube");
    Console.WriteLine("{0,-20} {1,-20}", "Strawberry", "Sea salt");
}

void DisplayToppings()
{
    Console.WriteLine("Toppings");
    Console.WriteLine("--------");
    Console.WriteLine("Sprinkles");
    Console.WriteLine("Mochi");
    Console.WriteLine("Sago");
    Console.WriteLine("Oreos");
}


//Option 1 (Ming Xuan)
void DisplayCustomerDetails()
{
    //header
    Console.WriteLine("{0,-10}{1,-10}{2,-15}{3,-20}{4,-20}{5,-20}", "Name", "MemberId", "DOB", "Membership Status", "Membership Points", "Punch Card");
    foreach (Customer customer in customerList) //get the customers
    {
        Console.WriteLine(customer.ToString() + customer.Rewards.ToString());
    }
    Console.WriteLine(""); //leave a line
}

//Option 2 (Jun Heng)
void CurrentOrder(Queue<Order> normalQueue, Queue<Order> goldQueue)
{
    Console.WriteLine("Gold queue:");
    foreach (Order order in goldQueue) //prints orders
    {
        DisplayIceCream(order); 
    }

    Console.WriteLine("Normal queue:"); 
    foreach (Order order in normalQueue) //prints orders
    {
       DisplayIceCream(order); 
    }
}

//Option 3 (Ming Xuan)
void RegisterNewCustomer()
{
    try
    {
        Console.WriteLine("Enter new customer name: ");   //prompt user to enter name
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name) || !name.Any(char.IsLetter)) //check if name is not empty or not a number
        {
            throw new ArgumentException("Customer name cannot be empty or number.");
        }

        Console.WriteLine("Enter new customer ID number: "); //prompt user to enter customer id 
        if (!int.TryParse(Console.ReadLine(), out int id) || id < 100000 || id > 999999) //check if id is 6 digits, not starting with 0 and not more than 6 digits
        {
            throw new ArgumentException("Invalid input for customer ID. Please enter a valid number.");
        }

        Console.WriteLine("Enter new customer date of birth (DD/MM/YYYY): ");  //prompt user to enter dob
        if (!DateTime.TryParse(Console.ReadLine(), CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dateOfBirth))  //convert input into Datetime object 
        {
            throw new ArgumentException("Invalid date of birth format. Please enter the date in DD/MM/YYYY format.");
        }

        Customer customer = new Customer(name, id, dateOfBirth);      //Create Customer and PointCard objects for the new customer
        PointCard pointcard = new PointCard(0, 0);
        customer.Rewards = pointcard;                                 //link customer membership points and tier
        pointcard.MembershipUpgrade();

        customerList.Add(customer);
        customer.OrderHistory = new List<Order>();

        string filePath = "customers.csv";
        using (StreamWriter sw = File.AppendText(filePath))             //Append customer information to the CSV file
        {
            string formatdateOfBirth = dateOfBirth.ToString("dd/MM/yyyy");
            sw.WriteLine("{0},{1},{2},{3},{4},{5}", customer.Name, customer.MemberId, formatdateOfBirth, pointcard.Tier, pointcard.Points, pointcard.PunchCard);
        }
        Console.WriteLine("Customer information has been appended to file.");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

//Option 4 (Ming Xuan)
void CreateCustomerOrder()
{
    try
    {
        DisplayCustomerDetails();
        Console.WriteLine("Select a customer ID to order: ");
        int selectedCustomerID;

        try
        {
            selectedCustomerID = Convert.ToInt32(Console.ReadLine());   //convert user input to an integer
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input for customer ID. Please enter a valid number.");
            return;
        }

        Customer selectedCustomer = customerList.FirstOrDefault(customer => customer.MemberId == selectedCustomerID); //find first element in list that match memberID

        if (selectedCustomer == null)
        {
            Console.WriteLine("Customer not found. ");
            return;
        }

        int OrderID = 0;
        using (StreamReader sr = new StreamReader("orders.csv")) //read existing orders to determine next OrderID
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] details = line.Split(',');
                    if (details.Length > 0)
                    {
                        int currentOrderID;
                        if (int.TryParse(details[0], out currentOrderID))
                        {
                            if (currentOrderID > OrderID)
                            {
                                OrderID = currentOrderID;
                            }
                        }
                    }
                }
            }
        }
        OrderID++;
        Order newOrder = new Order(OrderID, DateTime.Now);  //create order with orderID and time received

        string addOption;
        while (true)
        {
            try
            {
                Console.WriteLine("Enter Ice Cream order option: ");
                string option = Console.ReadLine();

                if (option.ToLower() != "waffle" && option.ToLower() != "cone" && option.ToLower() != "cup")
                {
                    Console.WriteLine("Invalid option. Please enter Waffle, Cone, or Cup.");
                    continue;
                }

                Console.WriteLine("Enter the amount of scoops: ");
                int scoops = Convert.ToInt32(Console.ReadLine());

                if (scoops < 1 || scoops > 3)
                {
                    Console.WriteLine("Invalid number of scoops. Please enter a number between 1 and 3.");
                    continue;
                }

                DisplayFlavours();
                Console.WriteLine("Enter the flavour of ice cream (Separated by commas): ");
                string[] flavour = Console.ReadLine().Split(",");

                List<Flavour> flavourList = new List<Flavour>();   //create new flavourList
                foreach (var iceCreamFlavour in flavour)
                {
                    bool isPremiumFlavor = (iceCreamFlavour.ToLower() == "durian" || iceCreamFlavour.ToLower() == "ube" || iceCreamFlavour.ToLower() == "sea salt");  //check if flavour chosen is Premium
                    flavourList.Add(new Flavour(iceCreamFlavour, isPremiumFlavor, scoops)); //add new flavour with true or false for if flavour is Premium
                }

                Console.WriteLine("Would you like to add toppings? (Y/N): ");
                string ToppingsOption = Console.ReadLine();
                List<Topping> toppingsList = new List<Topping>();
                if (ToppingsOption.ToUpper() == "Y")
                {
                    DisplayToppings();
                    Console.WriteLine("Enter the topping of ice cream (Separated by commas): ");
                    string[] iceCreamToppings = Console.ReadLine().Split(",");

                    foreach (var iceCreamTopping in iceCreamToppings)
                    {
                        toppingsList.Add(new Topping(iceCreamTopping));
                    }
                }
                else if (ToppingsOption.ToUpper() != "N")
                {
                    Console.WriteLine("Invalid input. Please enter Y for Yes or N for No.");
                    continue; // This will restart the loop and prompt the user for toppings input again.
                }

                IceCream iceCream;
                if (option.ToLower() == "cup")
                {
                    iceCream = new Cup(option, scoops, flavourList, toppingsList);  //create ice cream cup object
                    newOrder.AddIceCream(iceCream);
                }

                else if (option.ToLower() == "cone")
                {
                    Console.WriteLine("Upgrade to Chocolate-Dipped cone? (Y/N): ");
                    string yesNo = Console.ReadLine();                               //prompt user if they want to upgrade to chocolate dipped cone
                    if (yesNo.ToUpper() == "Y")
                    {
                        iceCream = new Cone(option, scoops, flavourList, toppingsList, true);      //create new ice cream cone option with true upgraded dipped cone
                    }

                    else if (yesNo.ToUpper() != "N")
                    {
                        Console.WriteLine("Invalid input. Please enter Y for Yes or N for No.");
                        continue;
                    }

                    else
                    {
                        iceCream = new Cone(option, scoops, flavourList, toppingsList, false);     //create new ice cream cone option with false upgraded dipped cone
                    }
                    newOrder.AddIceCream(iceCream);
                }

                else if (option.ToLower() == "waffle")
                {
                    string regularWaffle = "Original";                                               //set waffle to original flavour
                    Console.WriteLine("Upgrade to Red Velvet, Charcoal or Pandan Waffle? (Y/N): ");
                    string yesNo = Console.ReadLine();
                    if (yesNo.ToUpper() == "Y")
                    {
                        string waffleFlavour;
                        while (true)
                        {
                            Console.WriteLine("Enter which waffle flavour (Red velvet, Charcoal or Pandan): ");
                            waffleFlavour = Console.ReadLine();

                            if (waffleFlavour.ToLower() == "pandan" || waffleFlavour.ToLower() == "red velvet" || waffleFlavour.ToLower() == "charcoal")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid waffle flavor. Please enter Pandan, Red Velvet, or Charcoal.");
                            }
                        }
                        iceCream = new Waffle(option, scoops, flavourList, toppingsList, waffleFlavour);

                    }
                    else if (yesNo.ToUpper() != "N")
                    {
                        Console.WriteLine("Invalid input. Please enter Y for Yes or N for No.");
                        continue;
                    }
                    else
                    {
                        iceCream = new Waffle(option, scoops, flavourList, toppingsList, regularWaffle);
                    }
                    newOrder.AddIceCream(iceCream);
                }

                Console.WriteLine("Add another ice cream to your order? (Y/N): ");
                addOption = Console.ReadLine();
                if (addOption.ToUpper() != "Y" && addOption.ToUpper() != "N")
                {
                    Console.WriteLine("Invalid input. Please enter Y for Yes or N for No.");
                    continue;
                }
                else if (addOption.ToUpper() != "Y")
                {
                    break;
                }
            }

            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }
        }

        selectedCustomer.CurrentOrder = newOrder;
        if (selectedCustomer.Rewards.Tier == "Gold")   //check if customer membership is Gold, then add to the Gold Queue
        {
            goldQueue.Enqueue(newOrder);
        }
        else
        {
            normalQueue.Enqueue(newOrder);             //if customer membership is Silver or Ordinary, add to the Normal Queue
        }
        string filePath = "orders.csv";                                    //add order to orders.csv
        using (StreamWriter sw = File.AppendText(filePath))
        {
            foreach (IceCream iceCream in newOrder.IceCreamList)
            {
                string iceCreamInfo = $"{newOrder.Id}, {selectedCustomerID},{newOrder.TimeReceived},{newOrder.TimeFulfilled},{iceCream.Option},{iceCream.Scoops}";
                if (iceCream.Option.ToLower() == "cone")
                {
                    Cone cone = (Cone)iceCream;
                    iceCreamInfo += $",{cone.Dipped},,";
                }

                else if (iceCream.Option.ToLower() == "waffle")
                {
                    Waffle waffle = (Waffle)iceCream;
                    iceCreamInfo += $", ,{waffle.WaffleFlavour},";
                }

                else if (iceCream.Option.ToLower() == "cup")
                {
                    iceCreamInfo += ", , ,";
                }
                else
                {
                    iceCreamInfo += ", ,";
                }

                for (int i = 0; i < 3; i++)
                {
                    if (i < iceCream.Flavours.Count)
                    {
                        iceCreamInfo += $"{iceCream.Flavours[i].Type},";
                    }
                    else
                    {
                        iceCreamInfo += ",";
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (i < iceCream.Toppings.Count)
                    {
                        iceCreamInfo += $"{iceCream.Toppings[i].Type},";
                    }
                    else
                    {
                        iceCreamInfo += ",";
                    }
                }
                sw.WriteLine(iceCreamInfo);
            }
        }
        Console.WriteLine("Order has been made successfully. ");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

//Option 5 (Jun Heng)
void OrderDetailsOfCustomer(Customer customer)
{
    List <Order> orderHistory = customer.OrderHistory;
    if (orderHistory != null)  //Check if customer has previous orders
    { 
        orderHistory.Sort(); //Sort order history by date
        foreach (Order order in orderHistory) //Display every ice cream in order history
        {
            DisplayIceCream(order);                    
        }                
    }
    else { Console.WriteLine("No orders made yet.\n"); } //if customer has no previous orders
}

//Option 6 (Jun Heng)
void ModifyOrderDetails(Customer customer)
{
    int option = -1;
    while (true)
    {        
        DisplayIceCream(customer.CurrentOrder); //Display current orders
        Console.WriteLine("Modify ice cream \n[1] Choose an existing ice cream object to modify" +
            "\n[2] Add new ice cream object to the order" +
            "\n[3] Choose an existing ice cream object to delete from the order." +
            "\n[0] Exit.");
        Console.Write("Enter an option: ");

        try 
        {
            option = Convert.ToInt32(Console.ReadLine());
            if (option < 0 || option > 3 || option == null)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (option == 0)
            {
                break;
            }
            else
            {
                customer.CurrentOrder.ModifyIceCream(option); //Call method to modify ice cream
                DisplayIceCream(customer.CurrentOrder); //Display ice creams in the order
            }
            break;            
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid option. Please choose an integer. \n");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Please choose from the list. \n");
        }        
    }
}

//Option 7 (Ming Xuan)
void CheckoutOrder(Queue<Order> normalQueue, Queue<Order> goldQueue, List<Customer> customerList)
{
    try
    {
        bool orderProcessed = false;
        foreach (Customer customer in customerList)       //loop through each customer in customerList
        {
            Queue<Order> selectedQueue = customer.Rewards.Tier == "Gold" && goldQueue.Count > 0 ? goldQueue : normalQueue;   //check if customer membership is Gold and gold queue is not empty, use goldqueue, else use normal queue

            if (selectedQueue.Count > 0)           //check if selected order queue is not empty
            {
                try
                {
                    Order currentOrder = selectedQueue.Dequeue();           //dequeue first order from selected order queue
                    Customer orderCustomer = customerList.FirstOrDefault(customer => customer.CurrentOrder?.Id == currentOrder.Id);        //find customer linked with current order using currentOrder's ID with customer's orderID

                    if (orderCustomer == null)
                    {
                        Console.WriteLine("Customer not found.");
                        continue;
                    }

                    orderCustomer.CurrentOrder = currentOrder;         //set the current order for the customer and display the ice cream details

                    DisplayIceCream(currentOrder);                //display the order

                    double totalBill = currentOrder.CalculateTotal();       //calculate total bill 

                    if (orderCustomer.IsBirthday())                     //check if it's the customer's birthday and apply discount
                    {
                        var SortedIceCreams = currentOrder.IceCreamList.OrderByDescending(iceCream => iceCream.CalculatePrice());      //sort ice cream by their price
                        IceCream mostExpensive = SortedIceCreams.FirstOrDefault();

                        if (mostExpensive != null)
                        {
                            totalBill -= mostExpensive.CalculatePrice();               //most expensive ice cream will be made free
                        }
                    }

                    if (orderCustomer.Rewards.PunchCard >= 10)              //check if customer's punchcard is more than 10
                    {
                        if (currentOrder.IceCreamList.Count > 0)
                        {
                            totalBill -= currentOrder.IceCreamList[0].CalculatePrice();       //first ice cream in order list will be made free
                        }

                        orderCustomer.Rewards.PunchCard = 0;      //reset punchcard back to 0
                    }

                    if (orderCustomer.Rewards.Tier == "Silver" || orderCustomer.Rewards.Tier == "Gold")      //check if customer's membership is Gold or Silver
                    {
                        Console.WriteLine("You have {0} points.", orderCustomer.Rewards.Points);            //display customer's membership points
                        Console.WriteLine("Do you want to redeem your points? (Y/N): ");
                        string option = Console.ReadLine();
                        if (option.ToUpper() == "Y")
                        {
                            Console.WriteLine("Enter the number of points to redeem: ");
                            if (int.TryParse(Console.ReadLine(), out int RedeemPoints))
                            {
                                if (RedeemPoints > 0 && RedeemPoints <= orderCustomer.Rewards.Points)         //checks if points entered in is more than 0 and lesser or equal than existing membership points
                                {
                                    double discountAmount = orderCustomer.Rewards.RedeemPoints(RedeemPoints);
                                    totalBill -= discountAmount;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid number of points to redeem. ");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input for points. ");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Only Silver and Gold can use points to redeem. ");
                    }

                    Console.WriteLine("Total Bill: {0:F2}", totalBill);
                    Console.WriteLine("Press any key to make payment: ");
                    Console.ReadKey();
                    Console.WriteLine(" ");

                    foreach (IceCream iceCream in currentOrder.IceCreamList)      //punch card for each ice cream in the order
                    {
                        orderCustomer.Rewards.Punch();
                    }

                    int pointsEarned = (int)Math.Floor(totalBill * 0.72);      //calculate and add points from checkout
                    orderCustomer.Rewards.Points += pointsEarned;

                    orderCustomer.Rewards.MembershipUpgrade();                 //upgrade membership tier if eligible
                    currentOrder.TimeFulfilled = DateTime.Now;                 //set time fulfilled of order 
                    orderCustomer.OrderHistory.Add(currentOrder);              //add order to customer's order history

                    Console.WriteLine("Successfully made payment. ");
                    orderProcessed = true;
                    break;                                                     //exit the loop after processing order
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        if (!orderProcessed)
        {
            Console.WriteLine("Order queue is empty.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

//Option 8 (Jun Heng)
void DisplayPrices()
{   
    List<Order> yearOrder = new List<Order>(); //List of all orders in the year
    int year = 0;
    while (true)
    {
        Console.Write("Enter the year: ");
        try
        {
            year = Convert.ToInt32(Console.ReadLine());
            foreach (Customer customer in customerList)
            {
                if (customer.OrderHistory != null)
                {
                    foreach (Order order in customer.OrderHistory)
                    {
                        if (order.TimeReceived.Year == year)//Check if correct year
                        {
                            yearOrder.Add(order);
                        }
                    }
                }
            }
            break;
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid option. Please choose an integer. \n");
        }
    }

    Dictionary<string, double> yearDict = new Dictionary<string, double> //Dictionary with month as key and price amount as value
        {
            { "Jan", 0.0 },
            { "Feb", 0.0 },
            { "Mar", 0.0 },
            { "Apr", 0.0 },
            { "May", 0.0 },
            { "Jun", 0.0 },
            { "Jul", 0.0 },
            { "Aug", 0.0 },
            { "Sep", 0.0 },
            { "Oct", 0.0 },
            { "Nov", 0.0 },
            { "Dec", 0.0 }
        };
    foreach (Order order in yearOrder)
    {
        if (order.TimeFulfilled != null)
        {
            string month = order.TimeFulfilled?.ToString("MMM"); //Convert month to string
            foreach (IceCream iceCream in order.IceCreamList)
            {
                double price = iceCream.CalculatePrice(); //Get price of ice cream

                foreach (string month1 in yearDict.Keys)
                {
                    if (month == month1) //Check if correct month
                    {
                        yearDict[month] += price; //Update price
                    }
                }
            }
        }        
    }

    double amount = 0;
    foreach (string month in yearDict.Keys)
    {
        amount += yearDict[month]; //Get total amount
        Console.WriteLine($"{month} {year}: ${yearDict[month],-10:F2}"); //Display prices per month
    }
    Console.WriteLine($"Total amount: ${amount,-10:F2} \n"); //Display price of the whole month
}

while (true)
{
    int option = -1;
    displayMenu();
    Console.Write("Enter your option: ");

    //Exception handling
    try
    {
        option = Convert.ToInt32(Console.ReadLine());
        if (option < 0 || option > 8)
        {
            throw new ArgumentOutOfRangeException("Option must be 1, 2, 3, 4, 5, 6, 7, 8 or 0");
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("Invalid option. Please choose an integer. \n");
    }
    catch (ArgumentOutOfRangeException)
    {
        Console.WriteLine("Please choose from the list. \n");
    }

    if (option == 1)
    {
        DisplayCustomerDetails();
    }
    if (option == 2) //List all current order
    {
        CurrentOrder(normalQueue, goldQueue);
    }

    if (option == 3)
    {
        RegisterNewCustomer();
    }

    if (option == 4)
    {
        CreateCustomerOrder();
    }

    if (option == 5 || option ==6) //List out details of customer
    {
        while (true) //Loop forever
        {
            int id = -1;
            bool found = false;            

            DisplayCustomerDetails();
            Console.Write("Enter customer ID (0 to exit): ");
            try {
                id = Convert.ToInt32(Console.ReadLine());
                if (id == 0) { break; }
                foreach(Customer customer in customerList)
                {
                    if(id == customer.MemberId)
                    {
                        found = true;
                        if (customer.CurrentOrder != null)
                        {
                            if (option == 5)
                            {
                                OrderDetailsOfCustomer(customer);
                            }
                            else if (option == 6)
                            {
                                ModifyOrderDetails(customer);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No order found.\nGo to option 4 to create order. \n");
                        }
                    }
                }
            }
            catch (FormatException) //If input is not integer
            {
                Console.WriteLine("Invalid option. Please input an integer. \n");                
            }            
                 
            if (found == false)
            {
                Console.WriteLine("Customer not found.\n");
            }
        }        
    }
    
    if (option == 7)
    {
        CheckoutOrder(normalQueue, goldQueue, customerList);
    }
    if (option == 8) //Display price of each year by month
    {
        DisplayPrices();
    }
    if (option == 0)
    {
        Console.WriteLine("Exiting...");
        break;
    }    
}
