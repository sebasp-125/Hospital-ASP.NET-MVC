using Microsoft.Data.SqlClient;

public class Querys {
    private readonly string _connection = "Server=DESKTOP-CJE8DS1\\SQLEXPRESS;Database=Hospital;Trusted_Connection=True;TrustServerCertificate=True;";

    public InformationUser SearchInformation() {
        using (SqlConnection conn = new SqlConnection(_connection)) {
            string query = "SELECT * FROM UsuariosRegistrados";
            SqlCommand command = new SqlCommand(query, conn);
            conn.Open();
            using (SqlDataReader reader = command.ExecuteReader()) {
                if (reader.Read()) {
                    return new InformationUser {
                        NombreUsuarioI = reader["Nombre"].ToString()
                    };
                }
            }
            return null;
        }
    }
}
