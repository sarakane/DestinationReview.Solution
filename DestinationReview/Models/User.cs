using System.Collections.Generic;

namespace DestinationReview.Models
{
  public class User
  {
    public User()
    {
      this.Reviews = new HashSet<Review>();
      this.Destinations = new HashSet<Destination>();
    }
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public virtual ICollection<Review> Reviews { get; set; }
    public virtual ICollection<Destination> Destinations { get; set; }
  }
}