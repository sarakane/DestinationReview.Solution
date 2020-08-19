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

    [HttpGet("top")]
    public ActionResult<Destination> Get()
    {
      return _db.Destinations
        .Include(destination => destination.Reviews)
        .OrderByDescending(destination => destination.ReviewNumber).FirstOrDefault();
    }

    [HttpGet("best")]
    public ActionResult<Destination> GetBest()
    { 
      return _db.Destinations
        .Include(destination => destination.Reviews)
        .OrderByDescending(destination => destination.ReviewAverage).FirstOrDefault();
    }

    [Authorize]
    [HttpPost]
    public void Post([FromBody] Destination destination)
    {
      destination.UserId = int.Parse(User.Identity.Name);
      _db.Destinations.Add(destination);
      _db.SaveChanges();
    }

    [HttpGet("{id}")]
    public ActionResult<Destination> Get(int id)
    {
      return _db.Destinations
        .Include(destination => destination.Reviews)
        .FirstOrDefault(entry => entry.DestinationId == id);
    }

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