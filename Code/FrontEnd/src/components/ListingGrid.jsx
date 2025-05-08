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
    category: 'all',
    minPrice: '',
    maxPrice: '',
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get('/products');
        setItems(response.data);
      } catch (err) {
        console.error('Failed to fetch from /api/products:', err);
      }
    };

    fetchData();
  }, []);

  return (
    <section className="listing-grid">
      <h2 className="listing-title">Browse Listings</h2>

      {/* Filter UI (visual only) */}
      <form className="filter-form" onSubmit={(e) => e.preventDefault()}>
        <select
          value={filters.category}
          onChange={(e) => setFilters({ ...filters, category: e.target.value })}
        >
          <option value="all">All Categories</option>
          <option value="electronics">Electronics</option>
          <option value="furniture">Furniture</option>
          <option value="books">Books</option>
          <option value="clothing">Clothing</option>
          <option value="toys">Toys</option>
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
              {/* No image since backend doesn't send imageUrl */}
              <h3>{item.title}</h3>
              <p>Price: ${item.price.toFixed(2)}</p>
              <p>Category: {item.category?.name}</p>
              <p>{item.description?.header}</p>
              <p>{item.description?.paragraph}</p>
              <p>Status: {item.status}</p>
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
