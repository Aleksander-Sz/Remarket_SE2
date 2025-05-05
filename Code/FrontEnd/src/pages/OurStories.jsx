import './OurStories.css';
import Footer from '../components/Footer';
import storyImage from '../assets/secondhand.jpg'; // rename as needed

function OurStories() {
  return (
    <div className="our-stories">
      <h1>What is remarket</h1>

      <img src={storyImage} alt="Second Hand" className="story-image" />
      <p className="img-caption">Our mission: make reuse the first choice, not the last resort.</p>

      <div className="story-content">

<h2>ReMarket: A Smarter Way to Buy and Sell Second-Hand Goods</h2>
<p>
  In a world where fast fashion and consumerism continue to accelerate, ReMarket emerges as a digital solution to one of the most pressing challenges of our time: waste reduction and sustainable consumption...
</p>

<h3>🌿 The Philosophy Behind ReMarket</h3>
<p>
  ReMarket champions the circular economy — reuse, resale, and responsible consumption to reduce harm. It’s not just about making second-hand shopping easier; it’s about transforming it into a smart, scalable experience.
</p>

<h3>🧍 Built for Real People: Buyers, Sellers, and Admins Alike</h3>

<h4>👥 For Buyers:</h4>
<ul>
  <li>Discover Listings with detailed product pages, images, and categorization</li>
  <li>Search and filter by category, price, popularity</li>
  <li>Wishlist items and track orders</li>
  <li>Leave reviews to build trust</li>
</ul>

<h4>🛍️ For Sellers:</h4>
<ul>
  <li>Create listings with flexible pricing and rich descriptions</li>
  <li>Manage availability and respond to buyers</li>
  <li>Set shipping options that suit your workflow</li>
</ul>

<h4>🛠️ For Admins:</h4>
<ul>
  <li>Moderate content and user safety</li>
  <li>Monitor suspicious activity and platform metrics</li>
  <li>Maintain integrity with error handling and backups</li>
</ul>

<h3>⚙️ A Full E-Commerce Engine — Just for Pre-Loved Goods</h3>

<ul>
  <li>🔐 <strong>Authentication & Security:</strong> Secure login, encrypted data (bcrypt), password resets</li>
  <li>💳 <strong>Robust Checkout:</strong> End-to-end order tracking and payment integration</li>
  <li>📦 <strong>Order Management:</strong> Support for states like Draft, Published, Sold, Archived</li>
  <li>📊 <strong>Analytics & Admin Tools:</strong> Full admin dashboard for review and reporting</li>
</ul>

<p>
  Built with modular architecture, ReMarket is ready to grow — connect APIs, launch mobile apps, or scale operations, all without tech debt. 🌍
</p>

</div>


      <Footer />
    </div>
  );
}

export default OurStories;
