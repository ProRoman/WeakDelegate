using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly WeakEvent<EventHandler> _click = new WeakEvent<EventHandler>();

        private IList<Subscriber> subscribers = new List<Subscriber>();

        private int count = 1;

        public event EventHandler Click
        {
            add { _click.AddHandler(value); }
            remove { _click.RemoveHandler(value); }
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

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            subscribers.Clear();
            await Task.Factory.StartNew(() =>
            {
                while (IsDelegateAlive())
                {
                    Thread.Sleep(10000);
                }
            });
            if (_click.Target == null)
            {
                report.Visibility = Visibility.Visible;
            }
        }

        private bool IsDelegateAlive()
        {
            EventHandler eventHandler = _click.Target;
            return eventHandler != null;
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
    }
}
