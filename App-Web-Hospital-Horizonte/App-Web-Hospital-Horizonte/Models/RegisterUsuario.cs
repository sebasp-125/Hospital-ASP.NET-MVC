using Microsoft.Data.SqlClient;
using System.Globalization;

public class RegisterUsuario {
    public int TipoIdentificacion { get; set; }
    public int NumeroIdentificacion { get; set; }
    public string Nombre { get; set; }
    public string Apellido {get;set;}
    public DateTime Fecha {get;set;}
    public string Genero {get; set;}

    public int NumeroCelular { get; set; }
    public string Direccion {get;set;}
    public string CorreoElectronico { get; set; }
    public string Contrasena { get; set; }

    private string connectionString = "Server=DESKTOP-URHTSV2\\SQLEXPRESS;Database=Hospital;Trusted_Connection=True;TrustServerCertificate=True;";
    public RegisterUsuario GuardarUsuario()
    {
       
        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string verificarCorreo = "SELECT COUNT(*) FROM UsuariosRegistrados WHERE Email = @Email";

                using (SqlCommand verificarCmd = new SqlCommand(verificarCorreo, con))
                {
                    verificarCmd.Parameters.AddWithValue("@Email", this.CorreoElectronico );
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
                    cmd.Parameters.AddWithValue("@FechaNacimiento",fecha.ToString("yyyy-MM-dd"));
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

