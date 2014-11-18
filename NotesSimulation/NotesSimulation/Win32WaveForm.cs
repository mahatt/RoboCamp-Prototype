using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace Win32
{
    public struct WAVEHDR
    {
        public IntPtr lpData;
        public int dwBufferLength;
        public int dwBytesRecorded;
        public int dwUser;
        public int dwFlags;
        public int dwLoops;
        public int lpNext;
        public int Reserved;
    }

    public struct WAVEINCAPS
    {
        public short wMid;
        public short wPid;
        public int vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WinMM.MAXPNAMELEN)]
        public string szPname;
        public int dwFormats;
        public short wChannels;
    }

    public struct WAVEFORMATEX
    {
        public ushort wFormatTag;
        public ushort nChannels;
        public uint nSamplesPerSec;
        public uint nAvgBytesPerSec;
        public ushort nBlockAlign;
        public ushort wBitsPerSample;
        public ushort cbSize;
    }

    public abstract class WinMM
    {
        [DllImport("winmm")]
        public static extern int waveInGetNumDevs();
        [DllImport("winmm")]
        public static extern int waveInGetDevCaps(int uDeviceID, ref WAVEINCAPS lpCaps, int uSize);
        [DllImport("winmm")]
        public static extern int waveInGetErrorText(int err, string lpText, int uSize);
        [DllImport("winmm")]
        public static extern int waveInOpen(ref IntPtr lphWaveIn, uint DEVICEID, ref WAVEFORMATEX lpWaveFormat, uint dwCallback, uint dwInstance, uint dwFlags);
        [DllImport("winmm")]
        public static extern int waveInStart(IntPtr hWaveIn);
        [DllImport("winmm")]
        public static extern int waveInPrepareHeader(IntPtr hWaveIn, ref WAVEHDR lpWaveInHdr, int uSize);
        [DllImport("winmm")]
        public static extern int waveInAddBuffer(IntPtr hWaveIn, ref WAVEHDR lpWaveInHdr, int uSize);
        [DllImport("winmm")]
        public static extern int waveInStop(IntPtr hWaveIn);
        [DllImport("winmm")]
        public static extern int waveInReset(IntPtr hWaveIn);
        [DllImport("winmm")]
        public static extern int waveInClose(IntPtr hWaveIn);
        [DllImport("winmm")]
        public static extern int waveInUnprepareHeader(IntPtr hWaveIn, ref WAVEHDR lpWaveInHdr, int uSize);


        public const int MAXPNAMELEN = 32;
        public const int MMSYSERR_NOERROR = 0;
        public const int CALLBACK_NULL = 0x0;
        public const int WAVE_FORMAT_PCM = 1;
        public const int WHDR_DONE = 0x1;

        /* taken from PInvokeLibrary */

        // Can be used instead of a device id to open a device
        public const uint WAVE_MAPPER = unchecked((uint)(-1));

        // Flag specifying the use of a callback window for sound messages
        public const uint CALLBACK_WINDOW = 0x10000;

        // Error information...
        private const int WAVERR_BASE = 32;
        private const int MMSYSERR_BASE = 0;

        // Enum equivalent to MMSYSERR_*
        public enum MMSYSERR : int
        {
            NOERROR = 0,
            ERROR = (MMSYSERR_BASE + 1),
            BADDEVICEID = (MMSYSERR_BASE + 2),
            NOTENABLED = (MMSYSERR_BASE + 3),
            ALLOCATED = (MMSYSERR_BASE + 4),
            INVALHANDLE = (MMSYSERR_BASE + 5),
            NODRIVER = (MMSYSERR_BASE + 6),
            NOMEM = (MMSYSERR_BASE + 7),
            NOTSUPPORTED = (MMSYSERR_BASE + 8),
            BADERRNUM = (MMSYSERR_BASE + 9),
            INVALFLAG = (MMSYSERR_BASE + 10),
            INVALPARAM = (MMSYSERR_BASE + 11),
            HANDLEBUSY = (MMSYSERR_BASE + 12),
            INVALIDALIAS = (MMSYSERR_BASE + 13),
            BADDB = (MMSYSERR_BASE + 14),
            KEYNOTFOUND = (MMSYSERR_BASE + 15),
            READERROR = (MMSYSERR_BASE + 16),
            WRITEERROR = (MMSYSERR_BASE + 17),
            DELETEERROR = (MMSYSERR_BASE + 18),
            VALNOTFOUND = (MMSYSERR_BASE + 19),
            NODRIVERCB = (MMSYSERR_BASE + 20),
            LASTERROR = (MMSYSERR_BASE + 20)
        }

        // Enum equivalent to WAVERR_*
        public enum WAVERR : int
        {
            NONE = 0,
            BADFORMAT = WAVERR_BASE + 0,
            STILLPLAYING = WAVERR_BASE + 1,
            UNPREPARED = WAVERR_BASE + 2,
            SYNC = WAVERR_BASE + 3,
            LASTERROR = WAVERR_BASE + 3
        }

        public const uint WAVE_INVALIDFORMAT = 0x00000000;
        public const uint WAVE_FORMAT_1M08 = 0x00000001;
        public const uint WAVE_FORMAT_1S08 = 0x00000002;
        public const uint WAVE_FORMAT_1M16 = 0x00000004;
        public const uint WAVE_FORMAT_1S16 = 0x00000008;
        public const uint WAVE_FORMAT_2M08 = 0x00000010;
        public const uint WAVE_FORMAT_2S08 = 0x00000020;
        public const uint WAVE_FORMAT_2M16 = 0x00000040;
        public const uint WAVE_FORMAT_2S16 = 0x00000080;
        public const uint WAVE_FORMAT_4M08 = 0x00000100;
        public const uint WAVE_FORMAT_4S08 = 0x00000200;
        public const uint WAVE_FORMAT_4M16 = 0x00000400;
        public const uint WAVE_FORMAT_4S16 = 0x00000800;
    }
}

