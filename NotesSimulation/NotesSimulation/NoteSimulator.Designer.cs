namespace NotesSimulation
{
    partial class NoteSimulator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.NoteDisplay = new System.Windows.Forms.TabPage();
            this.DebugNoteDisplay = new System.Windows.Forms.TabPage();
            this.pictureBoxFrequencyDomain = new System.Windows.Forms.PictureBox();
            this.pictureBoxTimeDomain = new System.Windows.Forms.PictureBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.comboBoxInputDevice = new System.Windows.Forms.ComboBox();
            this.buttonStop = new System.Windows.Forms.Button();
            this.Pitch = new System.Windows.Forms.Label();
            this.MIDI = new System.Windows.Forms.Label();
            this.Note = new System.Windows.Forms.Label();
            this.textBoxPitch = new System.Windows.Forms.TextBox();
            this.textBoxMIDI = new System.Windows.Forms.TextBox();
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.ABCNOTES = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.DebugNoteDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrequencyDomain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTimeDomain)).BeginInit();
            this.ABCNOTES.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.NoteDisplay);
            this.tabControl1.Controls.Add(this.DebugNoteDisplay);
            this.tabControl1.Controls.Add(this.ABCNOTES);
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.tabControl1.Location = new System.Drawing.Point(12, 57);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(620, 477);
            this.tabControl1.TabIndex = 0;
            // 
            // NoteDisplay
            // 
            this.NoteDisplay.Location = new System.Drawing.Point(4, 22);
            this.NoteDisplay.Name = "NoteDisplay";
            this.NoteDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.NoteDisplay.Size = new System.Drawing.Size(612, 451);
            this.NoteDisplay.TabIndex = 0;
            this.NoteDisplay.Text = "NoteDisplay";
            this.NoteDisplay.UseVisualStyleBackColor = true;
            // 
            // DebugNoteDisplay
            // 
            this.DebugNoteDisplay.Controls.Add(this.pictureBoxFrequencyDomain);
            this.DebugNoteDisplay.Controls.Add(this.pictureBoxTimeDomain);
            this.DebugNoteDisplay.Location = new System.Drawing.Point(4, 22);
            this.DebugNoteDisplay.Name = "DebugNoteDisplay";
            this.DebugNoteDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.DebugNoteDisplay.Size = new System.Drawing.Size(612, 451);
            this.DebugNoteDisplay.TabIndex = 1;
            this.DebugNoteDisplay.Text = "Debug NoteDisplay";
            this.DebugNoteDisplay.UseVisualStyleBackColor = true;
            // 
            // pictureBoxFrequencyDomain
            // 
            this.pictureBoxFrequencyDomain.BackColor = System.Drawing.Color.Black;
            this.pictureBoxFrequencyDomain.Location = new System.Drawing.Point(7, 225);
            this.pictureBoxFrequencyDomain.Name = "pictureBoxFrequencyDomain";
            this.pictureBoxFrequencyDomain.Size = new System.Drawing.Size(599, 220);
            this.pictureBoxFrequencyDomain.TabIndex = 1;
            this.pictureBoxFrequencyDomain.TabStop = false;
            // 
            // pictureBoxTimeDomain
            // 
            this.pictureBoxTimeDomain.BackColor = System.Drawing.Color.Black;
            this.pictureBoxTimeDomain.Location = new System.Drawing.Point(7, 9);
            this.pictureBoxTimeDomain.Name = "pictureBoxTimeDomain";
            this.pictureBoxTimeDomain.Size = new System.Drawing.Size(599, 218);
            this.pictureBoxTimeDomain.TabIndex = 0;
            this.pictureBoxTimeDomain.TabStop = false;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(334, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(121, 39);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start Listening";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // comboBoxInputDevice
            // 
            this.comboBoxInputDevice.FormattingEnabled = true;
            this.comboBoxInputDevice.Location = new System.Drawing.Point(16, 22);
            this.comboBoxInputDevice.Name = "comboBoxInputDevice";
            this.comboBoxInputDevice.Size = new System.Drawing.Size(303, 21);
            this.comboBoxInputDevice.TabIndex = 2;
            this.comboBoxInputDevice.SelectedIndexChanged += new System.EventHandler(this.comboBoxInputDevice_SelectedIndexChanged);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(462, 13);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(111, 38);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // Pitch
            // 
            this.Pitch.AutoSize = true;
            this.Pitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pitch.Location = new System.Drawing.Point(651, 79);
            this.Pitch.Name = "Pitch";
            this.Pitch.Size = new System.Drawing.Size(56, 24);
            this.Pitch.TabIndex = 5;
            this.Pitch.Text = "Pitch";
            // 
            // MIDI
            // 
            this.MIDI.AutoSize = true;
            this.MIDI.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MIDI.Location = new System.Drawing.Point(653, 174);
            this.MIDI.Name = "MIDI";
            this.MIDI.Size = new System.Drawing.Size(51, 24);
            this.MIDI.TabIndex = 6;
            this.MIDI.Text = "MIDI";
            // 
            // Note
            // 
            this.Note.AutoSize = true;
            this.Note.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Note.Location = new System.Drawing.Point(653, 282);
            this.Note.Name = "Note";
            this.Note.Size = new System.Drawing.Size(54, 24);
            this.Note.TabIndex = 7;
            this.Note.Text = "Note";
            // 
            // textBoxPitch
            // 
            this.textBoxPitch.Enabled = false;
            this.textBoxPitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPitch.ForeColor = System.Drawing.Color.Red;
            this.textBoxPitch.Location = new System.Drawing.Point(713, 88);
            this.textBoxPitch.Name = "textBoxPitch";
            this.textBoxPitch.Size = new System.Drawing.Size(153, 80);
            this.textBoxPitch.TabIndex = 0;
            // 
            // textBoxMIDI
            // 
            this.textBoxMIDI.Enabled = false;
            this.textBoxMIDI.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMIDI.Location = new System.Drawing.Point(713, 194);
            this.textBoxMIDI.Name = "textBoxMIDI";
            this.textBoxMIDI.Size = new System.Drawing.Size(153, 80);
            this.textBoxMIDI.TabIndex = 8;
            // 
            // textBoxNote
            // 
            this.textBoxNote.Enabled = false;
            this.textBoxNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNote.ForeColor = System.Drawing.Color.Blue;
            this.textBoxNote.Location = new System.Drawing.Point(713, 326);
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.Size = new System.Drawing.Size(153, 80);
            this.textBoxNote.TabIndex = 9;
            // 
            // ABCNOTES
            // 
            this.ABCNOTES.Controls.Add(this.panel1);
            this.ABCNOTES.Location = new System.Drawing.Point(4, 22);
            this.ABCNOTES.Name = "ABCNOTES";
            this.ABCNOTES.Size = new System.Drawing.Size(612, 451);
            this.ABCNOTES.TabIndex = 2;
            this.ABCNOTES.Text = "ABCNOTES";
            this.ABCNOTES.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(606, 448);
            this.panel1.TabIndex = 0;
            // 
            // NoteSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 563);
            this.Controls.Add(this.textBoxNote);
            this.Controls.Add(this.textBoxMIDI);
            this.Controls.Add(this.textBoxPitch);
            this.Controls.Add(this.Note);
            this.Controls.Add(this.MIDI);
            this.Controls.Add(this.Pitch);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.comboBoxInputDevice);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.tabControl1);
            this.Name = "NoteSimulator";
            this.Text = "NoteSimulator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabControl1.ResumeLayout(false);
            this.DebugNoteDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrequencyDomain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTimeDomain)).EndInit();
            this.ABCNOTES.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage NoteDisplay;
        private System.Windows.Forms.TabPage DebugNoteDisplay;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ComboBox comboBoxInputDevice;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label Pitch;
        private System.Windows.Forms.Label MIDI;
        private System.Windows.Forms.Label Note;
        private System.Windows.Forms.TextBox textBoxPitch;
        private System.Windows.Forms.TextBox textBoxMIDI;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.PictureBox pictureBoxFrequencyDomain;
        private System.Windows.Forms.PictureBox pictureBoxTimeDomain;
        private System.Windows.Forms.TabPage ABCNOTES;
        private System.Windows.Forms.Panel panel1;

    }
}

