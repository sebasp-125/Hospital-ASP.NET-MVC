using Microsoft.Data.SqlClient;

public class Querys
{
    // REGISTRAR NUEVO
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


    // INICIAR SESION 

    public int TipoIdentificacionRef {get;set;}
    public int IdentificacionRef {get;set;}
    public string ContrasenaRef {get;set;}
// recerda cambiar el servidor de la base de datos
    private string _connection = "Server=DESKTOP-CJE8DS1\\SQLEXPRESS;Database=Hospital;Trusted_Connection=True;TrustServerCertificate=True;";


    public InformationUser SearchInformation()
    {
        using (SqlConnection conn = new SqlConnection(_connection))
        {
            
            string query = @"SELECT 
                U.UsuarioID,
                U.Nombre, 
                U.Apellido,
                U.Email,
                U.Genero,
                U.Identificacion,
                U.FechaNacimiento,
                U.Telefono, 
                U.Direccion,
                E.NombreEnfermedad, 
                E.Descripcion
            FROM 
                UsuariosRegistrados U
            LEFT JOIN Enfermedades E ON U.EnfermedadID = E.EnfermedadID
            WHERE 
                U.TipoIdentificacion = TipoIdentificacion 
                AND Identificacion = @Identificacion 
                AND Contrasena = @Contrasena;
            ";
            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@TipoIdentificacion", TipoIdentificacionRef);
            command.Parameters.AddWithValue("@Identificacion",IdentificacionRef);
            command.Parameters.AddWithValue("@Contrasena",ContrasenaRef);
            conn.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new InformationUser
                    {
                        idUsuario = reader["UsuarioID"].ToString(),
                        NombreUsuarioI = reader["Nombre"].ToString(),
                        ApellidoUsuarioI = reader["Apellido"].ToString(),
                        EmailUsuarioI = reader["Email"].ToString(),
                        Identificacion = reader["Identificacion"].ToString(),
                        FechaNacimiento = reader["FechaNacimiento"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Direccion = reader["Direccion"].ToString(),
                        NombreEnfermedad = reader["NombreEnfermedad"].ToString(),
                        DescripcionEnfermedad = reader["Descripcion"].ToString(),
                        Genero = reader["Genero"].ToString(),
                    };
                }
            }
            return null;
        }
    }

    //EXTRAER DOCTOR

    public List<InformatioDoctor> Doctores()
{
    var listaDoctores = new List<InformatioDoctor>();

    using (SqlConnection conn = new SqlConnection(_connection))
    {
        conn.Open();
        string query = @"SELECT 
                            U.Nombre AS NombreMedico,
                            U.Apellido AS ApellidoMedico,
                            U.Telefono,
                            U.Email,
                            U.DoctorID,
                            E.Nombre AS EspecialidadMedico
                        FROM Doctores U
                        INNER JOIN Especialidades E ON U.EspecialidadID = E.EspecialidadID";

        SqlCommand command = new SqlCommand(query, conn);

        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read()) 
            {
                listaDoctores.Add(new InformatioDoctor
                {
                    id = reader["DoctorID"].ToString(),
                    NombreDoctor = reader["NombreMedico"].ToString(),
                    ApellidoDoctor = reader["ApellidoMedico"].ToString(),
                    EspecialidadDoctor = reader["EspecialidadMedico"].ToString(),
                    Email = reader["Email"].ToString(),
                    Telefono = reader["Telefono"].ToString()
                });
            }
        }
    }
    return listaDoctores; 
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