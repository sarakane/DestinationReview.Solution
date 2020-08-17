using System.ComponentModel.DataAnnotations;

namespace DestinationReview.Models
{
  public class Review
  {
    public int ReviewId { get; set; }
    [Required]
    [StringLength(50)]
    public string Title { get; set; }
    [Required]
    public string Body { get; set; }
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
    public int Rating { get; set; }
  }
}