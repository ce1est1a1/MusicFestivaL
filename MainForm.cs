using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MusicFestivalDeserializer
{
    public class MainForm : Form
    {
        private TreeView treeView;
        private DataGridView dataGridView;
        private Label statusLabel;
        private Button buttonLoadXml;
        private Button buttonLoadJson;
        private SplitContainer splitContainer;

        private FestivalData currentData;

        public MainForm()
        {
            InitializeForm();
            InitializeControls();
        }

        private void InitializeForm()
        {
            Text = "Десериализация: Мировой фестиваль";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1200;
            Height = 720;
            MinimumSize = new Size(980, 620);
            BackColor = Color.White;
        }

        private void InitializeControls()
        {
            Panel topPanel = CreateTopPanel();
            Panel leftPanel = CreateLeftPanel();
            Panel rightPanel = CreateRightPanel();

            splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.SplitterDistance = 320;
            splitContainer.BorderStyle = BorderStyle.FixedSingle;
            splitContainer.Panel1.Controls.Add(leftPanel);
            splitContainer.Panel2.Controls.Add(rightPanel);

            statusLabel = new Label();
            statusLabel.Dock = DockStyle.Bottom;
            statusLabel.Height = 30;
            statusLabel.TextAlign = ContentAlignment.MiddleLeft;
            statusLabel.Padding = new Padding(10, 0, 0, 0);
            statusLabel.BorderStyle = BorderStyle.FixedSingle;
            statusLabel.Text = "Загрузите XML или JSON файл.";

            Controls.Add(splitContainer);
            Controls.Add(statusLabel);
            Controls.Add(topPanel);
        }

        private Panel CreateTopPanel()
        {
            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 100;
            topPanel.Padding = new Padding(16, 14, 16, 14);
            topPanel.BackColor = Color.White;

            Panel buttonsPanel = new Panel();
            buttonsPanel.Dock = DockStyle.Right;
            buttonsPanel.Width = 430;

            buttonLoadJson = new Button();
            buttonLoadJson.Text = "Загрузить JSON";
            buttonLoadJson.Width = 190;
            buttonLoadJson.Height = 40;
            buttonLoadJson.Left = 10;
            buttonLoadJson.Top = 24;
            buttonLoadJson.Click += ButtonLoadJson_Click;

            buttonLoadXml = new Button();
            buttonLoadXml.Text = "Загрузить XML";
            buttonLoadXml.Width = 190;
            buttonLoadXml.Height = 40;
            buttonLoadXml.Left = 210;
            buttonLoadXml.Top = 24;
            buttonLoadXml.Click += ButtonLoadXml_Click;

            buttonsPanel.Controls.Add(buttonLoadJson);
            buttonsPanel.Controls.Add(buttonLoadXml);

            Panel titlePanel = new Panel();
            titlePanel.Dock = DockStyle.Fill;

            Label titleLabel = new Label();
            titleLabel.Dock = DockStyle.Fill;
            titleLabel.Text = "Мировой фестиваль";
            titleLabel.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.AutoEllipsis = true;

            titlePanel.Controls.Add(titleLabel);

            topPanel.Controls.Add(titlePanel);
            topPanel.Controls.Add(buttonsPanel);

            return topPanel;
        }

        private Panel CreateLeftPanel()
        {
            Panel leftPanel = new Panel();
            leftPanel.Dock = DockStyle.Fill;

            Label leftLabel = new Label();
            leftLabel.Text = "Разделы фестиваля";
            leftLabel.Dock = DockStyle.Top;
            leftLabel.Height = 35;
            leftLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            leftLabel.TextAlign = ContentAlignment.MiddleLeft;
            leftLabel.Padding = new Padding(8, 0, 0, 0);

            treeView = new TreeView();
            treeView.Dock = DockStyle.Fill;
            treeView.HideSelection = false;
            treeView.Font = new Font("Segoe UI", 10);
            treeView.FullRowSelect = true;
            treeView.AfterSelect += TreeView_AfterSelect;
            treeView.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;

            leftPanel.Controls.Add(treeView);
            leftPanel.Controls.Add(leftLabel);
            return leftPanel;
        }

        private Panel CreateRightPanel()
        {
            Panel rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Fill;

            Label rightLabel = new Label();
            rightLabel.Text = "Информация";
            rightLabel.Dock = DockStyle.Top;
            rightLabel.Height = 35;
            rightLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            rightLabel.TextAlign = ContentAlignment.MiddleLeft;
            rightLabel.Padding = new Padding(8, 0, 0, 0);

            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.AutoGenerateColumns = true;
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowHeadersVisible = false;
            dataGridView.BackgroundColor = Color.White;

            rightPanel.Controls.Add(dataGridView);
            rightPanel.Controls.Add(rightLabel);
            return rightPanel;
        }

        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ShowSelectedDetails();
        }

        private void ButtonLoadXml_Click(object sender, EventArgs e)
        {
            LoadFile("xml");
        }

        private void ButtonLoadJson_Click(object sender, EventArgs e)
        {
            LoadFile("json");
        }

        private string GetDataDirectoryPath()
        {
            DirectoryInfo currentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (currentDirectory != null)
            {
                string dataDirectoryPath = Path.Combine(currentDirectory.FullName, "Data");
                if (Directory.Exists(dataDirectoryPath))
                {
                    return dataDirectoryPath;
                }

                currentDirectory = currentDirectory.Parent;
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }


        private void LoadFile(string extension)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                if (extension == "xml")
                {
                    dialog.Title = "Выберите XML файл";
                    dialog.Filter = "XML files (*.xml)|*.xml";
                }
                else
                {
                    dialog.Title = "Выберите JSON файл";
                    dialog.Filter = "JSON files (*.json)|*.json";
                }

                string initialDirectory = GetDataDirectoryPath();
                if (Directory.Exists(initialDirectory))
                {
                    dialog.InitialDirectory = initialDirectory;
                }

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                FestivalData loadedData;
                if (extension == "xml")
                {
                    loadedData = DeserializerService.LoadXml(dialog.FileName);
                }
                else
                {
                    loadedData = DeserializerService.LoadJson(dialog.FileName);
                }

                currentData = loadedData;
                FillTree();
                statusLabel.Text = "Файл успешно загружен: " + Path.GetFileName(dialog.FileName);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка при загрузке файла:\n" + exception.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Не удалось загрузить файл.";
            }
            finally
            {
                dialog.Dispose();
            }
        }

        private void FillTree()
        {
            treeView.Nodes.Clear();
            dataGridView.DataSource = null;

            if (currentData == null)
            {
                return;
            }

            TreeNode rootNode = new TreeNode("Мировой фестиваль");

            TreeNode festivalNode = new TreeNode("Фестиваль");
            festivalNode.Tag = currentData.Festival;
            rootNode.Nodes.Add(festivalNode);

            TreeNode artistsNode = new TreeNode("Исполнители");
            artistsNode.Tag = currentData.Artists;
            AddArtistNodes(artistsNode, currentData.Artists);
            rootNode.Nodes.Add(artistsNode);

            TreeNode stagesNode = new TreeNode("Сцены");
            stagesNode.Tag = currentData.Stages;
            AddStageNodes(stagesNode, currentData.Stages);
            rootNode.Nodes.Add(stagesNode);

            treeView.Nodes.Add(rootNode);
            rootNode.ExpandAll();
            treeView.SelectedNode = festivalNode;
        }

        private void AddArtistNodes(TreeNode parentNode, List<Artist> artists)
        {
            int i;
            for (i = 0; i < artists.Count; i = i + 1)
            {
                Artist artist = artists[i];
                TreeNode artistNode = new TreeNode(artist.StageName);
                artistNode.Tag = artist;
                parentNode.Nodes.Add(artistNode);
            }
        }

        private void AddStageNodes(TreeNode parentNode, List<StageInfo> stages)
        {
            int i;
            for (i = 0; i < stages.Count; i = i + 1)
            {
                StageInfo stage = stages[i];
                TreeNode stageNode = new TreeNode(stage.Name);
                stageNode.Tag = stage;
                parentNode.Nodes.Add(stageNode);
            }
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            object selectedObject = e.Node.Tag;

            if (selectedObject == null)
            {
                dataGridView.DataSource = null;
                return;
            }

            List<Artist> artistList = selectedObject as List<Artist>;
            if (artistList != null)
            {
                dataGridView.DataSource = BuildArtistRows(artistList);
                return;
            }

            List<StageInfo> stageList = selectedObject as List<StageInfo>;
            if (stageList != null)
            {
                dataGridView.DataSource = BuildStageRows(stageList);
                return;
            }

            if (selectedObject is Festival || selectedObject is Artist || selectedObject is StageInfo)
            {
                dataGridView.DataSource = ObjectFlattener.Flatten(selectedObject);
                return;
            }

            dataGridView.DataSource = null;
        }

        private List<ArtistGridRow> BuildArtistRows(List<Artist> artists)
        {
            List<ArtistGridRow> rows = new List<ArtistGridRow>();
            int i;
            for (i = 0; i < artists.Count; i = i + 1)
            {
                Artist artist = artists[i];
                ArtistGridRow row = new ArtistGridRow();
                row.Artist = artist.StageName;
                row.Genre = artist.Genre;
                row.Country = artist.Country;
                row.Headliner = GetYesNoText(artist.Headliner);
                row.Day = artist.PerformanceDay;
                row.Stage = artist.Performance.Stage;
                row.Time = artist.Performance.Time;
                row.Set = artist.Performance.SpecialSet;
                rows.Add(row);
            }

            return rows;
        }

        private List<StageGridRow> BuildStageRows(List<StageInfo> stages)
        {
            List<StageGridRow> rows = new List<StageGridRow>();
            int i;
            for (i = 0; i < stages.Count; i = i + 1)
            {
                StageInfo stage = stages[i];
                StageGridRow row = new StageGridRow();
                row.Name = stage.Name;
                row.Capacity = stage.Capacity;
                row.OpenAir = GetYesNoText(stage.IsOpenAir);
                row.SoundSystem = stage.SoundSystem;
                row.AccessLevel = stage.AccessLevel;
                row.Start = stage.WorkTime.Start;
                row.End = stage.WorkTime.End;
                row.Manager = stage.Manager.FullName;
                rows.Add(row);
            }

            return rows;
        }

        private string GetYesNoText(bool value)
        {
            if (value)
            {
                return "Да";
            }

            return "Нет";
        }

        private void ShowSelectedDetails()
        {
            if (treeView.SelectedNode == null)
            {
                MessageBox.Show("Сначала выберите сущность в дереве.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            object selectedObject = treeView.SelectedNode.Tag;
            if (selectedObject == null)
            {
                MessageBox.Show("Сначала выберите сущность в дереве.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (selectedObject is List<Artist> || selectedObject is List<StageInfo>)
            {
                MessageBox.Show("Для подробного просмотра выберите конкретную сущность, а не весь список.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DetailsForm detailsForm = new DetailsForm(treeView.SelectedNode.Text, selectedObject);
            try
            {
                detailsForm.ShowDialog(this);
            }
            finally
            {
                detailsForm.Dispose();
            }
        }
    }

    public class ArtistGridRow
    {
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Country { get; set; }
        public string Headliner { get; set; }
        public string Day { get; set; }
        public string Stage { get; set; }
        public string Time { get; set; }
        public string Set { get; set; }
    }

    public class StageGridRow
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string OpenAir { get; set; }
        public string SoundSystem { get; set; }
        public string AccessLevel { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Manager { get; set; }
    }
}
