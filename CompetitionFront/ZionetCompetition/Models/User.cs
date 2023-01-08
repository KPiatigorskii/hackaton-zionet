namespace ZionetCompetition.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string? Github { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Login { get; set; } = null!;

        //public string Password { get; set; } = null!;

        public int RoleId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int CreateUserId { get; set; }

        public int UpdateUserId { get; set; }

        public int StatusId { get; set; }

        public virtual Role Role { get; set; } = null!;

        public virtual Status Status { get; set; } = null!;
    }
}
