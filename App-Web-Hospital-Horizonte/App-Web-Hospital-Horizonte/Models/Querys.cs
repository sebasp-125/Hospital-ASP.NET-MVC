using Microsoft.Data.SqlClient;

public class Querys
{
    public int TipoIdentificacion { get; set; }
    public int NumeroIdentificacion { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime Fecha { get; set; }
    public string Genero { get; set; }

    public int NumeroCelular { get; set; }
    public string Direccion { get; set; }
    public string CorreoElectronico { get; set; }
    public string Contrasena { get; set; }
    private string _connection = "Server=DESKTOP-CJE8DS1\\SQLEXPRESS;Database=Hospital;Trusted_Connection=True;TrustServerCertificate=True;";


    public InformationUser SearchInformation()
    {
        using (SqlConnection conn = new SqlConnection(_connection))
        {
            string query = @"
            SELECT * FROM Enfermedades;
            SELECT U.Nombre, U.Apellido, U.Email, U.EnfermedadID, E.Nombre, E.Descripcion 
            FROM UsuariosRegistrados U 
            INNER JOIN Enfermedades E ON U.EnfermedadID = E.EnfermedadID 
            WHERE U.Dni IS NULL AND U.Contraseña IS NULL;
            ";
            SqlCommand command = new SqlCommand(query, conn);
            conn.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new InformationUser
                    {
                        NombreUsuarioI = reader["Nombre"].ToString(),
                        ApellidoUsuarioI = reader["Apellido"].ToString(),
                        EmailUsuarioI = reader["Email"].ToString()
                    };
                }
            }
            return null;
        }
    }

    public Querys GuardarUsuario()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(_connection))
            {
                con.Open();

                string verificarCorreo = "SELECT COUNT(*) FROM UsuariosRegistrados WHERE Email = @Email";

                using (SqlCommand verificarCmd = new SqlCommand(verificarCorreo, con))
                {
                    verificarCmd.Parameters.AddWithValue("@Email", this.CorreoElectronico);
                    int existe = (int)verificarCmd.ExecuteScalar();

                    if (existe > 0)
                    {
                        Console.WriteLine("HOLI");
                        return null;
                    }
                }

                DateOnly fecha = DateOnly.FromDateTime(Fecha);
                string query = "insert into UsuariosRegistrados (Nombre, Apellido, FechaNacimiento, Direccion, Telefono, Email, Contrasena, TipoIdentificacion, Identificacion, Genero) " +
                "VALUES (@Nombre, @Apellido, @FechaNacimiento, @Direccion, @Telefono, @Email, @Contrasena, @TipoIdentificacion, @Identificacion, @Genero)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", this.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", this.Apellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", fecha.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Direccion", this.Direccion);
                    cmd.Parameters.AddWithValue("@Telefono", this.NumeroCelular);
                    cmd.Parameters.AddWithValue("@Email", this.CorreoElectronico);
                    cmd.Parameters.AddWithValue("@Contrasena", this.Contrasena);
                    cmd.Parameters.AddWithValue("@TipoIdentificacion", this.TipoIdentificacion);
                    cmd.Parameters.AddWithValue("@Identificacion", this.NumeroIdentificacion);
                    cmd.Parameters.AddWithValue("@Genero", this.Genero);

                    cmd.ExecuteNonQuery();
                }
            }

            return this;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en " + ex.Message);
            return null;
        }

    }
}
