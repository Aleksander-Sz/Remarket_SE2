// src/pages/Register.jsx
import './Login.css';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { useUser } from '../context/UserContext';
import axios from '../api/axiosInstance';

function Register() {
  const { loginAs } = useUser();
  const navigate = useNavigate();

  const [form, setForm] = useState({
    username: '',
    email: '',
    password: '',
    role: 'user'
  });

  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    try {
      const response = await axios.post('/register', {
        email: form.email,
        username: form.username,
        password: form.password,
        role: form.role
      });

      const { token } = response.data;
      loginAs({ email: form.email });
      localStorage.setItem('token', token);
      navigate('/profile');
    } catch (err) {
      if (err.response) {
        const status = err.response.status;
        const msg = err.response.data;

        if (status === 400) {
          setError(typeof msg === 'string' ? msg : 'Missing or invalid input.');
        } else if (status === 409) {
          setError('Email or username already exists.');
        } else {
          setError('Something went wrong. Please try again.');
        }
      } else {
        setError('Unable to connect to the server.');
      }
    }
  };

  // ✅ Google and GitHub auth
  const handleGoogleRegister = () => {
    window.location.href = `${import.meta.env.VITE_BACKEND_URL || ''}/auth/google`;
  };

  const handleGithubRegister = () => {
    window.location.href = `${import.meta.env.VITE_BACKEND_URL || ''}/auth/github`;
  };

  return (
    <div className="login-page">
      <div className="login-box">
        <h2>Register</h2>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="Full Name"
            value={form.username}
            onChange={(e) => setForm({ ...form, username: e.target.value })}
            required
          />
          <input
            type="email"
            placeholder="Email"
            value={form.email}
            onChange={(e) => setForm({ ...form, email: e.target.value })}
            required
          />
          <input
            type="password"
            placeholder="Password"
            value={form.password}
            onChange={(e) => setForm({ ...form, password: e.target.value })}
            required
          />
          <select
            value={form.role}
            onChange={(e) => setForm({ ...form, role: e.target.value })}
          >
            <option value="user">User</option>
            <option value="seller">Seller</option>
            <option value="admin">Admin</option>
          </select>
          <button type="submit">Register</button>
          {error && <p style={{ color: 'red' }}>{error}</p>}
          {success && <p style={{ color: 'green' }}>{success}</p>}
        </form>

        {/* Divider */}
        <div className="divider">or</div>

        {/* ✅ External Register Options */}
        <div className="login-options">
          <button className="google" onClick={handleGoogleRegister}>Register with Google</button>
          <button className="github" onClick={handleGithubRegister}>Register with GitHub</button>
        </div>
      </div>
    </div>
  );
}

export default Register;
