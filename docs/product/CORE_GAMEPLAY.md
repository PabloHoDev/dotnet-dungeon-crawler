# Core Gameplay — Dungeon Crawler

**Projeto:** Dungeon Crawler
**Documento:** `docs/product/CORE_GAMEPLAY.md`
**Etapa:** ETAPA 00 — Visão e Definição do Produto
**Fase:** FASE 03 — Core Gameplay
**Versão:** 1.0
**Status:** ✅ Concluída

---

# 1. Objetivo

Este documento define o **ciclo central de jogo** do Dungeon Crawler: o que o jogador faz repetidamente, em que ordem e por quê.

Ele fica entre o conceito (`GAME_CONCEPT.md`), que descreve a experiência desejada, e as mecânicas (`GAME_MECHANICS.md`), que descrevem os sistemas que a tornam possível.

Este documento consolida definições já presentes em `GAME_CONCEPT.md`, `GAME_MECHANICS.md` e `MVP.md`. Ele não introduz regras novas.

---

# 2. Core Loop

O ciclo que sustenta o jogo é:

```text
EXPLORAR
    ↓
ENCONTRAR
    ↓
ENFRENTAR
    ↓
VENCER
    ↓
COLETAR
    ↓
EVOLUIR
    ↓
EXPLORAR NOVAMENTE
```

Cada volta do ciclo deixa o personagem um pouco mais forte e a dungeon um pouco mais perigosa.

---

# 3. Os Três Níveis do Ciclo

O jogo acontece em três ciclos encaixados, do mais curto ao mais longo.

```text
┌─────────────────────────────────────────────────────┐
│ CICLO DE PROGRESSÃO   (várias expedições)           │
│                                                     │
│   ┌─────────────────────────────────────────────┐   │
│   │ CICLO DE EXPEDIÇÃO   (uma dungeon)          │   │
│   │                                             │   │
│   │   ┌─────────────────────────────────────┐   │   │
│   │   │ CICLO DE COMBATE   (um encontro)    │   │   │
│   │   └─────────────────────────────────────┘   │   │
│   └─────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
```

## 3.1 Ciclo de Combate

Duração: um encontro.

```text
Início do combate
       ↓
Ação do jogador  (ataque, habilidade)
       ↓
Ação do inimigo
       ↓
Resolução  (dano, crítico, efeitos)
       ↓
Vitória / Derrota / Próximo turno
```

Pergunta do jogador: **qual é a melhor ação agora?**

## 3.2 Ciclo de Expedição

Duração: uma entrada na dungeon, do primeiro andar ao Boss.

```text
Entrar na dungeon
       ↓
Explorar sala
       ↓
Encontro  →  Combate  →  Recompensa (XP, ouro, loot)
       ↓
Subir de nível / Equipar
       ↓
Próxima sala / Próximo andar
       ↓
Elite  →  Mini Boss  →  Boss
       ↓
Vitória (expedição concluída)   ou   Morte (expedição encerrada)
```

Pergunta do jogador: **meus recursos aguentam o que vem pela frente?**

## 3.3 Ciclo de Progressão

Duração: várias expedições.

```text
Expedição
    ↓
Resultado  (vitória ou morte)
    ↓
Progressão permanente preservada  (nível, XP, atributos, habilidades, equipamentos)
    ↓
Preparação
    ↓
Nova expedição
```

Pergunta do jogador: **como quero construir meu personagem?**

---

# 4. Ações do Jogador

| Contexto | Ações |
|---|---|
| Preparação | Criar personagem, escolher classe, distribuir atributos, equipar e desequipar itens, iniciar expedição |
| Exploração | Avançar para uma sala, escolher caminho, avançar de andar |
| Combate | Ataque básico, habilidade |
| Recompensa | Receber XP, ouro e itens; guardar ou equipar |
| Evolução | Distribuir pontos de atributo, desbloquear e evoluir habilidades |

---

# 5. Decisões Significativas

O jogo deve colocar o jogador diante de decisões com consequência:

| Decisão | Tensão |
|---|---|
| Onde colocar os pontos de atributo | Especializar a build ou equilibrá-la |
| Qual habilidade usar | Dano agora ou recurso guardado para depois |
| Qual equipamento usar | Trocar poder imediato por um modificador diferente |
| O que manter no inventário | Espaço limitado |
| Qual caminho seguir | Sala opcional com mais risco e mais recompensa |

---

# 6. Risco e Recompensa

A profundidade aumenta as duas coisas ao mesmo tempo:

```text
Andar mais profundo
        ↓
Inimigos mais fortes  (escalonamento por nível e andar)
        ↓
Recompensas melhores  (mais XP, ouro e chance de loot raro)
```

A morte encerra a expedição, mas preserva a progressão permanente. Isso cria risco real sem apagar o desenvolvimento do personagem.

---

# 7. Condições de Fim

| Condição | Resultado |
|---|---|
| Derrotar o Boss do último andar | Vitória — expedição concluída |
| Vida do personagem chega a zero | Derrota — expedição encerrada |

Em ambos os casos, o jogador pode iniciar uma nova expedição com o mesmo personagem.

O abandono voluntário da expedição (retornar ao Hub antes do Boss) é citado em `GAME_CONCEPT.md` e `WORLD_AND_DUNGEON.md`, mas não faz parte do Golden Path do MVP.

---

# 8. Recorte do MVP

O ciclo do MVP é o Golden Path definido em `MVP.md` §28:

```text
Warrior → Floor 1 → Goblin → Level Up → Equipment
        → Floor 2 → Skeleton
        → Floor 3 → Elite
        → Floor 4 → Mini Boss
        → Floor 5 → Boss → Victory
```

| Elemento do ciclo | No MVP |
|---|---|
| Combate por turnos | ✅ |
| Exploração por salas e andares | ✅ |
| XP, nível, atributos, habilidades | ✅ |
| Loot e equipamentos | ✅ |
| Elite, Mini Boss, Boss | ✅ |
| Vitória e morte | ✅ |
| Hub | Mínimo: personagem, inventário, iniciar expedição |
| Eventos de sala | Fora do Golden Path |
| Loja, NPCs, melhorias | Futuro |
| Save/Load | Futuro (ETAPA 10) |

---

# 9. Relação com os Pilares

| Pilar (`GAME_CONCEPT.md`) | Onde aparece no ciclo |
|---|---|
| Combate estratégico | Ciclo de combate |
| Exploração | Ciclo de expedição |
| Gerenciamento de recursos | Vida, habilidades e inventário ao longo da expedição |
| Progressão | Ciclo de progressão |

---

# 10. Critérios de Validação

A FASE 03 é considerada concluída quando:

* [x] O core loop está definido;
* [x] Os três níveis do ciclo estão descritos;
* [x] As ações do jogador estão identificadas por contexto;
* [x] As decisões significativas estão identificadas;
* [x] As condições de fim estão definidas;
* [x] O recorte do MVP está definido;
* [x] A relação com os pilares está registrada.

**Próximo passo:** FASE 04 — Game Mechanics (`GAME_MECHANICS.md`).
