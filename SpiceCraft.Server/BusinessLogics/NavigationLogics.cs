using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.ML.Models.Navigation;
using SpiceCraft.Server.ML.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpiceCraft.Server.Helpers;
using MLModels = SpiceCraft.Server.ML.Models.Navigation;
using MLEntityModel = SpiceCraft.Server.Models;

namespace SpiceCraft.Server.BusinessLogics;

public class NavigationLogics
{
    private readonly SpiceCraftContext _context;
    private static ITransformer _mlModel;
    private static MLContext _mlContext = new MLContext();

    public NavigationLogics(SpiceCraftContext context)
    {
        _context = context;

        // Load the machine learning model once (singleton)
        if (_mlModel == null)
        {
            _mlModel = _mlContext.Model.Load("UserNavigationModel.zip", out var modelInputSchema);
        }
    }
    
    public async Task LogUserActivity(MLModels.UserActivityLog log)
    {
        int userId = log.UserId;
        string navigationItem = log.NavigationItem;
        double timeSpent = log.TimeSpent;
        string sessionId = log.SessionId;
        string routing = log.Routing;
        
        // Check if the navigation item already exists for the user in the current session
        var existingLog = await _context.UserActivityLogs
            .Where(log => log.UserId == userId && log.NavigationItem == navigationItem && log.SessionId == sessionId)
            .FirstOrDefaultAsync();

        if (existingLog != null)
        {
            // Update the existing log: increment click count and add time spent
            existingLog.ClickCount += 1;
            existingLog.TimeSpent += timeSpent;
           // existingLog.Timestamp = DateTime.UtcNow;  // Update timestamp to current time

            _context.UserActivityLogs.Update(existingLog);
        }
        else
        {
            // Insert a new log if this is the first interaction with this navigation item in the current session
            var newLog = new MLEntityModel.UserActivityLog
            {
                UserId = userId,
                NavigationItem = navigationItem,
                Routing = routing, 
                TimeSpent = timeSpent,
                ClickCount = 1,
                SessionId = sessionId,
             //   Timestamp = DateTime.UtcNow
            };

            _context.UserActivityLogs.Add(newLog);
        }

        // Save changes to the database
        await _context.SaveChangesAsync();
    }

    public async Task<ResultDetail<List<OrderedNavigationItem>>> GetOrderedNavigation(int userId)
    {
        var resultDetail = new ResultDetail<List<OrderedNavigationItem>>();
        // Step 1: Fetch user activity logs from the database
        var userLogs = await GetUserActivityLogs(userId);

        if (userLogs == null || !userLogs.Any())
        {
            // Return an empty list instead of null for easier handling
            resultDetail.Message = "No activity logs found for the user.";
            resultDetail.IsSuccess = false;
            return resultDetail;
        }

        // Step 2: Transform the logs into machine-learning ready data
        var transformedData = TransformUserActivityLogs(userLogs);

        // Step 3: Predict the click likelihood for each navigation item using batch prediction
        var predectionResult = PredictAndOrderNavigation(transformedData);
        resultDetail.Data = predectionResult;
        resultDetail.IsSuccess = true;
        return resultDetail;
    }

    private async Task<List<UserActivityLog>> GetUserActivityLogs(int userId)
    {
        return await _context.UserActivityLogs
            .Where(w => w.UserId == userId)
            .Select(w => new UserActivityLog()
            {
                UserId = w.UserId,
                NavigationItem = w.NavigationItem,
                Routing = w.Routing,
                TimeSpent = w.TimeSpent ?? 0,
                SessionId = w.SessionId,
                ClickCount = w.ClickCount ?? 0
            }).ToListAsync();
    }

    private List<UserActivityLog> GetMockUserActivityLogs(int userId)
    {
        // Mock data representing user activity logs for demonstration purposes
        return new List<UserActivityLog>
        {
            new UserActivityLog
            {
                Id = 1, UserId = userId, NavigationItem = "Home", Routing = "/home",
                Timestamp = DateTime.Now.AddMinutes(-30), SessionId = "session001", TimeSpent = 120, ClickCount = 3
            },
            new UserActivityLog
            {
                Id = 2, UserId = userId, NavigationItem = "Dashboard", Routing = "/dashboard",
                Timestamp = DateTime.Now.AddMinutes(-25), SessionId = "session001", TimeSpent = 90, ClickCount = 5
            },
            new UserActivityLog 
            {
                Id = 3, UserId = userId, NavigationItem = "Profile", Routing = "/profile",
                Timestamp = DateTime.Now.AddMinutes(-20), SessionId = "session001", TimeSpent = 60, ClickCount = 1
            },
            new UserActivityLog
            {
                Id = 4, UserId = userId, NavigationItem = "Menu", Routing = "/item-list",
                Timestamp = DateTime.Now.AddMinutes(-15), SessionId = "session001", TimeSpent = 150, ClickCount = 25
            },
            new UserActivityLog
            {
                Id = 5, UserId = userId, NavigationItem = "Orders", Routing = "/order-list",
                Timestamp = DateTime.Now.AddMinutes(-10), SessionId = "session001", TimeSpent = 200, ClickCount = 5
            },
            new UserActivityLog
            {
                Id = 6, UserId = userId, NavigationItem = "Cart", Routing = "/cart-list",
                Timestamp = DateTime.Now.AddMinutes(-5), SessionId = "session001", TimeSpent = 30, ClickCount = 1
            },
            new UserActivityLog
            {
                Id = 7, UserId = userId, NavigationItem = "Payments", Routing = "/payment-list",
                Timestamp = DateTime.Now.AddMinutes(-3), SessionId = "session002", TimeSpent = 60, ClickCount = 1
            },
            new UserActivityLog
            {
                Id = 8, UserId = userId, NavigationItem = "Enquiry", Routing = "/enquiry-list",
                Timestamp = DateTime.Now.AddMinutes(-2), SessionId = "session002", TimeSpent = 45, ClickCount = 7
            },
            new UserActivityLog
            {
                Id = 9, UserId = userId, NavigationItem = "Returns", Routing = "/returns",
                Timestamp = DateTime.Now.AddMinutes(-1), SessionId = "session002", TimeSpent = 20, ClickCount = 30
            },
            new UserActivityLog
            {
                Id = 10, UserId = userId, NavigationItem = "Gift Card", Routing = "/gift-card",
                Timestamp = DateTime.Now, SessionId = "session002", TimeSpent = 35, ClickCount = 10
            }
        };
    }

    private List<UserActivity> TransformUserActivityLogs(List<UserActivityLog> userLogs)
    {
        var transformedData = new List<UserActivity>();

        // Group by NavigationItem and select the most recent log for each
        var groupedLogs = userLogs
            .GroupBy(log => log.NavigationItem)
            .Select(group => group.OrderByDescending(log => log.Timestamp).First())
            .ToList();

        foreach (var log in groupedLogs)
        {
            var userActivity = new UserActivity
            {
                UserId = log.UserId,
                NavigationItem = log.NavigationItem,
                ClickCount = log.ClickCount,
                TimeSpent = (float)log.TimeSpent,
                LastClicked = CalculateLastClicked(userLogs, log.UserId, log.NavigationItem, log.Timestamp),
                SessionOrder = GetSessionOrder(userLogs, log.SessionId, log.NavigationItem),
                Clicked = log.ClickCount > 0
            };

            // Print feature values to check if they vary between navigation items
            Console.WriteLine($"Item: {log.NavigationItem}, ClickCount: {log.ClickCount}, TimeSpent: {log.TimeSpent}, LastClicked: {userActivity.LastClicked}, SessionOrder: {userActivity.SessionOrder}");

            transformedData.Add(userActivity);
        }

        return transformedData;
    }

        public static double Sigmoid(double rawScore)
        {
            return 1 / (1 + Math.Exp(-rawScore));
        }

        private float CalculateLastClicked(List<UserActivityLog> userLogs, int userId, string navigationItem, DateTime currentTimestamp)
        {
            var lastLog = userLogs
                .Where(x => x.UserId == userId && x.NavigationItem == navigationItem && x.Timestamp < currentTimestamp)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault();

            return lastLog == null ? float.MaxValue : (float)(currentTimestamp - lastLog.Timestamp).TotalSeconds;
        }

        private float GetSessionOrder(List<UserActivityLog> userLogs, string sessionId, string navigationItem)
        {
            var logsInSession = userLogs
                .Where(x => x.SessionId == sessionId)
                .OrderBy(x => x.Timestamp)
                .ToList();

            return logsInSession.FindIndex(x => x.NavigationItem == navigationItem) + 1;
        }

        // Batch prediction using ITransformer.Transform()
        private List<OrderedNavigationItem> PredictAndOrderNavigation(List<UserActivity> userActivities)
        {
            try
            {
                // Create IDataView from the list of user activities
                IDataView userActivitiesDataView = _mlContext.Data.LoadFromEnumerable(userActivities);
                
                // Transform the data using the loaded model
                IDataView predictions = _mlModel.Transform(userActivitiesDataView);
                
                // Extract predictions from the transformed data
                var predictedNavigationItems = _mlContext.Data
                    .CreateEnumerable<NavigationPrediction>(predictions, reuseRowObject: false)
                    .Select((prediction, index) => new OrderedNavigationItem
                    {
                        Name = userActivities[index].NavigationItem,
                        Likelihood = prediction.Score,
                        Probability = Sigmoid(prediction.Score)
                    })
                    .OrderByDescending(item => item.Likelihood) // Order by highest likelihood
                    .ToList();

                return predictedNavigationItems;
            }
            catch (Exception ex)
            {
                // Handle errors during prediction and log them if necessary
                Console.WriteLine($"Error during prediction: {ex.Message}");
                return new List<OrderedNavigationItem>(); // Return empty list if something goes wrong
            }
        }
}

public class NavigationPrediction
{
    [ColumnName("Score")]
    public float Score { get; set; } // The predicted score (click likelihood)
}

public class OrderedNavigationItem
{
    public string Name { get; set; } // The name of the navigation item
    public float Likelihood { get; set; } // The likelihood of the user clicking on this item
    public double Probability { get; set; } // The probability of the user clicking on this item (calculated using the likelihood)
}