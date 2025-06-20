import { useEffect, useState } from 'react';
import axios from '../api/axiosInstance';
import './ProfilePage.css';

function EditProfilePage() {
    const [formData, setFormData] = useState({
        bio: '',
        avatar: null,
        username: '',
        email: '',
        newPassword: '',
        confirmNewPassword: '',
    });

    const [loading, setLoading] = useState(true);
    const [user, setUser] = useState(null);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const response = await axios.get('/account');
                setUser(response.data);
                setFormData((prev) => ({
                    ...prev,
                    username: response.data.username,
                    email: response.data.email,
                }));
            } catch (err) {
                if (err.response?.status === 401) {
                    setError('You must be logged in to view this page.');
                } else {
                    setError('Failed to load profile.');
                }
            } finally {
                setLoading(false);
            }
        };

        fetchProfile();
    }, []);

    const handleChange = (e) => {
        const { name, value, files, type } = e.target;
        if (type === 'file') {
            setFormData((prev) => ({ ...prev, [name]: files[0] }));
        } else {
            setFormData((prev) => ({ ...prev, [name]: value }));
        }
    };

    const handleImageUpload = async () => {
        const formDataImg = new FormData();
        formDataImg.append('image', formData.avatar);
        const response = await axios.post('/photo', formDataImg, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });
        return response.data.id;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (formData.newPassword && formData.newPassword !== formData.confirmNewPassword) {
            alert('New passwords do not match.');
            return;
        }

        try {
            let photoId = null;
            if (formData.avatar) {
                photoId = await handleImageUpload();
            }

            const payload = {
                NewDescription: formData.bio,
                NewPhotoId: photoId,
                NewUsername: formData.username,
                NewEmail: formData.email,
                NewPassword: formData.newPassword || null,
            };

            await axios.post(`/changeProfile/${user.id}`, payload);
            alert('Profile updated!');
        } catch (err) {
            console.error('Error updating profile:', err);
            alert('Failed to update profile.');
        }
    };

    return (
        <div className="profile-container">
            <h1>Edit Your Profile</h1>
            {loading ? (
                <p>Loading...</p>
            ) : error ? (
                <p className="error">{error}</p>
            ) : (
                <form className="product-form" onSubmit={handleSubmit}>
                    <label>
                        Username:
                        <input
                            type="text"
                            name="username"
                            value={formData.username}
                            onChange={handleChange}
                        />
                    </label><br />

                    <label>
                        Email:
                        <input
                            type="email"
                            name="email"
                            value={formData.email}
                            onChange={handleChange}
                        />
                    </label><br />

                    <label>
                        New Password:
                        <input
                            type="password"
                            name="newPassword"
                            value={formData.newPassword}
                            onChange={handleChange}
                        />
                    </label><br />

                    <label>
                        Confirm New Password:
                        <input
                            type="password"
                            name="confirmNewPassword"
                            value={formData.confirmNewPassword}
                            onChange={handleChange}
                        />
                    </label><br />

                    <label>
                        Your Bio:
                        <textarea
                            name="bio"
                            value={formData.bio}
                            onChange={handleChange}
                            rows="4"
                        />
                    </label><br />

                    <label>
                        Profile Picture:
                        <input
                            type="file"
                            name="avatar"
                            accept="image/*"
                            onChange={handleChange}
                        />
                    </label><br />

                    <button type="submit">Save</button>
                </form>
            )}
        </div>
    );
}

export default EditProfilePage;
