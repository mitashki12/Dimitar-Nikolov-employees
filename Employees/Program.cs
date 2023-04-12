using System.Collections.Generic;

namespace Employees
{
    internal class Program
    {
        static private StreamReader? reader = null;
        static private Dictionary<int,List<EntryRecord>>? employeesByProject = null;
        static void Main(string[] args)
        {
            string? fileName = "";
            bool isError = false;
            do
            {
                Console.WriteLine("Please input file name!");
                fileName = Console.ReadLine();
                
                try
                {
                    reader = new StreamReader(fileName);
                    isError = false;
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    isError = true;
                    reader?.Close();
                }
            } while (isError == true);

            string? line = reader.ReadLine();
            DateTime currentDate=DateTime.Now;
            employeesByProject = new Dictionary<int, List<EntryRecord>>();
            List<EntryRecord> currentList;
            EntryRecord entry;
            while (line != null)
            {
                entry = parseLines(line, currentDate);
                if (employeesByProject.ContainsKey(entry.ProjectId))
                {
                    currentList = employeesByProject.GetValueOrDefault<int, List<EntryRecord>>(entry.ProjectId);
                    currentList.Add(entry);
                }
                else {
                    employeesByProject.Add(entry.ProjectId, new List<EntryRecord>());
                    currentList = employeesByProject.GetValueOrDefault<int, List<EntryRecord>>(entry.ProjectId);
                    currentList.Add(entry);
                }
                
                line = reader.ReadLine();
            }

           PairOfEmployees maximalPair = findMaximalPair(employeesByProject);
           Console.WriteLine("{0}, {1}, {2}", maximalPair.FirstEmployeeId, maximalPair.SecondEmployeeId, maximalPair.DurationInDays);
        }
        private static DateTime parseDate(string date, DateTime now) {
            string[] dateFragments = date.Split('-','/');
            if (dateFragments[0] == "NULL")
            {
                return new DateTime(now.Year, now.Month, now.Day);
            }
            else {
                return new DateTime(Convert.ToInt32(dateFragments[0]), Convert.ToInt32(dateFragments[1]), Convert.ToInt32(dateFragments[2]));
            }
        }
        private static EntryRecord parseLines(string line, DateTime currentDate) {
            EntryRecord entry = new EntryRecord();
            List<string> lineContent = new List<string>();
            lineContent.AddRange(line.Split(','));
            entry.EmployeeId = Convert.ToInt32(lineContent[0].Trim());
            entry.ProjectId = Convert.ToInt32(lineContent[1].Trim());
            entry.From = parseDate(lineContent[2].Trim(), currentDate);
            entry.To = parseDate(lineContent[3].Trim(), currentDate);
            return entry;
        }
        private static PairOfEmployees findMaximalPair(Dictionary<int, List<EntryRecord>> employeesByProject) {
            PairOfEmployees maximalPair = new PairOfEmployees();
            maximalPair.FirstEmployeeId = 0;
            maximalPair.SecondEmployeeId = 0;
            maximalPair.ProjectId = 0;
            maximalPair.DurationInDays = 0;
            foreach (int key in employeesByProject.Keys) {
                //Console.WriteLine("Current key is {0}", key);
                List<EntryRecord> currentList = employeesByProject.GetValueOrDefault<int, List<EntryRecord>>(key);
                //Console.WriteLine("Current list Count is {0}",currentList.Count);
                for (int i=0;i<currentList.Count-1;i++ ) {
                    for (int j = i + 1; j < currentList.Count; j++) {
                        double duration = calculateDuration(currentList[i], currentList[j]);
                        //Console.WriteLine("Current duration = {0}", duration);
                        if ( i!=j && maximalPair.DurationInDays < duration) {
                            maximalPair.FirstEmployeeId = currentList[i].EmployeeId;
                            maximalPair.SecondEmployeeId = currentList[j].EmployeeId;
                            maximalPair.ProjectId = key;
                            maximalPair.DurationInDays = duration;
                        }
                    }
                }
            }
            return maximalPair;
        }
        private static double calculateDuration(EntryRecord record1, EntryRecord record2) {
            double duration = 0;
            DateTime begin, end;
            if (record1.From > record2.To || record1.To < record2.From)
            {
                return 0;
            }
            else {
                if (record1.From >= record2.From)
                {
                    begin = record1.From;
                }
                else {
                    begin = record2.From;
                }
                if (record1.To <= record2.To)
                {
                    end = record1.To;
                }
                else {
                    end = record2.To;
                }
                duration = end.Subtract(begin).TotalDays;
            }
            return duration;
        }
    }
}