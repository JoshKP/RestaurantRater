using RestaurantRater.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantRater.Controllers
{
    public class RatingController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();

        // Create new ratings
        [HttpPost]
        public async Task<IHttpActionResult> CreateRating(Rating model)
        {
            if (model == null)
            {
                return BadRequest("Your request cannot be empty.");
            }
            // Check to see if the model is NOT valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the target restaurant
            var restaurant = await _context.Restaurants.FindAsync(model.RestaurantId);
            if (restaurant == null)
            {
                return BadRequest($"The target restaurant with ID {model.RestaurantId} does not exist.");

            }

            // The restaurant isn't null so we can rate it
            _context.Ratings.Add(model);
            if(await _context.SaveChangesAsync() == 1)
            {
                return Ok($"You rated {restaurant.Name} successfully!");
            }

            return InternalServerError();
        }

        // Get rating by ID ?
        [HttpGet]
        public async Task<IHttpActionResult> GetRatingById(int id)
        {
            Rating rating = await _context.Ratings.FindAsync(id);
            if(rating != null)
            {
                return Ok(rating);
            }
            return NotFound();
        }

        // Get ALL ratings for specific restaurant by restaurant ID
        [HttpGet]
        [Route("api/Rating/GetAll/{id}")]
        public async Task<IHttpActionResult> GetAllRatings(int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            List<Rating> ratings = restaurant.Ratings;
            return Ok(ratings);
        }

        // Update rating
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRating([FromUri] int id, [FromBody]Rating updatedRating)
        {
            // Check if updated restaurant is valid
            if (ModelState.IsValid)
            {
                // Find and Update the restaurant
                Rating rating = await _context.Ratings.FindAsync(id);

                if (rating != null)
                {
                    // Update rating(s) now that we found them
                    rating.CleanlinessScore = updatedRating.CleanlinessScore;
                    rating.EnvironmentScore = updatedRating.EnvironmentScore;
                    rating.FoodScore = updatedRating.FoodScore;

                    await _context.SaveChangesAsync();
                    return Ok();
                }
                // Did not find rating
                return NotFound();
            }
            // Return bad request
            return BadRequest(ModelState);
        }


        // Delete rating
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRatingRatingById(int id)
        {
            Rating rating = await _context.Ratings.FindAsync(id);
            
            if (rating == null)
                return NotFound();

            _context.Ratings.Remove(rating);

            if (await _context.SaveChangesAsync() == 1)
                return Ok("The restaurant was deleted.");

            return InternalServerError();
        }
    }
}
