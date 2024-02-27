namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Person person = new Person();

            person.FirstName = null;
            person.MiddleName = null;
            person.LastName = "Boiko";
        }
    }
}
