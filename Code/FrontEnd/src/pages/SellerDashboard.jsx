// src/pages/SellerDashboard.jsx
import React, { useState } from 'react';
import './SellerDashboard.css';

const dummySeller = {
  storeName: "Esbol's Vintage Finds",
  listings: [
    {
      id: 1,
      title: 'Leather Messenger Bag',
      price: 45.0,
      imageUrl: require('../assets/accessories.jpg'),
    },
    {
      id: 2,
      title: 'Retro Denim Jacket',
      price: 60.0,
      imageUrl: require('../assets/clothes.jpg'),
    },
  ],
};

function SellerDashboard() {
  const [listings, setListings] = useState(dummySeller.listings);

  const handleDelete = (id) => {
    const confirmed = window.confirm('Are you sure you want to delete this listing?');
    if (confirmed) {
      setListings(prev => prev.filter(item => item.id !== id));
    }
  };

  const handleEdit = (id) => {
    alert(`Editing listing with ID ${id} (not yet implemented)`);
  };

  const handleAdd = () => {
    alert('Add new listing (not yet implemented)');
  };

  return (
    <div className="seller-dashboard">
      <h1>Seller Dashboard</h1>
      <h2>Store: {dummySeller.storeName}</h2>
      <button className="add-btn" onClick={handleAdd}>+ Add New Listing</button>

      <div className="seller-grid">
        {listings.map(item => (
          <div key={item.id} className="seller-card">
            <img src={item.imageUrl} alt={item.title} />
            <h3>{item.title}</h3>
            <p>${item.price.toFixed(2)}</p>
            <div className="actions">
              <button className="edit-btn" onClick={() => handleEdit(item.id)}>Edit</button>
              <button className="delete-btn" onClick={() => handleDelete(item.id)}>Delete</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default SellerDashboard;
