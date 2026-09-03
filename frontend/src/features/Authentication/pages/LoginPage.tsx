import "../components/css/LoginPage.css";
import LoginForm from "../components/LoginForm";

function LoginPage() {
  return (
    <div className="login-page">
      <div className="login-page__intro">
        <p className="login-page__eyebrow">Finance Tracker</p>
        <h1>Control and analyze your expenses.</h1>
      </div>
      <LoginForm />
    </div>
  );
}

export default LoginPage;
