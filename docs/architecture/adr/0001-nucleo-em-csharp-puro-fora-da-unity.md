# ADR-0001 — Núcleo em C# puro, fora da Unity

**Status:** Aceita
**Data:** 2026-10-02

## Contexto

Os requisitos RNF-003, RNF-004 e RNF-024 exigem que as regras do jogo não dependam da Unity e possam ser testadas sem abrir o editor. O risco "Unity Coupling" está listado em `MVP.md` §39.

## Decisão

As regras ficam em uma solução .NET comum (`DungeonCrawler.sln`), em `src/`, dividida em três projetos: `Domain`, `Application` e `Infrastructure`. Os testes ficam em `tests/` e rodam com `dotnet test`.

O projeto Unity fica na raiz do repositório e consome `src/` como código-fonte, por meio de pacotes locais com Assembly Definitions sem referência à engine.

A regra de dependência entre as camadas é verificada por testes automatizados (`DungeonCrawler.Architecture.Tests`).

## Alternativas consideradas

- **Colocar o domínio dentro de `Assets/` e testar com o Unity Test Framework.** Rejeitada: os testes passariam a exigir o editor, e nada impediria um `using UnityEngine` no domínio.
- **Compilar o núcleo em DLL e copiar para `Assets/Plugins`.** Rejeitada: cria um passo de cópia manual e dificulta depurar o núcleo dentro da Unity.

## Consequências

- O núcleo pode ser desenvolvido e validado antes de existir qualquer cena.
- O CI do núcleo não precisa de licença nem de imagem da Unity.
- O código de `src/` fica limitado ao que a Unity compila (ver ADR-0002).
- A integração com a Unity ainda precisa ser validada na prática quando o projeto Unity for criado.
