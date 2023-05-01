namespace Demo; 

public class Major {
    public int Id { get; set; }
    
    public string Name { get; set; }

    public ICollection<Student> Students { get; set; } = new List<Student>();

    public List<Subject> Subjects { get; } = new();

    public Major() {
    }

    public Major(int id, string name) {
        Id = id;
        Name = name;
    }
    
    
}