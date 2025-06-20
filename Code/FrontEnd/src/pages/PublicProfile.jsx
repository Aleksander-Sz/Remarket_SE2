// src/pages/ProductDetail.jsx
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from '../api/axiosInstance';
import './PublicProfile.css';

function ProductDetail() {
  const { profileId } = useParams();
    const [profile, setProfile] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [reviews, setReviews] = useState([]);

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
        const fetchReviews = async () => {
            try {
                const response = await axios.get(`/reviews/forUser/${profileId}`);
                setReviews(response.data);
            } catch (err) {
                console.error("Failed to load reviews", err);
            }
        };

        fetchProfile();
        fetchReviews();
        
    }, [profileId]);

  if (!profile) return <p>Loading...</p>;

    const averageScore = reviews.length
        ? reviews.reduce((sum, r) => sum + r.score, 0) / reviews.length : 0;

    const roundedScore = Math.round(averageScore);

  return (
    <div className="public-profile-page">
  <div className="image-section">
    <img src={`/api/photo/${profile.photoId}`} alt={profile.username} />
  </div>
  <br/>
    <div className="info-section">
      <p><strong>{profile.username}</strong></p>
      {reviews.length > 0 && (
      <span className="stars">
        {'★'.repeat(roundedScore)}
        {'☆'.repeat(5 - roundedScore)}
      </span>
      )}
      <p>{profile.description}</p>
    </div>
    <div className="reviews-section">
        <h2>Reviews for this user's products</h2>
        {reviews.length === 0 ? (
            <p>No reviews found.</p>
        ) : (
            reviews.map((review, idx) => (
                <div className="review-card" key={idx}>
                    <p>
                        <strong>
                            <a href={`/user/${review.reviewer.id}`} className="plain-link">
                                {review.reviewer.username}
                            </a>
                        </strong>{' '}
                        on listing <strong>{review.listing.title}</strong>
                    </p>
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
