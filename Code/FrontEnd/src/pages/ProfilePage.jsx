// src/pages/ProfilePage.jsx
import React from 'react';
import './ProfilePage.css';
import { useUser } from '../context/UserContext';
import { useWishlist } from '../context/WishlistContext';

function ProfilePage() {
  const { role, name, email } = useUser();
  const { wishlist } = useWishlist();

  const dummyAdminData = {
    users: [
      { id: 1, name: 'User One', email: 'user1@example.com' },
      { id: 2, name: 'User Two', email: 'user2@example.com' },
    ],
    listings: [
      { id: 101, title: 'Old Chair', seller: 'User One' },
      { id: 102, title: 'Classic Clock', seller: 'User Two' },
    ],
  };

  const dummySellerListings = [
    'Vintage Bag',
    'Faded Jeans',
    'Antique Watch',
  ];
  // Later:
// useEffect(() => {
//   axios.get('/profile').then(res => {
//     update context with res.data
//   });
// }, []);


  return (
    <div className="profile-container">
      <h1>Welcome, {name}</h1>
      <p>Email: {email}</p>

      {/* USER SECTION */}
      {role === 'user' && (
        <>
          <h2>Your Wishlist</h2>
          {wishlist.length === 0 ? (
            <p>You have no items in your wishlist.</p>
          ) : (
            <ul>
              {wishlist.map((item) => (
                <li key={item.id}>{item.title}</li>
              ))}
            </ul>
          )}
        </>
      )}

      {/* SELLER SECTION */}
      {role === 'seller' && (
        <>
          <h2>Your Store: {name}'s Shop</h2>
          <h3>Your Listings</h3>
          <ul>
            {dummySellerListings.map((item, index) => (
              <li key={index}>{item}</li>
            ))}
          </ul>
        </>
      )}

      {/* ADMIN SECTION */}
      {role === 'admin' && (
        <>
          <h2>Admin Panel</h2>

          <h3>Manage Users</h3>
          <ul className="admin-list">
            {dummyAdminData.users.map((user) => (
              <li key={user.id}>
                <span>{user.name} ({user.email})</span>
                <button
                  className="ban-btn"
                  onClick={() => alert(`User ${user.name} banned (simulated)`)}
                >
                  Ban
                </button>
              </li>
            ))}
          </ul>

          <h3>Manage Listings</h3>
          <ul className="admin-list">
            {dummyAdminData.listings.map((listing) => (
              <li key={listing.id}>
                <span>{listing.title} by {listing.seller}</span>
                <button
                  className="delete-btn"
                  onClick={() => alert(`Listing ${listing.title} removed (simulated)`)}
                >
                  Delete
                </button>
              </li>
            ))}
          </ul>
        </>
      )}
    </div>
  );
}

export default ProfilePage;
