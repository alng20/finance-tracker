import { useState } from "react";

import { AuthProvider } from "../features/Authentication/AuthContext";
import LoginForm from "../features/Authentication/components/LoginForm";
import AddExpenseButton from "../features/Expenses/components/AddExpense/AddExpenseButton";
import AddExpenseModal from "../features/Expenses/components/AddExpense/AddExpenseModal";
import Header from "../features/Expenses/components/Header/Header";

function App() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <AuthProvider>
      <div>
        <Header title="Finance Tracker" date="21.08.26" />
        <LoginForm />

        <AddExpenseButton onClick={() => setIsOpen(true)} text="Add Expense" />
        {isOpen && <AddExpenseModal onClose={() => setIsOpen(false)} />}
      </div>
    </AuthProvider>
  );
}

export default App;
