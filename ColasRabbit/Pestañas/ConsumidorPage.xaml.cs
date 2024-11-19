namespace ColasRabbit;

public partial class ConsumidorPage : ContentPage
{
    private readonly Consumidor _consumidor;

    public ConsumidorPage()
    {
        InitializeComponent();

        // Inicia el consumidor
        _consumidor = new Consumidor();
        _consumidor.OnMensajeRecibido += MostrarMensaje;
        _consumidor.IniciarConsumo();
    }

    private void MostrarMensaje(string mensaje)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Estado.Text = $"Correo recibido: {mensaje}";
            Estado.TextColor = Colors.Blue;
        });
    }
}