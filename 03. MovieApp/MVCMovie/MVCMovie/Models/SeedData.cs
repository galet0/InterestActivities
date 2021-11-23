using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVCMovie.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCMovie.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MVCMovieContext(
                    serviceProvider.GetRequiredService<DbContextOptions<MVCMovieContext>>()
                ))
            {
                if (context.Movies.Any())
                {
                    return;
                }

                context.Movies.AddRange
                    (
                        new Movie
                        {
                            Title = "Eternals 3D",
                            ReleaseDate = DateTime.Parse("5-11-2021"),
                            Genre = "Action, Adventure, Fiction",
                            Price = 35.66M
                        },
                        new Movie
                        {
                            Title = "Dune 3D",
                            ReleaseDate = DateTime.Parse("22-10-2021"),
                            Genre = "Adventure, Drama, Fiction",
                            Price = 12.66M
                        },
                         new Movie
                         {
                             Title = "Ghostbusters: Afterlife",
                             ReleaseDate = DateTime.Parse("26-11-2021"),
                             Genre = "Comedy, Fantasy",
                             Price = 24.59M
                         }

                    );

                context.SaveChanges();
            }
        }

    }
}
