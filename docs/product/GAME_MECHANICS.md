FASE 01 — VISÃO
"O que estamos construindo?"
        ↓
FASE 02 — CONCEITO
"Que experiência queremos?"
        ↓
FASE 03 — CORE GAMEPLAY
"O que o jogador faz?"
        ↓
FASE 04 — MECÂNICAS

MAPA DE MECANICAS

"Quais sistemas e regras fazem isso funcionar?"

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

                       Os principais sistemas serão:

Exploração;
Combate;
Recursos;
Itens e equipamentos;
Inventário;
Eventos;
Progressão;
Risco e recompensa;
Vitória e derrota;
Hub e preparação.
3. MECÂNICA DE EXPLORAÇÃO

A exploração será a principal forma de interação do jogador com a dungeon.

O jogador deverá navegar por uma estrutura composta por diferentes áreas e encontros.

O ciclo será:

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

4. MECÂNICA DE SALAS

A dungeon será composta por diferentes tipos de salas.

Em alto nível:

DUNGEON
│
├── Sala de Entrada
├── Sala de Combate
├── Sala de Evento
├── Sala de Recurso
├── Sala de Recompensa
├── Sala Especial
└── Sala de Chefe

Cada tipo de sala deverá possuir uma função diferente dentro da experiência.

Sala de Entrada

Marca o início da expedição.

Sala de Combate

Inicia um confronto.

Sala de Evento

Apresenta uma situação que exige decisão.

Sala de Recurso

Permite obter ou interagir com recursos.

Sala de Recompensa

Oferece recompensas ao jogador.

Sala Especial

Representa conteúdos diferenciados.

Sala de Chefe

Representa um desafio de maior importância.

A implementação definitiva desses tipos será refinada na ETAPA 08 — Dungeon e Exploração.

5. MECÂNICA DE COMBATE

O combate será baseado em turnos.

O jogador deverá enfrentar inimigos utilizando diferentes ações.

O ciclo básico será:

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
         PRÓXIMO TURNO