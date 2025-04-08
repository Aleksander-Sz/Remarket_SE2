import './Footer.css';

function Footer() {
  return (
    <footer className="footer">
      <div className="footer-top">
        <div className="footer-brand">
          <h2>ReMarket</h2>
          <p>© 2025 ReMarket. All rights reserved.</p>
        </div>

        <div className="footer-links">
          <div>
            <h4>Info</h4>
            <ul>
              <li>About</li>
              <li>Stories</li>
              <li>Deals</li>
              <li>Help</li>
            </ul>
          </div>

          <div>
            <h4>Account</h4>
            <ul>
              <li>Login</li>
              <li>Register</li>
              <li>Orders</li>
              <li>Wishlist</li>
            </ul>
          </div>
        </div>
      </div>

      <div className="footer-bottom">
        <span>🌍 English (EN)</span>
        <div className="social-icons">
          <i className="fa-brands fa-facebook"></i>
          <i className="fa-brands fa-instagram"></i>
        </div>
      </div>
    </footer>
  );
}

export default Footer;
