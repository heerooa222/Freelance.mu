using System.Collections.Generic;

#nullable disable

namespace Services.Freelancer.Entities
{
    public partial class Type
    {
        public Type()
        {
            Users = new HashSet<User>();
        }

        public string IdType { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
