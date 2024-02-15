using System;
namespace core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime InitialDate { get; set; }

        public Order(int id, User user)
        {
            Id = id;
            User = user;
            InitialDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Order Id {Id}, User {User.Name}, Criation: {InitialDate:dd/MM/yyyy hh:mm:ss}";
        }
    }
}

