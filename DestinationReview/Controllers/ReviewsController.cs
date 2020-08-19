using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DestinationReview.Models;
using Microsoft.EntityFrameworkCore;

namespace DestinationReview.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ReviewsController : ControllerBase
  {
    private DestinationReviewContext _db;
    public ReviewsController(DestinationReviewContext db)
    {
      _db = db;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Review>> Get(string rating, string country, string city)
    {
      var query = _db.Reviews.Include(review => review.Destination).AsQueryable();
      if (rating != null)
      {
        int searchRating = int.Parse(rating);
        query = query.Where(entry => entry.Rating == searchRating);
      }
      if(country != null)
      {
        query = query.Where(entry => entry.Destination.Country == country);
      }
      if(city != null)
      {
        query = query.Where(entry => entry.Destination.City == city);
      }
      return query.ToList();
    }

    [HttpPost]
    public void Post([FromBody] Review review)
    {
      _db.Reviews.Add(review);
      _db.SaveChanges();
      var thisDestination = _db.Destinations.Include(destination => destination.Reviews).FirstOrDefault(destination => destination.DestinationId == review.DestinationId);
      thisDestination.GetReviewNumber();
      thisDestination.GetReviewAverage();
      _db.Entry(thisDestination).State = EntityState.Modified;
      _db.SaveChanges();
    }

    [HttpGet("{id}")]
    public ActionResult<Review> Get(int id)
    {
      return _db.Reviews
        .Include(review => review.Destination)
        .FirstOrDefault(entry => entry.ReviewId == id);
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Review review)
    {
      review.ReviewId = id;
      var thisDestination = _db.Destinations.Include(destination => destination.Reviews).FirstOrDefault(destination => destination.DestinationId == review.DestinationId);
      _db.Entry(review).State = EntityState.Modified;
      _db.SaveChanges();
      thisDestination.GetReviewAverage();
      _db.Entry(thisDestination).State = EntityState.Modified;
      _db.SaveChanges();
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      var reviewToDelete = _db.Reviews.FirstOrDefault(entry => entry.ReviewId == id);
      var thisDestination = _db.Destinations.Include(destination => destination.Reviews).FirstOrDefault(destination => destination.DestinationId == reviewToDelete.DestinationId);
      _db.Reviews.Remove(reviewToDelete);
      _db.SaveChanges();
      thisDestination.GetReviewNumber();
      thisDestination.GetReviewAverage();
      _db.Entry(thisDestination).State = EntityState.Modified;
      _db.SaveChanges();
    }
  }
}