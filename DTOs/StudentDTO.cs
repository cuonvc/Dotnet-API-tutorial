namespace Demo.DTOs {
    public class StudentDTO {

        public int Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Username { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public StudentDTO () {
        }

        public StudentDTO (int id, string firstName, string lastName, int age, string address) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Address = address;
        }
    }
}
