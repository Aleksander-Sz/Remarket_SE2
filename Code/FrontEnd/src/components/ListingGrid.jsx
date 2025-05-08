import React, { useEffect, useState } from 'react';
import './ListingGrid.css';
import { motion } from 'framer-motion';
import PaymentModal from '../components/PaymentModal';
import { useWishlist } from '../context/WishlistContext';
import { FaHeart, FaRegHeart } from 'react-icons/fa';
import axios from '../api/axiosInstance';

function ListingGrid() {
  const [items, setItems] = useState([]);
  const [selectedItem, setSelectedItem] = useState(null);
  const { wishlist, toggleWishlist } = useWishlist();

  const [filters, setFilters] = useState({
    category: '',
    minPrice: '',
    maxPrice: '',
  });

  const handleFilter = async (e) => {
    if (e) e.preventDefault();

    try {
      const response = await axios.get('/products'); // GET all, filter frontend for now
      let filtered = response.data.filter((item) => item.status === 'Available');

      if (filters.category) {
        filtered = filtered.filter((item) =>
          item.category?.name?.toLowerCase().includes(filters.category.toLowerCase())
        );
      }

      if (filters.minPrice) {
        filtered = filtered.filter((item) => item.price >= parseFloat(filters.minPrice));
      }

      if (filters.maxPrice) {
        filtered = filtered.filter((item) => item.price <= parseFloat(filters.maxPrice));
      }

      setItems(filtered);
    } catch (err) {
      console.error('Error loading from /api/products:', err);
      setItems([]); // fallback to empty list
    }
  };

  useEffect(() => {
    handleFilter();
  }, []);

  return (
    <section className="listing-grid">
      <h2 className="listing-title">Browse Listings</h2>

      <form className="filter-form" onSubmit={handleFilter}>
        <input
          type="text"
          placeholder="Category"
          value={filters.category}
          onChange={(e) => setFilters({ ...filters, category: e.target.value })}
        />
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
              <img
                src={require(`../assets/${item.category?.name?.toLowerCase() || 'default'}.jpg`)}
                alt={item.title}
              />
              <h3>{item.title}</h3>
              <p>${item.price.toFixed(2)}</p>
              <small>{item.description?.header}</small>
              <button onClick={() => setSelectedItem(item)}>Buy Now</button>
              <span className="wishlist-icon" onClick={() => toggleWishlist(item)}>
                {isWished ? <FaHeart color="red" /> : <FaRegHeart />}
              </span>
            </motion.div>
          );
        })}
      </div>

      {selectedItem && (
        <PaymentModal item={selectedItem} onClose={() => setSelectedItem(null)} />
      )}
    </section>
  );
}

export default ListingGrid;
