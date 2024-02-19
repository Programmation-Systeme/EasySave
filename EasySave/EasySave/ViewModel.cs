using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace EasySave
{
    internal class ViewModel
    {
        private Model _model;

        private ILog _log;

        internal ILog Log { get => _log; set => _log = value; }

        public Data[] Datas { get => _model.Datas; set => _model.Datas = value; }

        /// <summary>
        /// Entry point of the log
        /// </summary>
        public ViewModel()
        {
            _model = new Model();
            _log = null;
        }

        public void AddDataOnSlot(string sourceDirectory, string destinationFile, int saveType, int slot)
        {
            _model.AddDataOnSlot(sourceDirectory, destinationFile, saveType, slot);
        }

        public void SerializeDatas()
        {
            try { _model.SerializeDatas(); } catch { throw; }
        }

        public void SetLogsType(int type)
        {
            // JSON
            if(type.Equals(1))
            {
                _log = new JsonLog(_model);
            }
            // XML
            else if(type.Equals(2))
            {
                _log = new XmlLog(_model);
            }
        }

        public List<int>? GetSlotsNumbersFromCMD(string arg)
        {
            arg = arg.Trim();
            List<int> SlotsNumbers = [];

            string[] sections = arg.Split(';');

            foreach (string section in sections)
            {
                if (section.Contains('-'))
                {

                    string[] rangeParts = section.Split('-');
                    if (rangeParts.Length != 2 || !IsValidNumber(rangeParts[0]) || !IsValidNumber(rangeParts[1]))
                        return null;

                    int start = int.Parse(rangeParts[0]);
                    int end = int.Parse(rangeParts[1]);

                    if (end >= start)
                    {
                        for (int i = start; i <= end; i++)
                        {
                            SlotsNumbers.Add(i);
                        }
                    }
                    else
                    {
                        return null; // Incorrect range (if end is lower than start)
                    }
                }
                else
                {
                    // Check if input is integer and between 1 and 5
                    if (!IsValidNumber(section))
                        return null;
                    SlotsNumbers.Add(int.Parse(section));
                }
            }
            return SlotsNumbers; // Success
        }

        /// <summary>
        /// Check if a string can be parsed in integer and is between 1 and 5
        /// </summary>
        /// <param name="text">The number in string format to check</param>
        /// <returns>True if valid, otherwise false</returns>
        public static bool IsValidNumber(string text)
        {
            return (int.TryParse(text, out int parsedInt) && parsedInt >= 1 && parsedInt <= 5);
        }
    }
}
