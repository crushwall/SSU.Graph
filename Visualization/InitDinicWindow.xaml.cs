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
using System.Windows.Shapes;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for InitDinicWindow.xaml
    /// </summary>
    public partial class InitDinicWindow : Window
    {
        public InitDinicWindow()
        {
            InitializeComponent();

            sourceBox.Focus();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public int Sourse
        {
            get { return int.Parse(sourceBox.Text); }
        }

        public int Sink
        {
            get { return int.Parse(sinkBox.Text); }
        }
    }
}
