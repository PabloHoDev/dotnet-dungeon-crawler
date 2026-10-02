# Game Concept — Dungeon Crawler

## 1. Visão Geral

O **Dungeon Crawler** é um RPG 2D de exploração de dungeons e combate por turnos, desenvolvido em **C#** utilizando a **Unity**.

A experiência do jogador será construída em torno de quatro pilares principais:

* Combate estratégico;
* Exploração;
* Gerenciamento de recursos;
* Progressão de personagem.

O jogo terá como elemento central a relação entre **risco e recompensa**, incentivando o jogador a avaliar constantemente se deve continuar avançando ou retornar para um local seguro.

---

## 2. Fantasia do Jogador

A fantasia central do jogador será:

> Assumir o papel de um aventureiro que se prepara, entra em uma dungeon desconhecida, enfrenta desafios, coleta recursos e decide até onde está disposto a avançar em busca de recompensas maiores.

A experiência deverá transmitir a sensação de que:

> **Quanto mais fundo o jogador avança, maior o risco, mas também maior pode ser a recompensa.**

As decisões tomadas durante uma expedição deverão possuir consequências relevantes para o progresso do jogador.

A experiência desejada não deverá se resumir a:

```text
Entrar
    ↓
Lutar
    ↓
Ganhar
    ↓
Repetir
```

O fluxo desejado será mais próximo de:

```text
Preparar
    ↓
Explorar
    ↓
Avaliar o risco
    ↓
Tomar uma decisão
    ↓
Enfrentar a consequência
    ↓
Obter uma recompensa
    ↓
Decidir se continua
    ↓
Retornar ou avançar
```

---

## 3. Pilares da Experiência

A experiência do jogador será estruturada sobre quatro pilares.

### 3.1. Combate Estratégico

O combate será baseado em turnos e deverá exigir decisões do jogador.

Durante os confrontos, o jogador poderá escolher entre diferentes ações, considerando:

* Ataques;
* Defesa;
* Habilidades;
* Uso de itens;
* Recursos disponíveis;
* Características dos inimigos;
* Estado atual do personagem.

A intenção é que o jogador precise avaliar:

> **Qual é a melhor ação para este momento?**

O combate deverá valorizar estratégia e tomada de decisão, evitando depender exclusivamente de força bruta.

---

### 3.2. Exploração

A dungeon deverá incentivar o jogador a explorar e descobrir o ambiente.

A exploração poderá envolver:

* Salas;
* Caminhos;
* Portas;
* Baús;
* Recursos;
* Eventos;
* NPCs;
* Armadilhas;
* Inimigos;
* Áreas especiais;
* Chefes.

A experiência deverá estimular a curiosidade do jogador:

> **O que existe depois daquela porta?**

A exploração será uma parte importante da sensação de descoberta e progressão.

---

### 3.3. Gerenciamento de Recursos

O jogador deverá administrar recursos limitados durante suas expedições.

Entre os recursos que poderão existir estão:

* Vida;
* Itens consumíveis;
* Equipamentos;
* Ouro;
* Espaço de inventário;
* Recursos relacionados às habilidades.

Essa limitação deverá gerar decisões estratégicas, como:

> **Uso este recurso agora ou guardo para um desafio futuro?**

O gerenciamento de recursos deverá contribuir diretamente para a sensação de risco e recompensa.

---

### 3.4. Progressão

O jogador deverá sentir evolução ao longo de sua jornada.

A progressão poderá ocorrer por meio de:

* Experiência;
* Níveis;
* Atributos;
* Habilidades;
* Equipamentos;
* Recompensas;
* Desbloqueio de novas áreas.

A progressão conceitual será:

```text
AVENTUREIRO INICIANTE
        │
        ▼
    EXPERIÊNCIA
        │
        ▼
      NÍVEIS
        │
        ▼
    HABILIDADES
        │
        ▼
   EQUIPAMENTOS
        │
        ▼
AVENTUREIRO PREPARADO
```

A implementação detalhada do sistema de progressão será definida posteriormente.

---

## 4. Experiência Principal do Jogador

O jogo será estruturado em torno de um ciclo principal de gameplay:

```text
                 🏠 HUB
                   │
                   ▼
          PREPARAR EXPEDIÇÃO
                   │
                   ▼
            ENTRAR NA DUNGEON
                   │
                   ▼
                EXPLORAR
                   │
          ┌────────┼────────┐
          │        │        │
          ▼        ▼        ▼
       COMBATE   EVENTO   RECURSO
          │        │        │
          └────────┼────────┘
                   │
                   ▼
              PROGREDIR
                   │
                   ▼
            AUMENTAR O RISCO
                   │
                   ▼
           DECIDIR CONTINUAR?
              /          \
            SIM           NÃO
             │             │
             ▼             ▼
         AVANÇAR        RETORNAR
             │             │
             ▼             │
           CHEFE            │
             │              │
       ┌─────┴─────┐        │
       │           │        │
    VITÓRIA      DERROTA    │
       │           │        │
       ▼           ▼        │
    RECOMPENSA   RETORNO    │
       │           │        │
       └─────┬─────┴────────┘
             │
             ▼
            HUB
             │
             ▼
      NOVA EXPEDIÇÃO
```

Esse ciclo representa a experiência macro do produto.

O jogador deverá alternar entre momentos de:

* Preparação;
* Exploração;
* Combate;
* Decisão;
* Risco;
* Recompensa;
* Progressão.

---

## 5. Hub

O jogo contará com um **Hub**, que representará o ponto de preparação entre as expedições.

O Hub poderá concentrar funcionalidades como:

* Inventário;
* Loja;
* Compra e venda de itens;
* Gerenciamento de equipamentos;
* Melhorias;
* NPCs;
* Preparação da próxima expedição.

O conceito será:

```text
                 🏠 HUB
                   │
       ┌───────────┼───────────┐
       │           │           │
       ▼           ▼           ▼
    INVENTÁRIO    LOJA        NPCs
       │           │           │
       └───────────┼───────────┘
                   │
                   ▼
             PREPARAR-SE
                   │
                   ▼
                DUNGEON
                   │
                   ▼
                RETORNO
                   │
                   └──────────► HUB
```

O Hub terá como principal função preparar o jogador para uma nova expedição.

Sua implementação e seu nível de complexidade serão definidos de acordo com o escopo do MVP.

---

## 6. Risco e Recompensa

O conceito de risco e recompensa será uma característica central da experiência.

À medida que o jogador avança pela dungeon:

```text
PROFUNDIDADE DA DUNGEON
        │
        ▼
      RISCO ↑
        │
        ▼
  DIFICULDADE ↑
        │
        ▼
 RECOMPENSAS ↑
```

O jogador deverá avaliar constantemente:

> **Continuo avançando ou retorno agora?**

Continuar avançando poderá proporcionar:

* Recompensas melhores;
* Mais experiência;
* Itens mais valiosos;
* Acesso a áreas mais profundas;
* Acesso a desafios especiais.

Por outro lado, também poderá aumentar a exposição a:

* Inimigos mais difíceis;
* Perda de recursos;
* Eventos perigosos;
* Chefes;
* Derrota.

Essa dinâmica será uma das principais formas de criar tensão durante a exploração.

---

## 7. Experiência de Derrota

A derrota não deverá apagar completamente a evolução do jogador.

O conceito inicial será separar o progresso em duas categorias.

### 7.1. Progressão Permanente

Representa elementos que permanecem associados à evolução do personagem.

Exemplos:

* Nível;
* Habilidades desbloqueadas;
* Melhorias permanentes;
* Parte da evolução do personagem.

### 7.2. Progresso Temporário da Expedição

Representa elementos relacionados à expedição atual.

Exemplos:

* Localização atual na dungeon;
* Recursos consumíveis utilizados;
* Progresso da expedição;
* Recompensas ainda não garantidas.

Conceitualmente:

```text
                DERROTA
                   │
          ┌────────┴────────┐
          │                 │
          ▼                 ▼
     PROGRESSÃO           EXPEDIÇÃO
      PERMANENTE          ATUAL
          │                 │
          ▼                 ▼
       PRESERVADA         PERDIDA
```

As regras definitivas de perda e recuperação serão refinadas posteriormente durante a definição do MVP e dos sistemas de persistência.

---

## 8. Experiência de Vitória

A vitória de uma expedição deverá representar uma conquista significativa.

A estrutura conceitual será:

```text
ENTRAR NA DUNGEON
       │
       ▼
    EXPLORAR
       │
       ▼
   ENFRENTAR INIMIGOS
       │
       ▼
   SUPERAR DESAFIOS
       │
       ▼
      CHEFE
       │
       ▼
     VITÓRIA
       │
       ▼
   RECOMPENSA
       │
       ▼
 NOVO CONTEÚDO
```

A vitória deverá proporcionar:

* Recompensas;
* Sensação de conquista;
* Progressão;
* Desbloqueio de novos desafios.

A estrutura exata das condições de vitória será definida posteriormente.

---

## 9. Identidade e Atmosfera

A direção conceitual inicial será:

> **Fantasia medieval sombria, estilizada e acessível.**

A proposta não é buscar realismo visual, mas construir uma identidade visual coerente com o universo do jogo e compatível com o escopo de um projeto independente e de portfólio.

A ambientação poderá explorar elementos como:

* Masmorras;
* Ruínas;
* Cavernas;
* Florestas;
* Castelos;
* Criaturas fantásticas.

O projeto deverá priorizar:

> **Coerência visual acima de complexidade visual.**

A direção artística definitiva será estabelecida posteriormente, conforme a evolução do projeto.

---

## 10. Diferencial da Experiência

O diferencial proposto para o Dungeon Crawler será a combinação de:

> **Exploração + Decisão + Risco + Progressão**

O jogador não deverá pensar apenas:

> "Consigo derrotar este inimigo?"

Ele também deverá considerar:

> "Vale a pena enfrentar este inimigo agora?"

E, conforme avança:

> "Vale a pena continuar avançando?"

Esse conceito deverá orientar o desenvolvimento das futuras mecânicas do jogo.

---

## 11. Sentimentos Desejados

A experiência deverá estimular diferentes sensações ao longo da jornada.

### Curiosidade

> "O que vou encontrar?"

### Estratégia

> "Qual é a melhor decisão?"

### Tensão

> "Será que consigo continuar?"

### Risco

> "Será que devo avançar?"

### Recompensa

> "Valeu a pena arriscar."

### Progressão

> "Estou ficando mais preparado."

### Conquista

> "Consegui superar esse desafio."

Esses sentimentos representam a experiência desejada para o jogador.

---

## 12. Resumo Conceitual

| Elemento       | Definição                                                                   |
| -------------- | --------------------------------------------------------------------------- |
| Fantasia       | Aventureiro explorando dungeons perigosas                                   |
| Pilar 1        | Combate estratégico                                                         |
| Pilar 2        | Exploração                                                                  |
| Pilar 3        | Gerenciamento de recursos                                                   |
| Pilar 4        | Progressão                                                                  |
| Loop principal | Preparar → Explorar → Decidir → Arriscar → Recompensar → Retornar           |
| Hub            | Local de preparação entre expedições                                        |
| Risco          | Aumenta conforme o jogador avança                                           |
| Recompensa     | Potencialmente aumenta conforme o risco                                     |
| Derrota        | Perda do progresso da expedição, preservando parte da progressão permanente |
| Vitória        | Superação de desafios e chefes, gerando recompensas e progressão            |
| Atmosfera      | Fantasia medieval sombria, estilizada e acessível                           |
| Diferencial    | Decisões estratégicas baseadas em risco e recompensa                        |

---

## 13. Relação com as Próximas Fases

As definições desta fase servirão como base para as próximas definições do produto.

| Conceito                  | Fase relacionada                                  |
| ------------------------- | ------------------------------------------------- |
| Combate estratégico       | FASE 04 — Mecânicas                               |
| Exploração                | FASE 04 — Mecânicas / FASE 05 — Mundo e Dungeon   |
| Gerenciamento de recursos | FASE 04 — Mecânicas                               |
| Progressão                | FASE 08 — Sistema de Progressão                   |
| Hub                       | FASE 04 — Mecânicas / FASE 10 — Escopo do MVP     |
| Risco e recompensa        | FASE 04 — Mecânicas                               |
| Derrota                   | FASE 10 — Escopo do MVP / ETAPA 10 — Persistência |
| Vitória                   | FASE 10 — Escopo do MVP                           |
| Atmosfera                 | FASE 10 — Escopo do MVP                           |
| Conteúdo futuro           | FASE 10 — Escopo do MVP                           |

As definições apresentadas neste documento estabelecem a visão conceitual do produto. As fases posteriores poderão detalhar ou refinar essas definições sem alterar os princípios fundamentais da experiência.

---

## 14. Status da Fase

| Item                       | Status                 |
| -------------------------- | ---------------------- |
| Fantasia do jogador        | Definida               |
| Pilares da experiência     | Definidos              |
| Loop principal             | Definido               |
| Conceito do Hub            | Definido               |
| Risco e recompensa         | Definido               |
| Filosofia de derrota       | Definida em alto nível |
| Filosofia de vitória       | Definida em alto nível |
| Identidade e atmosfera     | Definida em alto nível |
| Diferencial da experiência | Definido               |
| Sentimentos desejados      | Definidos              |
| Relação com próximas fases | Definida               |

**Status da FASE 02:** Concluída e validada em nível conceitual.

**Próximo passo:** FASE 03 — Core Gameplay.
