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
  public class DestinationsController : ControllerBase
  {
    private DestinationReviewContext _db;
    public DestinationsController(DestinationReviewContext db)
    {
      _db = db;
    }
    ///<summary>
    ///Retreives all destinations from the database, filterable by country, city or both.
    ///</summary>
    [HttpGet]
    public ActionResult<IEnumerable<Destination>> Get(string country, string city)
    {
      var query = _db.Destinations.Include(destination => destination.Reviews).AsQueryable();

      if (country != null)
      {
        query = query.Where(entry => entry.Country == country);
      }
      if (city != null)
      {
        query = query.Where(entry => entry.City == city);
      }
      return query.ToList();
    }

    ///<summary>
    ///Retreives the most visited destination by number of reviews.
    ///</summary>
    [HttpGet("top")]
    public ActionResult<Destination> Get()
    {
      return _db.Destinations
        .Include(destination => destination.Reviews)
        .OrderByDescending(destination => destination.ReviewNumber).FirstOrDefault();
    }

    ///<summary>
    ///Retreives the destination with the highest average review rating.
    ///</summary>
    [HttpGet("best")]
    public ActionResult<Destination> GetBest()
    { 
      return _db.Destinations
        .Include(destination => destination.Reviews)
        .OrderByDescending(destination => destination.ReviewAverage).FirstOrDefault();
    }

    ///<summary>
    ///Adds a destination to the database.
    ///</summary>
    ///<remarks>
    ///Sample request:
    ///
    ///     POST /Destination
    ///     {
    ///         "country": "Canada",
    ///         "city": "Vancouver"
    ///     }
    ///
    ///</remarks>
    [Authorize]
    [HttpPost]
    public void Post([FromBody] Destination destination)
    {
      destination.UserId = int.Parse(User.Identity.Name);
      _db.Destinations.Add(destination);
      _db.SaveChanges();
    }

    ///<summary>
    ///Retreives a destination from the database by id number.
    ///</summary>
    [HttpGet("{id}")]
    public ActionResult<Destination> Get(int id)
    {
      return _db.Destinations
        .Include(destination => destination.Reviews)
        .FirstOrDefault(entry => entry.DestinationId == id);
    }

    ///<summary>
    ///Updates a destination.
    ///</summary>
    [Authorize]
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Destination destination)
    {
      destination.DestinationId = id;
      if (destination.UserId == int.Parse(User.Identity.Name))
      {
        _db.Entry(destination).State = EntityState.Modified;
        _db.SaveChanges();
      }
    }

    ///<summary>
    ///Removes a destination from the database.
    ///</summary>
    [Authorize]
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      var destinationToDelete = _db.Destinations.FirstOrDefault(entry => entry.DestinationId == id);
      if (destinationToDelete.UserId == int.Parse(User.Identity.Name))
      {
        _db.Destinations.Remove(destinationToDelete);
        _db.SaveChanges();
      }
    }
  }
}