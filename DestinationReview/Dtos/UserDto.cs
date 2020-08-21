using System.Collections.Generic;
using DestinationReview.Models;

namespace DestinationReview.Dtos
{
  public class UserDto
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<Destination> Destinations { get; set; }
  }
}