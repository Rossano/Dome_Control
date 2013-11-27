#define USE_DOUBLE_QUEUE
#undef USE_SINGLE_QUEUE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AVR_Device;
using System.Threading;
using System.Windows.Threading;

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
        private DispatcherTimer runTimer;
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
            runTimer = new DispatcherTimer();   
            runTimer.Interval = new TimeSpan(0, 0, 0, 500);
            runTimer.Tick += new EventHandler(runTimer_Tick);
            runTimer.Start();
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
        public ArduinoDome(string com):base()
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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Peltier"/> class.
        /// </summary>
        /// <param name="com">The AVR Serial Port Nale.</param>
        /// <param name="flag">if set to <c>true</c> Tells the application that AVR109 bootloader has to be used.</param>
        public ArduinoDome(string com, bool flag):base()
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
        }

        #endregion

        #region Event Handlers

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
            get;
            set
            {
                Angle oldPos = DomePosition;
                try
                {
                    List<MessageData> foo = receiveBuffer;
                    foreach (MessageData m in foo)
                    {
                        if (m.msg.Contains("Position="))
                        {
                            try
                            {
                                string[] tokens = m.msg.Split(' ');
                                DomePosition = (Angle)Convert.ToDouble(tokens[1]);
                            }
                            catch (Exception ex)
                            {
                                DomePosition = oldPos;
                                throw new Exception("Error reading dome position");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
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
                if (_avr != null)
                {
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
                string foo = _msg.msg + DomeCommands.Syntax.Space + "OK";
                //  If the result contains "OK" 
                if (result.Contains(foo))
                {
                    //  Push the received string into the Input FIFO and return
                    MessageData data;
                    data.reqID = _msg.reqID;
                    data.msg = result;
                    receiveBuffer.Add(data);
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

        /// <summary>
        /// Waits ta given answer to a request from the AVR Device.
        /// </summary>
        /// <param name="ID">The Request ID.</param>
        /// <param name="index">The index of the request answer into the receiveBufffer queue.</param>
        /// <returns>True if the request  false if not found after 100 attemps</returns>
        public bool waitAVRAnswer(ulong ID, int index)
        {
            index = 0;
            List<MessageData> foo = receiveBuffer;
            for (int i = 0; i < 100; i++)
            {
                foreach (MessageData res in foo)
                {
                    if (res.reqID == ID) return true;
                    else index++;
                }
                index = 0;
                Thread.Sleep(10);
            }
            index = int.MaxValue;
            return false;
        }

        /// <summary>
        /// Initialize the Dome Instance.
        /// </summary>        
        public void Init()
        {

        }

        /// <summary>
        /// Returns the Dome position.
        /// </summary>
        /// <returns>The Dome Angle position in string</returns>
        /// <exception cref="System.Text.EncoderFallbackException">Timeout from getting Dome Position</exception>
        public Angle GetDomePosition()
        {
            try
            {                
                //  Looks if there is already an information about position   
                Angle oldPos=DomePosition;
                List<MessageData>foo=receiveBuffer;
                foreach(MessageData m in foo)
                {
                    if(m.msg.Contains("Position="))
                    {
                        try
                        {
                            DomePosition=(Angle)Convert.ToDouble(m.msg);
                        }
                        catch(Exception ex)
                        {
                            
                        }
                    }
                }
                //  Ask directly the position of the dome
                if(DomePosition==oldPos)
                {
                    string cmd=string.Format("pos");
                    string result;
#if USE_DOUBLE_QUEUE
                    MessageData _msg;
                    _msg.reqID=reqCounter++;
                    _msg.msg=cmd;
                    sendBuffer.Enqueue(_msg);
                    int index=int.MaxValue;
                    if(waitAVRAnswer(_msg.reqID, index))
                    {
                        result=receiveBuffer[index].msg;
                        receiveBuffer.RemoveAt(index);
                        return result;
                    }
                    else throw new EncoderFallbackException("Timeout from getting Dome Position");
#else
                    if(sendCommand(cmd)) result=avrResult.Dequeue();
                    else throw new Exception(avrResult.Dequeue());
                    return result;
#endif
                }
                else return DomePosition.ToString();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Initialie the Peltier Cell PWM channel.
        /// </summary>
        /// <param name="pwm">The PWM channel.</param>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Invalid PWM channel</exception>        
        public string Stop()        
        {
            //  Initially check if the pwm channel is possible
            uint pwm=0;
            if ((pwm >= 0) && (pwm < 5))
            {
                try
                {
                    //  Creates the PWM initialization channel in the AVR Firmware format
                    string cmd = string.Format("pwm_init {0}", pwm);
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
            else throw new ArgumentOutOfRangeException("Invalid PWM channel");
        }

        /// <summary>
        /// Initialize the ADC channel connected to the Temperature sensors.
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        protected string InitSensors()
        //protected void InitSensors()
        {
            try
            {
                //  Create the ADC initialization command in AVR Firmware version
                string cmd = "adc_init";
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
                else throw new Exception("Timeout from Init Sensor function");
#endif
#if USE_SINGLE_QUEUE
                if (SendCommand(cmd)) result = avrResult.Dequeue();
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
        /// Set the level of the PWM channel.
        /// </summary>
        /// <param name="channel">The PWM channel.</param>
        /// <param name="level">The new PWM level.</param>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Invalid PWM channel</exception>
        public string PwmSet(uint channel, uint level)
        // public void PwmSet (uint channel, uint level)
        {
            //  Firstly check if the PWM channel is coherent
            if ((channel >= 0) && (channel < 5))
            {
                try
                {
                    //  Create the PWM set command in the AVR Firmware format
                    string cmd = string.Format("pwm_set {0} {1}", channel, level);
                    string result;
                    //  Send the command to the AVR instance, if the result is true (AVR instance responded "OK")
                    //  Read the result from the FIFO to clean it up.
                    //  Else send an exception using the resulting string as message.
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
                    else throw new Exception("Timeout from PWM Set function");
#endif
#if USE_SINGLE_QUEUE
                    if (SendCommand(cmd)) result = avrResult.Dequeue();
                    else throw new Exception(avrResult.Dequeue());
                    return result;
#endif
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else throw new ArgumentOutOfRangeException("Invalid PWM channel");
        }

        /// <summary>
        /// Start a PWMs channel.
        /// </summary>
        /// <param name="channel">The PWM channel.</param>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Invalid PWM channel</exception>
        public string PwmStart(uint channel)
        // public void PwmStart(uint channel)
        {
            //  Firstly check if the PWM channel is coherent
            if ((channel >= 0) && (channel < 5))
            {
                try
                {
                    //  Create the PWM start command in the AVR Firmware format
                    string cmd = string.Format("pwm_start {0}", channel);
                    string result;
                    //  Send the command to the AVR instance, if the result is true (AVR instance responded "OK")
                    //  Read the result from the FIFO to clean it up.
                    //  Else send an exception using the resulting string as message.
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
                    else throw new Exception("Timeout from PWM Start function");
#endif
#if USE_SINGLE_QUEUE
                    if (SendCommand(cmd)) result = avrResult.Dequeue();
                    else throw new Exception(avrResult.Dequeue());
                    return result;
#endif
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else throw new ArgumentOutOfRangeException("Invalid PWM channel");
        }

        /// <summary>
        /// Stop a PWMs channel.
        /// </summary>
        /// <param name="channel">The PWM channel.</param>
        /// <exception cref="System.
        /// Exception"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Invalid PWM channel</exception>
        public string PwmStop(uint channel)
        // public void PwmStop(uint channel)
        {
            //  Firstly check if the PWM channel is coherent
            if ((channel >= 0) && (channel < 5))
            {
                try
                {
                    //  Create the PWM stop command in the AVR Firmware format
                    string cmd = string.Format("pwm_stop {0}", channel);
                    string result;
                    //  Send the command to the AVR instance, if the result is true (AVR instance responded "OK")
                    //  Read the result from the FIFO to clean it up.
                    //  Else send an exception using the resulting string as message.
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
                    else throw new Exception("Timeout from PWM Start function");
#endif
#if USE_SINGLE_BUFFER
                    if (SendCommand(cmd)) result = avrResult.Dequeue();
                    else throw new Exception(avrResult.Dequeue());
                    return result;
#endif
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else throw new ArgumentOutOfRangeException("Invalid PWM channel");
        }

        /// <summary>
        /// Method to Calibrate the ADC.
        /// </summary>
        /// <exception cref="System.Exception">
        /// </exception>
        public void ADCCalibration()
        {
            //
            //  The Method is measuring two known voltages on the ADC (the ground and the Bandgap)
            //  And from the result and the exptected result is carrying out the gain and offset corrections.
            //
            try
            {
                //  Read the ADC ground channel
                string cmd = string.Format("adc_get  {0}", AVR_GND_ADC);
                string result;
                uint A0, A1;
                //  Send the command and reads out the result
                if (SendCommand(cmd))
                {
                    Thread.Sleep(100);
                    //  Read the response from FIFO
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
                    }
                    else throw new Exception("Timeout from PWM Start function");
#endif
#if USE_SINGLE_QUEUE
                    result = avrResult.Dequeue();
#endif
                    //  Split the string in its different words (tokens)
                    char[] delim = { ' ', ':', ';', '\r', '\n' };
                    string[] tokens = result.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                    uint i = 0;
                    //  Looks into the tokens for "ADC->"  the value is the next one
                    foreach (string s in tokens)
                    {
                        if (s.Equals("ADC->")) break;
                        i++;
                    }
                    //  Convert and store the ADC value
                    A0 = Convert.ToUInt16(tokens[i + 1]);
                }
                else
                {
#if USE_DOUBLE_QUEUE
                    throw new Exception("Erroror during thr ADC calicbration procedure");
#endif
#if USE_SINGLE_QUEUE
                    throw new Exception(avrResult.Dequeue());
#endif
                }
                //  Read teh ADC Bandgap channel
                cmd = string.Format("adc_get {0}", AVR_BandGap_ADC);
                //  Send the command and read out the result
                if (SendCommand(cmd))
                {
                    Thread.Sleep(100);
                    //  Read the response from the FIFO
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
                    }
                    else throw new Exception("Timeout from PWM Start function");
#endif
#if USE_SINGLE_QUEUE
                    result = avrResult.Dequeue();
#endif
                    //  Split the string in its different words (tokens)
                    char[] delim = { ' ', ':', ';', '\r', '\n' };
                    string[] tokens = result.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                    uint i = 0;
                    //  Looks into the tokens for "ADC->" the value conversion is the next one
                    foreach (string s in tokens)
                    {
                        if (s.Equals("ADC->")) break;
                        i++;
                    }
                    //  Convert and store the ADC value
                    A1 = Convert.ToUInt16(tokens[i + 1]);
                }
                else
                {
#if USE_DOUBLE_QUEUE
                    throw new Exception("Error reading the from ADC during calibration procedure");
#endif
#if USE_SINGLE_QUEUE
                    throw new Exception(avrResult.Dequeue());
#endif
                }
                //  Carry out the offset and gain correction factors
                //beta = (double)A0 / 1023 * ADC_Ref;
                //alpha = ((A1 - A0) * ADC_Ref) / 1023 / 1.1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the temperature from a sensor.
        /// This method read from an ADC channel and returns the conversion result. It is an ADC reading function.
        /// </summary>
        /// <param name="channel">The ADC channel.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Invalid ADC channel</exception>
        public uint GetTemperature(uint channel)
        {
            //  Firstly check if the channel are coherent with the Arduino Analog Inputs, or the Bandgap Channel
            if ((channel >= 0) && (channel <= 5) || ((channel >= 9) && (channel <= 10)) || (channel == AVR_BandGap_ADC))
            {
                try
                {
                    //  Create the ADC reading string in the AVR FIrmware format.
                    string cmd = string.Format("adc_get {0}", channel);
                    string result;
                    //  Send the command to the AVR instance and read the result
                    if (SendCommand(cmd))
                    {
                        bool endit = false;
                        do
                        {
                            //  Get the AVR result from FIFO
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
                                endit = true;
                            }
                            else throw new Exception("Timeout from PWM Start function");
#endif
#if USE_SINGLE_QUEUE
                            result = avrResult.Dequeue();
#endif
                            //  Split the result into the words (the tokens)
                            char[] delim = { ' ', ':', ';', '\r', '\n' };
                            string[] tokens = result.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                            uint i = 0;
                            //  Parse the tokens to look for "ADC->"
                            foreach (string s in tokens)
                            {
                                if (s.Equals("ADC->")) break;
                                i++;
                            }
                            //  the Conversion value is the next one
                            return Convert.ToUInt16(tokens[i + 1]);
                        }
#if USE_DOUBLE_QUEUE
                        while (!endit);
#endif
#if USE_SINGLE_QUEUE
                        while (avrResult.Count > 0);
#endif
                    }
                    else
                    {
#if USE_DOUBLE_QUEUE
                        throw new Exception("Error reading Temperature");
#endif
#if USE_SINGLE_QUEUE
                        throw new Exception(avrResult.Dequeue());
#endif
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else throw new ArgumentOutOfRangeException("Invalid ADC channel");
            return 0;
        }

        /// <summary>
        /// Gets the Peltier Cell and Room temperatures.
        /// This method reads the ADC channels connected to the temperature sensors and convert the result as temperatures.
        /// </summary>
        public string GetTemperatures()
        //public void GetTemperatures()
        {
            string result;
            try
            {
                //  Read the Peltier Cell temperature and convert it into voltage
                double VADC_Peltier = GetTemperature(Peltier_Temperature_ADC) * ADC_Ref / 1023;
                //                System.Threading.Thread.Sleep(100);
                //  Read the Room temperature and convert is into voltage
                double VADC_Room = GetTemperature(Room_Temperature_ADC) * ADC_Ref / 1023;
                //  Convert the ADV voltage values into temperature
                //room_temperature = (VADC_Room - 0.5) / 0.01;
                //peltier_temperature = (VADC_Peltier - 0.5) / 0.01;
                //result = string.Format("Peltier T={0}\nRoom T={1}\n", peltier_temperature, room_temperature);
                //return result;
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the Peltier Cell and Room temperatures.
        /// This method reads the ADC channels connected to the temperature sensors and convert the result as temperatures.
        /// </summary>
        public string GetTemperatures(uint samples)
        //public void GetTemperatures(uint samples)
        {
            string result;
            try
            {
                double foo1 = 0.0;
                double foo2 = 0.0;
                for (uint i = 0; i < samples; i++)
                {
                    //  Read the Peltier Cell temperature and convert it into voltage
                    double VADC_Peltier = GetTemperature(Peltier_Temperature_ADC) * ADC_Ref / 1023;
                    //                System.Threading.Thread.Sleep(100);
                    //  Read the Room temperature and convert is into voltage
                    double VADC_Room = GetTemperature(Room_Temperature_ADC) * ADC_Ref / 1023;
                    //  Convert the ADV voltage values into temperature
                    foo1 += (VADC_Room - 0.5) / 0.01;
                    foo2 += (VADC_Peltier - 0.5) / 0.01;
                }
                //room_temperature = foo1 / samples;
                //peltier_temperature = foo2 / samples;
                //result = string.Format("Peltier T={0}\nRoom T={1}", peltier_temperature, room_temperature);
                //return result;
                return string.Empty;
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
                string cmd = "info\r\n";
                //  Send the command to the AVR instance
                _avr.Send(cmd);
                Thread.Sleep(1000);
                //  Reads back the answer from the AVR and check if it is consistent
                string res = _avr.getCOMData();
                //  If it contains "OK" returns the string from AVR
                //  Else throw an exception
                if (res.Contains("OK")) return res;
                else if (res.Contains("Error:")) throw new Exception(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return string.Empty;
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
                string cmd = "get_ACK\r\n";
                //  Send the command and read the answer
                _avr.Send(cmd);
                Thread.Sleep(100);
                //  Reads back the answer from the AVR and checks if it is consisten
                string res = _avr.getCOMData();
                //  Return TRUE if the ACK is received, else return FALSE
                if (res.Contains("ACK")) return true;
                else if (res.Contains("Error:")) return false;
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
            return false;
        }

        #endregion
    }
}

