import './Login.css';

function Login() {
  return (
    <div className="login-page">
      <div className="login-box">
        <h2>Login to ReMarket</h2>

        <form>
          <input type="text" placeholder="Phone number or Email" />
          <input type="password" placeholder="Password" />
          <button type="submit">Login</button>
        </form>

        <div className="divider">or</div>

        <div className="login-options">
          <button className="google">Continue with Google</button>
          <button className="github">Continue with GitHub</button>
        </div>
      </div>
    </div>
  );
}

export default Login;
