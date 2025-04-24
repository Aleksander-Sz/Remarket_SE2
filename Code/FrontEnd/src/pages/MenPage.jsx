import React, { useState, useEffect } from 'react';
import './CategoryPage.css';
import AnimatedPage from '../components/AnimatedPage';

import Footer from '../components/Footer';
import axios from 'axios';

/*function MenPage() {
  const items = [
    { id: 1, name: 'Denim Jacket', price: '$24', image: require('../assets/men.jpg') },
    { id: 2, name: 'Flannel Shirt', price: '$16', image: require('../assets/men.jpg') },
    { id: 3, name: 'Classic Chinos', price: '$20', image: require('../assets/men.jpg') },
  ];*/

function MenPage() {
  // State to store fetched items
  const [items, setItems] = useState([]);

  // Fetch data when the component mounts
  useEffect(() => {
    // Make a GET request to fetch clothes items
    axios.get('/api/men')
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
      <h1>Men’s Section</h1>
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

export default MenPage;
