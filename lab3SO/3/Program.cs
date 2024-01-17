
namespace _3;

class Program
{
    static void Main()
    {
        // Створюємо чергу з максимальним розміром 10
        MessageQueue<string> queue = new MessageQueue<string>(10);

        // Використовуємо ThreadPool для додавання елементів до черги
        for (int i = 0; i < 15; i++)
        {
            string message = $"Message {i}";
            ThreadPool.QueueUserWorkItem(_ =>
            {
                queue.Add(message);
                Console.WriteLine($"Added: {message}");
            });
        }

        // Вилучаємо елементи з черги
        for (int i = 0; i < 15; i++)
        {
            string message = queue.Poll();
            Console.WriteLine($"Polled: {message}");
        }
    }
  
    public class MessageQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly int _maxSize;
        private readonly object _lockObject = new object();
        private readonly SemaphoreSlim _enqueueSemaphore;
        private readonly SemaphoreSlim _dequeueSemaphore;

        public MessageQueue(int maxSize)
        {
            this._maxSize = maxSize;
            _enqueueSemaphore = new SemaphoreSlim(maxSize, maxSize);
            _dequeueSemaphore = new SemaphoreSlim(0, maxSize);
        }

        public void Add(T message)
        {
            bool added = false;

            while (!added)
            {
                if (_enqueueSemaphore.Wait(0))
                {
                    lock (_lockObject)
                    {
                        if (_queue.Count < _maxSize)
                        {
                            _queue.Enqueue(message);
                            _dequeueSemaphore.Release();
                            added = true;
                        }
                    }
                }
                else
                {
                    _dequeueSemaphore.Wait();
                }
            }
        }

        public T Poll()
        {
            _dequeueSemaphore.Wait();

            lock (_lockObject)
            {
                T message = _queue.Dequeue();
                _enqueueSemaphore.Release();
                return message;
            }
        }
    }

}