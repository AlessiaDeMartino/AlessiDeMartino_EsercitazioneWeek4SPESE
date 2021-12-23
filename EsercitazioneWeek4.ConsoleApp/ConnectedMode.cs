using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsercitazioneWeek4.ConsoleApp
{
    static class ConnectedMode
    {
        static string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GestioneSpese;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void InserisciSpesa()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }                
                    int cat;
                    do
                    {
                        Console.WriteLine("Indicare l'Id della categoria della spesa");
                    } while (!(int.TryParse(Console.ReadLine(), out cat)));

                bool exist=GetResultCategoryById(cat, connessione);

                if (exist==true)

                {
                    connessione.Open();
                    Console.WriteLine("Inserire la data della spesa");
                    DateTime dataSpesa=DateTime.Parse(Console.ReadLine());
                    Console.WriteLine("Inserire la descrizione della spesa");
                    string descrizioneSpesa=Console.ReadLine();
                    Console.WriteLine("Inserire nome Utente spesa");
                    string utenteSpesa=Console.ReadLine();
                    decimal importo;
                     do {
                        Console.WriteLine("Inserire importo: ");
                    } while (!(decimal.TryParse(Console.ReadLine(),out importo)));

                    string insertSQL = "insert into Spese values (@DataSpesa, @Descrizione,@Utente,@Importo,@Approvato,@CategoriaId)";
                    SqlCommand insertCommand = connessione.CreateCommand();
                    insertCommand.CommandText = insertSQL;
                    insertCommand.Parameters.AddWithValue("@DataSpesa", dataSpesa);
                    insertCommand.Parameters.AddWithValue("@Descrizione", descrizioneSpesa);
                    insertCommand.Parameters.AddWithValue("@Utente", utenteSpesa);
                    insertCommand.Parameters.AddWithValue("@Importo", importo);
                    insertCommand.Parameters.AddWithValue("@Approvato", 0);
                    insertCommand.Parameters.AddWithValue("@CategoriaId", cat);
                    int righeInserite = insertCommand.ExecuteNonQuery();

                    if (righeInserite >= 1)
                        Console.WriteLine($"{righeInserite} riga/righe inserite correttamente");
                    else
                        Console.WriteLine("OOOOOOPS ...qualcosa non torna!");
                }
                else if (exist == false)
                {
                    Console.WriteLine("Non è stata trovata una categoria con quell'ID.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("Connessione chiusa");
            }
        }

        internal static void TotaleSpeseCategorie()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }
                                 
                    SqlCommand command = new SqlCommand();
                    command.Connection = connessione;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select Categorie.Categoria, Sum(Spese.Importo) as Totale from Spese join Categorie on spese.CategoriaId=Categorie.Id group by Categorie.Categoria";
                    
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var tot = (decimal)reader["Totale"];
                        var categoria=(string)reader["Categoria"];

                        Console.WriteLine($"Totale Spesa: {tot}, Categoria:{categoria}");
                    }
                    Console.WriteLine("-------------------------------");            

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("Connessione chiusa");
            }
        }

        internal static void TotaleSpeseCategoriaSceltaDallUtente()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }
                int cat;
                do
                {
                    Console.WriteLine("Indicare l'Id della categoria della spesa");
                } while (!(int.TryParse(Console.ReadLine(), out cat)));

                bool exist = GetResultCategoryById(cat, connessione);
                if (exist == true)
                {
                    connessione.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connessione;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select Spese.CategoriaId, Sum(Spese.Importo) as Totale from Spese where Spese.CategoriaId=@cat group by Spese.CategoriaId ";
                    command.Parameters.AddWithValue("@cat", cat);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var tot = (decimal)reader["Totale"];                      

                        Console.WriteLine($"Totale Spesa: {tot} per la categoria con ID: {cat}");
                    }
                    Console.WriteLine("-------------------------------");
                }
                else if (exist == false)
                {
                    Console.WriteLine("Errore");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("Connessione chiusa");
            }
        }

        internal static void MostraSpeseUtente(string? utente)
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }
                SqlCommand command = new SqlCommand();
                command.Connection = connessione;
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from Spese where Utente=@ut";
                command.Parameters.AddWithValue("@ut", utente);
                SqlDataReader reader = command.ExecuteReader();
                Console.Clear();
                Console.WriteLine($"\n---- Elenco Spese {utente} ----");
                Console.WriteLine();
                Console.WriteLine("ID \t Data Spesa \t Descrizione \t Importo \t");
                Console.WriteLine("--------------------------------");
                while (reader.Read())
                {
                    string formattedDate = ((DateTime)reader["DataSpesa"]).ToString("dd-MMM-yyyy");
                    Console.WriteLine(reader["Id"].ToString() + "\t" + formattedDate + "\t" + (string)reader["Descrizione"] + "\t" + (decimal)reader["Importo"]);
                }
                Console.WriteLine("-------------------------------");

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("Connessione chiusa");
            }
        }
        internal static void MostraSpeseApprovate()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }
                SqlCommand command = new SqlCommand();
                command.Connection = connessione;
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from Spese where Approvato=@app";
                command.Parameters.AddWithValue("@app", 1);
                SqlDataReader reader = command.ExecuteReader();
                Console.Clear();
                Console.WriteLine("\n---- Elenco Spese Approvate ----");
                Console.WriteLine();
                Console.WriteLine("ID \t Data Spesa \t Descrizione \t Utente \t Importo \t");
                Console.WriteLine("--------------------------------");
                while (reader.Read())
                {
                    string formattedDate = ((DateTime)reader["DataSpesa"]).ToString("dd-MMM-yyyy");
                    Console.WriteLine(reader["Id"].ToString() + "\t" + formattedDate + "\t" + (string)reader["Descrizione"] + "\t" + (string)reader["Utente"] + "\t" + (decimal)reader["Importo"] + (int)reader["CategoriaId"]);
                }
                Console.WriteLine("-------------------------------");            

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("Connessione chiusa");
            }
        }
        public static void CancellaSpesa()
        {
            DataSet spesaDS = new DataSet();
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al db");
                else
                    Console.WriteLine("NON connessi al db");

                var spesaAdapter = InizializzaDataSetEAdapter(spesaDS, connessione);

                connessione.Close();
                Console.WriteLine("Connessione chiusa");
                Console.WriteLine();
                Console.Write("ID della spesa da cancellare: ");
                int id;
                while (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.Write("Formato errato. Riprova. ID della spesa da cancellare: ");
                };
                DataRow rigaDaEliminare = spesaDS.Tables["Spese"].Rows.Find(id); //PK
                if (rigaDaEliminare != null)
                {
                    rigaDaEliminare.Delete();
                }
                //riconciliazione
                spesaAdapter.Update(spesaDS, "Spese");
                Console.WriteLine("Database Aggiornato");

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connessione.Close();
            }
        }
        public static SqlDataAdapter InizializzaDataSetEAdapter(DataSet spesaDS, SqlConnection connessione)
        {
            SqlDataAdapter speseAdapter = new SqlDataAdapter();

            //Fill
            speseAdapter.SelectCommand = new SqlCommand("Select * from Spese", connessione);
            speseAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            //DELETE
            speseAdapter.DeleteCommand = GeneraDeleteCommand(connessione);

            speseAdapter.Fill(spesaDS, "Spese");
            return speseAdapter;
        }
        public static SqlCommand GeneraDeleteCommand(SqlConnection connessione)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connessione;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Delete from Spese where ID=@id";
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "ID"));
            return cmd;
        }
        internal static void ApprovaSpesa()
        {
            using SqlConnection connessione = new SqlConnection(connectionStringSQL);
            try
            {
                connessione.Open();
                if (connessione.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Connessi al DB");
                }
                else
                {
                    Console.WriteLine("NON connessi al DB");
                }
                int spesaId;
                do
                {
                    Console.WriteLine("\nIndicare l'Id della spesa da approvare");
                } while (!(int.TryParse(Console.ReadLine(), out spesaId)));

                bool existSpesa = GetResultSpesaById(spesaId, connessione);

                if (existSpesa==true)
                {
                    connessione.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connessione;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "update Spese set Approvato=@app where Id=@id";
                    command.Parameters.AddWithValue("@app", true);
                    command.Parameters.AddWithValue("@id", spesaId);

                    int rigaInserita = command.ExecuteNonQuery();
                    if (rigaInserita == 1)
                    {
                        connessione.Close();
                        Console.WriteLine("Spesa Approvata!");

                    }
                    else
                    {
                        connessione.Close();
                        Console.WriteLine("Qualcosa è andato storto...:(");

                    }
                }
                else if (existSpesa == false)
                {
                    Console.WriteLine("Errore");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Errore SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                connessione.Close();
                Console.WriteLine("Connessione chiusa");
            }
        }
        private static bool GetResultSpesaById(int spesaId, SqlConnection connessione)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connessione;
            command.CommandType = CommandType.Text;
            command.CommandText = "select * from Spese where Id=@id";
            command.Parameters.AddWithValue("@id", spesaId);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read() == false)
            {
                connessione.Close();
                return false;
            }
            else
            {
                connessione.Close();
                return true;
            }
        }
        private static bool GetResultCategoryById(int cat, SqlConnection connessione)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connessione;
            command.CommandType = CommandType.Text;
            command.CommandText = "select * from Categorie where Id=@cat";
            command.Parameters.AddWithValue("@cat", cat);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read() == false)
            {
                connessione.Close();
                return false;
            }
            else
            {
                connessione.Close();
                return true;
            }
        }
    }
}

