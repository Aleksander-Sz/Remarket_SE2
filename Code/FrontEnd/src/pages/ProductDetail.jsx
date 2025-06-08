// src/pages/ProductDetail.jsx
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from '../api/axiosInstance';
import './ProductDetail.css';

function ProductDetail() {
  const { productId } = useParams();
  const [product, setProduct] = useState(null);

  useEffect(() => {
    axios.get(`/products?id=${productId}`)
      .then(res => setProduct(res.data))
      .catch(err => console.error('Failed to load product detail', err));
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
    <p><strong>Seller:</strong> {product.owner?.username}</p>
    <p><strong>Description:</strong> {product.description?.header}</p>
    <p>{product.description?.paragraph}</p>
  </div>

  <div className="reviews-section">
    <h2>Reviews</h2>
    {[
      { name: 'Alice', rating: 5, text: 'Absolutely love it!' },
      { name: 'Bob', rating: 4, text: 'Very useful, fast delivery.' },
      { name: 'Charlie', rating: 3, text: 'It’s okay, but has minor flaws.' },
    ].map((review, idx) => (
      <div className="review-card" key={idx}>
        <strong>{review.name}</strong> – <span className="stars">{'★'.repeat(review.rating)}{'☆'.repeat(5 - review.rating)}</span>
        <p>{review.text}</p>
      </div>
    ))}
  </div>
</div>

  );
}

export default ProductDetail;
