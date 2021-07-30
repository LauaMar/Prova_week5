using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prova_week5
{
    class Reports
    {
        public static void Report1()
        {
            //l'elenco dei prodotti con giacenza limitata (Quantità Disponibile < 10)

            using (SqlConnection conn = new SqlConnection(Manager.connectionString))
            {
                conn.Open();
                if (conn.State != System.Data.ConnectionState.Open)
                    Console.WriteLine("Errore di connessione...");

                SqlCommand leggi = conn.CreateCommand();
                leggi.CommandType = System.Data.CommandType.Text;
                leggi.CommandText = $"SELECT * FROM Prodotti WHERE QuantitaDisponibile<10;";

                SqlDataReader reader = leggi.ExecuteReader();

                Console.WriteLine();
                Console.WriteLine("{0,-20}{1,-30}{2,-50}{3,-30}{4,-30}", "Codice Prodotto", "Categoria", "Descrizione", "Prezzo", "Quantità");
                Console.WriteLine(new String('_', 150));
                Console.WriteLine();

                while (reader.Read())
                {
                    //int i = reader.GetInt32(0);
                    //DateTime nascita = reader.GetDateTime(3);

                    Console.WriteLine(
                        "{0,-20}{1,-30}{2,-50}{3,-30}{4,-30}",
                        reader["CodiceProdotto"],
                        reader["Categoria"],
                        reader["Descrizione"],
                        reader["PrezzoUnitario"],
                        reader["QuantitaDisponibile"]
                        );
                }

                conn.Close();

            }
        }
        public static void Report2(string categoria)
        {
            //visualizzare titolo, autore e anno di pubblicazione dei romanzi di autori russi, ordinati per autore e,
            //per lo stesso autore, ordinati per anno di pubblicazione

            using (SqlConnection conn = new SqlConnection(Manager.connectionString))
            {
                conn.Open();
                if (conn.State != System.Data.ConnectionState.Open)
                    Console.WriteLine("Errore di connessione...");

                SqlCommand leggi = conn.CreateCommand();
                leggi.CommandType = System.Data.CommandType.Text;
                leggi.CommandText = $"SELECT Categoria, count(*) as [Numero Prodotti] FROM Prodotti "+
                    "WHERE Categoria = @categoria GROUP BY Categoria; ";
                //leggi.CommandText = $"SELECT Articolo.Titolo as[TitoloArticolo], Articolo.DataPubblicazione, Pubblicazione.Titolo as [TitoloPub] " +
                //   "FROM Pubblicazione inner join Articolo on Pubblicazione.ID = Articolo.PubblicazioneID " +
                //   "WHERE YEAR(Articolo.DataPubblicazione)= @yearPub AND Pubblicazione.Titolo = @titoloPub";
                leggi.CommandType = System.Data.CommandType.Text;
                leggi.Parameters.AddWithValue("@categoria", categoria);


                SqlDataReader reader = leggi.ExecuteReader();

                Console.WriteLine();
                Console.WriteLine("{0,-20}{1,-30}", "Categoria", "Numero Prodotti");
                Console.WriteLine(new String('_', 60));
                Console.WriteLine();

                while (reader.Read())
                {
                    //int i = reader.GetInt32(0);
                    //DateTime nascita = reader.GetDateTime(3);

                    Console.WriteLine(
                        "{0,-20}{1,-30}",
                        reader["Categoria"],
                        reader["Numero Prodotti"]
                        );
                }
                conn.Close();

            }
        }
    }
}