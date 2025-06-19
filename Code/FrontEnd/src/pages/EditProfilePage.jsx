import { useEffect, useState } from 'react';
import axios from '../api/axiosInstance';
import './ProfilePage.css'; // Mo¿esz u¿yæ tej samej klasy co wczeœniej

function EditProfilePage() {
    const [formData, setFormData] = useState({
        bio: '',
        avatar: null,
    });

    const [loading, setLoading] = useState(true);
    const [user, setUser] = useState(null);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const response = await axios.get('/account');
                setUser(response.data); // { username, email }
            } catch (err) {
                if (err.response && err.response.status === 401) {
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
            setFormData({ ...formData, [name]: files[0] });
        } else {
            setFormData({ ...formData, [name]: value });
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
        try {
            let photoId = null;
            if (formData.avatar) {
                photoId = await handleImageUpload();
            }

            await axios.post(`/changeProfile/${user.id}`, {
                bio: formData.bio,
                photoId,
            });

            alert('Profile updated!');
        } catch (err) {
            console.error('Error updating profile:', err);
            alert('Failed to update profile.');
        }
    };

    return (
        <div className="profile-container">
            <h1>Edit Your Profile</h1>
            {loading ? <p>Loading...</p> : (
                <form className="product-form" onSubmit={handleSubmit}>
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
