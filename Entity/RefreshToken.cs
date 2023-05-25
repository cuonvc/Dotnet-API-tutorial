using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo; 

public class RefreshToken {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public Student Student { get; set; }

    public RefreshToken() {
    }

    public RefreshToken(int id, string token, DateTime expireDate, 
        DateTime modifiedDate, Student student) {
        Id = id;
        this.Token = token;
        this.ExpireDate = expireDate;
        this.ModifiedDate = modifiedDate;
        this.Student = student;
    }
}