namespace WebApplication2.Models
{
    public class User //Represents a User model with the basic properties
    {
        public string UserName { get; set; }   
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; } //Set status if it is active or inactive
    }
}