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

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await axios.get('/products');
        const mapped = response.data.map((item) => ({
          id: item.id,
          title: item.title,
          price: item.price,
          imageUrl: getImageByCategory(item.category?.name || ''),
        }));
        setItems(mapped);
      } catch (err) {
        console.error('Backend unavailable, using dummy data');
        const dummyListings = [
          {
            id: 1,
            title: 'Vintage Jacket',
            price: 29.99,
            imageUrl: require('../assets/clothes.jpg'),
          },
          {
            id: 2,
            title: 'Leather Bag',
            price: 24.99,
            imageUrl: require('../assets/accessories.jpg'),
          },
        ];
        setItems(dummyListings);
      }
    };

    fetchProducts();
  }, []);

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

  return (
    <section className="listing-grid">
      <h2 className="listing-title">Browse Listings</h2>

      <form className="filter-form" onSubmit={(e) => e.preventDefault()}>
        <select
          value={filters.category}
          onChange={(e) => setFilters({ ...filters, category: e.target.value })}
        >
          <option value="">All Categories</option>
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

        <button disabled>Filter (disabled)</button>
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
