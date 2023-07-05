using System;
using System.IO;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

public class ImpresoraTermica
{

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    public static void Main()
    {
        string nombreImpresora = "POS-58";
        string textoParaImprimir = "Hola impresora desde C#!";
        string hostname = System.Environment.MachineName;
        string ubicacionCompletaImpresora = string.Format("\\\\{0}\\{1}", System.Environment.MachineName, nombreImpresora);
        Console.WriteLine( ubicacionCompletaImpresora);
        SafeFileHandle fh = CreateFile(ubicacionCompletaImpresora, FileAccess.Write, 0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);
        if (fh.IsInvalid)
        {
            Console.WriteLine("Error abriendo impresora");
            return;
        }
        var impresoraComoArchivo = new FileStream(fh, FileAccess.ReadWrite);
        impresoraComoArchivo.WriteByte(0x1b); // ESC
        impresoraComoArchivo.WriteByte(0x40); // @
        impresoraComoArchivo.Write(Encoding.ASCII.GetBytes(textoParaImprimir), 0, textoParaImprimir.Length);
        impresoraComoArchivo.WriteByte(0x1b); // Feed
        impresoraComoArchivo.WriteByte(Convert.ToByte('d'));
        impresoraComoArchivo.WriteByte(Convert.ToByte(1)); // 1 línea
        impresoraComoArchivo.Dispose();
    }
}