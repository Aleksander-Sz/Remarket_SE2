import { loadStripe } from '@stripe/stripe-js';
import { useState } from 'react';

const stripePromise = loadStripe('pk_test_51RU8yxQO9jfbuQDDqUX46tXCJPhz6ZsVAqy7iMJwl2V2c3Jg84kj7lMpxjGIzIAyVIIbT0nm6jJWYjdQgKD3McA200JHNnGygV'); 

function PaymentModal({ item, onClose }) {
  const [loading, setLoading] = useState(false);

  const handlePay = async () => {
    setLoading(true);
    const stripe = await stripePromise;

    const response = await fetch('http://localhost:5000/api/payments/create-checkout-session', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        title: item.title,
        price: item.price,
      }),
    });
    const session = await response.json();

    const result = await stripe.redirectToCheckout({
      sessionId: session.id,
    });

    if (result.error) {
      alert(result.error.message);
    }

    setLoading(false);
  };

  return (
    <div className="payment-modal-overlay">
      <div className="payment-modal">
        <button className="close-btn" onClick={onClose}>×</button>
        <h2>Pay for {item.title}</h2>
        <p>Amount: ${item.price}</p>
        <button onClick={handlePay} disabled={loading}>
          {loading ? 'Redirecting…' : 'Pay with Stripe'}
        </button>
      </div>
    </div>
  );
}

export default PaymentModal;
