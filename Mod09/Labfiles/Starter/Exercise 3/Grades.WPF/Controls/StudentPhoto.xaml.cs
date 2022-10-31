using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Grades.WPF
{
    public partial class StudentPhoto : UserControl
    {
        #region Constructor
        public StudentPhoto()
        {
            InitializeComponent();
        }
        #endregion

        #region Storyboard
        // Exercise 3: Task 2a: Handle mouse events to trigger the storyboards that animate the photograph
        public void OnMouseEnter()
        {
            (this.Resources["sbMouseEnter"] as Storyboard).Begin();
        }

        public void OnMouseLeave()
        {
            (this.Resources["sbMouseLeave"] as Storyboard).Begin();
        }
        #endregion
    }
}
