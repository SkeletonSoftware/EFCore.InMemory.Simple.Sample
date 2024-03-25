using Microsoft.EntityFrameworkCore;
using Movies.Api.Models;

namespace Movies.Api;

public static class DatabaseSeeder
{
     public static void Initialize(IServiceProvider serviceProvider)
     {
         using var context = new MoviesDbContext(serviceProvider.GetRequiredService<DbContextOptions<MoviesDbContext>>());
         
         if (context.Movies.Any() && context.Actors.Any())
         {
             return;   // Data was already seeded
         }

         // CAST
         var holland = new Actor() { FirstName = "Tom", LastName = "Holland" };
         var tobey = new Actor() { FirstName = "Tobey", LastName = "Maguire" };
         var garfield = new Actor() { FirstName = "Andrew", LastName = "Garfield" };
         var hanks = new Actor() { FirstName = "Tom", LastName = "Hanks" };
         var leo = new Actor() { FirstName = "Leonardo", LastName = "DiCaprio" };
         context.Actors.AddRange(holland, tobey, garfield, hanks, leo);
            
         // MOVIES
         context.Movies.AddRange(
             new Movie
             {
                 Title = "Spider-man",
                 Description = "Je to movie o superhrdinech",
                 Actors = new List<Actor>() { holland, tobey } 
             },
             new Movie
             {
                 Title = "Terminál",
                 Description = "Terminál je movie amerického režiséra Stevena Spielberga v hlavní roli s Tomem Hanksem.",
                 Actors = new List<Actor>() { garfield, hanks }
             },
             new Movie
             {
                 Title = "Velký Gatsby",
                 Description = "Movie je adaptací stejnojmenného románu od Francise Scotta Fitzgeralda.",
                 Actors = new List<Actor>() { leo }
             }
         );
            
         context.SaveChanges();
     }
}