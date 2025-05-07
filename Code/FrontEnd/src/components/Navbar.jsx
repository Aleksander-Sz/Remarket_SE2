import './Navbar.css';
import { Link } from 'react-router-dom';

function Navbar() {
  return (
    <nav className="navbar">
      <div className="navbar-left">
        {/* ✅ Make logo clickable */}
        <Link to="/" className="logo">ReMarket</Link>
      </div>

      <ul className="navbar-center">
        <li><Link to="/categories">Categories</Link></li>
        <li><Link to="/our-stories">Our stories</Link></li>
        <li><Link to="/super-deals">Super deals</Link></li>
        <li><Link to="/about">About Us</Link></li>
        <li><Link to="/dashboard">Dashboard</Link></li>
      </ul>

      <div className="navbar-right">
        <Link to="#"><i className="icon">🔍</i></Link>
        <Link to="/login" className="login-link">Login</Link>
        <Link to="#"><i className="icon">🛒</i></Link>
      </div>
    </nav>
  );
}

export default Navbar;
