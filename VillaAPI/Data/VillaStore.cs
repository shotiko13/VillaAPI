﻿using VillaAPI.Models.Dto;

namespace VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO { Id = 1, Name="Mountain View",Sqft=100, Occupancy = 4},
            new VillaDTO { Id = 2, Name="Sea View" , Sqft=200, Occupancy = 7}
        };
    }
}
