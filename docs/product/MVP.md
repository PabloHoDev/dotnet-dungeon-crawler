# Minimum Viable Product — MVP

**Projeto:** Dungeon Crawler
**Documento:** `docs/product/MVP.md`
**Etapa:** ETAPA 00 — Visão e Definição do Produto
**Fase:** FASE 09 — MVP
**Versão:** 1.0
**Status:** Em definição

---

# 1. Objetivo

Este documento define o **Minimum Viable Product (MVP)** do Dungeon Crawler.

O MVP representa a primeira versão completa e jogável do projeto.

Seu objetivo não é representar todo o conteúdo futuro do jogo, mas fornecer uma experiência suficientemente completa para validar:

* Core Gameplay;
* Combate;
* Exploração;
* Progressão;
* Personagens;
* Inimigos;
* Dungeon;
* Inventário;
* Equipamentos;
* Recompensas;
* Boss;
* Arquitetura;
* POO;
* Regras de domínio;
* Testabilidade.

O MVP deverá ser pequeno em conteúdo, porém significativo em termos de **engenharia de software**.

---

# 2. Objetivo do MVP

O MVP deverá responder positivamente à seguinte pergunta:

> **É possível criar um personagem, explorar uma dungeon, combater inimigos, evoluir, obter equipamentos, enfrentar desafios progressivamente maiores e concluir a expedição?**

O ciclo mínimo deverá ser:

```text
Criar Personagem
       ↓
Entrar na Dungeon
       ↓
Explorar
       ↓
Encontrar Inimigo
       ↓
Combater
       ↓
Vencer
       ↓
Receber XP / Loot
       ↓
Evoluir
       ↓
Avançar
       ↓
Enfrentar Boss
       ↓
Concluir Expedição
```

---

# 3. Princípio do MVP

O MVP deverá seguir três princípios:

### 3.1 Pequeno em conteúdo

Não teremos centenas de inimigos, itens ou mapas.

### 3.2 Completo em gameplay

O MVP deverá conter o ciclo completo de jogo.

### 3.3 Forte tecnicamente

O projeto deverá demonstrar conhecimentos de:

* C#;
* POO;
* Arquitetura;
* Design de Software;
* Testes;
* Modelagem de domínio;
* Matemática aplicada;
* Engenharia de Software.

---

# 4. Escopo Geral

O MVP será composto por:

```text
Dungeon Crawler MVP
│
├── Character
├── Classes
├── Stats
├── Abilities
├── Equipment
├── Inventory
├── Combat
├── Enemies
├── Dungeon
├── Exploration
├── Progression
├── Rewards
├── Boss
└── Expedition
```

---

# 5. Personagens

O MVP terá três classes jogáveis:

```text
Warrior
Mage
Rogue
```

Cada personagem deverá possuir:

* Identidade;
* Classe;
* Level;
* XP;
* Atributos;
* Habilidades;
* Equipamentos;
* Inventário;
* Estado atual.

---

# 6. Atributos

O sistema utilizará:

```text
Strength
Vitality
Agility
Intelligence
Luck
```

Os atributos serão utilizados por diferentes sistemas.

Exemplo:

```text
Strength
   ↓
Physical Damage

Vitality
   ↓
Health / Defense

Agility
   ↓
Speed / Evasion

Intelligence
   ↓
Ability Power

Luck
   ↓
Critical / Loot
```

Os efeitos exatos serão definidos pelos documentos específicos do sistema.

---

# 7. Evolução do Personagem

O nível máximo do MVP será:

```text
Level 20
```

O jogador receberá XP através de:

* Combates;
* Elites;
* Mini Bosses;
* Boss;
* Eventos de gameplay;
* Recompensas específicas.

A progressão utilizará uma curva não linear.

---

# 8. Escolhas do Jogador

Ao evoluir, o jogador poderá distribuir pontos de atributo.

Exemplo:

```text
Level Up
   ↓
+5 Attribute Points
   ↓
Player Choice
```

Isso permitirá diferentes builds.

O MVP não deverá forçar uma única maneira de construir cada classe.

---

# 9. Habilidades

O MVP deverá possuir habilidades desbloqueadas progressivamente.

O sistema combinará:

```text
Level Requirement
+
Player Choice
+
Ability Evolution
```

As habilidades poderão evoluir em níveis.

Exemplo:

```text
Fireball I
    ↓
Fireball II
    ↓
Fireball III
```

O objetivo é demonstrar:

* Abstração;
* Polimorfismo;
* Composição;
* Scaling;
* Regras de domínio.

---

# 10. Combate

O MVP deverá possuir um sistema de combate funcional.

O combate deverá contemplar:

* Ataque básico;
* Habilidades;
* Dano;
* Defesa;
* Critical Hit;
* HP;
* Vitória;
* Derrota;
* Morte;
* Recompensas.

Fluxo:

```text
Encounter
    ↓
Combat Start
    ↓
Player Action
    ↓
Enemy Action
    ↓
Resolve
    ↓
Victory / Defeat
```

---

# 11. Inimigos

O MVP possuirá inicialmente:

```text
Goblin
Skeleton
Orc
Elite Enemy
Mini Boss
Boss
```

A quantidade poderá ser aumentada posteriormente.

A prioridade será criar **comportamentos diferentes**, e não simplesmente aumentar a quantidade.

---

# 12. Elite Enemy

O Elite deverá representar uma versão mais perigosa de um inimigo comum.

Ele poderá possuir:

* Mais HP;
* Mais dano;
* Modificadores;
* Melhor recompensa;
* Comportamento diferenciado.

Fluxo:

```text
Normal Enemy
      ↓
Elite Modifier
      ↓
Elite Enemy
```

---

# 13. Mini Boss

O Mini Boss será um encontro intermediário.

Sua função será:

* Aumentar a tensão;
* Testar a build;
* Preparar o jogador para o Boss;
* Oferecer recompensas melhores.

---

# 14. Boss

O MVP deverá obrigatoriamente possuir um Boss.

O Boss representará o objetivo final da expedição.

Deverá possuir:

* Mais vida;
* Maior poder;
* Mecânicas próprias;
* Recompensa diferenciada;
* Condição clara de vitória.

Fluxo:

```text
Dungeon Progression
        ↓
Final Floor
        ↓
Boss
        ↓
Victory
        ↓
Expedition Complete
```

---

# 15. Dungeon

O MVP possuirá uma dungeon com:

```text
5 Floors
```

Modelo:

```text
Floor 1
   ↓
Floor 2
   ↓
Floor 3
   ↓
Floor 4
   ↓
Floor 5
   ↓
Boss
```

Os andares deverão aumentar progressivamente a dificuldade.

---

# 16. Modelo Híbrido

A dungeon seguirá o modelo híbrido previamente definido.

Teremos:

### Estrutura controlada

* Floors;
* Rooms;
* Connections;
* Progression.

### Variabilidade

* Enemies;
* Encounters;
* Loot;
* Recompensas.

Modelo:

```text
Dungeon
│
├── Fixed Structure
│   ├── Floors
│   ├── Rooms
│   └── Connections
│
└── Variable Content
    ├── Enemies
    ├── Encounters
    └── Loot
```

---

# 17. Exploração

A exploração deverá permitir:

* Movimento;
* Descoberta de salas;
* Encontros;
* Combates;
* Recompensas;
* Progressão entre andares.

O jogador deverá possuir um objetivo claro:

```text
Explore
   ↓
Progress
   ↓
Reach Boss
```

---

# 18. Escalonamento

A dificuldade dos inimigos deverá considerar:

```text
Player Level
+
Dungeon Floor
```

Modelo:

```text
Enemy Power =
Base Power
× Level Multiplier
× Floor Multiplier
```

As fórmulas serão centralizadas para permitir balanceamento.

---

# 19. Inventário

O MVP deverá possuir um inventário funcional.

O jogador poderá:

* Visualizar itens;
* Armazenar itens;
* Equipar equipamentos;
* Desequipar equipamentos;
* Gerenciar espaço.

O inventário deverá estar separado conceitualmente do equipamento.

```text
Character
│
├── Inventory
│
└── Equipment
```

---

# 20. Equipamentos

O MVP terá:

```text
Weapon
Armor
Accessory
```

Equipamentos poderão fornecer:

* Atributos;
* Modificadores;
* Efeitos.

---

# 21. Raridade

As raridades iniciais serão:

```text
Common
Uncommon
Rare
Epic
Legendary
```

A raridade deverá influenciar a qualidade potencial dos itens.

---

# 22. Loot

O loot será probabilístico.

Modelo inicial:

```text
Common      → 60%
Uncommon    → 25%
Rare        → 10%
Epic        → 4%
Legendary   → 1%
```

A soma das probabilidades deverá representar:

```text
100%
```

O sistema poderá considerar `Luck`.

---

# 23. Recompensas

Os encontros poderão fornecer:

* XP;
* Ouro;
* Itens;
* Equipamentos;
* Recompensas especiais.

A relação será:

```text
Difficulty
    ↓
Risk
    ↓
Reward Potential
```

---

# 24. Progressão Permanente

Será preservado:

* Level;
* XP;
* Atributos;
* Habilidades;
* Equipamentos;
* Progressão do personagem.

---

# 25. Progressão da Expedição

Será considerada temporária:

* Floor atual;
* Salas exploradas;
* Estado da dungeon;
* Encontros;
* Progresso da expedição;
* Recompensas temporárias.

---

# 26. Morte

Se o personagem morrer:

```text
Character
    ↓
Death
    ↓
Expedition Ends
```

A progressão permanente será preservada.

O estado da expedição será encerrado.

Isso cria risco sem apagar todo o desenvolvimento do personagem.

---

# 27. Game Loop

O Game Loop principal do MVP será:

```text
START
  ↓
CHARACTER CREATION
  ↓
CLASS SELECTION
  ↓
DUNGEON
  ↓
EXPLORE
  ↓
ENCOUNTER
  ↓
COMBAT
  ↓
REWARD
  ↓
LEVEL UP
  ↓
EQUIPMENT
  ↓
NEXT FLOOR
  ↓
ELITE
  ↓
MINI BOSS
  ↓
BOSS
  ↓
VICTORY
  ↓
EXPEDITION COMPLETE
```

---

# 28. Golden Path

O Golden Path representa o caminho mínimo que deverá demonstrar o funcionamento do jogo.

```text
Warrior
  ↓
Floor 1
  ↓
Goblin
  ↓
Level Up
  ↓
Equipment
  ↓
Floor 2
  ↓
Skeleton
  ↓
Floor 3
  ↓
Elite
  ↓
Floor 4
  ↓
Mini Boss
  ↓
Floor 5
  ↓
Boss
  ↓
Victory
```

O Golden Path será utilizado como referência durante implementação e validação.

---

# 29. Matemática do MVP

O MVP incorporará regras matemáticas no domínio.

Os principais sistemas serão:

```text
Experience Curve
Attribute Scaling
Damage Scaling
Enemy Scaling
Critical Chance
Loot Probability
```

---

# 30. Mathematical Easter Eggs

Os Easter Eggs matemáticos fazem parte oficialmente da identidade do projeto.

Eles deverão ser documentados e implementados como regras reais do domínio.

### XP

```text
XP(n) = 50 × n × (n + 1)
```

### Atributos

```text
Derived Stat =
Base
+
Attribute × Scaling
+
Level × Growth
```

### Dano

```text
Damage =
Base Damage
+
Attribute Scaling
+
Ability Growth
+
Equipment Modifier
```

### Inimigos

```text
Enemy Power =
Base Power
× Level Multiplier
× Floor Multiplier
```

### Loot

Distribuição probabilística:

```text
60% + 25% + 10% + 4% + 1% = 100%
```

### Limites

Valores como Critical Chance deverão possuir limites máximos.

---

# 31. Arquitetura do MVP

O MVP deverá manter a separação entre domínio e apresentação.

Modelo:

```text
                 GAME
                   │
        ┌──────────┴──────────┐
        │                     │
      DOMAIN                UNITY
        │                     │
        │               Presentation
        │               Rendering
        │               Input
        │               Audio
        │                     │
        └──────────┬──────────┘
                   │
                Adapters
```

A Unity não deverá conter as regras centrais do domínio.

---

# 32. Núcleo de Domínio

O domínio deverá representar as regras principais.

Conceitualmente:

```text
Domain
│
├── Characters
├── Combat
├── Enemies
├── Inventory
├── Equipment
├── Dungeon
├── Progression
└── Rewards
```

O objetivo é permitir que o núcleo seja testado sem depender diretamente da Unity.

---

# 33. Engenharia de Software

O MVP deverá demonstrar:

* C#;
* POO;
* Encapsulamento;
* Abstração;
* Polimorfismo;
* Composição;
* Interfaces;
* SOLID;
* Design Patterns quando justificáveis;
* Domain Modeling;
* Testes automatizados;
* Separação de responsabilidades.

---

# 34. O que não faz parte do MVP

Os seguintes recursos ficam explicitamente fora do MVP:

* Multiplayer;
* PvP;
* Online;
* Guildas;
* Quests complexas;
* Crafting avançado;
* Classes avançadas;
* Meta-progressão complexa;
* Prestígio;
* Leaderboards;
* Economia complexa;
* Centenas de inimigos;
* Centenas de itens;
* Procedural generation extremamente sofisticada;
* Sistemas sociais;
* Conteúdo online;
* Sistemas de monetização.

Esses recursos poderão ser considerados em versões futuras.

---

# 35. MVP x Produto Futuro

```text
                    DUNGEON CRAWLER
                           │
             ┌─────────────┴─────────────┐
             │                           │
            MVP                       FUTURO
             │                           │
        3 Classes                  Novas Classes
        5 Floors                   Procedural
        Boss                       Multiplayer
        Combat                     Crafting
        Loot                       Quests
        Progression                Meta Progression
        Inventory                  New Systems
        Equipment                  More Content
```

---

# 36. Critérios de Sucesso

O MVP será considerado funcional quando:

* [ ] Criar personagem;
* [ ] Escolher classe;
* [ ] Entrar na dungeon;
* [ ] Explorar;
* [ ] Encontrar inimigos;
* [ ] Combater;
* [ ] Usar habilidades;
* [ ] Vencer combate;
* [ ] Receber XP;
* [ ] Subir de nível;
* [ ] Distribuir atributos;
* [ ] Receber equipamento;
* [ ] Equipar item;
* [ ] Avançar de andar;
* [ ] Encontrar Elite;
* [ ] Encontrar Mini Boss;
* [ ] Encontrar Boss;
* [ ] Vencer Boss;
* [ ] Concluir expedição;
* [ ] Morrer;
* [ ] Encerrar expedição após morte;
* [ ] Preservar progressão permanente;
* [ ] Iniciar nova expedição.

---

# 37. Definition of Done

O MVP somente será considerado concluído quando:

### Funcionalidade

* [ ] Golden Path executável;
* [ ] Sistemas principais integrados;
* [ ] Vitória funcionando;
* [ ] Derrota funcionando.

### Código

* [ ] Código organizado;
* [ ] Responsabilidades separadas;
* [ ] Domínio desacoplado da Unity;
* [ ] Regras principais encapsuladas.

### Testes

* [ ] Testes das principais regras;
* [ ] Testes das fórmulas;
* [ ] Testes de progressão;
* [ ] Testes de combate;
* [ ] Testes de recompensas.

### Documentação

* [ ] README atualizado;
* [ ] Arquitetura documentada;
* [ ] Gameplay documentado;
* [ ] Progressão documentada;
* [ ] MVP documentado.

### Execução

* [ ] Projeto executável;
* [ ] Build funcional;
* [ ] Golden Path validado.

---

# 38. Critérios de Aceite

O MVP deverá atender aos seguintes critérios:

### AC01 — Criação

O jogador consegue criar um personagem.

### AC02 — Classe

O jogador consegue escolher uma classe.

### AC03 — Exploração

O jogador consegue entrar e progredir na dungeon.

### AC04 — Combate

O jogador consegue enfrentar inimigos.

### AC05 — Progressão

O personagem recebe XP e pode evoluir.

### AC06 — Build

O jogador consegue distribuir atributos.

### AC07 — Equipamento

O jogador consegue obter e equipar itens.

### AC08 — Escalonamento

Os desafios aumentam conforme a progressão.

### AC09 — Boss

O jogador consegue enfrentar e derrotar o Boss.

### AC10 — Vitória

A expedição pode ser concluída.

### AC11 — Derrota

O personagem pode morrer.

### AC12 — Persistência de progresso

A progressão permanente não é apagada pela morte.

---

# 39. Riscos do MVP

Os principais riscos são:

### Scope Creep

Adicionar funcionalidades continuamente.

### Overengineering

Criar arquitetura excessivamente complexa antes da necessidade.

### Balanceamento

Criar fórmulas matemáticas difíceis de equilibrar.

### Conteúdo

Criar quantidade excessiva de inimigos, itens ou salas.

### Unity Coupling

Permitir que regras de domínio dependam diretamente da engine.

---

# 40. Estratégia contra Scope Creep

Qualquer nova funcionalidade deverá responder:

> **Ela é necessária para validar o Core Gameplay do MVP?**

Se a resposta for:

```text
SIM → considerar inclusão
NÃO → Future Roadmap
```

Essa regra deverá ser aplicada durante toda a implementação.

---

# 41. Priorização

As funcionalidades serão classificadas como:

### P0 — Obrigatório

Sem isso, não existe MVP.

### P1 — Importante

Melhora significativamente a experiência.

### P2 — Desejável

Pode ser adicionado se houver tempo.

### FUTURE

Não pertence ao MVP.

---

# 42. Priorização do MVP

| Sistema          | Prioridade        |
| ---------------- | ----------------- |
| Character        | P0                |
| Classes          | P0                |
| Stats            | P0                |
| Combat           | P0                |
| Enemies          | P0                |
| Dungeon          | P0                |
| Progression      | P0                |
| Inventory        | P0                |
| Equipment        | P0                |
| Loot             | P0                |
| Boss             | P0                |
| Exploration      | P0                |
| Save/Load        | FUTURE nesta fase |
| Multiplayer      | FUTURE            |
| Crafting         | FUTURE            |
| Quests           | FUTURE            |
| Meta Progression | FUTURE            |
| Multiplayer/PvP  | FUTURE            |

---

# 43. Escopo Técnico Inicial

O MVP deverá possuir conceitualmente:

```text
Domain
│
├── Characters
│   ├── Character
│   ├── Stats
│   ├── Class
│   └── Abilities
│
├── Combat
│
├── Enemies
│
├── Inventory
│
├── Equipment
│
├── Dungeon
│
├── Progression
│
└── Rewards
```

A estrutura física será refinada posteriormente na **ETAPA 02 — Arquitetura**.

---

# 44. Relação com as Fases Anteriores

O MVP consolida as decisões tomadas anteriormente.

```text
PRODUCT VISION
      ↓
GAME CONCEPT
      ↓
CORE GAMEPLAY
      ↓
GAME MECHANICS
      ↓
WORLD & DUNGEON
      ↓
CHARACTERS
      ↓
ENEMIES
      ↓
PROGRESSION
      ↓
      MVP
```

O MVP representa o primeiro recorte concreto de todas essas decisões.

---

# 45. Relação com as Próximas Etapas

Depois da conclusão da ETAPA 00:

```text
ETAPA 01
Setup e Repositório
       ↓
ETAPA 02
Arquitetura
       ↓
ETAPA 03
Núcleo de Domínio
       ↓
...
```

A documentação criada nesta etapa será utilizada como referência durante a implementação.

---

# 46. Checklist da FASE 09

## Produto

* [x] Objetivo do MVP definido;
* [x] Escopo definido;
* [x] Core Gameplay definido;
* [x] Golden Path definido;
* [x] MVP x Future definido.

## Gameplay

* [x] Personagem;
* [x] Classes;
* [x] Combate;
* [x] Inimigos;
* [x] Dungeon;
* [x] Progressão;
* [x] Loot;
* [x] Equipamentos;
* [x] Boss;
* [x] Morte;
* [x] Vitória.

## Engenharia

* [x] C# definido;
* [x] Unity definida;
* [x] Domínio desacoplado;
* [x] POO;
* [x] Testabilidade;
* [x] Matemática;
* [x] Fórmulas centralizadas.

## Controle

* [x] Scope definido;
* [x] Priorização definida;
* [x] Critérios de aceite definidos;
* [x] Definition of Done definido;
* [x] Riscos identificados.

---

# 47. Status da FASE

**Documento:** `docs/product/MVP.md`

**Etapa:** ETAPA 00 — Visão e Definição do Produto

**Fase:** FASE 09 — MVP

**Versão:** 1.0

**Status:** 🟡 Documentação concluída — aguardando Checking.

---

# 48. Próximo Passo

A FASE 09 deverá passar pelo processo oficial de validação:

```text
MVP.md
   ↓
CHECKING
   ↓
Verificar decisões
   ↓
Identificar pendências
   ↓
Corrigir
   ↓
Validação final
   ↓
Aprovar FASE 09
   ↓
Atualizar ROADMAP
```

Somente após a aprovação da FASE 09 será iniciado o trabalho da:

**FASE 10 — Requisitos Funcionais e Não Funcionais.**
