namespace Models.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Email { get; set; } = null!;
        public string? UserType { get; set; } = null!; // "Individual" أو "Company"
        public IndividualProfile? IndividualProfile { get; set; }
        public CompanyProfile? CompanyProfile { get; set; }

        public ICollection<Request> Requests { get; set; } = new List<Request>();

    }
}
