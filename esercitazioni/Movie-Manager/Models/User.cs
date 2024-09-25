public class User
{
    public int UserId { get; set; }  // Unique identifier for the user
    public string Username { get; set; }  // Username of the user
    public string Email { get; set; }  // Email of the user
    public string Password { get; set; }  // User's password (hashing recommended)
}