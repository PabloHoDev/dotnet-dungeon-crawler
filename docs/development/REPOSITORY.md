# Repository

## 1. Overview

This document describes the repository structure, version control strategy, development organization, and baseline configuration of the Dungeon Crawler project.

The repository is designed to support the development of a C# and Unity game while applying professional software engineering practices.

---

# 2. Project Identity

| Property | Value |
|---|---|
| Project Name | Dungeon Crawler |
| Repository Name | dungeon-crawler |
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

```text
dungeon-crawler/
│
├── Assets/
│
├── Packages/
│
├── ProjectSettings/
│
├── Tests/
│
├── docs/
│   ├── product/
│   ├── architecture/
│   │   └── adr/
│   ├── development/
│   ├── testing/
│   └── release/
│
├── README.md
├── LICENSE
└── .gitignore