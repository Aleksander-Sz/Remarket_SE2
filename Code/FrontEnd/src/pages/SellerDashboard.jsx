import React, { useEffect, useState } from 'react';
import { useUser } from '../context/UserContext';
import axios from '../api/axiosInstance';
import './SellerDashboard.css';

function SellerDashboard() {
  const { email } = useUser();
  const [listings, setListings] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchListings = async () => {
      try {
        const res = await axios.get('/products', {
          params: { seller: email }
        });
        setListings(res.data);
      } catch (err) {
        console.error('Failed to fetch listings:', err);
      } finally {
        setLoading(false);
      }
    };

    if (email) fetchListings();
  }, [email]);

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this listing?')) return;
    try {
      await axios.delete(`/products/${id}`);
      setListings((prev) => prev.filter((item) => item.id !== id));
    } catch (err) {
      alert('Failed to delete listing');
      console.error(err);
    }
  };

  const handleEdit = (id) => {
    window.location.href = `/edit/${id}`;
  };

  return (
    <div className="seller-dashboard">
      <h2>My Project Listings</h2>
      {loading ? (
        <p>Loading...</p>
      ) : listings.length === 0 ? (
        <p>
          You haven’t uploaded anything yet.{' '}
          <a href="/upload" className="upload-link">Upload your first project</a>
        </p>
      ) : (
        <div className="dashboard-grid">
          {listings.map((item) => (
            <div className="dashboard-card" key={item.id}>
              <img src={`/api/photo/${item.photoIds?.[0]}`} alt={item.title} />
              <h3>{item.title}</h3>
              <p>${item.price}</p>
              <div className="actions">
                <button onClick={() => handleEdit(item.id)}>Edit</button>
                <button className="danger" onClick={() => handleDelete(item.id)}>Delete</button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default SellerDashboard;
