public void login()
        {
            String userName, isUserName, isUserPassword = String.Empty;
            String[] userArray = File.ReadAllLines(@"C:\Users\yses9\Desktop\Application Development with .NET\Assignment1\BankSystem\user.txt");
            ArrayList users = new ArrayList(userArray);
            ConsoleKey key;
            
            foreach (string user in userArray)
            {
                Console.Write("Enter your UserName: ");
                userName = Console.ReadLine();
                if (user == userName)
                {
                    Console.Write("Enter your Password: ");
                    var userPassword = ReadPassword();


                    using (StreamReader sr = new StreamReader(File.Open(@"C:\Users\yses9\Desktop\Application Development with .NET\Assignment1\BankSystem\user.txt", FileMode.Open)))
                    {
                        isUserName = sr.ReadLine();
                        isUserPassword = sr.ReadLine();
                        sr.Close();
                    }

                    if (userName == isUserName && userPassword == isUserPassword)
                    {
                        using (StreamWriter sw = new StreamWriter(File.Create(@"C:\Users\yses9\Desktop\Application Development with .NET\Assignment1\BankSystem\login.txt")))
                        {
                            sw.Write(userName);
                            sw.Write("|");
                            sw.Write(userPassword);
                            sw.Close();
                        }

                        Console.WriteLine("Welcome, {0}", userName);

                    }
                    else
                    {
                        Console.WriteLine("Please check your username and password");
                    }
                }
            }
            Console.ReadKey();
        }
      
        //Encrypt the password instead with '*'
        public static string ReadPassword()
        {
            string userPassword = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    userPassword += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(userPassword))
                    {

                        userPassword = userPassword.Substring(0, userPassword.Length - 1);
                        int pos = Console.CursorLeft;
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            Console.WriteLine();
            return userPassword;

        }
        public void createAccount()
        {
            Console.ReadKey();
        }
        public void searchAccount()
        {
            Console.ReadKey();
        }
        public void deposit()
        {
            Console.ReadKey();
        }
        public void Withdrawal()
        {
            Console.ReadKey();
        }
        public void delete()
        {
            Console.ReadKey();
        }
        public void exit()
        {
            Console.ReadKey();
        }

    }

    public class main{ 
        static void Main(string[] args)
        {
            Console.WriteLine("Welcom to SIMPLE BANKING SYSTEM");
            bool login = false;
            User user1 = new User();
            user1.login();
            Console.ReadKey();

        start:
                if (login == true)
                {
                    goto menu;
                }
                
        menu:
            Console.Clear();
            

        }
    }