public class User : IComparable<User>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public int CompareTo(User? other)
    {
        if (other == null) return 1;
        return this.Id.CompareTo(other.Id);
    }
}