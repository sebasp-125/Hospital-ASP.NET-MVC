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

    public string NumeroCelular { get; set; }
    public string Direccion { get; set; }
    public string CorreoElectronico { get; set; }
    public string Contrasena { get; set; }


    // INICIAR SESION 

    public int TipoIdentificacionRef {get;set;}
    public int IdentificacionRef {get;set;}
    public string ContrasenaRef {get;set;}

    // ASIGNACION DE CITA
    public string Sede {get; set;}
    public string FechaCita {get;set;}
    public string HoraCita {get;set;}

    public string estado {get;set;}



// CONEXION A LA BASE DE DATOS
    private string _connection = "Server=DESKTOP-URHTSV2\\SQLEXPRESS;Database=Hospital;Trusted_Connection=True;TrustServerCertificate=True;";



// FUNCION PARA TRAER LA INFORMACION DEL USUARIO LOGUEADO
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


// FUNCION PARA REGISTRAR UN NUEVO USUARIO EN LA BASE DE DATOS
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

// FUNCION PARA ASIGNAR UNA CITA A UN USUARIO
    public bool AsignarLaCita(string Sede, string usuarioid)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(_connection))
            {
                con.Open();

                string verificarCorreo = "SELECT COUNT(*) FROM Citas WHERE UsuarioID = @UsuarioID";

                using (SqlCommand verificarCmd = new SqlCommand(verificarCorreo, con))
                {
                    verificarCmd.Parameters.AddWithValue("@UsuarioID", usuarioid);
                    int existe = (int)verificarCmd.ExecuteScalar();

                    if (existe > 0)
                    {
                        return false;
                    }
                }

                this.estado = "Asignada";
                string query = "insert into Citas (UsuarioID, FechaHora, Estado, Sede) " +
                "VALUES (@UsuarioID, @FechaHora, @Estado, @Sede)";
                
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UsuarioID", int.Parse(usuarioid));
                    cmd.Parameters.AddWithValue("@FechaHora", (this.FechaCita,this.HoraCita).ToString());
                    cmd.Parameters.AddWithValue("@Estado", this.estado);
                    cmd.Parameters.AddWithValue("@Sede", Sede);

                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Se hizo!");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en " + ex.Message);
            Console.WriteLine("No Se hizo!");
            return false;
        }


    }


    // FUNCION PARA EXTRAER DOCTOR DE FAMILIA

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

}
