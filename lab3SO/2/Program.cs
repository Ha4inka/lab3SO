namespace _2;

class Program
{
    static void Main()
    {
        Once once = new Once();

        // Приклад виклику методу Exec лише один раз
        for (int i = 0; i < 10; i++)
        {
            // action буде викликано тільки один раз
            once.Exec(() => Console.WriteLine("Action виконано!"));
        }
    }

    public class Once
    {
        private int _executed;
        private readonly object _lockObject = new object();

        public void Exec(Action? action)
        {
            if (Interlocked.Exchange(ref _executed, 1) == 0)
            {
                lock (_lockObject)
                {
                    if (_executed == 1)
                    {
                        action?.Invoke();
                    }
                }
            }
        }
    }
}