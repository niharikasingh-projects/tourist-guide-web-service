using TouristGuide.API.Models;
using System.Text.Json;

namespace TouristGuide.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TouristGuideDbContext context)
        {
            // Check if data already exists
            if (context.TouristAttractions.Any())
            {
                return; // DB has been seeded
            }

            SeedAttractions(context);
            SeedGuides(context);
            
            context.SaveChanges();
        }

        private static void SeedAttractions(TouristGuideDbContext context)
        {
            var attractions = new[]
            {
                // Paris
                new TouristAttraction { Id = "pa-1", Name = "Eiffel Tower", Location = "Champ de Mars, 5 Av. Anatole France, 75007 Paris", Description = "Iconic iron lattice tower and symbol of Paris", ImageUrl = "https://images.unsplash.com/photo-1511739001486-6bfe10ce785f?w=800&h=600&fit=crop", Category = "Landmark", Rating = 4.7m, City = "Paris", Country = "France" },
                new TouristAttraction { Id = "pa-2", Name = "Louvre Museum", Location = "Rue de Rivoli, 75001 Paris", Description = "World's largest art museum and historic monument", ImageUrl = "https://images.unsplash.com/photo-1499856871958-5b9627545d1a?w=800&h=600&fit=crop", Category = "Museum", Rating = 4.8m, City = "Paris", Country = "France" },
                new TouristAttraction { Id = "pa-3", Name = "Arc de Triomphe", Location = "Place Charles de Gaulle, 75008 Paris", Description = "Monumental arch honoring French military victories", ImageUrl = "https://images.unsplash.com/photo-1502602898657-3e91760cbb34?w=800&h=600&fit=crop", Category = "Monument", Rating = 4.6m, City = "Paris", Country = "France" },
                
                // London
                new TouristAttraction { Id = "lo-1", Name = "Big Ben", Location = "Westminster, London SW1A 0AA", Description = "Iconic clock tower and symbol of London", ImageUrl = "https://images.unsplash.com/photo-1513635269975-59663e0ac1ad?w=800&h=600&fit=crop", Category = "Landmark", Rating = 4.7m, City = "London", Country = "United Kingdom" },
                new TouristAttraction { Id = "lo-2", Name = "British Museum", Location = "Great Russell St, London WC1B 3DG", Description = "World-famous museum of human history and culture", ImageUrl = "https://images.unsplash.com/photo-1580539738736-a6b75a7a8f2e?w=800&h=600&fit=crop", Category = "Museum", Rating = 4.8m, City = "London", Country = "United Kingdom" },
                new TouristAttraction { Id = "lo-3", Name = "Tower of London", Location = "St Katharine's & Wapping, London EC3N 4AB", Description = "Historic castle on the River Thames", ImageUrl = "https://images.unsplash.com/photo-1526129318478-62ed807ebdf9?w=800&h=600&fit=crop", Category = "Historical Site", Rating = 4.6m, City = "London", Country = "United Kingdom" },
                
                // New York
                new TouristAttraction { Id = "ny-1", Name = "Statue of Liberty", Location = "Liberty Island, New York, NY 10004", Description = "Iconic symbol of freedom and democracy", ImageUrl = "https://images.unsplash.com/photo-1485738422979-f5c462d49f74?w=800&h=600&fit=crop", Category = "Monument", Rating = 4.8m, City = "New York", Country = "United States" },
                new TouristAttraction { Id = "ny-2", Name = "Central Park", Location = "New York, NY", Description = "Urban park in Manhattan, New York City", ImageUrl = "https://images.unsplash.com/photo-1512374382149-233c42b6a83b?w=800&h=600&fit=crop", Category = "Park", Rating = 4.8m, City = "New York", Country = "United States" },
                new TouristAttraction { Id = "ny-3", Name = "Empire State Building", Location = "20 W 34th St, New York, NY 10001", Description = "Iconic Art Deco skyscraper", ImageUrl = "https://images.unsplash.com/photo-1583415092413-23f3b91c7b4d?w=800&h=600&fit=crop", Category = "Landmark", Rating = 4.7m, City = "New York", Country = "United States" },
                
                // India - Delhi
                new TouristAttraction { Id = "de-1", Name = "Red Fort", Location = "Netaji Subhash Marg, Lal Qila, Chandni Chowk, New Delhi, Delhi 110006", Description = "Historic fort and UNESCO World Heritage Site", ImageUrl = "https://images.unsplash.com/photo-1587474260584-136574528ed5?w=800&h=600&fit=crop", Category = "Historical Site", Rating = 4.5m, City = "Delhi", Country = "India" },
                new TouristAttraction { Id = "de-2", Name = "Qutub Minar", Location = "Mehrauli, New Delhi, Delhi 110030", Description = "Tallest brick minaret in the world", ImageUrl = "https://images.unsplash.com/photo-1591267990532-e5bdb1b0b2d5?w=800&h=600&fit=crop", Category = "Monument", Rating = 4.4m, City = "Delhi", Country = "India" },
                
                // India - Agra
                new TouristAttraction { Id = "ag-1", Name = "Taj Mahal", Location = "Dharmapuri, Forest Colony, Tajganj, Agra, Uttar Pradesh 282001", Description = "Iconic white marble mausoleum and UNESCO World Heritage Site", ImageUrl = "https://images.unsplash.com/photo-1564507592333-c60657eea523?w=800&h=600&fit=crop", Category = "Monument", Rating = 4.9m, City = "Agra", Country = "India" },
                new TouristAttraction { Id = "ag-2", Name = "Agra Fort", Location = "Rakabganj, Agra, Uttar Pradesh 282003", Description = "Historic fort and UNESCO World Heritage Site", ImageUrl = "https://images.unsplash.com/photo-1587135941948-670b381f08ce?w=800&h=600&fit=crop", Category = "Historical Site", Rating = 4.6m, City = "Agra", Country = "India" },
                
                // India - Jaipur
                new TouristAttraction { Id = "ja-1", Name = "Hawa Mahal", Location = "Hawa Mahal Rd, Badi Choupad, J.D.A. Market, Pink City, Jaipur, Rajasthan 302002", Description = "Palace of Winds with unique five-story facade", ImageUrl = "https://images.unsplash.com/photo-1599661046289-e31897846e41?w=800&h=600&fit=crop", Category = "Palace", Rating = 4.5m, City = "Jaipur", Country = "India" },
                new TouristAttraction { Id = "ja-2", Name = "Amber Fort", Location = "Devisinghpura, Amer, Jaipur, Rajasthan 302001", Description = "Magnificent hilltop fort with stunning architecture", ImageUrl = "https://images.unsplash.com/photo-1603262110895-d8a8fbb0f1cd?w=800&h=600&fit=crop", Category = "Fort", Rating = 4.7m, City = "Jaipur", Country = "India" },
                
                // India - Mumbai
                new TouristAttraction { Id = "mu-1", Name = "Gateway of India", Location = "Apollo Bandar, Colaba, Mumbai, Maharashtra 400001", Description = "Iconic arch monument on Mumbai waterfront", ImageUrl = "https://images.unsplash.com/photo-1566552881560-0be862a7c445?w=800&h=600&fit=crop", Category = "Monument", Rating = 4.5m, City = "Mumbai", Country = "India" },
                
                // India - Goa
                new TouristAttraction { Id = "go-1", Name = "Baga Beach", Location = "Baga, Goa 403516", Description = "Popular beach known for water sports and nightlife", ImageUrl = "https://images.unsplash.com/photo-1512343879784-a960bf40e7f2?w=800&h=600&fit=crop", Category = "Beach", Rating = 4.4m, City = "Goa", Country = "India" },
                
                // India - Kerala
                new TouristAttraction { Id = "ke-1", Name = "Kerala Backwaters", Location = "Alappuzha, Kerala", Description = "Network of lagoons and lakes along Arabian Sea coast", ImageUrl = "https://images.unsplash.com/photo-1602216056026-ca0fd2650742?w=800&h=600&fit=crop", Category = "Nature", Rating = 4.8m, City = "Kerala", Country = "India" },
                
                // India - Varanasi
                new TouristAttraction { Id = "va-1", Name = "Kashi Vishwanath Temple", Location = "Lahori Tola, Varanasi, Uttar Pradesh 221001", Description = "Sacred Hindu temple dedicated to Lord Shiva", ImageUrl = "https://images.unsplash.com/photo-1561361513-2d000a50f0dc?w=800&h=600&fit=crop", Category = "Temple", Rating = 4.8m, City = "Varanasi", Country = "India" }
            };

            context.TouristAttractions.AddRange(attractions);
        }

        private static void SeedGuides(TouristGuideDbContext context)
        {
            var guides = new[]
            {
                // Paris guides
                new Guide
                {
                    Id = "g-pa-1",
                    Name = "Pierre Dubois",
                    ImageUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400&h=400&fit=crop",
                    Languages = "English,French,Spanish",
                    Availability = "Available",
                    HourlyRate = 35m,
                    Rating = 4.8m,
                    ExperienceYears = 10,
                    Specialties = "Paris History,Architecture,Fine Arts",
                    Bio = "Parisian guide with extensive knowledge of the city's art, history, and hidden gems.",
                    AvailableDates = JsonSerializer.Serialize(new[] {
                        new { from = "2025-12-10", to = "2025-12-31" },
                        new { from = "2026-01-05", to = "2026-01-25" },
                        new { from = "2026-02-01", to = "2026-02-28" }
                    }),
                    PhoneNumber = "+33 1 42 86 93 50",
                    Email = "pierre.dubois@parisguides.fr",
                    AttractionIds = "pa-1,pa-2,pa-3"
                },
                new Guide
                {
                    Id = "g-pa-2",
                    Name = "Sophie Laurent",
                    ImageUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400&h=400&fit=crop",
                    Languages = "English,French,Italian,German",
                    Availability = "Available",
                    HourlyRate = 38m,
                    Rating = 4.9m,
                    ExperienceYears = 12,
                    Specialties = "Louvre Museum,French Cuisine,Shopping",
                    Bio = "Art historian specializing in French Renaissance and Impressionist art.",
                    AvailableDates = JsonSerializer.Serialize(new[] {
                        new { from = "2025-12-09", to = "2025-12-20" },
                        new { from = "2025-12-26", to = "2026-01-15" },
                        new { from = "2026-01-20", to = "2026-03-31" }
                    }),
                    PhoneNumber = "+33 1 45 23 67 89",
                    Email = "sophie.laurent@parisguides.fr",
                    AttractionIds = "pa-1,pa-2"
                },
                
                // London guides
                new Guide
                {
                    Id = "g-lo-1",
                    Name = "James Harrison",
                    ImageUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400&h=400&fit=crop",
                    Languages = "English,French,Spanish",
                    Availability = "Available",
                    HourlyRate = 40m,
                    Rating = 4.8m,
                    ExperienceYears = 15,
                    Specialties = "British History,Royal Family,Parliament",
                    Bio = "Former history professor with encyclopedic knowledge of London's past.",
                    AvailableDates = JsonSerializer.Serialize(new[] {
                        new { from = "2025-12-12", to = "2026-01-15" },
                        new { from = "2026-01-20", to = "2026-03-10" }
                    }),
                    PhoneNumber = "+44 20 7946 0958",
                    Email = "james.harrison@londonguides.co.uk",
                    AttractionIds = "lo-1,lo-2,lo-3"
                },
                
                // India - Agra guides
                new Guide
                {
                    Id = "g-tj-1",
                    Name = "Rajesh Kumar",
                    ImageUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400&h=400&fit=crop",
                    Languages = "English,Hindi,French",
                    Availability = "Available",
                    HourlyRate = 25m,
                    Rating = 4.9m,
                    ExperienceYears = 12,
                    Specialties = "Mughal History,Architecture,Photography",
                    Bio = "Expert in Mughal architecture with over a decade of experience guiding tourists at the Taj Mahal.",
                    AvailableDates = JsonSerializer.Serialize(new[] {
                        new { from = "2025-12-10", to = "2026-01-10" },
                        new { from = "2026-01-20", to = "2026-03-15" }
                    }),
                    PhoneNumber = "+91 562-2234567",
                    Email = "rajesh.kumar@agraguides.in",
                    AttractionIds = "ag-1,ag-2"
                },
                new Guide
                {
                    Id = "g-tj-2",
                    Name = "Priya Sharma",
                    ImageUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400&h=400&fit=crop",
                    Languages = "English,Hindi,Spanish,German",
                    Availability = "Available",
                    HourlyRate = 30m,
                    Rating = 5.0m,
                    ExperienceYears = 8,
                    Specialties = "History,Cultural Tours,Storytelling",
                    Bio = "Passionate storyteller bringing the love story of Taj Mahal to life for visitors from around the world.",
                    AvailableDates = JsonSerializer.Serialize(new[] {
                        new { from = "2025-12-12", to = "2025-12-31" },
                        new { from = "2026-01-15", to = "2026-02-25" },
                        new { from = "2026-03-05", to = "2026-03-31" }
                    }),
                    PhoneNumber = "+91 562-2345678",
                    Email = "priya.sharma@agraguides.in",
                    AttractionIds = "ag-1,ag-2"
                },
                
                // India - Delhi guides
                new Guide
                {
                    Id = "g-dl-1",
                    Name = "Vikram Singh",
                    ImageUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400&h=400&fit=crop",
                    Languages = "English,Hindi,Punjabi",
                    Availability = "Available",
                    HourlyRate = 20m,
                    Rating = 4.8m,
                    ExperienceYears = 15,
                    Specialties = "Delhi History,British Colonial Era,Independence Movement",
                    Bio = "Historian specializing in Delhi's rich past from Mughal times to modern India.",
                    AvailableDates = JsonSerializer.Serialize(new[] {
                        new { from = "2025-12-09", to = "2026-01-05" },
                        new { from = "2026-01-18", to = "2026-02-28" },
                        new { from = "2026-03-10", to = "2026-03-31" }
                    }),
                    PhoneNumber = "+91 11-23456789",
                    Email = "vikram.singh@delhiguides.in",
                    AttractionIds = "de-1,de-2"
                },
                
                // India - Jaipur guides
                new Guide
                {
                    Id = "g-jp-1",
                    Name = "Arjun Rathore",
                    ImageUrl = "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?w=400&h=400&fit=crop",
                    Languages = "English,Hindi,Rajasthani",
                    Availability = "Available",
                    HourlyRate = 18m,
                    Rating = 4.6m,
                    ExperienceYears = 7,
                    Specialties = "Rajput History,Pink City Tours,Local Markets",
                    Bio = "Born and raised in Jaipur, passionate about sharing the stories of the royal city.",
                    AvailableDates = JsonSerializer.Serialize(new[] {
                        new { from = "2025-12-15", to = "2026-01-12" },
                        new { from = "2026-02-01", to = "2026-03-10" }
                    }),
                    PhoneNumber = "+91 141-2345678",
                    Email = "arjun.rathore@jaipurguides.in",
                    AttractionIds = "ja-1,ja-2"
                }
            };

            context.Guides.AddRange(guides);
        }
    }
}
