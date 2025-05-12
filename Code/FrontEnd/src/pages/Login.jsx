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

            // Optional: Decode token or fetch user details here
            // For now we assume backend uses JWT and you’ll decode it later if needed

            // Example user info (customize this based on your app's token or claims)
            loginAs({ email });

            // Store token (in localStorage or context)
            localStorage.setItem('token', token);

            navigate('/profile');
        } catch (err) {
            setError('Invalid email or password.');
        }
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

        {/* Register button below the form */}
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
