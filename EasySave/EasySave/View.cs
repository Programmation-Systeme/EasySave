using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    internal class View
{
    private static int languages;
    private static ViewModel viewModel;
    private static Data[] datas;
    private static bool exitRequested = false;

    static void Main(string[] args)
    {
            viewModel = new ViewModel();
            datas = viewModel.Model.Datas;
            ChooseLanguages();
        while (!exitRequested)
        {
            ChooseSlot();
        }
    }

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
                    continueInput = false;
                    break;
                case "2":
                    languages = 1;
                    continueInput = false;
                    break;
                default:
                    Console.WriteLine("Invalid input!");
                    break;
            }
        }
    }

    static void ChooseSlot()
    {
        DisplayAvailableSlots();
        Console.WriteLine("Choose save slots (e.g., 1;2;3) or 'q' to Quit:");
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
            Console.WriteLine("Error: One or more selected slots are empty or non-empty.");
        }
    }

    static void ProcessEmptySlots(List<int> choices)
    {
        foreach (int choice in choices)
        {
            Console.WriteLine($"Choose current source file for Slot {choice}: ");
            string sourceFile = Console.ReadLine();
            Console.WriteLine($"Destination file path for Slot {choice}: ");
            string destinationFile = Console.ReadLine();
            datas[choice - 1] = new Data($"Item{choice}", sourceFile, destinationFile);
        }
    }

    static void ProcessNonEmptySlots(List<int> choices)
    {
        Console.WriteLine("1 : Save ");
        Console.WriteLine("2 : Delete ");
        string result = Console.ReadLine();
        switch (result)
        {
            case "1":
                SaveSelectedSlots(choices);
                break;
            case "2":
                DeleteSelectedSlots(choices);
                break;
            default:
                Console.WriteLine("Invalid input! Please enter '1' to Save or '2' to Delete.");
                break;
        }
    }

    static void SaveSelectedSlots(List<int> choices)
    {
        foreach (int choice in choices)
        {
            Console.WriteLine($"Saving Slot {choice}");
        }
    }

    static void DeleteSelectedSlots(List<int> choices)
    {
        foreach (int choice in choices)
        {
            Console.WriteLine($"Deleting Slot {choice}");
            datas[choice - 1] = null;
        }
    }

    static bool IsEmpty(int choice)
    {
        return datas[choice] == null;
    }

    static void DisplayAvailableSlots()
    {
        Console.WriteLine("Available save slots:");
        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i] == null)
            {
                Console.WriteLine($"{i + 1}: Empty");
            }
            else
            {
                Console.WriteLine($"{i + 1}: {datas[i].FileName}");
            }
        }
        }
    }
}
