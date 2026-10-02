using System;
using System.Collections.Generic;
using DungeonCrawler.Domain.Abstractions;
using DungeonCrawler.Domain.Characters;
using DungeonCrawler.Domain.Enemies;
using DungeonCrawler.Domain.Progression;

namespace DungeonCrawler.Domain.Combat
{
    /// <summary>
    /// A turn-based fight between the character and one enemy (RF-008 to RF-014).
    /// Each round both sides act once, the faster one first. The combat owns the
    /// flow of the round; damage math lives in <see cref="AttackResolver"/> and
    /// the enemy's choices in its behavior.
    /// </summary>
    public sealed class CombatEncounter
    {
        public CombatEncounter(Character player, Enemy enemy)
        {
            Player = Guard.AgainstNull(player, nameof(player));
            Enemy = Guard.AgainstNull(enemy, nameof(enemy));

            if (player.IsDefeated)
            {
                throw new DomainException("A defeated character cannot start a combat.");
            }

            Status = CombatStatus.InProgress;
        }

        public Character Player { get; }

        public Enemy Enemy { get; }

        public CombatStatus Status { get; private set; }

        /// <summary>Number of rounds already played.</summary>
        public int Round { get; private set; }

        public bool IsOver
        {
            get { return Status != CombatStatus.InProgress; }
        }

        /// <summary>True when the player acts before the enemy. Ties favor the player.</summary>
        public bool PlayerActsFirst
        {
            get { return Player.Speed >= Enemy.Speed; }
        }

        /// <summary>Plays one full round: the player's action and the enemy's response.</summary>
        /// <exception cref="DomainException">The combat is over or the action is not possible.</exception>
        public RoundResult ExecuteRound(PlayerAction action, IRandomSource random)
        {
            Guard.AgainstNull(action, nameof(action));
            Guard.AgainstNull(random, nameof(random));

            if (IsOver)
            {
                throw new DomainException("The combat is already over.");
            }

            EnsurePossible(action);

            Round++;
            var log = new List<CombatLogEntry>();

            // Both sides commit to their action before anyone acts, so a guard
            // protects for the whole round regardless of who is faster.
            var intent = Enemy.DecideIntent(Player, random);
            DeclareGuards(action, intent, log);

            if (PlayerActsFirst)
            {
                PlayerTurn(action, random, log);
                if (!Enemy.IsDefeated)
                {
                    EnemyTurn(intent, random, log);
                }
            }
            else
            {
                EnemyTurn(intent, random, log);
                if (!Player.IsDefeated)
                {
                    PlayerTurn(action, random, log);
                }
            }

            EndRound(intent);
            return new RoundResult(Round, log, Status);
        }

        private void EnsurePossible(PlayerAction action)
        {
            if (action.Kind != PlayerActionKind.UseAbility)
            {
                return;
            }

            var ability = Player.FindAbility(action.AbilityId!);
            if (ability is null)
            {
                throw new DomainException($"The character has not learned '{action.AbilityId}'.");
            }

            if (!Player.Resource.CanAfford(ability.Definition.Cost))
            {
                throw new DomainException(
                    $"Not enough {Player.Class.ResourceName} to use {ability.Definition.Name}.");
            }
        }

        private void DeclareGuards(PlayerAction action, EnemyIntent intent, List<CombatLogEntry> log)
        {
            if (action.Kind == PlayerActionKind.Defend)
            {
                Player.StartGuarding();
                log.Add(CombatLogEntry.Guarding(true, Player.Name));
            }

            if (intent.Kind == EnemyIntentKind.Guard)
            {
                Enemy.StartGuarding();
                log.Add(CombatLogEntry.Guarding(false, Enemy.Name));
            }
        }

        private void PlayerTurn(PlayerAction action, IRandomSource random, List<CombatLogEntry> log)
        {
            switch (action.Kind)
            {
                case PlayerActionKind.BasicAttack:
                    var attack = new AttackProfile(
                        Player.BasicAttackDamage,
                        Player.CriticalChance,
                        Player.Class.CriticalMultiplier);
                    log.Add(CombatLogEntry.Attack(true, Player.Name, "Attack", AttackResolver.Resolve(attack, Enemy, random)));
                    break;

                case PlayerActionKind.UseAbility:
                    var ability = Player.FindAbility(action.AbilityId!)!;
                    Player.SpendResource(ability.Definition.Cost);
                    log.Add(ability.Definition.Effect.Apply(Player, Enemy, ability.Definition.Name, ability.Rank, random));
                    break;

                case PlayerActionKind.Defend:
                    // Declared at the start of the round; defending also recovers extra resource.
                    Player.RestoreResource(Player.Class.ResourceRegeneration);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action.Kind, "Unknown action.");
            }
        }

        private void EnemyTurn(EnemyIntent intent, IRandomSource random, List<CombatLogEntry> log)
        {
            if (intent.Kind != EnemyIntentKind.Attack)
            {
                return;
            }

            var attack = new AttackProfile(
                Enemy.AttackDamage(intent.PowerMultiplier),
                Enemy.Definition.CriticalChance,
                Balance.EnemyCriticalMultiplier);

            var outcome = AttackResolver.Resolve(attack, Player, random);
            log.Add(CombatLogEntry.Attack(false, Enemy.Name, intent.ActionName, outcome));

            var lifeSteal = intent.Ability?.LifeStealFraction ?? 0.0;
            if (lifeSteal > 0.0 && outcome.Damage > 0 && !Enemy.IsDefeated)
            {
                var before = Enemy.Health.Current;
                Enemy.Heal((int)Math.Round(outcome.Damage * lifeSteal, MidpointRounding.AwayFromZero));
                log.Add(CombatLogEntry.Heal(false, Enemy.Name, intent.ActionName, Enemy.Health.Current - before));
            }
        }

        private void EndRound(EnemyIntent intent)
        {
            Player.StopGuarding();
            Enemy.StopGuarding();
            Enemy.CompleteTurn(intent);

            if (Player.IsDefeated)
            {
                Status = CombatStatus.Defeat;
            }
            else if (Enemy.IsDefeated)
            {
                Status = CombatStatus.Victory;
            }
            else
            {
                Player.RestoreResource(Player.Class.ResourceRegeneration);
            }
        }
    }
}
