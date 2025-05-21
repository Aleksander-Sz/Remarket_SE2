import './Navbar.css';
import { Link, useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import axios from '../api/axiosInstance';

function Navbar() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const checkLogin = async () => {
      try {
        await axios.get('/account'); // backend will return 401 if not logged in
        setIsLoggedIn(true);
      } catch (err) {
        setIsLoggedIn(false);
      }
    };

    checkLogin();
  }, []);

  return (
    <nav className="navbar">
      <div className="navbar-left">
        <Link to="/" className="logo">ReMarket</Link>
      </div>

      <ul className="navbar-center">
        <li><Link to="/products">Products</Link></li>
        <li><Link to="/our-stories">Our stories</Link></li>
        <li><Link to="/super-deals">Super deals</Link></li>
        <li><Link to="/about">About Us</Link></li>
        <li><Link to="/dashboard">Dashboard</Link></li>
      </ul>

      <div className="navbar-right">
        <Link to="#"><i className="icon">🔍</i></Link>

        {isLoggedIn ? (
          <>
            <Link to="/profile" className="login-link">Profile</Link>
            <Link to="/cart"><i className="icon">🛒</i></Link>
          </>
        ) : (
          <Link to="/login" className="login-link">Login</Link>
        )}
      </div>
    </nav>
  );
}

export default Navbar;
