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
    /// Interaction logic for EdgeWeightWindow.xaml
    /// </summary>
    public partial class EdgeWeightWindow : Window
    {
        public EdgeWeightWindow()
        {
            InitializeComponent();

            weightBox.Focus();
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public double Weight
        {
            get
            {
                if (weightBox.Text != null)
                {
                    return double.Parse(weightBox.Text);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
