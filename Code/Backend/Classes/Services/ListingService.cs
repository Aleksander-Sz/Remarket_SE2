/*using ReMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReMarket.Services
{
    public class ListingService
    {
        //sorting by popularity not implemented, what is i even supposed to be?
        private readonly List<Listing> _listings = new();
        private int _nextId = 1;

        public Listing CreateDraft( //for now, this can be changed
            string title,
            decimal price,
            Category category,
            string descriptionHeader,
            string descriptionParagraph)
        {
            if (price <= 0)
                throw new ArgumentException("Price must be positive");
    
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required");
            Description description = new Description(descriptionHeader, descriptionParagraph);
            var listing = new Listing(_nextId++, title, price, category, description);

            _listings.Add(listing);
            return listing;
        }

        public ListingResult GetActiveListings(ListingQuery query)
        {
            var filtered = _listings
                .Where(l => l.Status == ListingStatus.Active)
                .AsQueryable();


            filtered = query.SortBy switch
            {
                ListingSort.PriceLowToHigh => filtered.OrderBy(l => l.Price),
                ListingSort.PriceHighToLow => filtered.OrderByDescending(l => l.Price),
                ListingSort.DateOldToNew => filtered.OrderBy(l=>l.Id),
                ListingSort.DateNewToOld => filtered.OrderByDescending(l=>l.Id),


                _ => filtered
            };

            var results = filtered
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return new ListingResult(results, filtered.Count());
        }

        public bool UpdateStatus(int listingId, ListingStatus newStatus)
        {
            var listing = _listings.FirstOrDefault(l => l.Id == listingId);
            if (listing == null) return false;

            listing.Status = newStatus;
            return true;
        }

        public void AddPhotos(int listingId, params Photo[] photos)
        {
            if (_listings.FirstOrDefault(l => l.Id == listingId) is not Listing listing) 
                return;

            listing.Photos.AddRange(photos);
        }

        public bool SetThumbnail(int listingId, string imageName, string image)
        {
            var listing = _listings.FirstOrDefault(l => l.Id == listingId);
            if (listing == null) return false;

            var newThumbnail = new Photo(imageName, image, isThumbnail: true);
            foreach (var photo in listing.Photos.Where(p => p.IsThumbnail))
                photo.IsThumbnail = false;
            
            listing.Thumbnail = newThumbnail;
            listing.Photos.Add(newThumbnail);
            return true;
        }

        public bool UpdateListing(int id, Action<Listing> updates)
        {
            var listing = _listings.FirstOrDefault(l => l.Id == id);
            if (listing == null) return false;

            updates(listing);
            return true;
        }
    }

    public record ListingQuery( //change details about pages here
        int Page = 1,
        int PageSize = 20,
        ListingSort SortBy = ListingSort.None
    );

    public enum ListingSort
    {
        None,
        PriceLowToHigh,
        PriceHighToLow,
        DateOldToNew, //this uses id assuming older listings are created sooner
        DateNewToOld
    }

    public record ListingResult(
        List<Listing> Listings,
        int TotalCount
    );
}*/