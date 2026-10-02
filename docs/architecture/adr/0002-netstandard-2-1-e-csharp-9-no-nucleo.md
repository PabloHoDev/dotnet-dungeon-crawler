# ADR-0002 — .NET Standard 2.1 e C# 9 no núcleo

**Status:** Aceita
**Data:** 2026-10-02

## Contexto

O mesmo código de `src/` é compilado pelo `dotnet` e pela Unity (ADR-0001). A Unity compila contra a API do .NET Standard 2.1 e suporta C# 9.

## Decisão

- Projetos em `src/`: `TargetFramework = netstandard2.1` e `LangVersion = 9.0`, definidos em `src/Directory.Build.props`.
- Projetos em `tests/`: `net10.0`, sem restrição de linguagem, pois nunca são compilados pela Unity.
- No núcleo não são usados `record` nem propriedades `init`: ambos dependem do tipo `IsExternalInit`, que não existe no .NET Standard 2.1. Value objects são classes imutáveis derivadas de `ValueObject`.
- A saída de build vai para `/artifacts` (`UseArtifactsOutput`), para que não existam `bin/` e `obj/` dentro das pastas que a Unity irá importar.

## Consequências

- Um recurso de linguagem mais novo usado por engano em `src/` quebra o `dotnet build`, e não só a Unity.
- O código do núcleo é um pouco mais verboso (namespaces em bloco, construtores explícitos).
- Se a versão de C# suportada pela Unity adotada for maior, o `LangVersion` pode ser elevado em um único lugar.
