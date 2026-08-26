type HeaderProps = {
  title: string;
  date: string;
};

function Header(header: HeaderProps) {
  return (
    <header>
      <h1>
        {header.title}, {header.date}
      </h1>
    </header>
  );
}

export default Header;
