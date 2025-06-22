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
        photograph: [],
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
            const fileArray = Array.from(files).slice(0, 5);
            setFormData({ ...formData, [name]: fileArray });
        } else {
            setFormData({ ...formData, [name]: value });
        }
    };

    const handleImageUpload = async () => {
        const uploadedIds = [];
        for (const file of formData.photograph) {
            const formDataImg = new FormData();
            formDataImg.append('image', file);

            const response = await axios.post('/photo', formDataImg, {
                headers: { 'Content-Type': 'multipart/form-data' }
            });

            uploadedIds.push(response.data.id);
        }

        return uploadedIds; // the photo IDs returned from backend
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            let photoIds = [];
            if (formData.photograph.length > 0) {
                photoIds = await handleImageUpload();
            }

            await axios.post('/addListing', {
                title: formData.name,
                header: formData.descriptionHeader,
                paragraph: formData.descriptionParagraph,
                category: parseInt(formData.categoryId),
                price: parseInt(formData.price),
                photoIds
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
                        Photographs:
                        <input
                            type="file"
                            name="photograph"
                            accept="image/*"
                            multiple
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
