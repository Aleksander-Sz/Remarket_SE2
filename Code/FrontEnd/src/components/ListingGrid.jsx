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
    category: 'clothes',
    minPrice: '',
    maxPrice: '',
  });

  const fetchAll = async () => {
    try {
      const response = await axios.get('/products');
      setItems(response.data);
    } catch (err) {
      console.error('Failed to fetch from /products:', err);
    }
  };

  const handleFilter = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('/products/filter', filters);
      setItems(response.data);
    } catch (err) {
      console.warn('Filter endpoint not available, fallback to GET /products');
      fetchAll();
    }
  };

  useEffect(() => {
    fetchAll();
  }, []);

  return (
    <section className="listing-grid">
      <h2 className="listing-title">Browse Listings</h2>

      <form className="filter-form" onSubmit={handleFilter}>
        <select
          value={filters.category}
          onChange={(e) => setFilters({ ...filters, category: e.target.value })}
        >
          <option value="clothes">Clothes</option>
          <option value="accessories">Accessories</option>
          <option value="jewelry">Jewelry</option>
          <option value="toys">Toys</option>
          <option value="kids">Kids</option>
          <option value="women">Women</option>
          <option value="men">Men</option>
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
              style={{ position: 'relative' }} 
            >
              <img src={item.imageUrl || '/fallback.jpg'} alt={item.title} />
              <h3>{item.title}</h3>
              <p>${item.price?.toFixed(2) || 'N/A'}</p>
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
