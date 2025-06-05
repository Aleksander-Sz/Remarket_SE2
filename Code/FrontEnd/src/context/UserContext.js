import axios from '../api/axiosInstance';
import { createContext, useContext, useState, useEffect } from 'react';

const UserContext = createContext();

export const UserProvider = ({ children }) => {
  const [id, setId] = useState(null);
  const [role, setRole] = useState(''); 
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');

  useEffect(() => {
    const stored = JSON.parse(localStorage.getItem('remarket-user'));
    if (stored) {
        //setRole(stored.role ?? '');
        //setName(stored.name ?? '');
        //setEmail(stored.email ?? '');
      setId(stored.id || null);
      setRole(stored.role || '');
      setName(stored.name || '');
      setEmail(stored.email || '');
    }

    const verifySession = async() =>{
      try{
        const res = await axios.get('/account');
        //setRole(res.data.role);
        //setName(res.userData.name);
        //setEmail(res.userData.email);
          setId(res.data.id);
          setRole(res.data.role);
          setName(res.data.name);
          setEmail(res.data.email);

      }
      catch(err){
        if(err.response?.status == 401){
          logout();
        }
      }
    }
    verifySession();
  }, []);

  const loginAs = (userData) => {
    localStorage.setItem('remarket-user', JSON.stringify(userData));
    setId(userData.id);  
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
    <UserContext.Provider value={{ id, role, name, email, loginAs, logout }}>
      {children}
    </UserContext.Provider>
  );
};

export const useUser = () => useContext(UserContext);
