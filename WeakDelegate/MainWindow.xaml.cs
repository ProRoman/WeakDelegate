using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeakDelegate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WeakDelegate<EventHandler> _click = new WeakDelegate<EventHandler>();

        private IList<Subscriber> subscribers = new List<Subscriber>();

        private int count = 1;

        public event EventHandler Click
        {
            add { _click.Combine(value); }
            remove { _click.Remove(value); }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i <= 1000; i++)
            {
                subscribers.Add(new Subscriber(count++));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            subscribers.Clear();
        }
    }

    public class Subscriber
    {
        private readonly String _name;

        public Subscriber(int number)
        {
            _name = "Subscriber" + number;
            ((MainWindow) Application.Current.MainWindow).Click += OnClick;
        }

        private void OnClick(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        ~Subscriber()
        {
            Console.WriteLine(_name + " Destroed");
        }
    }
}
