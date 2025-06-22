import React, { useEffect, useState } from 'react';
import './ListingGrid.css';
import { motion } from 'framer-motion';
import PaymentModal from '../components/PaymentModal';
import { useWishlist } from '../context/WishlistContext';
import { FaHeart, FaRegHeart } from 'react-icons/fa';
import axios from '../api/axiosInstance';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useCart } from '../context/CartContext';


function ListingGrid() {
    const [items, setItems] = useState([]);
    const [categories, setCategories] = useState([]);
    const [selectedItem, setSelectedItem] = useState(null);
    const { wishlist, toggleWishlist } = useWishlist();
    const navigate = useNavigate();
    const { addToCart } = useCart();

    const [searchParams] = useSearchParams();
    const [page, setPage] = useState(parseInt(searchParams.get('page')) || 1);


    const categoryParam = searchParams.get('category');
    const [filters, setFilters] = useState({
        category: categoryParam ? categoryParam : 'all categories',
        minPrice: parseInt(searchParams.get('minPrice')),
        maxPrice: parseInt(searchParams.get('maxPrice')),
    });

    
    const [hasMore, setHasMore] = useState(true);
    const ITEMS_PER_PAGE = 36;

    const fetchCategories = async () => {
        try {
            const response = await axios.get('/categories');
            setCategories(response.data);
        } catch (error) {
            console.error('Failed to load categories', error);
            setCategories([]);
        }
    };

    const fetchProducts = async (filters, page) => {
        try {
            const response = await axios.get('/products', {
                params: {
                    category: filters.category === 'all categories' ? null : filters.category,
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

    const goToPage = (newPage) => {
        setPage(newPage);

        const url = new URL(window.location);
        url.searchParams.set('page', newPage);
        window.history.pushState({}, '', url);
    };
    function createThrottled(fn, delay) {
        let lastCall = 0;
        let timeout = null;
        let queuedArgs = null;

        const callFn = (args) => {
            lastCall = Date.now();
            fn(...args);
        };

        return (...args) => {
            const now = Date.now();
            const timeSinceLastCall = now - lastCall;

            if (timeSinceLastCall >= delay) {
                // It's been long enough: run immediately
                callFn(args);
            } else {
                // Too soon: queue the latest args
                clearTimeout(timeout);
                queuedArgs = args;

                timeout = setTimeout(() => {
                    callFn(queuedArgs);
                    queuedArgs = null;
                    timeout = null;
                }, delay - timeSinceLastCall);
            }
        };
    }

    const updateFilters = (filters) => {
        //first the local page
        setPage(1);
        fetchProducts(filters, 1);

        // now the url
        const url = new URL(window.location);

        url.searchParams.delete('category');
        url.searchParams.delete('minPrice');
        url.searchParams.delete('maxPrice');

        // Add filters to query params if they have a value
        if (filters.category) {
            url.searchParams.set('category', filters.category);
        }

        if (filters.minPrice) {
            url.searchParams.set('minPrice', filters.minPrice);
        }

        if (filters.maxPrice) {
            url.searchParams.set('maxPrice', filters.maxPrice);
        }

        window.history.pushState({}, '', url);
    };

    const throttledUpdateFilters = React.useMemo(() => createThrottled(updateFilters, 2000), []);

    useEffect(() => {
        fetchCategories();
    }, []);

    useEffect(() => {
        fetchProducts(filters,page);
    }, [page]);

    const handleFilter = (e) => {
        e.preventDefault();
        setPage(1);
        fetchProducts(filters,page);
    };

    return (
        <section className="listing-grid">
            <h2 className="listing-title">Browse Listings</h2>

            <form className="filter-form" onSubmit={handleFilter}>
                <select
                    value={filters.category}
                    onChange={(e) => {
                        const newFilters = { ...filters, category: e.target.value };
                        setFilters(newFilters);
                        throttledUpdateFilters(newFilters);
                    }}
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
                    onChange={(e) => {
                        const newFilters = { ...filters, minPrice: e.target.value };
                        setFilters(newFilters);
                        throttledUpdateFilters(newFilters);
                    }}
                />

                <input
                    type="number"
                    placeholder="Max Price"
                    value={filters.maxPrice}
                    onChange={(e) => {
                        const newFilters = { ...filters, maxPrice: e.target.value };
                        setFilters(newFilters);
                        throttledUpdateFilters(newFilters);
                    }}
                />

                {/*<button type="submit">Filter</button>*/}
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
                <button onClick={() => goToPage(Math.max(page - 1, 1))} disabled={page === 1}>
                    ◀ Previous
                </button>
                <span>Page {page}</span>
                <button onClick={() => goToPage(page + 1)} disabled={!hasMore}>
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
