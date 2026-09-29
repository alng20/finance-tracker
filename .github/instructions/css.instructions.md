---
applyTo: "frontend/src/**/*.css"
---

# Finance Tracker CSS Instructions

## Styling approach

The frontend uses plain CSS.

Follow the existing CSS architecture and conventions.

Do not introduce a new styling system such as:

- Tailwind CSS
- CSS Modules
- styled-components
- Emotion
- other CSS-in-JS solutions

unless explicitly requested.

Do not replace the existing CSS approach as part of an unrelated feature or bug fix.

## File location

Component-specific styles should be colocated with the component.

Follow the existing project convention:

```text
components/
└── ComponentName/
    ├── ComponentName.tsx
    └── css/
        └── ComponentName.css
```

For example:

```text
frontend/src/features/Expenses/components/GetExpense/
├── GetExpense.tsx
└── css/
    └── GetExpense.css
```

Keep styles close to the component or feature that owns them.

Do not move component-specific styles into global CSS.

## Global styles

Use global styles only for genuinely application-wide concerns.

Global styles may include:

- CSS resets;
- base document styles;
- typography defaults;
- application-wide variables;
- shared global behavior.

Before adding a global selector, verify that the style really needs to affect multiple independent features.

Avoid adding feature-specific styles to `index.css` or other global stylesheets.

## Class naming

Follow the naming conventions already used by the surrounding component and feature.

Class names should clearly communicate their ownership and purpose.

Prefer component-specific names over generic names such as:

```text
.container
.wrapper
.item
.title
.button
```

when those names could create ambiguity or collisions.

Do not rename existing classes unless the task requires it.

## Reuse

Before creating a new CSS rule:

1. inspect the styles of the current component;
2. inspect similar components;
3. check whether an existing class or pattern can be reused;
4. add a new rule only when existing styles are insufficient.

Avoid duplicating identical styles across multiple CSS files.

If a style is genuinely shared by multiple unrelated features, consider whether it belongs in an existing shared/global stylesheet instead of duplicating it.

Do not create shared abstractions for styles used only by one component.

## Layout

Prefer modern CSS layout mechanisms:

- Flexbox;
- CSS Grid.

Use the layout mechanism that matches the structure of the content.

Do not use absolute positioning for primary page or component layout when Flexbox or Grid is appropriate.

Use absolute/fixed positioning when it is appropriate for the UI behavior, such as:

- overlays;
- positioned controls;
- badges;
- modals;
- dropdowns.

## Responsive design

Follow the existing responsive design patterns in the project.

Before adding a media query:

1. inspect existing breakpoints;
2. check how similar components handle the same viewport size;
3. make the smallest change required.

Do not introduce arbitrary breakpoints without a concrete layout requirement.

When modifying a responsive component, verify:

- desktop layout;
- narrow viewport layout;
- text wrapping;
- form controls;
- tables or horizontally scrollable content;
- interactive elements.

Do not redesign unrelated responsive layouts.

## CSS Grid

Use CSS Grid when the component has a two-dimensional layout or when column alignment is important.

For example, form fields or tabular layouts may use explicit grid columns.

Prefer readable grid definitions that communicate the intended structure.

Avoid unnecessarily complex grid configurations.

## Flexbox

Use Flexbox for one-dimensional layouts such as:

- rows of controls;
- navigation;
- button groups;
- vertically stacked content;
- alignment of icons and text.

Avoid adding unnecessary wrappers solely to make Flexbox work when the existing DOM structure is sufficient.

## Spacing

Follow the spacing patterns already used by the project.

Prefer consistent spacing between related elements.

Avoid arbitrary one-off values when an existing spacing pattern already provides the required result.

Do not perform broad spacing normalization as part of an unrelated task.

## Typography

Follow existing typography styles and hierarchy.

Do not introduce a new font family, font scale, or typography system for an individual component unless explicitly required.

Prefer existing global typography rules when they provide the required behavior.

## Colors and visual states

Reuse existing colors and visual conventions where possible.

Do not introduce arbitrary new colors when an existing project color already represents the required state.

Interactive states should remain visually distinguishable.

Consider states such as:

- hover;
- focus;
- active;
- disabled;
- validation error;
- success;
- loading.

Do not rely on color alone to communicate important information.

## Accessibility

Preserve visible keyboard focus states.

Do not remove `outline` or other focus indicators unless an accessible replacement is provided.

Ensure that interactive elements remain usable at supported viewport sizes.

Do not use CSS to hide content that is required for accessibility.

Avoid using pseudo-elements as the only way to communicate essential information.

## Specificity

Keep selector specificity as low as practical.

Prefer class selectors over deeply nested selectors.

Avoid unnecessarily deep selectors such as:

```css
.page .content .form .field .label {
}
```

Prefer component-scoped class selectors when possible.

Avoid `!important`.

Use `!important` only when there is a specific, justified reason and existing styles cannot reasonably be changed.

## Existing styles

When modifying an existing component:

1. read the complete relevant CSS file;
2. understand its current layout and responsive behavior;
3. modify the existing rule when appropriate;
4. avoid creating a second rule that conflicts with the first.

Do not leave obsolete or conflicting CSS behind after changing a component.

## CSS changes

Keep CSS changes scoped to the requested component or feature.

Do not perform unrelated:

- CSS cleanup;
- class renaming;
- formatting changes;
- selector restructuring;
- responsive redesign;
- global style changes.

Prefer the smallest coherent CSS change that completely implements the requested behavior.
