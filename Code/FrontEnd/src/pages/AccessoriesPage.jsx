import './CategoryPage.css';

import Footer from '../components/Footer';

function AccessoriesPage() {
  const items = [
    { id: 1, name: 'Leather Bag', price: '$18', image: require('../assets/accessories.jpg') },
    { id: 2, name: 'Vintage Earrings', price: '$12', image: require('../assets/accessories.jpg') },
    { id: 3, name: 'Retro Sunglasses', price: '$15', image: require('../assets/accessories.jpg') },
  ];

  return (
    <div className="category-page">
      <h1>Accessories</h1>
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
  );
}

export default AccessoriesPage;
