useEffect(() => {
  const stored = JSON.parse(localStorage.getItem('remarket-user'));
  if (stored) {
    setRole(stored.role);
    setName(stored.name);
    setEmail(stored.email);
  }

  const verifySession = async () => {
    try {
      const res = await axios.get('/account');
      setName(res.data.username);
      setEmail(res.data.email);
    } catch (err) {
      if (err.response?.status === 401) {
        logout(); 
      }
    }
  };

  verifySession();
}, []);
