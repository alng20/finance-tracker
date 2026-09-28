import "./css/RegisterPage.css";

import RegisterForm from "../components/RegisterForm";

function RegisterPage() {
  return (
    <div className="register-page">
      <div className="register-page__intro">
        <p className="register-page__eyebrow">Finance Tracker</p>
        <h1>Create your account</h1>
        <p className="register-page__subtitle">
          Track expenses, shops and reports in one place.
        </p>
      </div>
      <RegisterForm />
    </div>
  );
}

export default RegisterPage;
