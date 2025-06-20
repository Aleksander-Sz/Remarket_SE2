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
    const [usernames, setUsernames] = useState({});

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const response = await axios.get('/orders');
                setOrders(response.data);

                const userIds = Array.from(new Set(
                    response.data.flatMap(order => [order.buyerId, order.sellerId])
                ));

                const userResponses = await Promise.all(
                    userIds.map(id =>
                        axios.get(`/user/${id}`)
                            .then(res => ({ id, username: res.data.username }))
                            .catch(() => ({ id, username: 'Inexistent user' }))
                    )
                );

                const usernameMap = {};
                userResponses.forEach(user => {
                    usernameMap[user.id] = user.username;
                });

                setUsernames(usernameMap);
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
                                    <th>Product</th>
                                    <th>Product Name</th>
                                    <th>Payment</th>
                                </tr>
                            </thead>
                            <tbody>
                                {purchases.map(order => (
                                    <tr key={order.id}>
                                        <td>{order.id}</td>
                                        <td>{formatDate(order.shipped)}</td>
                                        <td>
                                            <Link to={`/user/${order.sellerId}`} className="plain-link">
                                                {usernames[order.sellerId] || order.sellerId}
                                            </Link>
                                        </td>
                                        <td>{order.shipTo}</td>
                                        <td>{order.description}</td>
                                        <td>
                                            <Link to={`/products/${order.listingId}`} className="plain-link">
                                                View
                                            </Link>
                                        </td>
                                        <td>{order.title}</td>
                                        <td>
                                            <Link to={order.paymentId
                                                ? `/payment/${order.paymentId}`
                                                : `/purchase/${order.id}`}>
                                                {order.paymentId
                                                    ? `#${order.paymentId}`
                                                    : 'Complete Payment'}
                                            </Link>
                                        </td>
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
                                    <th>Product</th>
                                    <th>Product Name</th>
                                    <th>Payment</th>
                                </tr>
                            </thead>
                            <tbody>
                                {sales.map(order => (
                                    <tr key={order.id}>
                                        <td>{order.id}</td>
                                        <td>{formatDate(order.shipped)}</td>
                                        <td>
                                            <Link to={`/user/${order.buyerId}`} className="plain-link">
                                                {usernames[order.buyerId] || order.buyerId}
                                            </Link>
                                        </td>
                                        <td>{order.shipTo}</td>
                                        <td>{order.description}</td>
                                        <td>
                                            <Link to={`/products/${order.listingId}`} className="plain-link">
                                                View
                                            </Link>
                                        </td>
                                        <td>{order.title}</td>
                                        <td>
                                            {order.paymentId ? (
                                                <Link to={`/payment/${order.paymentId}`}>
                                                    #{order.paymentId}
                                                </Link>
                                            ) : (
                                                <span style={{ color: 'gray', fontStyle: 'italic' }}>
                                                    Waiting for payment
                                                </span>
                                            )}
                                        </td>
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
