using System;

namespace WormsDeathmatch
{
#if WINDOWS || XBOX
    /// <summary>
    /// Load command line parametrs and start
    /// </summary>
    static class Program
    {

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        public static System.Collections.Generic.List<Player> Players = new System.Collections.Generic.List<WormsDeathmatch.Player>();

        [STAThreadAttribute]
        static void Main(string[] args)
        {
            string LevelToLoad = "";
            bool GotConsole = false;
            bool EnableWepons = false;
            string[] PlayerString = new string[0];

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-level="))
                {
                    LevelToLoad = args[i];
                }
                else if(args[i].StartsWith("-players="))
                {
                    PlayerString = args[i].Substring(args[i].IndexOf('=') + 1).Split('.');
                }
                else switch(args[i])
                {
                    case "-console":
                        {                
                            AllocConsole();
                            Console.WriteLine("Console & Logging subsytem initialized!");
                            GotConsole = true;
                        }
                        break;
                    case "-unlock_all_weapons":
                        {
                            EnableWepons = true;
                        }
                        break;


                }
            }

            if (GotConsole)
            {
                Console.WriteLine("Command line arguments: ");
                for (int i = 0; i < args.Length; i++)
                {

                    Console.Write(args[i] + " ");
                }
                Console.WriteLine();
            }

            if (string.IsNullOrEmpty(LevelToLoad))
            {
                Console.WriteLine("No level to load!");
            }
            
            Console.WriteLine("Creating window");
            GameWindow mainForm = new GameWindow();
            mainForm.Show();

            Console.WriteLine("Creating game");
            using (Game game = new Game(mainForm, LevelToLoad.Substring(LevelToLoad.IndexOf('=') + 1), PlayerString, EnableWepons))
            {
                game.Run();
            }
        }
    }
#endif
}

