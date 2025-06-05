import React, { useState } from 'react';
import axios from '../api/axiosInstance';
import './UploadItemForm.css';

function UploadItemForm({ onSuccess }) {
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    price: '',
    photos: [],
  });

  const [submitting, setSubmitting] = useState(false);

  const handleChange = (e) => {
    const { name, value, files } = e.target;
    if (name === 'photos') {
      setFormData((prev) => ({ ...prev, photos: files }));
    } else {
      setFormData((prev) => ({ ...prev, [name]: value }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSubmitting(true);
    const data = new FormData();
    data.append('title', formData.title);
    data.append('description', formData.description);
    data.append('price', formData.price);

    for (let i = 0; i < formData.photos.length; i++) {
      data.append('photos', formData.photos[i]);
    }

    try {
      await axios.post('/products', data, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });
      alert('Product uploaded!');
      setFormData({ title: '', description: '', price: '', photos: [] });
      if (onSuccess) onSuccess();
    } catch (err) {
      alert('Upload failed');
      console.error(err);
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form className="upload-form" onSubmit={handleSubmit}>
      <h3>Upload New Project</h3>
      <input
        type="text"
        name="title"
        placeholder="Project Title"
        required
        value={formData.title}
        onChange={handleChange}
      />
      <textarea
        name="description"
        placeholder="Short description"
        required
        value={formData.description}
        onChange={handleChange}
      />
      <input
        type="number"
        name="price"
        placeholder="Price in USD"
        required
        value={formData.price}
        onChange={handleChange}
        min="0"
        step="0.01"
      />
      <input
        type="file"
        name="photos"
        multiple
        accept="image/*"
        onChange={handleChange}
        required
      />
      <button type="submit" disabled={submitting}>
        {submitting ? 'Uploading...' : 'Upload'}
      </button>
    </form>
  );
}

export default UploadItemForm;
