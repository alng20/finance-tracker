import "./css/AddExpenseButton.css";

type AddExpenseButtonProps = {
  onClick: () => void;
  text: string;
};

function AddExpenseButton(props: AddExpenseButtonProps) {
  return (
    <div className="add-expense-button">
      <button onClick={props.onClick} className="add-expense-button_click">
        {props.text}
      </button>
    </div>
  );
}

export default AddExpenseButton;
