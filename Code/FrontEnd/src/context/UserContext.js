import axios from '../api/axiosInstance';
import { createContext, useContext, useState, useEffect } from 'react';

const UserContext = createContext();

export const UserProvider = ({ children }) => {
  const [role, setRole] = useState(''); 
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');

  useEffect(() => {
    const stored = JSON.parse(localStorage.getItem('remarket-user'));
    if (stored) {
        setRole(stored.role ?? '');
        setName(stored.name ?? '');
        setEmail(stored.email ?? '');

    }
    const verifySession = async() =>{
      try{
        const res = await axios.get('/account');
        setRole(res.data.role);
        setName(res.userData.name);
        setEmail(res.userData.email);

      }
      catch(err){
        if(err.response?.useState == 401){
          logout();
        }
      }
    }
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
