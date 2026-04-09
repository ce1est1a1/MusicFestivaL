using System.Drawing;
using System.Windows.Forms;

namespace MusicFestivalDeserializer
{
    public class DetailsForm : Form
    {
        private PropertyGrid propertyGrid;
        private Label titleLabel;

        public DetailsForm(string title, object data)
        {
            Text = "Подробная информация";
            StartPosition = FormStartPosition.CenterParent;
            Width = 720;
            Height = 560;
            MinimumSize = new Size(650, 460);

            titleLabel = new Label();
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 42;
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            titleLabel.Padding = new Padding(10, 0, 0, 0);
            titleLabel.Text = title;

            propertyGrid = new PropertyGrid();
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.SelectedObject = data;
            propertyGrid.ToolbarVisible = false;
            propertyGrid.HelpVisible = true;

            Controls.Add(propertyGrid);
            Controls.Add(titleLabel);
        }
    }
}
