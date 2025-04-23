import './App.css';
import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom';
import { AnimatePresence } from 'framer-motion';

import WipeTransition from './components/WipeTransition';
import Navbar from './components/Navbar';
import HeroBanner from './components/HeroBanner';
import CategoryGrid from './components/CategoryGrid';
import QuoteSection from './components/QuoteSection';
import GalleryShowcase from './components/GalleryShowcase';
import Footer from './components/Footer';
import Login from './components/Login';


import Login from './pages/Login';
import ClothesPage from './pages/ClothesPage';
import AccessoriesPage from './pages/AccessoriesPage';
import ToysPage from './pages/ToysPage';
import KidsPage from './pages/KidsPage';
import WomenPage from './pages/WomenPage';
import MenPage from './pages/MenPage';
import OurStories from './pages/OurStories';

function AnimatedRoutes() {
  const location = useLocation();

  return (
    <AnimatePresence mode="wait">
      <Routes location={location} key={location.pathname}>
        {/* Homepage */}
        <Route path="/" element={
          <>
            <WipeTransition />
            <HeroBanner />
            <QuoteSection />
            <GalleryShowcase />
            <Footer />
          </>
        } />

        {/* Categories Page */}
        <Route path="/categories" element={
          <>
            <WipeTransition />
            <CategoryGrid />
            <QuoteSection />
            <GalleryShowcase />
            <Footer />
          </>
        } />

        {/* Other Pages */}
        <Route path="/our-stories" element={<><WipeTransition /><OurStories /></>} />
        <Route path="/login" element={<><WipeTransition /><Login /></>} />
        <Route path="/category/clothes" element={<><WipeTransition /><ClothesPage /></>} />
        <Route path="/category/accessories" element={<><WipeTransition /><AccessoriesPage /></>} />
        <Route path="/category/toys" element={<><WipeTransition /><ToysPage /></>} />
        <Route path="/category/kids" element={<><WipeTransition /><KidsPage /></>} />
        <Route path="/category/women" element={<><WipeTransition /><WomenPage /></>} />
        <Route path="/category/men" element={<><WipeTransition /><MenPage /></>} />
      </Routes>
    </AnimatePresence>
  );
}

function App() {
  return (
    <Router>
      <Navbar />
      <AnimatedRoutes />
    </Router>
  );
}

export default App;
