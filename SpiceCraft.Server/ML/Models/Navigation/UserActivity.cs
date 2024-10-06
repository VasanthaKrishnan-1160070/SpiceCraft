namespace SpiceCraft.Server.ML.Models.Navigation;

public class UserActivity
{
    public int UserId { get; set; }
    public string NavigationItem { get; set; }
    public float ClickCount { get; set; }   // Number of times the user clicked this item
    public float TimeSpent { get; set; }    // Time spent on the page in seconds
    public float LastClicked { get; set; }  // Time since the last click on the item
    public float SessionOrder { get; set; } // Order in the session (1st, 2nd, etc.)
    // Label (optional): For supervised learning, this could be whether the user clicked (1 or 0)
    public bool Clicked { get; set; }
}