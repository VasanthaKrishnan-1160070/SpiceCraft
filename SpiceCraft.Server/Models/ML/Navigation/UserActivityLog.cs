namespace SpiceCraft.Server.Models.ML.Navigation;

// Data model for user activity log
public class UserActivityLog
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string NavigationItem { get; set; }
    public string Routing { get; set; }
    public DateTime Timestamp { get; set; }
    public string SessionId { get; set; }
    
    // Additional fields
    public double TimeSpent { get; set; } // Time spent on the page
    public int ClickCount { get; set; } // Number of times user clicked on the item
}