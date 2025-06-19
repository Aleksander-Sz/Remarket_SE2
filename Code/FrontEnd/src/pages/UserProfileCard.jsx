function UserProfileCard({ profile }) {
    return (
        <div className="wishlist-card">
            {profile.photoUrl && (
                <img src={profile.photoUrl} alt="Profile" />
            )}
            <h3>{profile.username}</h3>
            <p>{profile.bio}</p>
        </div>
    );
}
export default UserProfileCard;
