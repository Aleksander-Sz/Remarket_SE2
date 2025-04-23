import './HeroBanner.css';
import heroImage from '../assets/banner-image.jpg'; // replace this with your image path
import { motion } from 'framer-motion';

function HeroBanner() {
  return (
    <section className="hero">
      <motion.div
        className="hero-text"
        initial={{ opacity: 0, y: -60 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.8, ease: 'easeOut' }}
      >
        <h1>All of the second hand goods</h1>
        <p>
          Second-hand shopping promotes sustainability by giving pre-owned items
          a second life instead of sending them to landfills. It’s an affordable,
          eco-friendly alternative to fast consumerism — helping people save money
          while reducing environmental impact. From fashion to electronics,
          second-hand goods carry stories worth continuing.
        </p>
      </motion.div>

      <motion.div
        className="hero-image"
        initial={{ opacity: 0, x: 100 }}
        animate={{ opacity: 1, x: 0 }}
        transition={{ duration: 0.8, ease: 'easeOut', delay: 0.4 }}
      >
        <img src={heroImage} alt="stacked vintage luggage" />
      </motion.div>
    </section>
  );
}

export default HeroBanner;
