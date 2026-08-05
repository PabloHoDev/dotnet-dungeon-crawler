# Progression System

## ETAPA 00 — Visão e Definição do Produto

**Documento:** `PROGRESSION.md`

**Projeto:** Dungeon Crawler

**Versão:** 1.0

**Status:** Em Desenvolvimento

---

# 1. Objetivo do Documento

Este documento define o sistema de progressão do Dungeon Crawler.

A progressão será responsável por determinar como o jogador:

* Evolui o personagem;
* Obtém experiência;
* Sobe de nível;
* Desenvolve atributos;
* Desbloqueia habilidades;
* Evolui habilidades;
* Obtém equipamentos;
* Desenvolve diferentes builds;
* Enfrenta inimigos progressivamente mais fortes;
* Avança pelas dungeons;
* Obtém recompensas;
* Mantém progresso entre expedições.

O sistema deverá conectar:

```text
Character
    ↓
Progression
    ↓
Abilities
    ↓
Equipment
    ↓
Dungeon
    ↓
Enemies
    ↓
Rewards
    ↓
New Expedition
```

---

# 2. Princípio Fundamental

A progressão não será baseada apenas em números crescentes.

O objetivo é criar uma progressão que combine:

* Crescimento matemático;
* Escolhas do jogador;
* Estratégia;
* Construção de personagem;
* Recompensas;
* Risco;
* Exploração.

O jogador deverá sentir que está ficando mais poderoso, mas também deverá tomar decisões sobre **como** deseja evoluir.

---

# 3. Modelo de Progressão

O projeto utilizará um modelo de progressão híbrido.

```text
                    PROGRESSÃO
                         │
          ┌──────────────┼──────────────┐
          │              │              │
          ▼              ▼              ▼
      Personagem     Equipamentos    Dungeon
          │              │              │
       Level          Raridade       Andares
       Stats          Atributos      Desafios
       Skills         Modificadores  Bosses
          │              │              │
          └──────────────┼──────────────┘
                         │
                         ▼
                    EXPEDIÇÃO
                         │
                         ▼
                    RECOMPENSAS
                         │
                         ▼
                 NOVA EXPEDIÇÃO
```

Esse modelo possui duas dimensões principais:

### Progressão permanente

Permanece com o personagem.

### Progressão da expedição

Está relacionada à dungeon atual.

---

# 4. Progressão Permanente

A progressão permanente representa o desenvolvimento do personagem ao longo do jogo.

Inclui:

* Nível;
* XP;
* Atributos;
* Habilidades desbloqueadas;
* Evolução das habilidades;
* Equipamentos obtidos;
* Progresso relacionado ao personagem.

Exemplo:

```text
Character
│
├── Level
├── Experience
├── Attributes
├── Abilities
└── Equipment
```

---

# 5. Progressão da Expedição

A progressão da expedição representa o avanço dentro de uma dungeon específica.

Inclui:

* Andar atual;
* Salas exploradas;
* Encontros;
* Recompensas obtidas durante a expedição;
* Estado da dungeon;
* Progresso até o Boss.

Exemplo:

```text
Expedition
│
├── Current Floor
├── Explored Rooms
├── Encounters
├── Temporary Rewards
└── Dungeon Progress
```

---

# 6. Nível Máximo do MVP

O nível máximo definido para o MVP será:

```text
MAX LEVEL = 20
```

A escolha do nível 20 permite:

* Progressão significativa;
* Distribuição de habilidades;
* Evolução de atributos;
* Balanceamento controlável;
* Testes matemáticos;
* Espaço suficiente para diferentes builds.

O limite poderá ser aumentado posteriormente.

---

# 7. Sistema de Experiência

O personagem ganhará XP através de:

* Derrota de inimigos;
* Conclusão de encontros;
* Derrota de Elite;
* Derrota de Mini Boss;
* Derrota de Boss;
* Eventos específicos.

Fluxo:

```text
Encounter
    ↓
Reward
    ↓
Experience
    ↓
Progression
    ↓
Level Up
```

---

# 8. Curva de Experiência

A XP necessária para evolução utilizará uma curva não linear.

O objetivo é evitar uma progressão completamente linear.

A experiência acumulada poderá ser representada por uma função matemática.

Modelo inicial:

```text
XP(n) = 50 × n × (n + 1)
```

Onde:

* `n` representa o nível atual;
* `XP(n)` representa a referência de experiência acumulada.

A fórmula utiliza uma progressão baseada no produto de termos consecutivos.

Exemplo:

```text
Nível 1
XP = 50 × 1 × 2
XP = 100
```

```text
Nível 2
XP = 50 × 2 × 3
XP = 300
```

```text
Nível 3
XP = 50 × 3 × 4
XP = 600
```

```text
Nível 4
XP = 50 × 4 × 5
XP = 1.000
```

A fórmula poderá ser ajustada durante o balanceamento, mas a regra fundamental será:

> A progressão de XP deverá ser controlada por uma função centralizada, e não por valores independentes espalhados pelo código.

---

# 9. Experiência Acumulada x XP para o Próximo Nível

O sistema deverá distinguir:

```text
Total XP
```

de:

```text
XP necessária para o próximo nível
```

Conceitualmente:

```text
Current XP
     ↓
Progression Curve
     ↓
Current Level
     ↓
Next Level Requirement
```

Isso permitirá calcular:

* Nível atual;
* XP acumulada;
* XP necessária;
* XP restante;
* Percentual de progresso.

---

# 10. Easter Egg Matemático — Curva de XP

A curva de experiência é intencionalmente baseada em uma relação matemática.

O objetivo não é apenas criar números maiores.

A fórmula deverá produzir crescimento progressivo e previsível.

Esse detalhe será tratado como um **Mathematical Easter Egg** do projeto.

A implementação deverá manter a fórmula centralizada e testável.

---

# 11. Atributos

O personagem possuirá atributos que poderão ser desenvolvidos pelo jogador.

Atributos iniciais:

```text
Strength
Vitality
Agility
Intelligence
Luck
```

Cada atributo terá impacto diferente no personagem.

---

# 12. Distribuição de Pontos

Ao subir de nível, o jogador receberá pontos de atributo.

Modelo inicial:

```text
+5 Attribute Points
```

O jogador poderá distribuir os pontos livremente entre os atributos permitidos.

Exemplo:

```text
Level Up
   ↓
+5 Points
   ↓
Player Choice
   ├── Strength +3
   ├── Vitality +1
   └── Agility +1
```

Ou:

```text
Intelligence +5
```

Essa liberdade permitirá diferentes builds.

---

# 13. Atributos Automáticos

Nem todos os atributos derivados serão diretamente distribuídos pelo jogador.

Alguns valores serão calculados automaticamente.

Exemplo:

```text
Attributes
    ↓
Derived Statistics
```

Possíveis valores derivados:

* HP;
* Mana;
* Attack;
* Defense;
* Critical Chance;
* Critical Damage;
* Speed;
* Ability Power.

---

# 14. Fórmulas de Atributos Derivados

Os atributos derivados serão calculados por fórmulas centralizadas.

Exemplo conceitual:

```text
HP =
BaseHP
+
(Vitality × VitalityScaling)
+
(Level × LevelScaling)
```

A fórmula exata poderá ser ajustada durante o balanceamento.

O princípio é:

> Valores derivados não deverão ser armazenados arbitrariamente quando puderem ser calculados deterministicamente a partir do estado do personagem.

---

# 15. Easter Egg Matemático — Atributos Derivados

As estatísticas derivadas representarão outro Easter Egg matemático.

O jogador vê apenas:

```text
HP: 195
```

Mas internamente o sistema poderá calcular:

```text
Base
+
Attribute Scaling
+
Level Scaling
```

Isso cria uma camada de matemática de domínio escondida por trás das regras do jogo.

---

# 16. Relação entre Classes e Atributos

As classes poderão possuir diferentes pesos de atributos.

Exemplo conceitual:

```text
Warrior
→ Strength
→ Vitality

Mage
→ Intelligence
→ Luck

Rogue
→ Agility
→ Luck
```

Isso não significa que os demais atributos serão bloqueados.

O jogador continuará podendo construir personagens diferentes.

---

# 17. Builds

O sistema permitirá diferentes estilos de construção.

Exemplos:

### Warrior Tank

```text
High Vitality
High Defense
Medium Strength
```

### Warrior Damage

```text
High Strength
Medium Agility
Medium Vitality
```

### Mage

```text
High Intelligence
Medium Luck
Low Strength
```

### Rogue

```text
High Agility
High Luck
Medium Strength
```

As builds surgirão principalmente através das escolhas do jogador.

---

# 18. Habilidades

As habilidades serão desbloqueadas progressivamente.

O sistema combinará:

```text
Level Unlock
+
Ability Evolution
```

Exemplo:

```text
Level 1
→ Basic Attack

Level 3
→ Skill A

Level 5
→ Skill B

Level 8
→ Skill C
```

---

# 19. Evolução de Habilidades

Algumas habilidades poderão possuir níveis.

Exemplo:

```text
Fireball I
   ↓
Fireball II
   ↓
Fireball III
```

A evolução poderá melhorar:

* Dano;
* Alcance;
* Área;
* Custo;
* Cooldown;
* Efeitos.

---

# 20. Escolha do Jogador

O jogador não deverá necessariamente desbloquear tudo imediatamente.

Algumas habilidades poderão exigir escolhas.

Exemplo:

```text
Level Up
   ↓
Choice
   ├── Offensive Skill
   └── Defensive Skill
```

Isso aumenta a diversidade de builds.

---

# 21. Fórmula de Dano de Habilidade

O dano de uma habilidade poderá considerar:

```text
Base Damage
+
Attribute Scaling
+
Ability Level Scaling
+
Equipment Modifier
```

Exemplo conceitual:

```text
Damage =
BaseDamage
+
(Intelligence × Scaling)
+
(AbilityLevel × Growth)
```

A fórmula deverá permanecer centralizada no sistema de combate/progressão.

---

# 22. Easter Egg Matemático — Scaling de Habilidades

O crescimento das habilidades será outro elemento matemático do projeto.

Uma habilidade não precisará possuir apenas:

```text
Fireball = 50
```

Ela poderá possuir:

```text
Base Damage
+
Scaling
+
Growth
```

Isso permite que o mesmo sistema funcione para diferentes níveis de personagem.

---

# 23. Equipamentos

Equipamentos participarão diretamente da progressão.

Tipos iniciais:

```text
Weapon
Armor
Accessory
```

Cada equipamento poderá fornecer:

* Atributos;
* Modificadores;
* Efeitos;
* Raridade.

---

# 24. Raridade

O sistema utilizará inicialmente:

```text
Common
Uncommon
Rare
Epic
Legendary
```

A raridade influenciará a qualidade do equipamento.

---

# 25. Atributos de Equipamentos

Exemplo:

```text
Iron Sword
Common

+5 Strength
```

Ou:

```text
Flame Sword
Rare

+12 Strength
+Fire Damage
```

Os equipamentos poderão contribuir para diferentes builds.

---

# 26. Modificadores

Equipamentos poderão possuir modificadores.

Exemplo:

```text
+Attack
+Defense
+Critical Chance
+Fire Damage
+Health
```

Modificadores especiais poderão ser adicionados posteriormente.

---

# 27. Loot

As recompensas poderão ser determinadas por tabelas probabilísticas.

Exemplo:

```text
Common      → 60%
Uncommon    → 25%
Rare        → 10%
Epic        → 4%
Legendary   → 1%
```

A soma deverá sempre respeitar:

```text
100%
```

---

# 28. Easter Egg Matemático — Probabilidade

O sistema de loot utilizará conceitos de probabilidade.

A distribuição das raridades não será apenas uma lista arbitrária.

O sistema poderá calcular a seleção através de uma distribuição de pesos.

Exemplo:

```text
60 + 25 + 10 + 4 + 1 = 100
```

Posteriormente, atributos como Luck poderão modificar determinadas probabilidades.

---

# 29. Luck

O atributo `Luck` poderá influenciar:

* Chance de loot;
* Qualidade do loot;
* Chance de efeitos especiais;
* Eventos raros.

Entretanto, `Luck` deverá possuir limites para evitar que uma build quebre completamente o sistema.

---

# 30. Easter Egg Matemático — Limites

Os sistemas de progressão deverão utilizar limites matemáticos.

Exemplo:

```text
Critical Chance ≤ Maximum Critical Chance
```

```text
Loot Chance ≤ Maximum Loot Chance
```

```text
Scaling ≤ Maximum Scaling
```

Isso evita crescimento infinito.

O conceito de **clamping** será importante durante a implementação.

---

# 31. Progressão da Dungeon

A dungeon possuirá progressão por andares.

Modelo:

```text
Floor 1
   ↓
Floor 2
   ↓
Floor 3
   ↓
...
   ↓
Boss
```

Cada andar poderá aumentar:

* Dificuldade;
* Recompensa;
* Complexidade;
* Variedade de inimigos.

---

# 32. Escalonamento dos Inimigos

O poder dos inimigos deverá acompanhar:

```text
Player Level
+
Dungeon Floor
```

Modelo conceitual:

```text
Enemy Power =
Base Power
× Level Multiplier
× Floor Multiplier
```

Essa fórmula deverá ser centralizada.

---

# 33. Easter Egg Matemático — Enemy Scaling

O sistema de inimigos utilizará crescimento matemático para evitar que a dificuldade dependa de valores manuais para cada andar.

Conceitualmente:

```text
Player Level
       +
Dungeon Floor
       ↓
Scaling Function
       ↓
Enemy Power
```

Assim, o jogo possui uma relação matemática entre:

* Progressão;
* Dungeon;
* Inimigos.

---

# 34. Relação entre Progressão e Dungeon

A progressão deverá funcionar como um sistema integrado.

```text
Character Level
      │
      ▼
Dungeon Difficulty
      │
      ▼
Enemy Scaling
      │
      ▼
Combat
      │
      ▼
Rewards
      │
      ▼
Character Progression
```

Isso cria um ciclo fechado.

---

# 35. Recompensas

As recompensas poderão incluir:

* XP;
* Ouro;
* Equipamentos;
* Materiais;
* Itens;
* Recompensas especiais.

A qualidade das recompensas deverá acompanhar o risco.

---

# 36. Relação Risco x Recompensa

O sistema deverá buscar uma relação:

```text
Maior risco
     ↓
Maior dificuldade
     ↓
Maior recompensa potencial
```

Exemplo:

```text
Common Enemy
→ baixa recompensa

Elite
→ recompensa superior

Mini Boss
→ recompensa alta

Boss
→ recompensa especial
```

---

# 37. Progressão por Categoria de Inimigo

| Categoria |         XP |       Ouro | Loot      |
| --------- | ---------: | ---------: | --------- |
| Common    |      Baixa |      Baixo | Comum     |
| Elite     |      Média |      Médio | Melhor    |
| Mini Boss |       Alta |       Alta | Raro      |
| Boss      | Muito alta | Muito alta | Exclusivo |

Os valores exatos serão definidos durante o balanceamento.

---

# 38. Morte

A morte não deverá apagar completamente o progresso permanente.

Modelo escolhido:

```text
Morte
 ↓
Fim da Expedição
 ↓
Perda/risco sobre progresso temporário
 ↓
Retorno
 ↓
Personagem mantém progresso permanente
```

Isso cria risco sem tornar o jogo excessivamente punitivo.

---

# 39. Progressão Permanente x Temporária

| Elemento                | Permanente | Temporário |
| ----------------------- | ---------: | ---------: |
| Level                   |          ✅ |          ❌ |
| XP                      |          ✅ |          ❌ |
| Attributes              |          ✅ |          ❌ |
| Abilities               |          ✅ |          ❌ |
| Equipment obtido        |          ✅ |          ❌ |
| Floor atual             |          ❌ |          ✅ |
| Salas exploradas        |          ❌ |          ✅ |
| Estado da expedição     |          ❌ |          ✅ |
| Recompensas temporárias |          ❌ |          ✅ |

Essa separação deverá ser respeitada na arquitetura.

---

# 40. Game Loop de Progressão

O ciclo principal será:

```text
                 EXPLORE
                    ↓
                 ENCOUNTER
                    ↓
                  COMBAT
                    ↓
              DEFEAT ENEMY
                    ↓
                REWARDS
             ┌──────┼──────┐
             │      │      │
             ▼      ▼      ▼
            XP     GOLD    LOOT
             │             │
             ▼             ▼
          LEVEL UP      EQUIPMENT
             │             │
             └──────┬──────┘
                    ▼
                STRONGER
                    ↓
             DEEPER DUNGEON
                    ↓
                  BOSS
                    ↓
              NEW EXPEDITION
                    │
                    └──────────► LOOP
```

---

# 41. Easter Eggs Matemáticos

Esta seção registra oficialmente os Easter Eggs matemáticos do projeto.

O objetivo é utilizar matemática de maneira legítima dentro das regras do jogo.

## 41.1 Curva de XP

Utilização de função não linear para controlar a evolução.

```text
XP(n) = 50 × n × (n + 1)
```

---

## 41.2 Scaling de atributos

Atributos derivados serão calculados por funções.

```text
DerivedStat =
Base
+
Attribute × Scaling
+
Level × Growth
```

---

## 41.3 Scaling de dano

Habilidades poderão utilizar:

```text
Damage =
BaseDamage
+
AttributeScaling
+
AbilityGrowth
+
EquipmentModifier
```

---

## 41.4 Probabilidade

Loot será baseado em pesos probabilísticos.

```text
Common      → 60%
Uncommon    → 25%
Rare        → 10%
Epic        → 4%
Legendary   → 1%
```

---

## 41.5 Limites

Valores críticos possuirão limites superiores.

Exemplo:

```text
CriticalChance ≤ MaximumCriticalChance
```

---

## 41.6 Scaling de inimigos

A dificuldade utilizará uma função de escalonamento.

```text
EnemyPower =
BasePower
× LevelMultiplier
× FloorMultiplier
```

---

## 41.7 Determinismo

As fórmulas deverão produzir resultados previsíveis quando receberem os mesmos parâmetros.

Isso permitirá testes como:

```text
Same Input
     ↓
Same Formula
     ↓
Same Output
```

Essa característica será especialmente importante nos testes automatizados.

---

# 42. Princípio Matemático do Projeto

A matemática não será utilizada apenas como elemento decorativo.

Cada fórmula deverá possuir:

1. Uma finalidade de design;
2. Entradas claramente definidas;
3. Resultado determinístico quando aplicável;
4. Limites;
5. Testes;
6. Documentação.

A regra será:

> **Se uma fórmula existe, ela precisa resolver um problema real do jogo.**

---

# 43. Centralização das Fórmulas

As fórmulas não deverão ficar espalhadas pelas entidades.

Conceitualmente:

```text
Progression
│
└── Formulas
    │
    ├── ExperienceCurve
    ├── AttributeScaling
    ├── DamageScaling
    ├── EnemyScaling
    └── LootProbability
```

Isso facilita:

* Testes;
* Balanceamento;
* Manutenção;
* Evolução;
* Auditoria das regras.

---

# 44. Preparação para Testes

As fórmulas deverão ser projetadas para permitir testes unitários.

Exemplo conceitual:

```text
ExperienceCurve
Input: Level 5
Output: Expected XP
```

```text
DamageFormula
Input: Stats + Ability
Output: Expected Damage
```

```text
EnemyScaling
Input: Level + Floor
Output: Expected Power
```

```text
LootProbability
Input: Roll + Weights
Output: Expected Rarity
```

Esses testes serão implementados posteriormente na **ETAPA 12 — Testes**.

---

# 45. Balanceamento

Os valores definidos neste documento representam regras iniciais.

O balanceamento será refinado durante a implementação e testes.

Poderão ser ajustados:

* Multiplicadores;
* Curvas;
* Valores de XP;
* Pontos de atributo;
* Dano;
* Vida;
* Recompensas;
* Probabilidades.

Entretanto, alterações deverão preservar os princípios definidos neste documento.

---

# 46. Escopo do MVP

O MVP deverá possuir:

### Personagem

* [x] Nível;
* [x] XP;
* [x] Limite de nível;
* [x] Atributos;
* [x] Pontos distribuíveis;
* [x] Atributos derivados;
* [x] Builds.

### Habilidades

* [x] Desbloqueio por nível;
* [x] Evolução de habilidades;
* [x] Escolhas do jogador;
* [x] Scaling.

### Equipamentos

* [x] Equipamentos;
* [x] Raridade;
* [x] Atributos;
* [x] Modificadores.

### Dungeon

* [x] Progressão por andares;
* [x] Escalonamento;
* [x] Elite;
* [x] Mini Boss;
* [x] Boss.

### Recompensas

* [x] XP;
* [x] Ouro;
* [x] Loot;
* [x] Probabilidade.

### Matemática

* [x] Curva de XP;
* [x] Scaling;
* [x] Atributos derivados;
* [x] Dano;
* [x] Probabilidade;
* [x] Limites.

---

# 47. Fora do MVP

Os seguintes recursos poderão ser adicionados posteriormente:

* [ ] Meta-progressão complexa;
* [ ] Árvores extensas de habilidades;
* [ ] Classes avançadas;
* [ ] Prestígio;
* [ ] Sistema de talentos complexo;
* [ ] Equipamentos com dezenas de modificadores;
* [ ] Sets;
* [ ] Crafting avançado;
* [ ] Sistema econômico complexo;
* [ ] Leaderboards;
* [ ] Progressão online.

---

# 48. Princípios de Design

O sistema de progressão deverá seguir:

### Escolha

O jogador deverá possuir decisões significativas.

### Clareza

O jogador deverá compreender por que ficou mais forte.

### Consistência

As fórmulas deverão produzir resultados coerentes.

### Balanceamento

Nenhuma estratégia deverá dominar completamente o sistema.

### Extensibilidade

Novas regras deverão poder ser adicionadas sem reescrever o núcleo.

### Testabilidade

As fórmulas deverão ser testáveis isoladamente.

---

# 49. Critérios de Validação

A FASE 08 será considerada concluída quando:

* O nível máximo estiver definido;
* A curva de XP estiver definida;
* XP acumulada e XP necessária estiverem diferenciadas;
* Atributos estiverem definidos;
* Distribuição de pontos estiver definida;
* Atributos derivados estiverem definidos;
* Builds estiverem definidas;
* Habilidades estiverem definidas;
* Desbloqueios estiverem definidos;
* Evolução de habilidades estiver definida;
* Equipamentos estiverem definidos;
* Raridades estiverem definidas;
* Modificadores estiverem definidos;
* Loot estiver definido;
* Probabilidades estiverem definidas;
* Luck estiver definida;
* Dungeon progression estiver definida;
* Enemy scaling estiver definido;
* Recompensas estiverem definidas;
* Morte estiver definida;
* Progressão permanente estiver definida;
* Progressão temporária estiver definida;
* Game Loop estiver definido;
* Easter Eggs matemáticos estiverem documentados;
* Fórmulas estiverem centralizadas conceitualmente;
* Escopo do MVP estiver definido;
* Fora do MVP estiver definido.

---

# 50. Checking da Fase

| Item                                 | Status |
| ------------------------------------ | ------ |
| Objetivo da progressão definido      | ✅      |
| Modelo híbrido definido              | ✅      |
| Progressão permanente definida       | ✅      |
| Progressão temporária definida       | ✅      |
| Nível máximo definido                | ✅      |
| XP definida                          | ✅      |
| Curva não linear definida            | ✅      |
| Atributos definidos                  | ✅      |
| Pontos de atributo definidos         | ✅      |
| Atributos derivados definidos        | ✅      |
| Builds definidas                     | ✅      |
| Habilidades definidas                | ✅      |
| Evolução de habilidades definida     | ✅      |
| Equipamentos definidos               | ✅      |
| Raridades definidas                  | ✅      |
| Modificadores definidos              | ✅      |
| Loot definido                        | ✅      |
| Probabilidade definida               | ✅      |
| Luck definida                        | ✅      |
| Progressão da dungeon definida       | ✅      |
| Enemy scaling definido               | ✅      |
| Recompensas definidas                | ✅      |
| Morte definida                       | ✅      |
| Game Loop definido                   | ✅      |
| Fórmulas matemáticas definidas       | ✅      |
| Easter Eggs matemáticos documentados | ✅      |
| Preparação para testes definida      | ✅      |
| Balanceamento definido               | ✅      |
| Escopo do MVP definido               | ✅      |
| Fora do MVP definido                 | ✅      |
| Critérios de validação definidos     | ✅      |

---

# 51. Decisões Arquiteturais Registradas

As seguintes decisões passam a fazer parte oficialmente das especificações do projeto:

### 1. Progressão híbrida

O jogo possuirá progressão permanente e progressão temporária de expedição.

### 2. Nível máximo do MVP

```text
20
```

### 3. Progressão não linear

A experiência utilizará uma curva matemática.

### 4. Escolha do jogador

O jogador distribuirá pontos de atributo e tomará decisões sobre desenvolvimento.

### 5. Atributos derivados

Valores como HP, dano e outros poderão ser calculados matematicamente.

### 6. Scaling

Inimigos e habilidades utilizarão fórmulas de escalonamento.

### 7. Loot probabilístico

A obtenção de itens utilizará pesos/probabilidades.

### 8. Fórmulas centralizadas

As regras matemáticas deverão ser isoladas e testáveis.

### 9. Easter Eggs matemáticos

A matemática fará parte explícita da identidade técnica do projeto.

### 10. Meta-progressão

Será preparada arquiteturalmente, mas permanecerá fora do MVP.

---

# 52. Relação com Outras Áreas

A progressão depende diretamente de:

```text
CHARACTERS.md
       │
       ▼
PROGRESSION.md
       │
 ┌─────┼─────┐
 ▼     ▼     ▼
COMBAT ENEMIES DUNGEON
       │
       ▼
    REWARDS
       │
       ▼
  NEW EXPEDITION
```

Documentos relacionados:

```text
docs/product/CHARACTERS.md
docs/product/ENEMIES.md
docs/product/WORLD_AND_DUNGEON.md
docs/product/GAME_MECHANICS.md
```

---

# 53. Status da Fase

**Documento:** `PROGRESSION.md`

**Etapa:** ETAPA 00 — Visão e Definição do Produto

**Fase:** FASE 08 — Progressão

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

✅ FASE 08 — Progressão
   └── PROGRESSION.md

⬜ FASE 09 — MVP
⬜ FASE 10 — Requisitos Funcionais e Não Funcionais

⬜ CHECKING FINAL DA ETAPA 00
```

---

# 55. Conclusão

A FASE 08 estabelece o sistema de progressão do Dungeon Crawler.

O sistema combina:

```text
Escolha do jogador
        +
Progressão matemática
        +
Equipamentos
        +
Habilidades
        +
Dungeon
        +
Inimigos
        +
Recompensas
        ↓
     PROGRESSÃO
```

A progressão será baseada em escolhas significativas e em regras matemáticas centralizadas.

Os **Mathematical Easter Eggs** fazem parte oficialmente do projeto e deverão permanecer documentados durante a implementação.

O objetivo é que o sistema seja simultaneamente:

* Divertido;
* Balanceável;
* Extensível;
* Determinístico quando necessário;
* Testável;
* Arquiteturalmente consistente.

Com a conclusão deste documento:

**FASE 08 — PROGRESSÃO: 🟢 APROVADA EM NÍVEL CONCEITUAL.**
