class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Active {get; set;}

    public User(int id, string name, bool active)
    {
        Id = id;
        Name = name;
        Active = active;
    }
}
