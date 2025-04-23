import './CategoryPage.css';
import AnimatedPage from '../components/AnimatedPage';

import Footer from '../components/Footer';

function KidsPage() {
  const items = [
    { id: 1, name: 'Colorful T-shirt', price: '$9', image: require('../assets/kids.jpg') },
    { id: 2, name: 'Cartoon Hoodie', price: '$15', image: require('../assets/kids.jpg') },
    { id: 3, name: 'Mini Sneakers', price: '$18', image: require('../assets/kids.jpg') },
  ];

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
