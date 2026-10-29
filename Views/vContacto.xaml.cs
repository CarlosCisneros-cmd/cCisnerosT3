using System.Globalization;
using System.Text.RegularExpressions;

namespace cCisnerosT3.Views;

public partial class vContacto : ContentPage
{
    public vContacto()
    {
        InitializeComponent();
        dpFecha.Date = DateTime.Today;
        txtIdentificacion.IsEnabled = false;
    }

    private string TipoSeleccionado()
    {
        return pkTipo.SelectedItem?.ToString() ?? "";
    }

    private void pkTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtIdentificacion.Text = "";
        txtIdentificacion.IsEnabled = true;

        switch (TipoSeleccionado())
        {
            case "CI":
                txtIdentificacion.MaxLength = 10;
                txtIdentificacion.Keyboard = Keyboard.Numeric;
                break;
            case "RUC":
                txtIdentificacion.MaxLength = 13;
                txtIdentificacion.Keyboard = Keyboard.Numeric;
                break;
            default:
                txtIdentificacion.MaxLength = 20;
                txtIdentificacion.Keyboard = Keyboard.Default;
                break;
        }
    }

    private void txtIdentificacion_TextChanged(object sender, TextChangedEventArgs e)
    {
        string tipo = TipoSeleccionado();
        if (tipo != "CI" && tipo != "RUC")
            return;

        string soloNumeros = new string((e.NewTextValue ?? "").Where(char.IsDigit).ToArray());
        if (soloNumeros != e.NewTextValue)
            txtIdentificacion.Text = soloNumeros;
    }

    private bool SoloLetras(string texto)
    {
        foreach (char c in texto)
        {
            if (!char.IsLetter(c) && c != ' ')
                return false;
        }
        return true;
    }

    private async Task<bool> Validar()
    {
        string tipo = TipoSeleccionado();
        string identificacion = (txtIdentificacion.Text ?? "").Trim();
        string nombres = (txtNombres.Text ?? "").Trim();
        string apellidos = (txtApellidos.Text ?? "").Trim();
        string correo = (txtCorreo.Text ?? "").Trim();
        string salarioTexto = (txtSalario.Text ?? "").Trim().Replace(',', '.');

        if (tipo == "")
        {
            await DisplayAlertAsync("Alerta", "Seleccione el tipo de identificación", "OK");
            return false;
        }

        if (identificacion == "")
        {
            await DisplayAlertAsync("Alerta", "Ingrese la identificación", "OK");
            return false;
        }

        if (tipo == "CI" && (identificacion.Length != 10 || !identificacion.All(char.IsDigit)))
        {
            await DisplayAlertAsync("Alerta", "La cédula debe tener 10 números", "OK");
            return false;
        }

        if (tipo == "RUC" && (identificacion.Length != 13 || !identificacion.All(char.IsDigit)))
        {
            await DisplayAlertAsync("Alerta", "El RUC debe tener 13 números", "OK");
            return false;
        }

        if (nombres == "" || !SoloLetras(nombres))
        {
            await DisplayAlertAsync("Alerta", "Ingrese nombres válidos (solo letras)", "OK");
            return false;
        }

        if (apellidos == "" || !SoloLetras(apellidos))
        {
            await DisplayAlertAsync("Alerta", "Ingrese apellidos válidos (solo letras)", "OK");
            return false;
        }

        if (!Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlertAsync("Alerta", "Ingrese un correo válido", "OK");
            return false;
        }

        if (!decimal.TryParse(salarioTexto, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal salario) || salario <= 0)
        {
            await DisplayAlertAsync("Alerta", "El salario debe ser un número mayor a 0", "OK");
            return false;
        }

        return true;
    }

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (!await Validar())
                return;

            decimal salario = decimal.Parse(txtSalario.Text!.Trim().Replace(',', '.'), CultureInfo.InvariantCulture);

            await Navigation.PushAsync(new vDatos(
                TipoSeleccionado(),
                txtIdentificacion.Text!.Trim(),
                txtNombres.Text!.Trim(),
                txtApellidos.Text!.Trim(),
                (DateTime)dpFecha.Date!,
                txtCorreo.Text!.Trim(),
                salario));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "Cerrar");
        }
    }
}
