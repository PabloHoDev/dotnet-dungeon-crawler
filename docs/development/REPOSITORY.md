# Repository

## 1. Overview

This document describes the repository structure, version control strategy, development organization, and baseline configuration of the Dungeon Crawler project.

The repository is designed to support the development of a C# and Unity game while applying professional software engineering practices.

---

# 2. Project Identity

| Property | Value |
|---|---|
| Project Name | Dungeon Crawler |
| Repository Name | dotnet-dungeon-crawler |
| License | MIT |
| Primary Language | C# |
| Game Engine | Unity |
| Version Control | Git |
| Repository Platform | GitHub |
| Repository Visibility | Public |
| Main Branch | `main` |
| Project Type | Portfolio / Software Engineering Project |

---

# 3. Repository Objectives

The repository has the following objectives:

- Store the complete source code of the project;
- Maintain project documentation;
- Maintain automated tests;
- Track the evolution of the software;
- Record important architectural decisions;
- Support continuous integration and delivery;
- Provide a professional public portfolio artifact;
- Maintain a reproducible development environment.

The repository should make it possible for another developer to understand:

1. What the project is;
2. Why the project exists;
3. How it is structured;
4. How it is developed;
5. How it is tested;
6. Which architectural decisions were made;
7. How the project can be executed.

---

# 4. Repository Structure

The repository follows a high-level organization that separates the game project from its documentation and supporting materials.

The game rules live in a plain .NET solution (`src/` and `tests/`) that builds and tests without Unity. The Unity project sits at the repository root and consumes `src/` as source (see `docs/architecture/ARCHITECTURE.md`).

```text
dotnet-dungeon-crawler/
│
├── src/                              Game core (.NET Standard 2.1, C# 9)
│   ├── DungeonCrawler.Domain/
│   ├── DungeonCrawler.Application/
│   ├── DungeonCrawler.Infrastructure/
│   └── DungeonCrawler.Cli/           Terminal version of the game (.NET 10)
│
├── tests/                            Automated tests (.NET 10, xUnit)
│   ├── DungeonCrawler.Domain.Tests/
│   ├── DungeonCrawler.Application.Tests/
│   └── DungeonCrawler.Architecture.Tests/
│
├── Assets/                           Unity project (pending — created by Unity)
├── Packages/
├── ProjectSettings/
│
├── docs/
│   ├── product/                      Product definition (ETAPA 00)
│   ├── architecture/                 Architecture, roadmap
│   │   └── adr/                      Architecture Decision Records
│   └── development/                  This document
│
├── DungeonCrawler.sln
├── Directory.Build.props             Shared build settings
├── Directory.Packages.props          Central NuGet package versions
├── global.json                       Pinned .NET SDK
├── .editorconfig
├── .gitattributes
├── .gitignore
├── LICENSE
└── README.md
```

`docs/testing/` and `docs/release/` will be created by the stages that produce their content (ETAPA 12 and ETAPA 17).

---

# 5. Development Environment

| Tool | Version | Purpose |
|---|---|---|
| .NET SDK | 10.0 (pinned in `global.json`) | Build and test the core |
| Unity | To be defined (LTS) | Presentation layer |
| Git | Any recent version | Version control |

Build, test and play:

```text
dotnet build
dotnet test
dotnet run --project src/DungeonCrawler.Cli
```

Build output goes to `artifacts/`, never into the project folders.

## 5.1 Unity setup (pending)

The Unity project does not exist yet. To create it:

1. Install Unity Hub from <https://unity.com/download>, sign in and activate a Personal license.
2. In the Hub, install the current Unity 6 LTS editor.
3. Create a new **2D** project in a temporary folder, then move its `Assets/`, `Packages/` and `ProjectSettings/` folders into the repository root.
4. Open the repository root as a project in the Hub and confirm that it opens without errors.

After that, the core in `src/` is exposed to Unity as local packages (ETAPA 02 — FASE 06).

---

# 6. Build Configuration

| File | Scope | Settings |
|---|---|---|
| `Directory.Build.props` | Whole solution | Nullable reference types, warnings as errors, output to `artifacts/` |
| `src/Directory.Build.props` | Core | `netstandard2.1`, C# 9 (Unity compatibility) |
| `tests/Directory.Build.props` | Tests | `net10.0`, xUnit and coverage packages |
| `Directory.Packages.props` | Whole solution | NuGet package versions |

A project file only declares what is specific to it (its project references).

---

# 7. Branching Strategy

| Branch | Purpose |
|---|---|
| `main` | Stable history; always builds and passes the tests |
| `feature/<etapa>-<topic>` | Work in progress for a stage or phase, merged into `main` when validated |

---

# 8. Commit Convention

Commits follow [Conventional Commits](https://www.conventionalcommits.org/):

```text
<type>(<scope>): <summary>
```

| Type | Use |
|---|---|
| `feat` | New behaviour |
| `fix` | Bug fix |
| `test` | Tests only |
| `refactor` | Code change without behaviour change |
| `docs` | Documentation only |
| `build` / `ci` | Build system, dependencies, pipelines |
| `chore` | Maintenance |

The scope is the module or layer, for example `feat(domain): add Health value object`.

---

# 9. Ignored Files

`.gitignore` covers the .NET output (`artifacts/`, `bin/`, `obj/`, test results), the folders Unity generates (`Library/`, `Temp/`, `Logs/`, `UserSettings/`, builds) and IDE/OS files.

Unity `.meta` files **must** be committed.