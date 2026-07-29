# Game Mechanics — Dungeon Crawler

## 1. Visão Geral

O **Dungeon Crawler** é um RPG 2D de exploração de dungeons e combate por turnos, desenvolvido em **C#** utilizando a **Unity**.

A experiência do jogador será construída em torno de quatro pilares principais:

* Combate estratégico;
* Exploração;
* Gerenciamento de recursos;
* Progressão de personagem.

O jogo terá como elemento central a relação entre **risco e recompensa**, incentivando o jogador a avaliar constantemente se deve continuar avançando ou retornar para um local seguro.

Este documento apresenta as principais mecânicas previstas para o produto em nível conceitual.

As definições aqui apresentadas estabelecem o comportamento esperado dos sistemas, mas não representam ainda sua implementação técnica.

A implementação detalhada será realizada posteriormente nas respectivas etapas de desenvolvimento.

---

# 2. Objetivo da Fase

A definição das mecânicas tem como objetivo transformar o conceito e o Core Gameplay do Dungeon Crawler em sistemas concretos de interação.

Esta fase busca responder:

> **Quais sistemas e regras permitem que o Core Gameplay aconteça?**

As mecânicas deverão definir:

* O que o jogador pode fazer;
* Quais sistemas existem;
* Como os sistemas se relacionam;
* Quais são as regras fundamentais;
* Como as mecânicas alimentam o ciclo principal do jogo;
* Quais mecânicas são essenciais para o MVP.

A arquitetura técnica, os modelos de domínio e a implementação em C# serão definidos posteriormente.

---

# 3. Mapa de Mecânicas

O jogo será estruturado em torno dos seguintes sistemas:

```text
                         DUNGEON CRAWLER
                                │
          ┌─────────────────────┼─────────────────────┐
          │                     │                     │
          ▼                     ▼                     ▼
      EXPLORAÇÃO             COMBATE             PROGRESSÃO
          │                     │                     │
          │                     │                     │
     ┌────┼────┐           ┌────┼────┐          ┌────┼────┐
     │    │    │           │    │    │          │    │    │
     ▼    ▼    ▼           ▼    ▼    ▼          ▼    ▼    ▼
   Salas Eventos Recursos  Turnos Ações Inimigos Nível Habilidades Equipamentos
          │                     │                     │
          └─────────────────────┼─────────────────────┘
                                │
                                ▼
                       RISCO E RECOMPENSA
                                │
                                ▼
                       DECISÕES DO JOGADOR
```

As principais mecânicas do jogo serão:

1. Exploração;
2. Salas;
3. Combate;
4. Ações;
5. Recursos;
6. Itens;
7. Equipamentos;
8. Inventário;
9. Eventos;
10. Risco e recompensa;
11. Progressão;
12. Vitória;
13. Derrota;
14. Hub e preparação.

---

# 4. Mecânica de Exploração

A exploração será uma das principais formas de interação do jogador com a dungeon.

O jogador deverá navegar por uma estrutura composta por diferentes áreas, salas e encontros.

O ciclo básico de exploração será:

```text
ENTRAR NA DUNGEON
       ↓
EXPLORAR ÁREA
       ↓
ENCONTRAR SALA
       ↓
INTERAGIR
       ↓
RESOLVER SITUAÇÃO
       ↓
OBTER RESULTADO
       ↓
ESCOLHER PRÓXIMO CAMINHO
```

A exploração deverá permitir:

* Navegação;
* Descoberta;
* Escolha de caminhos;
* Interação com elementos do ambiente;
* Encontrar recursos;
* Encontrar inimigos;
* Encontrar eventos;
* Encontrar recompensas;
* Avançar pela dungeon.

A dungeon não deverá ser estruturada apenas como uma sequência linear de combates.

A exploração deverá criar oportunidades para o jogador tomar decisões e avaliar riscos.

---

# 5. Mecânica de Salas

A dungeon será composta por diferentes tipos de salas.

Em alto nível:

```text
DUNGEON
│
├── Sala de Entrada
├── Sala de Combate
├── Sala de Evento
├── Sala de Recurso
├── Sala de Recompensa
├── Sala Especial
└── Sala de Chefe
```

Cada tipo de sala deverá possuir uma função diferente dentro da experiência.

## 5.1. Sala de Entrada

Representa o ponto inicial da expedição.

Sua função é introduzir o jogador à dungeon e iniciar o ciclo de exploração.

---

## 5.2. Sala de Combate

Inicia um confronto entre o personagem e um ou mais inimigos.

O resultado do combate poderá gerar:

* Experiência;
* Recursos;
* Itens;
* Ouro;
* Progressão.

---

## 5.3. Sala de Evento

Apresenta uma situação especial que exige uma decisão do jogador.

O evento poderá resultar em:

* Recompensas;
* Penalidades;
* Combates;
* Alteração de recursos;
* Novas oportunidades.

---

## 5.4. Sala de Recurso

Permite que o jogador encontre ou obtenha algum tipo de recurso.

Exemplos:

* Ouro;
* Itens;
* Consumíveis;
* Materiais;
* Recursos especiais.

---

## 5.5. Sala de Recompensa

Apresenta uma recompensa ao jogador como consequência de sua exploração ou de um desafio superado.

---

## 5.6. Sala Especial

Representa uma área com comportamento diferenciado.

Pode conter:

* Eventos especiais;
* NPCs;
* Desafios únicos;
* Recompensas especiais;
* Conteúdo opcional.

---

## 5.7. Sala de Chefe

Representa um desafio de maior importância dentro da dungeon.

A sala de chefe deverá funcionar como um dos principais pontos de encerramento de uma expedição.

A implementação detalhada desses tipos de sala será definida posteriormente na **ETAPA 08 — Dungeon e Exploração**.

---

# 6. Mecânica de Combate

O combate será baseado em turnos.

O jogador deverá enfrentar inimigos utilizando diferentes ações e estratégias.

O ciclo básico será:

```text
INICIAR COMBATE
       ↓
DEFINIR ORDEM DOS TURNOS
       ↓
TURNO DO JOGADOR
       ↓
ESCOLHER AÇÃO
       ↓
EXECUTAR AÇÃO
       ↓
RESOLVER EFEITOS
       ↓
VERIFICAR ESTADO
       │
       ├── VITÓRIA
       │
       ├── DERROTA
       │
       └── CONTINUA
              ↓
         TURNO INIMIGO
              ↓
         EXECUTAR AÇÃO
              ↓
         VERIFICAR ESTADO
              ↓
         PRÓXIMO TURNO
```

As ações disponíveis poderão incluir:

* Ataque;
* Defesa;
* Habilidades;
* Uso de itens;
* Ações especiais.

O sistema deverá permitir que diferentes personagens e inimigos possuam comportamentos e capacidades distintas.

O combate deverá priorizar tomada de decisão e estratégia.

A implementação detalhada será definida na **ETAPA 05 — Combate**.

---

# 7. Mecânica de Ações

Durante o jogo, o jogador deverá interagir com diferentes sistemas por meio de ações.

As ações disponíveis dependerão do contexto atual.

## 7.1. Ações em Combate

Exemplos:

```text
ATAQUE
DEFESA
HABILIDADE
ITEM
AÇÃO ESPECIAL
```

---

## 7.2. Ações durante Exploração

Exemplos:

```text
MOVER
INTERAGIR
ABRIR
COLETAR
EXAMINAR
ESCOLHER
```

---

## 7.3. Ações durante Eventos

Exemplos:

```text
DECIDIR
ACEITAR
RECUSAR
ARRISCAR
```

A intenção é que as ações disponíveis sejam determinadas pelo contexto atual do jogo.

---

# 8. Mecânica de Recursos

Os recursos terão papel importante na tomada de decisão.

Os principais recursos previstos são:

* Pontos de vida;
* Itens consumíveis;
* Equipamentos;
* Ouro;
* Recursos relacionados às habilidades;
* Espaço disponível no inventário.

Os recursos poderão ser classificados conceitualmente como:

```text
RECURSOS
│
├── PERMANENTES
│   ├── Equipamentos
│   └── Progressão
│
├── TEMPORÁRIOS
│   ├── Vida
│   ├── Recursos de habilidade
│   └── Consumíveis
│
└── ECONÔMICOS
    └── Ouro
```

A administração desses recursos deverá impactar diretamente a decisão do jogador de continuar avançando ou retornar ao Hub.

O gerenciamento de recursos deverá contribuir para a tensão e para a tomada de decisões durante a expedição.

---

# 9. Mecânica de Itens

Os itens serão utilizados para melhorar a capacidade do personagem ou fornecer recursos durante sua jornada.

Em alto nível, os itens poderão ser classificados como:

```text
ITENS
│
├── Consumíveis
│   ├── Cura
│   └── Recuperação
│
├── Equipamentos
│   ├── Armas
│   ├── Armaduras
│   └── Acessórios
│
└── Recursos
    └── Materiais
```

Os itens consumíveis poderão ser utilizados durante as expedições.

Os equipamentos poderão modificar atributos ou capacidades do personagem.

Os recursos poderão ser utilizados em sistemas futuros, como melhorias ou outras mecânicas.

O sistema será aprofundado posteriormente na **ETAPA 07 — Inventário e Itens**.

---

# 10. Mecânica de Equipamentos

Os equipamentos terão como objetivo modificar ou ampliar a capacidade do personagem.

Os principais grupos previstos são:

* Armas;
* Armaduras;
* Acessórios.

Os equipamentos poderão influenciar aspectos como:

* Poder de ataque;
* Defesa;
* Atributos;
* Habilidades;
* Resistências;
* Outros efeitos específicos.

A estrutura definitiva dos equipamentos será definida posteriormente, de acordo com o sistema de personagens e progressão.

---

# 11. Mecânica de Inventário

O inventário será responsável por controlar os itens carregados pelo personagem.

O jogador poderá:

* Visualizar itens;
* Organizar itens;
* Utilizar consumíveis;
* Equipar itens;
* Desequipar itens;
* Gerenciar o espaço disponível.

O inventário poderá possuir uma capacidade limitada.

Isso criará uma decisão adicional:

> **O que vale a pena levar comigo?**

A limitação do inventário deverá contribuir para o gerenciamento de recursos e para as decisões durante a exploração.

O sistema detalhado será definido na **ETAPA 07 — Inventário e Itens**.

---

# 12. Mecânica de Eventos

Eventos serão situações especiais encontradas durante a exploração.

Um evento poderá apresentar:

```text
EVENTO
   ↓
SITUAÇÃO
   ↓
DECISÃO
   ↓
CONSEQUÊNCIA
```

Exemplo conceitual:

```text
O jogador encontra uma porta misteriosa.

        │
        ▼

┌───────────────────────────┐
│ Abrir a porta             │
│ Ignorar a porta           │
│ Procurar uma alternativa  │
└───────────────────────────┘
        │
        ▼
    CONSEQUÊNCIA
```

As consequências poderão incluir:

* Recompensas;
* Perdas;
* Combates;
* Recursos;
* Novas oportunidades;
* Alterações na situação atual.

O objetivo dos eventos é adicionar decisões que não dependam exclusivamente do combate.

---

# 13. Mecânica de Risco e Recompensa

O sistema de risco e recompensa será uma das mecânicas centrais do jogo.

O jogador deverá avaliar constantemente se vale a pena continuar avançando.

Conceitualmente:

```text
          CONTINUAR
             │
      ┌──────┴──────┐
      │             │
      ▼             ▼
    RISCO        RECOMPENSA
      │             │
      ▼             ▼
  Recursos      Itens melhores
  reduzidos     Mais experiência
  Inimigos      Mais ouro
  difíceis      Novas áreas
```

A decisão de continuar deverá ser relevante.

O jogador poderá escolher entre:

### Retornar

Menor risco e menor potencial de recompensa adicional.

### Continuar

Maior risco e maior potencial de recompensa adicional.

A mecânica deverá criar o seguinte ciclo:

```text
AVANÇAR
   ↓
OBTER RECOMPENSA
   ↓
AUMENTAR O RISCO
   ↓
AVALIAR RECURSOS
   ↓
DECIDIR
```

Essa dinâmica será uma das principais características da experiência do jogador.

---

# 14. Mecânica de Progressão

O jogador deverá evoluir ao longo de múltiplas expedições.

A progressão poderá envolver:

* Experiência;
* Nível;
* Atributos;
* Habilidades;
* Equipamentos;
* Melhorias;
* Desbloqueios.

O ciclo será:

```text
EXPEDIÇÃO
    ↓
DESAFIOS
    ↓
EXPERIÊNCIA
    ↓
RECOMPENSAS
    ↓
PROGRESSÃO
    ↓
MAIOR CAPACIDADE
    ↓
NOVOS DESAFIOS
```

A progressão deverá proporcionar uma sensação clara de crescimento.

O jogador deverá começar enfrentando desafios menores e, gradualmente, adquirir capacidade para enfrentar ameaças maiores.

O sistema será detalhado posteriormente na **ETAPA 09 — Progressão**.

---

# 15. Mecânica de Vitória

O jogador poderá obter vitória ao cumprir determinados objetivos da expedição.

No MVP, o modelo conceitual será:

```text
EXPLORAR
   ↓
AVANÇAR
   ↓
SUPERAR DESAFIOS
   ↓
DERROTAR CHEFE
   ↓
VITÓRIA
   ↓
RECOMPENSA
   ↓
RETORNO AO HUB
```

A vitória poderá proporcionar:

* Recompensas;
* Experiência;
* Itens;
* Ouro;
* Desbloqueio de novas áreas;
* Novos desafios;
* Progressão do personagem;
* Progressão do conteúdo.

Os critérios definitivos de vitória serão definidos durante a definição formal do MVP.

---

# 16. Mecânica de Derrota

A derrota ocorrerá quando o jogador não conseguir superar um desafio ou perder as condições necessárias para continuar a expedição.

O fluxo conceitual será:

```text
DERROTA
   ↓
FINALIZAÇÃO DA EXPEDIÇÃO
   ↓
RETORNO AO HUB
   ↓
ANÁLISE DO RESULTADO
   ↓
PREPARAÇÃO
   ↓
NOVA EXPEDIÇÃO
```

A derrota deverá representar uma consequência significativa, mas não necessariamente o fim da progressão geral do personagem.

O projeto seguirá inicialmente o princípio de separar:

```text
PROGRESSÃO PERMANENTE
        +
PROGRESSO TEMPORÁRIO DA EXPEDIÇÃO
```

A definição exata do que será preservado ou perdido será estabelecida posteriormente.

---

# 17. Mecânica de Hub

O Hub será responsável pela preparação do jogador entre expedições.

O jogador poderá acessar:

```text
HUB
│
├── INVENTÁRIO
├── EQUIPAMENTOS
├── LOJA
├── MELHORIAS
├── NPCs
└── INICIAR EXPEDIÇÃO
```

O Hub funcionará como ponto de transição entre:

```text
EXPEDIÇÃO
    ↓
RESULTADO
    ↓
HUB
    ↓
PREPARAÇÃO
    ↓
NOVA EXPEDIÇÃO
```

Sua principal função será permitir que o jogador analise seus resultados e se prepare adequadamente para novos desafios.

A complexidade do Hub será ajustada ao escopo definido para o MVP.

---

# 18. Interação entre as Mecânicas

As mecânicas não deverão funcionar de maneira isolada.

O objetivo é estabelecer uma cadeia de dependências entre os principais sistemas:

```text
EXPLORAÇÃO
     │
     ▼
ENCONTROS
     │
     ├──────────────┐
     ▼              ▼
  COMBATE         EVENTOS
     │              │
     └──────┬───────┘
            ▼
        RECURSOS
            │
            ▼
       RECOMPENSAS
            │
            ▼
        PROGRESSÃO
            │
            ▼
      PREPARAÇÃO
            │
            ▼
       NOVA DUNGEON
```

Essa integração deverá criar uma experiência consistente.

O jogador explora para encontrar desafios.

Os desafios consomem recursos.

Os desafios superados geram recompensas.

As recompensas permitem progressão.

A progressão melhora a preparação.

A preparação permite enfrentar desafios maiores.

Esse ciclo sustenta a experiência principal do produto.

---

# 19. Mecânicas Prioritárias do MVP

Para manter o projeto controlado, as mecânicas serão classificadas por prioridade.

## 19.1. Prioridade Alta

Mecânicas consideradas essenciais para o funcionamento do MVP:

* Exploração;
* Salas;
* Combate;
* Inimigos;
* Vida;
* Itens;
* Inventário;
* Equipamentos;
* Progressão básica;
* Vitória;
* Derrota;
* Hub simplificado;
* Persistência.

---

## 19.2. Prioridade Média

Mecânicas que poderão enriquecer o MVP:

* Eventos;
* NPCs;
* Loja;
* Sistema econômico;
* Habilidades mais complexas;
* Diferentes tipos de recompensa.

---

## 19.3. Prioridade Baixa

Mecânicas que poderão ser adicionadas após a consolidação do MVP:

* Sistemas narrativos avançados;
* Eventos altamente complexos;
* Sistemas de crafting;
* Árvores de habilidades extensas;
* Sistemas econômicos avançados;
* Conteúdo procedural sofisticado.

A classificação poderá ser revisada durante a definição formal do MVP.

---

# 20. Princípio de Design das Mecânicas

Todas as mecânicas deverão responder a uma pergunta fundamental:

> **Essa mecânica contribui para a experiência principal do jogador?**

A experiência central é:

```text
EXPLORAR
   ↓
DECIDIR
   ↓
ARRISCAR
   ↓
SUPERAR
   ↓
RECOMPENSAR
   ↓
PROGREDIR
```

Se uma mecânica não contribuir de forma relevante para esse ciclo, sua inclusão deverá ser avaliada cuidadosamente.

O projeto deverá evitar **feature creep**, mantendo o foco na experiência principal e no escopo definido para o MVP.

A complexidade técnica também deverá ser proporcional à necessidade real do produto.

---

# 21. Relação com as Próximas Etapas

As definições desta fase servirão como base para as etapas posteriores do projeto.

| Mecânica                    | Definição detalhada                    |
| --------------------------- | -------------------------------------- |
| Personagens                 | ETAPA 04 — Personagens                 |
| Combate                     | ETAPA 05 — Combate                     |
| Inimigos                    | ETAPA 06 — Inimigos                    |
| Itens e equipamentos        | ETAPA 07 — Inventário e Itens          |
| Inventário                  | ETAPA 07 — Inventário e Itens          |
| Dungeon e exploração        | ETAPA 08 — Dungeon e Exploração        |
| Progressão                  | ETAPA 09 — Progressão                  |
| Persistência                | ETAPA 10 — Persistência                |
| Interface                   | ETAPA 11 — Interface                   |
| Testes                      | ETAPA 12 — Testes                      |
| Qualidade e observabilidade | ETAPA 13 — Qualidade e Observabilidade |

Esta fase define o comportamento esperado dos sistemas em nível de produto.

As etapas posteriores serão responsáveis por transformar essas definições em:

* Regras detalhadas;
* Modelos de domínio;
* Entidades;
* Value Objects;
* Serviços;
* Componentes;
* Interfaces;
* Implementação em C#;
* Testes automatizados.

---

# 22. Resumo das Mecânicas

```text
EXPLORAÇÃO
Explorar salas e descobrir situações.

SALAS
Representar diferentes tipos de interação dentro da dungeon.

COMBATE
Enfrentar inimigos utilizando decisões estratégicas.

AÇÕES
Executar ações diferentes de acordo com o contexto.

RECURSOS
Gerenciar vida, itens, equipamentos e demais recursos.

ITENS
Obter e utilizar recursos e objetos.

EQUIPAMENTOS
Modificar e ampliar as capacidades do personagem.

INVENTÁRIO
Gerenciar capacidade e itens disponíveis.

EVENTOS
Tomar decisões com consequências.

RISCO E RECOMPENSA
Escolher entre avançar ou retornar.

PROGRESSÃO
Evoluir personagem e capacidades.

VITÓRIA
Superar desafios e objetivos.

DERROTA
Encerrar a expedição e retornar ao Hub.

HUB
Preparar o jogador para a próxima expedição.
```

---

# 23. Critérios de Validação da Fase

A FASE 04 será considerada concluída quando:

* As principais mecânicas do jogo estiverem identificadas;
* A exploração estiver definida em nível conceitual;
* O sistema de salas estiver definido em nível conceitual;
* O combate estiver definido em nível conceitual;
* As ações principais estiverem identificadas;
* Os recursos estiverem definidos;
* Os itens estiverem definidos;
* Os equipamentos estiverem definidos;
* O inventário estiver definido;
* Os eventos estiverem definidos;
* O sistema de risco e recompensa estiver definido;
* A progressão estiver definida em alto nível;
* As condições conceituais de vitória estiverem definidas;
* As condições conceituais de derrota estiverem definidas;
* O Hub estiver definido;
* A interação entre as mecânicas estiver definida;
* A priorização inicial do MVP estiver definida;
* As relações com as próximas etapas estiverem documentadas.

---

# 24. Status da Fase

| Item                               | Status    |
| ---------------------------------- | --------- |
| Mecânicas principais identificadas | Concluído |
| Exploração definida em alto nível  | Concluído |
| Sistema de salas definido          | Concluído |
| Combate definido em alto nível     | Concluído |
| Ações definidas em alto nível      | Concluído |
| Recursos definidos                 | Concluído |
| Itens definidos                    | Concluído |
| Equipamentos definidos             | Concluído |
| Inventário definido                | Concluído |
| Eventos definidos                  | Concluído |
| Risco e recompensa definidos       | Concluído |
| Progressão definida em alto nível  | Concluído |
| Vitória definida em alto nível     | Concluído |
| Derrota definida em alto nível     | Concluído |
| Hub definido                       | Concluído |
| Interação entre sistemas definida  | Concluído |
| Priorização inicial do MVP         | Concluído |
| Relação com próximas etapas        | Concluído |

---

# 25. Status do Documento

**Documento:** `GAME_MECHANICS.md`

**Etapa:** ETAPA 00 — Visão e Definição do Produto

**Fase:** FASE 04 — Mecânicas

**Status:** Concluída e validada em nível conceitual.

**Próximo passo:** ETAPA 00 — FASE 05 — Mundo e Dungeon.
