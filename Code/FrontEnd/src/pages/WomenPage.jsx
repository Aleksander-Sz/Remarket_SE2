import './CategoryPage.css';
import AnimatedPage from '../components/AnimatedPage';

import Footer from '../components/Footer';

function WomenPage() {
  const items = [
    { id: 1, name: 'Vintage Blazer', price: '$22', image: require('../assets/women.jpg') },
    { id: 2, name: 'Silk Scarf', price: '$10', image: require('../assets/women.jpg') },
    { id: 3, name: 'Midi Skirt', price: '$17', image: require('../assets/women.jpg') },
  ];

  return (
    <AnimatedPage>
      <div className="category-page">
      <h1>Women’s Section</h1>
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

export default WomenPage;
