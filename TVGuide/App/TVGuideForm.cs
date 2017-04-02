using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TVGuide.App;
using TVGuide;


namespace TVGuide.App
{
    public partial class TVGuideForm : System.Windows.Forms.Form
    {
        private TVGuide tvGuide = new TVGuide();


        public TVGuideForm()
        {
            InitializeComponent();
            List<TVGuideChannel> channels = tvGuide.buildData();
            string message = "";
            foreach (TVGuideChannel channel in channels)
            {
                message += channel.Name + "\n";
            }

            MessageBox.Show(message);


        }

        TVGuideChannel TVGuideChannel = new TVGuideChannel();
        
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Clean TV Guide\n A clean program by Adam Fearnehough and Steven Hunter", "Clean TV");
        }

        private void printToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var dlg = new PrintDialog();
            dlg.ShowDialog();
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var preDlg = new PrintPreviewDialog();
            preDlg.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {        
        }

        private void TVGuideForm_Load(object sender, EventArgs e)
        {
        }
    }
}
