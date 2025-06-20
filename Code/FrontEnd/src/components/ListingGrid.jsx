import React, { useEffect, useState } from 'react';
import './ListingGrid.css';
import { motion } from 'framer-motion';
import PaymentModal from '../components/PaymentModal';
import { useWishlist } from '../context/WishlistContext';
import { FaHeart, FaRegHeart } from 'react-icons/fa';
import axios from '../api/axiosInstance';
import { useNavigate } from 'react-router-dom';
import { useCart } from '../context/CartContext';

function ListingGrid() {
    const [items, setItems] = useState([]);
    const [categories, setCategories] = useState([]);
    const [selectedItem, setSelectedItem] = useState(null);
    const { wishlist, toggleWishlist } = useWishlist();
    const navigate = useNavigate();
    const { addToCart } = useCart();

    const [filters, setFilters] = useState({
        category: '',
        minPrice: '',
        maxPrice: '',
    });

    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(true);
    const ITEMS_PER_PAGE = 40;

    const fetchCategories = async () => {
        try {
            const response = await axios.get('/categories');
            setCategories(response.data);
        } catch (error) {
            console.error('Failed to load categories', error);
            setCategories([]);
        }
    };

    const fetchProducts = async () => {
        try {
            const response = await axios.get('/products', {
                params: {
                    category: filters.category,
                    min_price: filters.minPrice,
                    max_price: filters.maxPrice,
                    page,
                    limit: ITEMS_PER_PAGE,
                },
            });

            const mapped = response.data.map((item) => ({
                id: item.id,
                title: item.title,
                price: item.price,
                imageIds: item.photoIds,
            }));

            setItems(mapped);
            setHasMore(mapped.length === ITEMS_PER_PAGE);
        } catch (err) {
            console.error('Backend unavailable, using dummy data');
            setItems([]);
        }
    };

    useEffect(() => {
        fetchCategories();
    }, []);

    useEffect(() => {
        fetchProducts();
    }, [page]);

    const handleFilter = (e) => {
        e.preventDefault();
        setPage(1);
        fetchProducts();
    };

    return (
        <section className="listing-grid">
            <h2 className="listing-title">Browse Listings</h2>

            <form className="filter-form" onSubmit={handleFilter}>
                <select
                    value={filters.category}
                    onChange={(e) => setFilters({ ...filters, category: e.target.value })}
                >
                    <option value="">All Categories</option>
                    {categories.map((cat) => (
                        <option key={cat.id} value={cat.name.toLowerCase()}>
                            {cat.name}
                        </option>
                    ))}
                </select>

                <input
                    type="number"
                    placeholder="Min Price"
                    value={filters.minPrice}
                    onChange={(e) => setFilters({ ...filters, minPrice: e.target.value })}
                />

                <input
                    type="number"
                    placeholder="Max Price"
                    value={filters.maxPrice}
                    onChange={(e) => setFilters({ ...filters, maxPrice: e.target.value })}
                />

                <button type="submit">Filter</button>
            </form>

            <div className="grid">
                {items.map((item) => {
                    const isWished = wishlist.find((w) => w.id === item.id);
                    return (
                        <motion.div
                            className="listing-card"
                            key={item.id}
                            initial={{ opacity: 0, y: 20 }}
                            animate={{ opacity: 1, y: 0 }}
                            transition={{ duration: 0.4 }}
                        >
                            <img src={`/api/photo/${item.imageIds[0]}`} alt={item.title} />
                            <h3>{item.title}</h3>
                            <p>${item.price.toFixed(2)}</p>

                            <button onClick={() => navigate(`/placeorder/${item.id}`)}>Buy Now</button>
                            <button onClick={() => navigate(`/product/${item.id}`)}>Learn More</button>

                            {/* <button onClick={() => addToCart(item)}>Add to Cart</button> */}

                            <span className="wishlist-icon" onClick={() => toggleWishlist(item)}>
                                {isWished ? <FaHeart color="red" /> : <FaRegHeart />}
                            </span>
                        </motion.div>

                    );
                })}
            </div>

            <div className="pagination-controls">
                <button onClick={() => setPage((p) => Math.max(p - 1, 1))} disabled={page === 1}>
                    ◀ Previous
                </button>
                <span>Page {page}</span>
                <button onClick={() => setPage((p) => p + 1)} disabled={!hasMore}>
                    Next ▶
                </button>
            </div>

            {selectedItem && (
                <PaymentModal item={selectedItem} onClose={() => setSelectedItem(null)} />
            )}
        </section>
    );
}

export default ListingGrid;
