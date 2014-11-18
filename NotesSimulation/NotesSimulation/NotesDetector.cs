using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notes
{
    class NotesDetector
    {
        private const int minimalNoteTime = 70;
        private const int maximalNoteTime = 1000;
        private const int notesSamplesLimit = 10;
        private LimitedList<Note> playedNotes;
        private LimitedList<int> playedNotesTimes;

        public NotesDetector()
        {
            playedNotes = new LimitedList<Note>(notesSamplesLimit);
            playedNotesTimes = new LimitedList<int>(notesSamplesLimit);
        }

        public Note DetectNote(Note note, int timePlayed)
        {
            playedNotes.AddAtStart(note);
            playedNotesTimes.AddAtStart(timePlayed);

            if (playedNotes.Count < playedNotes.Limit)
            {
                return null;
            }

            int duration = 0;
            for (int i = playedNotes.Count - 1; i >= 0; i--)
            {
                Note oldNote = playedNotes[i];
                int oldTime = playedNotesTimes[i];

                if (note.MIDI == oldNote.MIDI)
                {
                    duration = timePlayed - oldTime;
                }
                else
                {
                    if (duration < minimalNoteTime)
                    {
                        duration = 0;
                    }
                    break;
                }
            }

            // if duration reached max, clear buffer
            if (maximalNoteTime < duration)
            {
                playedNotes.Clear();
                playedNotesTimes.Clear();
            }

            // add note
            if (0 < duration)
            {
                note.Duration = duration;
                return note;
            }
            else
            {
                return null;
            }
        }
    }

    public class LimitedList<T> : List<T>
    {
        private int limit = -1;

        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        public LimitedList(int limit)
            : base(limit)
        {
            this.Limit = limit;
        }

        public void AddAtStart(T item)
        {

            lock (this)
            {
                while (this.Count >= this.Limit)
                {
                    this.RemoveAt(0);
                }
            }

            base.Add(item);
        }
    }
}
