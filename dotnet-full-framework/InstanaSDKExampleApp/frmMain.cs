using Instana.ManagedTracing.Api;
using Instana.ManagedTracing.Sdk.Spans;
using InstanaSDKExampleApp.Services.Countries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;

namespace InstanaSDKExampleApp
{
    public partial class frmMain : Form
    {
        private HttpClient _mainClient;
        private CountryService _countryService;
        public frmMain()
        {
            // setup the services that we need later on
            _mainClient = new HttpClient();
            _countryService = new CountryService(_mainClient);
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // create a trace for loading the main-window. Nothing interesting happens here.
            // it just displays that any event-handler can be instrumented
            using (var span = CustomSpan.CreateEntry(this, (ISpanContext) null, "Main Window Load"))
            {
                Thread.Sleep(10);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            // when the refresh-button is hit, we have an interesting trace.
            // so let's start the trace with an entry-span, which will be the root.
            using (var span = CustomSpan.CreateEntry(this, (ISpanContext)null, "Get Country List"))
            {
                // set a tag on the span, so we know which region has been selected by the user
                span.SetTag("region", cbRegion.SelectedItem == null ? "NONE SELECTED" : (string)cbRegion.SelectedItem);
                span.SetTag(new string[] { "infrastructure", "machine" }, Environment.MachineName);

                if (cbRegion.SelectedItem == null)
                {
                    MessageBox.Show("Please select a region first");
                    return;
                }

                // call the CountryService to get the countries from the selected region
                // this call will create additional spans! Go ceck CountryService
                string selectedRegion = (string)cbRegion.SelectedItem;
                var result = _countryService.GetCountriesByRegion(selectedRegion);
                if (result.WasSuccessful)
                {
                    FillList(result.Result);
                }
                else
                {
                    if (result.CallException != null)
                    {
                        MessageBox.Show(result.CallException.Message, "Error calling REST-API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        span.SetException(result.CallException);
                    }
                }
                // set the result of the call on the span
                span.SetResult(result.StatusCode.ToString());
            }
        }

        private void FillList(List<Country> countries)
        {
            // create a span for filling the list with the result from the service
            // this will let us know whether this loop takes long or not
            using (var span = CustomSpan.Create(this, SpanType.INTERMEDIATE))
            {
                listView1.Items.Clear();

                foreach (Country country in countries)
                {
                    ListViewItem item = new ListViewItem(new string[] { country.Name, country.Capital, country.Capital, country.Population.ToString() });
                    listView1.Items.Add(item);
                }
            }
        }

        private void BtnOpenPopup_Click(object sender, EventArgs e)
        {
            using (var span = CustomSpan.CreateEntry(this, (ISpanContext)null, "Open Popup Form"))
            {
                frmPopup popup = new frmPopup();
                popup.Show();
            }
        }
    }
}
