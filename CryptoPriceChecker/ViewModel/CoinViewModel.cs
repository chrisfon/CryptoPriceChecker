using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using CryptoPriceChecker.Model;



namespace CryptoPriceChecker.ViewModel
{
    public class CoinViewModel : INotifyPropertyChanged
    {

        private string urlB;
        private CoinData data;
        private Boolean visible;
        private string buscarUrl;

        public Boolean isVisible 
        { 
            get { return visible; } 
            set { visible = value; OnPropertyChanged(); }  
        }
        public string Url
        {
            get { return urlB; }
            set { urlB = value; OnPropertyChanged(); }
        }

        public CoinData Data
        {
            
            get => data;

            set
            {
                data = value;
                OnPropertyChanged();
            }
        }

        public ICommand buscarCommand { get; set; }

        public CoinViewModel()
        {
            
            buscarCommand = new Command(async (ticker) =>
            {
                var buscar = ticker as string;
                var buscar2 = buscar.ToUpper();
                buscarUrl = buscar2.ToLower();
                urlB = $"https://assets.coincap.io/assets/icons/{buscarUrl}@2x.png";
               // Url = url;
                await GetData($"https://min-api.cryptocompare.com/data/price?fsym={buscar2}&tsyms=USD,CRC");

            });

        }

        private async Task GetData(String url)
        {
            var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(url);
            respuesta.EnsureSuccessStatusCode();
            
            var jSonResult = await respuesta.Content.ReadAsStringAsync();
            //data2 = JsonConvert.DeserializeObject<CoinData>(jSonResult).ToString();
            var resultado = JsonConvert.DeserializeObject<CoinData>(jSonResult);

           Data = resultado;
            Url = urlB;
            isVisible = true;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
