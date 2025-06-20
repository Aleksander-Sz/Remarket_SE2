import React, { useEffect, useState } from 'react';
import axios from '../api/axiosInstance';
import { useUser } from '../context/UserContext';
import { useNavigate } from 'react-router-dom';
import './MyOrdersPage.css'; // dodaj, jeœli chcesz style

function MyOrdersPage() {
    const { id: userId } = useUser();
    const [sales, setSales] = useState([]); // zamówienia gdzie ja jestem sprzedawc¹
    const [purchases, setPurchases] = useState([]); // zamówienia gdzie ja jestem kupuj¹cym
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        if (!userId) return;

        const fetchOrders = async () => {
            try {
                const response = await axios.get('/orders', {
                    params: { userId }
                });
                // Zak³adam, ¿e backend zwraca strukturê, np:
                // { sales: [...], purchases: [...] }
                setSales(response.data.sales);
                setPurchases(response.data.purchases);
            } catch (err) {
                setError('Failed to load orders.');
            } finally {
                setLoading(false);
            }
        };

        fetchOrders();
    }, [userId]);

    if (loading) return <p>Loading orders...</p>;
    if (error) return <p style={{ color: 'red' }}>{error}</p>;

    const formatDate = (dateString) => new Date(dateString).toLocaleDateString();

    return (
        <div className="myorders-container">
            <h1>My Sales (Orders of my products)</h1>
            {sales.length === 0 ? (
                <p>No sales yet.</p>
            ) : (
                <table className="orders-table">
                    <thead>
                        <tr>
                            <th>Product</th>
                            <th>Date Purchased</th>
                            <th>Amount</th>
                            <th>Buyer</th>
                            <th>Details</th>
                        </tr>
                    </thead>
                    <tbody>
                        {sales.map(order => (
                            <tr key={order.id}>
                                <td>{order.productTitle}</td>
                                <td>{formatDate(order.purchaseDate)}</td>
                                <td>${order.price.toFixed(2)}</td>
                                <td>{order.buyerName}</td>
                                <td>
                                    <button onClick={() => navigate(`/product/${order.productId}`)}>View</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}

            <h1>My Purchases</h1>
            {purchases.length === 0 ? (
                <p>No purchases yet.</p>
            ) : (
                <table className="orders-table">
                    <thead>
                        <tr>
                            <th>Product</th>
                            <th>Date Purchased</th>
                            <th>Amount</th>
                            <th>Seller</th>
                            <th>Details</th>
                        </tr>
                    </thead>
                    <tbody>
                        {purchases.map(order => (
                            <tr key={order.id}>
                                <td>{order.productTitle}</td>
                                <td>{formatDate(order.purchaseDate)}</td>
                                <td>${order.price.toFixed(2)}</td>
                                <td>{order.sellerName}</td>
                                <td>
                                    <button onClick={() => navigate(`/product/${order.productId}`)}>View</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}

export default MyOrdersPage;
