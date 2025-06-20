import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import axios from '../api/axiosInstance';
import './ProductDetail.css'; // Reuse existing styling, or create a new one

function PaymentDetail() {
    const { paymentId } = useParams();
    const [payment, setPayment] = useState(null);
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchPayment = async () => {
            try {
                const response = await axios.get(`/payment/${paymentId}`);
                setPayment(response.data);
            } catch (err) {
                console.error('Failed to load payment detail', err);
                setError('Failed to load payment details.');
            } finally {
                setLoading(false);
            }
        };

        fetchPayment();
    }, [paymentId]);

    if (loading) return <p>Loading...</p>;
    if (error) return <p style={{ color: 'red' }}>{error}</p>;
    if (!payment) return <p>No payment found.</p>;

    return (
        <div className="product-detail-page">
            <h1>Payment Details</h1>
            <div className="info-section">
                <p><strong>Payment ID:</strong> {payment.id}</p>
                <p><strong>Paid On:</strong> {new Date(payment.paidOn).toLocaleDateString()}</p>
                <p><strong>Total:</strong> ${payment.total}</p>
                <p>
                    <strong>Paid By:</strong>{' '}
                    <Link to={`/user/${payment.accountId}`} className="plain-link">
                        User #{payment.accountId}
                    </Link>
                </p>
            </div>
        </div>
    );
}

export default PaymentDetail;
