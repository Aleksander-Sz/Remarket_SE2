import React, { useEffect, useState } from 'react';
import axios from '../api/axiosInstance';
import { useUser } from '../context/UserContext';
import './AccountManager.css';

function AccountManager() {
    const { role } = useUser();
    const [users, setUsers] = useState([]);

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const res = await axios.get('/users');
                setUsers(res.data);
            } catch (err) {
                console.error('Failed to fetch users:', err);
            }
        };

        if (role?.toUpperCase() === 'A') {
            fetchUsers();
        }
    }, [role]);

    if (!role) {
        return <p>Loading user data...</p>;
    }

    if (role.toUpperCase() !== 'A') {
        return <p>⛔ Access denied. Admins only.</p>;
    }

    return (
        <div className="accountmanager">
            <h2>Account Manager</h2>
            <table>
                <thead>
                    <tr>
                        <th>Name</th><th>Email</th><th>Role</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map((u) => (
                        <tr key={u.id}>
                            <td>{u.name || '—'}</td>
                            <td>{u.email}</td>
                            <td>{u.role}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default AccountManager;
