# Characters

## ETAPA 00 — Visão e Definição do Produto

**Documento:** `CHARACTERS.md`

**Projeto:** Dungeon Crawler

**Versão:** 1.0

**Status:** Em Desenvolvimento

---

# 1. Objetivo do Documento

Este documento define as regras conceituais relacionadas aos personagens do Dungeon Crawler.

Seu objetivo é estabelecer:

* O conceito de personagem;
* Os tipos de personagens;
* O personagem controlado pelo jogador;
* As classes jogáveis;
* Os atributos;
* As habilidades;
* Os equipamentos;
* O inventário;
* A progressão;
* Os estados;
* A interação com outros sistemas.

Este documento define o comportamento esperado do sistema em nível de produto.

A implementação técnica será realizada posteriormente nas etapas de **Arquitetura** e **Núcleo de Domínio**.

---

# 2. Conceito de Personagem

Um personagem representa uma entidade capaz de participar das principais interações do jogo.

No contexto do Dungeon Crawler, personagens poderão:

* Explorar;
* Combater;
* Utilizar habilidades;
* Equipar itens;
* Possuir recursos;
* Evoluir;
* Interagir com eventos.

O personagem do jogador será o principal elemento controlável durante as expedições.

---

# 3. Princípio de Design

O sistema de personagens será construído priorizando **composição sobre herança excessiva**.

A estrutura conceitual será:

```text
Character
   │
   ├── Stats
   ├── Abilities
   ├── Equipment
   ├── Inventory
   └── Class
```

O objetivo é evitar uma hierarquia rígida como:

```text
Character
├── Warrior
│   ├── TankWarrior
│   └── Berserker
│
├── Mage
│   ├── FireMage
│   └── IceMage
│
└── Rogue
    ├── Assassin
    └── Ranger
```

Essa abordagem criaria acoplamento desnecessário e dificultaria a evolução do sistema.

O projeto utilizará composição para permitir que o comportamento dos personagens seja determinado pela combinação de:

* Classe;
* Atributos;
* Habilidades;
* Equipamentos;
* Itens.

---

# 4. Estrutura Conceitual

O personagem será composto por diferentes responsabilidades.

```text
                       Character
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
        ▼                  ▼                  ▼
      Stats            Abilities          Equipment
        │                  │                  │
        │                  │                  │
        └──────────────────┼──────────────────┘
                           │
                     ┌─────┴─────┐
                     ▼           ▼
                 Inventory     Class
```

Cada componente terá uma responsabilidade específica.

---

# 5. Character

`Character` representa o agregado principal do personagem.

Ele será responsável por manter o estado e as regras fundamentais relacionadas ao personagem.

Conceitualmente:

```text
Character
│
├── Identity
├── Class
├── Stats
├── Abilities
├── Equipment
├── Inventory
└── State
```

O personagem deverá possuir uma identidade única dentro do jogo.

---

# 6. Identidade do Personagem

Cada personagem deverá possuir uma identificação própria.

A identidade poderá incluir:

* ID;
* Nome;
* Classe;
* Nível;
* Estado atual.

O ID será utilizado para diferenciar personagens durante persistência e gerenciamento do estado do jogo.

---

# 7. Classes Jogáveis

O MVP contará inicialmente com três classes.

```text
                    CLASSES

        ┌────────────┼────────────┐
        │            │            │
        ▼            ▼            ▼
    Guerreiro     Arcanista      Ladino
```

As classes representarão diferentes estilos de jogo.

---

# 8. Guerreiro

O Guerreiro será especializado em combate direto.

### Características

* Alta resistência;
* Boa capacidade ofensiva;
* Maior tolerância a dano;
* Menor dependência de recursos mágicos.

### Papel

O Guerreiro deverá representar o arquétipo de combate corpo a corpo.

### Estilo

```text
Ataque direto
     +
Resistência
     +
Controle do combate
```

---

# 9. Arcanista

O Arcanista será especializado em habilidades mágicas.

### Características

* Alto poder de habilidade;
* Maior dependência de recursos;
* Menor resistência física;
* Capacidade de controle.

### Papel

O Arcanista deverá representar o arquétipo de combate baseado em habilidades.

### Estilo

```text
Habilidades
     +
Controle
     +
Gerenciamento de recursos
```

---

# 10. Ladino

O Ladino será especializado em agilidade e ataques de oportunidade.

### Características

* Alta velocidade;
* Maior potencial de dano crítico;
* Menor resistência;
* Maior capacidade de evasão.

### Papel

O Ladino deverá representar o arquétipo de mobilidade e risco.

### Estilo

```text
Velocidade
     +
Crítico
     +
Evasão
```

---

# 11. Comparação das Classes

| Característica          | Guerreiro | Arcanista | Ladino |
| ----------------------- | --------: | --------: | -----: |
| Vida                    |      Alta |     Baixa |  Média |
| Ataque físico           |      Alta |     Baixa |   Alta |
| Poder de habilidade     |     Média |      Alta |  Média |
| Defesa                  |      Alta |     Baixa |  Média |
| Velocidade              |     Baixa |     Média |   Alta |
| Crítico                 |     Média |     Baixa |   Alta |
| Dependência de recursos |     Baixa |      Alta |  Média |
| Dificuldade             |     Baixa |     Média |   Alta |

Os valores exatos serão definidos durante o balanceamento.

---

# 12. Atributos

Os atributos representam as características fundamentais do personagem.

Inicialmente serão considerados:

* Força;
* Defesa;
* Vitalidade;
* Inteligência;
* Agilidade;
* Sorte.

---

# 13. Força

A Força representa a capacidade física ofensiva.

Poderá influenciar:

* Dano físico;
* Ataques corpo a corpo;
* Determinados equipamentos;
* Habilidades físicas.

---

# 14. Defesa

A Defesa representa a capacidade de reduzir ou resistir a ataques.

Poderá influenciar:

* Dano recebido;
* Resistência física;
* Eficiência defensiva.

---

# 15. Vitalidade

A Vitalidade representa a capacidade de sobrevivência do personagem.

Poderá influenciar:

* Pontos de vida;
* Resistência;
* Recuperação.

---

# 16. Inteligência

A Inteligência representa a capacidade relacionada ao uso de habilidades.

Poderá influenciar:

* Poder de habilidades;
* Recursos mágicos;
* Efeitos de determinadas habilidades.

---

# 17. Agilidade

A Agilidade representa velocidade e mobilidade.

Poderá influenciar:

* Ordem dos turnos;
* Evasão;
* Chance de acerto;
* Ataques rápidos.

---

# 18. Sorte

A Sorte representa fatores probabilísticos.

Poderá influenciar:

* Chance de crítico;
* Qualidade de recompensas;
* Eventos;
* Descobertas.

A influência exata da Sorte será balanceada posteriormente.

---

# 19. Atributos Derivados

Alguns valores do personagem serão calculados a partir de seus atributos.

Exemplos:

```text
Vida Máxima
Ataque
Defesa
Velocidade
Crítico
Evasão
Poder de Habilidade
```

Conceitualmente:

```text
Atributos Base
      │
      ▼
Regras de Cálculo
      │
      ▼
Atributos Derivados
```

A implementação dessas fórmulas será definida posteriormente no sistema de domínio.

---

# 20. Vida

Todo personagem possuirá pontos de vida.

O estado básico será:

```text
Vida Atual
Vida Máxima
```

Durante o combate:

```text
Dano
 ↓
Vida Atual
 ↓
Verificação
 ↓
Vivo / Derrotado
```

A vida poderá ser recuperada através de:

* Habilidades;
* Consumíveis;
* Eventos;
* Descanso;
* Sistemas específicos.

---

# 21. Recursos

Algumas classes poderão utilizar recursos específicos.

Exemplo conceitual:

```text
Guerreiro
→ Energia

Arcanista
→ Mana

Ladino
→ Energia
```

O sistema deverá permitir que diferentes classes possuam recursos distintos sem exigir alterações estruturais no personagem.

---

# 22. Habilidades

As habilidades representam ações especiais que o personagem poderá executar.

Exemplos:

* Ataques especiais;
* Magias;
* Buffs;
* Debuffs;
* Cura;
* Controle;
* Evasão.

A estrutura conceitual será:

```text
Ability
│
├── Nome
├── Custo
├── Alcance
├── Efeito
├── Cooldown
└── Regras de Uso
```

---

# 23. Habilidades por Classe

Cada classe possuirá um conjunto inicial de habilidades.

### Guerreiro

Exemplos conceituais:

* Golpe Poderoso;
* Defesa;
* Provocação.

### Arcanista

Exemplos conceituais:

* Projétil Arcano;
* Explosão Elemental;
* Controle.

### Ladino

Exemplos conceituais:

* Ataque Rápido;
* Golpe Crítico;
* Evasão.

As habilidades específicas serão detalhadas posteriormente na ETAPA 05 — Combate.

---

# 24. Equipamentos

O personagem poderá equipar itens.

Categorias iniciais:

```text
Equipamento
│
├── Arma
├── Armadura
└── Acessório
```

Os equipamentos poderão modificar:

* Atributos;
* Dano;
* Defesa;
* Resistências;
* Habilidades;
* Outros efeitos.

---

# 25. Inventário

Todo personagem possuirá um inventário.

O inventário será responsável por armazenar:

* Consumíveis;
* Equipamentos;
* Recursos;
* Itens especiais.

A capacidade poderá ser limitada.

Essa limitação contribuirá para as decisões de exploração.

---

# 26. Estado do Personagem

O personagem possuirá um estado atual.

Estados conceituais:

```text
IDLE
EXPLORING
COMBAT
VICTORY
DEFEATED
```

Estados adicionais poderão ser introduzidos posteriormente.

O estado deverá representar a situação atual do personagem dentro do fluxo do jogo.

---

# 27. Estados Temporários

Durante combate, o personagem poderá receber efeitos temporários.

Exemplos:

* Atordoado;
* Envenenado;
* Queimando;
* Congelado;
* Fortalecido;
* Enfraquecido.

Esses efeitos serão modelados posteriormente no sistema de combate.

---

# 28. Progressão

O personagem poderá evoluir durante o jogo.

A progressão poderá envolver:

* Experiência;
* Nível;
* Atributos;
* Habilidades;
* Equipamentos.

Fluxo:

```text
Combate
   ↓
Experiência
   ↓
Nível
   ↓
Atributos
   ↓
Novas capacidades
```

---

# 29. Sistema de Experiência

O personagem receberá experiência ao superar desafios.

Principais fontes:

* Combates;
* Chefes;
* Eventos;
* Objetivos especiais.

Ao atingir determinados valores, o personagem poderá subir de nível.

---

# 30. Sistema de Níveis

O nível representa a evolução geral do personagem.

Conceitualmente:

```text
Level 1
   ↓
Level 2
   ↓
Level 3
   ↓
...
```

A evolução poderá conceder:

* Aumento de atributos;
* Novas habilidades;
* Pontos de especialização;
* Acesso a equipamentos melhores.

Os valores exatos serão definidos na ETAPA 09 — Progressão.

---

# 31. Especialização

O sistema de especialização não fará parte obrigatoriamente do MVP.

Entretanto, a arquitetura deverá permitir sua futura inclusão.

Exemplo:

```text
Guerreiro
│
├── Especialização A
└── Especialização B
```

O mesmo princípio poderá ser aplicado às demais classes.

A implementação deverá evitar criar uma hierarquia rígida de classes para representar essas especializações.

---

# 32. Morte e Derrota

Quando a vida do personagem chegar a zero, ele será considerado derrotado.

Fluxo:

```text
Vida = 0
   ↓
DEFEATED
   ↓
Finalização do Combate
   ↓
Derrota da Expedição
```

As regras sobre o que será perdido ou preservado serão definidas na ETAPA 09 — Progressão e ETAPA 10 — Persistência.

---

# 33. Relação com Combate

O personagem será uma das principais entidades do sistema de combate.

O combate utilizará:

* Atributos;
* Habilidades;
* Equipamentos;
* Recursos;
* Estados.

Fluxo:

```text
Character
    │
    ├── Stats
    ├── Abilities
    ├── Equipment
    └── State
          │
          ▼
       Combat
```

O personagem não deverá conter toda a lógica do combate.

As regras do combate deverão permanecer desacopladas.

---

# 34. Relação com Exploração

Durante a exploração, o personagem será responsável por representar o jogador dentro da dungeon.

Ele poderá:

* Navegar;
* Interagir;
* Encontrar eventos;
* Encontrar recursos;
* Entrar em combate;
* Receber recompensas.

---

# 35. Relação com Inventário

O personagem possuirá um inventário, porém o sistema de inventário deverá possuir responsabilidade própria.

Conceitualmente:

```text
Character
    │
    └── Inventory
          │
          ├── Items
          ├── Consumables
          └── Equipment
```

O personagem utilizará o inventário, mas não deverá ser responsável pelas regras internas de armazenamento.

---

# 36. Relação com Equipamentos

O equipamento poderá alterar os atributos efetivos do personagem.

Conceitualmente:

```text
Base Stats
    +
Equipment
    +
Modifiers
    ↓
Effective Stats
```

Isso permitirá que o personagem evolua sem alterar diretamente todos os seus atributos base.

---

# 37. Relação com Progressão

A progressão do personagem será integrada ao sistema geral do jogo.

```text
Expedição
   ↓
Desafios
   ↓
Experiência
   ↓
Nível
   ↓
Atributos
   ↓
Equipamentos
   ↓
Maior capacidade
   ↓
Novas expedições
```

---

# 38. NPCs

NPCs representarão personagens não controlados diretamente pelo jogador.

Eles poderão possuir:

* Nome;
* Identidade;
* Estado;
* Função;
* Diálogos;
* Interações.

Exemplos:

* Comerciante;
* Ferreiro;
* Curandeiro;
* Guia;
* Personagem de evento.

NPCs serão tratados como uma extensão do conceito geral de personagem, mas não necessariamente possuirão todos os componentes do `Character` jogável.

---

# 39. Personagens e Inimigos

Inimigos também representam entidades capazes de participar de combates.

Entretanto, o sistema de inimigos será definido separadamente.

Essa separação permite que:

```text
Player Character
        │
        │
      Combat
        │
        │
Enemy
```

compartilhem conceitos comuns sem obrigar ambos a possuírem exatamente a mesma estrutura.

A definição detalhada dos inimigos será realizada na **FASE 07 — Inimigos**.

---

# 40. Princípio de Composição

A arquitetura conceitual do personagem seguirá:

```text
Character
│
├── Class
├── Stats
├── Abilities
├── Equipment
├── Inventory
└── State
```

Cada componente terá uma responsabilidade.

### Character

Responsabilidade:

* Identidade;
* Estado geral;
* Coordenação dos componentes.

### Stats

Responsabilidade:

* Atributos;
* Valores derivados.

### Abilities

Responsabilidade:

* Habilidades disponíveis;
* Regras de uso das habilidades.

### Equipment

Responsabilidade:

* Equipamentos atualmente utilizados.

### Inventory

Responsabilidade:

* Itens carregados.

### Class

Responsabilidade:

* Definição do arquétipo;
* Regras específicas da classe;
* Capacidades iniciais.

---

# 41. Princípios de Arquitetura

A implementação deverá buscar:

* Alta coesão;
* Baixo acoplamento;
* Composição sobre herança;
* Responsabilidade única;
* Dependências controladas;
* Testabilidade.

A classe `Character` não deverá se tornar uma classe responsável por todos os sistemas.

O objetivo é evitar o chamado **God Object**.

---

# 42. Exemplo Conceitual de Composição

Um personagem poderá ser representado conceitualmente assim:

```text
Character
│
├── Class = Warrior
│
├── Stats
│   ├── Strength
│   ├── Defense
│   ├── Vitality
│   ├── Intelligence
│   ├── Agility
│   └── Luck
│
├── Abilities
│   ├── PowerStrike
│   └── Guard
│
├── Equipment
│   ├── Sword
│   ├── Armor
│   └── Ring
│
└── Inventory
    ├── Potion
    ├── Potion
    └── Material
```

A mesma estrutura poderá representar outra classe:

```text
Character
│
├── Class = Arcanist
│
├── Stats
│
├── Abilities
│   ├── ArcaneBolt
│   └── ElementalBurst
│
├── Equipment
│
└── Inventory
```

Não será necessário criar uma nova hierarquia de `Character` para cada combinação.

---

# 43. Escopo do MVP

O MVP deverá conter:

### Personagens

* [x] Personagem jogável;
* [x] Três classes;
* [x] Atributos básicos;
* [x] Vida;
* [x] Recursos;
* [x] Habilidades básicas;
* [x] Equipamentos;
* [x] Inventário;
* [x] Experiência;
* [x] Nível;
* [x] Estados básicos;
* [x] Derrota.

### Fora do MVP inicial

* [ ] Especializações complexas;
* [ ] Árvore extensa de habilidades;
* [ ] Multiclasse;
* [ ] Classes desbloqueáveis;
* [ ] Sistema avançado de talentos;
* [ ] Customização visual complexa.

Esses sistemas poderão ser adicionados futuramente.

---

# 44. Princípios de Balanceamento

As classes não deverão ser simplesmente classificadas como:

> Forte, média e fraca.

Cada classe deverá possuir vantagens e desvantagens.

Exemplo:

```text
Guerreiro
+ Resistência
+ Dano consistente
- Menor mobilidade

Arcanista
+ Alto potencial de habilidade
+ Controle
- Baixa resistência

Ladino
+ Mobilidade
+ Crítico
- Maior risco
```

O objetivo é criar diferentes estilos de jogo viáveis.

---

# 45. Critérios de Validação

A FASE 06 será considerada concluída quando:

* O conceito de personagem estiver definido;
* A estrutura do `Character` estiver definida;
* O princípio de composição estiver documentado;
* As classes do MVP estiverem definidas;
* Os atributos estiverem definidos;
* Os atributos derivados estiverem identificados;
* O sistema de vida estiver definido;
* Os recursos estiverem definidos;
* As habilidades estiverem definidas em nível conceitual;
* Os equipamentos estiverem definidos;
* O inventário estiver definido;
* Os estados estiverem definidos;
* A progressão estiver definida em alto nível;
* A morte e derrota estiverem definidas;
* NPCs estiverem contextualizados;
* A relação com inimigos estiver definida;
* A relação com combate estiver definida;
* A relação com exploração estiver definida;
* A relação com inventário estiver definida;
* A relação com progressão estiver definida;
* O escopo do MVP estiver definido;
* A arquitetura conceitual estiver registrada.

---

# 46. Checking da Fase

| Item                               | Status |
| ---------------------------------- | ------ |
| Objetivo da fase definido          | ✅      |
| Conceito de personagem definido    | ✅      |
| Estrutura do Character definida    | ✅      |
| Composição sobre herança definida  | ✅      |
| Classes do MVP definidas           | ✅      |
| Guerreiro definido                 | ✅      |
| Arcanista definido                 | ✅      |
| Ladino definido                    | ✅      |
| Atributos definidos                | ✅      |
| Atributos derivados definidos      | ✅      |
| Vida definida                      | ✅      |
| Recursos definidos                 | ✅      |
| Habilidades definidas              | ✅      |
| Equipamentos definidos             | ✅      |
| Inventário definido                | ✅      |
| Estados definidos                  | ✅      |
| Progressão definida                | ✅      |
| Experiência definida               | ✅      |
| Níveis definidos                   | ✅      |
| Especialização planejada           | ✅      |
| Morte e derrota definidas          | ✅      |
| NPCs contextualizados              | ✅      |
| Relação com inimigos definida      | ✅      |
| Relação com combate definida       | ✅      |
| Relação com exploração definida    | ✅      |
| Relação com inventário definida    | ✅      |
| Relação com progressão definida    | ✅      |
| Escopo do MVP definido             | ✅      |
| Princípios arquiteturais definidos | ✅      |
| Critérios de validação definidos   | ✅      |

---

# 47. Status da Fase

**Documento:** `CHARACTERS.md`

**Etapa:** ETAPA 00 — Visão e Definição do Produto

**Fase:** FASE 06 — Personagens

**Versão:** 1.0

**Status:** ✅ Concluída e validada em nível conceitual.

---

# 48. Atualização do Roadmap

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

⬜ FASE 07 — Inimigos
⬜ FASE 08 — Progressão
⬜ FASE 09 — MVP
⬜ FASE 10 — Requisitos Funcionais e Não Funcionais

⬜ CHECKING FINAL DA ETAPA 00
```

---

# 49. Decisão Arquitetural Registrada

A seguinte decisão passa a fazer parte oficialmente das especificações do projeto:

> O sistema de personagens utilizará composição como estratégia principal de modelagem, evitando hierarquias extensas de herança.

Estrutura conceitual:

```text
Character
   │
   ├── Stats
   ├── Abilities
   ├── Equipment
   ├── Inventory
   └── Class
```

Essa decisão deverá ser respeitada durante as etapas de:

* ETAPA 02 — Arquitetura;
* ETAPA 03 — Núcleo de Domínio;
* ETAPA 04 — Personagens;
* ETAPA 05 — Combate;
* ETAPA 07 — Inventário e Itens;
* ETAPA 09 — Progressão.

Caso uma necessidade futura exija alteração desse modelo, a mudança deverá ser analisada e documentada antes da implementação.

---

# 50. Conclusão

A FASE 06 estabelece a definição conceitual do sistema de personagens do Dungeon Crawler.

O personagem será construído como um núcleo composto por diferentes componentes especializados.

A arquitetura evitará uma hierarquia excessiva de classes e privilegiará composição, permitindo que novos comportamentos sejam adicionados com menor impacto sobre o núcleo existente.

O MVP contará inicialmente com:

```text
Guerreiro
Arcanista
Ladino
```

Cada classe possuirá identidade própria, mas utilizará a mesma estrutura fundamental de personagem.

O sistema também estará preparado para futuras expansões, como:

* Especializações;
* Novas classes;
* Novas habilidades;
* Novos equipamentos;
* Novos recursos;
* Novos sistemas de progressão.

Com isso, a documentação necessária para a **FASE 06 — Personagens** está concluída.

**Status final: 🟢 FASE 06 APROVADA.**
