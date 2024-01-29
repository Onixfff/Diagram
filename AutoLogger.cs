using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using MySqlX.XDevAPI.Relational;
using ru.nvg79;
using S7.Net;
using S7.Net.Types;
using ZedGraph;

namespace CrossLogger
{
    public class AutoLogger
    {
        System.Timers.Timer timer;
        object lockO;
        public AutoLogger()
        {
            try
            {
                Log();
            }
            catch (Exception ex)
            {
                //NVGLogg.WriteErrore("Ошибка при записи по таймеру " + ex.Message);
            }
            timer = new System.Timers.Timer(500);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Log();
            }
            catch (Exception ex)
            {
                //NVGLogg.WriteErrore("Ошибка при записи по таймеру " + ex.Message);
            }
        }
        private void Log()
        {
            Plc plc = new Plc(CpuType.S71200, "192.168.100.100", 0, 1);

            ErrorCode code = plc.Open();
            if (code == ErrorCode.NoError)
            {
                //byte status = (byte)plc.Read(DataType.DataBlock, 180, 1, VarType.Byte, 1);
                //bool trigger1, trigger2, trigger3, trigger4;
                //trigger1 = ReturnBit.GetBit(status, 0);
                //trigger2 = ReturnBit.GetBit(status, 1);
                //trigger3 = ReturnBit.GetBit(status, 2);
                //trigger4 = ReturnBit.GetBit(status, 3);
                double cond1 = 0, cond2 = 0, cond3 = 0, cond4 = 0;
                byte[] autoclave1 = null, autoclave2 = null, autoclave3 = null, autoclave4 = null;
                if (true)
                {
                    short log = (short)plc.Read(DataType.DataBlock, 270, 0, VarType.Int, 1);
                    if (log == 815)
                    {
                        autoclave1 = plc.ReadBytes(DataType.DataBlock, 71, 0, 90);
                        double plpd = (double)plc.Read(DataType.DataBlock, 265, 4, VarType.Real, 1);
                        double phpd = (double)plc.Read(DataType.DataBlock, 265, 12, VarType.Real, 1);
                        double rlpd = (double)plc.Read(DataType.DataBlock, 265, 20, VarType.Real, 1);
                        double rhpd = (double)plc.Read(DataType.DataBlock, 265, 28, VarType.Real, 1);
                        double hlpd = (double)plc.Read(DataType.DataBlock, 265, 36, VarType.Real, 1);
                        double hhpd = (double)plc.Read(DataType.DataBlock, 265, 44, VarType.Real, 1);
                        double slpd = (double)plc.Read(DataType.DataBlock, 265, 52, VarType.Real, 1);
                        double shpd = (double)plc.Read(DataType.DataBlock, 265, 60, VarType.Real, 1);
                        cond1 = plpd * 100 + phpd * 100 + rlpd * 100 + rhpd * 100 + hlpd * 100 + hhpd * 100 + slpd * 100 + shpd * 100;
                        //plc.Write("db270.dbw2", (short)815);
                    }
                }
                if (true)
                {
                    short log = (short)plc.Read(DataType.DataBlock, 270, 12, VarType.Int, 1); // (short)plc.Read("db270.dbw12");
                    if (log == 815)
                    {
                        autoclave2 = plc.ReadBytes(DataType.DataBlock, 72, 0, 90);
                        double plpd = (double)plc.Read(DataType.DataBlock, 265, 68, VarType.Real, 1);
                        double phpd = (double)plc.Read(DataType.DataBlock, 265, 76, VarType.Real, 1);
                        double rlpd = (double)plc.Read(DataType.DataBlock, 265, 84, VarType.Real, 1);
                        double rhpd = (double)plc.Read(DataType.DataBlock, 265, 92, VarType.Real, 1);
                        double hlpd = (double)plc.Read(DataType.DataBlock, 265, 100, VarType.Real, 1);
                        double hhpd = (double)plc.Read(DataType.DataBlock, 265, 108, VarType.Real, 1);
                        double slpd = (double)plc.Read(DataType.DataBlock, 265, 116, VarType.Real, 1);
                        double shpd = (double)plc.Read(DataType.DataBlock, 265, 124, VarType.Real, 1);
                        cond2 = plpd * 100 + phpd * 100 + rlpd * 100 + rhpd * 100 + hlpd * 100 + hhpd * 100 + slpd * 100 + shpd * 100;
                        // plc.Write("db270.dbw14", (short)815);
                    }
                }
                if (true)
                {
                    short log = (short)plc.Read(DataType.DataBlock, 270, 24, VarType.Int, 1);// (short)plc.Read("db270.dbw24");
                    if (log == 815)
                    {
                        autoclave3 = plc.ReadBytes(DataType.DataBlock, 73, 0, 90);
                        double plpd = (double)plc.Read(DataType.DataBlock, 265, 132, VarType.Real, 1);
                        double phpd = (double)plc.Read(DataType.DataBlock, 265, 140, VarType.Real, 1);
                        double rlpd = (double)plc.Read(DataType.DataBlock, 265, 148, VarType.Real, 1);
                        double rhpd = (double)plc.Read(DataType.DataBlock, 265, 156, VarType.Real, 1);
                        double hlpd = (double)plc.Read(DataType.DataBlock, 265, 164, VarType.Real, 1);
                        double hhpd = (double)plc.Read(DataType.DataBlock, 265, 172, VarType.Real, 1);
                        double slpd = (double)plc.Read(DataType.DataBlock, 265, 180, VarType.Real, 1);
                        double shpd = (double)plc.Read(DataType.DataBlock, 265, 188, VarType.Real, 1);
                        cond3 = plpd * 100 + phpd * 100 + rlpd * 100 + rhpd * 100 + hlpd * 100 + hhpd * 100 + slpd * 100 + shpd * 100;
                        //plc.Write("db270.dbw26", (short)815);
                    }
                }
                if (true)
                {
                    short log = (short)plc.Read(DataType.DataBlock, 270, 36, VarType.Int, 1);// (short)plc.Read("db270.dbw36");
                    if (log == 815)
                    {
                        autoclave4 = plc.ReadBytes(DataType.DataBlock, 74, 0, 90);
                        double plpd = (double)plc.Read(DataType.DataBlock, 265, 196, VarType.Real, 1);
                        double phpd = (double)plc.Read(DataType.DataBlock, 265, 204, VarType.Real, 1);
                        double rlpd = (double)plc.Read(DataType.DataBlock, 265, 212, VarType.Real, 1);
                        double rhpd = (double)plc.Read(DataType.DataBlock, 265, 220, VarType.Real, 1);
                        double hlpd = (double)plc.Read(DataType.DataBlock, 265, 228, VarType.Real, 1);
                        double hhpd = (double)plc.Read(DataType.DataBlock, 265, 236, VarType.Real, 1);
                        double slpd = (double)plc.Read(DataType.DataBlock, 265, 244, VarType.Real, 1);
                        double shpd = (double)plc.Read(DataType.DataBlock, 265, 252, VarType.Real, 1);
                        cond4 = plpd * 100 + phpd * 100 + rlpd * 100 + rhpd * 100 + hlpd * 100 + hhpd * 100 + slpd * 100 + shpd * 100;
                        // plc.Write("db270.dbw38", (short)815);
                    }
                }
                plc.Close();
                //if (autoclave1 != null) SaveAutoclaveData(autoclave1, (float)cond1, 1);
                //if (autoclave2 != null) SaveAutoclaveData(autoclave2, (float)cond2, 2);
                //if (autoclave3 != null) SaveAutoclaveData(autoclave3, (float)cond3, 3);
                //if (autoclave4 != null) SaveAutoclaveData(autoclave4, (float)cond4, 4);
            }
            else
            {
                if (plc != null && plc.IsConnected) plc.Close();
            }
        }
//        private bool SaveAutoclaveData(byte[] data, float cond, byte index)
//        {
//            short ActMinute, Stage, Open_Valve_Global_Vacuum, Open_Valve_Livesteam, Open_Valve_Vacuum,
//                Open_Valve_Release, Open_Valve_ReleaseFlap, Open_Valve_AirBleed, Open_Valve_Flushing,
//                Open_Valve_SteamExhaust;
//            float PressureLivesteam, PressureInside, PressureTimeProduct, SetpointPressure, SetpPressureCurve,
//                TemperatureTop, TemperatureBottom, TemperatureInside, TemperatureSatSteam, ConsumptionLiveSteam;
//            int Curve_ID_Setp, Curve_ID_Act;
//            byte Automatic, Fault;
//            ActMinute = (short)Int.FromByteArray(data.Skip(8).Take(2).ToArray<byte>());//Актуальная минута
//            Stage = (short)Int.FromByteArray(data.Skip(10).Take(2).ToArray<byte>());//Актуальный этап
//            PressureLivesteam = (float)S7.Net.Types.Double.FromByteArray(data.Skip(12).Take(4).ToArray<byte>());//Актуальное давление магистрали острого пара
//            PressureInside = (float)S7.Net.Types.Double.FromByteArray(data.Skip(16).Take(4).ToArray<byte>());//Актуальное давление внутри автоклава
//            PressureTimeProduct = (float)S7.Net.Types.Double.FromByteArray(data.Skip(20).Take(4).ToArray<byte>());//Актуальное значение бар*чс
//            SetpointPressure = (float)S7.Net.Types.Double.FromByteArray(data.Skip(24).Take(4).ToArray<byte>());//Актуальное скорректированное заданное давление
//            SetpPressureCurve = (float)S7.Net.Types.Double.FromByteArray(data.Skip(28).Take(4).ToArray<byte>());//Актуальное давление по программе
//            TemperatureTop = (float)S7.Net.Types.Double.FromByteArray(data.Skip(36).Take(4).ToArray<byte>());//Актуальная температура верха автоклава
//            TemperatureBottom = (float)S7.Net.Types.Double.FromByteArray(data.Skip(40).Take(4).ToArray<byte>());//Актуальная температура низа автоклава
//            TemperatureInside = (float)S7.Net.Types.Double.FromByteArray(data.Skip(44).Take(4).ToArray<byte>());//Актуальная температура внутри автоклава
//            TemperatureSatSteam = (float)S7.Net.Types.Double.FromByteArray(data.Skip(48).Take(4).ToArray<byte>());//Расчетная температура насыщенного пара
//            Open_Valve_Global_Vacuum = (short)Int.FromByteArray(data.Skip(60).Take(2).ToArray<byte>());
//            Open_Valve_Livesteam = (short)Int.FromByteArray(data.Skip(62).Take(2).ToArray<byte>());
//            Open_Valve_Vacuum = (short)Int.FromByteArray(data.Skip(64).Take(2).ToArray<byte>());
//            Open_Valve_Release = (short)Int.FromByteArray(data.Skip(66).Take(2).ToArray<byte>());
//            Open_Valve_ReleaseFlap = (short)Int.FromByteArray(data.Skip(68).Take(2).ToArray<byte>());
//            Open_Valve_AirBleed = (short)Int.FromByteArray(data.Skip(70).Take(2).ToArray<byte>());
//            Open_Valve_Flushing = (short)Int.FromByteArray(data.Skip(72).Take(2).ToArray<byte>());
//            Open_Valve_SteamExhaust = (short)Int.FromByteArray(data.Skip(74).Take(2).ToArray<byte>());
//            ConsumptionLiveSteam = (float)S7.Net.Types.Double.FromByteArray(data.Skip(76).Take(4).ToArray<byte>());
//            Curve_ID_Setp = (int)DInt.FromByteArray(data.Skip(80).Take(4).ToArray<byte>());//ID программы
//            Curve_ID_Act = (int)DInt.FromByteArray(data.Skip(84).Take(4).ToArray<byte>());//Количество запусков
//            Automatic = data[88];
//            Fault = data[89];
//#if LOCAL
//            string Connection = MySQLBase.ConnectionString("localhost", "auto_root", "12345", "autoclave"); 
//#else
//            string Connection = MySQLBase.ConnectionString("localhost", "root", "12345", "autoclave");
//#endif
//            MySQLWrite writer = new MySQLWrite(Connection);
//            writer.Add("actminute", ActMinute.ToString());
//            writer.Add("stage", Stage.ToString());
//            writer.Add("pressurelivesteam", PressureLivesteam.ToString());
//            writer.Add("pressureinside", PressureInside.ToString());
//            writer.Add("pressuretimeproduct", PressureTimeProduct.ToString());
//            writer.Add("setpointpressure", SetpointPressure.ToString());
//            writer.Add("setppressurecurve", SetpPressureCurve.ToString());
//            writer.Add("temperaturetop", TemperatureTop.ToString());
//            writer.Add("temperaturebottom", TemperatureBottom.ToString());
//            writer.Add("temperatureinside", TemperatureInside.ToString());
//            writer.Add("temperaturesatsteam", TemperatureSatSteam.ToString());
//            writer.Add("open_valve_global_vacuum", Open_Valve_Global_Vacuum.ToString());
//            writer.Add("open_valve_livesteam", Open_Valve_Livesteam.ToString());
//            writer.Add("open_valve_vacuum", Open_Valve_Vacuum.ToString());
//            writer.Add("open_valve_release", Open_Valve_Release.ToString());
//            writer.Add("open_valve_releaseflap", Open_Valve_ReleaseFlap.ToString());
//            writer.Add("open_valve_airbleed", Open_Valve_AirBleed.ToString());
//            writer.Add("open_valve_flushing", Open_Valve_Flushing.ToString());
//            writer.Add("open_valve_steamexhaust", Open_Valve_SteamExhaust.ToString());
//            //writer.Add("consumptionlivesteam",ConsumptionLiveSteam.ToString());
//            writer.Add("curve_id_setp", Curve_ID_Setp.ToString());
//            writer.Add("curve_id_act", Curve_ID_Act.ToString());
//            writer.Add("condensate", cond.ToString());
//            writer.Add("automatic", Automatic.ToString());
//            writer.Add("fault", Fault.ToString());
//            MySQLResult result = writer.Insert("autoclave" + index);
//            if (result.HasErrore)
//            {
//                NVGLogg.WriteErrore("Ошибка при сохранении записи данных автоклава № " + index + " " + result.ErroreText);
//                return false;
//            }
//            return true;
//        }
    }
}
