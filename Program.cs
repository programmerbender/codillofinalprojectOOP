using System;
using System.Collections.Generic;

namespace CODILLO_MECHSHOPLOGBOOK  //for avoiding naming conflicts for a larger project file
{
    class Program //main
    {
        const double VAT_RATE = 0.10; // 10% VAT set
        static Inventory inventory = new Inventory();

        static void Main(string[] args)
        {
            Console.WriteLine("PORMAN KALVIN CARSHOP SALES STOCKS LOGBOOK");

            while (true)
            {
                ManageInventory();
                PerformServices();

                Console.Write("GUSTO MO BA GAMITIN MOKO ULIT? (oo/hindi): ");
                string response = Console.ReadLine().Trim().ToLower();       //trimming for removing space //tolower - lowercasing
                if (response != "oo")
                {
                    break;
                }
            }

            Console.WriteLine("\nMARAMING SALAMAT SA PAGGAMIT HEHE");
        }

        static void ManageInventory()
        {
            while (true)
            {
                Console.WriteLine("\nSALES INVENTORY LOGBOOK:");      //methods choice
                Console.WriteLine("1. DAGDAGAN ANG ISTAKS");
                Console.WriteLine("2. BAWASAN ANG ISTAKS");       
                Console.WriteLine("3. IPAKITA ANG ISTAKS");
                Console.WriteLine("4. RESIBO PARA SA CUSTOMER");
                Console.Write("PUMILI NG GUSTO GAWEN: ");

                string choice = Console.ReadLine();
                switch (choice)                            //switch for executing
                {
                    case "1":
                        AddPartToInventory();
                        break;
                    case "2":
                        RemovePartFromInventory();
                        break;
                    case "3":
                        inventory.DisplayInventory();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("PARANG MAY MALI, SUBUKAN MO ULIT");  
                        break;
                }
            }
        }

        static void AddPartToInventory()    //static - belongs to the class void- function will not return any value
        {
            Console.Write("ILAGAY ANG PANGALAN NG STOCK: ");
            string partName = Console.ReadLine();

            Console.Write("ILAGAY MO YUNG BILANG NG NAKASTOCK: "); 
            int quantity;
            while (!int.TryParse(Console.ReadLine(), out quantity) || quantity < 0) //parsing the input of th users if valid integer
            {
                Console.WriteLine("ANG PWEDE MO LANG ILAGAY AY VALID INTEGER NUMBER LANG");
                Console.Write("ILAGAY ANG BILANG NG NAKA-STOCK: ");
            }

            inventory.AddPart(partName, quantity);
            Console.WriteLine("SUCCESSFULLY ADDED YUNG STOCK "); //will show if the input if ever na true
        }

        static void RemovePartFromInventory()  //almost the same as addpartfrominventory
        {
            Console.Write("ILAGAY ANG PANGALAN NG STOCK: ");
            string partName = Console.ReadLine();

            Console.Write("ILAN ANG GUSTO MONG IBAWAS: ");
            int quantity;
            while (!int.TryParse(Console.ReadLine(), out quantity) || quantity < 0)
            {
                Console.WriteLine("PAKIUSAP  LANG BAWAL NEGATIVE");
                Console.Write("PAKILAGAY KUNG ILAN ANG GUSTO MONG IBAWAS: ");
            }

            if (inventory.RemovePart(partName, quantity))  //loop continues until the users input is true
            {
                Console.WriteLine("SUCCESS ANG PAG-BAWAS");
            }
            else
            {
                Console.WriteLine("PARANG MAY MALI, PAKICHECK NAMAN KUNG MERON TALAGA O TAMA YUNG BILANG");
            }
        }

        static void PerformServices()
        {
            List<Service> services = new List<Service>();
            bool addMoreServices = true;

            while (addMoreServices)
            {
                Service service = GetServiceDetails();
                services.Add(service);

                Console.Write("GUSTO MO PA BA DAGDAGAN? (oo/hindi): ");
                string response = Console.ReadLine().Trim().ToLower();
                if (response != "oo")
                {
                    addMoreServices = false;
                }
            }

            GenerateReceipt(services);      
        }

        static Service GetServiceDetails()
        {
            Console.Write("\nPAKILAGAY ANG DESKRIPSYON: ");
            string description = Console.ReadLine();

            Console.Write("ILAGAY ANG LABOR (bago ang VAT): ");
            double cost;
            while (!double.TryParse(Console.ReadLine(), out cost) || cost < 0)
            {
                Console.WriteLine("PAKIUSAP MAGLAGAY NG VALID INTEGER LANG");
                Console.Write("ILAGAY KUNG MAGKANO ANG SERBISYO (bago VAT): ");
            }

            Service service = new Service(description, cost);

            bool addMoreParts = true;
            while (addMoreParts)
            {
                Console.Write("ILAGAY UNG PART NA GINAMIT (or 'tapos' to finish): ");
                string partName = Console.ReadLine().Trim();
                if (partName.ToLower() == "tapos")
                {
                    addMoreParts = false;
                    continue;
                }

                Console.Write("ILAGAY KUNG ILAN: ");
                int quantity;
                while (!int.TryParse(Console.ReadLine(), out quantity) || quantity < 0)
                {
                    Console.WriteLine("PAKIUSAP MAGLAGAY NG WHOLE NUMBER LANG");
                    Console.Write("ILAGAY KUNG ILAN ANG GINAMIT: ");
                }

                if (inventory.RemovePart(partName, quantity))
                {
                    service.AddPart(partName, quantity);
                    Console.WriteLine("MATAGUMPAY NA NAGAWA ANG KAILANGAN");
                }
                else
                {
                    Console.WriteLine("PARANG MAY MALI, PAKICHECK ULIT KAPAG TAMA ANG IYONG NAILAGAY");
                }
            }

            return service;
        }

        static void GenerateReceipt(List<Service> services)
        {
            double totalCostBeforeVAT = 0;
            double totalVATAmount = 0;
            double totalCostAfterVAT = 0;

            Console.WriteLine("\n--- Sales Receipt ---"); //shows the header of the receipt

            foreach (Service service in services)
            {
                double vatAmount = service.Cost * VAT_RATE;
                double totalCost = service.Cost + vatAmount;

                Console.WriteLine($"\nService Description: {service.Description}");
                Console.WriteLine($"Cost (before VAT): Php {service.Cost:F2}");
                Console.WriteLine($"VAT Amount (10%): Php {vatAmount:F2}");
                Console.WriteLine($"Total Cost (including VAT): Php {totalCost:F2}");

                foreach (var part in service.PartsUsed)
                {
                    Console.WriteLine($"Part Used: {part.Key}, BILANG: {part.Value}");
                }

                totalCostBeforeVAT += service.Cost;
                totalVATAmount += vatAmount;
                totalCostAfterVAT += totalCost;
            }

            Console.WriteLine("\n---------------------");
            Console.WriteLine($"Total Cost (before VAT): Php {totalCostBeforeVAT:F2}");
            Console.WriteLine($"Total VAT Amount (10%): Php {totalVATAmount:F2}");
            Console.WriteLine($"Total Cost (including VAT): Php {totalCostAfterVAT:F2}");
            Console.WriteLine("---------------------");   //shows the end of the receipt or border
        }
    }

    class Inventory    //inventory class
    {
        private Dictionary<string, int> parts = new Dictionary<string, int>();

        public void AddPart(string partName, int quantity)
        {
            if (parts.ContainsKey(partName))
            {
                parts[partName] += quantity;
            }
            else
            {
                parts[partName] = quantity;
            }
        }

        public bool RemovePart(string partName, int quantity)
        {
            if (parts.ContainsKey(partName) && parts[partName] >= quantity)
            {
                parts[partName] -= quantity;
                if (parts[partName] == 0)
                {
                    parts.Remove(partName);
                }
                return true;
            }
            return false;
        }

        public void DisplayInventory()
        {
            Console.WriteLine("\n--- Inventory ---");
            foreach (var part in parts)
            {
                Console.WriteLine($"Part: {part.Key}, BILANG: {part.Value}");
            }
            Console.WriteLine("----------------");
        }
    }

    class Service  //service class
    {
        public string Description { get; set; }      //get; - for read only & set; modifying the valeu
        public double Cost { get; set; }
        public Dictionary<string, int> PartsUsed { get; set; }

        public Service(string description, double cost)
        {
            Description = description;
            Cost = cost;
            PartsUsed = new Dictionary<string, int>();
        }

        public void AddPart(string partName, int quantity)
        {
            if (PartsUsed.ContainsKey(partName))          //containskey will find if the dictionary is empty.
            {
                PartsUsed[partName] += quantity;
            }
            else
            {
                PartsUsed[partName] = quantity;
            }
        }
    }
}
