import "./css/ReportBarList.css";

export type ReportBarListItem = {
  key: string;
  label: string;
  amount: number;
  onClick?: () => void;
};

export type ReportBarListProps = {
  items: ReportBarListItem[];
  amountFormatter: (amount: number) => string;
  emptyMessage: string;
};

function ReportBarList({
  items,
  amountFormatter,
  emptyMessage,
}: ReportBarListProps) {
  if (items.length === 0) {
    return <p className="report-bar-list__empty">{emptyMessage}</p>;
  }

  const maxAmount = Math.max(...items.map((item) => item.amount), 0);

  return (
    <div className="report-bar-list">
      {items.map((item) => {
        const barWidth = maxAmount > 0 ? (item.amount / maxAmount) * 100 : 0;

        const content = (
          <>
            <span className="report-bar-list__label">{item.label}</span>
            <div className="report-bar-list__bar-track">
              <div
                className="report-bar-list__bar"
                style={{ width: `${barWidth}%` }}
              />
            </div>
            <span className="report-bar-list__amount">
              {amountFormatter(item.amount)}
            </span>
          </>
        );

        if (item.onClick) {
          return (
            <button
              type="button"
              className="report-bar-list__row report-bar-list__row--clickable"
              key={item.key}
              onClick={item.onClick}
            >
              {content}
            </button>
          );
        }

        return (
          <div className="report-bar-list__row" key={item.key}>
            {content}
          </div>
        );
      })}
    </div>
  );
}

export default ReportBarList;
