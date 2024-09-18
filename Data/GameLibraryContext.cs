using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Models;

namespace GameLibrary.Data
{
    public class GameLibraryContext : DbContext
    {
        public GameLibraryContext (DbContextOptions<GameLibraryContext> options)
            : base(options)
        {
        }

        public DbSet<GameLibrary.Models.YourList> YourList { get; set; } = default!;
    }
}
