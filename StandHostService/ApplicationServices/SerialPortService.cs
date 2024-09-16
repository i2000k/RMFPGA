using System.IO.Ports;

namespace StandHostService.ApplicationServices;

public class SerialPortService : IDisposable
{
    private readonly SerialPort _serialPort;

    public SerialPortService(IHostApplicationLifetime appLifetime, IConfiguration configuration)
    {
        _serialPort = new SerialPort(configuration.GetSection("ComPortNumber").Value ?? "COM1", 9600);
        _serialPort.Parity = System.IO.Ports.Parity.None;
        _serialPort.StopBits = System.IO.Ports.StopBits.One;
        _serialPort.DataBits = 8;
        _serialPort.ReadTimeout = 300;

        try
        {
            Console.WriteLine("Intializing serial port");
            _serialPort.Open();
            _serialPort.ReadTimeout = 1000;
            _serialPort.Write("Hello");
            //char[] buffer = new char[5];
            //_serialPort.Read(buffer, 0, 1);
            //_serialPort.Read(buffer, 1, 1);
            //_serialPort.Read(buffer, 2, 1);
            //_serialPort.Read(buffer, 3, 1);
            //_serialPort.Read(buffer, 4, 1);

            //string spResponse = new string(buffer);

            Console.WriteLine("Success");

            //if (spResponse == "ardok")
            //{
            //    Console.WriteLine("Устройство подключено");
            //}
            //else
            //{
            //    throw new Exception("Нет ответа от устройства");
            //}
        }
        catch (Exception ex)
        {
            Console.WriteLine("Проблемы с подключением.\nУстройство не подключено.\n" + ex.Message);
            _serialPort.Close();
        }

        appLifetime.ApplicationStopping.Register(Dispose);
    }

    public void Dispose()
    {
        _serialPort.Close();
        _serialPort.Dispose();
    }

    public bool SendData(short[] inputData)
    {
        Console.WriteLine("$ Sending data...");
        if (_serialPort.IsOpen)
        {
            switch (inputData[0])
            {
                case 0:
                    _serialPort.Write("1L");
                    break;
                case 1:
                    _serialPort.Write("1H");
                    break;
                default:
                    break;
            }

            switch (inputData[1])
            {
                case 0:
                    _serialPort.Write("2L");
                    break;
                case 1:
                    _serialPort.Write("2H");
                    break;
                default:
                    break;
            }

            switch (inputData[2])
            {
                case 0:
                    _serialPort.Write("3L");
                    break;
                case 1:
                    _serialPort.Write("3H");
                    break;
                default:
                    break;
            }

            switch (inputData[3])
            {
                case 0:
                    _serialPort.Write("4L");
                    break;
                case 1:
                    Console.WriteLine("4 bit is 1");
                    _serialPort.Write("4H");
                    break;
                default:
                    break;
            }

            switch (inputData[4])
            {
                case 0:
                    _serialPort.Write("5L");
                    break;
                case 1:
                    _serialPort.Write("5H");
                    break;
                default:
                    break;
            }

            switch (inputData[5])
            {
                case 0:
                    _serialPort.Write("6L");
                    break;
                case 1:
                    _serialPort.Write("6H");
                    break;
                default:
                    break;
            }

            switch (inputData[6])
            {
                case 0:
                    _serialPort.Write("7L");
                    break;
                case 1:
                    _serialPort.Write("7H");
                    break;
                default:
                    break;
            }

            switch (inputData[7])
            {
                case 0:
                    _serialPort.Write("8L");
                    break;
                case 1:
                    _serialPort.Write("8H");
                    break;
                default:
                    break;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
