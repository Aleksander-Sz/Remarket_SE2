import './Login.css';
import { useUser } from '../context/UserContext';
import { useNavigate } from 'react-router-dom';

function Login() {
  const { loginAs } = useUser();
  const navigate = useNavigate();

  const handleLogin = (role) => {
    loginAs({
      role,
      name: role === 'admin' ? 'Admin Anna' : role === 'seller' ? 'Seller Sam' : 'User Uma',
      email: `${role}@example.com`,
    });
    navigate('/profile');
  };

  return (
    <div className="login-page">
      <div className="login-box">
        <h2>Login to ReMarket</h2>
        <p>Choose a role to simulate:</p>
        <div className="login-options">
          <button onClick={() => handleLogin('user')}>Login as User</button>
          <button onClick={() => handleLogin('seller')}>Login as Seller</button>
          <button onClick={() => handleLogin('admin')}>Login as Admin</button>
        </div>
      </div>
    </div>
  );
}
// Later:
// const response = await axios.post('/auth/login', { email, password });
// loginAs(response.data); // { role, name, email }

export default Login;
