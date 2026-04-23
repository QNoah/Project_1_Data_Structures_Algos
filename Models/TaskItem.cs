public class TaskItem : IComparable<TaskItem>
{
    public enum TaskPriority { P0, P1, P2 }
    public enum TaskStatus { ToDo, InProgress, Done }

    public int Id { get; set; }
    public int? ParentId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public TaskPriority Priority { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? UserId { get; set; }

    public int CompareTo(TaskItem? other)
    {
        if (other == null) return 1;
        return this.Id.CompareTo(other.Id);
    }
}