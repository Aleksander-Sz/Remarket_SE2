// src/pages/ProfilePage.jsx
import { useEffect, useState } from 'react';
import axios from '../api/axiosInstance';
import './ProfilePage.css';

function ProfilePage() {
    const [categories, setCategories] = useState([]);
    const [loadingCategories, setLoadingCategories] = useState(true);
    const [formData, setFormData] = useState({
        name: '',
        price: '',
        categoryId: '',
        descriptionHeader: '',
        descriptionParagraph: '',
        photograph: null,
    });

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const response = await axios.get('/categories');
                setCategories(response.data); // assuming format: [{ id, name }]
            } catch (err) {
                console.error('Failed to load categories:', err);
            } finally {
                setLoadingCategories(false);
            }
        };

        fetchCategories();
    }, []);

    const handleChange = (e) => {
        const { name, value, type, files } = e.target;
        if (type === 'file') {
            setFormData({ ...formData, [name]: files[0] });
        } else {
            setFormData({ ...formData, [name]: value });
        }
    };

    const handleImageUpload = async () => {
        const formDataImg = new FormData();
        formDataImg.append('image', formData.photograph);

        const response = await axios.post('/photo', formDataImg, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });

        return response.data.id; // the photo ID returned from backend
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            let photoId = null;
            if (formData.photograph) {
                photoId = await handleImageUpload();
            }

            await axios.post('/addListing', {
                title: formData.name,
                header: formData.descriptionHeader,
                paragraph: formData.descriptionParagraph,
                category: parseInt(formData.categoryId),
                price: parseInt(formData.price),
                photoId
            });


            alert('Product submitted successfully!');
        } catch (err) {
            console.error('Error submitting product:', err);
            alert('Failed to submit product.');
        }
    };

    return (
        <div className="profile-container">
            <h1>Add New Product</h1>
            {loadingCategories ? (
                <p>Loading categories...</p>
            ) : (
                    <form className="product-form" onSubmit={handleSubmit}>
                    <label>
                        Product Name:
                        <input
                            type="text"
                            name="name"
                            value={formData.name}
                            onChange={handleChange}
                        />
                    </label><br/>

                    <label>
                        Price:
                        <input
                            type="number"
                            name="price"
                            value={formData.price}
                            onChange={handleChange}
                        />
                    </label><br />

                    <label>
                        Category:
                        <select
                            name="categoryId"
                            value={formData.categoryId}
                            onChange={handleChange}
                        >
                            <option value="">Select a category</option>
                            {categories.map((cat) => (
                                <option key={cat.id} value={cat.id}>
                                    {cat.name}
                                </option>
                            ))}
                        </select>
                    </label><br />

                    <label>
                        Description Header:
                        <input
                            type="text"
                            name="descriptionHeader"
                            value={formData.descriptionHeader}
                            onChange={handleChange}
                        />
                    </label>

                    <label>
                        Description Paragraph:
                        <textarea
                            name="descriptionParagraph"
                            value={formData.descriptionParagraph}
                            onChange={handleChange}
                        />
                    </label><br />

                    <label>
                        Photograph:
                        <input
                            type="file"
                            name="photograph"
                            accept="image/*"
                            onChange={handleChange}
                        />
                    </label><br />

                    <button type="submit">
                        Submit
                    </button>
                </form>
            )}
        </div>
    );
}

export default ProfilePage;
