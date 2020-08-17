using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DestinationReview.Models;
using Microsoft.EntityFrameworkCore;
using System;

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
        //.Where(destination => destination.ReviewNumber == Math.Max(destination.ReviewNumber)).ToList();
        .OrderBy(destination => destination.ReviewNumber).First();
    }

    [HttpPost]
    public void Post([FromBody] Destination destination)
    {
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

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Destination destination)
    {
      destination.DestinationId = id;
      _db.Entry(destination).State = EntityState.Modified;
      _db.SaveChanges();
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      var destinationToDelete = _db.Destinations.FirstOrDefault(entry => entry.DestinationId == id);
      _db.Destinations.Remove(destinationToDelete);
      _db.SaveChanges();
    }
  }
}