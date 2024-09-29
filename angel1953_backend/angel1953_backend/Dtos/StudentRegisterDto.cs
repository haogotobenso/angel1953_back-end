namespace angel1953_backend.Dtos
{
    public class StudentRegisterDto
    {
        public string Account { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string PasswordCheck { get; set; }  = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string AuthCode { get; set; }

        public string School { get; set; }

        public int SchoolId { get; set; }

        public string? Class { get; set; }

        public int? ClassId { get; set; }

        public string? StudentId { get; set; }
    }
}
