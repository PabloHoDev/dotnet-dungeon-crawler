# ADR-0004 — Apresentação em terminal antes da Unity

**Status:** Aceita
**Data:** 2026-10-02

## Contexto

O núcleo do jogo (ETAPAs 03 a 09) ficou pronto antes de existir um projeto Unity: a Unity ainda não está instalada no ambiente de desenvolvimento. Sem uma camada de apresentação, o jogo só podia ser exercitado pelos testes automatizados, e não jogado.

## Decisão

Criar uma camada de apresentação em terminal, `src/DungeonCrawler.Cli`, que permite jogar o MVP completo com `dotnet run`.

- É uma camada **Presentation** como a Unity será: lê a entrada, chama Application e Domain, e escreve o resultado. Não contém regras de jogo.
- É o **Composition Root** dessa versão: `Program.cs` cria a fonte de aleatoriedade (Infrastructure), a `GameSession` (Application) e a interface.
- Tem como alvo `net10.0` e C# atual, porque nunca será compilada pela Unity. O núcleo continua em `netstandard2.1` / C# 9 (ADR-0002).
- Todo o texto exibido ao jogador (em português) fica nessa camada. O domínio expõe identificadores e dados estruturados, como as entradas do log de combate.

## Alternativas consideradas

- **Esperar a Unity.** Rejeitada: adiaria a validação do gameplay e do balanceamento por tempo indeterminado.
- **Colocar um modo de demonstração nos testes.** Rejeitada: testes verificam regras; não substituem jogar.

## Consequências

- O Golden Path pode ser jogado e validado hoje, sem engine.
- O desenho em camadas é posto à prova: uma segunda apresentação (Unity) deverá reutilizar `GameSession` e o domínio sem alterações.
- A versão em terminal não substitui a ETAPA 11 — Interface, que continua sendo a apresentação em Unity.
- Há uma segunda camada de apresentação para manter enquanto as duas existirem.
