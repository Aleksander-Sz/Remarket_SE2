import React, { useEffect, useState } from 'react';
import '../components/ListingGrid.css'; // Reuse existing styles
import { motion } from 'framer-motion';
import { useUser } from '../context/UserContext';
import { useWishlist } from '../context/WishlistContext';
import axios from '../api/axiosInstance';
import { useNavigate } from 'react-router-dom';
function MyListingsPage() {
  const [items, setItems] = useState([]);
  const { wishlist, toggleWishlist } = useWishlist();
  const { id: userId } = useUser();
  const ITEMS_PER_PAGE = 40;
  const navigate = useNavigate();

  const fetchSellerListings = async () => {
    try {
        const response = await axios.get('/products', {
            params: { ownerId: userId }
        });


      const mapped = response.data.map((item) => ({
        id: item.id,
        title: item.title,
        price: item.price,
        imageIds: item.photoIds
      }));

      setItems(mapped);
    } catch (err) {
      console.error('Failed to fetch seller listings:', err);
    }
  };

    useEffect(() => {
        //console.log('User ID:', userId);
    if (userId) fetchSellerListings();
  }, [userId]);

  return (
    <section className="listing-grid">
      <h2 className="listing-title">My Listings</h2>
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
              onClick={() => navigate(`/product/${item.id}`)}
            >
              <img src={`/api/photo/${item.imageIds[0]}`} alt={item.title} />
              <h3>{item.title}</h3>
              <p>${item.price.toFixed(2)}</p>
              <span className="wishlist-icon" onClick={() => toggleWishlist(item)}>
                {isWished ? '❤️' : '🤍'}
              </span>
            </motion.div>
          );
        })}
      </div>
    </section>
  );
}

export default MyListingsPage;
