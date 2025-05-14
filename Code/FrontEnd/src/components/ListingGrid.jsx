import React, { useEffect, useState } from 'react';
import './ListingGrid.css';
import { motion } from 'framer-motion';
import PaymentModal from '../components/PaymentModal';
import { useWishlist } from '../context/WishlistContext';
import { FaHeart, FaRegHeart } from 'react-icons/fa';
import axios from '../api/axiosInstance';
import { useNavigate } from 'react-router-dom';

function ListingGrid() {
  const [items, setItems] = useState([]);
  const [selectedItem, setSelectedItem] = useState(null);
  const { wishlist, toggleWishlist } = useWishlist();
  const navigate = useNavigate();


  const [filters, setFilters] = useState({
    category: '',
    minPrice: '',
    maxPrice: '',
  });

  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true); // determines if next page exists
  const ITEMS_PER_PAGE = 40;

  const fetchProducts = async () => {
    try {
      const response = await axios.get('/products', {
        params: {
          category: filters.category,
          min_price: filters.minPrice,
          max_price: filters.maxPrice,
          page,
          limit: ITEMS_PER_PAGE,
        },
      });

      const mapped = response.data.map((item) => ({
        id: item.id,
        title: item.title,
        price: item.price,
        imageUrl: getImageByCategory(item.category?.name || ''),
      }));

      setItems(mapped);
      setHasMore(mapped.length === ITEMS_PER_PAGE); // if less than 40 → no more pages
    } catch (err) {
      console.error('Backend unavailable, using dummy data');
      setItems([]);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, [page]); // fetch when page changes

  const getImageByCategory = (category) => {
    switch (category.toLowerCase()) {
      case 'clothing': return require('../assets/clothes.jpg');
      case 'furniture': return require('../assets/accessories.jpg');
      case 'books': return require('../assets/kids.jpg');
      case 'toys': return require('../assets/toys.jpg');
      case 'electronics': return require('../assets/men.jpg');
      default: return require('../assets/clothes.jpg');
    }
  };

  const handleFilter = (e) => {
    e.preventDefault();
    setPage(1); // reset to first page on filter
    fetchProducts();
  };

  return (
    <section className="listing-grid">
      <h2 className="listing-title">Browse Listings</h2>

      <form className="filter-form" onSubmit={handleFilter}>
        <select
          value={filters.category}
          onChange={(e) => setFilters({ ...filters, category: e.target.value })}
        >
          <option value="">All Categories</option>
          <option value="clothing">Clothes</option>
          <option value="accessories">Accessories</option>
          <option value="jewelry">Jewelry</option>
          <option value="toys">Toys</option>
          <option value="kids">Kids</option>
          <option value="women">Women</option>
          <option value="men">Men</option>
          <option value="electronics">Electronics</option>
          <option value="furniture">Furniture</option>
        </select>

        <input
          type="number"
          placeholder="Min Price"
          value={filters.minPrice}
          onChange={(e) => setFilters({ ...filters, minPrice: e.target.value })}
        />

        <input
          type="number"
          placeholder="Max Price"
          value={filters.maxPrice}
          onChange={(e) => setFilters({ ...filters, maxPrice: e.target.value })}
        />

        <button type="submit">Filter</button>
      </form>

      <div className="grid">
        {items.map((item) => {
          const isWished = wishlist.find((w) => w.id === item.id);
          return (
            <motion.div
              className="listing-card"
              key={item.id}
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.4 }}
            >
              <img src={item.imageUrl} alt={item.title} />
              <h3>{item.title}</h3>
              <p>${item.price.toFixed(2)}</p>
             <button onClick={() => setSelectedItem(item)}>Buy Now</button>
             <button onClick={() => navigate(`/product/${item.id}`)}>Learn More</button>

              <span className="wishlist-icon" onClick={() => toggleWishlist(item)}>
                {isWished ? <FaHeart color="red" /> : <FaRegHeart />}
              </span>
            </motion.div>
          );
        })}
      </div>

      {/* Pagination */}
      <div className="pagination-controls">
        <button onClick={() => setPage((p) => Math.max(p - 1, 1))} disabled={page === 1}>
          ◀ Previous
        </button>
        <span>Page {page}</span>
        <button onClick={() => setPage((p) => p + 1)} disabled={!hasMore}>
          Next ▶
        </button>
      </div>

      {selectedItem && (
        <PaymentModal item={selectedItem} onClose={() => setSelectedItem(null)} />
      )}
    </section>
  );
}

export default ListingGrid;
