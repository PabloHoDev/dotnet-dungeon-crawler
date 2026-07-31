# Enemies

## ETAPA 00 — Visão e Definição do Produto

**Documento:** `ENEMIES.md`

**Projeto:** Dungeon Crawler

**Versão:** 1.0

**Status:** Em Desenvolvimento

---

# 1. Objetivo do Documento

Este documento define as regras conceituais relacionadas aos inimigos do Dungeon Crawler.

Seu objetivo é estabelecer:

* O conceito de inimigo;
* O papel dos inimigos no jogo;
* Os tipos e arquétipos;
* A estrutura conceitual do `Enemy`;
* Atributos;
* Comportamentos;
* Habilidades;
* Estados;
* Resistências;
* Fraquezas;
* Inimigos comuns;
* Inimigos Elite;
* Mini Bosses;
* Bosses;
* Distribuição por regiões;
* Escalonamento;
* Recompensas;
* Relação com combate;
* Relação com dungeon;
* Relação com progressão.

A implementação técnica será realizada posteriormente nas etapas de **Arquitetura**, **Núcleo de Domínio**, **Combate** e **Inimigos**.

---

# 2. Conceito de Inimigo

Um inimigo é uma entidade controlada pelo sistema que representa uma ameaça ao personagem durante a exploração.

Os inimigos têm como principais objetivos:

* Criar desafios;
* Exigir decisões estratégicas;
* Consumir recursos;
* Impedir progressão;
* Recompensar o jogador após sua derrota.

O inimigo será um dos principais elementos responsáveis pelo ciclo:

```text
Exploração
    ↓
Encontro
    ↓
Combate
    ↓
Derrota do Inimigo
    ↓
Recompensa
    ↓
Continuação da Exploração
```

---

# 3. Papel dos Inimigos

Os inimigos deverão cumprir diferentes funções dentro da experiência.

## 3.1 Desafio

O inimigo deve representar uma ameaça adequada ao estágio atual da dungeon.

---

## 3.2 Ensino

Novos inimigos poderão ensinar ao jogador novas mecânicas.

Exemplo:

Um inimigo resistente a ataques físicos pode incentivar o jogador a utilizar habilidades.

---

## 3.3 Pressão sobre Recursos

Os combates deverão consumir recursos do jogador.

Exemplos:

* Vida;
* Mana;
* Energia;
* Consumíveis;
* Habilidades.

Isso aumenta a importância das decisões durante a exploração.

---

## 3.4 Progressão

A derrota de inimigos poderá conceder:

* Experiência;
* Ouro;
* Equipamentos;
* Recursos;
* Itens especiais.

---

# 4. Princípio de Arquitetura

O sistema de inimigos seguirá o mesmo princípio definido para personagens:

> **Composição sobre herança excessiva.**

A estrutura conceitual será:

```text
Enemy
   │
   ├── Stats
   ├── Abilities
   ├── Behavior
   ├── Resistances
   ├── Loot
   └── Archetype
```

Essa abordagem evita uma árvore extensa de classes.

---

# 5. Abordagem Evitada

O projeto não deverá utilizar uma hierarquia excessivamente profunda como:

```text
Enemy
├── Goblin
│   ├── GoblinWarrior
│   └── GoblinArcher
│
├── Skeleton
│   ├── SkeletonWarrior
│   └── SkeletonMage
│
└── Orc
    ├── OrcWarrior
    └── OrcBerserker
```

Essa abordagem aumenta:

* Acoplamento;
* Complexidade;
* Dificuldade de manutenção;
* Dificuldade de testes.

---

# 6. Estrutura Conceitual

O inimigo será composto por diferentes componentes.

```text
                       Enemy
                         │
        ┌────────────────┼────────────────┐
        │                │                │
        ▼                ▼                ▼
      Stats           Behavior         Abilities
        │                │                │
        └────────────────┼────────────────┘
                         │
                 ┌───────┴───────┐
                 ▼               ▼
            Resistances         Loot
```

Cada componente terá responsabilidade própria.

---

# 7. Enemy

`Enemy` representa a entidade principal do inimigo.

Conceitualmente:

```text
Enemy
│
├── Identity
├── Archetype
├── Stats
├── Abilities
├── Behavior
├── Resistances
├── Loot
└── State
```

O `Enemy` deverá possuir identidade e estado próprios.

---

# 8. Identidade do Inimigo

Cada inimigo poderá possuir:

* ID;
* Nome;
* Tipo;
* Arquétipo;
* Nível;
* Categoria.

Exemplo:

```text
ID: enemy_001
Nome: Goblin
Tipo: Goblin
Arquétipo: Melee
Categoria: Common
```

---

# 9. Arquétipos

Os arquétipos representam o comportamento predominante do inimigo.

Inicialmente serão considerados:

```text
Melee
Ranged
Tank
Support
Assassin
Caster
```

Um inimigo poderá possuir mais de uma característica comportamental.

---

# 10. Arquétipo Melee

Inimigos de combate próximo.

Características:

* Aproximação do jogador;
* Ataques físicos;
* Pressão direta;
* Distância curta.

Exemplos:

* Goblin;
* Skeleton Warrior;
* Orc.

---

# 11. Arquétipo Ranged

Inimigos especializados em ataques à distância.

Características:

* Mantêm distância;
* Atacam de longe;
* Podem exigir posicionamento.

Exemplos:

* Goblin Archer;
* Skeleton Archer.

---

# 12. Arquétipo Tank

Inimigos especializados em resistência.

Características:

* Alta defesa;
* Alta vida;
* Baixa mobilidade;
* Capacidade de absorver dano.

Sua principal função é prolongar o combate.

---

# 13. Arquétipo Support

Inimigos que auxiliam outros inimigos.

Podem:

* Curar;
* Aplicar buffs;
* Remover efeitos;
* Alterar o campo de batalha.

Esses inimigos deverão ser considerados prioritários em determinadas situações.

---

# 14. Arquétipo Assassin

Inimigos especializados em ataques de oportunidade.

Características:

* Alta velocidade;
* Alto dano;
* Baixa resistência;
* Possibilidade de atacar alvos vulneráveis.

---

# 15. Arquétipo Caster

Inimigos especializados em habilidades.

Características:

* Alto poder de habilidade;
* Ataques mágicos;
* Controle;
* Uso de recursos.

---

# 16. Categorias de Inimigos

Os inimigos serão divididos em quatro categorias principais.

```text
Enemy
│
├── Common
├── Elite
├── Mini Boss
└── Boss
```

---

# 17. Inimigos Comuns

São os inimigos encontrados com maior frequência.

Características:

* Baixa complexidade;
* Menor resistência;
* Recompensas menores;
* Alta frequência.

Exemplos iniciais:

* Goblin;
* Skeleton;
* Wolf.

---

# 18. Inimigos Elite

Inimigos Elite são versões mais poderosas dos inimigos comuns.

Possuem:

* Mais vida;
* Mais dano;
* Habilidades adicionais;
* Resistências superiores;
* Recompensas melhores.

Conceitualmente:

```text
Common Enemy
     ↓
Modificadores
     ↓
Elite Enemy
```

Um Elite não precisa ser uma nova classe.

Ele poderá ser criado através de modificadores sobre uma definição existente.

---

# 19. Mini Boss

Mini Bosses representam desafios intermediários.

Eles serão mais fortes que inimigos Elite, mas não representarão o chefe principal da dungeon.

Poderão possuir:

* Mecânicas próprias;
* Habilidades especiais;
* Maior quantidade de vida;
* Recompensas superiores.

---

# 20. Boss

O Boss representa o principal inimigo de uma dungeon.

Cada dungeon deverá possuir pelo menos um Boss principal.

O Boss deverá possuir:

* Identidade própria;
* Mecânicas exclusivas;
* Habilidades especiais;
* Maior dificuldade;
* Recompensa exclusiva.

---

# 21. Estrutura de Boss

O Boss poderá utilizar múltiplas fases.

Exemplo:

```text
Boss
 │
 ├── Fase 1
 │
 ├── Fase 2
 │
 └── Fase Final
```

Cada fase poderá alterar:

* Habilidades;
* Comportamento;
* Ataques;
* Resistências;
* Prioridades.

---

# 22. Atributos

Os inimigos utilizarão atributos compatíveis com o sistema de combate.

Principais atributos:

* Vida;
* Ataque;
* Defesa;
* Velocidade;
* Poder de habilidade;
* Resistências;
* Chance de crítico.

Alguns inimigos poderão possuir atributos especiais.

---

# 23. Atributos Derivados

Os atributos derivados serão calculados a partir da configuração do inimigo.

Exemplos:

```text
Dano
Velocidade de Ação
Chance de Crítico
Resistência Efetiva
Poder de Habilidade
```

Conceitualmente:

```text
Base Stats
     +
Modifiers
     ↓
Effective Stats
```

---

# 24. Habilidades

Os inimigos poderão possuir habilidades próprias.

Exemplos:

* Ataque poderoso;
* Ataque à distância;
* Cura;
* Buff;
* Debuff;
* Controle;
* Ataque em área.

Estrutura conceitual:

```text
Ability
│
├── Nome
├── Custo
├── Alcance
├── Efeito
├── Cooldown
└── Condições
```

---

# 25. Comportamento

O comportamento representa a lógica de decisão do inimigo.

Exemplo:

```text
Behavior
│
├── MeleeBehavior
├── RangedBehavior
├── SupportBehavior
├── TankBehavior
└── AssassinBehavior
```

Esses comportamentos deverão ser tratados como estratégias intercambiáveis.

Isso permitirá reutilização.

---

# 26. Exemplo de Composição

Um inimigo poderá ser configurado como:

```text
Goblin Archer

Archetype
→ Ranged

Stats
→ Baixa Vida
→ Médio Ataque
→ Alta Velocidade

Behavior
→ RangedBehavior

Abilities
→ BasicShot
→ PowerShot

Resistances
→ Física: Baixa
→ Mágica: Média

Loot
→ Gold
→ CommonMaterial
```

Outro inimigo poderá reutilizar o mesmo comportamento:

```text
Skeleton Archer

Archetype
→ Ranged

Behavior
→ RangedBehavior
```

Não será necessário duplicar a implementação.

---

# 27. Estados

Os inimigos poderão possuir estados como:

```text
IDLE
ALERT
COMBAT
STUNNED
DEFEATED
```

Fluxo básico:

```text
IDLE
 ↓
ALERT
 ↓
COMBAT
 ↓
DEFEATED
```

Estados adicionais poderão ser introduzidos posteriormente.

---

# 28. Resistências

Os inimigos poderão possuir resistências específicas.

Exemplos:

```text
Resistência Física
Resistência Mágica
Resistência Elemental
Resistência a Controle
```

Essas resistências deverão criar decisões estratégicas.

---

# 29. Fraquezas

Alguns inimigos possuirão fraquezas.

Exemplo:

```text
Skeleton

+ Resistência física
- Fraqueza a determinado tipo de dano
```

As fraquezas deverão incentivar o jogador a adaptar sua estratégia.

---

# 30. Estados Negativos

Os inimigos poderão receber efeitos temporários.

Exemplos:

* Poison;
* Burn;
* Freeze;
* Stun;
* Slow;
* Weaken.

O sistema de efeitos será detalhado posteriormente no sistema de combate.

---

# 31. Inteligência de Combate

A inteligência dos inimigos não terá como objetivo simular uma inteligência artificial complexa.

O objetivo será produzir comportamentos previsíveis o suficiente para serem compreendidos pelo jogador, mas variados o suficiente para criar desafios.

O sistema poderá considerar:

* Vida própria;
* Vida do jogador;
* Distância;
* Habilidades disponíveis;
* Estado do combate;
* Aliados;
* Prioridade de alvos.

---

# 32. Seleção de Alvos

O inimigo poderá selecionar seus alvos utilizando diferentes estratégias.

Exemplos:

```text
Nearest
LowestHealth
HighestThreat
Random
SupportPriority
```

Isso permitirá comportamentos distintos sem criar classes específicas para cada caso.

---

# 33. Estratégias de Comportamento

A arquitetura poderá utilizar o conceito de Strategy.

Exemplo:

```text
Enemy
 │
 └── Behavior Strategy
       │
       ├── MeleeStrategy
       ├── RangedStrategy
       ├── SupportStrategy
       └── AssassinStrategy
```

Essa abordagem permitirá alterar o comportamento sem alterar o `Enemy`.

---

# 34. Loot

Cada inimigo poderá possuir uma tabela de recompensas.

Exemplo:

```text
Enemy
 ↓
Loot Table
 ↓
 ├── Gold
 ├── Material
 ├── Equipment
 └── Consumable
```

A chance de obtenção poderá variar conforme:

* Tipo;
* Categoria;
* Dungeon;
* Modificadores;
* Raridade.

---

# 35. Recompensas por Categoria

| Categoria | Experiência |       Ouro | Loot      |
| --------- | ----------: | ---------: | --------- |
| Common    |       Baixa |      Baixo | Comum     |
| Elite     |       Média |      Médio | Melhor    |
| Mini Boss |        Alta |       Alta | Raro      |
| Boss      |  Muito alta | Muito alta | Exclusivo |

Os valores exatos serão definidos durante o balanceamento.

---

# 36. Distribuição por Região

Os inimigos deverão possuir relação com o ambiente.

Exemplo:

```text
Cripta
├── Skeleton
├── Skeleton Archer
└── Skeleton Knight

Floresta
├── Wolf
├── Goblin
└── Goblin Archer

Mina
├── Goblin
├── Rock Golem
└── Cave Creature
```

Essa distribuição ajudará a construir a identidade das regiões.

---

# 37. Distribuição por Dungeon

Cada dungeon possuirá uma seleção controlada de inimigos.

Conceitualmente:

```text
Dungeon
 │
 ├── Common Pool
 ├── Elite Pool
 ├── Mini Boss Pool
 └── Boss
```

O gerador híbrido poderá selecionar inimigos dessas listas durante a criação da expedição.

---

# 38. Escalonamento

A dificuldade dos inimigos deverá acompanhar a progressão do jogador.

O escalonamento poderá ocorrer através de:

* Nível;
* Vida;
* Dano;
* Defesa;
* Habilidades;
* Quantidade;
* Composição dos grupos.

Exemplo:

```text
Floor 1
→ Inimigos básicos

Floor 2
→ Inimigos básicos + variantes

Floor 3
→ Elite

Floor 4
→ Combinações mais complexas

Boss
→ Desafio máximo
```

---

# 39. Modificadores

O sistema deverá permitir modificar inimigos sem criar novos tipos.

Exemplos:

```text
Goblin
     +
Elite
     ↓
Elite Goblin
```

Ou:

```text
Skeleton
     +
Fire Modifier
     ↓
Flame Skeleton
```

Esses modificadores aumentarão a variedade do jogo.

---

# 40. Relação com Dungeon

O sistema de inimigos estará diretamente relacionado ao sistema de Dungeon.

A dungeon determinará:

* Pool de inimigos;
* Nível;
* Frequência;
* Categorias disponíveis;
* Boss.

Fluxo:

```text
Dungeon
   ↓
Floor
   ↓
Room
   ↓
Encounter
   ↓
Enemy Selection
   ↓
Combat
```

---

# 41. Relação com Combate

O inimigo será uma das principais entidades utilizadas pelo sistema de combate.

O combate deverá consumir informações do inimigo:

```text
Enemy
 ├── Stats
 ├── Abilities
 ├── Behavior
 └── Resistances
        ↓
      Combat
```

O `Enemy` não deverá conter toda a lógica do combate.

As regras deverão permanecer separadas.

---

# 42. Relação com Progressão

A derrota de inimigos contribuirá para a evolução do jogador.

```text
Enemy Defeated
      ↓
Experience
      ↓
Level
      ↓
Character Progression
```

A dificuldade dos inimigos também deverá acompanhar essa progressão.

---

# 43. Relação com Personagens

Personagens e inimigos compartilharão conceitos relacionados a combate.

Ambos poderão possuir:

* Vida;
* Atributos;
* Habilidades;
* Estados;
* Resistências.

Entretanto, isso não significa que deverão possuir exatamente a mesma implementação.

O objetivo é compartilhar regras quando fizer sentido e manter responsabilidades específicas quando necessário.

---

# 44. Bestiário Inicial do MVP

O MVP contará inicialmente com:

## Comuns

```text
Goblin
Skeleton
Wolf
```

## Elite

```text
Elite Goblin
```

## Mini Boss

```text
Skeleton Knight
```

## Boss

```text
Dungeon Boss
```

O conjunto poderá ser expandido posteriormente.

---

# 45. Objetivo do Bestiário

O objetivo do MVP não será possuir dezenas de inimigos.

O objetivo será demonstrar variedade comportamental.

Exemplo:

```text
Goblin
→ Melee

Skeleton
→ Ranged / Caster

Wolf
→ Assassin

Elite Goblin
→ Melee + Modifier

Skeleton Knight
→ Tank

Boss
→ Comportamento próprio
```

Poucos inimigos com comportamentos diferentes demonstrarão melhor a arquitetura do que dezenas de variantes simples.

---

# 46. Escopo do MVP

O MVP deverá conter:

### Inimigos

* [x] Estrutura conceitual de Enemy;
* [x] Atributos;
* [x] Habilidades;
* [x] Comportamentos;
* [x] Resistências;
* [x] Estados;
* [x] Loot;
* [x] Sistema de categorias;
* [x] Common;
* [x] Elite;
* [x] Mini Boss;
* [x] Boss;
* [x] Escalonamento básico;
* [x] Modificadores básicos.

### Fora do MVP

* [ ] Bestiário extenso;
* [ ] IA avançada;
* [ ] Comportamentos emergentes;
* [ ] Fações complexas;
* [ ] Sistema de relacionamento entre inimigos;
* [ ] Inimigos adaptativos;
* [ ] Chefes com dezenas de fases.

---

# 47. Princípios de Balanceamento

Os inimigos deverão ser balanceados considerando:

* Dificuldade;
* Frequência;
* Recursos consumidos;
* Recompensa;
* Contexto da dungeon.

Um inimigo difícil não deverá necessariamente possuir apenas números maiores.

A dificuldade também poderá surgir através de:

* Composição de grupo;
* Posicionamento;
* Habilidades;
* Sinergia;
* Resistências.

---

# 48. Princípios de Design

O sistema de inimigos deverá seguir os seguintes princípios:

### Clareza

O jogador deverá compreender o comportamento básico do inimigo.

### Variedade

Inimigos diferentes deverão exigir estratégias diferentes.

### Progressão

Novas regiões deverão apresentar novos desafios.

### Reutilização

Comportamentos deverão ser reutilizáveis.

### Extensibilidade

Novos inimigos deverão poder ser adicionados sem modificar o núcleo.

### Testabilidade

As regras de comportamento deverão ser testáveis isoladamente.

---

# 49. Arquitetura Conceitual Final

A estrutura consolidada será:

```text
                         Enemy
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
        ▼                  ▼                  ▼
      Stats             Abilities          Behavior
        │                  │                  │
        │                  │          ┌───────┼───────┐
        │                  │          │       │       │
        │                  │        Melee   Ranged  Support
        │                  │
        └──────────────────┼──────────────────┐
                           │                  │
                           ▼                  ▼
                      Resistances           Loot
```

O tipo concreto do inimigo será definido por composição.

Exemplo:

```text
Goblin
│
├── Stats
├── MeleeBehavior
├── BasicAttack
├── PhysicalResistance
└── CommonLoot
```

Outro:

```text
Skeleton Archer
│
├── Stats
├── RangedBehavior
├── ArrowAttack
├── MagicResistance
└── UndeadLoot
```

---

# 50. Critérios de Validação

A FASE 07 será considerada concluída quando:

* O conceito de inimigo estiver definido;
* O papel dos inimigos estiver definido;
* A estrutura do `Enemy` estiver definida;
* O princípio de composição estiver documentado;
* Os arquétipos estiverem definidos;
* As categorias estiverem definidas;
* Inimigos comuns estiverem especificados;
* Elite estiver especificado;
* Mini Boss estiver especificado;
* Boss estiver especificado;
* Atributos estiverem definidos;
* Habilidades estiverem definidas;
* Comportamentos estiverem definidos;
* Estratégias de comportamento estiverem definidas;
* Resistências estiverem definidas;
* Fraquezas estiverem definidas;
* Estados estiverem definidos;
* Seleção de alvo estiver definida;
* Loot estiver definido;
* Distribuição por região estiver definida;
* Escalonamento estiver definido;
* Modificadores estiverem definidos;
* Relação com Dungeon estiver definida;
* Relação com Combate estiver definida;
* Relação com Personagens estiver definida;
* Relação com Progressão estiver definida;
* Bestiário inicial estiver definido;
* Escopo do MVP estiver definido;
* Princípios arquiteturais estiverem registrados.

---

# 51. Checking da Fase

| Item                                   | Status |
| -------------------------------------- | ------ |
| Objetivo da fase definido              | ✅      |
| Conceito de inimigo definido           | ✅      |
| Papel dos inimigos definido            | ✅      |
| Estrutura do Enemy definida            | ✅      |
| Composição sobre herança definida      | ✅      |
| Arquétipos definidos                   | ✅      |
| Categorias definidas                   | ✅      |
| Common definido                        | ✅      |
| Elite definido                         | ✅      |
| Mini Boss definido                     | ✅      |
| Boss definido                          | ✅      |
| Atributos definidos                    | ✅      |
| Habilidades definidas                  | ✅      |
| Comportamentos definidos               | ✅      |
| Estratégias de comportamento definidas | ✅      |
| Resistências definidas                 | ✅      |
| Fraquezas definidas                    | ✅      |
| Estados definidos                      | ✅      |
| Seleção de alvos definida              | ✅      |
| Loot definido                          | ✅      |
| Distribuição por região definida       | ✅      |
| Escalonamento definido                 | ✅      |
| Modificadores definidos                | ✅      |
| Relação com Dungeon definida           | ✅      |
| Relação com Combate definida           | ✅      |
| Relação com Personagens definida       | ✅      |
| Relação com Progressão definida        | ✅      |
| Bestiário inicial definido             | ✅      |
| Escopo do MVP definido                 | ✅      |
| Critérios de validação definidos       | ✅      |

---

# 52. Decisão Arquitetural Registrada

A seguinte decisão passa a fazer parte oficialmente das especificações do projeto:

> O sistema de inimigos utilizará composição e estratégias comportamentais como mecanismos principais de extensão, evitando uma hierarquia extensa de classes específicas para cada tipo de inimigo.

Estrutura conceitual:

```text
Enemy
   │
   ├── Stats
   ├── Abilities
   ├── Behavior
   ├── Resistances
   ├── Loot
   └── Archetype
```

Comportamentos poderão ser reutilizados entre diferentes inimigos.

Exemplo:

```text
Goblin
   └── MeleeBehavior

Orc
   └── MeleeBehavior

Skeleton Knight
   └── TankBehavior
```

Essa decisão deverá ser respeitada durante as etapas de:

* ETAPA 02 — Arquitetura;
* ETAPA 03 — Núcleo de Domínio;
* ETAPA 05 — Combate;
* ETAPA 06 — Inimigos;
* ETAPA 08 — Dungeon e Exploração.

---

# 53. Status da Fase

**Documento:** `ENEMIES.md`

**Etapa:** ETAPA 00 — Visão e Definição do Produto

**Fase:** FASE 07 — Inimigos

**Versão:** 1.0

**Status:** ✅ Concluída e validada em nível conceitual.

---

# 54. Atualização do Roadmap

## ETAPA 00 — Visão e Definição do Produto

```text
✅ FASE 01 — Product Vision
   └── PRODUCT_VISION.md

✅ FASE 02 — Game Concept
   └── GAME_CONCEPT.md

✅ FASE 03 — Core Gameplay
   └── CORE_GAMEPLAY.md

✅ FASE 04 — Game Mechanics
   └── GAME_MECHANICS.md

✅ FASE 05 — World and Dungeon
   └── WORLD_AND_DUNGEON.md

✅ FASE 06 — Personagens
   └── CHARACTERS.md

✅ FASE 07 — Inimigos
   └── ENEMIES.md

⬜ FASE 08 — Progressão
⬜ FASE 09 — MVP
⬜ FASE 10 — Requisitos Funcionais e Não Funcionais

⬜ CHECKING FINAL DA ETAPA 00
```

---

# 55. Conclusão

A FASE 07 estabelece a definição conceitual do sistema de inimigos do Dungeon Crawler.

O sistema foi projetado para priorizar:

* Composição;
* Reutilização;
* Estratégias comportamentais;
* Baixo acoplamento;
* Extensibilidade;
* Testabilidade.

O MVP possuirá um conjunto reduzido de inimigos, mas com comportamentos suficientemente diferentes para demonstrar a variedade do sistema.

O bestiário inicial será:

```text
COMMON
├── Goblin
├── Skeleton
└── Wolf

ELITE
└── Elite Goblin

MINI BOSS
└── Skeleton Knight

BOSS
└── Dungeon Boss
```

O objetivo não é criar quantidade artificial de conteúdo, mas construir uma base arquitetural capaz de suportar a expansão do bestiário posteriormente.

Com a conclusão deste documento, a **FASE 07 — Inimigos** está formalmente documentada.

**Status final: 🟢 FASE 07 APROVADA.**
