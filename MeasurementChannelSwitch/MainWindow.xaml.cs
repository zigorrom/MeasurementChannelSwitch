using MeasurementChannelSwitch.Properties;
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

namespace MeasurementChannelSwitch
{
    internal enum Switch
    {
        On,
        Off
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Layout _layout;
        private int _currentChannel;
        private LayoutHelper _layoutHelper;
        
        private Button _currentButton;
        private const int MAX_CHANNELS = 32;
        private object syncRoot = new object();
        private SolidColorBrush _defaultBrush;
        private SolidColorBrush _OnBrush;
        private ChannelSwitch _switch;


        public MainWindow()
        {
            InitializeComponent();
            _defaultBrush = (SolidColorBrush) Resources["DefaultBrush"];
            _OnBrush = Brushes.Green;
            _switch = new ChannelSwitch();
            _switch.Setup();
            _switch.SwitchChannel(1, true);
            _switch.Exit();
        }

        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                throw new Exception("Refferenced object is not button");
            var N = ParseChannelNumber(button);
            await SwitchChannel(N, button);
            
        }

        private int ParseChannelNumber(Button sender)
        {
            var c = sender.Content.ToString() ;
            var n =int.Parse(c);
            if(n>MAX_CHANNELS||n<0)
                throw new Exception("Wrong channel number");
            return n;
        }

        private void SwitchOnBackground(Button sender)
        {
            sender.Background = _OnBrush;
        }
        private void SwitchOffBackground(Button sender)
        {
            sender.Background = _defaultBrush;
        }


        private async Task SwitchChannel(int ChannelNumber, Button Sender)
        {
            lock(syncRoot)
            {
                if(Object.ReferenceEquals(Sender, _currentButton))
                {
                    SwitchOffBackground(Sender);
                    SendRequest(_currentChannel, Switch.Off);
                    _currentChannel = 0;
                    _currentButton = null;
                }
                else
                {
                    if(_currentButton!=null)
                    {
                        SwitchOffBackground(_currentButton);
                        SendRequest(_currentChannel, Switch.Off);
                    }

                    _currentButton = Sender;
                    _currentChannel = ChannelNumber;
                    SwitchOnBackground(_currentButton);
                    SendRequest(ChannelNumber, Switch.On);
                    
                }
            }
        }

        private bool SendRequest(int channelNumber, Switch state)
        {
            return false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var a = new ContactPadsControl();
            a.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
