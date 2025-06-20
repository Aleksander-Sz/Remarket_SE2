import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

function PurchaseForm() {
    const { productId } = useParams();
    const [order, setOrder] = useState(null);

    useEffect(() => {
        axios.get(`http://localhost:5134/api/orders?orderId=${productId}`)
            .then(res => setOrder(res.data))
            .catch(err => console.error("Failed to load order", err));
    }, [productId]);

    if (!order) return <p style={{ textAlign: 'center', color: 'white' }}>Loading product info...</p>;

    const inputStyle = {
        display: 'block',
        width: '100%',
        padding: '0.5rem',
        marginTop: '0.25rem',
        borderRadius: '4px',
        border: '1px solid #ccc',
        fontSize: '1rem',
    };

    const halfInputStyle = {
        ...inputStyle,
        width: '48%',
        display: 'inline-block',
    };

    return (
        <div style={{
            padding: '2rem',
            backgroundColor: '#222',
            color: 'white',
            borderRadius: '8px',
            maxWidth: '500px',
            margin: '2rem auto',
            fontFamily: 'Arial, sans-serif'
        }}>
            <h2>Purchase: {order.product?.productName}</h2>
            <p>Price: {order.product?.productPrice} $</p>
            <p>Ship To: {order.shipTo}</p>
            <p>Description: {order.description}</p>

            <form>
                <label style={{ display: 'block', marginBottom: '0.75rem' }}>
                    Your Name:
                    <input type="text" name="name" required style={inputStyle} />
                </label>

                <label style={{ display: 'block', marginBottom: '0.75rem' }}>
                    Address:
                    <input type="text" name="address" required style={inputStyle} />
                </label>

                <label style={{ display: 'block', marginBottom: '0.75rem' }}>
                    Credit Card Number:
                    <input type="text" name="creditCard" placeholder="1234 5678 9012 3456" style={inputStyle} />
                </label>

                <label style={{ display: 'block', marginBottom: '0.75rem' }}>
                    Cardholder Name:
                    <input type="text" name="cardholderName" placeholder="John Doe" style={inputStyle} />
                </label>

                <div style={{ marginBottom: '1rem' }}>
                    <label style={{ fontWeight: 'bold' }}>Expiration Date:</label><br />
                    <input type="text" name="expiryMonth" placeholder="MM" maxLength={2} style={{ ...halfInputStyle, marginRight: '4%' }} />
                    <input type="text" name="expiryYear" placeholder="YY" maxLength={2} style={halfInputStyle} />
                </div>

                <label style={{ display: 'block', marginBottom: '1rem' }}>
                    CVC/CVV:
                    <input type="text" name="cvc" placeholder="123" maxLength={3} style={{ width: '100px', padding: '0.5rem', borderRadius: '4px', border: '1px solid #ccc', fontSize: '1rem' }} />
                </label>

                <button type="submit" style={{ backgroundColor: '#007bff', color: 'white', padding: '0.75rem 1.5rem', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '1rem', width: '100%' }}>
                    Submit Order
                </button>
            </form>
        </div>
    );
}

export default PurchaseForm;