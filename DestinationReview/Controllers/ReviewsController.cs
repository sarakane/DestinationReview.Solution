using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DestinationReview.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

    ///<summary>
    ///Retreives all reviews from the database, filterable by country, city or both.
    ///</summary>
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

    ///<summary>
    ///Adds a review to the database.
    ///</summary>
    [Authorize]
    [HttpPost]
    public void Post([FromBody] Review review)
    {
      review.UserId = int.Parse(User.Identity.Name);
      _db.Reviews.Add(review);
      _db.SaveChanges();
      var thisDestination = _db.Destinations.Include(destination => destination.Reviews).FirstOrDefault(destination => destination.DestinationId == review.DestinationId);
      thisDestination.GetReviewNumber();
      thisDestination.GetReviewAverage();
      _db.Entry(thisDestination).State = EntityState.Modified;
      _db.SaveChanges();
    }

    ///<summary>
    ///Retreives a review from the database by id number.
    ///</summary>
    [HttpGet("{id}")]
    public ActionResult<Review> Get(int id)
    {
      return _db.Reviews
        .Include(review => review.Destination)
        .FirstOrDefault(entry => entry.ReviewId == id);
    }

    ///<summary>
    ///Updates a review.
    ///</summary>
    [Authorize]
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Review review)
    {
      review.ReviewId = id;
      var thisDestination = _db.Destinations.Include(destination => destination.Reviews).FirstOrDefault(destination => destination.DestinationId == review.DestinationId);
      if (review.UserId == int.Parse(User.Identity.Name))
      {
        _db.Entry(review).State = EntityState.Modified;
        _db.SaveChanges();
        thisDestination.GetReviewAverage();
        _db.Entry(thisDestination).State = EntityState.Modified;
        _db.SaveChanges();
      }
    }

    ///<summary>
    ///Removes a review from the database.
    ///</summary>
    [Authorize]
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      var reviewToDelete = _db.Reviews.FirstOrDefault(entry => entry.ReviewId == id);
      var thisDestination = _db.Destinations.Include(destination => destination.Reviews).FirstOrDefault(destination => destination.DestinationId == reviewToDelete.DestinationId);
      if (reviewToDelete.UserId == int.Parse(User.Identity.Name))
      {
        _db.Reviews.Remove(reviewToDelete);
        _db.SaveChanges();
        thisDestination.GetReviewNumber();
        thisDestination.GetReviewAverage();
        _db.Entry(thisDestination).State = EntityState.Modified;
        _db.SaveChanges();
      }
    }
  }
}