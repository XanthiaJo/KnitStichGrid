# Repository Guidelines

## Project Structure & Module Organization
`KnitStichGrid` is a small WPF app targeting `net10.0-windows` with a straightforward MVVM layout.

- `Models/`: domain data such as gauge, dimensions, and finished-size records.
- `Services/`: calculation logic and service interfaces.
- `ViewModels/`: UI state and commands, including `MainViewModel` and `PatternCanvasViewModel`.
- `Views/`: WPF XAML views and code-behind, including `PatternCanvasControl`.
- `Commands/`: shared command helpers such as `RelayCommand`.
- `Notes/`: implementation/reference notes such as `Notes/overlay.md`.
- `bin/` and `obj/`: generated build output; do not edit or commit manually.

There is no dedicated test project yet. When adding one, place it in a sibling folder such as `KnitStichGrid.Tests/`.

## Preview Architecture Rules
- `MainViewModel` owns gauge input, finished-size calculation, and app-level orchestration.
- `PatternCanvasViewModel` owns preview/grid state: row and column counts, preview cells, preview sizing, and overlay UI state.
- `PatternCanvasControl` owns preview composition only; keep business and calculation logic out of view code-behind.

## Binding Conventions
- Controls that edit preview, grid, or overlay state should bind through `PatternCanvas.*`.
- Do not reintroduce preview-owned properties directly on `MainViewModel`.
- Keep coordination between `MainViewModel` and `PatternCanvasViewModel` explicit when recalculation or preview sizing needs to stay in sync.

## Overlay Reference
- Use `Notes/overlay.md` as the source of truth for planned overlay, calibration, and measurement behavior.
- The current preview split is structural groundwork; layered image and measurement rendering is still to be implemented.

## Build, Test, and Development Commands
- `dotnet restore`: restore NuGet dependencies for the solution.
- `dotnet build KnitStichGrid.csproj`: compile the WPF app.
- `dotnet run --project KnitStichGrid.csproj`: launch the app locally.
- `dotnet test`: run tests after a test project is added.

Run commands from the repository root. Prefer project-scoped commands when iterating on a single app.

## Coding Style & Naming Conventions
Use standard C# conventions with 4-space indentation and nullable reference types respected. This project already uses:

- file-scoped namespaces
- `PascalCase` for types, properties, and methods
- `_camelCase` for private fields
- concise expression-bodied members where they improve readability

Keep ViewModels focused on presentation logic, keep calculations in `Services/`, and avoid putting business rules in XAML code-behind.

## Testing Guidelines
Add automated tests for service and ViewModel behavior before expanding UI-only features. Prefer `xUnit` for new tests unless the repository adopts another framework later.

Name test files after the class under test, for example `FinishedSizeCalculatorTests.cs`, and write method names that describe behavior, such as `Calculate_ReturnsRoundedSize_ForValidGauge`.

## Commit & Pull Request Guidelines
Recent history is mixed: some commits use Conventional Commit prefixes like `feat(grid): ...`, while others use plain summaries. Prefer short, imperative commit messages and use Conventional Commits.

Pull requests should include:
- a brief summary of the change
- linked issue or task, if one exists
- screenshots or short notes for UI changes
- confirmation that the project builds cleanly and tests pass, if present

### Commit rules (Conventional Commits)
We use Conventional Commits to make releases and changelogs automatic. Follow this format for commit headers:

type(scope?): subject

- type: one of feat, fix, perf, docs, style, refactor, test, chore, build, ci, revert
- scope: optional, a short noun describing the area (e.g., grid, input, ci)
- subject: short imperative summary (max ~72 chars)

Examples:
- feat(grid): add export as PNG
- fix(gauge): correct rounding in calculation
- perf(renderer): reduce memory allocations in preview
- chore(deps): update Newtonsoft.Json to 13.0.2
- docs: update README with release workflow

Breaking changes:
- Indicate breaking API changes either by adding a footer with `BREAKING CHANGE: description` or put a `!` after the type/scope in the header (e.g., `feat!: remove legacy API`).

Body and footers:
- Use the commit body for motivation and contrast, wrap at ~72 chars.
- Reference issues in the footer with `Closes #123` if the PR/commit resolves an issue.

How types map to releases (configured by semantic-release):
- feat -> minor release
- fix, perf -> patch release
- BREAKING CHANGE or type! -> major release
- docs, chore, style, ci, build, test -> no release by default (these are considered maintenance)

Notes:
- Be concise and use the imperative mood ("add", "fix", "update").
- Use scopes to make the changelog clearer (e.g., `feat(canvas): ...`).
- If you want a commit to be recorded but not released (for example, internal chores), use the appropriate type (chore) so semantic-release does not bump the version.