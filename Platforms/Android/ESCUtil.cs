using Java.IO;
using System;
using System.Text;
using Android.Graphics;
using XamarinESCUtils.Platforms.Common;
using Console = System.Console;

namespace XamarinESCUtils.Platforms.Android
{
    public class EscUtil : IEscUtil
    {
        public static readonly byte Esc = 0x1B;// Escape
        public static readonly byte Fs = 0x1C;// Text delimiter
        public static readonly byte Gs = 0x1D;// Group separator
        public static readonly byte Dle = 0x10;// data link escape
        public static readonly byte Eot = 0x04;// End of transmission
        public static readonly byte Enq = 0x05;// Enquiry character
        public static readonly byte Sp = 0x20;// Spaces
        public static readonly byte Ht = 0x09;// Horizontal list
        public static readonly byte Lf = 0x0A;//Print and wrap (horizontal orientation)
        public static readonly byte Cr = 0x0D;// Home key
        public static readonly byte Ff = 0x0C;// Carriage control (print and return to the standard mode (in page mode))
        public static readonly byte Can = 0x18;// Canceled (cancel print data in page mode)

        byte[] IEscUtil.InitPrinter()
        {
            byte[] result = new byte[2];
            result[0] = Esc;
            result[1] = 0x40;
            return result;
        }

        byte[] IEscUtil.SetPrinterDarkness(int value)
        {
            byte[] result = new byte[9];
            result[0] = Gs;
            result[1] = 40;
            result[2] = 69;
            result[3] = 4;
            result[4] = 0;
            result[5] = 5;
            result[6] = 5;
            result[7] = (byte)(value >> 8);
            result[8] = (byte)value;
            return result;
        }

        byte[] IEscUtil.GetPrintQrCode(String code, int modulesize, int errorlevel)
        {
            ByteArrayOutputStream buffer = new ByteArrayOutputStream();
            try
            {
                buffer.Write(SetQrCodeSize(modulesize));
                buffer.Write(SetQrCodeErrorLevel(errorlevel));
                buffer.Write(GetQCodeBytes(code));
                buffer.Write(GetBytesForPrintQrCode(true));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return buffer.ToByteArray();
        }

      
        byte[] IEscUtil.GetPrintDoubleQrCode(String code1, String code2, int modulesize, int errorlevel)
        {
            ByteArrayOutputStream buffer = new ByteArrayOutputStream();
            try
            {
                buffer.Write(SetQrCodeSize(modulesize));
                buffer.Write(SetQrCodeErrorLevel(errorlevel));
                buffer.Write(GetQCodeBytes(code1));
                buffer.Write(GetBytesForPrintQrCode(false));
                buffer.Write(GetQCodeBytes(code2));

                buffer.Write(new byte[] { 0x1B, 0x5C, 0x18, 0x00 });

                buffer.Write(GetBytesForPrintQrCode(true));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return buffer.ToByteArray();
        }

        byte[] IEscUtil.GetPrintQrCode2(String data, int size)
        {
            byte[] bytes1 = new byte[4];
            bytes1[0] = Gs;
            bytes1[1] = 0x76;
            bytes1[2] = 0x30;
            bytes1[3] = 0x00;

            //byte[] bytes2 = BytesUtil.getZXingQRCode(data, size);
            return BytesUtil.ByteMerger(bytes1, null);
        }

        byte[] IEscUtil.GetPrintBarcode(String data, int symbology, int height, int width, int textposition)
        {
            if (symbology < 0 || symbology > 10)
            {
                return new byte[] { Lf };
            }

            if (width is < 2 or > 6)
            {
                width = 2;
            }

            if (textposition is < 0 or > 3)
            {
                textposition = 0;
            }

            if (height is < 1 or > 255)
            {
                height = 162;
            }

            ByteArrayOutputStream buffer = new ByteArrayOutputStream();
            try
            {
                buffer.Write(new byte[]{0x1D,0x66,0x01,0x1D,0x48,(byte)textposition,
                    0x1D,0x77,(byte)width,0x1D,0x68,(byte)height,0x0A});

                byte[] barcode;
                barcode = symbology == 10 ? BytesUtil.GetBytesFromDecString(data) : Encoding.UTF8.GetBytes(data);


                buffer.Write(symbology > 7
                    ? new byte[] { 0x1D, 0x6B, 0x49, (byte)(barcode.Length + 2), 0x7B, (byte)(0x41 + symbology - 8) }
                    : new byte[] { 0x1D, 0x6B, (byte)(symbology + 0x41), (byte)barcode.Length });
                buffer.Write(barcode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return buffer.ToByteArray();
        }

        public static byte[] PrintBitmap(Bitmap bitmap, int mode)
        {
            var bytes1 = new byte[4];
            bytes1[0] = Gs;
            bytes1[1] = 0x76;
            bytes1[2] = 0x30;
            bytes1[3] = (byte)mode;

            var bytes2 = BytesUtil.GetBytesFromBitMap(bitmap);
            return BytesUtil.ByteMerger(bytes1, bytes2);
        }

        public static byte[] PrintBitmap(Bitmap bitmap)
        {
            var bytes1 = new byte[4];
            bytes1[0] = Gs;
            bytes1[1] = 0x76;
            bytes1[2] = 0x30;
            bytes1[3] = 0x00;

            var bytes2 = BytesUtil.GetBytesFromBitMap(bitmap);
            return BytesUtil.ByteMerger(bytes1, bytes2);
        }

        byte[] IEscUtil.PrintBitmap(byte[] bytes)
        {
            byte[] bytes1 = new byte[4];
            bytes1[0] = Gs;
            bytes1[1] = 0x76;
            bytes1[2] = 0x30;
            bytes1[3] = 0x00;

            return BytesUtil.ByteMerger(bytes1, bytes);
        }

        byte[] IEscUtil.NextLine(int lineNum)
        {
            var result = new byte[lineNum];
            for (int i = 0; i < lineNum; i++)
            {
                result[i] = Lf;
            }

            return result;
        }

        byte[] IEscUtil.underlineWithOneDotWidthOn()
        {
            var result = new byte[3];
            result[0] = Esc;
            result[1] = 45;
            result[2] = 1;
            return result;
        }

        byte[] IEscUtil.underlineWithTwoDotWidthOn()
        {
            byte[] result = new byte[3];
            result[0] = Esc;
            result[1] = 45;
            result[2] = 2;
            return result;
        }

        byte[] IEscUtil.UnderlineOff()
        {
            byte[] result = new byte[3];
            result[0] = Esc;
            result[1] = 45;
            result[2] = 0;
            return result;
        }

     
        byte[] IEscUtil.BoldOn()
        {
            byte[] result = new byte[3];
            result[0] = Esc;
            result[1] = 69;
            result[2] = 0xF;
            return result;
        }

        byte[] IEscUtil.BoldOff()
        {
            byte[] result = new byte[3];
            result[0] = Esc;
            result[1] = 69;
            result[2] = 0;
            return result;
        }
      
        byte[] IEscUtil.SingleByte()
        {
            byte[] result = new byte[2];
            result[0] = Fs;
            result[1] = 0x2E;
            return result;
        }

        byte[] IEscUtil.SingleByteOff()
        {
            byte[] result = new byte[2];
            result[0] = Fs;
            result[1] = 0x26;
            return result;
        }

        byte[] IEscUtil.SetCodeSystemSingle(byte charset)
        {
            byte[] result = new byte[3];
            result[0] = Esc;
            result[1] = 0x74;
            result[2] = charset;
            return result;
        }

        byte[] IEscUtil.SetCodeSystem(byte charset)
        {
            byte[] result = new byte[3];
            result[0] = Fs;
            result[1] = 0x43;
            result[2] = charset;
            return result;
        }

        byte[] IEscUtil.AlignLeft()
        {
            byte[] result = new byte[3];
            result[0] = Esc;
            result[1] = 97;
            result[2] = 0;
            return result;
        }

        byte[] IEscUtil.AlignCenter()
        {
            byte[] result = new byte[3];
            result[0] = Esc;
            result[1] = 97;
            result[2] = 1;
            return result;
        }

    
        byte[] IEscUtil.AlignRight()
        {
            byte[] result = new byte[3];
            result[0] = Esc;
            result[1] = 97;
            result[2] = 2;
            return result;
        }

        private static byte[] SetQrCodeSize(int modulesize)
        {
            byte[] dtmp = new byte[8];
            dtmp[0] = Gs;
            dtmp[1] = 0x28;
            dtmp[2] = 0x6B;
            dtmp[3] = 0x03;
            dtmp[4] = 0x00;
            dtmp[5] = 0x31;
            dtmp[6] = 0x43;
            dtmp[7] = (byte)modulesize;
            return dtmp;
        }

        private static byte[] SetQrCodeErrorLevel(int errorlevel)
        {
            byte[] dtmp = new byte[8];
            dtmp[0] = Gs;
            dtmp[1] = 0x28;
            dtmp[2] = 0x6B;
            dtmp[3] = 0x03;
            dtmp[4] = 0x00;
            dtmp[5] = 0x31;
            dtmp[6] = 0x45;
            dtmp[7] = (byte)(48 + errorlevel);
            return dtmp;
        }


        private static byte[] GetBytesForPrintQrCode(bool single)
        {
            byte[] dtmp;
            if (single)
            {  
                dtmp = new byte[9];
                dtmp[8] = 0x0A;
            }
            else
            {
                dtmp = new byte[8];
            }
            dtmp[0] = 0x1D;
            dtmp[1] = 0x28;
            dtmp[2] = 0x6B;
            dtmp[3] = 0x03;
            dtmp[4] = 0x00;
            dtmp[5] = 0x31;
            dtmp[6] = 0x51;
            dtmp[7] = 0x30;
            return dtmp;
        }

        private static byte[] GetQCodeBytes(String code)
        {
            ByteArrayOutputStream buffer = new ByteArrayOutputStream();
            try
            {
                byte[] d = Encoding.UTF8.GetBytes(code);
                int len = d.Length + 3;
                if (len > 7092) len = 7092;
                buffer.Write((byte)0x1D);
                buffer.Write((byte)0x28);
                buffer.Write((byte)0x6B);
                buffer.Write((byte)len);
                buffer.Write((byte)(len >> 8));
                buffer.Write((byte)0x31);
                buffer.Write((byte)0x50);
                buffer.Write((byte)0x30);
                for (int i = 0; i < d.Length && i < len; i++)
                {
                    buffer.Write(d[i]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return buffer.ToByteArray();
        }
    }
}
