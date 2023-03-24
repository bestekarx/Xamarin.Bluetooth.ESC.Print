
using System;

namespace XamarinESCUtils.Platforms.Common
{
    public interface IEscUtil
    {
        byte[] InitPrinter();
        byte[] SetPrinterDarkness(int value);
        byte[] GetPrintQrCode(String code, int modulesize, int errorlevel);
        byte[] GetPrintDoubleQrCode(String code1, String code2, int modulesize, int errorlevel);
        byte[] GetPrintQrCode2(String data, int size);
        byte[] GetPrintBarcode(String data, int symbology, int height, int width, int textposition);
        byte[] PrintBitmap(byte[] bytes);
        byte[] NextLine(int lineNum);
        byte[] underlineWithOneDotWidthOn();
        byte[] underlineWithTwoDotWidthOn();
        byte[] UnderlineOff();
        byte[] BoldOn();
        byte[] BoldOff();
        byte[] SingleByte();
        byte[] SingleByteOff();
        byte[] SetCodeSystemSingle(byte charset);
        byte[] SetCodeSystem(byte charset);
        byte[] AlignLeft();
        byte[] AlignCenter();
        byte[] AlignRight();
    }
}
