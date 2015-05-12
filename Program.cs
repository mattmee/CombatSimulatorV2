using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
CombatSimulator plays a computer enemy with one attack vs a human player with 3 options: sword attack,
 magic attack, and heal.  All attack values are randomly generated along with a random hit %
*/
namespace CombatSimulatorV2
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();    //New game declared
            game.PlayGame();           //calls PlayGame() function which plays 1 round of a game
        }
    }
    /// <summary>
    /// base class for player and enemy objects
    /// </summary>
    public abstract class Actor
    {
        public Random rand = new Random();
        public string Name { get; set; }
        public int HP { get; set; }
        /// <summary>
        /// Read only. Returns true if HP > 0
        /// </summary>
        public bool IsAlive
        {
            get
            {
                if (this.HP > 0)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// Constructor for Actor class
        /// </summary>
        /// <param name="name">string value to declare Actor's name</param>
        /// <param name="hp">int value to declare Actor's starting HP</param>
        public Actor(string name, int hp)
        {
            this.Name = name;
            this.HP = hp;
        }

        public virtual void Attack(Actor actor) { }

    }
    /// <summary>
    /// Enemy class inherits from Actor class
    /// </summary>
    public class Enemy : Actor
    {
        public Enemy(string name, int hp) : base(name, hp) { }  //Enemy constructor passes to base class Actor
        /// <summary>
        /// Enemy's Attack function
        /// </summary>
        /// <param name="actor">Actor to attack</param>
        public override void Attack(Actor actor)               
        {
 	        if (rand.Next(1, 101) <= 80)              // %80 chance to have a successful attack
            {
                actor.HP -= rand.Next(5, 16);         //Enemy hits between 5 and 15
            }
        }
    }
    /// <summary>
    /// Player class inherits from Actor class
    /// </summary>
    public class Player : Actor
    {
        public enum AttackType  //refers to player choices
        {
            Sword = 1,
            Magic,
            Heal
        }

        public Player(string name, int hp) : base(name, hp) { }  //Player constructor passes to base class Actor

        public override void Attack(Actor actor)
        {
            switch (ChooseAttack())                          // switch statement based on the return value of ChooseAttack()
            {
                case AttackType.Sword:
                    if (rand.Next(1, 101) <= 70)             //70% chance to hit
                    {
                        actor.HP -= rand.Next(20, 36);       //attack points taken are randomly chosen between 20 and 35
                    }
                    break;
                case AttackType.Magic:
                    actor.HP -= rand.Next(10, 16);           //attack points taken are randomly chosen between 10 and 15
                    break;
                default:
                    this.HP += rand.Next(10, 21);            //adds health to the player
                    break;
            }
        }
        /// <summary>
        /// Takes in user input until 1, 2, or 3 is chosen.
        /// </summary>
        /// <returns>returns attack type based on int value of user input</returns>
        private AttackType ChooseAttack()
        {
            string userInput = string.Empty;
            while (true)                                                  //infinite loop until userInput is 'valid'
            {
                userInput = Console.ReadLine();
                if (userInput.Length == 1 && "123".Contains(userInput[0]))//if user input is '1' '2' or '3' the infinite loop is broken
                {
                    break;
                }
                Console.WriteLine("Choose either 1, 2, or 3");
            }
            return (AttackType)int.Parse(userInput);
        }
    }

    public class Game
    {
        public Player player { get; set; }
        public Enemy enemy { get; set; }

        /// <summary>
        /// Game constructor. Creates two Actors, a Player and an Enemy
        /// </summary>
        public Game()                                            
        {
            this.player = new Player("Player1", 100);
            this.enemy = new Enemy("Dragon", 200);
        }
        /// <summary>
        /// Displays Player hitpoints, Dragons hitpoints, and gives instructions for input
        /// </summary>
        public void DisplayCombatInfo()
        {
            Console.Clear();
            Console.WriteLine("{0} HP: {1}", player.Name, player.HP);
            Console.WriteLine("{0} HP: {1}", enemy.Name, enemy.HP);
            Console.WriteLine("Choose 1 for sword attack, 2 for magic attack, or 3 to heal");
        }
        /// <summary>
        /// Plays one round, until either player hitpoints < 0 or dragon hitpoints < 0
        /// </summary>
        public void PlayGame()
        {
            while (this.player.IsAlive && this.enemy.IsAlive)    //while enemy and player hp > 0
            {
                DisplayCombatInfo();                             //combat info and instructions are displayed 
                this.player.Attack(this.enemy);                  //player attack on enemy is processed
                this.enemy.Attack(this.player);                  //enemy attack on player is processed
            }
            if (this.player.IsAlive) { Console.WriteLine("You won!"); }  
            else { Console.WriteLine("You lost"); }
            System.Threading.Thread.Sleep(3000);
        }
    }
}
