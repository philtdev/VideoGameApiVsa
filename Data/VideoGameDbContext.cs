using Microsoft.EntityFrameworkCore;
using VideoGameApiVsa.Entities;

namespace VideoGameApiVsa.Data;

public class VideoGameDbContext(DbContextOptions<VideoGameDbContext> options) : DbContext(options)
{
    public DbSet<VideoGame> VideoGames { get; set; }
}
