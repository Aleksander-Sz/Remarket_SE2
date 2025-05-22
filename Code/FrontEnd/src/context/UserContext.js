import { createContext, useContext, useState, useEffect } from 'react';
import axios from '../api/axiosInstance'; 

const UserContext = createContext();

export const UserProvider = ({ children }) => {
  const [role, setRole] = useState(''); // '', 'user', 'seller', 'admin'
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');

  useEffect(() => {
    const stored = JSON.parse(localStorage.getItem('remarket-user'));
    if (stored) {
      setRole(stored.role || '');
      setName(stored.name || '');
      setEmail(stored.email || '');
    }

    // ✅ Confirm session with backend
    const verifySession = async () => {
      try {
        const res = await axios.get('/account');
        setName(res.data.username);
        setEmail(res.data.email);
        // Optional: setRole(res.data.role) if backend sends it
      } catch (err) {
        if (err.response?.status === 401) {
          logout(); // token invalid, clear context
        }
      }
    };

    verifySession();
  }, []);

  const loginAs = (userData) => {
    localStorage.setItem('remarket-user', JSON.stringify(userData));
    setRole(userData.role);
    setName(userData.name);
    setEmail(userData.email);
  };

  const logout = () => {
    localStorage.removeItem('remarket-user');
    localStorage.removeItem('token');
    setRole('');
    setName('');
    setEmail('');
  };

  return (
    <UserContext.Provider value={{ role, name, email, loginAs, logout }}>
      {children}
    </UserContext.Provider>
  );
};

export const useUser = () => useContext(UserContext);
