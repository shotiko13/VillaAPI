﻿using System.ComponentModel.DataAnnotations;

namespace VillaAPI.Models.Dto
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        public int Occupancy { get; set; }
        public int Sqft { get; set; }
    }
}
