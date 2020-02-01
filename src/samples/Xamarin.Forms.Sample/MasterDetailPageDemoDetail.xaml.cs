using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Sample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPageDemoDetail : ContentPage
    {
        public MasterDetailPageDemoDetail()
        {
            InitializeComponent();

            var button = FindByName("TestButton") as Button;
            button.Text = string.Format("Test Button => {0}", Guid.NewGuid());
        }
    }
}