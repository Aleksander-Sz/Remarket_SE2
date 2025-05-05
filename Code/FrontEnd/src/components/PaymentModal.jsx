// src/components/PaymentModal.jsx
import './PaymentModal.css';
import { useState } from 'react';

function PaymentModal({ item, onClose }) {
  const [formData, setFormData] = useState({
    name: '',
    cardNumber: '',
    expiry: '',
    cvc: '',
  });

  const handleChange = (e) => {
    setFormData(prev => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Paid for:', item.title, formData);
    alert(`Payment submitted for ${item.title} (simulated)`);
    onClose(); // Close modal after fake payment
  };

  return (
    <div className="payment-modal-overlay">
      <div className="payment-modal">
        <button className="close-btn" onClick={onClose}>×</button>
        <h2>Pay for {item.title}</h2>
        <form onSubmit={handleSubmit} className="payment-form">
          <input type="text" name="name" placeholder="Name on Card" required value={formData.name} onChange={handleChange} />
          <input type="text" name="cardNumber" placeholder="Card Number" required value={formData.cardNumber} onChange={handleChange} />
          <div className="payment-row">
            <input type="text" name="expiry" placeholder="MM/YY" required value={formData.expiry} onChange={handleChange} />
            <input type="text" name="cvc" placeholder="CVC" required value={formData.cvc} onChange={handleChange} />
          </div>
          <button type="submit">Pay Now</button>
        </form>
      </div>
    </div>
  );
}

export default PaymentModal;
