using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AracListesi
{
    public partial class AracFrm : Form
    {
        public AracFrm()
        {
            InitializeComponent();
        }

        public Arac arac = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            propertyGrid1.SelectedObject = arac;
        }

        private void EkleBtn(object sender, EventArgs e)
        {
            DialogResult= DialogResult.OK;
        }

        private void İptalBtn(object sender, EventArgs e)
        {
            DialogResult= DialogResult.Cancel;
        }
    }
}
