// src/pages/ProductDetail.jsx
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from '../api/axiosInstance';
import './ProductDetail.css';

function ProductDetail() {
  const { productId } = useParams();
  const [product, setProduct] = useState(null);
  const [reviews, setReviews] = useState([]);

  useEffect(() => {
    // Fetch product
    axios.get(`/products?id=${productId}`)
      .then(res => setProduct(res.data))
      .catch(err => console.error('Failed to load product detail', err));
    // Fetch reviews
    axios.get(`/reviews?listingId=${productId}`)
      .then(res => setReviews(res.data))
      .catch(err => console.error('Failed to load reviews', err));
  }, [productId]);

  if (!product) return <p>Loading...</p>;

  return (
    <div className="product-detail-page">
  <div className="image-section">
    <img src={`/api/photo/${product.photoIds[0]}`} alt={product.title} />
  </div>

  <div className="info-section">
    <h1>{product.title}</h1>
    <p><strong>Price:</strong> ${product.price}</p>
    <p><strong>Status:</strong> {product.status}</p>
    <a href={`/user/${product.owner.id}`} class="plain-link"><p><strong>Seller:</strong> {product.owner?.username}</p></a>
    <p><strong>Description:</strong> {product.description?.header}</p>
    <p>{product.description?.paragraph}</p>
  </div>

  <div className="reviews-section">
        <h2>Reviews</h2>
        {reviews.length === 0 ? (
          <p>No reviews yet.</p>
        ) : (
          reviews.map((review, idx) => (
            <div className="review-card" key={idx}>
              <strong>{review.account.username}</strong> –{' '}
              <span className="stars">
                {'★'.repeat(review.score)}
                {'☆'.repeat(5 - review.score)}
              </span>
              <p><strong>{review.title}</strong></p>
              <p>{review.description}</p>
            </div>
          ))
        )}
  </div>
</div>

  );
}

export default ProductDetail;
