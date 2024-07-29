namespace UserList.API.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Email { get; set; }

        public User() { }
        public User(string name, string surname, string email)
        {
            Name = name;
            Surname = surname;
            Email = email;
        }

    }
}
