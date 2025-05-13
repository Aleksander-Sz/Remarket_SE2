// WipeTransition.jsx
import { motion } from 'framer-motion';
import './WipeTransition.css';

function WipeTransition() {
  return (
    <motion.div
      className="wipe-overlay"
      initial={{ opacity: 0, scale: 0.95 }}
      animate={{ opacity: 1, scale: 1 }}
      exit={{ opacity: 0, scale: 1.05 }}
      transition={{ duration: 0.5, ease: 'easeInOut' }}
    />
  );
}

export default WipeTransition;
