using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Sample
{

    public class MasterDetailPageDemoMasterMenuItem
    {
        public MasterDetailPageDemoMasterMenuItem()
        {
            TargetType = typeof(MasterDetailPageDemoMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}