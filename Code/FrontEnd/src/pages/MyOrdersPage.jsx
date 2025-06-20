import { useEffect, useState } from 'react';
import axios from '../api/axiosInstance';
import { useUser } from '../context/UserContext';
import { Link } from 'react-router-dom';
import './MyOrdersPage.css';

function MyOrdersPage() {
    const { id: userId } = useUser();
    const [orders, setOrders] = useState([]);
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const response = await axios.get('/orders');
                setOrders(response.data); // Array of orders
            } catch (err) {
                console.error(err);
                setError('Failed to load orders.');
            } finally {
                setLoading(false);
            }
        };

        fetchOrders();
    }, []);

    const formatDate = (isoString) => {
        const date = new Date(isoString);
        return date.toLocaleDateString();
    };

    const purchases = orders.filter(order => order.buyerId === userId);
    const sales = orders.filter(order => order.sellerId === userId);

    return (
        <div className="orders-page">
            <h1>My Orders</h1>

            {loading ? (
                <p>Loading...</p>
            ) : error ? (
                <p style={{ color: 'red' }}>{error}</p>
            ) : (
                <>
                    <h2>Purchased Products</h2>
                    {purchases.length === 0 ? (
                        <p>No purchases yet.</p>
                    ) : (
                        <table>
                            <thead>
                                <tr>
                                    <th>Order ID</th>
                                    <th>Purchase Date</th>
                                    <th>Seller</th>
                                    <th>Ship To</th>
                                    <th>Description</th>
                                </tr>
                            </thead>
                            <tbody>
                                {purchases.map(order => (
                                    <tr key={order.id}>
                                        <td>{order.id}</td>
                                        <td>{formatDate(order.shipped)}</td>
                                        <td><Link to={`/user/${order.sellerId}`}>{order.sellerId}</Link></td>
                                        <td>{order.shipTo}</td>
                                        <td>{order.description}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    )}

                    <h2>Sold Products</h2>
                    {sales.length === 0 ? (
                        <p>No sales yet.</p>
                    ) : (
                        <table>
                            <thead>
                                <tr>
                                    <th>Order ID</th>
                                    <th>Ship Date</th>
                                    <th>Buyer</th>
                                    <th>Ship To</th>
                                    <th>Description</th>
                                </tr>
                            </thead>
                            <tbody>
                                {sales.map(order => (
                                    <tr key={order.id}>
                                        <td>{order.id}</td>
                                        <td>{formatDate(order.shipped)}</td>
                                        <td><Link to={`/user/${order.buyerId}`}>{order.buyerId}</Link></td>
                                        <td>{order.shipTo}</td>
                                        <td>{order.description}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    )}
                </>
            )}
        </div>
    );
}

export default MyOrdersPage;
