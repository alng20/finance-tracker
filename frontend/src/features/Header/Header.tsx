import "./Header.css";

import { useAuth } from "../Authentication/hooks/useAuth";
import { formatDate, getTodayDate } from "../../features/shared/common/utils";

type HeaderProps = {
  title: string;
};

function Header(header: HeaderProps) {
  const { user } = useAuth();

  const colors = ["#f49292", "#F4A6E8", "#80c8f1", "#6de59d", "#edc64f"];

  const index = user?.firstName
    ? user.firstName.charCodeAt(0) % colors.length
    : 0;

  const avatarColor = colors[index];

  return (
    <header className="appheader">
      <h1 className="appheader__title">
        {header.title}, {formatDate(getTodayDate())}
      </h1>
      <div className="appheader__user">
        <span className="appheader__user_name">
          {user?.firstName} {user?.lastName}
        </span>

        <div
          className="appheader__avatar"
          style={{ backgroundColor: avatarColor } as React.CSSProperties}
        >
          {user?.firstName?.[0]}
        </div>
      </div>
    </header>
  );
}

export default Header;
