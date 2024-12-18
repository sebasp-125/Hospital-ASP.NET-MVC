public class LandingViewModel
{
    public InformationUser UsuarioLogin { get; set; } // informacion del Usuario en acción
    public List<InformatioDoctor> InformacionDoctores { get; set; } //Informacion de todos los doctores
    public InformatioDoctor MedicoDeFamilia { get; set; }   //Medico de Familia asignado.
}
