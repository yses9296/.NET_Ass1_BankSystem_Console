using System;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BankSystem
{
    public abstract class form
    {
        protected static int origRow;
        protected static int origCol;
        protected static int keyID = 100001;

        protected string currentDate = DateTime.Today.ToShortDateString();
        
        protected void Form(int noLines, int formWidth, int startRow, int startCol)
        {
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
        //Clear unneccesary message
        protected void ClearArea(int top, int left, int height, int width)
        {
            ConsoleColor colorbefore = Console.BackgroundColor;
            try
            {
                Console.BackgroundColor = ConsoleColor.Black;
                string spaces = new string(' ', width);
                for (int i = 0; i < height; i++)
                {
                    Console.SetCursorPosition(left, top + i);
                    Console.Write(spaces);
                }
            }
            finally
            {
                Console.BackgroundColor = colorbefore;
            }
        }
    }
    public class ProgramDemo : form
    {
        //Generate KeyID
        public int generateKey()
        {
            int userKey = Interlocked.Increment(ref keyID);
            checkKey(userKey);
            return userKey;
        }
        public int checkKey(int accountNum)
        {
            string filePath = Convert.ToString(accountNum);
            FileInfo fileInfo = new FileInfo(filePath + ".txt");
            do
            {
                if (!File.Exists(filePath + ".txt"))
                {
                    return accountNum;
                    break;
                }
                accountNum++;
                return accountNum;
            } while (true);
        }
        static void Main(string[] args)
        {
            Console.Title = "UTS Bank System";
            Interface ConsoleInterface = new Interface();
            ConsoleInterface.LoginSreen(10, 40, 2, 10);
            Console.ReadKey();
        }
    }

    public class Interface : form
    {
        ProgramDemo setting = new ProgramDemo();
        Display displayDemo = new Display();
        Validator validator = new Validator();
        Balance userBalance = new Balance();

        //Part 1. Login
        public void LoginSreen(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();

            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;

            string[] loginWindowFields = { "Username : ", "Password : " };

            int[,] loginFieldPos = new int[2, 2];
            string[] loginUserInputs = new string[2]; //two: username and password
            Form(noLines, formWidth, startRow, startCol);

            //Display form headings and other details
            WriteAt("Welcome to My Bank", startCol + 11, startRow + 1);
            WriteAt("Login to Start", startCol + 13, startRow + 4);

            //Display the field names
            int item = 0;
            foreach (string fieldName in loginWindowFields)
            {
                WriteAt(fieldName, startCol + 6, startRow + 6 + item);
                loginFieldPos[item, 0] = Console.CursorLeft;
                loginFieldPos[item, 1] = Console.CursorTop;

                item++;
            }
            do
            {
                ClearArea(8, 27, 2, 20);
                ClearArea(12, 10, 1, 25);
                for (int field = 0; field < item; field++)
                {
                    Console.SetCursorPosition(loginFieldPos[field, 0], loginFieldPos[field, 1]);

                    if (field == item - 1)
                    {
                        var password = ReadPassword();
                        loginUserInputs[1] = password;
                    }
                    else
                    {
                        loginUserInputs[0] = Console.ReadLine();
                    }
                }

                    StreamReader sr = new StreamReader("login.txt");

                    sr.BaseStream.Position = 0;
                    while (sr.EndOfStream == false)
                    {
                        string key = sr.ReadLine();
                        bool checkId = key.Contains(loginUserInputs[0]);
                        if (checkId == true)
                        {
                            string isUserName = key.Substring(0, loginUserInputs[0].Length);
                            string isUserPassword = key.Substring(loginUserInputs[0].Length + 1);

                            if (loginUserInputs[0].CompareTo(isUserName) == 0 && loginUserInputs[1].CompareTo(isUserPassword) == 0)
                            {
                                WriteAt("Valid Credentials.. Please enter", startCol, noLines + 2);
                                Console.ReadKey();
                                using (StreamWriter sw = File.AppendText("log.txt"))
                                {
                                    sw.WriteLine(currentDate + "|" + isUserName + "|" + isUserPassword);
                                    sw.Close();
                                }
                                MainSreen(11, 50, 2, 10);
                                break;
                            }
                            else
                            {
                                WriteAt("Invalid Credentials", startCol, noLines + 2);
                                Console.ReadKey();
                            }
                        }
                    }
                    sr.Close();
            }

            while (true);
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

        //Part 2. Main Menu
        public void MainSreen(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();
            Form(noLines, formWidth, startRow, startCol);
            WriteAt("Welcome to Simple Bank System", startCol + 10, startRow + 1);
            string[] mainWindowFields = { "1. Create a New Account", "2. Search an Account", "3. Deposit", "4. Withdraw", "5. A/C statement", "6. Delete account", "7. Exit" };
            int item = 0;
            foreach (string input in mainWindowFields)
            {
                WriteAt(input, startCol + 6, startRow + 3 + item);
                item++;
            }
            do
            {
                ClearArea(14, 15, 1, 40);
                WriteAt("Enter your choice: ", startCol + 6, startRow + 11);
                switch (Console.ReadLine())
                {
                    case "1":
                        CreateAccount(16, 50, 2, 10);
                        break;
                    case "2":
                        SearchAccount(8, 50, 2, 10);
                        break;
                    case "3":
                        Deposit(8, 50, 2, 10);
                        break;
                    case "4":
                        Withdrawal(8, 50, 2, 10);
                        break;
                    case "5":
                        Statement(8, 50, 2, 10);
                        break;
                    case "6":
                        Delete(7, 50, 2, 10);
                        break;
                    case "7":
                        Exit(8, 50, 2, 10);
                        break;
                    default:
                        WriteAt("Unknown Command, Re-Enter", startCol + 6, noLines + 3);
                        Console.ReadKey();
                        break;
                }
            }
            while (true);
            //Console.ReadKey();
        }

        //Part 3. Create a New Account
        public void CreateAccount(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();
            string[] createFields = { "First Name: ", "Last Name: ", "Address: ", "Phone: ", "Email: " };
            string[] createUserInputs = new string[5];
            int[,] createFieldPos = new int[5, 2];

            Form(16, 50, 2, 10);

            //Description
            WriteAt("Create A New Account", startCol + 15, startRow + 1);
            WriteAt("Enter the Details", startCol + 16, startRow + 3);

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
                while (true)
                {
                    for (int field = 0; field < item; field++)
                    {
                        Console.SetCursorPosition(createFieldPos[field, 0], createFieldPos[field, 1]);
                        createUserInputs[field] = Console.ReadLine();
                        string array = string.Join("\n", createUserInputs);
                    }
                    //Check whether null value
                    if (createUserInputs[0] == "" || createUserInputs[1] == "" || createUserInputs[2] == "")
                    {
                        WriteAt("Something is missing.", startCol + 5, noLines + 2);
                    }
                    //Check integer data type and not more than 10 characters
                    else if (validator.isInt(createUserInputs[3]) == false || createUserInputs[3].Length > 10 || createUserInputs[3] == null)
                    {
                        WriteAt("Wrong Phone Form", startCol + 5, noLines + 3);
                    }
                    //Check email input contains '@'
                    else if (validator.IsValidEmail(createUserInputs[4]) == false || createUserInputs[4] == null)
                    {
                        WriteAt("Wrong Email Form", startCol + 5, noLines + 4);
                    }
                    else 
                    {
                        break;
                    }
                }
                do
                {
                    ClearArea(18, 0, 3, 60);
                    WriteAt("Is the Information correct (y/n)? ", startCol + 5, startRow + 13);
                    WriteAt("Want to go back home, enter 'h' ", startCol + 5, startRow + 14);

                    string input = Console.ReadLine();
                    if (input == "y")
                    {
                        int accountNum = setting.generateKey();
                        int checkedNum;
                        while (true)
                        {
                            accountNum++;
                            checkedNum = accountNum;
                            if (!File.Exists(accountNum + ".txt"))
                            {
                                break;
                                checkedNum = accountNum;
                            }
                        }

                        string accountNumber = checkedNum.ToString();

                        string[] result = new string[createFields.Length];

                        for (int i = 0; i < createFields.Length; i++)
                        {
                            result[i] = createFields[i] + createUserInputs[i];
                        }

                        //Resize the array for accountNumber and Balance amount
                        Array.Resize(ref result, result.Length + 2);
                        string add = "Account No: " + accountNumber;
                        string balance = "Balance: 0 \n\n";
                        result[result.Length - 2] = add;
                        result[result.Length - 1] = balance;

                        //Convert array to String with line break
                        string result2 = string.Join("\n", result);
                        try
                        {
                            File.WriteAllLines(accountNumber + ".txt", result, Encoding.UTF8);
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

                        string userEmail = createUserInputs[4];
                        sendEmail(userEmail, "Bank Account Detail", result2);

                        WriteAt("Account Created! Details will be provided via email.", startCol, startRow + 16);
                        WriteAt("Your account Number is: " + accountNumber, startCol, startRow + 17);

                        Console.ReadKey();
                        MainSreen(11, 50, 2, 10);
                        break;
                    }
                    else if (input == "n")
                    {
                        Console.Clear();
                        CreateAccount(15, 50, 2, 10);
                        break;
                    }
                    else if (input == "h")
                    {
                        MainSreen(11, 50, 2, 10);
                        break;
                    }
                    else
                    {
                        WriteAt("Unknown Command, Re-Enter", startCol, noLines + 2);
                        Console.ReadKey();
                    }
                } while (true);

            } while (true);
        }        

        //Setting smtp email server
        public void sendEmail(string userEmail, string subject, string body)
        {
            MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new System.Net.Mail.MailAddress("asdtesting789@gmail.com", "UTS Bank System");
            msg.To.Add(userEmail);
            msg.Subject = subject;
            msg.Body = body;
            msg.BodyEncoding = System.Text.Encoding.UTF8;

            try
            {
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;

                System.Net.NetworkCredential basiccAuthenticationInfo = new System.Net.NetworkCredential("asdtesting789@gmail.com", "app2021test");
                smtp.Credentials = basiccAuthenticationInfo;
                //smtp.Credentials = new System.Net.NetworkCredential(address.Address, password);
                smtp.Send(msg);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to send a message.");
            }
        }

        //Part 4. Search for an Account
        public void SearchAccount(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();
            Form(noLines, formWidth, startRow, startCol);

            //Description
            WriteAt("SEARCH AN ACCOUNT", startCol + 17, startRow + 1);
            WriteAt("ENTER THE DETAILS", startCol + 17, startRow + 3);

            //InputField
            do
            {
                ClearArea(7, 20, 1, 20);
                WriteAt("Account Number: ", startCol + 3, startRow + 5);
                string checkAccount = Console.ReadLine();
                if (validator.isInt(checkAccount) == true && checkAccount.Length < 10 && checkAccount != null)
                {
                    FileInfo fileInfo = new FileInfo(checkAccount + ".txt");
                    do
                    {
                        if (fileInfo.Exists)
                        {
                            WriteAt("Account found!", startCol, startRow + 8);
                            displayDemo.displayAccount(10, 50, 12, 10, checkAccount);
                            do
                            {
                                WriteAt("Check another account (y/n)?", startCol, startRow + 20);
                                string input = Console.ReadLine();
                                if (input == "y")
                                {
                                    SearchAccount(8, 50, 2, 10);
                                    break;
                                }
                                else if (input == "n")
                                {
                                    MainSreen(11, 50, 2, 10);
                                    break;
                                }
                                else
                                {
                                    WriteAt("Unknown Command, Re-Enter", startCol, startRow + 22);
                                    Console.ReadKey();
                                }
                            } while (true);
                        }
                        else
                        {
                            ClearArea(10, 10, 1, 50);
                            WriteAt("Account not found", startCol, startRow + 9);
                            WriteAt("Check another account (y/n)?", startCol, startRow + 8);

                            string input = Console.ReadLine();
                            if (input == "y")
                            {
                                SearchAccount(8, 50, 2, 10);
                                break;
                            }
                            else if (input == "n")
                            {
                                MainSreen(11, 50, 2, 10);
                                break;
                            }
                            else
                            {
                                WriteAt("Unknown Command, Re-Enter", startCol, startRow + 10);
                                Console.ReadKey();
                            }
                        }
                    } while (true);
                }
                else
                {
                    WriteAt("Wrong Format, Please re-enter your Account Number", startCol, startRow + 8);
                }
            } while (true);
            Console.ReadKey();
        }
        
        //Part 5. Deposit
        public void Deposit(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();
            Form(noLines, formWidth, startRow, startCol);

            //Description
            WriteAt("DEPOSIT", startCol + 20, startRow + 1);
            WriteAt("ENTER THE DETAILS", startCol + 16, startRow + 3);

            do
            {
                ClearArea(7, 20, 1, 20);
                WriteAt("Account Number: ", startCol + 3, startRow + 5);
                string checkAccount = Console.ReadLine();

                if (validator.isInt(checkAccount) == true && checkAccount.Length < 10 && checkAccount != null)
                {
                    FileInfo fileInfo = new FileInfo(checkAccount + ".txt");
                    do
                    {
                        if (fileInfo.Exists)
                        {
                            ClearArea(10, 5, 1, 60);
                            WriteAt("Account found! Enter the amount...", startCol, startRow + 9);
                            WriteAt("Amount: $", startCol + 3, startRow + 6);
                            string amount = Console.ReadLine();

                            if (validator.isInt(amount) == true && Convert.ToInt32(amount) > 0)
                            {
                                try
                                {
                                    double value;
                                    double.TryParse(amount, out value);
                                    double previousBalance = Convert.ToDouble(userBalance.getBalance(checkAccount));
                                    double total = previousBalance + value;
                                    string totalBalance = Convert.ToString(total);

                                    using (StreamWriter sw = File.AppendText(checkAccount + ".txt"))
                                    {
                                        sw.WriteLine(currentDate + " | Deposit | " + amount + " | " + totalBalance);
                                        sw.Close();
                                    }
                                    ClearArea(10,10,1,30);
                                    WriteAt("Deposit successful !", startCol, startRow + 10);

                                    userBalance.Update(checkAccount, totalBalance);

                                    Console.ReadKey();
                                    MainSreen(11, 50, 2, 10);
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
                            }
                            else
                            {
                                WriteAt("Wrong Format !", startCol, noLines + 2);
                                ClearArea(8, 22, 1, 30);
                            }
                        }
                        else
                        {
                            do
                            {
                                ClearArea(10, 5, 1, 60);
                                WriteAt("Account not found", startCol, startRow + 9);
                                WriteAt("Retry (y/n)?", startCol, startRow + 10);
                                string input = Console.ReadLine();
                                if (input == "y")
                                {
                                    Deposit(8, 50, 2, 10);
                                    break;
                                }
                                else if (input == "n")
                                {
                                    MainSreen(11, 50, 2, 10);
                                    break;
                                }
                                else
                                {
                                    WriteAt("Unknown Command, Re-Enter", startCol, startRow + 11);
                                }
                            } while (true);
                        }
                    } while (true);
                }
                else
                {
                    WriteAt("Wrong Format, Please re-enter your Account Number", startCol, noLines + 2);
                    Console.ReadKey();
                }
            } while (true);
            Console.ReadKey();
        }

        //Part 6. Withdrawal
        public void Withdrawal(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();
            Form(noLines, formWidth, startRow, startCol);

            //Description
            WriteAt("WITHDRAW", startCol + 20, startRow + 1);
            WriteAt("ENTER THE DETAILS", startCol + 16, startRow + 3);
            do
            {
                ClearArea(7, 20, 1, 20);
                WriteAt("Account Number: ", startCol + 3, startRow + 5);
                string checkAccount = Console.ReadLine();
            
                if (validator.isInt(checkAccount) == true && checkAccount.Length < 10 && checkAccount != null)
                {
                    FileInfo fileInfo = new FileInfo(checkAccount + ".txt");
                    do
                    {
                        if (fileInfo.Exists)
                        {
                            do
                            {
                                ClearArea(13, 10, 1, 40);
                                WriteAt("Account found! Enter the amount...", startCol, startRow + 8);
                                WriteAt("Amount: $", startCol + 3, startRow + 6);
                                string amount = Console.ReadLine();

                                if (validator.isInt(amount) == true)
                                {
                                    double previousBalance = Convert.ToDouble(userBalance.getBalance(checkAccount));
                                    double value = Convert.ToDouble(amount);
                                    double total = previousBalance - value;
                                    string totalBalance = Convert.ToString(total);

                                    double checkBalance = Convert.ToDouble(userBalance.getBalance(checkAccount));

                                    if (checkBalance >= value && value != 0)
                                    {
                                        try
                                        {
                                            using (StreamWriter sw = File.AppendText(checkAccount + ".txt"))
                                            {
                                                sw.WriteLine(currentDate + " | Withdraw | " + amount + " | " + totalBalance);
                                                sw.Close();
                                            }
                                            WriteAt("Withdraw successful !", startCol, startRow + 9);
                                            userBalance.Update(checkAccount, totalBalance);
                                            Console.ReadKey();
                                            MainSreen(11, 50, 2, 10);
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
                                    }
                                    else
                                    {
                                        do
                                        {
                                            ClearArea(12, 10, 2, 50);
                                            WriteAt("You entered Exceed Amount or Less than 0 !", startCol, noLines + 4);
                                            WriteAt("Want re-try (y/n)?", startCol, noLines + 5);
                                            string userInput = Console.ReadLine();
                                            if (userInput == "y")
                                            {
                                                ClearArea(12, 10, 3, 50);
                                                ClearArea(8, 20, 1, 20);
                                                break;
                                            }
                                            else if (userInput == "n")
                                            {
                                                MainSreen(11, 50, 2, 10);
                                                break;
                                            }
                                            else
                                            {
                                                WriteAt("Unknown Command, Re-Enter", startCol, noLines + 6);
                                            }
                                        } while (true);
                                    }
                                }
                                else
                                {
                                    WriteAt("Wrong Format !", startCol, noLines + 3);
                                    ClearArea(8, 20, 1, 20);
                                }
                            } while (true);
                        }
                        else
                        {
                            do
                            {
                                ClearArea(10, 10, 1, 50);
                                WriteAt("Account not found", startCol, startRow + 9);
                                WriteAt("Retry (y/n)?", startCol, startRow + 10);
                                string input = Console.ReadLine();
                                if (input == "y")
                                {
                                    Withdrawal(8, 50, 2, 10);
                                    break;
                                }
                                else if (input == "n")
                                {
                                    MainSreen(11, 50, 2, 10);
                                    break;
                                }
                                else
                                {
                                    WriteAt("Unknown Command, Re-Enter", startCol, startRow + 11);
                                }
                            } while (true);
                            //ClearArea(9, 20, 1, 20);
                        }
                    } while (true);
                }
                else
                {
                    WriteAt("Wrong Format, Please re-enter your Account Number", startCol, noLines + 2);
                }
            } while (true);
            Console.ReadKey();
        }
 
        //Part 7. Account Statement
        public void Statement(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();
            Form(noLines, formWidth, startRow, startCol);

            //Description
            WriteAt("STATEMENT", startCol + 20, startRow + 1);
            WriteAt("ENTER THE DETAILS", startCol + 16, startRow + 3);
            do
            {
                ClearArea(7, 20, 1, 20);
                WriteAt("Account Number: ", startCol + 3, startRow + 5);
                string checkAccount = Console.ReadLine();
                if (validator.isInt(checkAccount) == true && checkAccount.Length < 10 && checkAccount != null)
                {
                    FileInfo fileInfo = new FileInfo(checkAccount + ".txt");
                    do
                    {
                        if (fileInfo.Exists)
                        {
                            ClearArea(11, 10, 1, 50);
                            WriteAt("Account found! The statement is displayed below..", startCol, startRow + 8);
                            displayDemo.displayStatement(13, 50, 12, 10, checkAccount);

                            StreamReader sr = new StreamReader(checkAccount + ".txt");
                            string[] description = new string[7];

                            for (int i = 0; i < 7; i++)
                            {
                                description[i] = sr.ReadLine();
                            }
                            do
                            {
                                WriteAt("Email Statement (y/n)? ", startCol, startRow + 24);

                                string input = Console.ReadLine();
                                if (input == "y")
                                {
                                    string emailLine = File.ReadLines(checkAccount + ".txt").Skip(4).Take(1).First();
                                    string emailDetail = emailLine.Substring(7);

                                    string[] statement = File.ReadLines(checkAccount + ".txt").Skip(0).Take(7).ToArray();
                                    string detail = string.Join("\n", statement);

                                    sendEmail(emailDetail, "Bank Account Detail", detail);
                                    WriteAt("Email Sent Successfully..!", startCol, startRow + 26);

                                    sr.Close();
                                    Console.ReadKey();
                                    MainSreen(11, 50, 2, 10);
                                    break;
                                }
                                else if (input == "n")
                                {
                                    sr.Close();
                                    MainSreen(11, 50, 2, 10);
                                    break;
                                }
                                else
                                {
                                    WriteAt("Unknown Command, Re-Enter", startCol, startRow + 26);
                                }
                            } while (true);
                            sr.Close();
                        }
                        else
                        {
                            do
                            {
                                ClearArea(11, 10, 1, 50);
                                WriteAt("Account not found", startCol, noLines + 2);
                                WriteAt("Check another account (y/n)?", startCol, noLines + 3);
                                string input = Console.ReadLine();
                                if (input == "y")
                                {
                                    Statement(8, 50, 2, 10);
                                    break;
                                }
                                else if (input == "n")
                                {
                                    MainSreen(11, 50, 2, 10);
                                    break;
                                }
                                else
                                {
                                    WriteAt("Unknown Command, Re-Enter", startCol, noLines + 4);
                                    Console.ReadKey();
                                }
                            } while (true);
                        }
                    } while (true);
                }
                else
                {
                    WriteAt("Wrong Format, Please re-enter your Account Number", startCol, noLines + 3);
                }
            } while (true);
            Console.ReadKey();
        }

        //Part 8. Delete and Account
        public void Delete(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();
            Form(noLines, formWidth, startRow, startCol);

            //Description
            WriteAt("DELETE AN ACCOUNT", startCol + 16, startRow + 1);
            WriteAt("ENTER THE DETAILS", startCol + 16, startRow + 3);
            do
            {
                ClearArea(7, 20, 1, 20);
                WriteAt("Account Number: ", startCol + 3, startRow + 5);
                string checkAccount = Console.ReadLine();
                if (validator.isInt(checkAccount) == true && checkAccount.Length < 10 && checkAccount != null)
                {
                    FileInfo fileInfo = new FileInfo(checkAccount + ".txt");
                    do
                    {
                        if (fileInfo.Exists)
                        {
                            ClearArea(10, 20, 1, 50);
                            WriteAt("Account found! Details displayed below..", startCol, startRow + 8);
                            displayDemo.displayDetail(12, 50, 12, 10, checkAccount);
                            do
                            {
                                WriteAt("Are you sure Delete (y/n)? ", startCol, startRow + 24);
                                string input = Console.ReadLine();
                                if (input == "y")
                                {
                                    try
                                    {
                                        System.IO.File.Delete(checkAccount + ".txt");
                                    }
                                    catch (System.IO.IOException e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                    WriteAt("Account Deleted Successfully ", startCol, startRow + 26);

                                    Console.ReadKey();
                                    MainSreen(11, 50, 2, 10);
                                    break;
                                }
                                else if (input == "n")
                                {
                                    MainSreen(11, 50, 2, 10);
                                    break;
                                }
                                else
                                {
                                    WriteAt("Unknown Command, Re-Enter", startCol, startRow + 26);
                                    Console.ReadKey();
                                }
                            } while (true);
                        }
                        else
                        {
                            ClearArea(10, 10, 1, 50);
                            WriteAt("Account not found", startCol, startRow + 8);
                            WriteAt("Check another account (y/n)?", startCol, startRow + 9);

                            string input = Console.ReadLine();
                            if (input == "y")
                            {
                                Delete(7, 50, 2, 10);
                                break;
                            }
                            else if (input == "n")
                            {
                                MainSreen(11, 50, 2, 10);
                                break;
                            }
                            else
                            {
                                WriteAt("Unknown Command, Re-Enter", startCol, startRow + 10);
                            }
                        }
                    } while (true);
                }
                else
                {
                    WriteAt("Wrong Format, Please re-enter your Account Number", startCol, noLines + 3);
                }
            } while (true);
            Console.ReadKey();
        }
        
        //Part 9. Exit
        public void Exit(int noLines, int formWidth, int startRow, int startCol)
        {
            Console.Clear();
            Form(noLines, formWidth, startRow, startCol);

            //Description
            do
            {
                ClearArea(10, 10, 1, 40);
                ClearArea(6, 45, 1, 5);
                WriteAt("EXIT", startCol + 22, startRow + 1);
                WriteAt("Are you want to exit (y/n)? ", startCol + 8, startRow + 4);
            
                string userInput = Console.ReadLine();
            
                if (userInput == "y")
                {
                    Environment.Exit(0);
                    break;
                }
                else if (userInput == "n")
                {
                    MainSreen(11, 50, 2, 10);
                    break;
                }
                else
                {
                    WriteAt("Unknown Command, Re-Enter", startCol, noLines + 2);
                    Console.ReadKey();
                }
            } while (true);
            Console.ReadKey();
        }
    }
    public class Balance
    {
        //Get the previous balance
        public string getBalance(string filePath)
        {
            string[] lines = System.IO.File.ReadAllLines(filePath + ".txt");
            string gather = string.Join(":", lines);
            string[] splits = gather.Split(':');
            string balance = splits[13];
            return balance;
        }
        public void Update(string checkAccount, string total)
        {
            string filePath = checkAccount;
            string totalBalance = total;
            try
            {
                StreamReader reader = new StreamReader(filePath + ".txt");
                string values = reader.ReadToEnd();
                reader.BaseStream.Position = 0;

                while (reader.EndOfStream == false)
                {
                    string fromBalance = File.ReadLines(filePath + ".txt").Skip(6).Take(1).First();
                    string balanceValue = fromBalance.Substring(9);

                    string key = reader.ReadLine();
                    bool check = key.Contains("Balance");

                    if (check == true)
                    {
                        int offest = values.IndexOf(key, 0);
                        key = key.Replace(balanceValue, totalBalance);
                        values = values.Remove(offest, key.Length);
                        values = values.Insert(offest, key);
                    }
                }
                reader.Close();

                StreamWriter writer = new StreamWriter(filePath + ".txt");

                writer.Write(values);
                writer.Close();
            }
            catch(IOException e)
            {
                Console.WriteLine(e);
            }
        }
    }

    public class Display : form
    {
        //DisplayForm
        public void displayAccount(int noLines, int formWidth, int startRow, int startCol, string checkAccount)
        {
            Form(noLines, formWidth, startRow, startCol);
            //Description
            WriteAt("ACCOUNT DETAIL", startCol + 16, startRow + 1);
            description(12, 10, 3, 3, checkAccount, 6);

        }
        public void displayDetail(int noLines, int formWidth, int startRow, int startCol, string userAccount) 
        {
            Form(noLines, formWidth, startRow, startCol);
            //Description
            WriteAt("ACCOUNT DETAILS", startCol + 17, startRow + 1);
            description(12, 10, 4, 3, userAccount, 7);
        }
        public void displayStatement(int noLines, int formWidth, int startRow, int startCol, string userAccount) 
        { 
            Form(noLines, formWidth, startRow, startCol);
            //Description
            WriteAt("SIMPLE BANKIING SYSTEM", startCol + 15, startRow + 1);
            WriteAt("Account Statement", startCol + 16, startRow + 3);
            description(12, 10, 5, 3, userAccount, 7);
        }
        public void description(int startRow, int startCol, int addRow, int addCol, string userAccount, int value)
        {
            string[] lines = System.IO.File.ReadAllLines(userAccount + ".txt");
            string[] specfic = new string[value];
            Array.Copy(lines, specfic, value);

            int item = 0;
            foreach (string line in specfic)
            {
                WriteAt(line, startCol + addCol, startRow + addRow + item);
                item++;
            }
            return;
        }
    }
    public class Validator
    {
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public bool isInt(string inputString)
        {
            int num = 0;
            return int.TryParse(inputString, out num);
        }
    }
}
