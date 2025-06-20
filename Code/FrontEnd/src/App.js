import './App.css';
import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom';
import { AnimatePresence } from 'framer-motion';

import Navbar from './components/Navbar';
import WipeTransition from './components/WipeTransition';
import HeroBanner from './components/HeroBanner';
import CategoryGrid from './components/CategoryGrid';
import QuoteSection from './components/QuoteSection';
import GalleryShowcase from './components/GalleryShowcase';
import Footer from './components/Footer';

import { WishlistProvider } from './context/WishlistContext';
import { UserProvider } from './context/UserContext';
import { CartProvider } from './context/CartContext';

import ListingGrid from './components/ListingGrid';
import ProductDetail from './pages/ProductDetail';
import CartPage from './pages/CartPage';

// Pages
import Login from './pages/Login';
import ClothesPage from './pages/ClothesPage';
import AccessoriesPage from './pages/AccessoriesPage';
import ToysPage from './pages/ToysPage';
import KidsPage from './pages/KidsPage';
import WomenPage from './pages/WomenPage';
import MenPage from './pages/MenPage';
import OurStories from './pages/OurStories';
import SuperDealsPage from './pages/SuperDealsPage';
import SellerDashboard from './pages/SellerDashboard';
import Register from './pages/Register';
import AboutUs from './pages/AboutUs';
import MyListingPage from './pages/MyListingPage';
import AddProductPage from './pages/AddProductPage';
import PublicProfile from './pages/PublicProfile';
import PlaceOrder from './pages/PlaceOrderPage';


// Account Manager
import AccountManager from './pages/AccountManager';
import PurchaseForm from './pages/PurchaseForm';

// User profile pages
import ProfilePage from './pages/ProfilePage';
import EditProfilePage from './pages/EditProfilePage';
import MyOrdersPage from './pages/MyOrdersPage';


function AnimatedRoutes() {
    const location = useLocation();

    return (
        <AnimatePresence mode="wait">
            <Routes location={location} key={location.pathname}>
                {/* Homepage */}
                <Route
                    path="/"
                    element={
                        <>
                            <WipeTransition />
                            <HeroBanner />
                            <QuoteSection />
                            <GalleryShowcase />
                            <Footer />
                        </>
                    }
                />

                {/* Categories Page */}
                <Route
                    path="/products"
                    element={
                        <>
                            <WipeTransition />
                            <ListingGrid />
                            <QuoteSection />
                            <GalleryShowcase />
                            <Footer />
                        </>
                    }
                />

                {/* Dynamic Category Listings */}
                <Route path="/category/clothes" element={<><WipeTransition /><ClothesPage /></>} />
                <Route path="/category/accessories" element={<><WipeTransition /><AccessoriesPage /></>} />
                <Route path="/category/toys" element={<><WipeTransition /><ToysPage /></>} />
                <Route path="/category/kids" element={<><WipeTransition /><KidsPage /></>} />
                <Route path="/category/women" element={<><WipeTransition /><WomenPage /></>} />
                <Route path="/category/men" element={<><WipeTransition /><MenPage /></>} />

                {/* Other Pages */}
                <Route path="/our-stories" element={<><WipeTransition /><OurStories /></>} />
                <Route path="/login" element={<><WipeTransition /><Login /></>} />
                <Route path="/profile" element={<><WipeTransition /><ProfilePage /></>} />
                <Route path="/super-deals" element={<><WipeTransition /><SuperDealsPage /></>} />
                <Route path="/dashboard" element={<><WipeTransition /><SellerDashboard /></>} />
                <Route path="/register" element={<><WipeTransition /><Register /></>} />
                <Route path="/about" element={<><WipeTransition /><AboutUs /></>} />
                <Route path="/product/:productId" element={<><WipeTransition /><ProductDetail /></>} />
                <Route path="/cart" element={<><WipeTransition /><CartPage /></>} />
                <Route path="/mylistings" element={<><WipeTransition /><MyListingPage /></>} />
                <Route path="/addproduct" element={<><WipeTransition /><AddProductPage /></>} />
                <Route path="/accountmanager" element={<><WipeTransition /><AccountManager /></>} />
                <Route path="/purchase/:productId" element={<><WipeTransition /><PurchaseForm /></>} />
                <Route path="/user/:profileId" element={<><WipeTransition /><PublicProfile /></>} />
                <Route path="/editprofile" element={<><WipeTransition /><EditProfilePage /></>} />
                <Route path="/myorders" element={<MyOrdersPage />} />
                <Route path="/placeOrder/:productId" element={<PlaceOrder />} />







            </Routes>
        </AnimatePresence>
    );
}

function App() {
    return (
        <UserProvider>
            <WishlistProvider>
                <CartProvider>
                    <Router>
                        <Navbar />
                        <AnimatedRoutes />
                    </Router>
                </CartProvider>
            </WishlistProvider>
        </UserProvider>
    );
}

export default App;