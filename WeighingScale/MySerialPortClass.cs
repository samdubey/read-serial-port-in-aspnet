namespace WeighingScale
{
    //Serial Ports are used to communicate with other independant devices over 
    //a serial COM. More at https://en.wikipedia.org/wiki/COM_(hardware_interface) and https://en.wikipedia.org/wiki/Serial_communication
    //For windows .NET Framework (This will not work in .net core or on linux)
    //You will be using IO.Ports
    using System.IO.Ports;
    using System;

    //Only one source can use a serial port at a time so encaposlation is important
    public class MySerialPortClass : IDisposable
    {
        //This is the class that will do most of the heavy lifting
        public SerialPort SerialPort { get; private set; }
        //diffenent devices use diffent baud rates or rates of electrical symbol 
        //change. This will cause diffent rates of communication, but both devices 
        //must agree on a rate to exchange at. The most common is 6800 bauds.
        const int DefaultBaudRate = 6800;
        //The above is also true of the amount of bits in a attomic ecnoding in a 
        //message.The most common is 8 bits for a byte. Honesly this one rarely changes.
        const int DefaultSize = 8;
        //The same is true of the party bit. The party bit is to detect error.
        //Hardware is often good enough now that is is not used often, but it adds
        //a bit to the start and sets it to make sure the number of set bits is odd
        //or even. Or it can be used to just mark the start, but stop bits are 
        //enough usally to mark at the end.
        const Parity DefaultParityBit = Parity.None;
        //Stop bits or period is to mark the end of a message. Usally one bit is used.
        const StopBits DefaultStopBits = StopBits.One;
        private string v;

        //since only one source can access a com at a time you may want to expose 
        //if it is open. It will also be useful for resource freeing after.
        public bool Open { get; private set; } = false;
        //It will also be useful for resource freeing after.
        internal bool Disposed { get; private set; } = false;
        //In Constructor we should set all the com vars. But the one we havent
        //defined a default yet is the com port name. This will change depeneding
        //on the plug used or if a virual (USB) on is used. Check your 
        //'Device Manager' for valid ports it could be. 
        public MySerialPortClass(string ComPort, int BaudRateValue = DefaultBaudRate, Parity Par = DefaultParityBit, int DataSize = DefaultSize, StopBits Stop = DefaultStopBits)
        {
            SerialPort = new SerialPort(ComPort, BaudRateValue, Par, DataSize, Stop);
        }
        //after all the set up you must open the port. If the port is in use you
        //will get an exception and you may need to rest it or your computer to
        //get it open. Agan only one source can use a port at a time.
        void OpenPort()
        {
            Open = true;
            SerialPort.Open();
        }
        //reading and writeing is simple after set up and opening, but for each 
        //device messages will have to be formated differntly. Check with your
        //devices manual or data sheets for more on formatting. 
        //can read a single byte at a time for decoding messages as they come in
        byte Readbyte()
        {
            return (byte)SerialPort.ReadByte();//cast is because dispite it being a byte ms made it an int container
        }
        char ReadChar()
        {
            return (char)SerialPort.ReadByte();//cast is because dispite it being a byte ms made it an int container
        }
        //or you can read a string out if you know messages are on a line or 
        //would rather mass decode
        //to read all in the buffer into a string
        public string ReadExisting()
        {
            return SerialPort.ReadExisting();
        }
        //if you know messages are a line like in a doc
        public string ReadLine()
        {
            return SerialPort.ReadLine();
        }
        //Lastly you can decode as a buffer if you know message lengths
        //Not it will fill in the buffer you provide and not size.
        public int Read(byte[] buffer, int offset, int count)
        {
            return SerialPort.Read(buffer, offset, count);
        }
        //You can simply write a sting out
        public void Write(string text)
        {
            SerialPort.Write(text);
        }
        //Or you can write from a buffer like in a stream
        public void Write(byte[] buffer, int offset, int count)
        {
            SerialPort.Write(buffer, offset, count);
        }
        //Or you can write a line from a string
        public void WriteLine(string text)
        {
            SerialPort.WriteLine(text);
        }
        //Lastly it is recomended thay you dispose of your class and free system
        //Resorces
        //Close will free the port for use by a different source
        void Close()
        {
            Open = false;
            SerialPort.Close();
        }
        //allows a using statement to dispose after use elegantly
        public void Dispose()
        {
            Disposed = true;
            SerialPort.Close();
            SerialPort.Dispose();
        }
        //in the garbage collection ensure disposal so port will open back up after.
        ~MySerialPortClass()
        {
            if (!Disposed)
            {
                Dispose();
            }
        }
    }
}