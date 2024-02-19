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
        private ViewModel viewModel;
        public ViewModel ViewModel { get { return viewModel; } set { viewModel = value; } }

        /// <summary>
        /// Entry point of the app
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            View view = new View();
            view.viewModel = new ViewModel();

            //Launch by cmd line
            if (args.Length > 0)
            {
                string argument = args[0];

                List<int>? list = view.viewModel.GetSlotsNumbersFromCMD(argument);
                if (list != null)
                {
                    view.SaveSelectedSlots(list);
                }
            }
            //Normal launch (execution)
            else
            {
                //By default, the selected language is english
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                view.ChooseLanguages();
                view.ChooseLogType();
                while (true)
                {
                    view.ChooseSlot();
                }
            }
        }

        /// <summary>
        /// Display and set up the languages
        /// </summary>
        private void ChooseLanguages()
        {
            bool continueInput = true;
            while (continueInput)
            {
                Console.WriteLine("Choose language: \n  1 : Français \n  2 : English \n  q : Quit");
                string? result = Console.ReadLine();
                if (result == null || result.Equals("q", StringComparison.CurrentCultureIgnoreCase))
                {
                    Environment.Exit(0);
                }
                switch (result)
                {
                    case "1":
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr-FR");
                        continueInput = false;
                        break;
                    case "2":
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
        private void ChooseLogType()
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
        private void ChooseSlot()
        {
            DisplayAvailableSlots();
            Console.WriteLine(Properties.ts.ChooseSaveSlots);
            string? input = Console.ReadLine();
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
        private void ProcessEmptySlots(List<int> choices)
        {
            foreach (int choice in choices)
            {
                Console.WriteLine(string.Format(Properties.ts.ChooseSlotSourceFile, choice));
                string sourceDirectory = Console.ReadLine();
                Console.WriteLine(string.Format(Properties.ts.ChooseSlotDestination, choice));
                string destinationDirectory = Console.ReadLine();
                Console.WriteLine("Select the save type \n 1: Full \n 2: Differential");
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
                if (destinationFile == null)
                {
                    Console.WriteLine(Properties.ts.ErrorCreationFile);
                }
                else
                {
                    viewModel.AddDataOnSlot(sourceDirectory, destinationFile, saveType, choice);
                    viewModel.Log.AddLog(choices);
                }
            }

        }

        /// <summary>
        /// If the user choose a non Empty save, save or delete it
        /// </summary>
        /// <param name="choices"></param>
        private void ProcessNonEmptySlots(List<int> choices)
        {
            Console.WriteLine(ts.OptionSave);
            Console.WriteLine(ts.OptionDelete);
            string? result = Console.ReadLine();
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
                    Console.WriteLine(ts.InvalidInputSaveDelete);
                    break;
            }
        }
        /// <summary>
        /// Save the slot
        /// </summary>
        /// <param name="choices"></param>
        private void SaveSelectedSlots(List<int> choices)
        {
            foreach (int choice in choices)
            {
                if (viewModel.Datas[choice - 1] != null)
                {
                    Data actualData = viewModel.Datas[choice - 1];
                    Console.WriteLine(string.Format(Properties.ts.SavingSlot, choice));
                    Console.WriteLine(actualData.SourceFilePath);
                    Console.WriteLine(actualData.TargetFilePath);
                    EditSave.Update(actualData.SourceFilePath, actualData.TargetFilePath, actualData.SaveType);
                    try
                    {
                        viewModel.SerializeDatas();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(string.Format(ts.ErrorDuringSerialization,ex.Message));
                    }
                }
                else
                {
                    Console.WriteLine(string.Format(ts.ChooseSlotSourceFile, choice));
                }
            }
        }
        /// <summary>
        /// Delete the slot 
        /// </summary>
        /// <param name="choices"></param>
        private void DeleteSelectedSlots(List<int> choices)
        {
            foreach (int choice in choices)
            {
                Console.WriteLine(string.Format(Properties.ts.DeletingSlot, choice));

                EditSave.Delete(viewModel.Datas[choice - 1].TargetFilePath);
                viewModel.Datas[choice - 1] = null;
                try
                {
                    viewModel.SerializeDatas();
                }
                catch (Exception ex)
                {

                }
            }
        }
        /// <summary>
        /// Return True if the slot is empty
        /// </summary>
        /// <param name="choice"> The chosen slot </param>
        /// <returns></returns>
        private bool IsEmpty(int choice)
        {
            return viewModel.Datas[choice] == null;
        }
        /// <summary>
        /// Display the avaiable slots
        /// </summary>
        private void DisplayAvailableSlots()
        {
            Console.WriteLine(ts.DisplayAvailableSaveSlots);
            for (int i = 0; i < viewModel.Datas.Length; i++)
            {
                if (viewModel.Datas[i] == null)
                {
                    Console.WriteLine($"{i + 1}: {ts.EmptySlot}");
                }
                else
                {
                    Console.WriteLine($"{i + 1}: {viewModel.Datas[i].Name}");
                }
            }
        }

        private void SerializeDatas()
        {
            try
            {
                viewModel.SerializeDatas();
            }
            catch(Exception ex)
            {
                Console.WriteLine(string.Format(ts.ErrorDuringSerialization, ex.Message));
            }
        }
    }
}
