namespace DAL.Models
{
    public class Partners : BaseModel<int>
    {
        public string Name { get; set; }
        public int Total { get; set; }

        public List<TestModel> Tournaments { get; set; }
    }
}
