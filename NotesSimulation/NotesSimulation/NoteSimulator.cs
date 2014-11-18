using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Notes;
using NotesIO;
using Wave;
using Analysis;
using  Delegates;
using Recorder;

namespace NotesSimulation
{
    public partial class NoteSimulator : Form
    {


        const uint NumberOfSamples = 256;
        const uint SamplesPerSec = 1024;
        const uint RecorderMaxFrequency = 1024;
 

        WaveIn wave;
        byte[] waveArray;
        double[] waveArrayFrequency;
        FourierTransform fourierTransform;

        // domains graphics
        Graphics timeDomainGraphics;
        Graphics frequencyDomainGraphics;
        bool printGraphsEnabled;

        // notes drawing
        Graphics MusicSheetGraphics;
        NoteDrawer noteDrawer;

        // notes
        List<Note> NotesPlayed;
        NotesDB notesDB;
        NotesDetector notesDetector;

        // recorder Hero
        RecorderHero recorderHero;

        // delegates
        Delegates.Delegates DelegatesUI;

        Thread recordingLoopThread;

        bool isRecording;


        // files
        string tempWaveFilePath;

        public NoteSimulator()
        {
            try
            {
                InitializeComponent();
                /*Detecting Input devices*/
                comboBoxInputDevice.Enabled = true;
                /*Working with actions*/
                buttonStart.Enabled = false;
                isRecording = false;


                notesDB = new NotesDB();

                wave = new WaveIn(NumberOfSamples, SamplesPerSec);
                waveArray = new byte[NumberOfSamples];
                waveArrayFrequency = new double[NumberOfSamples / 2 + 1];
                fourierTransform = new FourierTransform(NumberOfSamples);

                timeDomainGraphics = pictureBoxTimeDomain.CreateGraphics();
                frequencyDomainGraphics = pictureBoxFrequencyDomain.CreateGraphics();                 
                MusicSheetGraphics = NoteDisplay.CreateGraphics();
                
                // notes drawing
                noteDrawer = new NoteDrawer();
                notesDetector = new NotesDetector(); 
                // notes
                NotesPlayed = new List<Note>();
                DelegatesUI = new Delegates.Delegates();

                // Load microphone devices list
                comboBoxInputDevice.Items.Add("Choose Input Device");
                comboBoxInputDevice.SelectedIndex = 0;

                foreach (string deviceName in wave.getDevices())
                {
                    comboBoxInputDevice.Items.Add(deviceName);
                }

                /*Load Input File*/


            }
            catch (Exception ex)
            {
                string error_message;

                if (ex is WaveInException)
                {
                    error_message = "Recording Error: " + ex.Message + "Please connect a microphone and run the program again.";
                }
                else
                {
                    error_message = "Unknown exception: Window initialization: " + ex.Message;
                }

                MessageBox.Show(error_message, "Prototype Labs");
                
            }

        }

        /* Start sampling for Input*/

        private void RecordingLoop()
        {
            Note prevNote = null;
            Pen pen = new Pen(Color.Orange);
            Brush brush = new SolidBrush(Color.Black);


            while (isRecording)
            {
               // open the waveform-audio input device, and start recording
                wave.recordBuffer(waveArray, 60);
                // perform a frequney analysis on the sound wave
                float maxPitch = (float)fourierTransform.performTranform(FourierTransform.TRANSFORM.FFT, waveArray, waveArrayFrequency, NumberOfSamples, SamplesPerSec);

                // get the current note based on the frequency analysis
                Note currentNote = notesDB.FindNote(maxPitch);
                if (!isRecording) { return; }

                /*
                if (printGraphsEnabled)
                {
                    // print time domain
                    int[] tmpArray = new int[waveArray.Length];
                    for (int i = 0; i <waveArray.Length; i++) { tmpArray[i] = (int)waveArray[i]; }
                    WaveGraphics.DrawGraph(timeDomainGraphics, tmpArray, byte.MaxValue, Color.FromArgb(39, 40, 34), pen, tmpArray.Length / 4);
                    if (!isRecording) { return; }

                    // print fequency domain
                    Array.Resize(ref tmpArray, waveArrayFrequency.Length);
                    for (int i = 0; i < waveArrayFrequency.Length; i++) { tmpArray[i] = (int)waveArrayFrequency[i]; }
                    WaveGraphics.DrawGraph(frequencyDomainGraphics, tmpArray, 1800, Color.FromArgb(39, 40, 34), pen,
                        (int)((double)RecorderMaxFrequency * (double)NumberOfSamples / (double)SamplesPerSec));
                    if (!isRecording) { return; }
                }

                 */
                 

                DelegatesUI.SetControlText(this,textBoxPitch, maxPitch.ToString());
                DelegatesUI.SetControlText(this, textBoxNote, currentNote.NoteDescription);
                DelegatesUI.SetControlText(this, textBoxMIDI, currentNote.MIDI.ToString());

                                    // Detect the note based on the last and current samples
               Note detectedNote = notesDetector.DetectNote(currentNote, Environment.TickCount);

                    if (null != detectedNote)
                    {
                        if (prevNote == null || prevNote.MIDI != detectedNote.MIDI)
                        {
                            // Draw note in interactive music sheet
                            if (72 <= detectedNote.MIDI && detectedNote.MIDI <= 99)
                            {
                                NotesPlayed.Add(detectedNote);

                                noteDrawer.Draw(MusicSheetGraphics, Color.White, brush, brush, detectedNote);
                            }
                        }
                        else if (prevNote != null && prevNote.MIDI == detectedNote.MIDI)
                        {
                            if (72 <= detectedNote.MIDI && detectedNote.MIDI <= 99)
                            {
                                // a new instance of the same note
                                if (detectedNote.Duration < prevNote.Duration)
                                {
                                    NotesPlayed.Add(detectedNote);

                                    noteDrawer.Draw(MusicSheetGraphics, Color.White, brush, brush, detectedNote);
                                }
                            }

                            // redraw note
                            //noteDrawer.RedrawNote(MusicSheetGraphics, brush, detectedNote, -1);
                        }
                    }
                    prevNote = detectedNote;

                    System.GC.Collect();
                
            
            }
        }




        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStop.Enabled = true;
            buttonStart.Enabled = false;
            isRecording = true;

            //remove temp file, and create a new one instead
            /*if (null != tempWaveFilePath)
            {
                File.Delete(tempWaveFilePath);
            }

            */
            // open the waveform-audio input device, and start recording
            tempWaveFilePath = Path.GetTempFileName() + "Record.wav";
            wave.startDevice((uint)(comboBoxInputDevice.SelectedIndex - 1), tempWaveFilePath);


            ThreadStart recordingLoopThreadStart = new ThreadStart(RecordingLoop);
            recordingLoopThread = new Thread(recordingLoopThreadStart);
            recordingLoopThread.Start();

        }

        private void comboBoxInputDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxInputDevice.SelectedIndex == 0)
            {
                buttonStart.Enabled = false;
                buttonStop.Enabled = false;
            }
            else
            {
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;
            }

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            isRecording = false;
            buttonStop.Enabled = false;
            buttonStart.Enabled = true;

        }

        private void panelMusicSheet_Paint(object sender, PaintEventArgs e)
        {
            MusicSheetGraphics = NoteDisplay.CreateGraphics();
            SolidBrush brush = new SolidBrush(Color.Black);
            noteDrawer.Draw(MusicSheetGraphics, Color.White, brush, brush);

        }

        private void pictureBoxTimeDomain_Paint(object sender, PaintEventArgs e)
        {
            timeDomainGraphics = pictureBoxTimeDomain.CreateGraphics();
        }

        private void pictureBoxFrequencyDomain_Paint(object sender, PaintEventArgs e)
        {
            frequencyDomainGraphics = pictureBoxFrequencyDomain.CreateGraphics();
        }

        private void ABCNOTES_Paint(object sender, PaintEventArgs e)
        {
            if (null != recorderHero)
            {
                recorderHero.OnPaint(ABCNOTES.panel1.CreateGraphics());
            }
            else
            {
                splitContainerRecorderHero.Panel1.BackColor = Color.WhiteSmoke;
                Graphics graphics = splitContainerRecorderHero.Panel1.CreateGraphics();
                RectangleF graphicsSize = graphics.VisibleClipBounds;
                Size imageSize = Recorder.Properties.Resources.BalanceTheStraw.Size;

                //graphics.DrawImage(Recorder.Properties.Resources.BalanceTheStraw,
                //    (graphicsSize.Width - imageSize.Width) / 2,
                //    (graphicsSize.Height - imageSize.Height) / 2);

                string recorderHeroUsage = "Open an ABC music notation file,\n and start playing the recorder!";
                Font font = new Font("Gisha", 24);
                graphics.DrawString(recorderHeroUsage,
                    font,
                    new SolidBrush(Color.Black),
                    new PointF(graphicsSize.X + graphicsSize.Width / 4,
                        graphicsSize.Y + graphicsSize.Height / 4));
            }
        }
 

 /****************************************************************************/
        private void openABCfile()
        {
            // open a new ABC file, convert to Note objects, and display it in a new tab

            recorderHero = new RecorderHero(ABCNOTES.CreateGraphics(), @"E:\twinkle.abc");
            recorderHero.SleepMultiplier = GetTrackBarSleepMultiplier();

        }

        private float GetTrackBarSleepMultiplier()
        {
            float precent = 50;

            if (precent <= 50)
            {
                return (-9F / 50F) * precent + 10F;
            }
            else
            {
                return (-9F / 500F) * precent + 1.9F;
            }
        }
    }
}
