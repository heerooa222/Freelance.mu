using System;
using System.Collections.Generic;

namespace Services.Freelancer.Models
{
    public class UserModel
    {
        public string IdUser { get; set; }
        public string TypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identifiant { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public DateTime? LastConnected { get; set; }
        public string CompanyName { get; set; }
        public List<CompetenceModel> Competences { get; set; } = new List<CompetenceModel>();
        public string Next { get; set; }
        public bool IsAssignedTo { get; set; }
        public TypeModel UserType { get; set; }
    }
}