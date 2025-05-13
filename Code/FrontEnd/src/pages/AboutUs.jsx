// src/pages/AboutUs.jsx
import './AboutUs.css';

function AboutUs() {
  return (
    <div className="about-page">
      <div className="about-content">
        <h1>About ReMarket</h1>
        <p>
          ReMarket is a platform built for a Software Engineering course project at Warsaw University of Technology.
          It enables users to buy and sell secondhand items, promoting sustainable consumption and reducing waste.
        </p>
        <p>
          Our 4-person team has worked together on frontend and backend integration, user account management,
          product listings, wishlist functionality, and more.
        </p>
        <p>
          Key features implemented include:
        </p>
        <ul style={{ textAlign: 'left', maxWidth: '600px', margin: '1rem auto' }}>
          <li>User login and registration with role-based access</li>
          <li>Dynamic product listing from the database</li>
          <li>Wishlist and filtering functionality</li>
          <li>Admin and seller dashboards</li>
          <li>Clean responsive UI with animations</li>
        </ul>
        <p>
          This project reflects our understanding of full-stack development, team collaboration, and practical
          software engineering practices.
        </p>
      </div>
    </div>
  );
}

export default AboutUs;
