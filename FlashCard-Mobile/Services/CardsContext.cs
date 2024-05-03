using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCard_Mobile.Model;
using Microsoft.EntityFrameworkCore;

namespace FlashCard_Mobile.Services
{
    public class CardsContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        public CardsContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbpath = Path.Combine(FileSystem.AppDataDirectory, "cards.sqlite");
            optionsBuilder.UseSqlite($"Filename={dbpath}");
        }
    }
}
