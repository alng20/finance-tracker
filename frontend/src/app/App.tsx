import { useState } from "react";

import Header from "../features/expenses/components/Header/Header";
import AddExpenseButton from "../features/expenses/components/AddExpense/AddExpenseButton";
import AddExpenseModal from "../features/expenses/components/AddExpense/AddExpenseModal";

function App() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div>
      <Header title="Finance Tracker" date="21.08.26" />
      <AddExpenseButton onClick={() => setIsOpen(true)} text="Add Expense" />

      {isOpen && <AddExpenseModal onClose={() => setIsOpen(false)} />}
    </div>
  );
}

export default App;
