namespace NovoePokolenie.ViewModels
{
    public class StaffViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; } // UserName
        public string Role { get; set; }
        public bool Blocked { get; set; }

        public StaffViewModel(string id, string firstName, string lastName, string login, string role, bool blocked)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            Role = role;
            Blocked = blocked;
        }
    }
}
