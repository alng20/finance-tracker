import { useNavigate } from "react-router-dom";

function RegisterForm() {
  const navigate = useNavigate();
  return (
    <div>
      <p>Registration will be implemented later.</p>
      <button onClick={() => navigate(-1)}>Back</button>
    </div>
  );
}

export default RegisterForm;
