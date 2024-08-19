namespace SimpleVMSBlazor.Data;

public class Camera 
{
    public int Id { get; set; }  // Primary key property
    public string? Username { get; set; }
    public string? Password { get; set;}
    public string? Address { get; set; }
    public bool Recording { get; set; }
    public int RecordingRententionPeriod { get; set; } = 30;
    public string Name { get; set; } = Guid.NewGuid().ToString();
}