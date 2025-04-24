namespace InternManagementAPI.Models;

public class TaskModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int InternId { get; set; }
    public DateTime AssignedDate { get; set; }
    public bool IsCompleted { get; set; }
}
