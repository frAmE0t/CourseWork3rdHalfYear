﻿namespace CourseWork3rdHalfYear.Forms
{
    public partial class LevelCreatorForm : Form
    {
        private int _columns = 0;
        private int _rows = 0;

        private int _windowWidth = 0;
        private int _flowLayoutPanelWith = 0;
        private int _flowLayoutPanelHeight = 0;

        private List<Control> _objectsToResize = null!;

        private bool _isBox = false;
        private bool _isWall = false;
        private bool _isPerson = false;
        private bool _isMark = false;
        private bool _isBroom = false;

        private byte _personAmount = 0;
        private int _boxesAmount = 0;
        private int _marksAmount = 0;

        private bool _isMapSaved = false;
        private int _savedMapNumber = 0;

        public LevelCreatorForm()
        {
            InitializeComponent();
            SetValuesBeforeFormLoad();
        }

        private void LevelCreatorForm_Resize(object sender, EventArgs e)
        {
            foreach (Control control in _objectsToResize)
                control.Location = new Point(control.Location.X + this.Width - _windowWidth, control.Location.Y);

            _windowWidth = this.Width;
        }

        private void FillFlowLayoutPanel()
        {
            for (int j = 1; j < _rows + 1; j++)
            {
                for (int k = 1; k < _columns + 1; k++)
                {
                    PictureBox picBox = new();

                    picBox.BorderStyle = BorderStyle.FixedSingle;
                    picBox.BackColor = Color.White;
                    picBox.Margin = new Padding(0);
                    picBox.Size = new Size(FlowLayoutPanel.Width / _columns, FlowLayoutPanel.Height / _rows);
                    picBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    picBox.Cursor = Cursors.Hand;
                    picBox.Name = "Empty";

                    FlowLayoutPanel.Controls.Add(picBox);
                }
            }

            foreach (Control control in FlowLayoutPanel.Controls)
                control.Click += new EventHandler(PutPictureInPicBoxOnClick!);
        }

        private void FlowLayoutPanel_Resize(object sender, EventArgs e)
        {
            if ((double)FlowLayoutPanel.Width >= (double)_flowLayoutPanelWith * 1.15 || (double)FlowLayoutPanel.Width * 1.15 <= (double)_flowLayoutPanelWith)
            {
                _flowLayoutPanelWith = FlowLayoutPanel.Width;

                if (FlowLayoutPanel.Controls.Count != 0)
                {
                    foreach (Control control in FlowLayoutPanel.Controls)
                        control.Size = new Size(FlowLayoutPanel.Width / _columns, _flowLayoutPanelWith / _rows);
                }
            }

            if ((double)FlowLayoutPanel.Height >= (double)_flowLayoutPanelHeight * 1.15 || (double)FlowLayoutPanel.Height * 1.15 <= (double)_flowLayoutPanelHeight)
            {
                _flowLayoutPanelHeight = FlowLayoutPanel.Height;

                if (FlowLayoutPanel.Controls.Count != 0)
                {
                    foreach (Control control in FlowLayoutPanel.Controls)
                        control.Size = new Size(_flowLayoutPanelWith / _columns, _flowLayoutPanelHeight / _rows);
                }
            }
        }

        private void PutPictureInPicBoxOnClick(object sender, EventArgs e)
        {
            string pathRecources = @"..\..\..\Resources\";
            PictureBox control = (sender as PictureBox)!;

            if (_isBox)
            {
                _boxesAmount++;

                control.Load(Path.Combine(pathRecources + "Box.png"));
                control.Name = "Box";
            }
            else if (_isMark)
            {
                _marksAmount++;

                control.Load(Path.Combine(pathRecources + "RedCross.png"));
                control.Name = "Mark";
            }
            else if (_isPerson)
            {
                if (_personAmount == 0)
                {
                    _personAmount++;

                    control.Load(Path.Combine(pathRecources + "Person.png"));
                    control.Name = "Person";
                }
                else
                    MessageBox.Show("На карте не может быть больше одного персонажа", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (_isWall)
            {
                control.Load(Path.Combine(pathRecources + "StoneBlock.jpg"));
                control.Name = "Wall";
            }
            else if (_isBroom)
            {
                control.Image = null;
                control.Name = "Empty";
            }
        }

        private void BoxPictureBox_Click(object sender, EventArgs e)
        {
            _isBox = true;

            _isWall = false;
            _isPerson = false;
            _isMark = false;
            _isBroom = false;
        }

        private void MarkPictureBox_Click(object sender, EventArgs e)
        {
            _isMark = true;

            _isBox = false;
            _isWall = false;
            _isPerson = false;
            _isBroom = false;
        }

        private void PersonPictureBox_Click(object sender, EventArgs e)
        {
            _isPerson = true;

            _isBox = false;
            _isWall = false;
            _isMark = false;
            _isBroom = false;
        }

        private void WallPictureBox_Click(object sender, EventArgs e)
        {
            _isWall = true;

            _isBox = false;
            _isPerson = false;
            _isMark = false;
            _isBroom = false;
        }

        private void BroomPictureBox_Click(object sender, EventArgs e)
        {
            _isBroom = true;

            _isWall = false;
            _isPerson = false;
            _isMark = false;
            _isBox = false;
        }

        private void InformationPictureBox_Click(object sender, EventArgs e)
        {
            LevelCreatorInformationForm levelCreatorInformationForm = new();
            levelCreatorInformationForm.ShowDialog();
        }

        private void StartOrSaveButton_Click(object sender, EventArgs e)
        {
            if (StartOrSaveButton.Text == "Начать")
            {
                StartOrSaveButton.Font = new Font("Segoe UI", 11);

                if (Int32.TryParse(ColumsTextBox.Text, out int colums) && Int32.TryParse(RowsTextBox.Text, out int rows))
                {
                    if (colums < 4 || rows < 4)
                    {
                        ColumsTextBox.Clear();
                        RowsTextBox.Clear();
                        MessageBox.Show("Количество рядов и столбцов не может быть меньше 4", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (colums > 25 || rows > 25)
                    {
                        ColumsTextBox.Clear();
                        RowsTextBox.Clear();
                        MessageBox.Show("Количество рядов и столбцов не может быть больше 25", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        StartOrSaveButton.Text = "Сохранить";

                        MapDimensionLabel.Hide();
                        ColumsLabel.Hide();
                        RowsLabel.Hide();
                        ColumsTextBox.Hide();
                        RowsTextBox.Hide();

                        _columns = colums;
                        _rows = rows;

                        ColumsTextBox.Clear();
                        RowsTextBox.Clear();
                        FillFlowLayoutPanel();
                    }
                }
                else
                {
                    ColumsTextBox.Clear();
                    RowsTextBox.Clear();
                    MessageBox.Show("Ввведите корректные числа", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (StartOrSaveButton.Text == "Сохранить")
            {
                if (_personAmount == 0)
                {
                    MessageBox.Show("Установите персонажа на карту", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (_boxesAmount != _marksAmount)
                {
                    MessageBox.Show("Количество ящиков не соответствует количеству меток", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string pathMaps = @"..\..\..\Maps\";

                if (_isMapSaved)
                {
                    DialogResult result = MessageBox.Show("Сохранить изменения в уже созданную карту?", "Уточнение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        pathMaps = Path.Combine(Path.Combine(pathMaps + $"map{_savedMapNumber}.txt"));
                        SaveMap(pathMaps);
                        return;
                    }
                }
                _isMapSaved = true;

                for (int i = 0; ; i++)
                {
                    if (!File.Exists(Path.Combine(pathMaps + $"map{i}.txt")))
                    {
                        _savedMapNumber = i;
                        pathMaps = Path.Combine(Path.Combine(pathMaps + $"map{i}.txt"));

                        SaveMap(pathMaps);

                        break;
                    }
                }
            }
        }

        private void BackToMenuFormPictureBox_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();

            MenuForm menuForm = new();

            menuForm.StartPosition = FormStartPosition.Manual;
            menuForm.Location = this.Location;
            menuForm.Size = this.Size;

            menuForm.ShowDialog();
        }

        private void SetValuesBeforeFormLoad()
        {
            _windowWidth = this.Width;

            _objectsToResize = new()
            {
                BackToMenuFormPictureBox, InformationPictureBox, StartOrSaveButton, MapDimensionLabel, RowsLabel, ColumsLabel, ColumsTextBox, RowsTextBox
            };

            _flowLayoutPanelWith = FlowLayoutPanel.Width;
            _flowLayoutPanelHeight = FlowLayoutPanel.Height;
        }

        private void SaveMap(string path)
        {
            char[,] map = new char[_rows + 2, _columns + 2];

            for (int j = 0; j < _columns + 2; j++)
            {
                map[0, j] = '#';
                map[_rows + 1, j] = '#';
            }

            for (int j = 1; j < _rows + 2; j++)
            {
                map[j, 0] = '#';
                map[j, _columns + 1] = '#';
            }

            for (int j = 1; j < _rows + 1; j++)
            {
                for (int k = 1; k < _columns + 1; k++)
                {
                    map[j, k] = FlowLayoutPanel.Controls[(j - 1) * _columns + (k - 1)].Name switch
                    {
                        "Box" => 'B',
                        "Person" => 'P',
                        "Wall" => '#',
                        "Mark" => 'X',
                        _ => '\0'
                    };
                }
            }

            using (StreamWriter textWriter = new(path))
            {
                for (int j = 0; j < _rows + 2; j++)
                {
                    for (int k = 0; k < _columns + 2; k++)
                    {
                        textWriter.Write(map[j, k]);
                    }
                    textWriter.Write('\n');
                }
            }
        }
    }
}
