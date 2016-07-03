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
using Server.Database;

namespace Arbeitsverwaltung
{
    /// <summary>
    /// Interaktionslogik für ShiftElement.xaml
    /// </summary>
    public partial class ShiftElement : UserControl
    {
        public ShiftElement(Shift shift)
        {
            InitializeComponent();

            textBlockShiftTime.Text = $"{shift.StartTime} - {shift.EndTime}";
            textBlockShiftGestime.Text = shift.WorkSpan.ToString();

            textBlockBreakGestime.Text = shift.BreakSpan.ToString();
        }
    }
}
