using Microsoft.EntityFrameworkCore;

namespace DestinationReview.Models
{
  public class DestinationReviewContext : DbContext
  {
    public DestinationReviewContext(DbContextOptions<DestinationReviewContext> options)
      : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Destination> Destinations { get; set; }
  }
}