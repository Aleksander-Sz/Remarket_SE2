// src/pages/ProfilePage.jsx
import { useEffect, useState } from 'react';
import axios from '../api/axiosInstance';
import './ProfilePage.css';
import { useUser } from '../context/UserContext';
import { useNavigate } from 'react-router-dom';

function ProfilePage() {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const { logout } = useUser();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = await axios.get('/account');
        setUser(response.data); // { username, email }
      } catch (err) {
        if (err.response && err.response.status === 401) {
          setError('You must be logged in to view this page.');
        } else {
          setError('Failed to load profile.');
        }
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, []);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="profile-container">
      {loading ? (
        <p>Loading profile...</p>
      ) : error ? (
        <p style={{ color: 'red' }}>{error}</p>
      ) : (
        <>
          <h1>Welcome, {user.username}!</h1>
          <p>Email: {user.email}</p>
          <button className="logout-btn" onClick={handleLogout}>
            Log Out
          </button>
        </>
      )}
    </div>
  );
}

export default ProfilePage;
