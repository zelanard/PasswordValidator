using System.Runtime.CompilerServices;

namespace PasswordValidator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunValidator();
        }

        #region View

        /// <summary>
        /// Writes message to the console. <br/>
        /// rgb takes green, yellow or red as an argument for foreground colour, which is set for this output only.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="rgb"></param>
        private static void WriteMessage(string message, string rgb = "")
        {
            switch (rgb.ToLower())
            {
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// gets a text message from the screen.
        /// </summary>
        /// <returns></returns>
        private static string ReadInput()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Takes a key input of either enter, to retrn true or escape to return fales.
        /// </summary>
        /// <returns></returns>
        private static bool Proceed()
        {
            ConsoleKey key;
            while (true)
            {
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.Enter)
                {
                    return true;
                }
                else if (key == ConsoleKey.Escape)
                {
                    return false;
                }
                else
                {
                }
            }
        }

        #endregion


        #region Model
        //I18n English - Localization
        private static string[] English = new string[]
        {
            "This is a password validator.\r\n" +
            "These are the rules you must abide by, to get a valid password.\r\n\r\n" +
            "Passowrd must contain: \r\n" +
            "   12 to 64 characters.\r\n" +
            "   Both uppper and lower case characters.\r\n" +
            "   At least one number.\r\n" +
            "   At least one special character.\r\n\r\n" +
            "The password is considered weak if:\r\n" +
            "   It contains a sequence such as 1234, 4321, abcd, dcba, etc.\r\n" +
            "   it contains 4 repeating characters.\r\n\r\n" +
            "Red: The password is invalid.\r\n" +
            "Yellow: The password is Valid but Weak.\r\n" +
            "Green: the password is valid and strong.\r\n",                 // 0
            "You must type a password!",        // 1
            "Invalid Password: Your password must be between 12 and 64 characters!",            // 2
            "Invalid Password: Your password must contain upper case letters!",                 // 3
            "Invalid Password: Your password must contain lower case letters!",                 // 4
            "Invalid Password: Your password must contain at least one number!",                // 5
            "Invalid Password: Your password must contain at least one special character!",     // 6
            "\r\nYour password is valid but weak.",           // 7
            "Weak Password: Your password contains repeating characters.",               // 8
            "Weak Password: Your password contains sequential characters.",               // 9
            "\r\nYour Password is strong.",            // 10
            "Press Enter to validate another password.\r\n Press Escape to Exit the application.", // 11
            "Please enter password to validate:", // 12
            "\r\nYour password is invalid!"
        };
        private static string? input; //the ? operator makes the string nullable.

        /** Validity
         * 0 = Length evaluation
         * 1 = Uppper case 
         * 2 = Lower case
         * 3 = Number
         * 4 = Special character
         */
        private static bool[] Validity = new bool[5];
        /** Strength
         * 0 = Sequential Input
         * 1 = Repeated Characters
         */
        private static bool[] Strength = new bool[2];

        /// <summary>
        /// Checks if the input value is null or empty and return false if it is.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>True/False</returns>
        private static bool IsInputValid(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Set Validity position 0 to true if password is longer than 12 and shorter than 64. <br/>
        /// Else we set Validity position 0 to false.
        /// </summary>
        /// <param name="input"></param>
        private static void EvaluateLength(string input)
        {
            if (input.Length > 11 && input.Length < 65)
            {
                Validity[0] = true;
                return;
            }
            Validity[0] = false;
        }

        /// <summary>
        /// Set Validity position 1 to true if the input string contains at least one upper case character. <br/>
        /// Else we set Validity position 1 to false.
        /// </summary>
        /// <param name="input"></param>
        private static void ContainsUpper(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsUpper(input[i]))
                {
                    Validity[1] = true;
                    return;
                }
            }
            Validity[1] = false;
        }

        /// <summary>
        /// Set Validity position 2 to true if the input string contains at least one lower case character. <br/>
        /// Else we set Validity position 2 to false.
        /// </summary>
        /// <param name="input"></param>
        private static void ContainsLower(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsLower(input[i]))
                {
                    Validity[2] = true;
                    return;
                }
            }
            Validity[2] = false;
        }

        /// <summary>
        /// Set Validity position 3 to true if the input string contains at least one number. <br/>
        /// Else we set Validity position 3 to false.
        /// </summary>
        /// <param name="input"></param>
        private static void ContainsNumber(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsNumber(input[i]))
                {
                    Validity[3] = true;
                    return;
                }
            }
            Validity[3] = false;
        }

        /// <summary>
        /// Set Validity position 4 to true if the input string contains at least one special character. <br/>
        /// Else we set Validity position 4 to false.
        /// </summary>
        /// <param name="input"></param>
        private static void ContainsSpeical(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!Char.IsLetter(input[i]) && !Char.IsNumber(input[i]))
                {
                    Validity[4] = true;
                    return;
                }
            }
            Validity[4] = false;
        }

        /// <summary>
        /// Run all the validators and loop through the Validity array. If any element in the array is false, we return false.<br/>
        /// If we don't return false in the loop, we return true after the loop.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool IsPasswordValid(string input)
        {
            EvaluateLength(input);
            ContainsUpper(input);
            ContainsLower(input);
            ContainsNumber(input);
            ContainsSpeical(input);

            for (int i = 0; i < Validity.Length; i++)
            {
                if (Validity[i])
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Set Strength position 0 to true if the input string contains no sequence of 4. <br/>
        /// Else we set Validity position 1 to false.
        /// </summary>
        /// <param name="input"></param>
        private static void ContainsSequence(string input)
        {
            int seqLen = 1; //use 1 because the first character in a sequence will always be the first character in a sequence.
            for (int i = 0; i < input.Length - 1; ++i) //We do not need test on the last character.
            {
                //is the current character one lower than the next character?
                if (input[i] + 1 == input[i + 1])
                {
                    seqLen++;
                }
                else
                {
                    seqLen = 1;
                }

                //is the current character one lower than the next character?
                if (input[i] + 1 == input[i + 1])
                {
                    seqLen++;
                }
                else
                {
                    seqLen = 1;
                }

                if (seqLen == 4)
                {
                    Strength[0] = false;
                    return;
                }
                Strength[0] = true;
            }
        }

        /// <summary>
        /// Set Strenth position 1 to true if the input string contains 4 repeating characters. <br/>
        /// Ese we set Strength position 1 to false.
        /// </summary>
        /// <param name="input"></param>
        private static void ContainsRepetition(string input)
        {
            int seqLen = 1; //use 1 because the first character in a sequence will always be the first character in a sequence.
            for (int i = 0; i < input.Length - 1; ++i)
            {
                //is the current character equevelant to the next character?
                if (input[i] == input[i + 1])
                {
                    seqLen++;
                }
                else
                {
                    seqLen = 1;
                }
                if (seqLen == 4)
                {
                    Strength[1] = false;
                    return;
                }
                Strength[1] = true;
            }
        }

        /// <summary>
        /// Run all the strength validators and loop through the Strength array. If any element in the array is false, we return false.<br/>
        /// If we don't return false in the loop, we return true after the loop.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool IsPasswordStrong(string input)
        {
            ContainsSequence(input);
            ContainsRepetition(input);

            for (int i = 0; i < Strength.Length; i++)
            {
                if (Strength[i])
                {
                    continue;
                }
                return false;
            }
            return true;

        }

        #endregion


        #region Controller

        /// <summary>
        /// Organize the code to run the password validator.
        /// </summary>
        private static void RunValidator()
        {
            WriteMessage(English[0]); // write the welcome message
            while (true)
            {
                WriteMessage(English[12]); // prompt the user to enter a password
                input = ReadInput();
                if (IsInputValid(input))
                {
                    if (IsPasswordValid(input))
                    {
                        if (IsPasswordStrong(input))
                        {
                            WriteMessage(English[10], "green"); //inform the user that the password is both valid and strong.
                        }
                        else
                        {
                            //Password is valid but not strong.
                            WriteMessage(English[7]); //inform the user that the password is weak.
                            for (int i = 0; i < Strength.Length; i++)
                            {
                                if (!Strength[i])
                                {
                                    WriteMessage(English[i + 8], "yellow"); //inform the user of why the passwork is weak.
                                }
                            }
                        }
                    }
                    else
                    {
                        //iform the user that the Password is invalid.
                        WriteMessage(English[13]);
                        for (int i = 0; i < Validity.Length; i++)
                        {
                            if (!Validity[i])
                            {
                                WriteMessage(English[i + 2], "red"); //inform the user of why the password is invalid.
                            }
                        }
                        continue;
                    }
                }
                else
                {
                    WriteMessage(English[1]); //inform user that he must type a password
                    continue;
                }
                WriteMessage(English[11]); //prmopt user to press escape or enter, to leave the validator or validate a new password
                if (!Proceed())
                {
                    return;
                }
            }
        }

        #endregion
    }
}