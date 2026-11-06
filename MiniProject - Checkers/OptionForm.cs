using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MiniProject___Checkers
{
    public partial class OptionForm : Form
    {
        public OptionForm()
        {
            InitializeComponent();
        }

      
        private void btnMultiplayer_Click(object sender, EventArgs e)
        {

            BoardForm boardForm = new BoardForm();
            boardForm.Show();

            this.Hide();
        }
    }
}
