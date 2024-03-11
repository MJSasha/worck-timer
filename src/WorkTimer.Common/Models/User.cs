using System.ComponentModel.DataAnnotations.Schema;
using WorkTimer.Common.Definitions;

namespace WorkTimer.Common.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? CredentialsId { get; set; }
        public Credentials Credentials { get; set; }
        public UserRole Role { get; set; }
        public decimal Salary { get; set; }
    }
}