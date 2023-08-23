using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace FIFA22_INFO
{
    namespace LiveContext.Designer.GUI.Components
    {
        public class AutoScrollPreventer : StackPanel
        {
            public AutoScrollPreventer()
            {

                this.RequestBringIntoView += delegate (object sender, RequestBringIntoViewEventArgs e)
                {
                    // stop this event from bubbling so that a scrollviewer doesn't try to BringIntoView..
                    e.Handled = true;
                };

            }
        }
    }
}
