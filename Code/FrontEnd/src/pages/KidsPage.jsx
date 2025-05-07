import React, { useState, useEffect } from 'react';
import './CategoryPage.css';
import AnimatedPage from '../components/AnimatedPage';

import Footer from '../components/Footer';
import axios from 'axios';

/*function KidsPage() {
  const items = [
    { id: 1, name: 'Colorful T-shirt', price: '$9', image: require('../assets/kids.jpg') },
    { id: 2, name: 'Cartoon Hoodie', price: '$15', image: require('../assets/kids.jpg') },
    { id: 3, name: 'Mini Sneakers', price: '$18', image: require('../assets/kids.jpg') },
  ];*/

function KidsPage() {
  // State to store fetched items
  const [items, setItems] = useState([]);

  // Fetch data when the component mounts
  useEffect(() => {
    // Make a GET request to fetch clothes items
    axios.get('/api/kids')
      .then(response => {
        setItems(response.data);  // Set the fetched items into state
      })
      .catch(error => {
        console.error('Error fetching clothes data:', error);
      });
  }, []);

  return (
    <AnimatedPage>
    <div className="category-page">
      <h1>Kids Collection</h1>
      <div className="item-grid">
        {items.map(item => (
          <div className="item-card" key={item.id}>
            <img src={item.image} alt={item.name} />
            <h3>{item.name}</h3>
            <p className="price">{item.price}</p>
            <div className="actions">
              <button className="buy-btn">Buy Now</button>
              <button className="cart-btn">Add to Cart</button>
            </div>
          </div>
        ))}
      </div>
      <Footer />
    </div>
    </AnimatedPage>
  );
}

export default KidsPage;
