import './CategoryPage.css';
import AnimatedPage from '../components/AnimatedPage';

import Footer from '../components/Footer';

function ToysPage() {
  const items = [
    { id: 1, name: 'Wooden Blocks', price: '$10', image: require('../assets/toys.jpg') },
    { id: 2, name: 'Lego Set', price: '$25', image: require('../assets/toys.jpg') },
    { id: 3, name: 'Toy Car', price: '$8', image: require('../assets/toys.jpg') },
  ];

  return (
    <AnimatedPage>
    <div className="category-page">
      <h1>Toys</h1>
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

export default ToysPage;
