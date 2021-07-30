using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prova_week5
{
    public class Menu
    {
        public static void Start()
        {
            Console.WriteLine("===Benvenuto al Magazzino !===");

            int scelta = -1;
            bool uscita = false;
            while (!uscita)
            {
                Console.WriteLine("Inserire il codice corrispondente all'azione che si desidera compiere: ");
                Console.WriteLine("Premere 1 per aggiungere un nuovo prodotto,");
                Console.WriteLine("Premere 2 per visualizzare tutti i prodotti,");
                Console.WriteLine("Premere 3 per modificare un prodotto,");
                Console.WriteLine("Premere 4 per eliminare un prodotto,");
                Console.WriteLine("Premere 5 per visualizzare un report,");
                Console.WriteLine("Premere 0 per uscire");
                while (!int.TryParse(Console.ReadLine(), out scelta))
                {
                    Console.WriteLine("Codice inserito non corretto, riprova");
                }

                switch (scelta)
                {
                    case 1:
                        Prodotto newP = Manager.InsertProductProp(true);
                       bool isUnivoco= Manager.InsertData(newP.CodiceProdotto, newP.Categoria, newP.Descrizione, newP.PrezzoUnitario, newP.QuantitaDisponibile);
                        if (!isUnivoco)
                            Console.WriteLine("Il codice prodotto inserito corrisponde ad un prodotto già presente");
                        Console.WriteLine();
                        break;
                    case 2:
                        Manager.ReadData();
                        Console.WriteLine();
                        break;
                    case 3:
                        Manager.ReadData();
                        Console.WriteLine();
                        Console.WriteLine("ID del prodotto da modificare:");
                        string idUpdate = Manager.CheckString();
                        bool successoU = Manager.UpdateData(idUpdate);
                        if (successoU)
                            Console.WriteLine("Prodotto modificato con successo!");
                        else
                            Console.WriteLine("Nessun oggetto corrispondente al codice inserito");
                        Console.WriteLine();
                        break;
                    case 4:
                        Manager.ReadData();
                        Console.WriteLine();
                        Console.WriteLine("Inserire l'ID del prodotto da eliminare:");
                        string idDelete = Manager.CheckString();
                        bool successoD = Manager.DeleteData(idDelete);
                        if (successoD)
                            Console.WriteLine("Prodotto eliminato con successo!");
                        else
                            Console.WriteLine("Nessun oggetto corrispondente al codice inserito");
                        Console.WriteLine();
                        break;
                    case 5:
                        ScegliReport();
                        Console.WriteLine();
                        break;
                    case 0:
                        uscita = true;
                        break;
                    default:
                        Console.WriteLine("La scelta effettuata non corrisponde a nessuna azione!");
                        Console.WriteLine();
                        break;
                }

            }
            Console.WriteLine("Grazie per aver usato i nostri servizi! Arrivederci");
        }

        public static void ScegliReport()
        {
            bool uscita = false;
            while (!uscita)
            {
                int sceltaReport = -1;
                Console.WriteLine("Inserire il codice corrispondente al report da visualizzare: ");
                Console.WriteLine("Premere 1 per visualizzare l'elenco dei prodotti con disponibilità limitata,");
                Console.WriteLine("Premere 2 per filtrare i prodotti per categoria,");
                Console.WriteLine("Premere 0 per tornare al menù principale");
                while (!int.TryParse(Console.ReadLine(), out sceltaReport))
                {
                    Console.WriteLine("Codice inserito non corretto, riprova");
                }

                switch (sceltaReport)
                {
                    case 1:
                        Reports.Report1();
                        Console.WriteLine();
                        break;
                    case 2:
                        Console.WriteLine("Scegli la categoria del prodotto: ");
                        Console.WriteLine("[0] --> Alimentari,");
                        Console.WriteLine("[1] --> Cancelleria,");
                        Console.WriteLine("[2] --> Sanitari");
                        string categoria = Manager.ChoseCategory();
                        Reports.Report2(categoria);
                        Console.WriteLine();
                        break;
                    case 0:
                        uscita = true;
                        break;
                    default:
                        Console.WriteLine("La scelta effettuata non corrisponde a nessun report!");
                        Console.WriteLine();
                        break;
                }
            }
        }
    }
}
