# Architecture

**Projeto:** Dungeon Crawler
**Documento:** `docs/architecture/ARCHITECTURE.md`
**Etapa:** ETAPA 02 — Arquitetura
**Versão:** 1.1
**Status:** 🟡 Aguardando Checking

---

# 1. Objetivo

Este documento define a arquitetura do Dungeon Crawler: as camadas, o que cada uma pode conter, em que direção as dependências apontam e como o núcleo em C# se conecta à Unity.

Ele atende aos requisitos RNF-003, RNF-004, RNF-011, RNF-012, RNF-019, RNF-020, RNF-021 e RNF-024 de `docs/product/REQUIREMENTS.md`.

As decisões que originaram este desenho estão registradas em `docs/architecture/adr/`.

---

# 2. Visão Geral

O jogo é dividido em um **núcleo em C# puro**, que contém todas as regras, e uma **camada de apresentação em Unity**, que apenas exibe o estado e coleta a entrada do jogador.

```text
┌──────────────────────────────────────────────────────────────┐
│ PRESENTATION — Unity                                         │
│ Cenas, MonoBehaviours, UI, input, áudio, Composition Root    │
└───────────────┬──────────────────────────────┬───────────────┘
                │                              │
                ▼                              ▼
┌───────────────────────────────┐   ┌─────────────────────────┐
│ APPLICATION                   │◄──│ INFRASTRUCTURE          │
│ Casos de uso e portas         │   │ Aleatoriedade,          │
│                               │   │ persistência, logging   │
└───────────────┬───────────────┘   └────────────┬────────────┘
                │                                │
                ▼                                ▼
┌──────────────────────────────────────────────────────────────┐
│ DOMAIN                                                       │
│ Regras do jogo: personagens, combate, inimigos, dungeon,     │
│ inventário, equipamentos, progressão, recompensas            │
└──────────────────────────────────────────────────────────────┘
```

O núcleo compila e é testado com `dotnet build` / `dotnet test`, sem abrir a Unity.

---

# 3. Camadas

| Camada | Projeto | Contém | Não contém |
|---|---|---|---|
| Domain | `src/DungeonCrawler.Domain` | Entidades, value objects, agregados, serviços de domínio, eventos de domínio, regras e fórmulas | Qualquer I/O, `System.Random`, relógio, Unity, pacotes NuGet |
| Application | `src/DungeonCrawler.Application` | Casos de uso (criar personagem, iniciar expedição, entrar em sala, executar turno) e o estado da sessão | Regras de jogo, detalhes de persistência, Unity |
| Infrastructure | `src/DungeonCrawler.Infrastructure` | Implementações das portas: aleatoriedade, persistência, logging | Regras de jogo, Unity |
| Presentation (terminal) | `src/DungeonCrawler.Cli` | Menus, leitura de entrada, textos em português, Composition Root | Regras de jogo |
| Presentation (Unity) | Projeto Unity na raiz (`Assets/`) — pendente | Cenas, UI, input, áudio, animação, Composition Root | Regras de jogo |

Hoje o jogo é jogável pela apresentação em terminal (ADR-0004). A apresentação em Unity depende da instalação da engine.

---

# 4. Regra de Dependência

As dependências apontam sempre para dentro:

```text
Presentation ──► Infrastructure ──► Application ──► Domain
      │                                  ▲             ▲
      └──────────────────────────────────┴─────────────┘
```

| Camada | Pode referenciar |
|---|---|
| Domain | Apenas a biblioteca base do .NET (`netstandard`) |
| Application | Domain |
| Infrastructure | Application, Domain |
| Presentation | Application, Domain, Infrastructure (somente no Composition Root) |

Nenhum projeto em `src/` pode referenciar `UnityEngine` ou `UnityEditor`.

Essa regra é verificada automaticamente por `tests/DungeonCrawler.Architecture.Tests`, que falha o build de testes se um projeto declarar ou usar uma dependência proibida.

---

# 5. Domain

## 5.1 Organização

```text
DungeonCrawler.Domain
│
├── Abstractions      Blocos de construção
├── SharedKernel      Level, Probability, Health
│
├── Characters        Character, CharacterClass, Attributes, Resource
├── Abilities         AbilityDefinition, efeitos (dano, cura), LearnedAbility
├── Combat            CombatEncounter, AttackResolver, PlayerAction, log de combate
├── Enemies           Enemy, EnemyDefinition, EnemyModifier, Bestiary, Behaviors
├── Items             Equipment, Inventory, Loadout, LootGenerator
├── Dungeon           DungeonDefinition, DungeonGenerator, Expedition
├── Progression       Balance e fórmulas
└── Rewards           Reward
```

| Agregado | Responsabilidade |
|---|---|
| `Character` | Progressão permanente: nível, XP, atributos, habilidades, inventário, equipamento |
| `Expedition` | Progressão temporária: andar, trecho, combate atual. Encerra em vitória ou morte |

`CombatEncounter` conduz a rodada; `Expedition` aplica a consequência do combate (recompensa e avanço, ou fim da expedição).

Cada módulo é uma pasta/namespace dentro do mesmo projeto. Separar módulos em projetos distintos só será considerado se surgir uma necessidade concreta.

## 5.2 Blocos de construção (`Abstractions`)

| Tipo | Papel |
|---|---|
| `ValueObject` | Objeto imutável, sem identidade, comparado pelos seus componentes |
| `Entity<TId>` | Objeto comparado pela identidade; expõe `CheckRule` |
| `AggregateRoot<TId>` | Raiz de agregado; registra os eventos de domínio produzidos |
| `IDomainEvent` | Fato relevante que aconteceu no domínio |
| `IBusinessRule` | Regra de jogo dependente de estado, nomeada e testável |
| `DomainException` / `BusinessRuleViolationException` | Falhas previsíveis do domínio (RNF-014) |
| `Guard` | Validação de argumentos |
| `IRandomSource` | Fonte de aleatoriedade injetada (RNF-011) |

Serviços de domínio não têm tipo base: são classes sem estado, usadas quando uma regra envolve mais de um agregado (por exemplo, resolver o dano entre atacante e alvo).

## 5.3 Shared Kernel

| Tipo | Invariante |
|---|---|
| `Level` | Sempre entre 1 e 20 (RF-004) |
| `Probability` | Sempre entre 0 e 1; `Clamp` e `CappedAt` implementam o Easter Egg de limites |
| `Health` | `0 ≤ Current ≤ Maximum` e `Maximum ≥ 1` |

Value objects específicos de um módulo (atributos, XP, raridade, número do andar) entram na etapa do respectivo módulo.

## 5.4 Tratamento de estados inválidos

O domínio distingue dois tipos de falha:

| Situação | Mecanismo | Exemplo |
|---|---|---|
| Argumento inválido (erro de programação) | `ArgumentException` e derivadas, via `Guard` | `Level.Of(21)`, `Health.Full(0)` |
| Operação não permitida no estado atual (regra de jogo) | `IBusinessRule` + `BusinessRuleViolationException`, ou `DomainException` | Inventário cheio, agir estando derrotado, passar do nível máximo |

## 5.5 Aleatoriedade e determinismo

Nenhuma regra do domínio usa `System.Random` diretamente. Tudo que é probabilístico (crítico, loot, encontros, variação da dungeon) recebe um `IRandomSource`.

- Em produção: `SystemRandomSource` (Infrastructure), com seed opcional para reproduzir uma expedição.
- Em testes: um dublê que devolve uma sequência roteirizada de valores.

## 5.6 Fórmulas

As fórmulas do jogo ficam centralizadas no módulo `Progression`, como funções puras, conforme `docs/product/PROGRESSION.md` §43. As entidades chamam as fórmulas; não as reimplementam.

| Easter Egg matemático | Classe |
|---|---|
| Curva de XP — `XP(n) = 50 × n × (n + 1)` | `ExperienceCurve` |
| Atributos derivados — `Base + Atributo × Scaling + Nível × Growth` | `StatFormulas` |
| Dano — `Base + Atributo × Scaling + Rank × Growth + Equipamento` | `DamageFormula` |
| Probabilidade de loot — pesos 60 / 25 / 10 / 4 / 1, com influência limitada de Luck | `LootProbability` |
| Limites — crítico, evasão e influência de Luck com teto | `StatFormulas`, `LootProbability`, `Probability` |
| Escalonamento de inimigos — `Base × Nível × Andar` | `EnemyScaling` |

Todos os números de balanceamento estão em `Progression/Balance.cs` (RNF-013).

## 5.7 Composição sobre herança

`Character` e `Enemy` são montados por composição, sem subclasses por tipo de personagem ou inimigo:

- uma classe jogável é um `CharacterClass` (dados) plugado no `Character`;
- uma habilidade é um `AbilityDefinition` com um `AbilityEffect` (dano ou cura);
- um inimigo é um `EnemyDefinition` com um `IEnemyBehavior` (padrão Strategy): `AggressiveBehavior`, `SkirmisherBehavior`, `TankBehavior`, `BossBehavior`;
- um Elite é um inimigo comum com um `EnemyModifier`.

Decisões registradas em `CHARACTERS.md` §49 e `ENEMIES.md` §52.

## 5.8 Modelo híbrido de dungeon

`DungeonDefinition` é a parte fixa (andares, sequência de trechos, pools de inimigos, mini-chefe e chefe). `DungeonGenerator` decide, a cada expedição, qual inimigo ocupa cada sala. `DungeonLayout` valida as regras de geração: sempre existe caminho até o chefe, e o chefe é a única sala do último trecho.

Cada andar é uma sequência de trechos; em cada trecho o jogador escolhe uma entre uma ou duas salas. Esse modelo garante, por construção, que não existem salas inacessíveis nem loops.

---

# 6. Application

`GameSession` reúne os casos de uso de uma sessão de jogo — criar personagem, iniciar expedição, entrar em sala, executar turno — e guarda o estado da sessão em memória (personagem e expedição atual). Ela entrega ao domínio a fonte de aleatoriedade, de modo que a apresentação não lida com isso.

Enquanto o MVP não tiver persistência, um único serviço de aplicação é suficiente. Quando Save/Load entrar (ETAPA 10), as portas de repositório serão declaradas em Application e implementadas em Infrastructure.

A única abstração de infraestrutura declarada no Domain é `IRandomSource`, porque as próprias regras de domínio dependem dela.

Operações que envolvem apenas o personagem (distribuir atributos, evoluir habilidade, equipar item) são chamadas pela apresentação diretamente no agregado `Character`.

---

# 7. Infrastructure

Implementa as portas. Hoje contém `SystemRandomSource`. Persistência (Save/Load) está classificada como FUTURE no MVP e será tratada na ETAPA 10.

---

# 8. Presentation

## 8.1 Terminal

`src/DungeonCrawler.Cli` é a apresentação que existe hoje (ADR-0004). Executa com:

```text
dotnet run --project src/DungeonCrawler.Cli
```

| Arquivo | Papel |
|---|---|
| `Program.cs` | Composition Root: cria `SystemRandomSource`, `GameSession` e a interface |
| `GameApp.cs` | Fluxo das telas: hub, expedição, combate, personagem, inventário |
| `ConsoleUi.cs` | Leitura e validação de entrada |
| `Texts.cs` | Todo o texto em português; traduz identificadores e dados do domínio |

A opção `--seed <n>` torna a sessão reproduzível.

## 8.2 Unity

O projeto Unity fica na raiz do repositório (`Assets/`, `Packages/`, `ProjectSettings/`), conforme `docs/development/REPOSITORY.md`.

A Unity consome o código de `src/` como **código-fonte**, por meio de pacotes locais (UPM) com Assembly Definitions marcadas como `noEngineReferences`. Assim existe uma única cópia das regras, compilada tanto pelo `dotnet` quanto pela Unity.

Por isso, todo o código em `src/`:

- tem como alvo **.NET Standard 2.1**;
- usa **C# 9** (sem `namespace` com escopo de arquivo, `record struct`, `required`, `global using`, nem `record`/`init`, que exigem tipos ausentes no .NET Standard 2.1);
- não gera `bin/` nem `obj/` dentro das pastas de código (a saída vai para `/artifacts`).

Os MonoBehaviours não contêm regras: traduzem input em chamadas a casos de uso e reagem a eventos de domínio para atualizar a tela.

**Pendente:** a criação do projeto Unity e dos arquivos `package.json` / `.asmdef` depende da instalação da Unity (ETAPA 01 — FASE 02).

---

# 9. Composition Root

O único lugar que conhece todas as camadas e monta o grafo de objetos é o ponto de entrada da apresentação.

```text
Program.cs (terminal)  /  Bootstrap (Unity)
   │
   ├── cria Infrastructure   (SystemRandomSource)
   ├── cria Application      (GameSession recebendo a fonte de aleatoriedade)
   └── entrega a GameSession à interface
```

A injeção é feita manualmente por construtor. Um contêiner de DI só será adotado se o grafo crescer a ponto de justificá-lo.

Implementado na versão em terminal (`src/DungeonCrawler.Cli/Program.cs`). **Pendente:** o equivalente na Unity.

---

# 10. Testes

| Projeto | Verifica |
|---|---|
| `tests/DungeonCrawler.Domain.Tests` | Regras, invariantes e fórmulas do domínio; Golden Path; simulação de balanceamento |
| `tests/DungeonCrawler.Application.Tests` | Casos de uso da `GameSession` |
| `tests/DungeonCrawler.Architecture.Tests` | Regra de dependência e independência da Unity |

Dois testes merecem destaque:

- **Golden Path** (`GoldenPathTests`): um Guerreiro novo percorre o fluxo completo do RF-043, do primeiro Goblin ao chefe.
- **Simulação de balanceamento** (`BalanceSimulationTests`): um jogador automático disputa 300 expedições com sementes fixas por classe. O teste falha se a taxa de vitória da primeira expedição sair da faixa de 25% a 85%, ou se o progresso permanente não tornar as expedições seguintes mais fáceis.

Dublês de aleatoriedade usados nos testes: `FixedRandomSource` (sequência roteirizada), `ConstantRandomSource` (sempre o mesmo valor) e `SeededRandomSource` (pseudoaleatório reproduzível).

A estratégia completa de testes é assunto da ETAPA 12.

---

# 11. Validação Arquitetural

| Item | Verificação | Status |
|---|---|---|
| Domain não depende de nada além do .NET | Teste automatizado | ✅ |
| Application depende apenas de Domain | Teste automatizado | ✅ |
| Infrastructure depende apenas de Application e Domain | Teste automatizado | ✅ |
| Núcleo não referencia Unity | Teste automatizado | ✅ |
| Núcleo compatível com Unity (.NET Standard 2.1 / C# 9) | Teste automatizado + `LangVersion` no build | ✅ |
| Regras testáveis sem Unity | `dotnet test` | ✅ |
| Aleatoriedade determinística em testes | `IRandomSource` + dublê de teste | ✅ |
| Segunda apresentação sobre o mesmo núcleo | Versão em terminal jogável de ponta a ponta | ✅ |
| Composition Root | `Program.cs` da versão em terminal | ✅ |
| Integração do núcleo na Unity | Manual, após instalar a Unity | ⬜ |

---

# 12. Rastreabilidade

| Requisito | Onde é atendido |
|---|---|
| RNF-001 — C# | Toda a solução |
| RNF-002 — Unity | Camada Presentation (pendente) |
| RNF-003 — Independência do domínio | §4, testes de arquitetura |
| RNF-004 / RNF-024 — Testabilidade sem Unity | §10 |
| RNF-011 — Determinismo testável | §5.5 |
| RNF-012 — Fórmulas centralizadas | §5.6 |
| RNF-014 — Estados inválidos | §5.4 |
| RNF-019 — Domain Core | §5.1 |
| RNF-020 — Separação de camadas | §3 |
| RNF-021 — Direção das dependências | §4 |
