using System.ComponentModel.DataAnnotations;
using DestinationReview.Models;
using System.Collections.Generic;

namespace DestinationReview.Models
{
  public class Destination
  {
    public int DestinationId { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public virtual ICollection<Review> Reviews {get; set;}
    public int ReviewNumber { get { return Reviews.Count;}}
    public double ReviewAverage 
    { 
      get 
      {
        if(Reviews.Count != 0)
        {
          double sum = 0;
          foreach(var review in Reviews)
          {
            sum += review.Rating;
          }
          return sum/Reviews.Count;
        }
        else
        {
            return (double)0;
        }
      }
    }
  }
}