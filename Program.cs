using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleVM
{
    class Program
    {
        static bool isRunning = true;

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            string banner = @"
/═══════════════════════════════════════\
║                                       ║
║   SHIRO VM                            ║
║   v1.0                                ║
║   All needs in one place              ║
║                                       ║
\═══════════════════════════════════════/";
            Console.WriteLine(banner);
            Console.WriteLine("Введите HELP для появления списка команд");
            Console.WriteLine("====================\n");
            Console.ForegroundColor = ConsoleColor.Green;

            while (isRunning)
            {
                string currentPath = Directory.GetCurrentDirectory();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"SH {currentPath}> ");
                Console.ForegroundColor= ConsoleColor.Green;
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                string[] parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToUpper();

                try
                {
                    ExecuteCommand(command, parts);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ERROR] {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }
            static void ExecuteCommand(string cmd, string[] parts)
            {
                switch (cmd)
                {
                    case "HELP":
                        Console.WriteLine("---===--- Доступные команды ---===---");
                        Console.WriteLine("  HELP - место, где вы сейчас находитесь");
                        Console.WriteLine("  CALCULATOR - простой калькулятор (прямо в терминале)");
                        Console.WriteLine("  CAT <path> - вывод текста любого текстового файла");
                        Console.WriteLine("  LS, DIR - список файлов и папок");
                        Console.WriteLine("  CD <путь> - сменить директорию (CD .. - наверх, CD ~ - домой)");
                        Console.WriteLine("  PWD - показать текущую директорию");
                        Console.WriteLine("  WHOAMI - текущий пользователь");
                        Console.WriteLine("  MKDIR <имя> - создать папку");
                        Console.WriteLine("  TOUCH <имя> - создать файл");
                        Console.WriteLine("  RM <имя> - удалить файл или папку");
                        Console.WriteLine("  CLS - очистить экран");
                        Console.WriteLine("  EXIT - выход из SHIRO VM");
                        break;
                    case "CALCULATOR":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n=============================\n");
                        Console.WriteLine(@"
/═══════════════════════════════════════\
║                                       ║
║   SHIRO CALCULATOR                    ║
║   v0.1                                ║
║                                       ║
\═══════════════════════════════════════/");
                        Console.WriteLine("\n=============================\n");
                        Console.ForegroundColor = ConsoleColor.Green;

                        Console.Write("calculator> Введите первое число: ");
                        string firstInput = Console.ReadLine();

                        if (!double.TryParse(firstInput, out double firstNumber))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ошибка: это не число!");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nНажмите любую клавишу для возврата...");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }

                        Console.Write("calculator> Введите оператор (+, -, *, /): ");
                        string operatorInput = Console.ReadLine();

                        Console.Write("calculator> Введите второе число: ");
                        string secondInput = Console.ReadLine();

                        if (!double.TryParse(secondInput, out double secondNumber))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ошибка: это не число!");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nНажмите любую клавишу для возврата...");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }

                        double result = 0;
                        bool validOperator = true;

                        switch (operatorInput)
                        {
                            case "+":
                                result = firstNumber + secondNumber;
                                break;
                            case "-":
                                result = firstNumber - secondNumber;
                                break;
                            case "*":
                                result = firstNumber * secondNumber;
                                break;
                            case "/":
                                if (secondNumber == 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Ошибка: деление на ноль!");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    validOperator = false;
                                }
                                else
                                {
                                    result = firstNumber / secondNumber;
                                }
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Ошибка: неизвестный оператор '{operatorInput}'");
                                Console.ForegroundColor = ConsoleColor.Green;
                                validOperator = false;
                                break;
                        }

                        if (validOperator)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\nРезультат: {firstNumber} {operatorInput} {secondNumber} = {result}");
                        }

                        Console.WriteLine("\nНажмите любую клавишу для возврата в VM...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "CAT":
                        if (parts.Length < 2)
                        {
                            throw new Exception("Используйте: CAT <имя_файла>");
                        }

                        string path = parts[1];

                        try
                        {
                            string content = File.ReadAllText(path);
                            Console.WriteLine(content);
                        }
                        catch (FileNotFoundException)
                        {
                            throw new Exception($"Файл '{path}' не найден...");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Ошибка чтения файла: '{ex.Message}'");
                        }
                        break;

                    case "LS":
                    case "DIR":
                        string currentDirectory = Directory.GetCurrentDirectory();
                        var directories = Directory.GetDirectories(currentDirectory);
                        var files = Directory.GetFiles(currentDirectory);

                        Console.ForegroundColor = ConsoleColor.Blue;
                        foreach (var dir in directories)
                        {
                            string dirName = Path.GetFileName(dir);
                            Console.WriteLine($"[DIR] {dirName}");
                        }

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        foreach (var file in files)
                        {
                            string fileName = Path.GetFileName(file);
                            long size = new FileInfo(file).Length;
                            Console.WriteLine($"       {fileName} ({size} bytes)");
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case "CD":
                        if (parts.Length < 2)
                        {
                            break;
                        }

                        string newPath = parts[1];

                        if (newPath == "..")
                        {
                            var parent = Directory.GetParent(Directory.GetCurrentDirectory());
                            if (parent != null)
                            {
                                Directory.SetCurrentDirectory(parent.FullName);
                            }
                            else
                            {
                                throw new Exception("Вы уже в корневой директории!");
                            }
                        }
                        else if (newPath == "~" || newPath == "%HOME%")
                        {
                            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                            Directory.SetCurrentDirectory(home);
                        }
                        else
                        {
                            try
                            {
                                if (Path.IsPathRooted(newPath))
                                {
                                    Directory.SetCurrentDirectory(newPath);
                                }
                                else
                                {
                                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), newPath);
                                    Directory.SetCurrentDirectory(fullPath);
                                }
                            }
                            catch (DirectoryNotFoundException)
                            {
                                throw new Exception($"Директория '{newPath}' не найдена!");
                            }
                        }
                        break;

                    case "PWD":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(Directory.GetCurrentDirectory());
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;

                    case "WHOAMI":
                        string userName = Environment.UserName;
                        string machineName = Environment.MachineName;
                        Console.WriteLine($"{machineName}\\{userName}");
                        break;

                    case "MD":
                    case "MKDIR":
                        if (parts.Length < 2)
                        {
                            throw new Exception("Используйте MKDIR <имя_папки>");
                        }

                        string directoryName = parts[1];
                        try
                        {
                            Directory.CreateDirectory(directoryName);
                            Console.WriteLine($"Директория '{directoryName}' создана");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Не удалось создать директорию '{directoryName}': {ex.Message}");
                        }
                        break;

                    case "TOUCH":
                        if (parts.Length < 2)
                        {
                            throw new Exception("Используйте TOUCH <имя_файла>");
                        }
                        
                        string touchFileName = parts[1];
                        try
                        {
                            File.Create(touchFileName).Close();
                            Console.WriteLine($"Файл '{touchFileName}' успешно создан!");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Не удалось создать файл '{touchFileName}': {ex}");
                        }

                        break;

                    case "RM":
                    case "DEL":
                        if (parts.Length < 2)
                        {
                            throw new Exception("Используйте RM <имя_файла_или_директории>");
                        }

                        string target = parts[1];
                        try
                        {
                            if (Directory.Exists(target))
                            {
                                Directory.Delete(target, true); Console.WriteLine($"Директория '{target}' была удалена");
                            }
                            else if (File.Exists(target))
                            {
                                File.Delete(target); Console.WriteLine($"Файл '{target}' был удалён");
                            }
                            else
                            {
                                Console.WriteLine($"Файл или директория '{target}' не были найдены");
                            }
                        }
                        catch (Exception ex )
                        {
                            throw new Exception($"Ошибка удаления '{target}': {ex.Message}");
                        }

                        break;

                    case "CLEAR":
                    case "CLS":
                        Console.Clear();
                        break;

                    case "EXIT":
                        Console.WriteLine("Спасибо что выбираете нас! Программа закроется через 3 секунды...");
                        Thread.Sleep(3000);
                        isRunning = false;
                        break;
                }
            }
        }
    }
}
