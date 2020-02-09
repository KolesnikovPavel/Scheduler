using System;
using System.Collections.Generic;

namespace Scheduler
{
    public class KnapSackTask
    {
        public static List<int> idCollection = new List<int>();

        public static List<int> ProcessRequest(List<InspectionClass> inspectionCollection, int inspectorTime)
        {
            ItemCollection[] ic = new ItemCollection[inspectorTime + 1];
            for (int i = 0; i <= inspectorTime; i++)
                ic[i] = new ItemCollection();
            for (int i = 0; i < inspectionCollection.Count; i++)
                for (int j = inspectorTime; j >= 0; j--)
                    if (j >= inspectionCollection[i].Time)
                    {
                        int quantity = Math.Min(1, j / inspectionCollection[i].Time);
                        for (int k = 1; k <= quantity; k++)
                        {
                            ItemCollection lighterCollection = ic[j - k * inspectionCollection[i].Time];
                            int testValue = lighterCollection.TotalValue + k * inspectionCollection[i].ImportanceValue;
                            if (testValue > ic[j].TotalValue)
                                (ic[j] = lighterCollection.Copy()).AddItem(inspectionCollection[i], k);
                        }
                    }
            Console.WriteLine("Total working hours: " + inspectorTime + "\nUsed working hours: " + ic[inspectorTime].TotalTime + "\nObject ids and their importance:");
            foreach (KeyValuePair<int, int> kvp in ic[inspectorTime].Contents)
            {
                Console.Write(kvp.Key + "\t");
                foreach (var e in inspectionCollection)
                    if (kvp.Key == e.Id)
                        Console.Write(e.ImportanceValue + "\n");
                idCollection.Add(kvp.Key);
            }
            Console.WriteLine("");
            return idCollection;
        }

        public class ItemCollection
        {
            public Dictionary<int, int> Contents = new Dictionary<int, int>();
            public int TotalTime;
            public int TotalValue;
            public void AddItem(InspectionClass inspection, int quantity)
            {
                if (Contents.ContainsKey(inspection.Id))
                    Contents[inspection.Id] += quantity;
                else
                    Contents[inspection.Id] = quantity;
                TotalTime += quantity * inspection.Time;
                TotalValue += quantity * inspection.ImportanceValue;
            }
            public ItemCollection Copy()
            {
                var ic = new ItemCollection();
                ic.Contents = new Dictionary<int, int>(this.Contents);
                ic.TotalTime = this.TotalTime;
                ic.TotalValue = this.TotalValue;
                return ic;
            }
        }
    }
}