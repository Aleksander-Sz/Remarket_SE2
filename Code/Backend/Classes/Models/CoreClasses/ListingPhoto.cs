using System;
using System.Collections.Generic;
using ReMarket.Services;
using MySql.Data.MySqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReMarket.Models
{
    public class ListingPhoto
    {
        public int ListingId { get; set; }
        public Listing Listing { get; set; }

        public int PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}
