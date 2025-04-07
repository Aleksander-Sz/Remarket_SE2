import './HeroBanner.css';
import heroImage from '../assets/banner-image.jpg'; // replace this with your image path

function HeroBanner() {
  return (
    <section className="hero">
      <div className="hero-text">
        <h1>All of the second hand goods</h1>
        <p>
          Second-hand shopping promotes sustainability by giving pre-owned items
          a second life instead of sending them to landfills. It’s an affordable,
          eco-friendly alternative to fast consumerism — helping people save money
          while reducing environmental impact. From fashion to electronics,
          second-hand goods carry stories worth continuing.
        </p>
      </div>
      <div className="hero-image">
        <img src={heroImage} alt="stacked vintage luggage" />
      </div>
    </section>
  );
}

export default HeroBanner;
