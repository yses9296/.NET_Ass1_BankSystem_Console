using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace BankSystem
{
    class UserInterface
    {
        protected static int origRow;
        protected static int origCol;
        string path = @"C:\Users\yses9\Desktop\Application Development with .NET\Assignment1\BankSystem\";
        public List<int> KeyList = new List<int>();
        public void LoginSreen(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();

            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;

            string[] loginWindowFields = { "Username : ", "Password : " };

            int[,] loginFieldPos = new int[2, 2];
            string[] loginUserInputs = new string[2]; //two: username and password

            //Form
            for (int line = 0; line < noLines; line++)
            {
                if (line == 0 | line == 2 | line == (noLines - 1))
                {
                    for (int col = 0; col < formWidth; col++)
                    {
                        WriteAt("=", startCol + col, startRow + line);
                    }
                }
                else
                {
                    WriteAt("|", startCol, startRow + line);
                    WriteAt("|", startCol + formWidth - 1, startRow + line);
                }
            }

            //Display form headings and other details
            WriteAt("Welcome to My Bank", startCol + 10, startRow + 1);
            WriteAt("Login to Start", startCol + 10, startRow + 4);

            //Display the field names
            int item = 0;
            string isUserName, isUserPassword = string.Empty;
            //string path = @"C:\Users\yses9\Desktop\Application Development with .NET\Assignment1\BankSystem\user.txt";
            //string[] userArray = File.ReadAllLines(@"C:\Users\yses9\Desktop\Application Development with .NET\Assignment1\BankSystem\user.txt");
            //rrayList users = new ArrayList(userArray);
            ConsoleKey key;

            foreach (string fieldName in loginWindowFields)
            {
                WriteAt(fieldName, startCol + 6, startRow + 6 + item);
                loginFieldPos[item, 1] = Console.CursorTop;
                loginFieldPos[item, 0] = Console.CursorLeft;

                item++;

            }
            do
            {
                using (StreamReader sr = new StreamReader(File.Open(@"C:\Users\yses9\Desktop\Application Development with .NET\Assignment1\BankSystem\user.txt", FileMode.Open)))
                {
                    isUserName = sr.ReadLine();
                    isUserPassword = sr.ReadLine();
                    sr.Close();
                }
                for (int field = 0; field < item; field++)
                {
                    Console.SetCursorPosition(loginFieldPos[field, 0], loginFieldPos[field, 1]);
                    loginUserInputs[field] = Console.ReadLine();
                    //NEED TO ADD HIDE PASSWORD FUNCTION
                }

                if (loginUserInputs[0].CompareTo(isUserName) == 0 && loginUserInputs[1].CompareTo(isUserPassword) == 0)
                {
                    WriteAt("Valid Credentials", startCol, noLines + 2);
                    MainSreen(11, 50, 2, 10);
                    break;
                }
                else
                {
                    WriteAt("Invalid Credentials", startCol, noLines + 2);
                    Console.ReadKey();
                }
            }
            while (true);
            Console.ReadKey();
        }

        //Method to print strings at speficie positions on the console screen
        protected void WriteAt(string s, int col, int row)
        {
            //set the cursor position 
            try
            {
                Console.SetCursorPosition(origRow + col, origCol + row);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        protected void WriteAtObject(string s, string o, int col, int row)
        {
            //set the cursor position 
            try
            {
                Console.SetCursorPosition(origRow + col, origCol + row);
                Console.Write(s+o);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
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
                    userPassword += info.KeyChar;
                    Console.Write("*");
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

        public void MainSreen(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();

            string[] mainWindowFields = { "1. Create a New Account", "2. Search an Account", "3. Deposit", "4. Withdraw", "5. A/C statement", "6. Delete account", "7. Exit" };

            //Form
            for (int line = 0; line < noLines; line++)
            {
                if (line == 0 | line == 2 | line == (noLines - 1))
                {
                    for (int col = 0; col < formWidth; col++)
                    {
                        WriteAt("=", startCol + col, startRow + line);
                    }
                }
                else
                {
                    WriteAt("|", startCol, startRow + line);
                    WriteAt("|", startCol + formWidth - 1, startRow + line);
                }
            }

            WriteAt("Welcome to Simple Bank System", startCol + 10, startRow + 1);

            int item = 0;
            foreach (string input in mainWindowFields)
            {

                WriteAt(input, startCol + 6, startRow + 3 + item);
                item++;
                WriteAt("Enter your choice: ", startCol + 6, startRow + 11);
            }
            switch (Console.ReadLine())
            {
                case "1":
                    createAccount(15, 50, 2, 10);
                    break;
                case "2":
                    searchAccount(10, 50, 2, 10);
                    break;
                case "3":
                    deposit(10, 50, 2, 10);
                    break;
                case "4":
                    Withdrawal(10, 50, 2, 10);
                    break;
                case "5":
                    statement(10, 50, 2, 10);
                    break;
                case "6":
                    delete(10, 50, 2, 10);
                    break;
                case "7":
                    exit(10, 50, 2, 10);
                    break;
                default:
                    WriteAt("Unknown Command", startCol + 6, startRow + 12);
                    return;
            }
            while (true) ;
            Console.ReadKey();
        }

        public void createAccount(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();
            string[] createFields = { "First Name: ", "Last Name: ", "Address: ", "Phone: ", "Email: " };
            string[] createUserInputs = new string[5];
            int[,] createFieldPos = new int[5, 2];

            string accountText;

            //Form
            for (int line = 0; line < noLines; line++)
            {
                if (line == 0 | line == 2 | line == (noLines - 1))
                {
                    for (int col = 0; col < formWidth; col++)
                    {
                        WriteAt("=", startCol + col, startRow + line);
                    }
                }
                else
                {
                    WriteAt("|", startCol, startRow + line);
                    WriteAt("|", startCol + formWidth - 1, startRow + line);
                }
            }
            WriteAt("Create A New Account", startCol + 10, startRow + 1);
            WriteAt("Enter the Details", startCol + 10, startRow + 3);

            int item = 0;
            foreach (string input in createFields)
            {
                WriteAt(input, startCol + 6, startRow + 6 + item);
                createFieldPos[item, 1] = Console.CursorTop;
                createFieldPos[item, 0] = Console.CursorLeft;
                item++;
            }
            do
            {
                for (int field = 0; field < item; field++)
                {
                    Console.SetCursorPosition(createFieldPos[field, 0], createFieldPos[field, 1]);
                    createUserInputs[field] = Console.ReadLine();
                }
                WriteAt("Is the Information correct (y/n)? ", startCol + 5, startRow + 13);

                string input = Console.ReadLine();
                if (input == "y")
                {
                    int accountNum = generateKey();
                    string accountNumber = accountNum.ToString();
                    accountText = accountNum.ToString() + ".txt";

                    try
                    {
                        File.WriteAllLines(path+accountNum, createUserInputs, Encoding.UTF8);
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("File Saved Error");
                        Console.WriteLine(e);
                    }
                    catch (SerializationException e)
                    {
                        Console.WriteLine("Binary Error");
                        Console.WriteLine(e);
                    }

                    
                    WriteAt("Account Created! Details will be provided via email.", startCol + 2, startRow + 15);
                    WriteAtObject("Your account Number is: ", accountNumber, startCol + 2, startRow + 16);

                    Console.ReadKey();
                    MainSreen(11, 50, 2, 10);
                    break;
                }
                else if (input == "n")
                {
                    return;
                }
                else
                {
                    Console.Write("Unknwon Command");
                    return;
                }

            } while (true);
            Console.ReadKey();
        }

        private static int key = 10001;
        public int generateKey()
        {
            int userKey = Interlocked.Increment(ref key);
            checkKey(userKey);

            return userKey;
        }

        public void checkKey(int accountNum)
        {
            string numPath = Convert.ToString(accountNum);
            string filePath = path + numPath + ".txt";

            if (File.Exists(filePath))
            {
                accountNum++;
            }
            else
            {
                accountNum = accountNum;
            }
        }

        public void Validator()
        {

        }

        public void searchAccount(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();

            //Form
            for (int line = 0; line < noLines; line++)
            {
                if (line == 0 | line == 2 | line == (noLines - 1))
                {
                    for (int col = 0; col < formWidth; col++)
                    {
                        WriteAt("=", startCol + col, startRow + line);
                    }
                }
                else
                {
                    WriteAt("|", startCol, startRow + line);
                    WriteAt("|", startCol + formWidth - 1, startRow + line);
                }
            }
            //Description
            WriteAt("SEARCH AN ACCOUNT", startCol + 15, startRow + 1);
            WriteAt("ENTER THE DETAILS", startCol + 15, startRow + 3);
            //InputField
            WriteAt("Account Number: ", startCol + 3, startRow + 5);

            string checkAccount = Console.ReadLine();
            int userNumInput = Convert.ToInt32(checkAccount);
            
            Stream account = File.Open(path+checkAccount, FileMode.Create, FileAccess.ReadWrite);

            Console.ReadKey();
        }

        public void deposit(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();

            //Form
            for (int line = 0; line < noLines; line++)
            {
                if (line == 0 | line == 2 | line == (noLines - 1))
                {
                    for (int col = 0; col < formWidth; col++)
                    {
                        WriteAt("=", startCol + col, startRow + line);
                    }
                }
                else
                {
                    WriteAt("|", startCol, startRow + line);
                    WriteAt("|", startCol + formWidth - 1, startRow + line);
                }
            }

            Console.ReadKey();
        }
        public void Withdrawal(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();

            //Form
            for (int line = 0; line < noLines; line++)
            {
                if (line == 0 | line == 2 | line == (noLines - 1))
                {
                    for (int col = 0; col < formWidth; col++)
                    {
                        WriteAt("=", startCol + col, startRow + line);
                    }
                }
                else
                {
                    WriteAt("|", startCol, startRow + line);
                    WriteAt("|", startCol + formWidth - 1, startRow + line);
                }
            }

            Console.ReadKey();
        }
        public void statement(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();

            //Form
            for (int line = 0; line < noLines; line++)
            {
                if (line == 0 | line == 2 | line == (noLines - 1))
                {
                    for (int col = 0; col < formWidth; col++)
                    {
                        WriteAt("=", startCol + col, startRow + line);
                    }
                }
                else
                {
                    WriteAt("|", startCol, startRow + line);
                    WriteAt("|", startCol + formWidth - 1, startRow + line);
                }
            }

            Console.ReadKey();
        }
        public void delete(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();

            //Form
            for (int line = 0; line < noLines; line++)
            {
                if (line == 0 | line == 2 | line == (noLines - 1))
                {
                    for (int col = 0; col < formWidth; col++)
                    {
                        WriteAt("=", startCol + col, startRow + line);
                    }
                }
                else
                {
                    WriteAt("|", startCol, startRow + line);
                    WriteAt("|", startCol + formWidth - 1, startRow + line);
                }
            }

            Console.ReadKey();
        }
        public void exit(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();



            Console.ReadKey();
        }
    }


    public class Program
    { 
        static void Main(string[] args)
        {
            UserInterface ConsoleInterface = new UserInterface();
            ConsoleInterface.LoginSreen(10, 40, 2, 10);
            //ConsoleInterface.MainSreen(11, 50, 2, 10);

            Console.ReadKey();

        }
    }






}
