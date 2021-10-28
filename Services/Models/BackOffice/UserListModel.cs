using System.Collections.Generic;

namespace Services.Freelancer.Models.BackOffice
{
    public class UserListModel
    {
        public TypeModel Type { get; set; }
        public List<UserModel> Users { get; set; } = new List<UserModel>();
    }
}