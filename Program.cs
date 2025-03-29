using System.Runtime.InteropServices;
using System.Text;

namespace HotelReservationSystem
{
    internal class Program
    {
        [DllImport("user32.dll")] private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("kernel32.dll")] private static extern IntPtr GetConsoleWindow();
        private const int SW_MAXIMIZE = 3;

        #region Main Method
        static void Main(string[] args)
        {

            #region Command Prompt Maximization and Text Formatting.
            ShowWindow(GetConsoleWindow(), SW_MAXIMIZE);
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Hotel Reservation System";
            Console.Clear();
            int screenWidth = Console.WindowWidth;
            string title = "WELCOME TO THE STAAH HOTEL RESERVATION";
            string border = new string('=', title.Length + 10);
            Console.ForegroundColor = ConsoleColor.Magenta;
            PrintCentered(border); PrintCentered(title); PrintCentered(border);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintCentered("🏨 Book your stay with comfort & luxury! 🏨");
            Console.ResetColor();
            Console.WriteLine();
            #endregion

            #region Data Loading and Menu Selection  
            HotelService hotelService = new HotelService();
            CommandHandler commandHandler = new CommandHandler(hotelService);
            bool exit = false;

            try
            {
                do
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("\nPlease select an option to perform an operation:");
                    Console.WriteLine("1 - Check Room Availability (Press 1 and Enter)");
                    Console.WriteLine("2 - Search for Available Rooms (Press 2 and Enter)");
                    Console.WriteLine("0 - Exit the Application (Press 0 and Enter)");
                    Console.ResetColor(); Console.WriteLine();

                    Console.Write("Please Enter your choice: ");
                    string? choice = Console.ReadLine();


                    switch (choice)
                    {
                        case "0":
                            Console.WriteLine("Exiting STAAH HOTEL RESERVATION Application...");
                            Console.WriteLine("Thank you for using our service! We hope to see you again soon. Have a great day!");
                            Thread.Sleep(4000);
                            Environment.Exit(0);
                            break;

                        case "1":
                            Console.Write("\nEnter command (e.g., Availability(H1, 20250901, SGL) OR Availability(H1, 20250901-20250905, SGL)): ");
                            string? availabilityInput = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(availabilityInput))
                            {
                                ExitApplication(); 
                            }
                            else
                            {
                                commandHandler.ProcessCommand(availabilityInput);
                            }
                            break;

                        case "2":
                            Console.Write("\nEnter command (e.g., Search(H1, 365, SGL)): ");
                            string? searchInput = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(searchInput))
                            {
                                ExitApplication();
                            }
                            else
                            {
                                commandHandler.ProcessCommand(searchInput);
                            }
                            break;
                        default:
                            Console.WriteLine("\nInvalid choice, please try again.");
                            break;
                    }
                    if (!exit)
                    {
                        Console.WriteLine("\n\n\nPress any key to return to the main menu...");
                        Console.ReadKey();
                    }

                } while (!exit);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An issue occurred while performing menu selection. We will check and resolve it soon. Error: {ex.Message}");
                Environment.Exit(0);
            }
            #endregion
        } 
        #endregion

        #region Command Method 
        static void PrintCentered(string text)
        {
            int screenWidth = Console.WindowWidth;
            int padding = (screenWidth - text.Length) / 2;
            Console.WriteLine($"{new string(' ', Math.Max(0, padding))}{text}");
        }
        static void ExitApplication()
        {
            Console.WriteLine("\n🌟 You didn't enter any choice, so we're closing the application.");
            Console.WriteLine("💖 Thank you for using STAAH Hotel Reservation! See you again soon! 💖");
            Thread.Sleep(4000);
            Environment.Exit(0);
        } 
        #endregion

    }
}
