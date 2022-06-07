using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.Data_Retrieving;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;

namespace KGB_Dev_.Pages
{
    partial class Index
    {
        [Inject]
        public IDataRetrivingServices IServices { get; set; } = default!;
        private IEnumerable<KGB_Knowledge> ListOfKGB;
        private string searchString1 = "";
        protected override async Task OnInitializedAsync()
        {
            ListOfKGB = await IServices.GetListOfKnowledge();
        }


    }
}
