using Microsoft.EntityFrameworkCore;
using MvcMusic.Models;

namespace MvcMusic.Data
{
    public class MvcMusicContext : DbContext
    {
        public MvcMusicContext(DbContextOptions<MvcMusicContext> options)
            : base(options)
        {
        }

        public DbSet<Music> Music { get; set; }
    }
}