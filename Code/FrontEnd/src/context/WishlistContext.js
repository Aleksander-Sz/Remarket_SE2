import React, { createContext, useState, useContext } from 'react';

const WishlistContext = createContext();

export const WishlistProvider = ({ children }) => {
  const [wishlist, setWishlist] = useState([]);

  const toggleWishlist = (item) => {
    const exists = wishlist.find(w => w.id === item.id);
    if (exists) {
      setWishlist(prev => prev.filter(w => w.id !== item.id));
    } else {
      setWishlist(prev => [...prev, item]);
    }
  };

  return (
    <WishlistContext.Provider value={{ wishlist, toggleWishlist }}>
      {children}
    </WishlistContext.Provider>
  );
};
// Later:
// axios.post('/wishlist', wishlist)
// axios.get('/wishlist').then(res => setWishlist(res.data))

export const useWishlist = () => useContext(WishlistContext);
