import { createContext, useContext, useState, useEffect } from 'react';

const UserContext = createContext();

export const UserProvider = ({ children }) => {
  const [role, setRole] = useState(''); // '', 'user', 'seller', 'admin'
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');

  useEffect(() => {
    const stored = JSON.parse(localStorage.getItem('remarket-user'));
    if (stored) {
      setRole(stored.role);
      setName(stored.name);
      setEmail(stored.email);
    }
  }, []);

  const loginAs = (userData) => {
    localStorage.setItem('remarket-user', JSON.stringify(userData));
    setRole(userData.role);
    setName(userData.name);
    setEmail(userData.email);
  };

  const logout = () => {
    localStorage.removeItem('remarket-user');
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
