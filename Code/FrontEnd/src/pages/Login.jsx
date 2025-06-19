// src/pages/Login.jsx
import './Login.css';
import { useUser } from '../context/UserContext';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import axios from '../api/axiosInstance';

function Login() {
  const { loginAs } = useUser();
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleLogin = async (e) => {
    e.preventDefault();
    setError('');

    try {
      const response = await axios.post('/login', {
        email,
        password
      });

      const { token } = response.data;

      loginAs({ email });
      localStorage.setItem('token', token);
      navigate('/profile');
    } catch (err) {
      setError('Invalid email or password.');
    }
  };

  // ✅ Google login method
  const handleGoogleLogin = () => {
    window.location.href = `${import.meta.env.VITE_BACKEND_URL || ''}/auth/google`;
  };

  // ✅ GitHub login method
  const handleGithubLogin = () => {
    window.location.href = `${import.meta.env.VITE_BACKEND_URL || ''}/auth/github`;
  };

  return (
    <div className="login-page">
      <div className="login-box">
        <h2>Login to ReMarket</h2>
        <form onSubmit={handleLogin}>
          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <button type="submit">Login</button>
          {error && <p style={{ color: 'red' }}>{error}</p>}
        </form>

        {/* Divider */}
        <div className="divider">or</div>

        {/* ✅ Login Options */}
        <div className="login-options">
          <button className="google" onClick={handleGoogleLogin}>Login with Google</button>
          <button className="github" onClick={handleGithubLogin}>Login with GitHub</button>
        </div>

        <p style={{ marginTop: '1rem' }}>
          Don’t have an account?{' '}
          <button
            onClick={() => navigate('/register')}
            style={{
              background: 'none',
              border: 'none',
              color: '#4bb869',
              textDecoration: 'underline',
              cursor: 'pointer'
            }}
          >
            Register here
          </button>
        </p>
      </div>
    </div>
  );
}

export default Login;
