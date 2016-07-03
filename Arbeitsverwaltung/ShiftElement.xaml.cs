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
        public ShiftElement(Shift shift, double wage)
        {
            InitializeComponent();
            ShiftFromToTextBlock.Text =
                $"{shift.StartTime.Day}.{shift.StartTime.Month}.{shift.StartTime.Year}: " +
                $"{shift.StartTime.Hour}:{shift.StartTime.Minute} - " +
                $"{shift.EndTime.Hour}:{shift.EndTime.Minute}";

            ShiftTimeSpanTextBlock.Text = $"{shift.WorkSpan.Hours}:{shift.WorkSpan.Minutes}:{shift.WorkSpan.Seconds}";

            BreakTimeSpan.Text = $"{shift.BreakSpan.Hours}:{shift.BreakSpan.Minutes}:{shift.BreakSpan.Seconds}";

            WageTextBlock.Text = $"{shift.WorkSpan.Hours*wage + shift.WorkSpan.Minutes*(wage/60)}€";
        }
    }
}
