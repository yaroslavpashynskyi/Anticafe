
using DAL.Models;

namespace DAL
{
    public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if (context.Activities.Any() || context.Halls.Any()) return;


            var activities = new List<Activity>
            {
                new Activity
                {
                    Title = "Past Activity 1",
                    Date = DateTime.UtcNow.AddMonths(-2),
                    Description = "Activity 2 months ago",
                    Category = "film",
                },
                new Activity
                {
                    Title = "Past Activity 2",
                    Date = DateTime.UtcNow.AddMonths(-1),
                    Description = "Activity 1 month ago",
                    Category = "sport",
                },
                new Activity
                {
                    Title = "Future Activity 1",
                    Date = DateTime.UtcNow.AddMonths(1),
                    Description = "Activity 1 month in future",
                    Category = "tablegames",
                },
                new Activity
                {
                    Title = "Future Activity 2",
                    Date = DateTime.UtcNow.AddMonths(2),
                    Description = "Activity 2 months in future",
                    Category = "computergames",
                },
                new Activity
                {
                    Title = "Future Activity 3",
                    Date = DateTime.UtcNow.AddMonths(3),
                    Description = "Activity 3 months in future",
                    Category = "drinks",
                },
                new Activity
                {
                    Title = "Future Activity 4",
                    Date = DateTime.UtcNow.AddMonths(4),
                    Description = "Activity 4 months in future",
                    Category = "tablegames",
                },
                new Activity
                {
                    Title = "Future Activity 5",
                    Date = DateTime.UtcNow.AddMonths(5),
                    Description = "Activity 5 months in future",
                    Category = "computergames",
                },
                new Activity
                {
                    Title = "Future Activity 6",
                    Date = DateTime.UtcNow.AddMonths(6),
                    Description = "Activity 6 months in future",
                    Category = "sport",
                },
                new Activity
                {
                    Title = "Future Activity 7",
                    Date = DateTime.UtcNow.AddMonths(7),
                    Description = "Activity 2 months ago",
                    Category = "film",
                },
                new Activity
                {
                    Title = "Future Activity 8",
                    Date = DateTime.UtcNow.AddMonths(8),
                    Description = "Activity 8 months in future",
                    Category = "sport",
                }
            };
            var halls = new List<Hall>
            {
                new Hall
                {
                    Name = "Hall 1",
                    Activities = new List<Activity>{
                        activities[0],
                        activities[1],
                        activities[2],
                    }
                },
                new Hall
                {
                    Name = "Hall 1",
                    Activities = new List<Activity>{
                        activities[3],
                        activities[4],
                    }
                },
                new Hall
                {
                    Name = "Hall 1",
                    Activities = new List<Activity>{
                        activities[5],
                        activities[6],
                        activities[7],
                        activities[8],
                    }
                },
                new Hall
                {
                    Name = "Hall 1",
                    Activities = new List<Activity>{
                        activities[9]
                    }
                }
            };

            await context.Halls.AddRangeAsync(halls);
            await context.SaveChangesAsync();
        }
    }
}