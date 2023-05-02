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
        
        public string salt { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public Major? Major { get; set; }

        public List<Subject>? Subjects { get; set; }

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
