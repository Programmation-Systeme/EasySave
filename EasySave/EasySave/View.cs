﻿using System;
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
            viewModel = new ViewModel();
            datas = viewModel.Model.Datas;

            //Launch by cmd line
            if (args.Length > 0)
            {
                string argument = args[0];

                List<int>? list = viewModel.GetSlotsNumbersFromCMD(argument);
                if(list != null)
                {
                    SaveSelectedSlots(list);
                }
            }
            //Normal launch (execution)
            else
            {
                //By default, the selected language is english
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                ChooseLanguages();
                ChooseLogType();
                while (!exitRequested)
                {
                    ChooseSlot();
                }
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
        /// Allow the user to choose log format
        /// </summary>
        static void ChooseLogType()
        {
            bool continueInput = true;
            while (continueInput)
            {
                Console.WriteLine(Properties.ts.ChooseFormatLogs);
                string result = Console.ReadLine();
                if (result.Equals("q", StringComparison.CurrentCultureIgnoreCase))
                {
                    Environment.Exit(0);
                }
                switch (result)
                {
                    case "1":
                        // JSON
                        viewModel.SetLogsType(1);
                        continueInput = false;
                        break;
                    case "2":
                        // XML
                        viewModel.SetLogsType(2);
                        continueInput = false;
                        break;
                    default:
                        Console.WriteLine(Properties.ts.InvalidInputLogsFormat);
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
            Console.WriteLine(Properties.ts.ChooseSaveSlots);
            string input = Console.ReadLine();
        if (input.Equals("q", StringComparison.CurrentCultureIgnoreCase))
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
                Console.WriteLine(string.Format(Properties.ts.ChooseSlotSourceFile, choice));
                string sourceDirectory = Console.ReadLine();
                Console.WriteLine(string.Format(Properties.ts.ChooseSlotDestination, choice));
                string destinationDirectory = Console.ReadLine();
                Console.WriteLine("Select the save type \n 1: Full \n 2: Differential");
                //string saveType = Console.ReadLine();
                int saveType = -1;
                switch (Console.ReadLine())
                {
                    case "1":
                        saveType = 1;
                        break;
                    case "2":
                        saveType = 2;
                        break;
                }

                string destinationFile = EditSave.Create(sourceDirectory, destinationDirectory, saveType);
                if(destinationFile == null ) 
                {
                    Console.WriteLine(Properties.ts.ErrorCreationFile);
                }
                else
                {
                    var saveData1 = new Data
                    {
                        Name = Path.GetFileName(sourceDirectory),
                        SourceFilePath = sourceDirectory,
                        TargetFilePath = destinationFile,
                        State = "ACTIVE",
                        TotalFilesToCopy = 3300,
                        TotalFilesSize = 1240312777,
                        NbFilesLeftToDo = 3274,
                        Progression = 0,
                        SaveType = saveType,
                    };
                    datas[choice - 1] = saveData1;
                    Data.Serialize(datas);
                    viewModel.Log.AddLog(choices);
                }
            }
       
        }

        /// <summary>
        /// If the user choose a non Empty save, save or delete it
        /// </summary>
        /// <param name="choices"></param>
    static void ProcessNonEmptySlots(List<int> choices)
    {
        Console.WriteLine(Properties.ts.OptionSave);
            Console.WriteLine(Properties.ts.OptionDelete);
            string result = Console.ReadLine();
        switch (result)
        {
            case "1":
                SaveSelectedSlots(choices);
                viewModel.Log.AddLog(choices);
                break;
            case "2":
                DeleteSelectedSlots(choices);
                break;
            default:
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
            if (datas[choice - 1] != null)
            {
                Console.WriteLine(string.Format(Properties.ts.SavingSlot, choice));
                Console.WriteLine(datas[choice - 1].SourceFilePath);
                Console.WriteLine(datas[choice - 1].TargetFilePath);
                EditSave.Update(datas[choice - 1].SourceFilePath, datas[choice - 1].TargetFilePath, datas[choice - 1].SaveType);

                Data.Serialize(datas);
            }
            else
                {
                    Console.WriteLine(string.Format(Properties.ts.ChooseSlotSourceFile, choice));
                }
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
        Console.WriteLine(Properties.ts.DisplayAvailableSaveSlots);
            for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i] == null)
            {
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
