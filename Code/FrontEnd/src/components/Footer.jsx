import './Footer.css';

function Footer() {
  return (
    <footer className="footer">
      <div className="footer-top">
        <div className="footer-brand">
          <h2>ReMarket</h2>
          <p>© 2025 ReMarket. All rights reserved.</p>
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
