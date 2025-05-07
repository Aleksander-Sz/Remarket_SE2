// src/pages/SuperDealsPage.jsx
import React, { useEffect, useState } from 'react';
import './SuperDealsPage.css';
import { motion } from 'framer-motion';
import { useWishlist } from '../context/WishlistContext';
import { FaHeart, FaRegHeart } from 'react-icons/fa';
import PaymentModal from '../components/PaymentModal';
// import axios from 'axios';
//import axios from '../api/axiosInstance';


function SuperDealsPage() {
  const [deals, setDeals] = useState([]);
  const { wishlist, toggleWishlist } = useWishlist();
  const [selectedItem, setSelectedItem] = useState(null); // For modal

  /*
  useEffect(() => {
    const fetchDeals = async () => {
      try {
        const response = await axios.get('https://your-backend.com/api/listings?type=deal');
        setDeals(response.data);
      } catch (error) {
        console.error('Failed to fetch super deals:', error);
      }
    };

    fetchDeals();
  }, []);
  */

  useEffect(() => {
    const dummyDeals = [
      {
        id: 101,
        title: 'Vintage Leather Boots',
        price: 49.99,
        discountPrice: 29.99,
        imageUrl: require('../assets/clothes.jpg'),
      },
      {
        id: 102,
        title: 'Retro Sunglasses',
        price: 19.99,
        discountPrice: 9.99,
        imageUrl: require('../assets/accessories.jpg'),
      },
    ];
    setDeals(dummyDeals);
  }, []);

  return (
    <section className="super-deals-page">
      <h2 className="deals-title">💥 Super Deals</h2>
      <div className="deals-grid">
        {deals.map((item) => {
          const isWished = wishlist.find((w) => w.id === item.id);
          return (
            <motion.div
              key={item.id}
              className="deal-card"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.4 }}
            >
              <img src={item.imageUrl} alt={item.title} />
              <h3>{item.title}</h3>
              <p>
                <span className="old-price">${item.price.toFixed(2)}</span>{' '}
                <span className="new-price">${item.discountPrice.toFixed(2)}</span>
              </p>
              <button onClick={() => setSelectedItem(item)}>Buy Now</button>
              <span className="wishlist-icon" onClick={() => toggleWishlist(item)}>
                {isWished ? <FaHeart color="red" /> : <FaRegHeart />}
              </span>
            </motion.div>
          );
        })}
      </div>

      {/* Payment modal */}
      {selectedItem && (
        <PaymentModal item={selectedItem} onClose={() => setSelectedItem(null)} />
      )}
    </section>
  );
}

export default SuperDealsPage;
