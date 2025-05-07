import React, { useState, useEffect } from 'react';
import './ClothesPage.css';
import Footer from '../components/Footer';
import AnimatedPage from '../components/AnimatedPage';
import axios from 'axios';

/*function ClothesPage() {
  const items = [
    { id: 1, name: 'Vintage Jacket', price: '$25', image: require('../assets/clothes.jpg') },
    { id: 2, name: 'Retro Shirt', price: '$15', image: require('../assets/clothes.jpg') },
    { id: 3, name: 'Denim Jeans', price: '$20', image: require('../assets/clothes.jpg') },
  ];*/

function ClothesPage() {
  // State to store fetched items
  const [items, setItems] = useState([]);

  // Fetch data when the component mounts
  useEffect(() => {
    // Make a GET request to fetch clothes items
    axios.get('/api/clothes')
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
      <h1>Clothes Collection</h1>
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

export default ClothesPage;
