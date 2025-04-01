using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Projekt_01
{
    internal class DatabaseHelper
    {
    private string connectionString = "Data Source=AMIN\\SQLEXPRESS02;Initial Catalog=LagerVerwaltung;Integrated Security=True;Encrypt=False";

    // 🟢 Benutzer hinzufügen (Registrierung)
    public bool InsertUser(int personalId, string benutzername, string passwort)
    {
        string hashedPassword = HashPassword(passwort);
        string query = "INSERT INTO Benutzer (PersonalId, Benutzername, PasswortHash) VALUES (@PersonalId, @Benutzername, @PasswortHash)";
        return ExecuteNonQuery(query, new SqlParameter("@PersonalId", personalId),
                                      new SqlParameter("@Benutzername", benutzername),
                                      new SqlParameter("@PasswortHash", hashedPassword));
    }

    // 🟢 Produkt hinzufügen
    public bool InsertProduct(string name, string kategorie, decimal preis, int lagerbestand)
    {
        string query = "INSERT INTO Produkte (Name, Kategorie, Preis, Lagerbestand) VALUES (@Name, @Kategorie, @Preis, @Lagerbestand)";
        return ExecuteNonQuery(query, new SqlParameter("@Name", name),
                                      new SqlParameter("@Kategorie", kategorie),
                                      new SqlParameter("@Preis", preis),
                                      new SqlParameter("@Lagerbestand", lagerbestand));
    }

    // 🟢 Materialbewegung hinzufügen (Materialfluss)
    public bool InsertMaterialFlow(int produktId, string bewegungstyp, int menge, DateTime datum)
    {
        string query = "INSERT INTO Materialfluss (ProduktId, Bewegungstyp, Menge, Datum) VALUES (@ProduktId, @Bewegungstyp, @Menge, @Datum)";
        return ExecuteNonQuery(query, new SqlParameter("@ProduktId", produktId),
                                      new SqlParameter("@Bewegungstyp", bewegungstyp),
                                      new SqlParameter("@Menge", menge),
                                      new SqlParameter("@Datum", datum));
    }

    // 🟢 Alle Materialfluss-Daten abrufen
    public DataTable GetMaterialFlow()
    {
        string query = "SELECT * FROM Materialfluss";
        return ExecuteQuery(query);
    }

    // 🟢 Materialfluss-Daten nach Produkt abrufen
    public DataTable GetMaterialFlowByProduct(int produktId)
    {
        string query = "SELECT * FROM Materialfluss WHERE ProduktId = @ProduktId";
        return ExecuteQuery(query, new SqlParameter("@ProduktId", produktId));
    }

    // 🟢 Materialbewegung löschen
    public bool DeleteMaterialFlow(int id)
    {
        string query = "DELETE FROM Materialfluss WHERE Id = @Id";
        return ExecuteNonQuery(query, new SqlParameter("@Id", id));
    }

    // 🔵 Hilfsmethoden für Datenbankoperationen 🔵

    // 🟢 Passwort-Hashing
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    // 🟢 Generische Methode für `INSERT`, `UPDATE`, `DELETE`
    private bool ExecuteNonQuery(string query, params SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters);
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message);
                return false;
            }
        }
    }

    // 🟢 Generische Methode für `SELECT`-Abfragen
    private DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddRange(parameters);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message);
                return null;
            }
        }
    }
}
