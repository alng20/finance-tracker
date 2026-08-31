import { useAuth } from "../../../Authentication/hooks/useAuth";

type HeaderProps = {
  title: string;
  date: string;
};

function Header(header: HeaderProps) {
  const { isAuthenticated, user } = useAuth();

  return (
    <header className="app-header">
      <h1>
        {header.title}, {header.date}
      </h1>
      {isAuthenticated && (
        <h2>
          Hello, {user?.firstName} {user?.lastName}!!!
        </h2>
      )}
    </header>
  );
}

export default Header;
