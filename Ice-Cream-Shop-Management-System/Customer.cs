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
    internal class Customer
    {
        private string name;
        public string Name { get; set; }

        private int memberId;
        public int MemberId { get; set; }

        private DateTime dob;
        public DateTime Dob { get; set; }

        private Order currentorder;
        public Order CurrentOrder { get; set; }

        private List<Order> orderHistory;
        public List<Order> OrderHistory { get; set; }

        private PointCard rewards;
        public PointCard Rewards { get; set; }

        public Customer() { }

        public Customer(string _name, int _memberId, DateTime _dob)
        {
            Name = _name;
            MemberId = _memberId;
            Dob = _dob;
            rewards = new PointCard();
        }

        public Order MakeOrder(int orderId, DateTime timeReceived)
        {
            Order order = new Order(orderId, timeReceived);
            CurrentOrder = order;
            OrderHistory.Add(order);

            if (IsBirthday())
            {
                int birthdayCount = 0;
                foreach (Order customerOrder in OrderHistory)
                {
                    if (order.TimeReceived.Date == Dob.Date)
                    {
                        birthdayCount += 1;
                        if (birthdayCount > 1)
                        {
                            break;
                        }
                    }
                }
                if (birthdayCount == 1)
                {
                    double amount = 0;
                }
            }   
            return order;
        }

        public bool IsBirthday()
        {
            return DateTime.Now.Month == Dob.Month && DateTime.Now.Day == Dob.Day;
        }

        public override string ToString()
        {
            return $"{Name,-10}{MemberId,-10}{Dob.ToString("dd/MM/yyyy"),-15}";
        }
    }
}
