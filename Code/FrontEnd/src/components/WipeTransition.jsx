import { motion } from 'framer-motion';
import './WipeTransition.css';

function WipeTransition() {
  return (
    <motion.div
      className="wipe-overlay"
      initial={{ x: '-100%' }}
      animate={{ x: '100%' }}
      exit={{ x: '0%' }}
      transition={{ duration: 0.6, ease: 'easeInOut' }}
    />
  );
}

export default WipeTransition;
