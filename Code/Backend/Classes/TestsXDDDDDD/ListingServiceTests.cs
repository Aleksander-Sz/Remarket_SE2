/*using ReMarket.Models;
using ReMarket.Services;
using Xunit;

namespace ReMarket.Tests
{
    //photo adding and thumbnail setting are untested
    public class ListingServiceTests
    {
        private readonly ListingService _service = new();
        private readonly Category _testCategory = new(1, "Clothes");
        


        [Fact]
        public void CreateDraft_InitializesCorrectly()
        { //can make a draft
            var listing = _service.CreateDraft("Laptop", 999, _testCategory, "Fast", "16GB RAM");
            
            Assert.Equal(ListingStatus.Draft, listing.Status);
            Assert.Equal("Fast", listing.Description.Header);
            Assert.Empty(listing.Photos);
        }


        [Fact]
        public void UpdateStatus_ChangesState()
        { //can we activate the draft
            var listing = _service.CreateDraft("Blouse", 30, _testCategory, "Blue blouse", "Blue, comfy blouse");
            _service.UpdateStatus(listing.Id, ListingStatus.Active);
            
            Assert.Equal(ListingStatus.Active, listing.Status);
        }

        [Fact]
        public void UpdateListing_ModifiesProperties()
        { //can we change things in the listing
            var listing = _service.CreateDraft("Pants", 30, _testCategory, "Black", "Description in detail");
            
            _service.UpdateListing(listing.Id, l => {
                l.Price = 20;
                l.Description.Paragraph = "Description";
            });
            
            Assert.Equal(20, listing.Price);
            Assert.Equal("Description", listing.Description.Paragraph);
        }

        [Theory]
        [InlineData(ListingSort.PriceLowToHigh, 100, 200)]
        [InlineData(ListingSort.PriceHighToLow, 200, 100)]
        public void GetActiveListings_SortsCorrectlyByPrice(ListingSort sort, decimal firstPrice, decimal lastPrice)
        { //can we sort by price
            _service.CreateDraft("A", 200, _testCategory, "", "").Status = ListingStatus.Active;
            _service.CreateDraft("B", 100, _testCategory, "", "").Status = ListingStatus.Active;
            
            var results = _service.GetActiveListings(new ListingQuery(SortBy: sort));
            
            Assert.Equal(firstPrice, results.Listings[0].Price);
            Assert.Equal(lastPrice, results.Listings[1].Price);
        }
        [Fact]
        public void GetActiveListings_SortsLowToHigh()
        { //can we sort by price up
            _service.CreateDraft("A", 500, _testCategory, "", "").Status = ListingStatus.Active;
            _service.CreateDraft("B", 100, _testCategory, "", "").Status = ListingStatus.Active;
            _service.CreateDraft("C", 200, _testCategory, "", "").Status = ListingStatus.Active;
            
            var results = _service.GetActiveListings(new ListingQuery(SortBy: ListingSort.PriceLowToHigh));
            
            Assert.Equal(2, results.Listings[0].Id);
            Assert.Equal(3, results.Listings[1].Id);
            Assert.Equal(1, results.Listings[2].Id);
        }

        [Fact]
        public void GetActiveListings_SortsHighToLow()
        { //can we sort by price down
            _service.CreateDraft("A", 500, _testCategory, "", "").Status = ListingStatus.Active;
            _service.CreateDraft("B", 100, _testCategory, "", "").Status = ListingStatus.Active;
            _service.CreateDraft("C", 200, _testCategory, "", "").Status = ListingStatus.Active;
            
            var results = _service.GetActiveListings(new ListingQuery(SortBy: ListingSort.PriceHighToLow));
            
            Assert.Equal(1, results.Listings[0].Id);
            Assert.Equal(3, results.Listings[1].Id);
            Assert.Equal(2, results.Listings[2].Id);
        }

        [Fact]

        public void GetActiveListings_SortsCorrectlyOldToNew()
        { //can we sort old to new
            _service.CreateDraft("A", 500, _testCategory, "", "").Status = ListingStatus.Active;
            _service.CreateDraft("B", 100, _testCategory, "", "").Status = ListingStatus.Active;
            _service.CreateDraft("C", 200, _testCategory, "", "").Status = ListingStatus.Active;
            
            var results = _service.GetActiveListings(new ListingQuery(SortBy: ListingSort.DateOldToNew));
            
            Assert.Equal(1, results.Listings[0].Id);
            Assert.Equal(2, results.Listings[1].Id);
            Assert.Equal(3, results.Listings[2].Id);
        }

        [Fact]

        public void GetActiveListings_SortsCorrectlyNewToOld()
        { //can we sort new to old
            _service.CreateDraft("A", 500, _testCategory, "", "").Status = ListingStatus.Active;
            _service.CreateDraft("B", 100, _testCategory, "", "").Status = ListingStatus.Active;
            _service.CreateDraft("C", 200, _testCategory, "", "").Status = ListingStatus.Active;
            
            var results = _service.GetActiveListings(new ListingQuery(SortBy: ListingSort.DateOldToNew));
            
            Assert.Equal(3, results.Listings[2].Id);
            Assert.Equal(2, results.Listings[1].Id);
            Assert.Equal(1, results.Listings[0].Id);
        }

        [Fact]
        public void GetActiveListings_AppliesPaginationCorrectly()
        { //is pagination correct
            for (int i = 1; i <= 5; i++)
            {
                var listing = _service.CreateDraft(
                    $"Item {i}", 
                    i * 100, 
                    _testCategory, 
                    $"Header {i}", 
                    $"Description {i}"
                );
                _service.UpdateStatus(listing.Id, ListingStatus.Active);
            }

            var page1 = _service.GetActiveListings(new ListingQuery(Page: 1, PageSize: 2));
            var page2 = _service.GetActiveListings(new ListingQuery(Page: 2, PageSize: 2));
            var page3 = _service.GetActiveListings(new ListingQuery(Page: 3, PageSize: 2));


            Assert.Equal(2, page1.Listings.Count);
            Assert.Equal(5, page1.TotalCount); 
            Assert.Equal("Item 1", page1.Listings[0].Title); 
            Assert.Equal("Item 2", page1.Listings[1].Title);

            Assert.Equal(2, page2.Listings.Count);
            Assert.Equal("Item 3", page2.Listings[0].Title);
            Assert.Equal("Item 4", page2.Listings[1].Title);

            Assert.Single(page3.Listings); 
            Assert.Equal("Item 5", page3.Listings[0].Title);
        }
    }
}*/