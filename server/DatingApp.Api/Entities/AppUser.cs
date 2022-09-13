namespace DatingApp.Api.Entities
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string KnownAs { get; set; }
        public DateTime Dob { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastActive { get; set; } = DateTimeOffset.UtcNow;

        public string Introduction { get; set; }
        public Gender Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public string Interests { get; set; }
        public string LookingFor { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        // public int Age
        // {
        //     get
        //     {
        //         if (Dob == DateTime.MinValue)
        //             return 0;

        //         var today = DateTime.Now;
        //         var age =  today.Year - Dob.Year;

        //         if (today.Month * 100 + today.Day < Dob.Month * 100 + Dob.Day)
        //             --age;

        //         return age;
        //     }
        // }
    }
}