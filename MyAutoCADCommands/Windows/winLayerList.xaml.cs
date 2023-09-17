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

namespace MyAutoCADCommands.Windows
{
    /// <summary>
    /// Lógica de interacción para winLayerList.xaml
    /// </summary>
    public partial class winLayerList : Window
    {
        public winLayerList(Objects.LayerObjectCollection Layers)
        {
            InitializeComponent();
            this.DataContext = Layers;
            cboLayers.SelectedIndex = 0;
            
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
