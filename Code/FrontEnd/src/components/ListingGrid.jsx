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
              const listings = response.data.filter(item => item.status === 'Available').map(item => ({
                  id: item.id,
                  title: item.title,
                  price: item.price,
                  description: item.description?.header || '',
                  category: item.category?.name || '',
                  imageUrl: getImageByCategory(item.category?.name || ''),
              }));
              setItems(listings);
          } catch (err) {
              console.error('Backend unavailable, using dummy data');
              const dummyListings = [
                  {
                      id: 1,
                      title: 'Error',
                      price: 29.99,
                      description: 'Classic leather jacket',
                      category: 'Clothing',
                      imageUrl: require('../assets/clothes.jpg'),
                  },
                  {
                      id: 2,
                      title: 'Leather Bag',
                      price: 24.99,
                      description: 'High-quality leather handbag',
                      category: 'Accessories',
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
                      <p>${Number(item.price).toFixed(2)}</p>
                      <small>{item.description}</small> {/* <-- description from backend */}
                      <button onClick={() => setSelectedItem(item)}>Buy Now</button>
                      <span className="wishlist-icon" onClick={() => toggleWishlist(item)}>
                          {isWished ? <FaHeart color="red" /> : <FaRegHeart />}
                      </span>
                  </motion.div>
              );
          })}
      </div>

  );
}

export default ListingGrid;
