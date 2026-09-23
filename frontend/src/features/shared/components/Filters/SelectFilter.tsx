import { useEffect, useId, useRef, useState } from "react";

import type { FilterData } from "../../../shared/types/FilterData";

const FILTER_OPEN_EVENT = "filter-open";

type SelectFilterProps = {
  label: string;
  options: FilterData[];
  selected: FilterData[];
  onChange: (selected: FilterData[]) => void;

  searchable?: boolean;
  onSearch?: (search: string) => void;
  onClear: () => void;
};

export function SelectFilter({
  label,
  options,
  selected,
  onChange,
  searchable = false,
  onSearch,
  onClear,
}: SelectFilterProps) {
  const [isOpen, setIsOpen] = useState(false);
  const [search, setSearch] = useState("");
  const filterId = useId();
  const filterRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleOutsidePointer = (event: PointerEvent) => {
      if (!filterRef.current?.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    const handleOtherFilterOpen = (event: Event) => {
      const openedFilterId = (event as CustomEvent<string>).detail;

      if (openedFilterId !== filterId) {
        setIsOpen(false);
      }
    };

    document.addEventListener("pointerdown", handleOutsidePointer);
    window.addEventListener(FILTER_OPEN_EVENT, handleOtherFilterOpen);

    return () => {
      document.removeEventListener("pointerdown", handleOutsidePointer);
      window.removeEventListener(FILTER_OPEN_EVENT, handleOtherFilterOpen);
    };
  }, [filterId]);

  const handleToggleOpen = () => {
    setIsOpen((current) => {
      const nextValue = !current;

      if (nextValue) {
        window.dispatchEvent(
          new CustomEvent(FILTER_OPEN_EVENT, { detail: filterId }),
        );
      }

      return nextValue;
    });
  };

  const handleToggle = (option: FilterData) => {
    if (selected.some((elem) => elem.id === option.id)) {
      onChange(selected.filter((elem) => elem.id !== option.id));
    } else {
      onChange([...selected, option]);
    }
  };

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;

    setSearch(value);
    onSearch?.(value);
  };

  const renderOptions = () => {
    const availableOptions = options.filter(
      (option) => !selected.some((elem) => elem.id === option.id),
    );

    if (availableOptions.length === 0 && search.trim()) {
      return <div>No results</div>;
    }

    return availableOptions.map((option) => (
      <label className="multi-select-filter__option" key={option.id}>
        <input
          type="checkbox"
          checked={selected.some((elem) => elem.id === option.id)}
          onChange={() => handleToggle(option)}
        />
        <span>{option.name}</span>
      </label>
    ));
  };

  const renderSelected = () => {
    if (selected.length === 0) {
      return null;
    }

    return (
      <div className="multi-select-filter__selected">
        <span className="multi-select-filter__selected-title">Selected</span>
        {selected.map((option) => (
          <label className="multi-select-filter__option" key={option.id}>
            <input
              type="checkbox"
              checked
              onChange={() => handleToggle(option)}
            />
            <span>{option.name}</span>
          </label>
        ))}
      </div>
    );
  };

  const handleClear = () => {
    setSearch("");
    onSearch?.("");
    onClear();
  };

  const renderDropdown = () => {
    if (!isOpen) {
      return null;
    }

    return (
      <div className="multi-select-filter__dropdown">
        {searchable && (
          <div className="multi-select-filter__search">
            <input
              type="search"
              value={search}
              onChange={handleSearchChange}
              placeholder={`Search ${label.toLowerCase()}...`}
              aria-label={`Search ${label.toLowerCase()}`}
            />
            <button type="button" onClick={handleClear}>
              Clear
            </button>
          </div>
        )}
        {renderSelected()}
        <div className="multi-select-filter__options">{renderOptions()}</div>
        <button
          className="multi-select-filter__clear"
          type="button"
          onClick={handleClear}
        >
          Clear selection
        </button>
      </div>
    );
  };

  return (
    <div className="multi-select-filter" ref={filterRef}>
      <button
        className="multi-select-filter__trigger"
        type="button"
        aria-expanded={isOpen}
        onClick={handleToggleOpen}
      >
        {label}
        <span className="multi-select-filter__summary">
          {selected.length > 0 ? selected.length : "All"}
        </span>
      </button>
      {renderDropdown()}
    </div>
  );
}
