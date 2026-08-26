import "./css/AddExpenseButton.css";

type AddExpenseButtonProps = {
  onClick: () => void;
  text: string;
};

function AddExpenseButton(props: AddExpenseButtonProps) {
  return (
    <button onClick={props.onClick} className="add-expense-button">
      {props.text}
    </button>
  );
}

export default AddExpenseButton;
