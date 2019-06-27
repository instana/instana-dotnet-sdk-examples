using Instana.ManagedTracing.Sdk.Spans;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstanaSDKExampleApp
{
    public partial class frmPopup : Form
    {
        public frmPopup()
        {
            InitializeComponent();
        }

        private void FrmPopup_Load(object sender, EventArgs e)
        {
            using (var span = CustomSpan.Create(this, SpanType.INTERMEDIATE, "Popup Form Load"))
            {
                Thread.Sleep(10);
            }
        }
    }
}
