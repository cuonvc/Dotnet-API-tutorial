using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo {
    public class Student {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public byte[] salt { get; set; }

        public int Age { get; set; } = 0;

        public string Address { get; set; } = string.Empty;

        public Major Major { get; set; } = new Major();

        public List<Subject> Subjects { get; set; } = new List<Subject>();

        public Student () {

        }

        public Student (int id, string firstName, string lastName, string username, int age, string address) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Age = age;
            Address = address;
        }
    }
}
