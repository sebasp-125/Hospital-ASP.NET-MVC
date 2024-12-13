using Microsoft.Data.SqlClient;

public class Querys {
    private  string _connection = "Server=DESKTOP-CJE8DS1\\SQLEXPRESS;Database=Hospital;Trusted_Connection=True;TrustServerCertificate=True;";

    
    public InformationUser SearchInformation() {
        using (SqlConnection conn = new SqlConnection(_connection)) {
            string query = @"
            SELECT * FROM Enfermedades;
            SELECT U.Nombre, U.Apellido, U.Email, U.EnfermedadID, E.Nombre, E.Descripcion 
            FROM UsuariosRegistrados U 
            INNER JOIN Enfermedades E ON U.EnfermedadID = E.EnfermedadID 
            WHERE U.Dni IS NULL AND U.Contraseña IS NULL;
            ";
            SqlCommand command = new SqlCommand(query, conn);
            conn.Open();
            using (SqlDataReader reader = command.ExecuteReader()) {
                if (reader.Read()) {
                    return new InformationUser {
                        NombreUsuarioI = reader["Nombre"].ToString(),
                        ApellidoUsuarioI = reader["Apellido"].ToString(),
                        EmailUsuarioI = reader["Email"].ToString()
                    };
                }
            }
            return null;
        }
    }
}
