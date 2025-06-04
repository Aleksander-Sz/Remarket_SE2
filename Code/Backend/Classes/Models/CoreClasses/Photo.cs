using System;
using System.Collections.Generic;
using ReMarket.Services;
using MySql.Data.MySqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReMarket.Models
{
    public class Photo
    {
        //"The Photo class provides additional details for each listing by storing images.
        //It contains attributes like name and bytes,representing the image data.
        //A listing can have zero or more photos (0..*), reflecting optional visual content for listings.
        public int? Id { get; set; }
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        //public bool IsThumbnail { get; set;} //added for the sake of user story 3.1.1
        public ICollection<ListingPhoto> ListingPhotos { get; set; } = new List<ListingPhoto>();

        public Photo(int id, byte[] bytes)
        {
            //Name = name;
            Id = id;
            Bytes = bytes;
            //IsThumbnail = isThumbnail;
        }
        public Photo(string name, byte[] bytes)
        {
            Name = name;
            Bytes = bytes;
        }
    }
}
