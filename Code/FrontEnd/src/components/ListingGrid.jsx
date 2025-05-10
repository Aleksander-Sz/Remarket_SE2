// src/components/ListingGrid.jsx
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

  // 🔁 Function to fetch products with filter query params
  const fetchProducts = async () => {
    try {
      const params = {};
      if (filters.category) params.category = filters.category;
      if (filters.minPrice) params.minPrice = filters.minPrice;
      if (filters.maxPrice) params.maxPrice = filters.maxPrice;

      const response = await axios.get('/products', { params });

      const products = response.data.map((item) => ({
        ...item,
        imageUrl: item.imageData
          ? `data:image/jpeg;base64,${item.imageData}`
          : null,
      }));

      setItems(products);
    } catch (err) {
      console.error('Failed to fetch products:', err);
      setItems([]);
    }
  };

  // ⏬ Load once on mount
  useEffect(() => {
    fetchProducts();
  }, []);

  // 🔘 Apply filters button
  const handleFilter = (e) => {
    e.preventDefault();
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
          <option value="Electronics">Electronics</option>
          <option value="Furniture">Furniture</option>
          <option value="Books">Books</option>
          <option value="Clothing">Clothing</option>
          <option value="Toys">Toys</option>
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

        <button type="submit">Apply Filters</button>
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
              {item.imageUrl ? (
                <img src={item.imageUrl} alt={item.title} />
              ) : (
                <div className="image-placeholder">No image</div>
              )}
              <h3>{item.title}</h3>
              <p>${item.price.toFixed(2)}</p>
              <p className="desc">{item.description?.header}</p>
              <p className="desc-small">{item.description?.paragraph}</p>
              <p className="status">{item.status}</p>

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
