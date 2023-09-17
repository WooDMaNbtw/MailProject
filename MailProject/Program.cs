using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml.Linq;

namespace MailProject
{
    public struct MailDB
    {
        public string address_getter { get; set; } // Адрес получателя
        public string surname_getter { get; set; } // Фамилия получателя 
        public string name_getter { get; set; } // Имя получателя
        public string address_setter { get; set; } // Адрес отправителя
        public string surname_setter { get; set; } // Фамилия отправителя
        public string name_setter { get; set; } // Имя отправителя
        public double cost { get; set; } // стоимость письма
    }


    public class Mail
    {
        private List<MailDB> mailbase = new List<MailDB>();

        public void AddMail(MailDB mailDB)
        {
            mailbase.Add(mailDB);
            SaveDB();
        }



        // Создадим функцию, которая возвращает данные о письме с указанными фамилией и именем отправителя.
        public List<MailDB> FindMailBySender(string name, string surname)
        {
            List<MailDB> result = new List<MailDB>();

            foreach (MailDB mail in mailbase)
            {
                if (mail.name_setter == name && mail.surname_setter == surname)
                {
                    result.Add(mail);
                }
            }
            Console.WriteLine($"Found {result.Count} mail(s) from {name} {surname}");

            PrintFoundedMails(result);

            return result;
        }

        // Функция, возвращающая данные о письме с указанной минимальной ценой
        public List<MailDB> FindMailByCost(double minCost)
        {
            List<MailDB> result = new List<MailDB>();

            foreach (MailDB mail in mailbase)
            {
                if (mail.cost > minCost)
                {
                    result.Add(mail);
                }
            }
            Console.WriteLine($"Found {result.Count} mail(s) where cost greater than {minCost}");

            PrintFoundedMails(result);

            return result;
        }

        // загружаем данные из txt
        public void LoadDB()
        {
            if (File.Exists("mailDB.txt"))
            {
                Console.Write("Loading database");
                for (int sec = 0; sec < 10; sec++)
                {
                    Thread.Sleep(200);
                    Console.Write(".");
                }

                var lines = File.ReadAllLines("mailDB.txt");

                foreach (var line in lines)
                {
                    // line имеет такую структуру: address_g | name_g | surname_g | address_s | name_s | surname_s | cost  
                    var data = line.Split('|');
                    // разделяем на отдельные поля

                    // читаем из файла и записываем в бд
                    AddMail(new MailDB
                    {
                        address_getter = data[0].Trim(),
                        name_getter = data[1].Trim(),
                        surname_getter = data[2].Trim(),
                        address_setter = data[3].Trim(),
                        name_setter = data[4].Trim(),
                        surname_setter = data[5].Trim(),
                        cost = double.Parse(data[6].Trim())
                    });
                }
                Console.WriteLine("\nAll data loaded successfully!");
            }
            else
            {
                SaveDB();
            }
        }
        // загружаем данные в txt
        public void SaveDB()
        {

            using (StreamWriter writer = new StreamWriter("mailDB.txt")) // ..\LAB_1\LAB_1\bin\Debug\mailDB.txt
            {
                foreach (var mail in mailbase)
                {
                    // записываем данные 
                    string line = $"{mail.address_getter} | {mail.name_getter} | {mail.surname_getter} | {mail.address_setter} | {mail.name_setter} | {mail.surname_setter} | {mail.cost}";
                    writer.WriteLine(line);
                }
                writer.Close();
            }
            
        }

        // функция, которая выводит список с 'удовлетворенными условиями в FindMailBySender, FindMailByCost'
        private void PrintFoundedMails(List<MailDB> mailDB)
        {
            Console.WriteLine("|----------------------FROM-----------------------|------------------------TO-----------------------|----COST----|");
            foreach (var mail in mailDB)
            {
                Console.WriteLine($"| {mail.surname_setter,-10} {mail.name_setter,-10} {mail.address_setter,25} | {mail.surname_getter,-10} {mail.name_getter,-10} {mail.address_getter,25} | {mail.cost,10} |");
            }
            Console.WriteLine("|-------------------------------------------------|-------------------------------------------------|------------|");
        }

        // Выводит весь список почты
        public void PrintAllMails()
        {
            Console.WriteLine("|-------------------------------------------------|");
            Console.WriteLine($"| Found {mailbase.Count} mails" + $"{"|",35}");
            Console.WriteLine("|----------------------FROM-----------------------|------------------------TO-----------------------|----COST----|");
            foreach (var mail in mailbase)
            {
                Console.WriteLine($"| {mail.surname_setter,-10} {mail.name_setter,-10} {mail.address_setter,25} | {mail.surname_getter,-10} {mail.name_getter,-10} {mail.address_getter,25} | {mail.cost,10} |");
            }
            Console.WriteLine("|-------------------------------------------------|-------------------------------------------------|------------|");


        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Mail mail = new Mail(); // класс с функциями
            MailDB mailDB = new MailDB(); // сама база данных


            mail.LoadDB(); // загрузка данных из txt

            bool proccess = true;
            while (proccess)
            {

                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("---|----------------------------------|---");
                Console.WriteLine($"---|  {" 1 --> Add a new mail",-30}  {"|---",4}");
                Console.WriteLine($"---|  {" 2 --> Search mails by sender",-30}  {"|---",4}");
                Console.WriteLine($"---|  {" 3 --> Search mails by cost",-30}  {"|---",4}");
                Console.WriteLine($"---|  {" 4 --> Print the database",-30}  {"|---",4}");
                Console.WriteLine($"---|  {" else --> EXIT",-30}  {"|---",4}");
                Console.WriteLine("---|----------------------------------|---");


                Console.WriteLine("ENTER NUMBER: ");
                string option = Console.ReadLine();


                switch (option)
                {
                    case "1":
                        Console.Write("Enter sender's address: ");
                        string address_sender = Console.ReadLine();
                        Console.Write("Enter sender's name: ");
                        string name_sender = Console.ReadLine();
                        Console.Write("Enter sender's surname: ");
                        string surname_sender = Console.ReadLine();

                        Console.Write("Enter getter's address: ");
                        string address_getter = Console.ReadLine();
                        Console.Write("Enter getter's name: ");
                        string name_getter = Console.ReadLine();
                        Console.Write("Enter getter's surname: ");
                        string surname_getter = Console.ReadLine();

                        Random random = new Random();
                        double cost = Math.Round(random.NextDouble() * 100.0, 1);

                        mail.AddMail(new MailDB
                        {
                            address_setter = address_sender,
                            name_setter = name_sender,
                            surname_setter = surname_sender,
                            address_getter = address_getter,
                            name_getter = name_getter,
                            surname_getter = surname_getter,
                            cost = cost
                        });
                        break;

                    case "2":
                        Console.Write("Enter sender's name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter sender's surname: ");
                        string surname = Console.ReadLine();
                        mail.FindMailBySender(name, surname); // вызывается функция и сразу вывод на экран
                        break;

                    case "3":
                        Console.Write("Enter minimal cost (use comma): ");
                        double min_cost = double.Parse(Console.ReadLine());
                        mail.FindMailByCost(min_cost); // вызывается функция и сразу вывод на экран  
                        break;

                    case "4":
                        mail.PrintAllMails();
                        break;

                    default:
                        proccess = false;
                        break;
                }
            }

        }

    }
}

