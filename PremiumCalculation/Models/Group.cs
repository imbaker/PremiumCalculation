using System.Collections.Generic;

namespace PremiumCalculation.Models
{
    public class Group
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}
