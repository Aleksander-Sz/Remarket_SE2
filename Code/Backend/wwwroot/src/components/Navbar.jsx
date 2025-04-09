import './Navbar.css';
import { FaSearch, FaShoppingCart } from 'react-icons/fa';
import { Link } from 'react-router-dom';


function Navbar() {
  return (
    <nav className="navbar">
      <div className="navbar-left">
        <span className="logo">ReMarket</span>
      </div>

      <ul className="navbar-center">
      <ul className="navbar-center">
  <li><Link to="/categories">Categories</Link></li>
  <li><Link to="/our-stories">Our stories</Link></li>
  <li><Link to="/super-deals">Super deals</Link></li>
  <li><Link to="/about">About Us</Link></li>
</ul>
</ul>

<div className="navbar-right">
  <Link to="#"><i className="icon">🔍</i></Link>
  <Link to="#" className="login-link">Login</Link>
  <Link to="#"><i className="icon">🛒</i></Link>
</div>

    </nav>
  );
}

export default Navbar;
