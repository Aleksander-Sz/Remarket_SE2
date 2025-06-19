import { useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import axios from '../api/axiosInstance';

function UserProfileView() {
    const { userId } = useParams();
    const [profile, setProfile] = useState(null);

    useEffect(() => {
        axios.get(`/userProfile/${userId}`)
            .then(res => setProfile(res.data))
            .catch(err => console.error('Failed to load user profile', err));
    }, [userId]);

    if (!profile) return <p>Loading profile...</p>;

    return (
        <div className="profile-container">
            <h1>{profile.username}'s Profile</h1>
            <img src={profile.avatarUrl} alt="Avatar" width="200" />
            <p>{profile.description}</p>
        </div>
    );
}

export default UserProfileView;
