import { useNavigate } from "react-router-dom";

function Home() {
  const navigate = useNavigate();

  // TODO: Update Home page
  return (
    <main>
      <h1>Finance Tracker</h1>

      <p>Track your expenses and understand your spending.</p>

      <div>
        <button type="button" onClick={() => navigate("/login")}>
          Sign in
        </button>

        <button type="button" onClick={() => navigate("/register")}>
          Sign up
        </button>
      </div>
    </main>
  );
}

export default Home;
