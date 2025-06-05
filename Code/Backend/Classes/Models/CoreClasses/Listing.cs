using System;
using System.Collections.Generic;
using ReMarket.Services;
using MySql.Data.MySqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReMarket.Models
{
    public class Listing
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        private string _status = "Draft";

        [Required]
        public string Status
        {
            get => _status;
            set
            {
                if (_status == "Draft" && value == "Archived")
                {
                    throw new InvalidOperationException(
                        "Cannot change status directly from Draft to Archived");
                }
                _status = value;
            }
        }

        // Navigation properties
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int DescriptionId { get; set; }
        public Description Description { get; set; } = null!;
        [NotMapped]
        public int? ThumbnailId { get; set; }
        [NotMapped]
        public Photo? Thumbnail { get; set; }

        public ICollection<ListingPhoto> ListingPhotos { get; set; } = new List<ListingPhoto>();


    }
}

/*namespace ReMarket.Models
{
    //"The Listing class is central to the marketplace, representing items posted by sellers."
    //"Key attributes include title, price,and status"
    //"Each listing belongs to a specific Category, has a Description, and can include multiple Photo objects to showcase the item visually.

    public class Listing
    {
        public int Id { get; private set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public ListingStatus Status { get; set; }
        public Description Description { get; set; }
        public Photo? Thumbnail { get; set; }
        public List<Photo> Photos { get; } = new();

        public Listing(int id, string title, decimal price, Category category, Description description)
        {
            Id = id;
            Title = title;
            Price = price;
            Category = category;
            Description = description;
            Status = ListingStatus.Draft;
        }

        private Listing(int id, string title, decimal price, Category category,
                   Description description, ListingStatus status)
        : this(id, title, price, category, description)
        {
            Status = status;
        }

        //for now this is ignoring photos!!
        //THIS IS UNTESTED

        public void SaveToDatabase(MySqlConnection connection)
        {
            using var transaction = connection.BeginTransaction();
            try
            {
                if (Description.Id == 0)
                {
                    Description.SaveToDatabase(connection);
                }

                using var command = new MySqlCommand(
                    Id == 0
                        ? @"INSERT INTO Listing (title, price, status, descriptionId, categoryId) 
                           VALUES (@Title, @Price, @Status, @DescriptionId, @CategoryId);
                           SELECT LAST_INSERT_ID();"
                        : @"UPDATE Listing SET 
                           title = @Title, 
                           price = @Price, 
                           status = @Status,
                           descriptionId = @DescriptionId,
                           categoryId = @CategoryId
                           WHERE id = @Id",
                    connection, transaction);

                command.Parameters.AddWithValue("@Title", Title);
                command.Parameters.AddWithValue("@Price", Price);
                command.Parameters.AddWithValue("@Status", Status.ToString());
                command.Parameters.AddWithValue("@DescriptionId", Description.Id);
                command.Parameters.AddWithValue("@CategoryId", Category.ID);

                if (Id == 0)
                {
                    Id = Convert.ToInt32(command.ExecuteScalar());
                }
                else
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }


                // SavePhotos(connection, transaction); to be implemented

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public static Listing? LoadById(MySqlConnection connection, int id)
        {
            using var command = new MySqlCommand(@"
                SELECT l.id, l.title, l.price, l.status, 
                       l.descriptionId, l.categoryId, l.thumbnailId,
                       d.header, d.paragraph,
                       c.name as categoryName
                FROM Listing l
                JOIN Description d ON l.descriptionId = d.id
                JOIN Category c ON l.categoryId = c.id
                WHERE l.id = @Id", connection);

            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;

            var listing = new Listing(
                id: reader.GetInt32("id"),
                title: reader.GetString("title"),
                price: reader.GetDecimal("price"),
                category: new Category(reader.GetInt32("categoryId"), reader.GetString("categoryName")),
                description: new Description(reader.GetInt32("descriptionId"),
                                          reader.GetString("header"),
                                          reader.GetString("paragraph")),
                status: Enum.Parse<ListingStatus>(reader.GetString("status"))
            );


            return listing;
        }

        public static List<Listing> LoadActiveListings(MySqlConnection connection, ListingQuery query)
        {
            var baseQuery = @"
                SELECT l.id, l.title, l.price, l.status, 
                       l.descriptionId, l.categoryId,
                       d.header, d.paragraph,
                       c.name as categoryName
                FROM Listing l
                JOIN Description d ON l.descriptionId = d.id
                JOIN Category c ON l.categoryId = c.id
                WHERE l.status = 'Active'";

            var sortClause = query.SortBy switch
            {
                ListingSort.PriceLowToHigh => "ORDER BY l.price ASC",
                ListingSort.PriceHighToLow => "ORDER BY l.price DESC",
                ListingSort.DateOldToNew => "ORDER BY l.id ASC",
                ListingSort.DateNewToOld => "ORDER BY l.id DESC",
                _ => ""
            };

            var pagedQuery = $"{baseQuery} {sortClause} LIMIT @Skip, @Take";

            using var countCommand = new MySqlCommand(
                $"SELECT COUNT(*) FROM ({baseQuery}) AS filtered", connection);
            var totalCount = Convert.ToInt32(countCommand.ExecuteScalar());

            using var command = new MySqlCommand(pagedQuery, connection);
            command.Parameters.AddWithValue("@Skip", (query.Page - 1) * query.PageSize);
            command.Parameters.AddWithValue("@Take", query.PageSize);

            var listings = new List<Listing>();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var listing = new Listing(
                    id: reader.GetInt32("id"),
                    title: reader.GetString("title"),
                    price: reader.GetDecimal("price"),
                    category: new Category(reader.GetInt32("categoryId"),
                              reader.GetString("categoryName")),
                    description: new Description(reader.GetInt32("descriptionId"),
                                              reader.GetString("header"),
                                              reader.GetString("paragraph")),
                    status: ListingStatus.Active
                );

                listings.Add(listing);
            }

            return listings;
        }
    }
}*/
