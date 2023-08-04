using System;
using System.Collections.Generic;

namespace SnakeGame
{

    struct Position
    {
        public int X;
        public int Y;
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    class Snake
    {
        private List<Position> body;
        private Direction direction;

        public Snake()
        {
            body = new List<Position>
            {
                new Position { X = 0, Y = 0 }
            };
            direction = Direction.Right;
        }

        public void ChangeDirection(Direction newDirection)
        {
            // Prevenir que el snake se mueva en la dirección opuesta y autocolisione.
            if (Math.Abs((int)newDirection - (int)direction) != 2)
            {
                direction = newDirection;
            }
        }

        public void Move()
        {
            Position head = body[0];
            switch (direction)
            {
                case Direction.Up:
                    head.Y--;
                    break;
                case Direction.Down:
                    head.Y++;
                    break;
                case Direction.Left:
                    head.X--;
                    break;
                case Direction.Right:
                    head.X++;
                    break;
            }

            body.Insert(0, head);
            body.RemoveAt(body.Count - 1);
        }

        public void Grow()
        {
            // Simular el crecimiento del Snake.
            Position tail = body[body.Count - 1];
            body.Add(tail);
        }

        public List<Position> GetBody()
        {
            return body;
        }
    }

    class Food
    {
        private Position position;
        private Random random;

        public Food(int maxX, int maxY)
        {
            random = new Random();
            position.X = random.Next(0, maxX);
            position.Y = random.Next(0, maxY);
        }

        public Position GetPosition()
        {
            return position;
        }
    }

   class Game
{
    private const int BoardWidth = 40;
    private const int BoardHeight = 20;
    private Snake snake;
    private Food food;
    private int score;


    public Game()
    {
        snake = new Snake();
        food = new Food(BoardWidth, BoardHeight);
        score = 0;
    }
         public void DrawBoard()
    {
        Console.Clear();
        
        Console.ForegroundColor = ConsoleColor.White;

        // Dibujar el borde superior del tablero
        for (int i = 0; i < BoardWidth + 2; i++)
        {
            Console.Write("=");
        }
        Console.WriteLine();

        for (int y = 0; y < BoardHeight; y++)
        {
            Console.Write("|"); // Dibujar el borde izquierdo del tablero

            for (int x = 0; x < BoardWidth; x++)
            {
                Position currentPos = new Position { X = x, Y = y };

                if (snake.GetBody().Contains(currentPos))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("■");
                }
                else if (food.GetPosition().X == x && food.GetPosition().Y == y)
                {   
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("♥");
                }
                else
                {
                    Console.Write(" ");
                }
            }

            Console.WriteLine("|"); // Dibujar el borde derecho del tablero
        }

        // Dibujar el borde inferior del tablero
        for (int i = 0; i < BoardWidth + 2; i++)
        {
            Console.Write("=");
        }
        Console.WriteLine();
    }
   public void HandleInput()
{
    if (Console.KeyAvailable)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        switch (keyInfo.Key)
        {
            case ConsoleKey.W:
                snake.ChangeDirection(Direction.Up);
                break;
            case ConsoleKey.S:
                snake.ChangeDirection(Direction.Down);
                break;
            case ConsoleKey.A:
                snake.ChangeDirection(Direction.Left);
                break;
            case ConsoleKey.D:
                snake.ChangeDirection(Direction.Right);
                break;
        }
    }
}
        public void Run()
        {
            Thread inputThread = new Thread(HandleInput);
            inputThread.Start();
            while (true)
            {
                DrawBoard(); 

                //Capturar el input del jugador
                         if (Console.KeyAvailable){
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.W:
                        snake.ChangeDirection(Direction.Up);
                        break;
                    case ConsoleKey.S:
                        snake.ChangeDirection(Direction.Down);
                        break;
                    case ConsoleKey.A:
                        snake.ChangeDirection(Direction.Left);
                        break;
                    case ConsoleKey.D:
                        snake.ChangeDirection(Direction.Right);
                        break;  
                    }
                    System.Threading.Thread.Sleep(100);
                }

                // Función: Mover Snake
                snake.Move();

                // Evitar el choque con comida y que la coleccione
               Position head = snake.GetBody()[0];
                Position foodPosition = food.GetPosition();
                if (head.X == foodPosition.X && head.Y == foodPosition.Y)
                {
                    //Crecimiento del Snake al comer
                    snake.Grow();
                    //Nueva comida random en el tablero
                    food = new Food(BoardWidth, BoardHeight);
                }

                // Chocar con los bordes/limites
                if (head.X < 0 || head.X >= BoardWidth || head.Y < 0 || head.Y >= BoardHeight)
                {
                    Console.Clear();
                    Console.WriteLine("Game Over! Tu puntuación es: " + (snake.GetBody().Count - 1));
                    break;
                }

                // Colisión o choque con el cuerpo del Snake
                for (int i = 1; i < snake.GetBody().Count; i++)
                {
                    if (head.X == snake.GetBody()[i].X && head.Y == snake.GetBody()[i].Y)
                    {
                        Console.Clear();
                        Console.WriteLine("Game Over! Tu puntuación es: " + (snake.GetBody().Count - 1));
                        break;
                    }
                }
                score = snake.GetBody().Count - 1;


                System.Threading.Thread.Sleep(100); // Añadir un retraso para controlar la velocidad del juego
            }
        }
    }


 class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine(@"
███████╗███╗   ██╗ █████╗ ██╗  ██╗███████╗     ██████╗  █████╗ ███╗   ███╗███████╗    ███╗   ███╗███████╗███╗   ██╗██╗   ██╗
██╔════╝████╗  ██║██╔══██╗██║ ██╔╝██╔════╝    ██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ████╗ ████║██╔════╝████╗  ██║██║   ██║
███████╗██╔██╗ ██║███████║█████╔╝ █████╗      ██║  ███╗███████║██╔████╔██║█████╗      ██╔████╔██║█████╗  ██╔██╗ ██║██║   ██║
╚════██║██║╚██╗██║██╔══██║██╔═██╗ ██╔══╝      ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║╚██╔╝██║██╔══╝  ██║╚██╗██║██║   ██║
███████║██║ ╚████║██║  ██║██║  ██╗███████╗    ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ██║ ╚═╝ ██║███████╗██║ ╚████║╚██████╔╝
╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝     ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝    ╚═╝     ╚═╝╚══════╝╚═╝  ╚═══╝ ╚═════╝ 
                                                                                                    
            ");
            Console.WriteLine(@"
1.  
╔═╗┬  ┌─┐┬ ┬
╠═╝│  ├─┤└┬┘
╩  ┴─┘┴ ┴ ┴");
            Console.WriteLine(@"
2.
╔═╗─┐ ┬┬┌┬┐
║╣ ┌┴┬┘│ │ 
╚═╝┴ └─┴ ┴ ");
            Console.WriteLine("Enter your choice:");

            char choice = Console.ReadKey().KeyChar;

            switch (choice)
            {
                case '1':
                    Game game = new Game();
                    game.Run();
                    break;
                case '2':
                    return; // Exit the program
                default:
                    Console.WriteLine("\nElección incorrecta. Presiona cualquier tecla para continuar...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}