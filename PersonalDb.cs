using System;
using System.Data;
using System.Data.SqlClient;

public class PersonalDb
{
    private string connectionString = "Data Source=AMIN\\SQLEXPRESS02;Initial Catalog=LagerVerwaltung;Integrated Security=True;Encrypt=False";

    // 🔹 Führt eine SQL-Abfrage aus und gibt ein DataTable zurück
    public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
    {
        DataTable dataTable = new DataTable();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
            }
        }
        return dataTable;
    }

    // 🔹 Führt INSERT, UPDATE oder DELETE aus
    public bool ExecuteNonQuery(string query, params SqlParameter[] parameters)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
                return false;
            }
        }
    }

    // 🔹 Personal-Daten abrufen
    public DataTable GetPersonalById(int personalId)
    {
        string query = "SELECT * FROM Personal WHERE Id = @PersonalId";
        return ExecuteQuery(query, new SqlParameter("@PersonalId", personalId));
    }

    // 🔹 Personal hinzufügen
    public bool InsertPersonal(string firstname, string name, string position, string abteilung, decimal gehalt, string benutzername, string passwort)
    {
        string query = "INSERT INTO Personal (Firstname, Name, Position, Abteilung, Gehalt, Benutzername, Passwort) VALUES (@Firstname, @Name, @Position, @Abteilung, @Gehalt, @Benutzername, @Passwort)";
        return ExecuteNonQuery(query,
            new SqlParameter("@Firstname", firstname),
            new SqlParameter("@Name", name),
            new SqlParameter("@Position", position),
            new SqlParameter("@Abteilung", abteilung),
            new SqlParameter("@Gehalt", gehalt),
            new SqlParameter("@Benutzername", benutzername),
            new SqlParameter("@Passwort", passwort));
    }

    // 🔹 Personal aktualisieren
    public bool UpdatePersonal(int personalId, string firstname, string name, string position, string abteilung, decimal gehalt, string benutzername, string passwort)
    {
        string query = "UPDATE Personal SET Firstname = @Firstname, Name = @Name, Position = @Position, Abteilung = @Abteilung, Gehalt = @Gehalt, Benutzername = @Benutzername, Passwort = @Passwort WHERE Id = @PersonalId";
        return ExecuteNonQuery(query,
            new SqlParameter("@PersonalId", personalId),
            new SqlParameter("@Firstname", firstname),
            new SqlParameter("@Name", name),
            new SqlParameter("@Position", position),
            new SqlParameter("@Abteilung", abteilung),
            new SqlParameter("@Gehalt", gehalt),
            new SqlParameter("@Benutzername", benutzername),
            new SqlParameter("@Passwort", passwort));
    }

    // 🔹 Personal löschen
    public bool DeletePersonal(int personalId)
    {
        string query = "DELETE FROM Personal WHERE Id = @PersonalId";
        return ExecuteNonQuery(query, new SqlParameter("@PersonalId", personalId));
    }
}
