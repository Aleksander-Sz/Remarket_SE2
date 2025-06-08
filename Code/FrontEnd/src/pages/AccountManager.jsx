import React, { useEffect, useState } from 'react';
import axios from '../api/axiosInstance';
import { useUser } from '../context/UserContext';
import './AccountManager.css';

function AccountManager() {
    const { role } = useUser();
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchUsers = async () => {
        try {
            setLoading(true);
            console.log('Fetching users...');
            const res = await axios.get('/accounts');
            console.log('Users fetched:', res.data);
            setUsers(res.data);
            setError(null);
        } catch (err) {
            console.error('Failed to fetch users:', err);
            setError('Failed to load users');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        console.log('Current role:', role);
        if (role === 'A') {
            fetchUsers();
        } else {
            setLoading(false);
        }
    }, [role]);

    const handleDelete = async (userId) => {
        console.log('Deleting user with id:', userId);
        if (!window.confirm('Are you sure you want to delete this user?')) {
            return;
        }
        try {
            await axios.delete(`/accounts/${userId}`);
            setUsers(users.filter(u => u.id !== userId));
        } catch (err) {
            console.error('Failed to delete user:', err);
            alert('Failed to delete user.');
        }
    };

    const handlePromote = async (userId) => {
        console.log('Promoting user with id:', userId);
        if (!window.confirm('Are you sure you want to promote this user?')) {
            return;
        }
        try {
            const response = await axios.post(`/changeRole/${userId}`, {
                newRole: 'A'
            });
            setUsers(users.map(u =>
                u.id === userId ? { ...u, role: 'A' } : u
            ));
            alert('User promoted successfully.');
            console.log(response.data.message);
        } catch (err) {
            console.error('Failed to promote user:', err);
            alert('Failed to promote user.');
        }
    };

    if (loading) {
        return <p>Loading user data...</p>;
    }

    if (role != 'A') {
        return <p>⛔ Access denied. Admins only.</p>;
    }

    return (
        <div className="account-manager">
            <h2>Account Manager</h2>
            {error && <p className="error">{error}</p>}
            <table>
                <thead>
                    <tr>
                        <th>Name</th><th>Email</th><th>Role</th><th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {users.length === 0 ? (
                        <tr><td colSpan="4">No users found</td></tr>
                    ) : (
                        users.map(u => (
                            <tr key={u.id}>
                                <td>{u.name || u.username || '—'}</td>
                                <td>{u.email}</td>
                                <td>{u.role}</td>
                                <td>
                                    <button onClick={() => handlePromote(u.id)}>Promote</button>
                                    <button onClick={() => handleDelete(u.id)}>Delete</button>
                                </td>
                            </tr>
                        ))
                    )}
                </tbody>
            </table>
        </div>
    );
}

export default AccountManager;
