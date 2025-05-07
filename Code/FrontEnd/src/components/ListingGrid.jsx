import React, { useEffect, useState } from 'react';
import './ListingGrid.css';
import { motion } from 'framer-motion';
import PaymentModal from '../components/PaymentModal';
import { useWishlist } from '../context/WishlistContext';
import { FaHeart, FaRegHeart } from 'react-icons/fa';
import axios from '../api/axiosInstance'; // Update path if needed

function ListingGrid() {
  const [items, setItems] = useState([]);
  const [selectedItem, setSelectedItem] = useState(null);
  const { wishlist, toggleWishlist } = useWishlist();

  const [filters, setFilters] = useState({
    category: 'clothes',
    minPrice: '',
    maxPrice: '',
  });

  // Fetch filtered items on form submit
  const handleFilter = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post('/listings/filter', filters);
      setItems(response.data);
    } catch (err) {
      console.error('Backend unavailable, using dummy data');
      const dummyListings = {
        clothes: [
          { id: 1, title: 'Vintage Jacket', price: 29.99, imageUrl: require('../assets/clothes.jpg') },
          { id: 2, title: 'Denim Shirt', price: 19.99, imageUrl: require('../assets/clothes.jpg') },
        ],
        accessories: [
          { id: 3, title: 'Leather Bag', price: 24.99, imageUrl: require('../assets/accessories.jpg') },
          { id: 4, title: 'Vintage Keychain', price: 9.99, imageUrl: require('../assets/accessories.jpg') },
        ],
        toys: [
          { id: 5, title: 'LEGO Set', price: 14.99, imageUrl: require('../assets/toys.jpg') },
          { id: 6, title: 'Action Figures', price: 12.99, imageUrl: require('../assets/toys.jpg') },
        ],
        kids: [
          { id: 7, title: 'Colorful T-Shirts', price: 6.99, imageUrl: require('../assets/kids.jpg') },
          { id: 8, title: 'Kid’s Hoodie', price: 11.99, imageUrl: require('../assets/kids.jpg') },
        ],
        women: [
          { id: 9, title: 'Wool Coat', price: 39.99, imageUrl: require('../assets/women.jpg') },
          { id: 10, title: 'Formal Blazer', price: 34.99, imageUrl: require('../assets/women.jpg') },
        ],
        men: [
          { id: 11, title: 'Denim Jacket', price: 32.99, imageUrl: require('../assets/men.jpg') },
          { id: 12, title: 'Flannel Shirt', price: 22.99, imageUrl: require('../assets/men.jpg') },
        ],
      };

      setItems(dummyListings[filters.category] || []);
    }
  };

  // On load, fetch default category (e.g., clothes)
  useEffect(() => {
    handleFilter(new Event('load'));
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
            >
              <img src={item.imageUrl} alt={item.title} />
              <h3>{item.title}</h3>
              <p>${item.price.toFixed(2)}</p>
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
