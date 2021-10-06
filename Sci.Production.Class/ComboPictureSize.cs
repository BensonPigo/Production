using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// Combo Factory
    /// </summary>
    public partial class ComboPictureSize : Win.UI.ComboBox
    {
        /// <summary>
        /// TargetPictureBox
        /// </summary>
        public PictureBox TargetPictureBox { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboPictureSize"/> class.
        /// </summary>
        public ComboPictureSize()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(100, 23);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboPictureSize"/> class.
        /// </summary>
        /// <param name="container">container</param>
        public ComboPictureSize(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(100, 23);
        }

        /// <summary>
        /// SetDataSource
        /// </summary>
        /// <param name="pictureBox">pictureBox</param>
        public void SetDataSource(PictureBox pictureBox)
        {
            this.TargetPictureBox = pictureBox;
            this.DisplayMember = "Text";
            this.ValueMember = "Value";
            this.DataSource =
                Enum.GetValues(typeof(PictureBoxSizeMode))
                    .Cast<PictureBoxSizeMode>()
                    .Where(s => s != PictureBoxSizeMode.AutoSize)
                    .Select(item => new
                    {
                        Text = item.ToString(),
                        Value = item,
                    })
                    .ToList();

            this.SelectedValue = this.TargetPictureBox.SizeMode;

            this.SelectedIndexChanged += new System.EventHandler(this.ComboPictureSize_SelectedIndexChanged);
        }

        /// <inheritdoc/>
        private void ComboPictureSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.TargetPictureBox.SizeMode = (PictureBoxSizeMode)this.SelectedValue;
        }
    }
}
