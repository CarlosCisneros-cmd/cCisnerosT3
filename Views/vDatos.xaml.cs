namespace cCisnerosT3.Views;

public partial class vDatos : ContentPage
{
    const decimal porcentajeIess = 0.0945m;

    public vDatos(string tipo, string identificacion, string nombres, string apellidos,
                  DateTime fecha, string correo, decimal salario)
    {
        InitializeComponent();
        lblTipo.Text = tipo;
        lblIdentificacion.Text = identificacion;
        lblNombres.Text = nombres;
        lblApellidos.Text = apellidos;
        lblFecha.Text = fecha.ToString("dd/MM/yyyy");
        lblCorreo.Text = correo;
        lblSalario.Text = "$" + salario.ToString("0.00");
        lblAporte.Text = "$" + Math.Round(salario * porcentajeIess, 2).ToString("0.00");
    }

    private async void btnExportar_Clicked(object sender, EventArgs e)
    {
        try
        {
            string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory(carpeta);

            string nombreArchivo = "Contacto_" + lblIdentificacion.Text + "_"
                                   + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            string ruta = Path.Combine(carpeta, nombreArchivo);

            string contenido =
                "DATOS DEL CONTACTO" + Environment.NewLine +
                "Tipo de identificación: " + lblTipo.Text + Environment.NewLine +
                "Identificación: " + lblIdentificacion.Text + Environment.NewLine +
                "Nombres: " + lblNombres.Text + Environment.NewLine +
                "Apellidos: " + lblApellidos.Text + Environment.NewLine +
                "Fecha: " + lblFecha.Text + Environment.NewLine +
                "Correo: " + lblCorreo.Text + Environment.NewLine +
                "Salario: " + lblSalario.Text + Environment.NewLine +
                "Aporte IESS (9.45%): " + lblAporte.Text + Environment.NewLine;

            await File.WriteAllTextAsync(ruta, contenido);

            await DisplayAlertAsync("Archivo exportado",
                "Nombre: " + nombreArchivo + Environment.NewLine + "Ruta: " + carpeta, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "Cerrar");
        }
    }
}
