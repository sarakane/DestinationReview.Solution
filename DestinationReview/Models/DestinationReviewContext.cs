using Microsoft.EntityFrameworkCore;

namespace DestinationReview.Models
{
  public class DestinationReviewContext : DbContext
  {
    public DestinationReviewContext(DbContextOptions<DestinationReviewContext> options)
      : base(options)
    {
    }

    public DbSet<Review> Reviews { get; set; }
  }
}