using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    struct Flight
    {
        public string Code;
        public string From;
        public string To;
        public double Price;
        public DateTime Time;
    }

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "Аеропорт – Система Бронювання";

        Console.Write("Оберіть регіон: ");
        string region = Console.ReadLine();

        Console.Write("Введіть місто вильоту: ");
        string fromCity = Console.ReadLine();

        List<Flight> flights = GenerateFlights(fromCity);

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nГОЛОВНЕ МЕНЮ");
            Console.ResetColor();

            Console.WriteLine("1. Переглянути рейси");
            Console.WriteLine("2. Пошук рейсу за напрямком");
            Console.WriteLine("3. Придбати квиток");
            Console.WriteLine("4. Вийти в головне меню");

            Console.Write("\nОберіть дію: ");
            string choice = Console.ReadLine();

            if (choice == "1") ShowAndBuyFlights(flights);
            else if (choice == "2") SearchAndBuy(flights);
            else if (choice == "3") BuyTicketByCode(flights);
            else if (choice == "4")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nВас повернуто в головне меню");
                Console.ResetColor();
            }
            else Console.WriteLine("Невірний вибір");
        }
    }

    static List<Flight> GenerateFlights(string fromCity)
    {
        List<Flight> flights = new List<Flight>();
        string[] destinations = { "Варшава", "Лондон", "Нью-Йорк", "Токіо", "Берлін", "Париж" };
        Random rnd = new Random();
        int code = 111;

        foreach (var dest in destinations)
        {
            for (int i = 0; i < 2; i++)
            {
                flights.Add(new Flight
                {
                    Code = code.ToString(),
                    From = fromCity,
                    To = dest,
                    Price = rnd.Next(100, 800),
                    Time = DateTime.Today.AddHours(rnd.Next(0, 24)).AddMinutes(rnd.Next(0, 60))
                });
                code++;
            }
        }
        return flights;
    }

    static void ShowAndBuyFlights(List<Flight> flights)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nСписок рейсів:");
        Console.ResetColor();

        foreach (var f in flights)
        {
            Console.WriteLine($"{f.Code}: {f.From} → {f.To}, час {f.Time:HH:mm}, ціна {f.Price}$");
        }

        Console.WriteLine("\nЩоб купити квиток, введіть код рейсу або Enter, щоб повернутися в меню.");
        string code = Console.ReadLine();
        if (!string.IsNullOrEmpty(code))
        {
            Flight selectedFlight = flights.Find(f => f.Code == code);
            if (selectedFlight.Code == null)
            {
                Console.WriteLine("Рейс не знайдено. Повернення в меню.");
                return;
            }
            BuyMultipleTickets(selectedFlight);
        }
    }

    static void SearchAndBuy(List<Flight> flights)
    {
        Console.Write("\nВведіть місто призначення: ");
        string direction = Console.ReadLine().ToLower();

        List<Flight> foundFlights = flights.FindAll(f => f.To.ToLower().Contains(direction));

        if (foundFlights.Count == 0)
        {
            Console.WriteLine("\nРейси не знайдено");
            return;
        }

        Console.WriteLine("\nЗнайдені рейси:");
        foreach (var f in foundFlights)
        {
            Console.WriteLine($"{f.Code}: {f.From} → {f.To}, час {f.Time:HH:mm}, ціна {f.Price}$");
        }

        Flight selectedFlight;
        while (true)
        {
            Console.Write("\nВведіть код рейсу для покупки: ");
            string code = Console.ReadLine();

            selectedFlight = foundFlights.Find(f => f.Code == code);

            if (selectedFlight.Code == null)
            {
                Console.WriteLine("Рейс не знайдено. Спробуйте ще раз.");
                continue;
            }
            break;
        }

        BuyMultipleTickets(selectedFlight);
    }

    static void BuyTicketByCode(List<Flight> flights)
    {
        ShowFlights(flights);

        Flight selectedFlight;
        while (true)
        {
            Console.Write("\nВведіть код рейсу: ");
            string code = Console.ReadLine();

            selectedFlight = flights.Find(f => f.Code == code);

            if (selectedFlight.Code == null)
            {
                Console.WriteLine("Рейс не знайдено. Спробуйте ще раз.");
                continue;
            }

            break;
        }

        BuyMultipleTickets(selectedFlight);
    }

    static void BuyMultipleTickets(Flight flight)
    {
        int ticketCount;
        while (true)
        {
            Console.Write("\nСкільки квитків хочете придбати (максимум 5): ");
            if (!int.TryParse(Console.ReadLine(), out ticketCount) || ticketCount < 1)
            {
                Console.WriteLine("Невірне значення, спробуйте ще раз.");
                continue;
            }
            if (ticketCount > 5)
            {
                Console.WriteLine("Ви не можете купити більше 5 квитків.");
                continue;
            }
            break;
        }

        for (int i = 0; i < ticketCount; i++)
        {
            Console.WriteLine($"\n=== Квиток {i + 1} ===");
            BuyTicket(flight);
        }
    }

    static void BuyTicket(Flight flight)
    {
        Console.Write("\nВаше ім'я: ");
        string name = Console.ReadLine();

        Console.Write("Вага багажу (кг): ");
        double weight;
        while (!double.TryParse(Console.ReadLine(), out weight) || weight < 0)
        {
            Console.WriteLine("Невірне значення. Введіть число для ваги багажу:");
        }

        Console.Write("Клас (1 — Економ, 2 — Бізнес): ");
        int cls;
        while (!int.TryParse(Console.ReadLine(), out cls) || (cls != 1 && cls != 2))
        {
            Console.WriteLine("Невірне значення. Введіть 1 або 2 для класу:");
        }

        double price = flight.Price + Math.Pow(weight, 1.1);
        if (cls == 2) price *= 1.5;
        price = Math.Round(price, 2);

        string ticketId = new Random().Next(100000, 999999).ToString();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nКВИТОК ОФОРМЛЕНО");
        Console.ResetColor();

        Console.WriteLine($"Пасажир: {name}");
        Console.WriteLine($"Рейс: {flight.Code} ({flight.From} → {flight.To})");
        Console.WriteLine($"Час: {flight.Time:dd.MM HH:mm}");
        Console.WriteLine($"Багаж: {weight} кг");
        Console.WriteLine($"Клас: {(cls == 1 ? "Економ" : "Бізнес")}");
        Console.WriteLine($"Номер квитка: {ticketId}");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nДо оплати: {price}$");
        Console.ResetColor();
    }

    static void ShowFlights(List<Flight> flights)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nСписок рейсів:");
        Console.ResetColor();

        foreach (var f in flights)
        {
            Console.WriteLine($"{f.Code}: {f.From} → {f.To}, час {f.Time:HH:mm}, ціна {f.Price}$");
        }
    }
}

