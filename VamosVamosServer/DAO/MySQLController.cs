using System.Data;
using MySqlConnector;

namespace VamosVamosServer.DAO;

public class MySQLController : IDisposable
{
    //Connection
    private MySqlConnection connection;

    /// <summary>
    /// Création de la connection
    /// </summary>
    public MySQLController()
    {
        string connectionString = new MySqlConnectionStringBuilder
        {
            Server = "172.18.0.2",
            UserID = "root",
            Password = "root_password",
            Database = "VamosVamos"
        }.ConnectionString;

        connection = new MySqlConnection(connectionString);
    }

    /// <summary>
    /// Exécute une requête
    /// </summary>
    /// <param name="query">Requête</param>
    /// <param name="parameters">Paramètres</param>
    /// <returns>La réponse de la bdd</returns>
    public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
    {
        connection.Open();
        DataTable dataTable = new DataTable();

        using (MySqlCommand command = connection.CreateCommand())
        {
            command.CommandText = query;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
            }

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                dataTable.Load(reader);
            }
        }

        connection.Close();
        return dataTable;
    }

    /// <summary>
    /// Execute un insert et renvoie l'id de celui-ci
    /// </summary>
    /// <param name="query">La requête d'insert</param>
    /// <param name="parameters">Le dictionnaire des paramètres</param>
    /// <returns>L'id de la ligne inséré</returns>
    public long ExecuteInsert(string query, Dictionary<string, object> parameters = null)
    {
        connection.Open();
        long id;

        using (MySqlCommand command = connection.CreateCommand())
        {
            command.CommandText = query + "; SELECT last_insert_rowid();";
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
            }

            id = Convert.ToInt64(command.ExecuteScalar());
        }

        connection.Close();
        return id;
    }

    /// <summary>
    /// Fermeture
    /// </summary>
    public void Dispose()
    {
        if (connection != null)
        {
            if (connection.State != System.Data.ConnectionState.Closed) connection.Close();
            connection.Dispose();
            connection = null;
        }
    }
}