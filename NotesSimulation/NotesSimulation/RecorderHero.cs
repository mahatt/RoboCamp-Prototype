using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using Notes;
using NotesIO;


namespace Recorder
{
    class RecorderHero : IDisposable
    {
        private NoteDrawer Drawer;

        private RecordingData AbcData;

        private Thread RecorderHeroThread;

        private Graphics NotesGraphics;


        private List<Color> NotesColors;

        private int CurrentNoteIndex;

        private int NotesPerPage;

        private int FirstNoteInPage;

        private bool m_isPlaying;

        public bool isPlaying
        {
            get { return m_isPlaying; }
        }

        private float m_sleepMultiplier = 1f;

        public float SleepMultiplier
        {
            get
            {
                return m_sleepMultiplier;
            }
            set
            {
                this.m_sleepMultiplier = value;
            }
        }

        private Color REGULAR_COLOR = Color.Black;
        private Color WRONG_COLOR = Color.FromArgb(240, 230, 53, 98);
        private Color RIGHT_COLOR = Color.FromArgb(240, 33, 250, 98);
        private Color CURRENT_COLOR = Color.FromArgb(240, 63, 149, 220);



        public RecorderHero(Graphics graphics, string abcFilePath)
        {
            Drawer = new NoteDrawer();

            // convert ABC file to notes
            AbcData = AbcParser.AbcToNotes(abcFilePath);

            // Creating a new music sheet, with first notes in it
            Restart(graphics);
        }

        public void Start()
        {
            m_isPlaying = true;
            RecorderHeroThread = new Thread(new ThreadStart(NotesIteration));
            RecorderHeroThread.Start();
        }

        public void Stop()
        {
            m_isPlaying = false;
            if (null != RecorderHeroThread)
            {
                RecorderHeroThread.Join();
            }
        }

        public void Dispose()
        {
            Stop();
        }

        public void UpdateCurrentNote(Note currentNote)
        {
            lock (this)
            {
                if (CurrentNoteIndex >= AbcData.Notes.Count)
                {
                    return;
                }
                if (currentNote.MIDI == AbcData.Notes[CurrentNoteIndex].MIDI)
                {
                    NotesColors[CurrentNoteIndex] = RIGHT_COLOR;

                    // Draw with current note as correct
                    int a = FirstNoteInPage;
                    int b = Math.Min(FirstNoteInPage + NotesPerPage, AbcData.Notes.Count)
                        - FirstNoteInPage;

                    NotesPerPage = Drawer.DrawNotes(NotesGraphics,
                        Color.White,
                        new SolidBrush(Color.Black),
                        AbcData.Notes.GetRange(a, b),
                        NotesColors.GetRange(a, b));
                }
            }
        }

        public void OnPaint(Graphics graphics)
        {
            lock (this)
            {
                NotesGraphics = graphics;

                // Create new Music Sheet - get number of notes in current music sheet
                NotesPerPage = Drawer.DrawNotes(graphics,
                    Color.White,
                    new SolidBrush(Color.Black),
                    null,
                    null);

                if (FirstNoteInPage + NotesPerPage <= CurrentNoteIndex)
                {
                    FirstNoteInPage = CurrentNoteIndex;
                }

                // give that number of notes
                int a = FirstNoteInPage;
                int b = Math.Min(FirstNoteInPage + NotesPerPage, AbcData.Notes.Count)
                    - FirstNoteInPage;

                NotesPerPage = Drawer.DrawNotes(graphics,
                    Color.White,
                    new SolidBrush(Color.Black),
                    AbcData.Notes.GetRange(a, b),
                    NotesColors.GetRange(a, b));
            }
        }

        public Note GetCurrentNote()
        {
            if (m_isPlaying && 0 <= CurrentNoteIndex && CurrentNoteIndex < AbcData.Notes.Count)
            {
                return AbcData.Notes[CurrentNoteIndex];
            }
            else
            {
                return null;
            }
        }

        private void Restart(Graphics graphics)
        {
            NotesColors = new List<Color>();
            FirstNoteInPage = 0;
            CurrentNoteIndex = 0;
            m_isPlaying = false;
            NotesGraphics = graphics;

            for (int i = 0; AbcData.Notes.Count > i; ++i)
            {
                NotesColors.Add(REGULAR_COLOR);
            }

            OnPaint(graphics);
        }

        private void NotesIteration()
        {
            while (m_isPlaying)
            {
                int CurrentNoteDuration = 0;

                lock (this)
                {
                    // Some kind of documentation
                    if (CurrentNoteIndex >= NotesColors.Count)
                    {
                        return;
                    }

                    NotesColors[CurrentNoteIndex] = CURRENT_COLOR;

                    // Check if it's the first note in the page
                    if (FirstNoteInPage == CurrentNoteIndex)
                    {
                        Drawer.Clear(NotesGraphics, Color.White, new SolidBrush(Color.Black));
                    }

                    // Draw notes
                    int a = FirstNoteInPage;
                    int b = Math.Min(FirstNoteInPage + NotesPerPage, AbcData.Notes.Count)
                        - FirstNoteInPage;

                    NotesPerPage = Drawer.DrawNotes(NotesGraphics,
                        Color.White,
                        new SolidBrush(Color.Black),
                        AbcData.Notes.GetRange(a, b),
                        NotesColors.GetRange(a, b));

                    CurrentNoteDuration = AbcData.Notes[CurrentNoteIndex].Duration;
                }

                Thread.Sleep((int)((float)CurrentNoteDuration * m_sleepMultiplier));

                lock (this)
                {
                    // Check if it's the last note in the page
                    if (FirstNoteInPage + NotesPerPage == CurrentNoteIndex)
                    {
                        FirstNoteInPage = CurrentNoteIndex + 1;
                    }

                    // If no right answer yet, color this note as wrong.
                    if (NotesColors[CurrentNoteIndex] != RIGHT_COLOR)
                    {
                        NotesColors[CurrentNoteIndex] = WRONG_COLOR;
                    }

                    // Advance note
                    ++CurrentNoteIndex;

                    // Check if the previous note was the last note
                    if (CurrentNoteIndex == AbcData.Notes.Count)
                    {
                        // Draw again
                        int a = FirstNoteInPage;
                        int b = Math.Min(FirstNoteInPage + NotesPerPage, AbcData.Notes.Count)
                            - FirstNoteInPage;

                        NotesPerPage = Drawer.DrawNotes(NotesGraphics,
                            Color.White,
                            new SolidBrush(Color.Black),
                            AbcData.Notes.GetRange(a, b),
                            NotesColors.GetRange(a, b));

                        // This was last note
                        m_isPlaying = false;
                        return;
                    }
                }
            }
        }
    }
}
