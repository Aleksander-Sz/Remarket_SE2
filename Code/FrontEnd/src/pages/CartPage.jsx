// src/pages/CartPage.jsx
import './CartPage.css';
import { useCart } from '../context/CartContext';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

function CartPage() {
  const { cartItems, removeFromCart } = useCart();
  const navigate = useNavigate();

  const total = cartItems.reduce((sum, item) => sum + item.price, 0).toFixed(2);

  useEffect(() => {
    if (!cartItems.length) {
      // Optionally redirect or show message
    }
  }, [cartItems]);

  return (
    <div className="cart-page">
      <h2>Your Cart</h2>
      {cartItems.length === 0 ? (
        <p>Your cart is empty.</p>
      ) : (
        <>
          <div className="cart-grid">
            {cartItems.map((item) => (
              <div key={item.id} className="cart-card">
                <img src={`/api/photo/${item.imageIds?.[0]}`} alt={item.title} />
                <h3>{item.title}</h3>
                <p>${item.price.toFixed(2)}</p>
                <button onClick={() => removeFromCart(item)}>❌ Remove</button>
              </div>
            ))}
          </div>
          <div className="cart-footer">
            <h3>Total: ${total}</h3>
            <button className="pay-now-btn" onClick={() => alert('Proceed to payment!')}>
              Pay Now
            </button>
          </div>
        </>
      )}
    </div>
  );
}

export default CartPage;
