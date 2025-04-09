import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

import Navbar from './components/Navbar';
import HeroBanner from './components/HeroBanner';
import CategoryGrid from './components/CategoryGrid';
import QuoteSection from './components/QuoteSection';
import GalleryShowcase from './components/GalleryShowcase';
import Footer from './components/Footer';
import Login from './components/Login';


import OurStories from './components/OurStories'; // ✅ Updated path here

function App() {
  return (
    <Router>
      <Navbar />
      <Routes>
        {/* Homepage */}
        <Route path="/" element={
          <>
            <HeroBanner />
            <QuoteSection />
            <GalleryShowcase />
            <Footer />
          </>
        } />

        {/* Categories Page */}
        <Route path="/categories" element={
          <>
            <CategoryGrid />
            <QuoteSection />
            <GalleryShowcase />
            <Footer />
          </>
        } />

        {/* Our Stories Page */}
        <Route path="/our-stories" element={
          <>
            <OurStories />
          </>
        } />
      <Route path="/login" element={<Login />} />
      </Routes>

    </Router>
  );
}

export default App;
