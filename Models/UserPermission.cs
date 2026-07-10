namespace MyFirstWebAPI.Models
{
    public class UserPermission
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public Permission Permission { get; set; }

        public virtual User? User { get; set; }
    }
}
