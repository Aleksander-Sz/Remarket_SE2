// src/pages/ProductDetail.jsx
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from '../api/axiosInstance';
import './ProductDetail.css';
import { Link } from 'react-router-dom';

function ProductDetail() {
  const { productId } = useParams();
  const [product, setProduct] = useState(null);
  const [reviews, setReviews] = useState([]);

  const [newReview, setNewReview] = useState({
    title: '',
    score: 5,
    description: ''
  });

  const [submitting, setSubmitting] = useState(false);

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

  const handleReviewChange = (e) => {
    const { name, value } = e.target;
    setNewReview(prev => ({ ...prev, [name]: value }));
    };

  const handleReviewSubmit = async (e) => {
    e.preventDefault();
    setSubmitting(true);
    try {
      const response = await axios.post('/addReview', {
        listingId: parseInt(productId),
        title: newReview.title,
        score: parseInt(newReview.score),
        description: newReview.description
      });

      // Optional: prepend the new review to the list (assumes response returns the full review)
      setReviews(prev => [response.data, ...prev]);

      // Reset form
      setNewReview({ title: '', score: 5, description: '' });
    } catch (err) {
      console.error('Failed to submit review', err);
      alert('You must be logged in to submit a review.');
    } finally {
      setSubmitting(false);
    }
  };

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
              <Link to={`/user/${review.account.accountId}`} className="plain-link">
                <strong>{review.account.username}</strong>
              </Link>{' '}
              –{' '}
              <span className="stars">
                {'★'.repeat(review.score)}
                {'☆'.repeat(5 - review.score)}
              </span>
              <p><strong>{review.title}</strong></p>
              <p>{review.description}</p>
            </div>
          ))
        )}

        {/* Review submission form */}
        <form className="review-form" onSubmit={handleReviewSubmit}>
          <h3>Add a Review</h3>
          <label>
            Title:
            <input
              type="text"
              name="title"
              value={newReview.title}
              onChange={handleReviewChange}
              required
            />
          </label>
          <br />
          <label>
            Score:
            <select
              name="score"
              value={newReview.score}
              onChange={handleReviewChange}
            >
              {[5, 4, 3, 2, 1].map(n => (
                <option key={n} value={n}>{n}</option>
              ))}
            </select>
          </label>
          <br />
          <label>
            Description:
            <textarea
              name="description"
              value={newReview.description}
              onChange={handleReviewChange}
              rows="4"
              required
            />
          </label>
          <br />
          <button type="submit" disabled={submitting}>
            {submitting ? 'Submitting...' : 'Submit Review'}
          </button>
        </form>
  </div>
</div>

  );
}

export default ProductDetail;
