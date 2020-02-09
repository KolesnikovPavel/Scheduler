namespace Scheduler
{
    public class InspectionClass
    {
        public int Id;
        public int Time;
        public int ImportanceValue;
        public System.DateTime Date;
        public InspectionClass(int id, int time, int importanceValue, System.DateTime date)
        {
            Id = id;
            Time = time;
            ImportanceValue = importanceValue;
            Date = date;
        }
    }
}
