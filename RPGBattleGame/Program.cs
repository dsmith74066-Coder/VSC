using System;
using System.Collections.Generic;

// Enum for Character classes.
enum CharacterClass
{
    Warrior,
    Mage,
    Rogue
}

// Enum for action types.
enum ActionType
{
    Attack,
    Defend,
    SpecialAbility,
    Heal,
    Quit
}

// Interface for all combatants
interface ICombatant
{
    string Name { get; }
    int Health { get; set; }
    int MaxHealth { get; }
    bool IsAlive { get; }
    ActionType ChooseAction();
    void TakeDamage(int damage);
    void DisplayStatus();
}

// Abstract base class for all characters
abstract class Character : ICombatant
{
    public string Name { get; protected set; }
    public int Health { get; set; }
    public int MaxHealth { get; protected set; }
    public int AttackPower { get; protected set; }
    public int Defense { get; protected set; }
    public CharacterClass Class { get; protected set; }
    public bool IsAlive => Health > 0;

    protected Character(string name, int maxHealth, int attackPower, int defense, CharacterClass characterClass)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        AttackPower = attackPower;
        Defense = defense;
        Class = characterClass;
    }

    public abstract ActionType ChooseAction();
    public abstract int PerformAttack();
    public abstract int UseSpecialAbility();

    public void TakeDamage(int damage)
    {
        int actualDamage = Math.Max(0, damage - Defense);
        Health -= actualDamage;
        if (Health < 0) Health = 0;
        Console.WriteLine($"{Name} takes {actualDamage} damage! (Health: {Health}/{MaxHealth})");
    }

    public void Heal(int amount)
    {
        Health = Math.Min(Health + amount, MaxHealth);
        Console.WriteLine($"{Name} heals for {amount} HP! (Health: {Health}/{MaxHealth})");
    }

    public void DisplayStatus()
    {
        Console.WriteLine($"{Name} ({Class}) - HP: {Health}/{MaxHealth}");
    }

    public void BoostDefense(int amount)
    {
        Defense += amount;
    }

    public void ResetDefense(int originalDefense)
    {
        Defense = originalDefense;
    }
}

// Warrior class High health and Defense.
class Warrior : Character
{
    public Warrior(string name) : base(name, 150, 20, 10, CharacterClass.Warrior)
    {
    }

    public override ActionType ChooseAction()
    {
        Console.WriteLine("\n1. Attack");
        Console.WriteLine("2. Defend");
        Console.WriteLine("3. Power Strike (Special)");
        Console.WriteLine("4. Heal");
        Console.WriteLine("5. Quit");
        Console.Write("Choose action: ");

        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 5)
        {
            return (ActionType)(choice - 1);
        }
        return ActionType.Attack;
    }

    public override int PerformAttack()
    {
        Console.WriteLine($"{Name} swings their sword!");
        return AttackPower;
    }

    public override int UseSpecialAbility()
    {
        Console.WriteLine($"{Name} uses Power Strike!");
        return AttackPower * 2;
    }
}

// Mage class lower health, High attack.
class Mage : Character
{
    public Mage(string name) : base(name, 100, 30, 5, CharacterClass.Mage)
    {
    }
    
    public override ActionType ChooseAction()
    {
        Console.WriteLine("\n1. Attack");
        Console.WriteLine("2. Defend");
        Console.WriteLine("3. Fireball (Special)");
        Console.WriteLine("4. Heal");
        Console.WriteLine("5. Quit");
        Console.Write("Choose action: ");

        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 5)
        {
            return (ActionType)(choice - 1);
        }
        return ActionType.Attack;
    }

    public override int PerformAttack()
    {
        Console.WriteLine($"{Name} casts magic missile!");
        return AttackPower;
    }

    public override int UseSpecialAbility()
    {
        Console.WriteLine($"{Name} casts Fireball!");
        return AttackPower * 2;
    }
}

// Rogue class balanced stats
class Rogue : Character
{
    public Rogue(string name) : base(name, 120, 25, 7, CharacterClass.Rogue)
    {
    }

    public override ActionType ChooseAction()
    {
        Console.WriteLine("\n1. Attack");
        Console.WriteLine("2. Defend");
        Console.WriteLine("3. Backstab (Special)");
        Console.WriteLine("4. Heal");
        Console.WriteLine("5. Quit");
        Console.Write("Choose action: ");

        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 5)
        {
            return (ActionType)(choice - 1);
        }
        return ActionType.Attack;
    }

    public override int PerformAttack()
    {
        Console.WriteLine($"{Name} strikes with daggers!");
        return AttackPower;
    }

    public override int UseSpecialAbility()
    {
        Console.WriteLine($"{Name} performs a Backstab!");
        return (int)(AttackPower * 2.5);
    }
}

// Enemy class with ai.
class Enemy : Character
{
    private Random random;
    
    public Enemy(string name, CharacterClass enemyClass) : base(name, 120, 18, 6, enemyClass)
    {
        random = new Random();
    }

    public override ActionType ChooseAction()
    {
        int choice = random.Next(1, 5);
        return (ActionType)(choice - 1);
    }

    public override int PerformAttack()
    {
        Console.WriteLine($"{Name} attacks!");
        return AttackPower;
    }

    public override int UseSpecialAbility()
    {
        Console.WriteLine($"{Name} uses a special attack!");
        return AttackPower * 2;
    }
}

// battle Manager
class BattleManager
{
    private Character player;
    private Enemy enemy;

    public BattleManager(Character playerCharacter, Enemy enemyCharacter)
    {
        player = playerCharacter;
        enemy = enemyCharacter;
    }

    public void StartBattle()
    {
        Console.WriteLine("\n=== Battle Start ===\n");

        while (player.IsAlive && enemy.IsAlive)
        {
            DisplayBattleStatus();

            // Player turn
            ExecuteTurn(player, enemy);
            if (!enemy.IsAlive) break;

            Console.WriteLine("\n--- Enemy Turn ---");
            System.Threading.Thread.Sleep(1000);

            // Enemy turn
            ExecuteTurn(enemy, player);

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        DeclareWinner();
    }

    private void ExecuteTurn(Character attacker, Character defender)
    {
        ActionType action = attacker.ChooseAction();
        int originalDefense = attacker.Defense;

        switch (action)
        {
            case ActionType.Attack:
                int damage = attacker.PerformAttack();
                defender.TakeDamage(damage);
                break;
            case ActionType.Defend:
                attacker.BoostDefense(5);
                Console.WriteLine($"{attacker.Name} raises their guard! (Defense + 5)");
                System.Threading.Thread.Sleep(500);
                attacker.ResetDefense(originalDefense);
                break;
            case ActionType.SpecialAbility:
                int specialDamage = attacker.UseSpecialAbility();
                defender.TakeDamage(specialDamage);
                break;
            case ActionType.Heal:
                attacker.Heal(30);
                break;
            case ActionType.Quit:
                Console.WriteLine($"\n{attacker.Name} surrenders the battle!");
                attacker.Health = 0;
                break;
        }
    }

    private void DisplayBattleStatus()
    {
        Console.WriteLine("\n--- Battle Status ---");
        player.DisplayStatus();
        enemy.DisplayStatus();
        Console.WriteLine("--------------------");
    }

    private void DeclareWinner()
    {
        Console.WriteLine("\n=== Battle End ===\n");
        if (player.IsAlive)
        {
            Console.WriteLine($"🎉 Victory! {player.Name} wins!");
        }
        else
        {
            Console.WriteLine($"💀 Defeat! {enemy.Name} wins!");
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== RPG Battle Game ===\n");
        Console.WriteLine("Choose your class:");
        Console.WriteLine("1. Warrior (High HP, High Defense)");
        Console.WriteLine("2. Mage (High Attack, Low Defense)");
        Console.WriteLine("3. Rogue (Balanced, Highest Special)");
        Console.Write("\nEnter choice (1-3): ");

        Character player;
        string input = Console.ReadLine() ?? string.Empty;

        switch (input)
        {
            case "1":
                player = new Warrior("Hero");
                break;
            case "2":
                player = new Mage("Hero");
                break;
            case "3":
                player = new Rogue("Hero");
                break;
            default:
                Console.WriteLine("Invalid choice. Defaulting to Warrior.");
                player = new Warrior("Hero");
                break;
        }
        
        Enemy enemy = new Enemy("Dark Knight", CharacterClass.Warrior);
        BattleManager battle = new BattleManager(player, enemy);
        battle.StartBattle();

        Console.WriteLine("\nThanks for playing!");
    }
}