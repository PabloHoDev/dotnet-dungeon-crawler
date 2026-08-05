# Requirements Specification

**Projeto:** Dungeon Crawler
**Documento:** `docs/product/REQUIREMENTS.md`
**Etapa:** ETAPA 00 — Visão e Definição do Produto
**Fase:** FASE 10 — Requisitos Funcionais e Não Funcionais
**Versão:** 1.0
**Status:** Final — Aguardando Checking

---

# 1. Objetivo

Este documento define os requisitos funcionais e não funcionais do projeto **Dungeon Crawler**.

Seu objetivo é transformar as decisões tomadas durante a definição do produto em requisitos:

* identificáveis;
* rastreáveis;
* verificáveis;
* testáveis;
* alinhados ao MVP;
* alinhados à arquitetura planejada.

Este documento será utilizado como referência durante as etapas de arquitetura, implementação, testes, qualidade e release.

---

# 2. Escopo

Os requisitos deste documento contemplam a primeira versão jogável do projeto, definida no documento:

`docs/product/MVP.md`

O escopo considera:

* criação de personagens;
* classes;
* atributos;
* habilidades;
* combate;
* inimigos;
* dungeon;
* exploração;
* inventário;
* equipamentos;
* loot;
* progressão;
* matemática aplicada;
* Boss;
* vitória;
* derrota;
* Golden Path;
* requisitos arquiteturais;
* requisitos de qualidade.

---

# 3. Convenções

Os requisitos funcionais serão identificados como:

```text
RF-XXX
```

Os requisitos não funcionais serão identificados como:

```text
RNF-XXX
```

### RF

Define **o que o sistema deve fazer**.

### RNF

Define **como o sistema deve se comportar ou quais características deve possuir**.

---

# 4. Prioridade

Cada requisito poderá possuir uma prioridade:

| Prioridade | Significado            |
| ---------- | ---------------------- |
| **P0**     | Obrigatório para o MVP |
| **P1**     | Importante para o MVP  |
| **P2**     | Desejável              |
| **FUTURE** | Fora do MVP            |

---

# 5. Requisitos Funcionais

## 5.1 Personagem

### RF-001 — Criação de personagem

**Prioridade:** P0

O sistema deve permitir ao jogador criar um personagem.

O personagem deverá possuir, no mínimo:

* Identidade;
* Classe;
* Level;
* XP;
* Atributos;
* Habilidades;
* Inventário;
* Equipamentos.

**Critério de aceite:**

O jogador consegue criar um personagem válido e iniciar uma expedição.

---

### RF-002 — Seleção de classe

**Prioridade:** P0

O sistema deve permitir ao jogador selecionar uma classe disponível.

Classes do MVP:

* Warrior;
* Mage;
* Rogue.

A classe deverá influenciar as características e o estilo de gameplay do personagem.

**Critério de aceite:**

O jogador consegue selecionar uma das classes disponíveis e o personagem recebe a configuração correspondente.

---

### RF-003 — Atributos

**Prioridade:** P0

O sistema deve manter os atributos:

```text
Strength
Vitality
Agility
Intelligence
Luck
```

Os atributos deverão participar dos cálculos aplicáveis aos sistemas do jogo.

**Critério de aceite:**

O personagem possui valores válidos para todos os atributos.

---

### RF-004 — Progressão de nível

**Prioridade:** P0

O sistema deve permitir que o personagem evolua através do ganho de XP.

O nível máximo definido para o MVP é:

```text
Level 20
```

**Critério de aceite:**

Ao atingir a quantidade necessária de XP, o personagem evolui corretamente até o limite estabelecido.

---

### RF-005 — Ganho de XP

**Prioridade:** P0

O sistema deve permitir que o personagem receba XP através de eventos de gameplay.

Fontes iniciais:

* Combates;
* Elite;
* Mini Boss;
* Boss;
* Recompensas.

**Critério de aceite:**

Após uma fonte válida de XP, o sistema atualiza corretamente o progresso do personagem.

---

### RF-006 — Distribuição de atributos

**Prioridade:** P0

O sistema deve permitir ao jogador distribuir pontos de atributo durante a progressão.

A escolha deverá permanecer sob responsabilidade do jogador.

**Critério de aceite:**

O jogador consegue distribuir pontos válidos sem ultrapassar as regras estabelecidas.

---

### RF-007 — Habilidades

**Prioridade:** P0

O sistema deve permitir:

* Desbloquear habilidades;
* Utilizar habilidades;
* Evoluir habilidades;
* Aplicar seus efeitos.

Habilidades poderão possuir requisitos de nível.

**Critério de aceite:**

Uma habilidade válida pode ser desbloqueada, utilizada e processada corretamente.

---

# 6. Requisitos de Combate

### RF-008 — Início de combate

**Prioridade:** P0

O sistema deve iniciar um combate quando o jogador encontrar um inimigo que resulte em confronto.

**Critério de aceite:**

Um encontro válido cria um estado de combate.

---

### RF-009 — Ações do jogador

**Prioridade:** P0

O jogador deve poder realizar ações disponíveis durante o combate.

Exemplos:

* Ataque básico;
* Habilidades;
* Outras ações definidas pelo sistema.

**Critério de aceite:**

Uma ação válida é processada pelo sistema de combate.

---

### RF-010 — Ações do inimigo

**Prioridade:** P0

O inimigo deve possuir comportamento capaz de realizar ações durante o combate.

**Critério de aceite:**

O inimigo realiza uma ação válida de acordo com seu comportamento.

---

### RF-011 — Cálculo de dano

**Prioridade:** P0

O sistema deve calcular o dano considerando os modificadores aplicáveis.

Podem participar do cálculo:

* Atributos;
* Habilidades;
* Equipamentos;
* Modificadores;
* Estado do combatente.

**Critério de aceite:**

Dado um conjunto conhecido de valores, o sistema retorna um resultado de dano válido.

---

### RF-012 — Critical Hit

**Prioridade:** P0

O sistema deve permitir a ocorrência de ataques críticos.

A probabilidade deverá respeitar os limites definidos pelo sistema.

**Critério de aceite:**

O sistema consegue determinar corretamente se um ataque resulta em Critical Hit.

---

### RF-013 — Vitória em combate

**Prioridade:** P0

Ao derrotar um inimigo, o sistema deve:

1. Encerrar o combate;
2. Registrar a vitória;
3. Conceder XP;
4. Processar recompensas;
5. Permitir continuidade da exploração.

---

### RF-014 — Derrota em combate

**Prioridade:** P0

Quando o personagem atingir um estado de derrota, o sistema deve reconhecer o resultado.

**Critério de aceite:**

O combate é encerrado e a consequência correspondente é aplicada.

---

# 7. Requisitos de Inimigos

### RF-015 — Tipos de inimigos

**Prioridade:** P0

O MVP deverá possuir:

```text
Goblin
Skeleton
Orc
Elite Enemy
Mini Boss
Boss
```

---

### RF-016 — Escalonamento de inimigos

**Prioridade:** P0

O poder dos inimigos deverá considerar fatores como:

* Level do jogador;
* Floor da dungeon;
* Tipo do inimigo;
* Modificadores.

**Critério de aceite:**

O sistema produz valores coerentes de dificuldade para diferentes níveis e floors.

---

### RF-017 — Elite Enemy

**Prioridade:** P0

O sistema deve permitir a existência de inimigos Elite.

Um Elite poderá possuir:

* Atributos superiores;
* Modificadores;
* Comportamento diferenciado;
* Recompensas superiores.

---

### RF-018 — Mini Boss

**Prioridade:** P0

O sistema deve possuir pelo menos um Mini Boss no MVP.

---

### RF-019 — Boss

**Prioridade:** P0

O sistema deve possuir um Boss final.

O Boss deverá representar o objetivo principal da expedição.

---

# 8. Requisitos de Dungeon

### RF-020 — Iniciar expedição

**Prioridade:** P0

O jogador deve conseguir iniciar uma expedição.

---

### RF-021 — Floors

**Prioridade:** P0

A dungeon do MVP deverá possuir:

```text
5 Floors
```

---

### RF-022 — Exploração

**Prioridade:** P0

O jogador deve conseguir explorar a dungeon.

A exploração deverá permitir progressão entre salas e floors.

---

### RF-023 — Encontros

**Prioridade:** P0

A exploração deverá permitir encontros com:

* Inimigos;
* Elite;
* Mini Boss;
* Eventos definidos posteriormente.

---

### RF-024 — Progressão da dungeon

**Prioridade:** P0

O jogador deverá conseguir avançar progressivamente até o encontro final.

---

# 9. Requisitos de Inventário

### RF-025 — Armazenamento

**Prioridade:** P0

O sistema deve permitir armazenar itens no inventário.

---

### RF-026 — Visualização

**Prioridade:** P0

O jogador deve conseguir visualizar os itens disponíveis.

---

### RF-027 — Equipar item

**Prioridade:** P0

O jogador deve conseguir equipar equipamentos compatíveis.

---

### RF-028 — Desequipar item

**Prioridade:** P0

O jogador deve conseguir desequipar equipamentos.

---

# 10. Requisitos de Equipamentos

### RF-029 — Tipos de equipamento

**Prioridade:** P0

O MVP deverá suportar:

```text
Weapon
Armor
Accessory
```

---

### RF-030 — Raridade

**Prioridade:** P0

Os equipamentos deverão possuir uma das seguintes raridades:

```text
Common
Uncommon
Rare
Epic
Legendary
```

---

### RF-031 — Modificadores

**Prioridade:** P0

Equipamentos poderão modificar atributos ou fornecer efeitos.

---

# 11. Requisitos de Loot e Recompensas

### RF-032 — Geração de loot

**Prioridade:** P0

O sistema deve permitir gerar recompensas após determinados encontros.

---

### RF-033 — Probabilidade de loot

**Prioridade:** P0

O sistema deverá utilizar probabilidades para determinar possíveis recompensas.

Configuração inicial:

```text
Common       60%
Uncommon     25%
Rare         10%
Epic          4%
Legendary     1%
```

A soma das probabilidades deverá representar:

```text
100%
```

---

### RF-034 — Influência de Luck

**Prioridade:** P0

O atributo `Luck` poderá influenciar probabilidades relacionadas ao sistema de loot.

A implementação deverá respeitar os limites definidos pelo sistema.

---

# 12. Requisitos de Morte

### RF-035 — Morte do personagem

**Prioridade:** P0

O sistema deve detectar quando o personagem entra em estado de morte.

---

### RF-036 — Encerramento da expedição

**Prioridade:** P0

A morte do personagem deverá encerrar a expedição atual.

---

### RF-037 — Preservação da progressão

**Prioridade:** P0

A morte não deverá apagar a progressão permanente definida pelo sistema.

---

# 13. Requisitos de Vitória

### RF-038 — Derrota do Boss

**Prioridade:** P0

O sistema deve reconhecer a derrota do Boss.

---

### RF-039 — Conclusão da expedição

**Prioridade:** P0

Após a derrota do Boss, o sistema deve considerar a expedição concluída.

---

# 14. Requisitos Matemáticos

### RF-040 — Cálculo de XP

**Prioridade:** P0

O sistema deverá utilizar uma curva de XP não linear.

Fórmula inicial:

```text
XP(n) = 50 × n × (n + 1)
```

A fórmula poderá ser refinada durante o balanceamento sem alterar o requisito de existência de uma curva progressiva não linear.

---

### RF-041 — Scaling

**Prioridade:** P0

O sistema deverá possuir cálculos para elementos como:

* Dano;
* Poder dos inimigos;
* Progressão;
* Recompensas;
* Probabilidades.

---

### RF-042 — Easter Eggs matemáticos

**Prioridade:** P0

O sistema deverá possuir Easter Eggs matemáticos documentados em:

`docs/product/PROGRESSION.md`

Eles deverão representar regras reais do domínio.

Não deverão existir apenas como elementos visuais ou comentários no código.

---

# 15. Golden Path

### RF-043 — Execução do Golden Path

**Prioridade:** P0

O MVP deverá permitir executar o seguinte fluxo:

```text
Criar Personagem
       ↓
Escolher Classe
       ↓
Entrar na Dungeon
       ↓
Explorar
       ↓
Combater
       ↓
Receber XP
       ↓
Evoluir
       ↓
Equipar Item
       ↓
Avançar
       ↓
Elite
       ↓
Mini Boss
       ↓
Boss
       ↓
Vitória
```

O Golden Path será utilizado como cenário principal de integração.

---

# 16. Requisitos Não Funcionais

## RNF-001 — Linguagem

**Prioridade:** P0

O projeto deverá ser desenvolvido em **C#**.

---

## RNF-002 — Engine

**Prioridade:** P0

A camada de apresentação do jogo deverá utilizar **Unity**.

---

## RNF-003 — Independência do domínio

**Prioridade:** P0

O núcleo de domínio não deverá depender diretamente da Unity.

```text
Domain
   ↓
independente
   ↓
Unity
```

---

## RNF-004 — Testabilidade

**Prioridade:** P0

As principais regras de negócio deverão poder ser testadas sem depender da interface gráfica.

---

## RNF-005 — Programação Orientada a Objetos

**Prioridade:** P0

O sistema deverá utilizar princípios de POO de forma coerente.

---

## RNF-006 — SOLID

**Prioridade:** P0

Os princípios SOLID deverão orientar a construção do sistema quando forem aplicáveis.

Não deverão ser utilizados artificialmente.

---

## RNF-007 — Coesão

**Prioridade:** P0

Componentes deverão possuir responsabilidades claras e relacionadas.

---

## RNF-008 — Baixo acoplamento

**Prioridade:** P0

Os principais componentes do domínio deverão possuir baixo acoplamento.

---

## RNF-009 — Extensibilidade

**Prioridade:** P0

A arquitetura deverá permitir a inclusão futura de:

* Classes;
* Inimigos;
* Habilidades;
* Itens;
* Floors;
* Sistemas.

sem exigir alterações extensas no núcleo existente.

---

## RNF-010 — Manutenibilidade

**Prioridade:** P0

O código deverá ser estruturado para facilitar:

* Leitura;
* Manutenção;
* Testes;
* Evolução.

---

## RNF-011 — Determinismo testável

**Prioridade:** P0

Sistemas probabilísticos deverão permitir mecanismos para execução de testes determinísticos.

Aplicável principalmente a:

* Loot;
* Critical;
* Random encounters;
* Dungeon variation.

---

## RNF-012 — Centralização de regras matemáticas

**Prioridade:** P0

As principais fórmulas do gameplay deverão ser centralizadas em componentes apropriados.

As regras não deverão ser espalhadas aleatoriamente pelo código.

---

## RNF-013 — Configurabilidade

**Prioridade:** P1

Valores de balanceamento deverão ser configuráveis sempre que possível.

Exemplos:

```text
XP
Damage
Enemy Scaling
Loot Probability
Critical Chance
```

---

## RNF-014 — Tratamento de estados inválidos

**Prioridade:** P0

O sistema deverá tratar estados inválidos de forma previsível.

Exemplos:

* Item incompatível;
* Atributo inválido;
* Level inválido;
* Ação impossível;
* Inventário cheio.

---

## RNF-015 — Observabilidade

**Prioridade:** P1

O sistema deverá produzir informações suficientes para diagnosticar problemas durante o desenvolvimento.

A observabilidade será aprofundada posteriormente na:

`ETAPA 13 — Qualidade e Observabilidade`

---

## RNF-016 — Versionamento

**Prioridade:** P0

O projeto deverá utilizar Git para controle de versão.

---

## RNF-017 — Documentação

**Prioridade:** P0

Decisões importantes de produto, arquitetura e implementação deverão ser documentadas.

---

## RNF-018 — Testes automatizados

**Prioridade:** P0

As principais regras de domínio deverão possuir testes automatizados.

---

# 17. Requisitos de Arquitetura

## RNF-019 — Domain Core

**Prioridade:** P0

O núcleo deverá representar as principais regras de domínio:

```text
Character
Combat
Enemy
Dungeon
Inventory
Equipment
Progression
Reward
```

---

## RNF-020 — Separação de camadas

**Prioridade:** P0

A arquitetura deverá permitir separação conceitual entre:

```text
Domain
Application
Infrastructure
Presentation
```

---

## RNF-021 — Direção das dependências

**Prioridade:** P0

As dependências deverão respeitar os limites arquiteturais definidos posteriormente.

O domínio não deverá depender diretamente de detalhes de apresentação.

---

# 18. Requisitos de Qualidade

## RNF-022 — Legibilidade

**Prioridade:** P0

O código deverá ser compreensível por outro desenvolvedor.

---

## RNF-023 — Consistência

**Prioridade:** P0

Nomenclatura, estruturas e padrões deverão seguir convenções consistentes.

---

## RNF-024 — Testes independentes

**Prioridade:** P0

Sempre que possível, os testes do domínio deverão executar sem necessidade de iniciar a Unity.

---

## RNF-025 — Reprodutibilidade

**Prioridade:** P0

O projeto deverá possuir documentação suficiente para permitir que outro desenvolvedor:

1. Clone o repositório;
2. Configure o ambiente;
3. Execute o projeto;
4. Execute os testes;
5. Gere uma build.

---

# 19. Requisitos Fora do MVP

Os seguintes requisitos não fazem parte da primeira versão:

| Recurso                        | Status |
| ------------------------------ | ------ |
| Multiplayer                    | FUTURE |
| PvP                            | FUTURE |
| Online                         | FUTURE |
| Guildas                        | FUTURE |
| Crafting avançado              | FUTURE |
| Quests complexas               | FUTURE |
| Classes avançadas              | FUTURE |
| Meta Progression complexa      | FUTURE |
| Prestígio                      | FUTURE |
| Leaderboards                   | FUTURE |
| Economia complexa              | FUTURE |
| Centenas de inimigos           | FUTURE |
| Centenas de itens              | FUTURE |
| Procedural Generation avançada | FUTURE |
| Sistemas sociais               | FUTURE |
| Monetização                    | FUTURE |

Esses itens não deverão ser incorporados ao MVP sem uma revisão explícita do escopo.

---

# 20. Matriz de Rastreabilidade

A matriz relaciona os requisitos aos principais sistemas do projeto.

| Requisitos        | Sistema                   |
| ----------------- | ------------------------- |
| RF-001 → RF-007   | Character / Progression   |
| RF-008 → RF-014   | Combat                    |
| RF-015 → RF-019   | Enemies                   |
| RF-020 → RF-024   | Dungeon / Exploration     |
| RF-025 → RF-031   | Inventory / Equipment     |
| RF-032 → RF-034   | Loot / Rewards            |
| RF-035 → RF-037   | Character / Expedition    |
| RF-038 → RF-039   | Boss / Expedition         |
| RF-040 → RF-042   | Progression / Mathematics |
| RF-043            | Game Integration          |
| RNF-001 → RNF-003 | Technology / Architecture |
| RNF-004 → RNF-012 | Domain / Engineering      |
| RNF-013 → RNF-018 | Quality / Configuration   |
| RNF-019 → RNF-021 | Architecture              |
| RNF-022 → RNF-025 | Quality / Development     |

---

# 21. Relação com a Documentação do Produto

Os requisitos não substituem os documentos de definição do produto.

Eles consolidam e tornam verificáveis as decisões existentes.

```text
PRODUCT_VISION.md
        ↓
GAME_CONCEPT.md
        ↓
CORE_GAMEPLAY.md
        ↓
GAME_MECHANICS.md
        ↓
WORLD_AND_DUNGEON.md
        ↓
CHARACTERS.md
        ↓
ENEMIES.md
        ↓
PROGRESSION.md
        ↓
MVP.md
        ↓
REQUIREMENTS.md
```

---

# 22. Critérios de Aceite da FASE 10

A FASE 10 será considerada concluída quando:

* [ ] Todos os principais sistemas possuem requisitos;
* [ ] Requisitos funcionais foram identificados;
* [ ] Requisitos não funcionais foram identificados;
* [ ] Cada requisito possui identificador;
* [ ] Os requisitos possuem prioridade;
* [ ] Os requisitos são verificáveis;
* [ ] O Golden Path está contemplado;
* [ ] O MVP está contemplado;
* [ ] A arquitetura está contemplada;
* [ ] Os requisitos matemáticos estão contemplados;
* [ ] Os requisitos de qualidade estão contemplados;
* [ ] Não existem conflitos relevantes com as fases anteriores.

---

# 23. Critérios de Aceite do Documento

O documento deverá ser considerado adequado quando:

### Cobertura

Todos os sistemas definidos no MVP possuem requisitos correspondentes.

### Consistência

Os requisitos não contradizem os documentos anteriores.

### Rastreabilidade

Cada requisito pode ser relacionado a um sistema ou área do projeto.

### Verificabilidade

É possível imaginar um teste ou validação para cada requisito relevante.

### Evolução

O documento pode ser atualizado sem necessidade de reescrever toda a documentação do produto.

---

# 24. Definition of Done — FASE 10

A FASE 10 será considerada concluída quando:

```text
┌──────────────────────────────────┐
│       FASE 10 — REQUIREMENTS     │
├──────────────────────────────────┤
│                                  │
│ Functional Requirements      ✅  │
│ Non-Functional Requirements  ✅  │
│ Priorities                   ✅  │
│ Acceptance Criteria          ✅  │
│ Traceability                 ✅  │
│ Architecture                ✅  │
│ Quality                     ✅  │
│ Mathematics                 ✅  │
│ Golden Path                 ✅  │
│ MVP                         ✅  │
│                                  │
│        CHECKING PENDENTE         │
└──────────────────────────────────┘
```

---

# 25. Relação com as próximas etapas

Após a aprovação desta fase, os requisitos servirão como entrada para:

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
ETAPA 04
Personagens
        ↓
ETAPA 05
Combate
        ↓
...
```

Durante a implementação, cada requisito relevante deverá poder ser relacionado a:

* código;
* teste;
* documentação;
* commit;
* eventualmente uma issue ou milestone.

Isso permitirá rastrear:

```text
Requisito
   ↓
Implementação
   ↓
Teste
   ↓
Validação
```

---

# 26. Controle de mudanças

Alterações nos requisitos deverão ser avaliadas antes de serem incorporadas.

Uma alteração poderá:

```text
Não alterar escopo
       ↓
Atualizar requisito
```

ou:

```text
Alterar escopo
       ↓
Reavaliar MVP
       ↓
Atualizar documentação
       ↓
Registrar decisão
```

Nenhuma funcionalidade relevante deverá entrar no MVP simplesmente durante a implementação sem avaliação do impacto.

---

# 27. Status do Documento

**Documento:** `docs/product/REQUIREMENTS.md`

**Projeto:** Dungeon Crawler

**Etapa:** ETAPA 00 — Visão e Definição do Produto

**Fase:** FASE 10 — Requisitos Funcionais e Não Funcionais

**Versão:** 1.0

**Status atual:** 🟡 Aguardando Checking

---

# 28. Próximo Passo

O documento deverá passar pelo processo oficial de validação:

```text
REQUIREMENTS.md
       ↓
CHECKING
       ↓
Validar cobertura
       ↓
Validar consistência
       ↓
Validar rastreabilidade
       ↓
Identificar pendências
       ↓
Corrigir
       ↓
Validação final
       ↓
APROVAR FASE 10
       ↓
ETAPA 00 — 100%
```

Após a aprovação da FASE 10, a ETAPA 00 estará oficialmente concluída.
