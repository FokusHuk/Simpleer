using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pleer.Views
{
    /// <summary>
    /// Логика взаимодействия для PleerView.xaml
    /// </summary>
    public partial class PleerView : Window
    {
        public PleerView()
        {
            InitializeComponent();
        }
    }
}

// cal:Message.Attach="[Event Click] = [Action PlayMethod]"