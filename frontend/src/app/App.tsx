import { useState } from "react";

import "./App.css";

import { AuthProvider } from "../features/Authentication/AuthContext";
import LoginForm from "../features/Authentication/components/LoginForm";
import AddExpenseButton from "../features/Expenses/components/AddExpense/AddExpenseButton";
import AddExpenseModal from "../features/Expenses/components/AddExpense/AddExpenseModal";
import Header from "../features/Expenses/components/Header/Header";
import ExpensesTable from "../features/Expenses/components/GetExpenses/ExpensesTable";

function App() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <AuthProvider>
      <div className="app-main">
        <Header title="Finance Tracker" date="21.08.26" />
        <LoginForm />

        <AddExpenseButton onClick={() => setIsOpen(true)} text="Add Expense" />
        {isOpen && <AddExpenseModal onClose={() => setIsOpen(false)} />}
        <ExpensesTable isAddExpenseOpen={isOpen} />
      </div>
    </AuthProvider>
  );
}

export default App;
