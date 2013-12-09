#undef USE_DOUBLE_QUEUE
#define USE_SINGLE_QUEUE
#define USE_SYSTEM_TIMER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AVR_Device;
using System.Threading;
using System.Windows.Threading;
using System.Timers;

namespace Arduino.Dome
{
    /// <summary>
    /// Message Data Structure
    /// </summary>
    public struct MessageData
    {
        public ulong reqID;
        public string msg;
    }

    /// <summary>
    /// Enumerator data structure to select the slewing direction
    /// </summary>
    public enum Direction
    {
        ANTICLOCWISE,                                   //  Turning Dome anticlockwise
        CLOCKWISE                                       //  Turning Dome clockwise
    }

    /// <summary>
    /// Peltier Objcet
    /// </summary>
    public class ArduinoDome
    {
        #region Constants
        // 
        //  Board Configurations
        //

        /// <summary>
        /// Command to turn clockwise
        /// </summary>
        const uint MotorClockwise = 4;                  
        /// <summary>
        /// Command to turn anticlockwise
        /// </summary>
        const uint MotorAnticlockwise = 5;
        /// <summary>
        /// Variable to indicate the Capability of the driver to use Encoder 
        /// quadrature signals A & B
        /// </summary>
        const bool use_Encoder_A_B = false;
        /// <summary>
        /// Variable to indicate the capability of the dri er to use encoder
        /// home signal
        /// </summary>
        const bool use_Encoder_Home = false;
        /// <summary>
        /// The encoder dummy request to identify a message of new dome position
        /// </summary>
        const ulong EncoderDummyRequest = ulong.MaxValue;
        /// <summary>
        /// The ADC reference voltage
        /// </summary>
        const double ADC_Ref = 5.0;
        /// <summary>
        /// Data structure to store all the Arduino implemented commands
        /// </summary>
        public struct DomeCommands
        {
            /// <summary>
            /// Command syntax
            /// </summary>
            public struct Syntax
            {
                public static string End = "\r\n";
                public static string Space = " ";
                public static string CR = "\r\n'";
            }
            /// <summary>
            /// Command to turn anticlockwise
            /// </summary>
            public static string TurnAnticlockwise = "turn_left";
            /// <summary>
            /// Command to turn clockwise
            /// </summary>
            public static string TurnClockwise = "turn_right";
            /// <summary>
            /// Command to stop turning
            /// </summary>
            public static string stopTurning = "stop";
            /// <summary>
            /// Command to get The Dome position
            /// </summary>
            public static string getPosition = "pos";
            /// <summary>
            /// Command to get help
            /// </summary>
            public static string getHelp = "help";
            /// <summary>
            /// Command to get FW info
            /// </summary>
            public static string getInfo = "info";
            public static string getAck = "get_ACK";
        }

        #endregion

        #region Members

        /// <summary>
        /// The message data structure
        /// </summary>
        private MessageData message;
        /// <summary>
        ///  counter to generate an unique message ID
        /// </summary>
        private ulong reqCounter = 0;
        /// <summary>
        /// Timer to synchronize the unfill the write FIFO queue
        /// </summary>       
#if USE_SYSTEM_TIMER
        private  System.Timers.Timer runTimer=new System.Timers.Timer(500);
#else
        private DispatcherTimer runTimer = new DispatcherTimer();
#endif
#if USE_DOUBLE_QUEUE
        /// <summary>
        /// FIFO Queue of the write buffer
        /// </summary>
        private Queue<MessageData> sendBuffer;
        /// <summary>
        /// FIFO Queue of the read buffer
        /// </summary>
        private List<MessageData> receiveBuffer;
#endif        
        /// <summary>
        /// The Arduino object
        /// </summary>
        public AVRDevice _avr;
#if USE_SINGLE_QUEUE
        private Queue<string> avrResult;                //  FIFO storing the Strings received from AVR
#endif        

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Peltier"/> class.
        /// Creates all the data structures but nothing else.
        /// AVR object is NOT INITIALIZED
        /// </summary>
        public ArduinoDome()
        {
            //  AVR NOT INITIALIZED
            _avr = null;                                        //  Arduino not connected yet
            DomePosition = 0.0;                                 //  Initialize Dome at Home position
            //  Initialize the timer that checks periodically
            //  the In/Out queues
#if USE_DOUBLE_QUEUE
    #if USE_SYSTEM_TIMER
            runTimer.Elapsed += new ElapsedEventHandler(runTimer_Elapsed);
            runTimer.Enabled = true;
    #else
            runTimer = new DispatcherTimer();   
            runTimer.Interval = new TimeSpan(0, 0, 0, 500);
            runTimer.Tick += new EventHandler(runTimer_Tick);
            runTimer.IsEnabled = true;
            runTimer.Start();
    #endif
#endif
            //  Initialize the Input FIFO
#if USE_DOUBLE_QUEUE
            //  Create In/Out queues
            sendBuffer = new Queue<MessageData>();
            receiveBuffer = new List<MessageData>();
#endif
#if USE_SINGLE_QUEUE
            avrResult = new Queue<string>();
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Peltier"/> class.
        /// </summary>
        /// <param name="com">The AVR Serial Port Name.</param>
        public ArduinoDome(string com)
        {        
//#if USE_DOUBLE_QUEUE
//            sendBuffer = new Queue<MessageData>();
//            receiveBuffer = new List<MessageData>();
//#endif
//#if USE_SINGLE_QUEUE
//            avrResult = new Queue<string>();
//#endif
            try
            {
                //  Initialize the AVR object
                _avr = new AVRDevice(com);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            DomePosition = 0.0;                                 //  Initialize Dome at Home position
            //  Initialize the timer that checks periodically
            //  the In/Out queues
#if USE_DOUBLE_QUEUE
    #if USE_SYSTEM_TIMER
            runTimer.Elapsed += new ElapsedEventHandler(runTimer_Elapsed);
            runTimer.Enabled = true;
    #else
            runTimer = new DispatcherTimer();
            runTimer.Interval = new TimeSpan(0, 0, 0, 500);
            runTimer.Tick += new EventHandler(runTimer_Tick);
            runTimer.Start();
    #endif
            //  Initialize the Input FIFO
//#if USE_DOUBLE_QUEUE
            //  Create In/Out queues
            sendBuffer = new Queue<MessageData>();
            receiveBuffer = new List<MessageData>();
#endif
#if USE_SINGLE_QUEUE
            avrResult = new Queue<string>();
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Peltier"/> class.
        /// </summary>
        /// <param name="com">The AVR Serial Port Nale.</param>
        /// <param name="flag">if set to <c>true</c> Tells the application that AVR109 bootloader has to be used.</param>
        public ArduinoDome(string com, bool flag)
        {         
            DomePosition = 0.0;
            //  Initialize the Input FIFO
//#if USE_DOUBLE_QUEUE
//            sendBuffer = new Queue<MessageData>();
//            receiveBuffer = new List<MessageData>();
//#endif
//#if USE_SINGLE_BUFFER
//            avrResult = new Queue<string>();
//#endif
            try
            {
                //  Initialize the AVR object
                _avr = new AVRDevice(com, flag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            DomePosition = 0.0;                                 //  Initialize Dome at Home position
            //  Initialize the timer that checks periodically
            //  the In/Out queues
//            runTimer = new DispatcherTimer();
#if USE_DOUBLE_QUEUE
    #if USE_SYSTEM_TIMER
            runTimer.Elapsed += new ElapsedEventHandler(runTimer_Elapsed);
            runTimer.Enabled = true;
    #else
            runTimer.Interval = new TimeSpan(0, 0, 1); //0, 500);
            runTimer.Tick += runTimer_Tick;// new EventHandler(runTimer_Tick);
            runTimer.IsEnabled = true;
            runTimer.Start();
    #endif
            //  Initialize the Input FIFO
//#if USE_DOUBLE_QUEUE
            //  Create In/Out queues
            sendBuffer = new Queue<MessageData>();
            receiveBuffer = new List<MessageData>();
#endif

#if USE_SINGLE_QUEUE
            avrResult = new Queue<string>();
#endif
        }

        #endregion

        #region Event Handlers

#if USE_DOUBLE_QUEUE
        /// <summary>
        /// RunTimer Timer Tick Event handler. This handler sends all the requests into the
        /// out queue
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void runTimer_Tick(object sender, EventArgs e)
        {
            // Send all the request in queue
            foreach (MessageData req in sendBuffer)
            {                
                SendCommand(req);
            }
        }

        private void runTimer_Elapsed(object sender, ElapsedEventArgs e)         
        {
            // Send all the request in queue
            foreach (MessageData req in sendBuffer)
            {                
                SendCommand(req);
            }
        }
#endif

        #endregion

        #region Properties

        /// <summary>
        /// Dome angle position.
        /// </summary>
        /// <value>
        /// The dome angle position.
        /// </value>
        public Angle DomePosition //{ get; set; }
        {
            set
            {
//                DomePosition = (Angle)value;
            }
            get
            {
                //  Stores last Dome angle position
                Angle oldPos = DomePosition;
                //
                //  Look into the RX queue if the Arduino hasn't alrady
                //  sent a position update
                //
                try
                {
                    
#if USE_DOUBLE_BUFFER
                    //  Copy the RX queue to parse
                    List<MessageData> foo = receiveBuffer;
                    //  Parse the queue trying to catch the tag "Position=" indicating
                    //  a position update
                    foreach (MessageData m in foo)
                    {
                        if (m.msg.Contains("Position="))
                        {
                            //  If found split the received string into its tokens and covert it
                            //  into a double to update the property
                            try
                            {
                                string[] tokens = m.msg.Split(' ');
                                return (Angle)Convert.ToDouble(tokens[1]);
                            }
                            catch (Exception)
                            {
                                //  In case of error restore old position and throw an exception
                                return oldPos;
                                throw new Exception("Error reading dome position");
                            }
                        }
                    }
#elif USE_SINGLE_QUEUE
                    //  Copy the RX queue to parse
                    Queue<string> foo = avrResult;
                    //  Parse the queue trying to catch the tag "Position=" indicating
                    //  a position update
                    foreach (string s in foo)
                    {
                        if (s.Contains("Position="))
                        {
                            //  If found split the received string into its tokens and covert it
                            //  into a double to update the property
                            try
                            {
                                string[] tokens = s.Split(' ');
                                return (Angle)Convert.ToDouble(tokens[1]);
                            }
                            catch (Exception)
                            {
                                //  In case of error restore old position and throw an exception
                                return oldPos;
                                throw new Exception("Error reading dome position");
                            }
                        }                        
                    }
#endif
                    //
                    //  If there are no position update into the RX queue, launch an Arduino 
                    //  command to read the position
                    //
                    if (oldPos == DomePosition)
                    {
                        //  Build up the Arduino command string for requesting the Dome position
                        string cmd = BuildArduinoCommand(DomeCommands.getPosition);
                        string result;
#if USE_DOUBLE_QUEUE
                        MessageData _msg;
                        //  Check if the request is equal to the requestless Encoder Position information
                        //  if yes roll up the counter
                        if (reqCounter++ == EncoderDummyRequest)
                        {
                            try { reqCounter++; }
                            catch { }
                        }
                        //  Build up position the request
                        _msg.reqID = reqCounter;
                        _msg.msg = cmd;
                        //  Enqueue the request on the TX queue
                        sendBuffer.Enqueue(_msg);
                        int index = int.MaxValue;
                        //  Awaits the answer to the position request, if it is received 
                        //  the RX data are consumed and property update, else an exception
                        //  is returned
                        if (waitAVRAnswer(_msg.reqID, index))
                        {
                            //  Retrieve the Arduino message corresponding position request
                            result = receiveBuffer[index].msg;
                            //  Remove this item from the RX queue
                            receiveBuffer.RemoveAt(index);
                            //  Split the RX data into their tokens to consume the information
                            char[] delim = { ' ', '=', '\r', '\n' };
                            string[] tokens = result.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                            //  Try to convert the data in a double and then return the angle
                            try
                            {
                                return (Angle)Convert.ToDouble(tokens[1]);
                            }
                            catch
                            {                                
                                throw new Exception("Error reading Dome position");
                                return oldPos;
                            }
                        }
                        else
                        {                            
                            throw new Exception("Timeout getting Dome position");
                            return oldPos;
                        }
#elif USE_SINGLE_QUEUE                        
                        //  Send the request to the Arduino
                        _avr.Send(cmd);                        
                        //  Awaits the answer to the position request, if it is received 
                        //  the RX data are consumed and property update, else an exception
                        //  is returned
                        Thread.Sleep(100);
                        result = _avr.getCOMData();
                        //  Split the RX data into their tokens to consume the information
                        char[] delim = { ' ', '=', '\r', '\n' };
                        string[] tokens = result.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                        //  Try to convert the data in a double and then return the angle
                        try
                        {
                            return (Angle)Convert.ToDouble(tokens[1]);
                        }
                        catch
                        {
                            throw new Exception("Error reading Dome position");
                            return oldPos;
                        }                        
#endif
                    }
                    else
                    {
                        return oldPos;
                    }
                }
                catch (Exception ex)
                {
                    //  In case of error throw the relative exception                    
                    throw ex;
                    return oldPos;
                }
            }
        }

        public string Version
        {
            get
            {
                string cmd = BuildArduinoCommand(DomeCommands.getInfo);
                string result;
#if USE_DOUBLE_QUEUE
                MessageData _msg;
                //  Check if the request is equal to the requestless Encoder Position information
                //  if yes roll up the counter
                if (reqCounter++ == EncoderDummyRequest)
                {
                    try { reqCounter++; }
                    catch { }
                }
                //  Build up position the request
                _msg.reqID = reqCounter;
                _msg.msg = cmd;
                //  Enqueue the request on the TX queue
                sendBuffer.Enqueue(_msg);
                int index = int.MaxValue;
                //  Awaits the answer to the position request, if it is received 
                //  the RX data are consumed and property update, else an exception
                //  is returned
                if (waitAVRAnswer(_msg.reqID, index))
                {
                    //  Retrieve the Arduino message corresponding position request
                    result = receiveBuffer[index].msg;
                    //  Remove this item from the RX queue
                    receiveBuffer.RemoveAt(index);
                    //  Return the Arduino answer
                    return result;
                }
                else
                {
                    return null;
                    throw new Exception("Timeout getting Dome position");
                }
#elif USE_SINGLE_QUEUE
                //  Send the request to the Arduino
                _avr.Send(cmd);
                //  Awaits the answer to the position request, if it is received 
                //  the RX data are consumed and property update, else an exception
                //  is returned
                Thread.Sleep(100);
                result = _avr.getCOMData();
                return result;
#endif
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the arduino command.
        /// </summary>
        /// <param name="cmd">The command to pass to Arduino.</param>
        /// <returns>The Arduino string</returns>
        public string BuildArduinoCommand(string cmd)
        {
            return cmd + DomeCommands.Syntax.Space + DomeCommands.Syntax.End;
        }

        /// <summary>
        /// Builds the arduino command.
        /// </summary>
        /// <param name="cmd">The command to pass to Arduino.</param>
        /// <param name="args">The argument of the command.</param>
        /// <returns></returns>
        public string BuildArduinoCommand(string cmd, string args)
        {
            return cmd + DomeCommands.Syntax.Space + args + DomeCommands.Syntax.End;
        }

        /// <summary>
        /// Connects the Application to the AVR instance.
        /// </summary>
        /// <returns>True f operation was good, false otherwise</returns>
        /// <exception cref="System.NullReferenceException">AVR Objcet not referenced in Peltier class</exception>
        public bool Connect()
        {
            //  If the AVR instance has already been instantied launch its connect method, else throw an exception
            try
            {
                Thread.Sleep(200);
                if (_avr != null)
                {
                    //_avr.Connect();
                    //Thread.Sleep(100);
                    runTimer.Start();
                    Thread.Sleep(100);
                    _avr.Connect();
                    return true;
                }
                else return false;//throw new NullReferenceException("AVR Objcet not referenced in Peltier class");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Disconnects the Application to the AVR instance.
        /// </summary>
        /// <returns>True of operatop, was good, false otehrwise</returns>
        /// <exception cref="System.NullReferenceException">AVR Object not referenced in Peltrier Class</exception>
        public bool Disconnect()
        {
            try
            {
                if (_avr == null) //throw new NullReferenceException("AVR Object not referenced in Peltrier Class");
                {
                    return false;
                }
                else
                {
                    _avr.Disconnect();
                    runTimer.Stop();
                    return true;
                }
                //_avr.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sends a command to the AVR instance and checks the operation result.
        /// </summary>
        /// <param name="_msg">The _MSG.</param>
        /// <returns></returns>
        private bool SendCommand(MessageData _msg)
        {
            //  Create the request string

            string request = _msg.msg + DomeCommands.Syntax.End;
            //  Send the request to the AVR instance
            _avr.Send(request);
            //
            //  For robustness reasons the request is sent 10 times before sending an exception
            //
            uint count = 10;
            do
            {
                //  Waits the AVR to complete the operation and read from the AVR instance
                Thread.Sleep(100);
                string result = _avr.getCOMData();
                //  Command return string
                string foo = _msg.msg.Remove(_msg.msg.Length - 3) + DomeCommands.Syntax.Space + "OK";
                //  If the result contains "OK" 
                if (result.Contains(foo))
                {
                    //  Push the received string into the Input FIFO and return
                    MessageData data;
                    data.reqID = _msg.reqID;
                    data.msg = result;
#if USE_DOUBLE_BUFFER
                    receiveBuffer.Add(data);
#endif
                    return true;
                }
                else if (result.Contains("Error:"))
                {
                    //  If string contains Error decrease the counter and send twice 
                    //  a CR LF string reading the output.
                    //  Then the string is send again to the AVR instance
                    count--;
                    for (int i = 0; i < 3; i++)
                    {
                        _avr.Send(DomeCommands.Syntax.CR);
                        Thread.Sleep(100);
                    }
                    //_avr.Send("\r\n");
                    //Thread.Sleep(100);
                    //_avr.Send("\r\n");
                    //Thread.Sleep(100);
                    _avr.getCOMData();
                }
                else
                {
                    //  It shouldn't come here, but in case push the received string into the FIFO and returns                    
                    //avrResult.Enqueue(result);
                    //return true;
                    throw new Exception("Send managmement branch not supported", new EncoderFallbackException(""));
                }
            }
            //  If counter is not expired send the command again
            while (count > 0);
            //  Counter has expired, operation failed
            return false;
        }

        /// <summary>
        /// Sends a command to the AVR instance and checks the operations is fine.
        /// </summary>
        /// <param name="cmd">The command to be sent to AVR.</param>
        /// <returns></returns>
        public bool SendCommand(string cmd)
        {
            //  Create the request string
            string request = cmd + DomeCommands.Syntax.End;
            //  Send the request to the AVR instance
            _avr.Send(request);
            //
            //  For robustness reasons the request is sent 10 times before sending an exception
            //
            uint count = 10;
            do
            {
                //  Waits the AVR to complete the operation and read from the AVR instance
                Thread.Sleep(100);
                string result = _avr.getCOMData();
                //  If the result contains "OK" 
                if (result.Contains("OK"))
                {
                    //  Push the received string into the Input FIFO and return
#if USE_SINGLE_QUEUE
                    avrResult.Enqueue(result);
#endif
                    return true;
                }
                else if (result.Contains("Error:"))
                {
                    //  If string contains Error decrease the counter and send twice 
                    //  a CR LF string reading the output.
                    //  Then the string is send again to the AVR instance
                    count--;
                    _avr.Send("\r\n");
                    Thread.Sleep(100);
                    _avr.Send("\r\n");
                    Thread.Sleep(100);
                    _avr.getCOMData();
                }
                else
                {
                    //  It shouldn't come here, but in case push the received string into the FIFO and returns                    
#if USE_SINGLE_QUEUE
                    avrResult.Enqueue(result);
#endif
                    return true;
                }

            }
            //  If counter is not expired send the command again
            while (count > 0);
            //  Counter has expired, operation failed
            return false;
        }

#if USE_DOUBLE_BUFFER
        /// <summary>
        /// Waits ta given answer to a request from the AVR Device.
        /// </summary>
        /// <param name="ID">The Request ID.</param>
        /// <param name="index">The index of the request answer into the receiveBufffer queue.</param>
        /// <returns>True if the request  false if not found after 100 attemps</returns>
        public bool waitAVRAnswer(ulong ID, int index)
        {
            index = 0;
            Thread.Sleep(500);
//            List<MessageData> foo = receiveBuffer;
            for (int i = 0; i < 100; i++)
            {
                //List<MessageData> foo = receiveBuffer;
                MessageData[] foo = new MessageData[100];
                receiveBuffer.CopyTo(foo);
                foreach (MessageData res in foo)
                {
                    if (res.reqID == ID) return true;
                    else index++;
                }
                index = 0;
                Thread.Sleep(100);
            }
            index = int.MaxValue;
            return false;
        }
#endif

        /// <summary>
        /// Initialize the Dome Instance.
        /// </summary>        
        public void Init()
        {

        }

//        /// <summary>
//        /// Returns the Dome position.
//        /// </summary>
//        /// <returns>The Dome Angle position in string</returns>
//        /// <exception cref="System.Text.EncoderFallbackException">Timeout from getting Dome Position</exception>
//        public string GetDomePosition()
//        {
//            try
//            {                
//                //  Looks if there is already an information about position   
//                Angle oldPos=DomePosition;
//                List<MessageData>foo=receiveBuffer;
//                foreach(MessageData m in foo)
//                {
//                    if(m.msg.Contains("Position="))
//                    {
//                        try
//                        {
//                            DomePosition=(Angle)Convert.ToDouble(m.msg);
//                        }
//                        catch(Exception ex)
//                        {
                            
//                        }
//                    }
//                }
//                //  Ask directly the position of the dome
//                if(DomePosition==oldPos)
//                {
//                    string cmd=string.Format("pos");
//                    string result;
//#if USE_DOUBLE_QUEUE
//                    MessageData _msg;
//                    _msg.reqID=reqCounter++;
//                    _msg.msg=cmd;
//                    sendBuffer.Enqueue(_msg);
//                    int index=int.MaxValue;
//                    if(waitAVRAnswer(_msg.reqID, index))
//                    {
//                        result=receiveBuffer[index].msg;
//                        receiveBuffer.RemoveAt(index);
//                        return result;
//                    }
//                    else throw new EncoderFallbackException("Timeout from getting Dome Position");
//#else
//                    if(sendCommand(cmd)) result=avrResult.Dequeue();
//                    else throw new Exception(avrResult.Dequeue());
//                    return result;
//#endif
//                }
//                else return DomePosition.ToString();
//            }
//            catch(Exception ex)
//            {
//                throw ex;
//            }
//        }

        /// <summary>
        /// Method to Initialie the Peltier Cell PWM channel.
        /// </summary>
        /// <param name="pwm">The PWM channel.</param>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Invalid PWM channel</exception>        
        public string Stop()        
        {
            try
            {
                //  Creates the Arduino stop command
                string cmd = BuildArduinoCommand(DomeCommands.stopTurning);
                string result;
                //  Send the command to the AVR instance, if the result is true (AVR instance responded "OK")
                //  Read the result from the FIFO to clean it up.
                //  Else send an exception using the resulting string as message
#if USE_DOUBLE_QUEUE
                MessageData _msg;
                _msg.reqID = reqCounter++;
                _msg.msg = cmd;
                sendBuffer.Enqueue(_msg);
                int index = int.MaxValue;
                if (waitAVRAnswer(_msg.reqID, index))
                {
                    result = receiveBuffer[index].msg;
                    receiveBuffer.RemoveAt(index);
                    return result;
                }
                else throw new Exception("Timeout from PWM Init function");
#endif
#if USE_SINGLE_QUEUE
                    if (SendCommand(cmd) == true) result = avrResult.Dequeue();
                    else throw new Exception(avrResult.Dequeue());
                    return result;
#endif
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the AVR Firmware version.
        /// </summary>
        /// <returns>The Firmware Version String</returns>
        /// <exception cref="System.Exception"></exception>
        public string GetVersion()
        {
            try
            {
                //  Prepare the AVR Firmware Version string command
                string cmd = BuildArduinoCommand(DomeCommands.getInfo);
                string result;
#if USE_DOUBLE_QUEUE
                MessageData _msg;
                if(reqCounter++ == EncoderDummyRequest)
                {
                    try { reqCounter++; }
                    catch { }
                }
                _msg.reqID=reqCounter;
                _msg.msg=cmd;
                //  Enqueue the request
                sendBuffer.Enqueue(_msg);
                Thread.Sleep(100);
                int index=int.MaxValue;
                //  Waits the answer
                if(waitAVRAnswer(_msg.reqID, index))
                {
                    result=receiveBuffer[index].msg;
                    receiveBuffer.RemoveAt(index);
                    return result;
                }
#elif USE_SINGLE_QUEUE
                _avr.Send(cmd);
                Thread.Sleep(500);
                //  Reads back the answer from the AVR and check if it is consistent
                string res = _avr.getCOMData();
                //  If it contains "OK" returns the string from AVR
                //  Else throw an exception
                if (res.Contains(DomeCommands.getInfo + " OK")) return res;
                else
                {
                    throw new TimeoutException("Timeout getting FW Version");
                }
#endif
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// Gets a ACK from the AVR.
        /// </summary>
        /// <returns>True if ACK is received, false if not</returns>
        public bool GetAck()
        {
            try
            {
                //  Prepare the AVR Firmware command to request the ACK
                string cmd = BuildArduinoCommand(DomeCommands.getAck);
                string result;
#if USE_DOUBLE_QUEUE
                MessageData _msg;
                if(reqCounter++ == EncoderDummyRequest)
                {
                    try { reqCounter++; }
                    catch { }
                }
                _msg.reqID=reqCounter;
                _msg.msg=cmd;
                //  Enqueue the request
                sendBuffer.Enqueue(_msg);
                int index=int.MaxValue;
                //  Waits the answer
                if(waitAVRAnswer(_msg.reqID, index))
                {
                    result=receiveBuffer[index].msg;
                    receiveBuffer.RemoveAt(index);
                    if (result.Contains("OK")) return true;
                    else return false;
                }
#elif USE_SINGLE_QUEUE
                _avr.Send(cmd);
                Thread.Sleep(500);
                //  Reads back the answer from the AVR and check if it is consistent
                string res = _avr.getCOMData();
                //  If it contains "OK" returns the string from AVR
                //  Else throw an exception
                if (res.Contains(DomeCommands.getAck + " OK")) return true;
                else
                {
                    throw new TimeoutException("Timeout getting FW Version");
                }
#endif
                                
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }            
        }

        public bool Slew(Direction dir)
        {
            try
            {
                string cmd;
                //  First check in which direction to turn and build the Arduino FW command
                if(dir==Direction.CLOCKWISE) cmd=BuildArduinoCommand(DomeCommands.TurnClockwise);
                else cmd=BuildArduinoCommand(DomeCommands.TurnAnticlockwise);
                string result;                                
#if USE_DOUBLE_QUEUE
                //  Create the request ID, avoiding that it is the requestless Position gotten from encoder
                MessageData _msg;
                //  Build up the message
                if(reqCounter++ ==EncoderDummyRequest)
                {
                    try{reqCounter++;}catch {}
                }
                _msg.reqID=reqCounter;
                _msg.msg=cmd;
                //  Put the reques on the outgoing queue
                sendBuffer.Enqueue(_msg);
                int index=int.MaxValue;
                //  Awaits an answer from Arduino
                if(waitAVRAnswer(_msg.reqID, index))
                {
                    //  Store the answer to the slew request on a variable and
                    //  clean up the RX queue from this request
                    result=receiveBuffer[index].msg;
                    receiveBuffer.RemoveAt(index);
                }
                else
                {
                    throw new TimeoutException("Timeount on Dome slewing request");
                }
                                //  Now check up if the Arduino correctely implement the request
                if((dir==Direction.CLOCKWISE) && (result.Contains(DomeCommands.TurnClockwise+DomeCommands.Syntax.Space+"OK")))
                {                    
                    return true;
                }
                if((dir==Direction.ANTICLOCWISE)&&(result.Contains(DomeCommands.TurnAnticlockwise+DomeCommands.Syntax.Space+"OK")))
                {
                    return true;
                }
                else
                {
                    return false;
                }
#elif USE_SINGLE_QUEUE
                return SendCommand(cmd);
#endif
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

