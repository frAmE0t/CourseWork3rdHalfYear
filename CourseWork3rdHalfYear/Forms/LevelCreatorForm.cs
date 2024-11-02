﻿using System.Data;

namespace CourseWork3rdHalfYear.Forms
{
    public partial class LevelCreatorForm : Form
    {
        private int _columns = 0;
        private int _rows = 0;

        private int _windowWidth = 0;
        private int _flowLayoutPanelWith = 0;
        private int _flowLayoutPanelHeight = 0;

        private List<Control> _objectsToResize = null!;

        private DataTable _table = new();
        private Panel _panel = new();

        private bool _isBox = false;
        private bool _isWall = false;
        private bool _isPerson = false;
        private bool _isMark = false;

        private byte _personAmount = 0;

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
                    picBox.Size = new Size(flowLayoutPanel1.Width / _columns, flowLayoutPanel1.Height / _rows);
                    picBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    picBox.Cursor = Cursors.Hand;
                    picBox.Name = "Empty";

                    flowLayoutPanel1.Controls.Add(picBox);
                }
            }

            foreach (Control control in flowLayoutPanel1.Controls)
                control.Click += new EventHandler(PutPictureInPicBoxOnClick!);
        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            if ((double)flowLayoutPanel1.Width >= (double)_flowLayoutPanelWith * 1.15 || (double)flowLayoutPanel1.Width * 1.15 <= (double)_flowLayoutPanelWith)
            {
                _flowLayoutPanelWith = flowLayoutPanel1.Width;

                if (flowLayoutPanel1.Controls.Count != 0)
                {
                    foreach (Control control in flowLayoutPanel1.Controls)
                        control.Size = new Size(flowLayoutPanel1.Width / _columns, _flowLayoutPanelWith / _rows);
                }
            }

            if ((double)flowLayoutPanel1.Height >= (double)_flowLayoutPanelHeight * 1.15 || (double)flowLayoutPanel1.Height * 1.15 <= (double)_flowLayoutPanelHeight)
            {
                _flowLayoutPanelHeight = flowLayoutPanel1.Height;

                if (flowLayoutPanel1.Controls.Count != 0)
                {
                    foreach (Control control in flowLayoutPanel1.Controls)
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
                control.Load(Path.Combine(pathRecources + "Box.png"));
                control.Name = "Box";
            }
            else if (_isMark)
            {
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
        }

        private void pictureBoxBox_Click(object sender, EventArgs e)
        {
            _isBox = true;

            _isWall = false;
            _isPerson = false;
            _isMark = false;
        }

        private void pictureBoxMark_Click(object sender, EventArgs e)
        {
            _isMark = true;

            _isBox = false;
            _isWall = false;
            _isPerson = false;
        }

        private void pictureBoxPerson_Click(object sender, EventArgs e)
        {
            _isPerson = true;

            _isBox = false;
            _isWall = false;
            _isMark = false;
        }

        private void pictureBoxWall_Click(object sender, EventArgs e)
        {
            _isWall = true;

            _isBox = false;
            _isPerson = false;
            _isMark = false;
        }

        private void pictureBoxInformation_Click(object sender, EventArgs e)
        {
            LevelCreatorInformationForm levelCreatorInformationForm = new();
            levelCreatorInformationForm.ShowDialog();
        }

        private void buttonStartOrSave_Click(object sender, EventArgs e)
        {
            if (buttonStartOrSave.Text == "Начать")
            {
                buttonStartOrSave.Font = new Font("Segoe UI", 11);

                if (Int32.TryParse(textBoxColums.Text, out int colums) && Int32.TryParse(textBoxRows.Text, out int rows))
                {
                    if (colums < 4 || rows < 4)
                    {
                        textBoxColums.Clear();
                        textBoxRows.Clear();
                        MessageBox.Show("Количество рядов и столбцов не может быть меньше 4", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (colums > 25 || rows > 25)
                    {
                        textBoxColums.Clear();
                        textBoxRows.Clear();
                        MessageBox.Show("Количество рядов и столбцов не может быть больше 25", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        buttonStartOrSave.Text = "Сохранить";

                        _columns = colums;
                        _rows = rows;

                        textBoxColums.Clear();
                        textBoxRows.Clear();
                        FillFlowLayoutPanel();
                    }
                }
                else
                {
                    textBoxColums.Clear();
                    textBoxRows.Clear();
                    MessageBox.Show("Ввведите корректные числа", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (buttonStartOrSave.Text == "Сохранить")
            {
                if (_personAmount == 0)
                {
                    MessageBox.Show("Установите персонажа на карту", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                buttonStartOrSave.Font = new Font("Segoe UI", 12);

                string pathMaps = @"..\..\..\Maps\";

                for (int i = 0; ; i++)
                {
                    if (!File.Exists(Path.Combine(pathMaps + $"map{i}.txt")))
                    {
                        pathMaps = Path.Combine(Path.Combine(pathMaps + $"map{i}.txt"));
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
                                map[j, k] = flowLayoutPanel1.Controls[(j - 1) * _columns + (k - 1)].Name switch
                                {
                                    "Box" => 'B',
                                    "Person" => 'P',
                                    "Wall" => '#',
                                    "Mark" => 'X',
                                    _ => '\0'
                                };
                            }
                        }

                        using (StreamWriter textWriter = new(pathMaps))
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

                        break;
                    }
                }
            }
        }

        private void pictureBoxBackToMenuForm_Click(object sender, EventArgs e)
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
                pictureBoxBackToMenuForm, pictureBoxInformation, buttonStartOrSave, labelMapDimension, labelRows, labelColums, textBoxColums, textBoxRows
            };

            _flowLayoutPanelWith = flowLayoutPanel1.Width;
            _flowLayoutPanelHeight = flowLayoutPanel1.Height;
        }
    }
}
