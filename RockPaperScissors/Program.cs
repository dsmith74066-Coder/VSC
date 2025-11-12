using System;

// Interface 
interface IPlayer
{
    string Name { get; }
    int GetChoice();
    void DisplayChoice(int choice);
}

// Human player implementation.
class HumanPlayer : IPlayer
{
    public string Name { get; }

    public HumanPlayer(string name)
    {
        Name = name;
    }

    public int GetChoice()
    {
        Console.WriteLine("\nChoose your move:");
        Console.WriteLine("1. Rock");
        Console.WriteLine("2. Paper");
        Console.WriteLine("3. Scissors");
        Console.WriteLine("Enter your choice (1-3): ");

        string input = Console.ReadLine() ?? string.Empty;

        if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 3)
        {
            return choice;
        }

        Console.WriteLine("Invalid choice! Please try again.");
        return GetChoice();
    }

    public void DisplayChoice(int choice)
    {
        string[] moves = { "", "Rock", "Paper", "Scissors" };
        Console.WriteLine($"{Name} chose: {moves[choice]}");
    }
}

// Computer player implementation.
class ComputerPlayer : IPlayer
{
    private Random random;
    public string Name { get; }

    public ComputerPlayer(string name)
    {
        Name = name;
        random = new Random();
    }

    public int GetChoice()
    {
        return random.Next(1, 4);
    }

    public void DisplayChoice(int choice)
    {
        string[] moves = { "", "Rock", "Paper", "Scissors" };
        Console.WriteLine($"{Name} chose: {moves[choice]}");
    }
}

// Game Manager
class GameManager
{
    private IPlayer player1;
    private IPlayer player2;
    private int player1Wins;
    private int player2Wins;
    private int ties;

    public GameManager(IPlayer p1, IPlayer p2)
    {
        player1 = p1;
        player2 = p2;
        player1Wins = 0;
        player2Wins = 0;
        ties = 0;
    }

    public void PlayRound()
    {
        int choice1 = player1.GetChoice();
        int choice2 = player2.GetChoice();

        Console.WriteLine();
        player1.DisplayChoice(choice1);
        player2.DisplayChoice(choice2);

        DetermineWinner(choice1, choice2);
        DisplayScore();
    }

    private void DetermineWinner(int choice1, int choice2)
    {
        if (choice1 == choice2)
        {
            Console.WriteLine("It's a tie!");
            ties++;
        }
        else if ((choice1 == 1 && choice2 == 3) ||
                (choice1 == 2 && choice2 == 1) ||
                (choice1 == 3 && choice2 == 2))
        {
            Console.WriteLine($"{player1.Name} wins this round!");
            player1Wins++;
        }
        else
        {
            Console.WriteLine($"{player2.Name} wins this round!");
            player2Wins++;
        }
    }

    private void DisplayScore()
    {
        Console.WriteLine($"\nScore - {player1.Name}: {player1Wins} | {player2.Name}: {player2Wins} | Ties: {ties}");
    }

    public void DisplayFinalScore()
    {
        Console.WriteLine("\n=== Final Score ===");
        Console.WriteLine($"{player1.Name}: {player1Wins}");
        Console.WriteLine($"{player2.Name}: {player2Wins}");
        Console.WriteLine($"Ties: {ties}");
    }

    public bool AskPlayAgain()
    {
        Console.Write("\nPlay again? (y/n): ");
        string response = Console.ReadLine() ?? string.Empty.ToLower();
        return (response == "y" || response == "yes");
    }
}

class RockPaperScissors
{
    static void Main()
    {
        Console.WriteLine("=== Rock Paper Scissors ===\n");

        IPlayer human = new HumanPlayer("Player");
        IPlayer computer = new ComputerPlayer("Computer");

        GameManager game = new GameManager(human, computer);

        bool playAgain = true;
        while (playAgain)
        {
            game.PlayRound();
            playAgain = game.AskPlayAgain();
        }

        game.DisplayFinalScore();
        Console.WriteLine("\nThanks for playing!");
    }
}