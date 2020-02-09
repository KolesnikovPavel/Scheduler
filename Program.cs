using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler
{
    public class Program
    {
        public static int GetImportanceValue(DateTime date, DateTime startDate)
        {
            System.TimeSpan diff = startDate.Subtract(date);
            if (diff.ToString().Contains("-"))
                return 0;
            if (!diff.ToString().Contains("."))
                return 8;
            else
                return 1;
        }

        public static void ExcludeDublicateId (List<int> idCollection, List<InspectionClass> inspectionCollection)
        {
            foreach (var inspection in inspectionCollection)
                if (idCollection.Contains(inspection.Id))
                    inspection.Time = 10;
            idCollection.Clear();
        }

        public static void FillWithDatabaseValues(List<InspectionClass> inspectionCollection)
        {
            using (InspectionContext db = new InspectionContext())
            {
                var inspectionsDB = db.Inspections.ToList();
                foreach (var inspection in inspectionsDB)
                    inspectionCollection.Add(new InspectionClass(inspection.InspectionId, inspection.Time, 0, DateTime.Parse(inspection.Date)));
            }
        }

        public static void AssignInspector(List<int> idCollection, int i)
        {
            foreach (var id in idCollection)
            {
                using (InspectionContext db = new InspectionContext())
                {
                    var inspectionsDB = db.Inspections.Where(inspection => idCollection.Contains(inspection.InspectionId)).ToList();
                    var inspectorsDB = db.Inspectors.ToList();
                    foreach (var inspection in inspectionsDB)
                        inspection.ActingInspector = inspectorsDB[i].Name;
                    db.SaveChanges();
                }
            }
        }

        public static int GetNumberOfInspectors()
        {
            using (InspectionContext db = new InspectionContext())
            {
                var inspectorDB = db.Inspectors.Where(inspector => inspector.Status.ToLower() == "готов").ToList();
                return inspectorDB.Count();
            }
        }

        public static void Main()
        {
            var inspectionCollection = new List<InspectionClass>();
            var idCollection = new List<int>();

            DateTime startDate = new DateTime(2019, 10, 01);
            DateTime endDate = new DateTime(2019, 10, 31);
            TimeSpan diff = endDate.Subtract(startDate);
            int totalDays = Int32.Parse(diff.ToString().Remove(diff.ToString().IndexOf(".")));

            int inspectorTime = 8;
            FillWithDatabaseValues(inspectionCollection);
            for (int day = 0; day <= totalDays; day++)
            {
                Console.WriteLine("Date: " + startDate.AddDays(day));
                foreach (var inspection in inspectionCollection)
                    inspection.ImportanceValue = GetImportanceValue(inspection.Date, startDate.AddDays(day));
                for (int i = 0; i < GetNumberOfInspectors(); i++)
                {
                    if (idCollection.Count != 0)
                        ExcludeDublicateId(idCollection, inspectionCollection);
                    idCollection = KnapSackTask.ProcessRequest(inspectionCollection, inspectorTime);
                    AssignInspector(idCollection, i);
                }
            }
        }
    }
}