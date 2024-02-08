using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.Properties;

namespace EasySave
{
    internal class View
{
    private static int languages;
    private static ViewModel viewModel;
    private static Data[] datas;
    private static bool exitRequested = false;
    
        /// <summary>
        /// Entry point of the app
        /// </summary>
        /// <param name="args"></param>
    static void Main(string[] args)
    {
            //By default, the selected language is english
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        viewModel = new ViewModel();
        datas = viewModel.Model.Datas;
        ChooseLanguages();
        while (!exitRequested)
        {
            ChooseSlot();
        }
    }

/// <summary>
/// Display and set up the languages
/// </summary>
    static void ChooseLanguages()
    {
        bool continueInput = true;
        while (continueInput)
        {
            Console.WriteLine("Choose language: \n  1 : Français \n  2 : English \n  q : Quit");
            string result = Console.ReadLine();
            if (result.ToLower() == "q")
            {
                Environment.Exit(0);
            }
            switch (result)
            {
                case "1":
                    languages = 0;
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr-FR");
                    continueInput = false;
                    break;
                case "2":
                    languages = 1;
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                    continueInput = false;
                    break;
                default:
                    Console.WriteLine("Invalid input!");
                    break;
            }
        }
    }

        /// <summary>
        /// Allow the user to choose a slot
        /// </summary>
    static void ChooseSlot()
    {
        DisplayAvailableSlots();
            //Console.WriteLine("Choose save slots (e.g., 1;2;3) or 'q' to Quit:");
            Console.WriteLine(Properties.ts.ChooseSaveSlots);
            string input = Console.ReadLine();
        if (input.ToLower() == "q")
        {
            Environment.Exit(0);
        }
        List<int> choices = input.Split(';').Select(int.Parse).ToList();
        if (choices.All(choice => IsEmpty(choice - 1)))
        {
            ProcessEmptySlots(choices);
        }
        else if (choices.All(choice => !IsEmpty(choice - 1)))
        {
            ProcessNonEmptySlots(choices);
        }
        else
        {
            //Console.WriteLine("Error: One or more selected slots are empty or non-empty.");
            Console.WriteLine(Properties.ts.ErrorSelectedSlotsEmptyNonEmpty);
            }
    }
        /// <summary>
        /// If the user choose an empty slot, create a new save with the entered information 
        /// </summary>
        /// <param name="choices"></param>
    static void ProcessEmptySlots(List<int> choices)
    {
        foreach (int choice in choices)
        {
                //Console.WriteLine($"Choose current source file for Slot {choice}: ");
                Console.WriteLine(string.Format(Properties.ts.ChooseSlotSourceFile, choice));
                string sourceFile = Console.ReadLine();
            //Console.WriteLine($"Destination file path for Slot {choice}: ");
            Console.WriteLine(string.Format(Properties.ts.ChooseSlotDestination, choice));
                string destinationDirectory = Console.ReadLine();

                bool error = !EditSave.Create(sourceFile, destinationDirectory);
                if(error) 
                {
                    Console.WriteLine("Error in creation of file");
                }
                else
                {
                    var saveData1 = new Data
                    {
                        Name = Path.GetFileName(sourceFile),
                        SourceFilePath = sourceFile,
                        TargetFilePath = destinationDirectory,
                        State = "ACTIVE",
                        TotalFilesToCopy = 3300,
                        TotalFilesSize = 1240312777,
                        NbFilesLeftToDo = 3274,
                        Progression = 0
                    };
                    datas[choice - 1] = saveData1;
                    Data.Serialize(datas);
                    viewModel.Log.Indexes = choices;
                    viewModel.Log.AddLog();
                }
            }
       
        }

        /// <summary>
        /// If the user choose a non Empty save, save or delete it
        /// </summary>
        /// <param name="choices"></param>
    static void ProcessNonEmptySlots(List<int> choices)
    {
        //Console.WriteLine("1 : Save ");
        Console.WriteLine(Properties.ts.OptionSave);
            //Console.WriteLine("2 : Delete ");
            Console.WriteLine(Properties.ts.OptionDelete);
            string result = Console.ReadLine();
        switch (result)
        {
            case "1":
                SaveSelectedSlots(choices);
                viewModel.Log.Indexes = choices;
                viewModel.Log.AddLog();
                break;
            case "2":
                DeleteSelectedSlots(choices);
                break;
            default:
                //Console.WriteLine("Invalid input! Please enter '1' to Save or '2' to Delete.");
                Console.WriteLine(Properties.ts.InvalidInputSaveDelete);
                    break;
        }
    }
        /// <summary>
        /// Save the slot
        /// </summary>
        /// <param name="choices"></param>
    static void SaveSelectedSlots(List<int> choices)
    {
        foreach (int choice in choices)
        {
            //Console.WriteLine($"Saving Slot {choice}");
            Console.WriteLine(string.Format(Properties.ts.SavingSlot, choice));
                EditSave.Update(datas[choice - 1].SourceFilePath, datas[choice - 1].TargetFilePath);
                Data.Serialize(datas);

            }
    }
        /// <summary>
        /// Delet the slot 
        /// </summary>
        /// <param name="choices"></param>
    static void DeleteSelectedSlots(List<int> choices)
    {
        foreach (int choice in choices)
        {
            //Console.WriteLine($"Deleting Slot {choice}");
            Console.WriteLine(string.Format(Properties.ts.DeletingSlot, choice));

                EditSave.Delete(datas[choice - 1].TargetFilePath);
                datas[choice - 1] = null;
                Data.Serialize(datas);
            }
    }
        /// <summary>
        /// Return True if the slot is empty
        /// </summary>
        /// <param name="choice"> The chosen slot </param>
        /// <returns></returns>
    static bool IsEmpty(int choice)
    {
        return datas[choice] == null;
    }
        /// <summary>
        /// Display the avaiable slots
        /// </summary>
    static void DisplayAvailableSlots()
    {
        //Console.WriteLine("Available save slots:");
        Console.WriteLine(Properties.ts.DisplayAvailableSaveSlots);
            for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i] == null)
            {
                //Console.WriteLine($"{i + 1}: Empty");
                Console.WriteLine($"{i + 1}: {Properties.ts.EmptySlot}");
                }
            else
            {
                Console.WriteLine($"{i + 1}: {datas[i].Name}");
            }
        }
        }
    }
}
