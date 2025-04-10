using System.Data;
using MySqlConnector;

namespace ScorePALServer.DAO;

public class MySqlController : IDisposable
{
    //Connection
    private MySqlConnection connection;

    /// <summary>
    /// Création de la connection
    /// </summary>
    public MySqlController()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // racine du projet
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        string connectionString = new MySqlConnectionStringBuilder
        {
            // Utilisation du container pour facilement changer de bdd, pour le reste utiliser les variables d'environnement
            Server = Environment.GetEnvironmentVariable("DB_HOST"),
            UserID = configuration.GetConnectionString("DB_USER"),
            Password = configuration.GetConnectionString("DB_PASSWORD"),
            Database = configuration.GetConnectionString("DB_NAME"),
            Port = uint.Parse(configuration.GetConnectionString("DB_PORT") ?? throw new InvalidOperationException())
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
            command.CommandText = query + "; SELECT LAST_INSERT_ID();";
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