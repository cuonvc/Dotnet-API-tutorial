namespace Demo {
    public class Student {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public Student (int id, string name, int age, string address) {
            Id = id;
            Name = name;
            Age = age;
            Address = address;
        }
    }
}
