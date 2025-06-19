// src/pages/ProductDetail.jsx
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from '../api/axiosInstance';
import './ProductDetail.css';

function ProductDetail() {
  const { profileId } = useParams();
    const [profile, setProfile] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const response = await axios.get(`/user/${profileId}`);
                setProfile(response.data);
            } catch (err) {
                setError('Failed to load profile.');
            } finally {
                setLoading(false);
            }
        };

        fetchProfile();
    }, [profileId]);

  if (!profile) return <p>Loading...</p>;

  return (
    <div className="product-detail-page">
  <div className="image-section">
    <img src={`/api/photo/${profile.photoId}`} alt={profile.username} />
  </div>
  <br/>
  <div className="info-section">
    <p><strong>{profile.username}</strong></p>
    <p>{profile.description}</p>
  </div>
</div>

  );
}

export default ProductDetail;
