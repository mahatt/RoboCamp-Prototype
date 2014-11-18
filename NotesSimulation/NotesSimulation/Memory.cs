using System;
using System.Runtime.InteropServices;

namespace PInvokeLibrary
{
    public class Memory
    {
        public const uint LMEM_FIXED = 0x0000;
        public const uint LMEM_ZEROINIT = 0x0040;
        public const uint LMEM_MOVEABLE = 0x0002;
        public const uint LMEM_MODIFY = 0x0080;

        [DllImport("Kernel32.dll")]
        extern public static IntPtr LocalAlloc(uint uFlags, uint uBytes);

        [DllImport("Kernel32.dll")]
        extern public static IntPtr LocalFree(IntPtr hMem);

        [DllImport("Kernel32.dll")]
        extern public static IntPtr LocalReAlloc(IntPtr hMem, uint uBytes, uint fuFlags);
    }
}
