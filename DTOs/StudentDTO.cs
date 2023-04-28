namespace Demo.DTOs {
    public class StudentDTO {

        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public StudentDTO () {
        }

        public StudentDTO (int id, string name, int age, string address) {
            Id = id;
            Name = name;
            Age = age;
            Address = address;
        }
    }
}
