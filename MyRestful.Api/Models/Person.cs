namespace MyRestful.Api.Models
{
    public class Person
    {
        public Person()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string AccountName { get; set; }
        public string Email { get; set; }
        public string BlogUrl { get; set; }
        public decimal Salary { get; set; }
    }
}