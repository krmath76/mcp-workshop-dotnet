# Implement Monkey Console Application

> Tracking note: GitHub Issues 기능이 저장소에서 비활성화되어 실제 이슈를 생성할 수 없어(HTTP 410) 동일 내용을 Markdown 파일로 기록합니다. Issues 기능을 활성화한 뒤 정식 이슈로 이전할 수 있습니다.

## Goal
Create a new C# console application (e.g., `MyMonkeyApp`) that can:
- List all available monkeys
- Get details for a specific monkey by name
- Pick and display a random monkey (with details)

## Functional Requirements
- Create a `Monkey` model class with at least: `Name`, `ScientificName`, `Region`, `Description`.
- Provide a seed data collection (in-memory list or JSON file) of several monkey entries.
- Implement commands / menu options:
  1. List all monkeys (names only or brief summary)
  2. Show details of a monkey by name (case-insensitive lookup)
  3. Random monkey (choose one at random and display details)
  4. Exit
- Input loop should gracefully handle invalid choices.
- Add ASCII art banner (monkey themed) shown at startup and before returning to the menu.

## Non-Functional Requirements
- Clear separation of concerns: data loading, rendering/output, and user interaction.
- Use async patterns where appropriate (e.g., potential future data source changes).
- Handle edge cases: empty data set, name not found, invalid menu selection.

## Suggested Structure
```
MyMonkeyApp/
  Program.cs                // Entry point, menu loop
  Models/Monkey.cs          // Monkey POCO
  Data/MonkeyData.cs        // Static list or data loading logic
  Services/MonkeyService.cs // Query, random selection, search by name
  Rendering/AsciiArt.cs     // ASCII banner(s)
```

## Sample ASCII Banner (improvable)
```
  __  __            _              
 |  \/  | ___  _ __| | _____ _   _ 
 | |\/| |/ _ \| '__| |/ / _ \ | | |
 | |  | | (_) | |  |   <  __/ |_| |
 |_|  |_|\___/|_|  |_|\_\___|\__, |
                             |___/ 
    (monkey explorer)
```

## Implementation Checklist
- [ ] Create project scaffolding (if not already present)
- [ ] Add `Monkey` model
- [ ] Add seed data (at least 5 monkeys)
- [ ] Implement `MonkeyService` with: GetAll, FindByName, GetRandom
- [ ] Implement ASCII art banner helper
- [ ] Implement interactive menu loop
- [ ] Handle invalid input gracefully
- [ ] Add basic null / empty checks
- [ ] (Optional) Add colorized console output for headers
- [ ] Update root README with brief usage instructions

## Future Enhancements (Optional)
- Load data from external JSON file
- Add filtering by region
- Add search by partial name
- Expose as an MCP tool in the future

## Labels (Intended)
`enhancement`, `good first issue`

## How to Convert to a Real GitHub Issue Later
1. Enable Issues in repository settings: Settings > General > Features > check "Issues".
2. (Optional CLI) `gh repo edit <owner>/<repo> --enable-issues`
3. Create a new issue and copy/paste the contents of this file.
4. Apply labels: enhancement, good first issue.
5. Close or remove this markdown file once the issue is tracked in GitHub.

---
Created on: 2025-09-26
Author: Automated assistant