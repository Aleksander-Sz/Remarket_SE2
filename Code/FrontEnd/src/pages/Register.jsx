// src/pages/Register.jsx
import './Login.css';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { dummyUsers } from '../data/dummyUsers';

function Register() {
  const navigate = useNavigate();

  const [form, setForm] = useState({
    name: '',
    email: '',
    password: '',
    role: 'user'
  });

  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    const exists = dummyUsers.find(u => u.email === form.email);
    if (exists) {
      setError('Email already registered.');
      setSuccess('');
      return;
    }

    dummyUsers.push(form);
    setSuccess('Registered successfully. You can now log in.');
    setError('');
    setForm({ name: '', email: '', password: '', role: 'user' });

    setTimeout(() => navigate('/login'), 1500);
  };

  return (
    <div className="login-page">
      <div className="login-box">
        <h2>Register</h2>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="Full Name"
            value={form.name}
            onChange={(e) => setForm({ ...form, name: e.target.value })}
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
      </div>
    </div>
  );
}

export default Register;
