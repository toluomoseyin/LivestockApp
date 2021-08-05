using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnimalFarmsMarket.Data.Models
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(250, ErrorMessage = "First Name can not be longer than 250 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(250, ErrorMessage = "Last Name can not be longer than 250 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public byte Gender { get; set; }
        public bool IsActive { get; set; }

        public string Photo { get; set; }

        [MaxLength(50, ErrorMessage = "Zip Code can not be more than 50 characters")]
        public string ZipCode { get; set; }

        public string PublicId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Address Address { get; set; }

        public Agent Agent { get; set; }

        public DeliveryPerson  DeliveryPerson { get; set; }

        public ICollection<Testimony> Testimonies { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<BroadcastNews> Broadcasts { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public AppUser()
        {
            Testimonies = new List<Testimony>();
            Ratings = new List<Rating>();
            Broadcasts = new List<BroadcastNews>();
            Comments = new List<Comment>();
            Reviews = new List<Review>();
            Orders = new List<Order>();
        }

    }
}
