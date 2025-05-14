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
        <img src={`data:image/jpeg;base64,${product.imageData}`} alt={product.title} />
      </div>
      <div className="info-section">
        <h1>{product.title}</h1>
        <p><strong>Price:</strong> ${product.price}</p>
        <p><strong>Description:</strong> {product.description?.paragraph}</p>
        <p><strong>Status:</strong> {product.status}</p>
      </div>
    </div>
  );
}

export default ProductDetail;
