using Ict;
using Symbol.RFID3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using static Symbol.RFID3.Events;

namespace Sci.Production.Prg
{
    /// <summary>
    /// RfidFX7510Reader
    /// </summary>
    public class RfidFX7500Reader
    {
        public RFIDReader RFIDReader;
        public bool IsConnect = false;
        public bool IsScanOn = false;
        private Action<DataTable> afterRFIDScanDo;
        public Hashtable tagTable = new Hashtable();
        private Form mainForm;
        private DataTable dtTagTemp = new DataTable();

        private delegate void UpdateRead(Events.ReadEventData eventData);

        private UpdateRead UpdateReadHandler = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RFIDFX7510Reader"/> class.
        /// </summary>
        /// <param name="afterRFIDScanDo">掃描後的處理</param>
        /// <param name="mainForm">使用當下的Form</param>
        public RfidFX7500Reader(Action<DataTable> afterRFIDScanDo, Form mainForm)
        {
            this.dtTagTemp.Columns.Add("tagID", typeof(string));
            this.afterRFIDScanDo = afterRFIDScanDo;
            this.mainForm = mainForm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RfidFX7500Reader"/> class.
        /// </summary>
        public RfidFX7500Reader()
        {
        }

        /// <summary>
        /// RFID連線
        /// </summary>
        /// <returns>錯誤訊息</returns>
        public DualResult RFIDConnect()
        {
            DualResult result = new DualResult(true);

            // 設定RFID ip & port
            this.RFIDReader = new RFIDReader("169.254.10.1", uint.Parse("5084"), 0);
            try
            {
                // 建立連線
                this.RFIDReader.Connect();

                // 設定掃描距離，Antennas[4]為port4
                Antennas.Config antConfig = this.RFIDReader.Config.Antennas[4].GetConfig();
                antConfig.TransmitPowerIndex = (ushort)10;
                this.RFIDReader.Config.Antennas[4].SetConfig(antConfig);
                this.IsConnect = true;
            }
            catch (Exception ex)
            {
                this.RFIDdisConnect();
                return new DualResult(false, ex);
            }

            return new DualResult(true);
        }

        /// <summary>
        /// RFID斷開連線
        /// </summary>
        /// <returns>錯誤訊息</returns>
        public DualResult RFIDdisConnect()
        {
            try
            {
                if (this.RFIDReader == null)
                {
                    return new DualResult(true);
                }

                // 停止掃描
                this.RFIDReader.Actions.Inventory.Stop();

                // 斷開連線
                this.RFIDReader.Disconnect();
                this.RFIDReader = null;
                this.IsConnect = false;
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }

            return new DualResult(true);
        }

        /// <summary>
        /// 開啟掃描
        /// </summary>
        /// <returns>錯誤訊息</returns>
        public DualResult RFIDscanOn()
        {
            try
            {
                if (this.RFIDReader == null)
                {
                    return new DualResult(true);
                }

                // 設定背景掃描參數
                this.UpdateReadHandler = new UpdateRead(this.UpdateReadTags);
                this.RFIDReader.Events.ReadNotify += new Events.ReadNotifyHandler(this.Events_ReadNotify);
                this.RFIDReader.Events.StatusNotify += new Events.StatusNotifyHandler(this.OnStatusNotify);
                this.RFIDReader.Events.AttachTagDataWithReadEvent = false;
                this.RFIDReader.Events.NotifyGPIEvent = true;
                this.RFIDReader.Events.NotifyBufferFullEvent = true;
                this.RFIDReader.Events.NotifyBufferFullWarningEvent = true;
                this.RFIDReader.Events.NotifyReaderDisconnectEvent = true;
                this.RFIDReader.Events.NotifyReaderExceptionEvent = true;
                this.RFIDReader.Events.NotifyAccessStartEvent = true;
                this.RFIDReader.Events.NotifyAccessStopEvent = true;
                this.RFIDReader.Events.NotifyInventoryStartEvent = true;
                this.RFIDReader.Events.NotifyInventoryStopEvent = true;

                // 開始掃描
                this.RFIDReader.Actions.Inventory.Perform(null, null, null);
                this.IsScanOn = true;
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }

            return new DualResult(true);
        }

        private void OnStatusNotify(object sender, Events.StatusEventArgs e)
        {
            var eventType = e.StatusEventData.StatusEventType;

            if (eventType == STATUS_EVENT_TYPE.DISCONNECTION_EVENT)
            {
                // 讀取器斷線事件（USB拔除或網路斷線等）
                // 暫不作任何動作 
            }
            else
            {
               // 暫不作任何動作
            }
        }

        /// <summary>
        /// 停止掃描
        /// </summary>
        /// <returns>錯誤訊息</returns>
        public DualResult RFIDscanOff()
        {
            try
            {
                if (this.RFIDReader == null)
                {
                    return new DualResult(true);
                }

                // 停止掃描-不清暫存
                this.RFIDReader.Actions.Inventory.Stop();
                this.IsScanOn = false;
            }
            catch (Exception ex)
            {
                return new DualResult(false, ex);
            }

            return new DualResult(true);
        }

        private void UpdateReadTags(Events.ReadEventData eventData)
        {
            int index = 0;
            ListViewItem item;
            Symbol.RFID3.TagData[] tagData = this.RFIDReader.Actions.GetReadTags(1000);
            if (tagData != null)
            {
                for (int nIndex = 0; nIndex < tagData.Length; nIndex++)
                {
                    if (tagData[nIndex].OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_NONE ||
                        (tagData[nIndex].OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_READ &&
                        tagData[nIndex].OpStatus == ACCESS_OPERATION_STATUS.ACCESS_SUCCESS))
                    {
                        Symbol.RFID3.TagData tag = tagData[nIndex];
                        string tagID = tag.TagID;
                        bool isFound = false;

                        lock (this.tagTable.SyncRoot)
                        {
                            isFound = this.tagTable.ContainsKey(tagID);
                            if (!isFound)
                            {
                                tagID += (uint)tag.MemoryBank + tag.MemoryBankDataOffset;
                                isFound = this.tagTable.ContainsKey(tagID);
                            }
                        }

                        if (isFound)
                        {
                            uint count = 0;
                            item = (ListViewItem)this.tagTable[tagID];
                            try
                            {
                                count = uint.Parse(item.SubItems[2].Text) + tagData[nIndex].TagSeenCount;
                            }
                            catch (FormatException)
                            {
                                break;
                            }

                            item.SubItems[1].Text = tag.AntennaID.ToString();
                            item.SubItems[2].Text = count.ToString();
                            item.SubItems[3].Text = tag.PeakRSSI.ToString();

                            string memoryBank = tag.MemoryBank.ToString();
                            index = memoryBank.LastIndexOf('_');
                            if (index != -1)
                            {
                                memoryBank = memoryBank.Substring(index + 1);
                            }

                            if (tag.MemoryBankData.Length > 0 && !memoryBank.Equals(item.SubItems[5].Text))
                            {
                                item.SubItems[5].Text = tag.MemoryBankData;
                                item.SubItems[6].Text = memoryBank;
                                item.SubItems[7].Text = tag.MemoryBankDataOffset.ToString();

                                lock (this.tagTable.SyncRoot)
                                {
                                    this.tagTable.Remove(tagID);
                                    this.tagTable.Add(
                                        tag.TagID + tag.MemoryBank.ToString()
                                        + tag.MemoryBankDataOffset.ToString(), item);
                                }
                            }
                        }
                        else
                        {
                            item = new ListViewItem(tag.TagID);
                            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tag.AntennaID.ToString()));
                            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tag.TagSeenCount.ToString()));
                            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tag.PeakRSSI.ToString()));
                            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tag.PC.ToString("X")));
                            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, string.Empty));
                            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, string.Empty));
                            item.SubItems.Add(new ListViewItem.ListViewSubItem(item, string.Empty));

                            lock (this.tagTable.SyncRoot)
                            {
                                this.tagTable.Add(tagID, item);
                                DataRow drTagTemp = this.dtTagTemp.NewRow();
                                drTagTemp["tagID"] = tag.TagID;
                                this.dtTagTemp.Rows.Add(drTagTemp);
                            }
                        }
                    }
                }

                this.afterRFIDScanDo(this.dtTagTemp);
                this.dtTagTemp.Clear();
            }
        }

        /// <summary>
        /// RFIDgettag
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetRFIDTag()
        {
            this.dtTagTemp.Columns.Add("tagID", typeof(string));
            try
            {
                this.tagTable.Clear();
                int index = 0;
                ListViewItem item;

                Symbol.RFID3.TagData[] tagData = this.RFIDReader.Actions.GetReadTags(1000);
                if (tagData != null)
                {
                    for (int nIndex = 0; nIndex < tagData.Length; nIndex++)
                    {
                        if (tagData[nIndex].OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_NONE ||
                            (tagData[nIndex].OpCode == ACCESS_OPERATION_CODE.ACCESS_OPERATION_READ &&
                            tagData[nIndex].OpStatus == ACCESS_OPERATION_STATUS.ACCESS_SUCCESS))
                        {
                            Symbol.RFID3.TagData tag = tagData[nIndex];
                            string tagID = tag.TagID;
                            bool isFound = false;

                            lock (this.tagTable.SyncRoot)
                            {
                                isFound = this.tagTable.ContainsKey(tagID);
                                if (!isFound)
                                {
                                    tagID += (uint)tag.MemoryBank + tag.MemoryBankDataOffset;
                                    isFound = this.tagTable.ContainsKey(tagID);
                                }
                            }

                            if (isFound)
                            {
                                uint count = 0;
                                item = (ListViewItem)this.tagTable[tagID];
                                try
                                {
                                    count = uint.Parse(item.SubItems[2].Text) + tagData[nIndex].TagSeenCount;
                                }
                                catch (FormatException)
                                {
                                    break;
                                }

                                item.SubItems[1].Text = tag.AntennaID.ToString();
                                item.SubItems[2].Text = count.ToString();
                                item.SubItems[3].Text = tag.PeakRSSI.ToString();

                                string memoryBank = tag.MemoryBank.ToString();
                                index = memoryBank.LastIndexOf('_');
                                if (index != -1)
                                {
                                    memoryBank = memoryBank.Substring(index + 1);
                                }

                                if (tag.MemoryBankData.Length > 0 && !memoryBank.Equals(item.SubItems[5].Text))
                                {
                                    item.SubItems[5].Text = tag.MemoryBankData;
                                    item.SubItems[6].Text = memoryBank;
                                    item.SubItems[7].Text = tag.MemoryBankDataOffset.ToString();

                                    lock (this.tagTable.SyncRoot)
                                    {
                                        this.tagTable.Remove(tagID);
                                        this.tagTable.Add(
                                            tag.TagID + tag.MemoryBank.ToString()
                                            + tag.MemoryBankDataOffset.ToString(), item);
                                    }
                                }
                            }
                            else
                            {
                                item = new ListViewItem(tag.TagID);
                                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tag.AntennaID.ToString()));
                                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tag.TagSeenCount.ToString()));
                                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tag.PeakRSSI.ToString()));
                                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, tag.PC.ToString("X")));
                                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, string.Empty));
                                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, string.Empty));
                                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, string.Empty));

                                lock (this.tagTable.SyncRoot)
                                {
                                    this.tagTable.Add(tagID, item);
                                    DataRow drTagTemp = this.dtTagTemp.NewRow();
                                    drTagTemp["tagID"] = tag.TagID;
                                    this.dtTagTemp.Rows.Add(drTagTemp);
                                }
                            }
                        }
                    }

                    this.afterRFIDScanDo(this.dtTagTemp);
                }
            }
            catch (Exception)
            {
            }

            return this.dtTagTemp;
        }

        private void Events_ReadNotify(object sender, Events.ReadEventArgs readEventArgs)
        {
            try
            {
                this.UpdateReadHandler = new UpdateRead(this.UpdateReadTags);
                this.mainForm.Invoke(this.UpdateReadHandler, new object[] { readEventArgs.ReadEventData });
            }
            catch (Exception)
            {
            }
        }
    }
}
