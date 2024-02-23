namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            User user  = new User();    

            user.Age = 19;

            Func<User, bool> func = user => user.Age > 18;

            bool result = func(user);

            Console.WriteLine(result);
        }

        //public TimeSpan GetTimeSpan()
        //{
        //    TimeSpan timeSpan = new TimeSpan();
        //}
    }
}
