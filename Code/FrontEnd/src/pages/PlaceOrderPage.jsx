import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from '../api/axiosInstance';
import './ProfilePage.css';

function EditProfilePage() {
    const { productId } = useParams();
    const navigate = useNavigate();

    const [product, setProduct] = useState(null);
    const [formData, setFormData] = useState({
        address: '',
        comments: ''
    });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchProduct = async () => {
            try {
                const res = await axios.get(`/products?id=${productId}`);
                setProduct(res.data);
            } catch (err) {
                console.error('Failed to fetch product:', err);
                setError('Failed to load product information.');
            } finally {
                setLoading(false);
            }
        };

        fetchProduct();
    }, [productId]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e, redirectTo, includeOrderId = false) => {
        e.preventDefault();

        if (!product || !product.owner?.id) {
            alert('Invalid product or seller info.');
            return;
        }

        const payload = {
            shipTo: formData.address,
            description: formData.comments,
            sellerId: product.owner.id
        };

        try {
            const response = await axios.post('/createOrder', payload);
            const orderId = response.data.id;

            alert('Order placed successfully!');

            if (includeOrderId) {
                navigate(`${redirectTo}/${orderId}`);
            } else {
                navigate(redirectTo);
            }
        } catch (err) {
            console.error('Error placing order:', err);
            alert('You must be logged in to place an order.');
        }
    };


    return (
        <div className="profile-container">
            <h1>Place an order for the product</h1>
            {loading ? (
                <p>Loading...</p>
            ) : error ? (
                <p className="error">{error}</p>
            ) : (
                    <form className="product-form" onSubmit={handleSubmit}>
                    <label>
                        Address:
                        <textarea
                            name="address"
                            value={formData.bio}
                            onChange={handleChange}
                            rows="4"
                        />
                    </label><br />
                    <label>
                        Comments:
                        <textarea
                            name="comments"
                            value={formData.bio}
                            onChange={handleChange}
                            rows="4"
                        />
                    </label><br />

                    <button type="button" onClick={(e) => handleSubmit(e, '/purchase', true)}>Pay now</button>
                    <button type="button" onClick={(e) => handleSubmit(e, '/profile', false)}>Pay later</button>

                </form>
            )}
        </div>
    );
}

export default EditProfilePage;
