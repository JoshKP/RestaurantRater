using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantRater.Models
{
    // Restaurant Entity (The class that gets stored in the database)
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Rating { get; set; }
        public bool IsRecommended        // "public bool IsRecommended => Rating > 3.5;"
        {
            get
            {
                return Rating > 3.5;
            }
        }   
        

    }
}