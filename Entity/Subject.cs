namespace Demo; 

public class Subject {
    public int Id { get; set; }
    
    public string Name { get; set; }

    public List<Major> Majors { get; set; } = new();

    public List<Student> Students { get; set; } = new();

    public Subject() {
    }

    public Subject(int id, string name) {
        Id = id;
        Name = name;
    }
}