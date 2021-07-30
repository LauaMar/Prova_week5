using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prova_week5
{
    public class Manager
    {
        public const string connectionString = @"Server=(localdb)\mssqllocaldb;Database=Magazzino;Trusted_Connection=True;";

        //static DataSet dsMagazzino = new DataSet();

         public static void ReadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Can't establish connection!");

                DataSet dsMagazzino = new DataSet();

                SqlDataAdapter prodottiAdapter = new();

                SqlCommand selectProdotti = new SqlCommand("SELECT * FROM Prodotti", conn);

                prodottiAdapter.SelectCommand = selectProdotti;

                prodottiAdapter.Fill(dsMagazzino, "Prodotti"); 

                conn.Close(); 

                Console.WriteLine("=====Prodotti disponibili=====");

                Console.WriteLine("{0,-10}{1,-20}{2,-30}{3,-50}{4,-30}{5,-30}", "ID", "Codice Prodotto", "Categoria", "Descrizione", "Prezzo", "Quantità");
                Console.WriteLine(new String('_', 150));
                Console.WriteLine();

                foreach (DataRow row in dsMagazzino.Tables["Prodotti"].Rows)
                {
                    Console.WriteLine(
                       "{0,-10}{1,-20}{2,-30}{3,-50}{4,-30}{5,-30}",
                       row["ID"],
                       row["CodiceProdotto"],
                       row["Categoria"],
                       row["Descrizione"],
                       row["PrezzoUnitario"],
                       row["QuantitaDisponibile"]
                       );

                }

            }

        }

        public static bool InsertData(string codiceProdotto, string categoria, string descrizione, decimal prezzoUnitario, int quantita)
        {
            bool isUnivoco = true;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Can't establish connection!");

                DataSet dsMagazzino = new DataSet();

                SqlDataAdapter prodottiAdapter = new();

                SqlCommand selectProdotti = new SqlCommand("SELECT * FROM Prodotti", conn);

                prodottiAdapter.SelectCommand = selectProdotti; 

                prodottiAdapter.InsertCommand = ProdottiInsertCommand(conn);

                prodottiAdapter.Fill(dsMagazzino, "Prodotti"); 

                conn.Close();


                foreach (DataRow row in dsMagazzino.Tables["Prodotti"].Rows)
                {
                    if ((string)row["CodiceProdotto"] == codiceProdotto)
                        isUnivoco = false;

                }
                if (isUnivoco)
                {
                    DataRow newRow = dsMagazzino.Tables["Prodotti"].NewRow();

                    newRow["CodiceProdotto"] = codiceProdotto;
                    newRow["Categoria"] = categoria;
                    newRow["Descrizione"] = descrizione;
                    newRow["PrezzoUnitario"] = prezzoUnitario;
                    newRow["QuantitaDisponibile"] = quantita;

                    dsMagazzino.Tables["Prodotti"].Rows.Add(newRow);

                    prodottiAdapter.Update(dsMagazzino, "Prodotti");
                }
            }
            return isUnivoco;
        }

        public static bool UpdateData(string id)
        {
            bool successo = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();
                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Can't establish connection!");

                DataSet dsMagazzino = new DataSet();

                SqlDataAdapter prodottiAdapter = new();

                SqlCommand selectProdotti = new SqlCommand("SELECT * FROM Prodotti", conn);

                prodottiAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;


                prodottiAdapter.SelectCommand = selectProdotti;

                prodottiAdapter.UpdateCommand = ProdottiUpdateCommand(conn);

                prodottiAdapter.Fill(dsMagazzino, "Prodotti");

                conn.Close(); 


                DataRow rowToChange = dsMagazzino.Tables["Prodotti"].Rows.Find(id);

                if (rowToChange != null)
                {
                    Prodotto modP= Manager.InsertProductProp(false);
                
                    rowToChange["PrezzoUnitario"] =modP.PrezzoUnitario;
                    rowToChange["QuantitaDisponibile"] = modP.QuantitaDisponibile;

                    Console.WriteLine($"ROW STATE: {rowToChange.RowState.ToString()}");

                    prodottiAdapter.Update(dsMagazzino, "Prodotti");
                    successo = true;
                }
               
            }
            return successo;
        }

        public static bool DeleteData(string id)
        {
            bool successo = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (conn.State != ConnectionState.Open)
                    Console.WriteLine("Can't establish connection!");

                DataSet dsMagazzino = new DataSet();

                SqlDataAdapter prodottiAdapter = new();

                SqlCommand selectProdotti = new SqlCommand("SELECT * FROM Prodotti", conn);

                prodottiAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                prodottiAdapter.SelectCommand = selectProdotti; 

                prodottiAdapter.DeleteCommand = ProdottiDeleteCommand(conn);

                prodottiAdapter.Fill(dsMagazzino, "Prodotti"); 

                conn.Close(); 

                DataRow rowToChange = dsMagazzino.Tables["Prodotti"].Rows.Find(id);

                if (rowToChange != null)
                {
                    rowToChange.Delete();

                    prodottiAdapter.Update(dsMagazzino, "Prodotti");
                    successo = true;
                }
            }
            return successo;
        }



        private static SqlCommand ProdottiInsertCommand(SqlConnection conn)
        {
            SqlCommand insertCommand = new SqlCommand();
            insertCommand.Connection = conn;

            insertCommand.CommandText = "INSERT INTO Prodotti " +
                      "VALUES(@codiceProdotto,@categoria,@descrizione,@prezzoUnitario, @quantita)";


            insertCommand.CommandType = System.Data.CommandType.Text;

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@codiceProdotto",
                    SqlDbType.NVarChar,
                    10, 
                    "CodiceProdotto"
                    )
                );

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@categoria",
                    SqlDbType.NVarChar,
                    20, 
                    "Categoria"
                    )
                );

            insertCommand.Parameters.Add(
                new SqlParameter(
                    "@descrizione",
                    SqlDbType.NVarChar,
                    500,
                    "Descrizione"
                    )
                );

            insertCommand.Parameters.Add(
               new SqlParameter(
                   "@prezzoUnitario",
                   SqlDbType.Decimal,
                   3,
                   "PrezzoUnitario"
                   )
               );

            insertCommand.Parameters.Add(
           new SqlParameter(
               "@quantita",
               SqlDbType.Int,
               3,
               "QuantitaDisponibile"
               )
           );
               
            return insertCommand;
        }
        private static SqlCommand ProdottiUpdateCommand(SqlConnection conn)
        {
            SqlCommand updateCommand = new SqlCommand();

            updateCommand.Connection = conn;

            updateCommand.CommandText = "UPDATE Prodotti " +
                      "SET CodiceProdotto= @codiceProdotto, Categoria= @categoria, Descrizione= @descrizione, "+
                      "PrezzoUnitario = @prezzoUnitario, QuantitaDisponibile = @quantita WHERE ID=@ID;";


            updateCommand.CommandType = System.Data.CommandType.Text;

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@ID",
                    SqlDbType.Int,
                    50, 
                    "ID"
                    )
                );

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@codiceProdotto",
                    SqlDbType.NVarChar,
                    10,
                    "CodiceProdotto"
                    )
                );

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@categoria",
                    SqlDbType.NVarChar,
                    20, 
                    "Categoria"
                    )
                );

            updateCommand.Parameters.Add(
                new SqlParameter(
                    "@descrizione",
                    SqlDbType.NVarChar,
                    500, 
                    "Descrizione"
                    )
                );

            updateCommand.Parameters.Add(
               new SqlParameter(
                   "@prezzoUnitario",
                   SqlDbType.Decimal,
                   3,
                   "PrezzoUnitario"
                   )
               );

            updateCommand.Parameters.Add(
           new SqlParameter(
               "@quantita",
               SqlDbType.Int,
               3,
               "QuantitaDisponibile"
               )
           );
            return updateCommand;
        }
        private static SqlCommand ProdottiDeleteCommand(SqlConnection conn)
        {
            SqlCommand deleteCommand = new SqlCommand();
            deleteCommand.Connection = conn;

            deleteCommand.CommandText = "DELETE FROM Prodotti WHERE ID = @ID";


            deleteCommand.CommandType = System.Data.CommandType.Text;

            deleteCommand.Parameters.Add(
                new SqlParameter(
                    "@ID",
                    SqlDbType.Int,
                    50, 
                    "ID"
                    )
                );
            return deleteCommand;
        }



        public static string CheckString()
        {
            string stringa = Console.ReadLine();
            while (string.IsNullOrEmpty(stringa))
            {
                Console.WriteLine("La stringa inserita non è corretta, riprova:");
                stringa = Console.ReadLine();
            }
            return stringa;
        }
        public static string ChoseCategory()
        {
            int sceltaCat = -1;
            while (!(int.TryParse(Console.ReadLine(), out sceltaCat) && sceltaCat >= (int)Categoria.Alimentari && sceltaCat <= (int)Categoria.Sanitari))
                Console.WriteLine("Hai inserito un valore non valido, riprova!");
            return ((Categoria)sceltaCat).ToString();
        }
        enum Categoria
        {
            Alimentari, 
            Cancelleria, 
            Sanitari
        }
        public static decimal CheckDecimal()
        {
            decimal prezzo = -1;
            while (!(decimal.TryParse(Console.ReadLine(), out prezzo) && prezzo > 0))
                Console.WriteLine("Il valore inserito non è corretto, riprova: ");
            return prezzo;
        }
        public static int CheckInt()
        {
            int quantita = -1;
            while (!(int.TryParse(Console.ReadLine(), out quantita) && quantita > 0))
                Console.WriteLine("Il vallore inserito non è corretto, riprova: ");
            return quantita;
        }

        public static Prodotto InsertProductProp(bool isNew)
        {
            Prodotto prod = new Prodotto();
            if (isNew)
            {
                Console.WriteLine("Inserisci il codice del prodotto:");
                prod.CodiceProdotto = Manager.CheckString();
                Console.WriteLine("Inserisci la categoria del prodotto:");
                Console.WriteLine("[0] --> Alimentari,");
                Console.WriteLine("[1] --> Cancelleria,");
                Console.WriteLine("[2] --> Sanitari");
                prod.Categoria = Manager.ChoseCategory();
                Console.WriteLine("Inserisci la descrizione del prodotto:");
                prod.Descrizione = Manager.CheckString();
            }
            Console.WriteLine("Inserisci il prezzo unitario del prodotto:");
            prod.PrezzoUnitario = Manager.CheckDecimal();
            Console.WriteLine("Inserisci la quantià disponibile del prodotto:");
            prod.QuantitaDisponibile = Manager.CheckInt();
            return prod;
        }
    }
}
