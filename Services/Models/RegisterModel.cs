using System.Collections.Generic;

namespace Services.Freelancer.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<CompetenceModel> Competences { get; set; }
    }
}